#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk
{
    /// <summary>
    /// Status of a DbRow object.
    /// </summary>
    public enum DbRowStatus : int
    {
        /// <summary>
        /// A new row object has been created by the client.
        /// </summary>
        New = 0,

        /// <summary>
        /// A row object has been loaded from the database or successfully committed and loaded from the database.
        /// </summary>
        Loaded = 1,

        /// <summary>
        /// A row object with Loaded status has been modified but not yet committed in the database.
        /// </summary>
        Modified = 2,

        /// <summary>
        /// A row object has been deleted in the database.
        /// </summary>
        Deleted = 3,

        /// <summary>
        /// A row object has been successfully committed in the database, but not successfully loaded.
        /// </summary>
        Faulted = 4,
    }
}
