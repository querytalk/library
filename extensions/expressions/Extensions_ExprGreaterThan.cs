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
        /// Compares two expressions. When comparing non-null expressions, the result is TRUE if the left operand has a value higher than the right operand; otherwise, the result is FALSE.
        /// </summary>
        /// <param name="leftExpression">Is the left expression.</param>
        /// <param name="rightExpression">Is the right expression.</param>
        public static Expression GreaterThan(this string leftExpression, ValueScalarArgument rightExpression)
        {
            return new Expression(Operator.GreaterThan, leftExpression, rightExpression);
        }

        /// <summary>
        /// Compares two expressions. When comparing non-null expressions, the result is TRUE if the left operand has a value higher than the right operand; otherwise, the result is FALSE.
        /// </summary>
        /// <param name="leftExpression">Is the left expression.</param>
        /// <param name="rightExpression">Is the right expression.</param>
        public static Expression GreaterThan(this IScalar leftExpression, ValueScalarArgument rightExpression)
        {
            return new Expression(Operator.GreaterThan, new ScalarArgument(leftExpression), rightExpression);
        }

        /// <summary>
        /// Compares two expressions. When comparing non-null expressions, the result is TRUE if the left operand has a value higher than or equal to the right operand; otherwise, the result is FALSE.
        /// </summary>
        /// <param name="leftExpression">Is the left expression.</param>
        /// <param name="rightExpression">Is the right expression.</param>
        public static Expression GreaterOrEqualThan(this string leftExpression, ValueScalarArgument rightExpression)
        {
            return new Expression(Operator.GreaterOrEqualThan, leftExpression, rightExpression);
        }

        /// <summary>
        /// Compares two expressions. When comparing non-null expressions, the result is TRUE if the left operand has a value higher than or equal to the right operand; otherwise, the result is FALSE.
        /// </summary>
        /// <param name="leftExpression">Is the left expression.</param>
        /// <param name="rightExpression">Is the right expression.</param>
        public static Expression GreaterOrEqualThan(this IScalar leftExpression, ValueScalarArgument rightExpression)
        {
            return new Expression(Operator.GreaterOrEqualThan, new ScalarArgument(leftExpression), rightExpression);
        }

        /// <summary>
        /// Compares two expressions. When comparing non-null expressions, the result is TRUE if the left operand has a value lower than the right operand; otherwise, the result is FALSE.
        /// </summary>
        /// <param name="leftExpression">Is the left expression.</param>
        /// <param name="rightExpression">Is the right expression.</param>
        public static Expression NotGreaterThan(this string leftExpression, ValueScalarArgument rightExpression)
        {
            return new Expression(Operator.NotGreaterThan, leftExpression, rightExpression);
        }

        /// <summary>
        /// Compares two expressions. When comparing non-null expressions, the result is TRUE if the left operand has a value lower than the right operand; otherwise, the result is FALSE.
        /// </summary>
        /// <param name="leftExpression">Is the left expression.</param>
        /// <param name="rightExpression">Is the right expression.</param>
        public static Expression NotGreaterThan(this IScalar leftExpression, ValueScalarArgument rightExpression)
        {
            return new Expression(Operator.NotGreaterThan, new ScalarArgument(leftExpression), rightExpression);
        }
    }
}
