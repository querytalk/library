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
        /// Combines the results of two or more queries into a single result set that includes all the rows that belong to all queries in the union. Duplicate rows are removed.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        public static UnionChainer Union(this IUnion prev)
        {
            return new UnionChainer((Chainer)prev, UnionChainer.UnionType.Union);
        }

        /// <summary>
        /// Combines the results of two or more queries into a single result set that includes all the rows that belong to all queries in the union. Incorporates all rows into the results. This includes duplicates.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        public static UnionChainer UnionAll(this IUnion prev)
        {
            return new UnionChainer((Chainer)prev, UnionChainer.UnionType.UnionAll);
        }

        /// <summary>
        /// Returns any distinct values from the left query that are not also found on the right query.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        public static UnionChainer Except(this IUnion prev)
        {
            return new UnionChainer((Chainer)prev, UnionChainer.UnionType.Except);
        }

        /// <summary>
        /// Returns any distinct values that are returned by both the query on the left and right sides of the INTERSECT operand.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        public static UnionChainer Intersect(this IUnion prev)
        {
            return new UnionChainer((Chainer)prev, UnionChainer.UnionType.Intersect);
        }
    }
}
