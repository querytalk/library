#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class RankingPartitionByChainer : Chainer, IQuery,
        IRankingOrderBy
    {
        internal override string Method
        {
            get
            {
                return Text.Method.SysRankingPartitionBy;
            }
        }

        internal RankingPartitionByChainer(Chainer prev, GroupingArgument[] columns) 
            : base(prev)
        {
            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(30)
                    .Append(Text.PartitionBy).S()
                    .Append(GroupingArgument.Concatenate(columns, buildContext, buildArgs, false)).S()
                    .ToString();

                TryThrow(buildContext);

                return sql;
            };
        }
    }
}
