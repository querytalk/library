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
        /// Injects a SQL code directly into the procedure body.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="sqlOrInliner">Is a SQL code or SQL inliner.</param>
        public static InjectChainer Inject(this IAny prev, string sqlOrInliner)
        {
            return new InjectChainer((Chainer)prev, sqlOrInliner);
        }

        /// <summary>
        /// Injects a snippet directly into the procedure body.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="snippet">Is a snippet object.</param>
        public static InjectChainer Inject(this IAny prev, Snippet snippet)
        {
            return new InjectChainer((Chainer)prev, snippet);
        }
    }
}
