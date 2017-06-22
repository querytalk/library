#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Linq;
using System.Text;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>

    public sealed class InsertChainer : WriteChainer
    {
        internal override string Method 
        { 
            get 
            {
                return Text.Method.Insert; 
            } 
        }

        // insert types
        private enum InsertType
        {
            SingleValueInsert,
            MultiValueInsert,
            QueryInsert
        }

        private InsertType _insertType
        {
            get
            {
                var selectable = Prev.GetPrev<ISelectable>();
                if (((Chainer)selectable).Query.HasFrom)
                {
                    return InsertType.QueryInsert;
                }
                else
                {
                    if (Prev is ThenCollectChainer || (Prev is ColumnsChainer && Prev.Prev is ThenCollectChainer))
                    {
                        return InsertType.MultiValueInsert;
                    }
                    else
                    {
                        return InsertType.SingleValueInsert;
                    }
                }
            }
        }

        // table: is TableArgument or DbNode
        internal InsertChainer(Chainer prev, object table)
            : base(prev)
        {
            Query.SetInsert(this);

            Table tableArgument;
            if (table is Table)
            {
                tableArgument = (Table)table;
            }
            else
            {
                tableArgument = new Table(((DbNode)table).Root.NodeID.GetNodeName());
            }

            Build = (buildContext, buildArgs) =>
            {
                switch (_insertType)
                {
                    case InsertType.SingleValueInsert:
                        return BuildSingleValueInsert(buildContext, buildArgs, tableArgument);

                    case InsertType.MultiValueInsert:
                        return BuildMultiValueInsert(buildContext, buildArgs, tableArgument);

                    default:
                        return BuildQueryInsert(buildContext, buildArgs, tableArgument);
                }
            };
        }

        private StringBuilder BuildHeader(BuildContext buildContext, BuildArgs buildArgs, Table table)
        {
            var sql = Text.GenerateSql(200);
            sql.NewLine(BuildTop(buildContext, buildArgs))
                .Append(table.Build(buildContext, buildArgs)).S();

            Column[] columns = null;
            var columnsObject = Prev.GetPrev<ColumnsChainer>();
            if (columnsObject != null)
            {
                columnsObject.Build(buildContext, buildArgs);

                // Do not build the ColumnsChainer object (it has been built already) .
                // This is not actually not needed because the Query does not control ColumnsChainer object.
                // However we set this flag here to make it clear and explicit.
                columnsObject.SkipBuild = true;

                // handle column inliner in .ColumnInto method passed as collection
                columns = ProcessValueArrayInliner(buildContext, buildArgs, Columns);
            }

            if (columns == null)
            {
                columns = Columns;  
            }

            // build inline columns
            if (columns != null)
            {
                sql.NewLine(Text.LeftBracket);

                int i = 0;
                foreach (var column in columns)
                {
                    CheckNull(Arg(() => column, column));
                    if (chainException != null)
                    {
                        chainException.Arguments = String.Format("{0} = null{1}   columns = {2}",
                            chainException.Arguments, Environment.NewLine, ToString());
                        throw chainException;
                    }

                    if (i++ > 0)
                    {
                        sql.Append(Text.Comma).S();
                    }

                    sql.Append(column.Build(buildContext, buildArgs));

                    TryThrow(buildContext);
                }

                sql.Append(Text.RightBracket)
                    .TrimEnd();
            }

            OutputChainer.TryAppendOutput(this, sql, buildContext, buildArgs);

            return sql;
        }

        // single-value insert
        private string BuildSingleValueInsert(BuildContext buildContext, BuildArgs buildArgs, Table table)
        {
            var selectable = GetPrev<ISelectable>();

            // skip building the SelectChainer object
            ((Chainer)selectable).SkipBuild = true;

            var sql = BuildHeader(buildContext, buildArgs, table);

            // treat string column as value
            UseStringAsValue = true;

            var columns = selectable.Columns;
            var isDefaultValues = false;

            // check if there is a single column with DEFAULT VALUES insert
            if (columns.Length == 1)
            {
                if (columns[0].ArgType == typeof(Value))
                {
                    var value = (Value)columns[0].Original;
                    if (value.IsSpecialValue && (string)value.Original == Text.DefaultValues)
                    {
                        isDefaultValues = true;
                    }
                }
            }

            if (isDefaultValues)
            {
                sql.NewLine(Text.DefaultValues).S()
                    .Terminate();
            }
            else
            {
                sql.NewLine(Text.Values).S()
                    .Append(Text.LeftBracket)
                    .Append(Column.Concatenate(columns, buildContext, buildArgs, null, Text.CommaWithSpace))
                    .Append(Text.RightBracket)
                    .Terminate();
            }

            return sql.ToString();
        }

        // multi-value insert
        private string BuildMultiValueInsert(BuildContext buildContext, BuildArgs buildArgs, Table table)
        {
            var sql = BuildHeader(buildContext, buildArgs, table)
                .NewLine(Text.Values).S();

            // treat string columns as values
            UseStringAsValue = true;

            var node = (Chainer)GetPrev<ISelectable>();
            while (node.Prev != null && node.Prev is ISelectable) { node = node.Prev; }
            var i = 0;

            var valueCount = ((ISelectable)node).ColumnCount;

            while (node is ISelectable)
            {
                // skip building the SelectChainer object
                node.SkipBuild = true;

                var selectable = (ISelectable)node;

                // check column-value count match 
                if (buildArgs.Executable == null)   // skip checking in case of inlining
                {
                    if (selectable.ColumnCount != valueCount)
                    {
                        Throw(QueryTalkExceptionType.InvalidColumnCount,
                            String.Format("previous value count = {0}{1}   current value count = {2}{3}   values = ({4})",
                                valueCount, Environment.NewLine,
                                selectable.ColumnCount, Environment.NewLine,
                                String.Join(", ", selectable.Columns.Select(v => 
                                    {
                                        if (v != null)
                                        {
                                            return v.Build(buildContext, buildArgs);
                                        }
                                        else
                                        {
                                            return Text.DbNull;
                                        }
                                    }
                                ))));
                    }
                }

                if (i++ > 0)
                {
                    sql.NewLineIndent(Text.Comma);
                }

                // append value
                sql.Append(Text.LeftBracket)
                    .Append(Column.Concatenate(selectable.Columns, buildContext, buildArgs, null, Text.CommaWithSpace))
                    .Append(Text.RightBracket);

                node = node.Next;
            }

            return sql
                .Terminate()
                .ToString();
        }

        private string BuildQueryInsert(BuildContext buildContext, BuildArgs buildArgs, Table table)
        {
            return BuildHeader(buildContext, buildArgs, table).ToString();
        }

        private string BuildTop(BuildContext buildContext, BuildArgs buildArgs)
        {
            return String.Format("{0} {1}{2} ",
                Text.Insert,
                base.BuildTop(buildContext),
                Text.Into);
        }

    }
}
