#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        #region OnOr

        /// <summary>
        /// Specifies the additional search condition for the rows returned by the query. 
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="condition">Defines the condition to be met for the rows to be returned.</param>
        public static OnOrChainer Or(this IOnOr prev, Expression condition)
        {
            return new OnOrChainer((Chainer)prev, condition);
        }

        /// <summary>
        /// Specifies the additional search condition for the rows returned by the query. 
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="condition">Defines the negative condition to be met for the rows to be returned.</param>
        public static OnOrChainer OrNot(this IOnOr prev, Expression condition)
        {
            return new OnOrChainer((Chainer)prev, condition.Not());
        }

        /// <summary>
        /// Specifies the additional search condition for the rows returned by the query based on the comparison between the two scalar arguments.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first scalar argument.</param>
        /// <param name="argument2">Is the second scalar argument.</param>
        public static OnOrChainer Or(this IOnOr prev, ScalarArgument argument1, ScalarArgument argument2)
        {
            return new OnOrChainer((Chainer)prev, argument1, argument2, true);
        }

        /// <summary>
        /// Specifies the additional negative search condition for the rows returned by the query based on the comparison between the two scalar arguments.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first scalar argument.</param>
        /// <param name="argument2">Is the second scalar argument.</param>
        public static OnOrChainer OrNot(this IOnOr prev, ScalarArgument argument1, ScalarArgument argument2)
        {
            return new OnOrChainer((Chainer)prev, argument1, argument2, false);
        }

        #endregion

        #region WhereOr

        /// <summary>
        /// Specifies the additional search condition for the rows returned by the query. 
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="condition">Defines the condition to be met for the rows to be returned.</param>
        public static WhereOrChainer Or(this IWhereOr prev, Expression condition)
        {
            return new WhereOrChainer((Chainer)prev, condition);
        }

        /// <summary>
        /// Specifies the additional search condition for the rows returned by the query. 
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="condition">Defines the negative condition to be met for the rows to be returned.</param>
        public static WhereOrChainer OrNot(this IWhereOr prev, Expression condition)
        {
            return new WhereOrChainer((Chainer)prev, condition.Not());
        }

        /// <summary>
        /// Specifies the additional search condition for the rows returned by the query based on the comparison between the two scalar arguments.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first argument in the equality expression.</param>
        /// <param name="argument2">Is the second argument in the equality expression.</param>
        public static WhereOrChainer Or(this IWhereOr prev, ScalarArgument argument1, ValueScalarArgument argument2)
        {
            return new WhereOrChainer((Chainer)prev, argument1, argument2, true);
        }

        /// <summary>
        /// Specifies the additional negative search condition for the rows returned by the query based on the comparison between the two scalar arguments.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first argument in the equality expression.</param>
        /// <param name="argument2">Is the second argument in the equality expression.</param>
        public static WhereOrChainer OrNot(this IWhereOr prev, ScalarArgument argument1, ValueScalarArgument argument2)
        {
            return new WhereOrChainer((Chainer)prev, argument1, argument2, false);
        }

        #endregion

        #region WhereOrExists

        /// <summary>
        /// Specifies a view (subquery) to test for the existence of rows.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="nonSelectView">Is a subquery without the .Select method.</param>
        public static WhereOrChainer OrExists(this IWhereOr prev, INonSelectView nonSelectView)
        {
            return new WhereOrChainer((Chainer)prev, nonSelectView, true);
        }

        /// <summary>
        /// Specifies a view (subquery) to test for the negation of the existence of rows.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="nonSelectView">Is a subquery without the .Select method.</param>
        public static WhereOrChainer OrNotExists(this IWhereOr prev, INonSelectView nonSelectView)
        {
            return new WhereOrChainer((Chainer)prev, nonSelectView, false);
        }

        #endregion

    }
}
