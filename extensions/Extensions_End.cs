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
        /// Ends the view design and creates the View object.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static View EndView(this IEndView prev)
        {
            return new View((Chainer)prev);
        }

        /// <summary>
        /// Ends the procedure design and creates the Procedure object.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static Procedure EndProc(this IEndProc prev)
        {
            return new Procedure((Chainer)prev);
        }

        /// <summary>
        /// Ends the snippet design and creates the Snippet object.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static Snippet EndSnip(this IEndProc prev)
        {
            return new Snippet((Chainer)prev);
        }

        internal static Procedure EndProcInternal(this IEndProc prev)
        {
            return new Procedure((Chainer)prev, true);
        }

        internal static Snippet EndSnipInternal(this IEndProc prev)
        {
            return new Snippet((Chainer)prev, true);
        }
    }
}
