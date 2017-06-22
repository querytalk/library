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
        /// Defines a label used by GOTO.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="value">Is the point after which processing starts if a GOTO is targeted to that label.</param>
        public static LabelChainer Label(this IAny prev, string value)
        {
            return new LabelChainer((Chainer)prev, value);
        }

        /// <summary>
        /// Alters the flow of execution to a label.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="value">Is the point after which processing starts if a GOTO is targeted to that label.</param>
        public static GotoChainer Goto(this IAny prev, string value)
        {
            return new GotoChainer((Chainer)prev, value);
        }
    }
}
