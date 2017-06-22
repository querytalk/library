#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        #region Plus

        /// <summary>
        /// Adds two expressions.
        /// </summary>
        /// <param name="firstExpression">The first expression to add.</param>
        /// <param name="secondExpression">The second expression to add.</param>
        public static Expression Plus(this string firstExpression, ScalarArgument secondExpression)
        {
            return new Expression(Operator.Plus, firstExpression, secondExpression);
        }

        /// <summary>
        /// Adds two expressions.
        /// </summary>
        /// <param name="firstExpression">The first expression to add.</param>
        /// <param name="secondExpression">The second expression to add.</param>
        public static Expression Plus(this IScalar firstExpression, ScalarArgument secondExpression)
        {
            return new Expression(Operator.Plus, new ScalarArgument(firstExpression), secondExpression);
        }

        #endregion

        #region Minus

        /// <summary>
        /// Substracts two numbers.
        /// </summary>
        /// <param name="firstNumeric">The first expression to substract.</param>
        /// <param name="secondNumeric">The second expression to substract.</param>
        public static Expression Minus(this string firstNumeric, ScalarArgument secondNumeric)
        {
            return new Expression(Operator.Minus, firstNumeric, secondNumeric);
        }

        /// <summary>
        /// Substracts two numbers.
        /// </summary>
        /// <param name="firstNumeric">The first numeric expression to substract.</param>
        /// <param name="secondNumeric">The second numeric expression to substract.</param>
        public static Expression Minus(this IScalar firstNumeric, ScalarArgument secondNumeric)
        {
            return new Expression(Operator.Minus, new ScalarArgument(firstNumeric), secondNumeric);
        }

        #endregion

        #region MultiplyBy

        /// <summary>
        /// Multiplies two numbers.
        /// </summary>
        /// <param name="firstNumeric">The first numeric expression to multiply.</param>
        /// <param name="secondNumeric">The second numeric expression to multiply.</param>
        public static Expression MultiplyBy(this string firstNumeric, ScalarArgument secondNumeric)
        {
            return new Expression(Operator.MultiplyBy, firstNumeric, secondNumeric);
        }

        /// <summary>
        /// Multiplies two numbers.
        /// </summary>
        /// <param name="firstNumeric">The first numeric expression to multiply.</param>
        /// <param name="secondNumeric">The second numeric expression to multiply.</param>
        public static Expression MultiplyBy(this IScalar firstNumeric, ScalarArgument secondNumeric)
        {
            return new Expression(Operator.MultiplyBy, new ScalarArgument(firstNumeric), secondNumeric);
        }

        #endregion

        #region DivideBy

        /// <summary>
        /// Divides two numbers.
        /// </summary>
        /// <param name="dividend">Is the numeric expression to divide.</param>
        /// <param name="divisor">Is the numeric expression by which to divide the dividend.</param>
        public static Expression DivideBy(this string dividend, ScalarArgument divisor)
        {
            return new Expression(Operator.DivideBy, dividend, divisor);
        }

        /// <summary>
        /// Divides two numbers.
        /// </summary>
        /// <param name="dividend">Is the numeric expression to divide.</param>
        /// <param name="divisor">Is the numeric expression by which to divide the dividend.</param>
        public static Expression DivideBy(this IScalar dividend, ScalarArgument divisor)
        {
            return new Expression(Operator.DivideBy, new ScalarArgument(dividend), divisor);
        }

        #endregion

        #region Modulo

        /// <summary>
        /// Returns the remainder of one number divided by another.
        /// </summary>
        /// <param name="dividend">Is the numeric expression to divide.</param>
        /// <param name="divisor">Is the numeric expression by which to divide the dividend.</param>
        public static Expression Modulo(this string dividend, ScalarArgument divisor)
        {
            return new Expression(Operator.Modulo, dividend, divisor);
        }

        /// <summary>
        /// Returns the remainder of one number divided by another.
        /// </summary>
        /// <param name="dividend">Is the numeric expression to divide.</param>
        /// <param name="divisor">Is the numeric expression by which to divide the dividend.</param>
        public static Expression Modulo(this IScalar dividend, ScalarArgument divisor)
        {
            return new Expression(Operator.Modulo, new ScalarArgument(dividend), divisor);
        }

        #endregion

        #region AndBitwise

        /// <summary>
        /// Performs a bitwise logical AND operation between two integer values.
        /// </summary>
        /// <param name="firstNumeric">Is the first number for the bitwise operation.</param>
        /// <param name="secondNumeric">Is the second number for the bitwise operation.</param>
        public static Expression AndBitwise(this string firstNumeric, ScalarArgument secondNumeric)
        {
            return new Expression(Operator.AndBitwise, firstNumeric, secondNumeric);
        }

        /// <summary>
        /// Performs a bitwise logical AND operation between two integer values.
        /// </summary>
        /// <param name="firstNumeric">Is the first number for the bitwise operation.</param>
        /// <param name="secondNumeric">Is the second number for the bitwise operation.</param>
        public static Expression AndBitwise(this IScalar firstNumeric, ScalarArgument secondNumeric)
        {
            return new Expression(Operator.AndBitwise, new ScalarArgument(firstNumeric), secondNumeric);
        }

        #endregion

        #region OrBitwise

        /// <summary>
        /// Performs a bitwise logical OR operation between two integer values.
        /// </summary>
        /// <param name="firstNumeric">Is the first number for the bitwise operation.</param>
        /// <param name="secondNumeric">Is the second number for the bitwise operation.</param>
        public static Expression OrBitwise(this string firstNumeric, ScalarArgument secondNumeric)
        {
            return new Expression(Operator.OrBitwise, firstNumeric, secondNumeric);
        }

        /// <summary>
        /// Performs a bitwise logical OR operation between two integer values.
        /// </summary>
        /// <param name="firstNumeric">Is the first number for the bitwise operation.</param>
        /// <param name="secondNumeric">Is the second number for the bitwise operation.</param>
        public static Expression OrBitwise(this IScalar firstNumeric, ScalarArgument secondNumeric)
        {
            return new Expression(Operator.OrBitwise, new ScalarArgument(firstNumeric), secondNumeric);
        }

        #endregion

        #region ExclusiveOrBitwise

        /// <summary>
        /// Performs a bitwise exclusive OR operation between two integer values.
        /// </summary>
        /// <param name="firstNumeric">Is the first number for the bitwise operation.</param>
        /// <param name="secondNumeric">Is the second number for the bitwise operation.</param>
        public static Expression ExclusiveOrBitwise(this string firstNumeric, ScalarArgument secondNumeric)
        {
            return new Expression(Operator.ExclusiveOrBitwise, firstNumeric, secondNumeric);
        }

        /// <summary>
        /// Performs a bitwise exclusive OR operation between two integer values.
        /// </summary>
        /// <param name="firstNumeric">Is the first number for the bitwise operation.</param>
        /// <param name="secondNumeric">Is the second number for the bitwise operation.</param>
        public static Expression ExclusiveOrBitwise(this IScalar firstNumeric, ScalarArgument secondNumeric)
        {
            return new Expression(Operator.ExclusiveOrBitwise, new ScalarArgument(firstNumeric), secondNumeric);
        }

        #endregion

        #region BitwiseNot

        /// <summary>
        /// Performs a bitwise logical NOT operation on an integer value.
        /// </summary>
        /// <param name="numeric">Is the number for the bitwise operation.</param>
        public static Expression BitwiseNot(this string numeric)
        {
            return new Expression(Operator.BitwiseNot, numeric);
        }

        /// <summary>
        /// Performs a bitwise logical NOT operation on an integer value.
        /// </summary>
        /// <param name="numeric">Is the number for the bitwise operation.</param>
        public static Expression BitwiseNot(this IScalar numeric)
        {
            return new Expression(Operator.BitwiseNot, new ScalarArgument(numeric));
        }

        #endregion

    }
}
