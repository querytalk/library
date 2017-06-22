#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class BeginTableChainer : Chainer, IBegin, 
        IBeginTableColumn,
        IBeginTablePrimaryKey,
        IBeginTableUniqueKey,
        IEndTable
    {
        internal override string Method
        {
            get
            {
                return Text.Method.DesignTable;
            }
        }

        private string _tableName;
        internal string TableName
        {
            get
            {
                return _tableName;
            }
        }

        private TableType _tableType;

        internal View DataView { get; private set; }

        internal bool IsDesignedByType
        {
            get
            {
                return DataView != null;
            }
        }

        private bool _hasColumns;
        internal void SetHasColumns()
        {
            _hasColumns = true;
        }

        internal bool HasColumns
        {
            get
            {
                return _hasColumns 
                    || (DataView != null && DataView.IsValidDataView);
            }
        }

        // if true the data view will be inserted into the table
        private bool _fill;

        internal bool IsFill
        {
            get
            {
                return _fill
                    && (DataView != null && DataView.RowCount > 0);
            }
        }

        // design table using CLR type (converted into empty data view)
        internal static BeginTableChainer DesingByType<T>(Chainer prev, string tableName, bool skipTimestampColumn)
        {
            View view;

            // always convert QueryTalk compliant scalar type T into Row<T>
            Type clrType;
            QueryTalkException exception;
            if (Mapping.CheckClrCompliance(typeof(T), out clrType, out exception) == Mapping.ClrTypeMatch.ClrMatch)
            {
                view = Designer.ToView<Row<T>>();
            }
            else
            {
                view = Designer.ToView<T>();
            }

            return new BeginTableChainer(prev, tableName, view, true, skipTimestampColumn);  
        }

        internal BeginTableChainer(Chainer prev, string tableName, View view, bool fill, bool skipTimestampColumn)     
            : base(prev)
        {
            _tableName = tableName;
            _fill = fill;

            if (view != null)
            {
                if (!view.IsValidDataView)
                {
                    Throw(QueryTalkExceptionType.InvalidDataView,
                        String.Format("table = {0}", tableName));
                }
            }
            DataView = view;

            EvalAndThrow();

            Build = (buildContext, buildArgs) =>
            {
                var sql = BuildHeader();

                if (view != null)
                {
                    BuildFromView(sql, view, skipTimestampColumn);
                }

                return sql.ToString();
            };
        }

        // evaluate the table type
        private void EvalAndThrow()
        {
            _tableType = Common.TryDetectTableType(GetRoot(), _tableName, Method);

            var variable = new Variable(0, _tableName, DT.TableVariable, IdentifierType.SqlVariable);
            GetRoot().TryAddVariableOrThrow(variable, Method, false);
        }

        private StringBuilder BuildHeader()
        {
            var sql = Text.GenerateSql(500);

            if (_tableType == TableType.Variable)
            {
                sql.NewLine(Text.Declare).S()
                    .Append(_tableName)
                    .Append(Text._As_)
                    .Append(Text.Table).S()
                    .NewLine(Text.LeftBracket);
            }
            // temp table
            else
            {
                sql.NewLine(Text.CreateTable).S()
                    .Append(_tableName).S()
                    .NewLine(Text.LeftBracket);
            }

            return sql;
        }

        // build table declaration from the data view
        internal static void BuildFromView(StringBuilder sql, View view, bool skipTimestampColumn)
        {
            if (view == null)
            {
                throw new QueryTalkException("BeginTable.BuildFromView",
                    QueryTalkExceptionType.ArgumentNull, "view = null");
            }

            int i = 0;
            foreach (var column in view.Columns)
            {
                // check timestamp column
                if (skipTimestampColumn && column.DataType.DT.IsRowversion())
                {
                    continue;
                }

                sql.NewLine(Text.TwoSpaces);

                if (i++ > 0)
                {
                    sql.Append(Text.Comma);
                }

                sql.Append(Filter.Delimit(column.ColumnName)).S();
                if (column.DataType != null)
                {
                    sql.Append(column.DataType.Build());
                }
                else
                {
                    sql.Append(Mapping.ClrMapping[column.ClrType].DefaultDataType.Build());
                }

                if (column.IsNullable)
                {
                    sql.S().Append(Text.Null).S();
                }
                else
                {
                    sql.S().Append(Text.NotNull).S();
                }
            }
        }

        // fill table with view data
        internal static void Fill(StringBuilder sql, string tableName, View view)
        {
            var hasRowversion = view.Columns != null
                && view.Columns.Where(c => c.DataType != null && ((DataType)c.DataType).DT.IsRowversion()).Any();

            if (!hasRowversion)
            {
                sql.NewLine(Text.InsertInto).S()
                    .Append(tableName).S()
                    .NewLine(view.Sql)
                    .Terminate();

                return;
            }

            // skip rowversion column
            var columns = String.Join(Text.Comma,
                view.Columns.Where(c => !((DataType)c.DataType).DT.IsRowversion())
                    .Select(c => Filter.Delimit(c.ColumnName))
                    .ToArray());

            sql.NewLine(Text.InsertInto).S()
                .Append(tableName).S()
                .NewLine(Text.LeftBracket)
                .Append(columns)
                .Append(Text.RightBracket).S()
                .NewLine(Text.Select).S().Append(columns).S()
                .NewLine(Text.From).S().Append(Text.LeftBracket).S()
                .NewLine(view.Sql)
                .Append(Text.RightBracket).Append(Text._As_).Append(Text.DelimitedTargetAlias2)

                .Terminate();
        }

    }
}
