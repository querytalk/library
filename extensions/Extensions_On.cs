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
        /// Specifies the condition for matching the rows between the two tables to be joined.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="condition">Defines the matching condition.</param>
        public static OnChainer On(this IOn prev, Expression condition)
        {
            return new OnChainer((Chainer)prev, condition);
        }

        /// <summary>
        /// Specifies the negative condition for matching the rows between the two tables to be joined. 
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="condition">Defines the negative matching condition.</param>
        public static OnChainer OnNot(this IOn prev, Expression condition)
        {
            return new OnChainer((Chainer)prev, condition.Not());
        }

        /// <summary>
        /// Specifies the condition for matching the rows between the two tables based on the comparison between the two scalar arguments.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first argument.</param>
        /// <param name="argument2">Is the second argument.</param>
        public static OnChainer On(this IOn prev, ScalarArgument argument1, ScalarArgument argument2)
        {
            return new OnChainer((Chainer)prev, argument1, argument2, true);
        }

        /// <summary>
        /// Specifies the negative condition for matching the rows between the two tables based on the comparison between the two scalar arguments.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first argument.</param>
        /// <param name="argument2">Is the second argument.</param>
        public static OnChainer OnNot(this IOn prev, ScalarArgument argument1, ScalarArgument argument2)
        {
            return new OnChainer((Chainer)prev, argument1, argument2, false);
        }
    }
}
