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
        /// Returns TRUE if the value of test expression is greater than or equal to the value of begin expression and less than or equal to the value of end expression.
        /// </summary>
        /// <param name="test">Is the expression to test for in the range defined by begin expression and end expression.</param>
        /// <param name="beginExpression">Is a begin expression.</param>
        /// <param name="endExpression">Is an end expression.</param>
        public static Expression Between(this string test, ValueScalarArgument beginExpression, ValueScalarArgument endExpression)
        {
            return new Expression(Operator.Between, test, beginExpression, endExpression);
        }

        /// <summary>
        /// Returns TRUE if the value of test expression is greater than or equal to the value of begin expression and less than or equal to the value of end expression.
        /// </summary>
        /// <param name="test">Is the expression to test for in the range defined by begin expression and end expression.</param>
        /// <param name="beginExpression">Is a begin expression.</param>
        /// <param name="endExpression">Is an end expression.</param>
        public static Expression Between(this IScalar test, ValueScalarArgument beginExpression, ValueScalarArgument endExpression)
        {
            return new Expression(Operator.Between, new ScalarArgument(test), beginExpression, endExpression);
        }

        /// <summary>
        /// Returns TRUE if the value of test expression is NOT greater than or equal to the value of begin expression and less than or equal to the value of end expression.
        /// </summary>
        /// <param name="test">Is the expression to test for NOT to be in the range defined by begin expression and end expression.</param>
        /// <param name="beginExpression">Is a begin expression.</param>
        /// <param name="endExpression">Is an end expression.</param>
        public static Expression NotBetween(this string test, ValueScalarArgument beginExpression, ValueScalarArgument endExpression)
        {
            return new Expression(Operator.NotBetween, test, beginExpression, endExpression);
        }

        /// <summary>
        /// Returns TRUE if the value of test expression is NOT greater than or equal to the value of begin expression and less than or equal to the value of end expression.
        /// </summary>
        /// <param name="test">Is the expression to test for NOT to be in the range defined by begin expression and end expression.</param>
        /// <param name="beginExpression">Is a begin expression.</param>
        /// <param name="endExpression">Is an end expression.</param>
        public static Expression NotBetween(this IScalar test, ValueScalarArgument beginExpression, ValueScalarArgument endExpression)
        {
            return new Expression(Operator.NotBetween, new ScalarArgument(test), beginExpression, endExpression);
        }

    }
}
