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
        /// Exits unconditionally from a batch.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static ReturnChainer Return(this IAny prev)
        {
            return new ReturnChainer((Chainer)prev);
        }

        /// <summary>
        /// Exits unconditionally from a batch.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="returnValue">Is a return value.</param>
        public static ReturnChainer Return(this IAny prev, int returnValue)
        {
            return new ReturnChainer((Chainer)prev, returnValue);
        }

        /// <summary>
        /// Exits unconditionally from a batch.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="returnVariable">Is a variable whose value is returned.</param>
        public static ReturnChainer Return(this IAny prev, string returnVariable)
        {
            return new ReturnChainer((Chainer)prev, returnVariable);
        }
    }
}
