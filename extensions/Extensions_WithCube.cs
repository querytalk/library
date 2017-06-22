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
        /// Specifies that in addition to the usual rows provided by GROUP BY, summary rows are introduced into the result set. Groups are summarized in a hierarchical order, from the lowest level in the group to the highest. The group hierarchy is determined by the order in which the grouping columns are specified.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        public static WithCubeChainer WithRollup(this IWithCube prev)
        {
            return new WithCubeChainer((Chainer)prev, true);
        }

        /// <summary>
        /// Specifies that in addition to the usual rows provided by GROUP BY, summary rows are introduced into the result set. A GROUP BY summary row is returned for every possible combination of group and subgroup in the result set.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        public static WithCubeChainer WithCube(this IWithCube prev)
        {
            return new WithCubeChainer((Chainer)prev, false);
        }
    }
}
