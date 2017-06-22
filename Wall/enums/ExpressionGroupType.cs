#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    internal enum ExpressionGroupType : int
    {
        None = 0,
        Begin = 1,
        End = 2,
        BeginEnd = 3
    }
}
