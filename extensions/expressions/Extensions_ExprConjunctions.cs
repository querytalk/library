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
        /// Combines two Boolean expressions and returns TRUE when both expressions are TRUE. When more than one logical operator, the order of evaluation can be controled using parentheses.
        /// </summary>
        /// <param name="firstExpression">Is any valid expression that returns a Boolean value.</param>
        /// <param name="secondExpression">Is any valid expression that returns a Boolean value.</param>
        public static Expression And(this Expression firstExpression, Expression secondExpression)
        {
            return new Expression(Operator.And, firstExpression, secondExpression);
        }

        /// <summary>
        /// Combines two Boolean expressions and returns TRUE when both expressions are TRUE. When more than one logical operator, the order of evaluation can be controled using parentheses.
        /// </summary>
        /// <param name="firstExpression">Is any valid expression that returns a Boolean value.</param>
        /// <param name="argument1">Is the first argument in the equality expression.</param>
        /// <param name="argument2">Is the second argument in the equality expression.</param>
        public static Expression And(this Expression firstExpression, ScalarArgument argument1, ValueScalarArgument argument2)
        {
            return new Expression(Operator.And, firstExpression, Expression.EqualitySimplifier(argument1, argument2, true));
        }

        /// <summary>
        /// Combines two Boolean expressions and returns TRUE when both expressions, where the second is negated, are TRUE. When more than one logical operator, the order of evaluation can be controled using parentheses.
        /// </summary>
        /// <param name="firstExpression">Is any valid expression that returns a Boolean value.</param>
        /// <param name="secondExpression">Is any valid expression that returns a Boolean value. This expression is negated.</param>
        public static Expression AndNot(this Expression firstExpression, Expression secondExpression)
        {
            return new Expression(Operator.AndNot, firstExpression, secondExpression);
        }

        /// <summary>
        /// Combines two Boolean expressions and returns TRUE when both expressions are TRUE. Specifies the additional negative search condition for the rows returned by the query based on the comparison between the two scalar arguments.
        /// </summary>
        /// <param name="firstExpression">Is any valid expression that returns a Boolean value.</param>
        /// <param name="argument1">Is the first argument in the equality expression.</param>
        /// <param name="argument2">Is the second argument in the equality expression.</param>
        public static Expression AndNot(this Expression firstExpression, ScalarArgument argument1, ValueScalarArgument argument2)
        {
            return new Expression(Operator.And, firstExpression, Expression.EqualitySimplifier(argument1, argument2, false));
        }

        /// <summary>
        /// Combines two Boolean expressions and returns TRUE when at least one expression is TRUE. When more than one logical operator, the order of evaluation can be controled using parentheses.
        /// </summary>
        /// <param name="firstExpression">Is any valid expression that returns a Boolean value.</param>
        /// <param name="secondExpression">Is any valid expression that returns a Boolean value.</param>
        public static Expression Or(this Expression firstExpression, Expression secondExpression)
        {
            return new Expression(Operator.Or, firstExpression, secondExpression);
        }

        /// <summary>
        /// Combines two Boolean expressions and returns TRUE when at least one expression is TRUE. When more than one logical operator, the order of evaluation can be controled using parentheses.
        /// </summary>
        /// <param name="firstExpression">Is any valid expression that returns a Boolean value.</param>
        /// <param name="argument1">Is the first argument in the equality expression.</param>
        /// <param name="argument2">Is the second argument in the equality expression.</param>
        public static Expression Or(this Expression firstExpression, ScalarArgument argument1, ValueScalarArgument argument2)
        {
            return new Expression(Operator.Or, firstExpression, Expression.EqualitySimplifier(argument1, argument2, true));
        }

        /// <summary>
        /// Combines two Boolean expressions and returns TRUE when at least one expression, where the second is negated, is TRUE. When more than one logical operator, the order of evaluation can be controled using parentheses.
        /// </summary>
        /// <param name="firstExpression">Is any valid expression that returns a Boolean value.</param>
        /// <param name="secondExpression">Is any valid expression that returns a Boolean value. This expression is negated.</param>
        public static Expression OrNot(this Expression firstExpression, Expression secondExpression)
        {
            return new Expression(Operator.OrNot, firstExpression, secondExpression);
        }

        /// <summary>
        /// Combines two Boolean expressions and returns TRUE when at least one expression, where the second is negated, is TRUE. When more than one logical operator, the order of evaluation can be controled using parentheses.
        /// </summary>
        /// <param name="firstExpression">Is any valid expression that returns a Boolean value.</param>
        /// <param name="argument1">Is the first argument in the equality expression.</param>
        /// <param name="argument2">Is the second argument in the equality expression.</param>
        public static Expression OrNot(this Expression firstExpression, ScalarArgument argument1, ValueScalarArgument argument2)
        {
            return new Expression(Operator.OrNot, firstExpression, Expression.EqualitySimplifier(argument1, argument2, false));
        }
    }
}
