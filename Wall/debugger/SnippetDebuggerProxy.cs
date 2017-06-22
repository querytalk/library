#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    internal class SnippetDebuggerProxy
    {
        public SnippetDebuggerProxy(Snippet snippet)
        {
            if (snippet == null)
            {
                throw new ArgumentNullException("snippet");
            }
        }
    }
}
