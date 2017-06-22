#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class NodeTree
    {
        internal DB3 Parent { get; private set; }

        internal DB3 Child { get; private set; }

        internal int Level { get; private set; }

        /// <summary>
        /// Initializes a new instance of the NodeTree class.
        /// </summary>
        /// <param name="parent">Is a DB3 identifier of a parent.</param>
        /// <param name="child">Is a DB3 identifier of a child.</param>
        /// <param name="level">A tree level.</param>
        /// <param name="maxLevels">level should not exceed maxLevels.</param>
        public NodeTree(DB3 parent, DB3 child, int level, int maxLevels)
        {
            if (maxLevels <= level)
            {
                throw new QueryTalkException("NodeTree.ctor", QueryTalkExceptionType.MaxLevelsExceeded,
                    String.Format("maxLevels = {0}", maxLevels), Text.Method.DeleteCascadeGo); 
            }

            Parent = parent;
            Child = child;
            Level = level;
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("Parent: {0}, Child: {1}, Level: {2}", Parent, Child, Level);
        }
    }
}
