#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    internal enum IdentifierValidity : int
    {
        // Identifier is null, empty or has more than 128 characters.
        InvalidLength = 0,

        // Identifier has invalid character set.
        InvalidChars = 1,

        // Identifier begins with @@ (reserved for global variabels) or ## (reserved for global temp tables).
        Reserved = 2,

        // Identifier is a regular identifier and begins with @.
        Variable = 3,

        // Identifier is a regular identifier and begins with #.
        TempTable = 4,

        // Identifier is a regular identifier and does not begin with @ or #.
        RegularIdentifier = 5
    }
}
