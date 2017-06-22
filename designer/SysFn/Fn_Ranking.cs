#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {
        /// <summary>
        /// <para>ROW_NUMBER built-in function.</para>
        /// <para>Returns the sequential number of a row within a partition of a result set, starting at 1 for the first row in each partition.</para>
        /// </summary>
        public static RowNumberRankingChainer RowNumber()
        {
            return new RowNumberRankingChainer();
        }

        /// <summary>
        /// <para>RANK built-in function.</para>
        /// <para>Returns the checksum of unique values in a group. Null values are ignored.</para>
        /// </summary>
        public static RankRankingChainer Rank()
        {
            return new RankRankingChainer();
        }

        /// <summary>
        /// <para>DENSE_RANK built-in function.</para>
        /// <para>Returns the rank of rows within the partition of a result set, without any gaps in the ranking. The rank of a row is one plus the number of distinct ranks that come before the row in question.</para>
        /// </summary>
        public static DenseRankRankingChainer DenseRank()
        {
            return new DenseRankRankingChainer();
        }

        /// <summary>
        /// <para>NTILE built-in function.</para>
        /// <para>Distributes the rows in an ordered partition into a specified number of groups. The groups are numbered, starting at one. For each row, NTILE returns the number of the group to which the row belongs.</para>
        /// </summary>
        /// <param name="argument">Is a positive integer constant expression that specifies the number of groups into which each partition must be divided. The argument can be of type int, or bigint.</param>
        public static CNTileRanking NTile(int argument)
        {
            return new CNTileRanking(argument);
        }
    }
}
