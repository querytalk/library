#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// Provides a foreign key of the relation. 
    /// </summary>
    public interface IRelation
    {
        /// <summary>
        /// Gets a foreign key.
        /// </summary>
        DB3 FK { get; set; }
    }

}
