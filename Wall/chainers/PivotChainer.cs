#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;

namespace QueryTalk.Wall
{
    // Used for pivoting or unpivoting.
    //   note:
    //     The pivot/unpivot is the last table expression in the query. In order to use other table expression 
    //     a pivot/unpivot expression can only be used as a subquery.
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class PivotChainer : TableChainer, IQuery, IViewAllowed,
        IPivotAs,
        IWhere,
        IGroupBy,
        IOrderBy,
        ISelect
    {
        private string _method = Text.NotAvailable;

        internal override string Method
        {
            get
            {
                return _method;
            }
        }

        // pivot
        internal PivotChainer(Chainer prev, 
            SysFn aggregate, 
            NonSelectColumnArgument rotatingColumn,
            string[] rotatedColumns) 
            : base(prev, true)
        {
            _method = Text.Method.Pivot;

            CheckNullAndThrow(Arg(() => rotatingColumn, rotatingColumn));
            CheckNullOrEmptyAndThrow(Argc(() => rotatedColumns, rotatedColumns));
            Array.ForEach(rotatedColumns, column => { if (column == null) { Throw(QueryTalkExceptionType.ArgumentNull, "rotatedColumn = null"); } });

            Build = (buildContext, buildArgs) =>
                {
                    // build IN values
                    List<string> columns = new List<string>();
                    Array.ForEach(rotatedColumns, column =>
                        {
                            columns.Add(Filter.Delimit(column));
                        });

                    var sql = Text.GenerateSql(300)
                        .NewLine(Text.Pivot).S()
                        .Append(Text.LeftBracket)
                        .Append(aggregate.Build(buildContext, buildArgs)).S()
                        .Append(Text.For).S()
                        .Append(rotatingColumn.Build(buildContext, buildArgs)).S()
                        .NewLineIndent(Text.In).S()
                        .Append(Text.LeftBracket)
                        .Append(String.Join(Text.Comma, columns))
                        .Append(Text.RightBracket).Append(Text.RightBracket)
                        .Append(Text._As_).Append(Filter.Delimit(Alias.Name))
                        .ToString();

                    TryThrow(buildContext);

                    return sql;
                };
        }

        // unpivot
        internal PivotChainer(Chainer prev, 
            NonSelectColumnArgument valueColumn,
            NonSelectColumnArgument rotatingColumn,
            string[] rotatedColumns)
            : base(prev, true)
        {
            _method = Text.Method.Unpivot;

            CheckNullAndThrow(Arg(() => valueColumn, valueColumn));
            CheckNullAndThrow(Arg(() => rotatingColumn, rotatingColumn));
            CheckNullOrEmptyAndThrow(Argc(() => rotatedColumns, rotatedColumns));
            Array.ForEach(rotatedColumns, column => { if (column == null) { Throw(QueryTalkExceptionType.ArgumentNull, "rotatedColumn = null"); } });

            Build = (buildContext, buildArgs) =>
            {
                // build IN values
                List<string> columns = new List<string>();
                Array.ForEach(rotatedColumns, column =>
                {
                    columns.Add(Filter.Delimit(column));
                });

                var sql = Text.GenerateSql(300)
                    .NewLine(Text.Unpivot).S()
                    .Append(Text.LeftBracket)
                    .Append(valueColumn.Build(buildContext, buildArgs)).S()
                    .Append(Text.For).S()
                    .Append(rotatingColumn.Build(buildContext, buildArgs)).S()
                    .NewLineIndent(Text.In).S()
                    .Append(Text.LeftBracket)
                    .Append(String.Join(Text.Comma, columns))
                    .Append(Text.RightBracket).Append(Text.RightBracket)
                    .Append(Text._As_).Append(Filter.Delimit(Alias.Name))
                    .ToString();

                TryThrow(buildContext);

                return sql;
            };
        }
    }
}
