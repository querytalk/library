#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class RankingOrderByChainer : Chainer, IQuery,
        IEndRanking
    {
        internal override string Method
        {
            get
            {
                return Text.Method.SysRankingOrderBy;
            }
        }

        internal RankingOrderByChainer(Chainer prev, OrderingArgument[] columns) 
            : base(prev)
        {
            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(30)
                    .Append(Text.OrderBy).S()
                    .Append(OrderingArgument.Concatenate(columns, buildContext, buildArgs, false))
                    .ToString();

                TryThrow(buildContext);

                return sql;
            };
        }
    }
}
