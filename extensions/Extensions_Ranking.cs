#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {
        /// <summary>
        /// Divides the result set produced by the FROM clause into partitions to which the ranking function is applied. Columns specifies the columns by which the result set is partitioned. If PARTITION BY is not specified, the function treats all rows of the query result set as a single group.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="firstColumn">The first column to partition by.</param>
        /// <param name="otherColumns">Other columns to partition by.</param>
        /// <returns></returns>
        public static RankingPartitionByChainer PartitionBy(this IRankingPartitionBy prev,
            GroupingArgument firstColumn, params GroupingArgument[] otherColumns)
        {
            return new RankingPartitionByChainer((Chainer)prev,
                Common.MergeArrays<GroupingArgument>(firstColumn, otherColumns ?? new GroupingArgument[] { null }));
        }

        /// <summary>
        /// The ORDER BY clause determines the sequence in which the rows are assigned their unique ROW_NUMBER within a specified partition. It is required. 
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="firstColumn">The first column to order by.</param>
        /// <param name="otherColumns">Other columns to order by.</param>
        /// <returns></returns>
        public static RankingOrderByChainer OrderBy(this IRankingOrderBy prev,
            OrderingArgument firstColumn, params OrderingArgument[] otherColumns)
        {
            return new RankingOrderByChainer((Chainer)prev,
                Common.MergeArrays<OrderingArgument>(firstColumn, otherColumns ?? new OrderingArgument[] { null }));
        }

        /// <summary>
        /// Ends the block of a ranking function.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static SysFn End(this IEndRanking prev)
        {
            var endRanking = new EndRankingChainer((Chainer)prev);
            return new SysFn(endRanking.Build);
        }
    }
}
