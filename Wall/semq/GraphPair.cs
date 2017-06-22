#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    // Represents the node pair (reference, foreign key) in graph relation.
    internal class GraphPair
    {
        // identifier of a foreign key node
        internal DB3 ForeignKeyID { get; private set; }

        // identifier of a reference node
        internal DB3 ReferenceID { get; private set; }

        // index of a foreign key node
        internal int ForeignKeyRX { get; private set; }

        // index of a reference node
        internal int ReferenceRX { get; private set; }

        internal GraphPair(DB3 foreignKeyID, DB3 referenceID, int foreignKeyRX, int referenceRX)
        {
            ForeignKeyID = foreignKeyID;
            ReferenceID = referenceID;
            ForeignKeyRX = foreignKeyRX;
            ReferenceRX = referenceRX;
        }
    }
}
