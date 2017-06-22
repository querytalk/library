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
        /// Compares two expressions. When comparing non-null expressions, the result is TRUE if the left operand has a value lower than the right operand; otherwise, the result is FALSE.
        /// </summary>
        /// <param name="leftExpression">Is the left expression.</param>
        /// <param name="rightExpression">Is the right expression.</param>
        public static Expression LessThan(this string leftExpression, ValueScalarArgument rightExpression)
        {
            return new Expression(Operator.LessThan, leftExpression, rightExpression);
        }

        /// <summary>
        /// Compares two expressions. When comparing non-null expressions, the result is TRUE if the left operand has a value lower than the right operand; otherwise, the result is FALSE.
        /// </summary>
        /// <param name="leftExpression">Is the left expression.</param>
        /// <param name="rightExpression">Is the right expression.</param>
        public static Expression LessThan(this IScalar leftExpression, ValueScalarArgument rightExpression)
        {
            return new Expression(Operator.LessThan, new ScalarArgument(leftExpression), rightExpression);
        }

        /// <summary>
        /// Compares two expressions. When comparing non-null expressions, the result is TRUE if the left operand has a value lower than or equal to the right operand; otherwise, the result is FALSE.
        /// </summary>
        /// <param name="leftExpression">Is the left expression.</param>
        /// <param name="rightExpression">Is the right expression.</param>
        public static Expression LessOrEqualThan(this string leftExpression, ValueScalarArgument rightExpression)
        {
            return new Expression(Operator.LessOrEqualThan, leftExpression, rightExpression);
        }

        /// <summary>
        /// Compares two expressions. When comparing non-null expressions, the result is TRUE if the left operand has a value lower than or equal to the right operand; otherwise, the result is FALSE.
        /// </summary>
        /// <param name="leftExpression">Is the left expression.</param>
        /// <param name="rightExpression">Is the right expression.</param>
        public static Expression LessOrEqualThan(this IScalar leftExpression, ValueScalarArgument rightExpression)
        {
            return new Expression(Operator.LessOrEqualThan, new ScalarArgument(leftExpression), rightExpression);
        }

        /// <summary>
        /// Compares two expressions. When comparing non-null expressions, the result is TRUE if the left operand has a value greater than the right operand; otherwise, the result is FALSE.
        /// </summary>
        /// <param name="leftExpression">Is the left expression.</param>
        /// <param name="rightExpression">Is the right expression.</param>
        public static Expression NotLessThan(this string leftExpression, ValueScalarArgument rightExpression)
        {
            return new Expression(Operator.NotLessThan, leftExpression, rightExpression);
        }

        /// <summary>
        /// Compares two expressions. When comparing non-null expressions, the result is TRUE if the left operand has a value greater than the right operand; otherwise, the result is FALSE.
        /// </summary>
        /// <param name="leftExpression">Is the left expression.</param>
        /// <param name="rightExpression">Is the right expression.</param>
        public static Expression NotLessThan(this IScalar leftExpression, ValueScalarArgument rightExpression)
        {
            return new Expression(Operator.NotLessThan, new ScalarArgument(leftExpression), rightExpression);
        }
    }
}
