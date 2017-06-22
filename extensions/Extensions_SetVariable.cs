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
        /// Sets the specified local variable or parameter, previously created by using the DECLARE statement or .Param method in the procedure body, to the specified value.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="variable">Is a name of a previously declared variable or parameter.</param>
        /// <param name="value">Is a value to be assigned to the variable or parameter.</param>
        public static SetChainer Set(this IAny prev, string variable, VariableArgument value)
        {
            return new SetChainer((Chainer)prev, variable, value);
        }

        /// <summary>
        /// Increments a value of a variable.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="variable">A variable to increment.</param>
        /// <param name="increment">An increment value.</param>
        public static SetChainer SetIncrement(this IAny prev, string variable, int increment = 1)
        {
            return new SetChainer((Chainer)prev, variable, variable.Plus(increment));
        }

        /// <summary>
        /// Decrements a value of a variable.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="variable">A variable to increment.</param>
        /// <param name="decrement">A decrement value.</param>
        public static SetChainer SetDecrement(this IAny prev, string variable, int decrement = -1)
        {
            return new SetChainer((Chainer)prev, variable, variable.Plus(decrement));
        }

        /// <summary>
        /// Sets the local variable, declared by a precedent .Declare method, to the specified value.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="value">Is a value to be assigned to the variable or parameter.</param>
        public static SetChainer Set(this IDeclareSet prev, VariableArgument value)
        {
            return new SetChainer((Chainer)prev, null, value, true);
        }
    }
}
