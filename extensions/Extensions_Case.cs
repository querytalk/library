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
        /// Evaluates an input expression in a simple CASE statement.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument">An input expression.</param>
        public static CaseValueChainer Input(this ICaseInput prev, ScalarArgument argument)
        {
            return new CaseValueChainer((Chainer)prev, argument);
        }

        /// <summary>
        /// Evaluates an input expression against the input in a simple CASE format.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="expression">An input expression.</param>
        public static CaseWhenChainer When(this ICaseWhen prev, ValueScalarArgument expression)
        {
            return new CaseWhenChainer((Chainer)prev, expression);
        }

        /// <summary>
        /// Evaluates an input expression against the input in a simple CASE format.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first argument in the equality expression.</param>
        /// <param name="argument2">Is the second argument in the equality expression.</param>
        /// <param name="equality">Specifies the type of the relation between the two arguments. true indicates the equality operator, false indicates the inequality operator.</param>
        public static CaseWhenChainer When(this ICaseWhen prev, ScalarArgument argument1, ValueScalarArgument argument2, bool equality = true)
        {
            return new CaseWhenChainer((Chainer)prev, argument1, argument2, equality);
        }

        /// <summary>
        /// Returns the expression when an input or Boolean expression evaluates to TRUE.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="expression">The returning expression.</param>
        public static CaseThenChainer Then(this ICaseThen prev, ValueScalarArgument expression)
        {
            return new CaseThenChainer((Chainer)prev, expression);
        }

        /// <summary>
        /// Returns the expression when an input or Boolean expression evaluates to TRUE.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first argument in the equality expression.</param>
        /// <param name="argument2">Is the second argument in the equality expression.</param>
        /// <param name="equality">Specifies the type of the relation between the two arguments. true indicates the equality operator, false indicates the inequality operator.</param>
        public static CaseThenChainer Then(this ICaseThen prev, ScalarArgument argument1, ValueScalarArgument argument2, bool equality = true)
        {
            return new CaseThenChainer((Chainer)prev, argument1, argument2, equality);
        }

        /// <summary>
        /// Returns the expression when no comparison operation evaluates to TRUE.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="expression">The returning expression.</param>
        public static CaseElseChainer Else(this ICaseElse prev, ValueScalarArgument expression)
        {
            return new CaseElseChainer((Chainer)prev, expression);
        }

        /// <summary>
        /// Returns the expression when no comparison operation evaluates to TRUE.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first argument in the equality expression.</param>
        /// <param name="argument2">Is the second argument in the equality expression.</param>
        /// <param name="equality">Specifies the type of the relation between the two arguments. true indicates the equality operator, false indicates the inequality operator.</param>
        public static CaseElseChainer Else(this ICaseElse prev, ScalarArgument argument1, ValueScalarArgument argument2, bool equality = true)
        {
            return new CaseElseChainer((Chainer)prev, argument1, argument2, equality);
        }

        /// <summary>
        /// Ends the CASE statement.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <returns></returns>
        public static CaseExpressionChainer End(this IEndCase prev)
        {
            return new CaseExpressionChainer((Chainer)prev);
        }
    }
}
