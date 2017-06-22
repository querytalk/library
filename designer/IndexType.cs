#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {
        /// <summary>
        /// Types of the SQL Server indexes.
        /// </summary>
        public enum IndexType : int
        {
            /// <summary>
            /// An index that specifies the logical ordering of a table. With a nonclustered index, the physical order of the data rows is independent of their indexed order.
            /// </summary>
            Nonclustered = 0,

            /// <summary>
            /// An index in which the logical order of the key values determines the physical order of the corresponding rows in a table.
            /// </summary>
            Clustered = 1,

            /// <summary>
            /// A unique nonclustered index.
            /// </summary>
            UniqueNonclustered = 2,

            /// <summary>
            /// A unique clustered index.
            /// </summary>
            UniqueClustered = 3,
        }
    }
}