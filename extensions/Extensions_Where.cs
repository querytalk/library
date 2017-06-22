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
        /// Specifies the search condition for the rows returned by the query. 
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="condition">Defines the condition to be met for the rows to be returned.</param>
        public static WhereChainer Where(this IWhere prev, Expression condition)
        {
            return new WhereChainer((Chainer)prev, condition);
        }

        /// <summary>
        /// Specifies the search condition for the rows returned by the query. 
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="condition">Defines the negative condition to be met for the rows to be returned.</param>
        public static WhereChainer WhereNot(this IWhere prev, Expression condition)
        {
            return new WhereChainer((Chainer)prev, condition.Not());
        }

        /// <summary>
        /// Specifies the search condition for the rows returned by the query based on the comparison between the two scalar arguments.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first argument in the equality expression.</param>
        /// <param name="argument2">Is the second argument in the equality expression.</param>
        public static WhereChainer Where(this IWhere prev, ScalarArgument argument1, ValueScalarArgument argument2)
        {
            return new WhereChainer((Chainer)prev, argument1, argument2, true);
        }

        /// <summary>
        /// Specifies the negative search condition for the rows returned by the query based on the comparison between the two scalar arguments.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first argument in the equality expression.</param>
        /// <param name="argument2">Is the second argument in the equality expression.</param>
        public static WhereChainer WhereNot(this IWhere prev, ScalarArgument argument1, ValueScalarArgument argument2)
        {
            return new WhereChainer((Chainer)prev, argument1, argument2, false);
        }

        /// <summary>
        /// Specifies a view (subquery) to test for the existence of rows.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="nonSelectView">Is a subquery without the .Select method.</param>
        public static WhereChainer WhereExists(this IWhere prev, INonSelectView nonSelectView)
        {
            return new WhereChainer((Chainer)prev, nonSelectView, true);
        }

        /// <summary>
        /// Specifies a view (subquery) to test for the negation of the existence of rows.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="nonSelectView">Is a subquery without the .Select method.</param>
        public static WhereChainer WhereNotExists(this IWhere prev, INonSelectView nonSelectView)
        {
            return new WhereChainer((Chainer)prev, nonSelectView, false);
        }

    }
}
