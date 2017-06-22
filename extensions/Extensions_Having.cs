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
        /// Specifies a search condition for a group or an aggregate.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="condition">Specifies the search condition for the group or the aggregate to meet.</param>
        public static HavingChainer Having(this IHaving prev, Expression condition)
        {
            return new HavingChainer((Chainer)prev, condition);
        }

        /// <summary>
        /// Specifies a search condition for a group or an aggregate.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first argument.</param>
        /// <param name="argument2">Is the second argument.</param>
        /// <param name="equality">Specifies the type of the relation between the two arguments. true indicates the equality operator, false indicates the inequality operator.</param>
        public static HavingChainer Having(this IHaving prev, ScalarArgument argument1, ScalarArgument argument2, bool equality = true)
        {
            return new HavingChainer((Chainer)prev, argument1, argument2, equality);
        }
    }
}
