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
        /// Groups a selected set of rows into a set of summary rows by the values of one or more columns or expressions.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="columns">Are column expressions on which grouping is performed.</param>
        public static GroupByChainer GroupBy(this IGroupBy prev, params GroupingArgument[] columns)
        {
            return new GroupByChainer((Chainer)prev, columns);
        }
    }
}
