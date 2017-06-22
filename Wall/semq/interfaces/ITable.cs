#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Provides the mapping data of all child nodes.
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// Loads the children mapping data.
        /// </summary>
        /// <param name="children">A children collection</param>
        /// <param name="level">The current level.</param>
        /// <param name="maxLevels">Max levels allowed.</param>
        void LoadChildren(ref List<NodeTree> children, int level, int maxLevels);  
    }
}
