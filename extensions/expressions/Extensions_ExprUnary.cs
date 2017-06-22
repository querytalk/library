#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        #region IsNull

        /// <summary>
        /// Determines whether a specified expression is NULL.
        /// </summary>
        /// <param name="expression">Is the expression to evaluate.</param>
        public static Expression IsNull(this string expression)
        {
            return new Expression(Operator.IsNull, expression);
        }

        /// <summary>
        /// Determines whether a specified expression is NULL.
        /// </summary>
        /// <param name="expression">Is the expression to evaluate.</param>
        public static Expression IsNull(this IScalar expression)
        {
            return new Expression(Operator.IsNull, new ScalarArgument(expression));
        }

        /// <summary>
        /// Determines whether a specified expression is NOT NULL.
        /// </summary>
        /// <param name="expression">Is the expression to evaluate.</param>
        public static Expression IsNotNull(this string expression)
        {
            return new Expression(Operator.IsNotNull, expression);
        }

        /// <summary>
        /// Determines whether a specified expression is NULL.
        /// </summary>
        /// <param name="expression">Is the expression to evaluate.</param>
        public static Expression IsNotNull(this IScalar expression)
        {
            return new Expression(Operator.IsNotNull, new ScalarArgument(expression));
        }

        #endregion

        #region Not

        /// <summary>
        /// Negates a Boolean input.
        /// </summary>
        /// <param name="expression">Is the Boolean expression to negate.</param>
        public static Expression Not(this Expression expression)
        {
            return new Expression(Operator.Not, expression);
        }

        #endregion

        #region Exists

        /// <summary>
        /// Specifies a view (subquery) to test for the existence of rows.
        /// </summary>
        /// <param name="view">Is a view to evaluate. The COMPUTE clause and the INTO keyword are not allowed.</param>
        public static Expression Exists(this View view)
        {
            return new Expression(Operator.Exists, view);
        }

        /// <summary>
        /// Specifies a view (subquery) to test for the negation of the existence of rows.
        /// </summary>
        /// <param name="view">Is a view to evaluate. The COMPUTE clause and the INTO keyword are not allowed.</param>
        public static Expression NotExists(this View view)
        {
            return new Expression(Operator.NotExists, view);
        }

        #endregion

        #region Any

        /// <summary>
        /// Compares a scalar value with a single-column set of values. SOME and ANY are equivalent. ANY requires the scalar expression to compare positively to at least one value returned by the view.
        /// </summary>
        /// <param name="view">Is a one-column (scalar) view.</param>
        public static Expression Any(this View view)
        {
            return new Expression(Operator.Any, view);
        }

        #endregion

        #region Some

        /// <summary>
        /// Compares a scalar value with a single-column set of values. SOME and ANY are equivalent. SOME requires the scalar expression to compare positively to at least one value returned by the view.
        /// </summary>
        /// <param name="view">Is a one-column (scalar) view.</param>
        public static Expression Some(this View view)
        {
            return new Expression(Operator.Some, view);
        }

        #endregion

        #region All

        /// <summary>
        /// Compares a scalar value with a single-column set of values. ALL requires the scalar expression to compare positively to every value that is returned by the view.
        /// </summary>
        /// <param name="view">Is a one-column (scalar) view.</param>
        public static Expression All(this View view)
        {
            return new Expression(Operator.All, view);
        }

        #endregion

    }
}
