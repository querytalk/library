#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    internal class ValueDebuggerProxy
    {
        public ValueDebuggerProxy(Value value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
        }
    }
}
