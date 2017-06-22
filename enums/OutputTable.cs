#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk
{
    /// <summary>
    /// Specifies the source table of an OUTPUT clause.
    /// </summary>
    public enum OutputSource : int
    {
        /// <summary>
        /// The Inserted table.
        /// </summary>
        Inserted = 0,

        /// <summary>
        /// The Deleted table.
        /// </summary>
        Deleted = 1
    }
}
