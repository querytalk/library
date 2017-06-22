#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates all public extension methods of the QueryTalk library.
    /// </summary>
    public static partial class Extensions
    {

        /// <summary>
        /// Compares the equality of two expressions.
        /// </summary>
        /// <param name="leftExpression">Is a string identifier of a column.</param>
        /// <param name="rightExpression">Is a column to compare with leftExpression.</param>
        public static Expression EqualTo(this string leftExpression, ValueScalarArgument rightExpression)
        {
            return new Expression(Operator.Equal, leftExpression, rightExpression);
        }

        /// <summary>
        /// Compares the equality of two expressions.
        /// </summary>
        /// <param name="leftExpression">Is the left expression.</param>
        /// <param name="rightExpression">Is the right expression.</param>
        public static Expression EqualTo(this IScalar leftExpression, ValueScalarArgument rightExpression)
        {
            return new Expression(Operator.Equal, new ScalarArgument(leftExpression), rightExpression);
        }

        /// <summary>
        /// Compares the non-equality of two expressions.
        /// </summary>
        /// <param name="leftExpression">Is the left expression.</param>
        /// <param name="rightExpression">Is the right expression.</param>
        public static Expression NotEqualTo(this string leftExpression, ValueScalarArgument rightExpression)
        {
            return new Expression(Operator.NotEqual, leftExpression, rightExpression);
        }

        /// <summary>
        /// Compares the non-equality of two expressions.
        /// </summary>
        /// <param name="leftExpression">Is the left expression.</param>
        /// <param name="rightExpression">Is the right expression.</param>
        public static Expression NotEqualTo(this IScalar leftExpression, ValueScalarArgument rightExpression)
        {
            return new Expression(Operator.NotEqual, new ScalarArgument(leftExpression), rightExpression);
        }

        #region Internal - (Scalar, Scalar)

        internal static Expression EqualTo(this IScalar leftExpression, ScalarArgument rightExpression)
        {
            return new Expression(Operator.Equal, new ScalarArgument(leftExpression), rightExpression);
        }

        internal static Expression NotEqualTo(this IScalar leftExpression, ScalarArgument rightExpression)
        {
            return new Expression(Operator.NotEqual, new ScalarArgument(leftExpression), rightExpression);
        }

        #endregion

    }
}
