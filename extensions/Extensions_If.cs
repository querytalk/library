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
        /// Imposes a condition on the execution of a Transact-SQL statement. The Transact-SQL statement that follows an IF keyword and its condition is executed if the condition is satisfied.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="condition">Is a Boolean expression that returns TRUE or FALSE.</param>
        public static IfChainer If(this IAny prev, Expression condition)
        {
            return new IfChainer((Chainer)prev, condition);
        }

        /// <summary>
        /// Imposes a condition on the execution of a Transact-SQL statement. The Transact-SQL statement that follows an IF keyword and its condition is executed if the condition is satisfied.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first argument in the equality expression.</param>
        /// <param name="argument2">Is the second argument in the equality expression.</param>
        /// <param name="equality">Specifies the type of the relation between the two arguments. true indicates the equality operator, false indicates the inequality operator.</param>
        public static IfChainer If(this IAny prev, ScalarArgument argument1, ValueScalarArgument argument2, bool equality = true)
        {
            return new IfChainer((Chainer)prev, argument1, argument2, equality);
        }

        /// <summary>
        /// The optional ELSE IF keyword imposes a condition on the execution of a Transact-SQL statement. It is executed when the previous IF condition on the same nested level is not satisfied.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="condition">Is a Boolean expression that returns TRUE or FALSE.</param>
        public static ElseIfChainer ElseIf(this IAny prev, Expression condition)
        {
            return new ElseIfChainer((Chainer)prev, condition);
        }

        /// <summary>
        /// The optional ELSE IF keyword imposes a condition on the execution of a Transact-SQL statement. It is executed when the previous IF condition on the same nested level is not satisfied.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first argument in the equality expression.</param>
        /// <param name="argument2">Is the second argument in the equality expression.</param>
        /// <param name="equality">Specifies the type of the relation between the two arguments. true indicates the equality operator, false indicates the inequality operator.</param>
        public static ElseIfChainer ElseIf(this IAny prev, ScalarArgument argument1, ValueScalarArgument argument2, bool equality = true)
        {
            return new ElseIfChainer((Chainer)prev, argument1, argument2, equality);
        }

        /// <summary>
        /// The optional ELSE keyword is executed when none of the previous IF/ELSE IF conditions on the same nested level are satisfied.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static ElseChainer Else(this IAny prev)
        {
            return new ElseChainer((Chainer)prev);
        }

        /// <summary>
        /// Ends the IF block.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static EndIfChainer EndIf(this IAny prev)
        {
            return new EndIfChainer((Chainer)prev);
        }
    }
}
