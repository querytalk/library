#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        #region OnAnd

        /// <summary>
        /// Specifies the additional search condition for the rows returned by the query. 
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="condition">Defines the condition to be met for the rows to be returned.</param>
        public static OnAndChainer And(this IOnAnd prev, Expression condition)
        {
            return new OnAndChainer((Chainer)prev, condition);
        }

        /// <summary>
        /// Specifies the additional search condition for the rows returned by the query. 
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="condition">Defines the negative condition to be met for the rows to be returned.</param>
        public static OnAndChainer AndNot(this IOnAnd prev, Expression condition)
        {
            return new OnAndChainer((Chainer)prev, condition.Not());
        }

        /// <summary>
        /// Specifies the additional search condition for the rows returned by the query based on the comparison between the two scalar arguments.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first scalar argument.</param>
        /// <param name="argument2">Is the second scalar argument.</param>
        public static OnAndChainer And(this IOnAnd prev, ScalarArgument argument1, ScalarArgument argument2)
        {
            return new OnAndChainer((Chainer)prev, argument1, argument2, true);
        }

        /// <summary>
        /// Specifies the additional negative search condition for the rows returned by the query based on the comparison between the two scalar arguments.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first scalar argument.</param>
        /// <param name="argument2">Is the second scalar argument.</param>
        public static OnAndChainer AndNot(this IOnAnd prev, ScalarArgument argument1, ScalarArgument argument2)
        {
            return new OnAndChainer((Chainer)prev, argument1, argument2, false);
        }

        #endregion

        #region WhereAnd

        /// <summary>
        /// Specifies the additional search condition for the rows returned by the query. 
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="condition">Defines the condition to be met for the rows to be returned.</param>
        public static WhereAndChainer And(this IWhereAnd prev, Expression condition)
        {
            return new WhereAndChainer((Chainer)prev, condition);
        }

        /// <summary>
        /// Specifies the additional search condition for the rows returned by the query. 
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="condition">Defines the negative condition to be met for the rows to be returned.</param>
        public static WhereAndChainer AndNot(this IWhereAnd prev, Expression condition)
        {
            return new WhereAndChainer((Chainer)prev, condition.Not());
        }

        /// <summary>
        /// Specifies the additional search condition for the rows returned by the query based on the comparison between the two scalar arguments.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first argument in the equality expression.</param>
        /// <param name="argument2">Is the second argument in the equality expression.</param>
        public static WhereAndChainer And(this IWhereAnd prev, ScalarArgument argument1, ValueScalarArgument argument2)
        {
            return new WhereAndChainer((Chainer)prev, argument1, argument2, true);
        }

        /// <summary>
        /// Specifies the additional negative search condition for the rows returned by the query based on the comparison between the two scalar arguments.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first argument in the equality expression.</param>
        /// <param name="argument2">Is the second argument in the equality expression.</param>
        public static WhereAndChainer AndNot(this IWhereAnd prev, ScalarArgument argument1, ValueScalarArgument argument2)
        {
            return new WhereAndChainer((Chainer)prev, argument1, argument2, false);
        }

        #endregion

        #region WhereAndExists

        /// <summary>
        /// Specifies a view (subquery) to test for the existence of rows.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="nonSelectView">Is a subquery without the .Select method.</param>
        public static WhereAndChainer AndExists(this IWhereAnd prev, INonSelectView nonSelectView)
        {
            return new WhereAndChainer((Chainer)prev, nonSelectView, true);
        }

        /// <summary>
        /// Specifies a view (subquery) to test for the negation of the existence of rows.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="nonSelectView">Is a subquery without the .Select method.</param>
        public static WhereAndChainer AndNotExists(this IWhereAnd prev, INonSelectView nonSelectView)
        {
            return new WhereAndChainer((Chainer)prev, nonSelectView, false);
        }

        #endregion

    }
}
