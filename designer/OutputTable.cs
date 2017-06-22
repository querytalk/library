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
        /// The Inserted table used in OUTPUT clause.
        /// </summary>
        public const OutputSource Inserted = OutputSource.Inserted;

        /// <summary>
        /// The Deleted table used in OUTPUT clause.
        /// </summary>
        public const OutputSource Deleted = OutputSource.Deleted;

    }
}