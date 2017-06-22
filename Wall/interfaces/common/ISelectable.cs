#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    // Used for interexchange the column value data in multi-value inserts.
    internal interface ISelectable
    {
        Column[] Columns { get; }

        int ColumnCount { get; }

        bool IsEmpty { get; }

        string[] Variables { set; }
    }
}
