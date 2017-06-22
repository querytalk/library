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
        /// Generates a comment line.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="value">A comment text.</param>
        public static CommentChainer Comment(this IAny prev, string value)
        {
            return new CommentChainer((Chainer)prev, value);
        }
    }
}
