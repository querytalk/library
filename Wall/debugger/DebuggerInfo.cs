#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    internal static class DebuggerInfo
    {
        internal static string ToDebugString(Type type, string data)
        {
            return String.Format("{0} <{1}>", type, data);
        }
    }
}
