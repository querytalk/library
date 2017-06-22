#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// Specifies the type of a database column.
    /// </summary>
    public enum ColumnType : int
    {
        /// <summary>
        /// A regular column.
        /// </summary>
        Regular = 0,

        /// <summary>
        /// An identity column.
        /// </summary>
        Identity = 1,

        /// <summary>
        /// A timestamp (rowversion) column.
        /// </summary>
        Rowversion = 2,

        /// <summary>
        /// A computed column.
        /// </summary>
        Computed = 3
    }
}
