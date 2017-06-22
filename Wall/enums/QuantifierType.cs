#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    internal enum QuantifierType : int
    {
        AtLeastOne = 0,
        None = 1,
        Many = 2,
        AtLeast = 3,
        AtMost = 4,
        MoreThan = 5,
        LessThan = 6,
        Exactly = 7,
        Between = 8,
        Most = 9
    }
}
