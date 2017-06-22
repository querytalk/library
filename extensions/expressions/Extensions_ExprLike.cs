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
        /// Determines whether a specific character string matches a specified pattern.
        /// </summary>
        /// <param name="matchExpression">Is a string identifier of a column.</param>
        /// <param name="pattern">
        /// Is the specific string of characters to search for in match expression.
        /// </param>
        public static Expression Like(this string matchExpression, ValueScalarArgument pattern)
        {
            return new Expression(Operator.Like, matchExpression, pattern);
        }

        /// <summary>
        /// Determines whether a specific character string matches a specified pattern.
        /// </summary>
        /// <param name="matchExpression">Is any valid expression of character data type.</param>
        /// <param name="pattern">
        /// Is the specific string of characters to search for in match expression.
        /// </param>
        public static Expression Like(this IScalar matchExpression, ValueScalarArgument pattern)
        {
            return new Expression(Operator.Like, new ScalarArgument(matchExpression), pattern);
        }

        /// <summary>
        /// Determines whether a specific character string matches a specified pattern.
        /// </summary>
        /// <param name="matchExpression">Is a string identifier of a column.</param>
        /// <param name="pattern">
        /// Is the specific string of characters to search for in match expression.
        /// </param>
        /// <param name="escapeCharacter">Is a character that is put in front of a wildcard character to indicate that the wildcard should be interpreted as a regular character and not as a wildcard.</param>
        public static Expression Like(this string matchExpression, ValueScalarArgument pattern, char escapeCharacter)
        {
            return new Expression(Operator.Like, matchExpression, pattern, escapeCharacter.ToString());
        }

        /// <summary>
        /// Determines whether a specific character string matches a specified pattern.
        /// </summary>
        /// <param name="matchExpression">Is any valid expression of character data type.</param>
        /// <param name="pattern">
        /// Is the specific string of characters to search for in match expression.
        /// </param>
        /// <param name="escapeCharacter">Is a character that is put in front of a wildcard character to indicate that the wildcard should be interpreted as a regular character and not as a wildcard.</param>
        public static Expression Like(this IScalar matchExpression, ValueScalarArgument pattern, char escapeCharacter)
        {
            return new Expression(Operator.Like, new ScalarArgument(matchExpression), pattern, escapeCharacter.ToString());
        }

        /// <summary>
        /// Determines whether a specific character string does NOT match a specified pattern.
        /// </summary>
        /// <param name="matchExpression">Is a string identifier of a column.</param>
        /// <param name="pattern">
        /// Is the specific string of characters to search for in match expression.
        /// </param>
        public static Expression NotLike(this string matchExpression, ValueScalarArgument pattern)
        {
            return new Expression(Operator.NotLike, matchExpression, pattern);
        }

        /// <summary>
        /// Determines whether a specific character string does NOT match a specified pattern.
        /// </summary>
        /// <param name="matchExpression">Is any valid expression of character data type.</param>
        /// <param name="pattern">
        /// Is the specific string of characters to search for in match expression.
        /// </param>
        public static Expression NotLike(this IScalar matchExpression, ValueScalarArgument pattern)
        {
            return new Expression(Operator.NotLike, new ScalarArgument(matchExpression), pattern);
        }

        /// <summary>
        /// Determines whether a specific character string does NOT match a specified pattern.
        /// </summary>
        /// <param name="matchExpression">Is a string identifier of a column.</param>
        /// <param name="pattern">
        /// Is the specific string of characters to search for in match expression.
        /// </param>
        /// <param name="escapeCharacter">Is a character that is put in front of a wildcard character to indicate that the wildcard should be interpreted as a regular character and not as a wildcard.</param>
        public static Expression NotLike(this string matchExpression, ValueScalarArgument pattern, char escapeCharacter)
        {
            return new Expression(Operator.NotLike, matchExpression, pattern, escapeCharacter.ToString());
        }

        /// <summary>
        /// Determines whether a specific character string does NOT match a specified pattern.
        /// </summary>
        /// <param name="matchExpression">Is any valid expression of character data type.</param>
        /// <param name="pattern">
        /// Is the specific string of characters to search for in match expression.
        /// </param>
        /// <param name="escapeCharacter">Is a character that is put in front of a wildcard character to indicate that the wildcard should be interpreted as a regular character and not as a wildcard.</param>
        public static Expression NotLike(this IScalar matchExpression, ValueScalarArgument pattern, char escapeCharacter)
        {
            return new Expression(Operator.NotLike, new ScalarArgument(matchExpression), pattern, escapeCharacter.ToString());
        }

    }
}
