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
        /// A TRY block of a try-catch construct.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static TryChainer Try(this IAny prev)
        {
            return new TryChainer((Chainer)prev);
        }

        /// <summary>
        /// A CATCH block of a try-catch construct.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static CatchChainer Catch(this IAny prev)
        {
            return new CatchChainer((Chainer)prev);
        }

        /// <summary>
        /// Ends the try-catch construct.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static EndTryCatchChainer EndTryCatch(this IAny prev)
        {
            return new EndTryCatchChainer((Chainer)prev);
        }
    }
}
