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
        /// Sets a condition for the repeated execution of a SQL statement or statement block.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        /// <param name="condition">Is a specified loop condition. The statements are executed repeatedly as long as the specified condition is true.</param>
        public static WhileChainer While(this IAny prev, Expression condition)
        {
            return new WhileChainer((Chainer)prev, condition);
        }

        /// <summary>
        /// Sets a condition for the repeated execution of a SQL statement or statement block.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="argument1">Is the first argument in the equality expression.</param>
        /// <param name="argument2">Is the second argument in the equality expression.</param>
        /// <param name="equality">Specifies the type of the relation between the two arguments. true indicates the equality operator, false indicates the inequality operator.</param>
        public static WhileChainer While(this IAny prev, ScalarArgument argument1, ValueScalarArgument argument2, bool equality = true)
        {
            return new WhileChainer((Chainer)prev, argument1, argument2, equality);
        }

        /// <summary>
        /// Causes an exit from the innermost WHILE loop. Any statements that appear after the END keyword, marking the end of the loop, are executed.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        public static BreakChainer Break(this IAny prev)
        {
            return new BreakChainer((Chainer)prev);
        }

        /// <summary>
        /// Causes the WHILE loop to restart, ignoring any statements after the CONTINUE keyword.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        public static ContinueChainer Continue(this IAny prev)
        {
            return new ContinueChainer((Chainer)prev);
        }

        /// <summary>
        /// Ends the WHILE block.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        public static EndWhileChainer EndWhile(this IAny prev)
        {
            return new EndWhileChainer((Chainer)prev);
        }
    }
}
