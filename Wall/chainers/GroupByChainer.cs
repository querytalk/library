#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class GroupByChainer : Chainer, IQuery, IViewAllowed, IOpenView,  
        ISelect,
        IOrderBy,
        IDelete,
        IWithCube,
        IHaving
    {
        internal override string Method
        {
            get
            {
                return Text.Method.GroupBy;
            }
        }

        // true : WithCube
        // false: WithRollup
        // null : no cube/rollup
        internal Nullable<bool> IsWithCube;

        internal GroupByChainer(Chainer prev, GroupingArgument[] columns) 
            : base(prev)
        {
            Query.Clause.GroupBy = this;

            CheckNullOrEmptyAndThrow(Argc(() => columns, columns));

            Query.AddArguments(columns);

            Build = (buildContext, buildArgs) =>
                {
                    var sql = Text.GenerateSql(100)
                        .NewLine(Text.GroupBy).S()
                        .Append(GroupingArgument.Concatenate(columns, buildContext, buildArgs, false));

                    if (IsWithCube == true)
                    {
                        sql.NewLine(Text.WithCube);
                    }
                    else if (IsWithCube == false)
                    {
                        sql.NewLine(Text.WithRollup);
                    }

                    TryThrow(buildContext);

                    return sql.ToString();
                };
        }

        internal GroupByChainer(ISemantic prev, GroupingArgument[] columns)
            : base(prev.Translate(new SemqContext(((DbNode)prev).Root), null))
        {
            Query.Clause.GroupBy = this;

            CheckNullOrEmptyAndThrow(Argc(() => columns, columns));

            Query.AddArguments(columns);

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(100)
                    .NewLine(Text.GroupBy).S()
                    .Append(GroupingArgument.Concatenate(columns, buildContext, buildArgs, false))
                    .ToString();

                TryThrow(buildContext);

                return sql;
            };
        }
    }
}
