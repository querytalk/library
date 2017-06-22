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
        /// Generates the SELECT statement without the FROM clause returning the result set of the specified values.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="firstValue">Is the first value.</param>
        /// <param name="otherValues">Are the other values.</param>
        public static CollectChainer Collect(this ICollect prev, Column firstValue, params Column[] otherValues)
        {
            return new CollectChainer((Chainer)prev,
                Common.MergeArrays(firstValue, otherValues));
        }

        internal static CollectChainer Collect(this ICollect prev, Column[] values)
        {
            return new CollectChainer((Chainer)prev, values);
        }

        /// <summary>
        /// Adds another VALUES row to be inserted in the database.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="values">The specified values.</param>
        public static ThenCollectChainer ThenCollect(this IThenCollect prev, params Column[] values)
        {
            return new ThenCollectChainer((Chainer)prev, values);
        }
    }
}
