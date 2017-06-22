#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// Provides a unique row identifier.
    /// </summary>
    public interface IRow
    {
        /// <summary>
        /// Gets a unique row identifier.
        /// </summary>
        long RowID { get; set; }
    }
}
