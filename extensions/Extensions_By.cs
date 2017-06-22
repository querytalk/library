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
        /// Generates the expression of the equality between the two columns. If more than one key is specified than the equality expressions are combined with AND operator.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="firstColumn">A column of the first equality expression.</param>
        /// <param name="otherColumns">Columns of other equality expressions.</param>
        public static ByChainer By(this IOn prev,
            ByArgument firstColumn, params ByArgument[] otherColumns)
        {
            return new ByChainer((Chainer)prev,
                Common.MergeArrays<ByArgument>(firstColumn, otherColumns));
        }
    }
}
