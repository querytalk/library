#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {
        /// <summary>
        /// Represents all column of the specified node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node that contains the columns.</param>
        public static DbColumns Columns<TNode>(this DbTable<TNode> node)
            where TNode : DbRow
        {
            if (node == null)
            {
                throw new QueryTalkException("Extensions", QueryTalkExceptionType.ArgumentNull,
                    "node = null", "Extensions.Columns");
            }

            return new DbColumns(node, node.NodeID);
        }

        /// <summary>
        /// Represents all column of the specified node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node that contains the columns.</param>
        /// <param name="alias">The alias of the columns.</param>
        public static DbColumns Columns<TNode>(this DbTable<TNode> node, int alias)
            where TNode : DbRow
        {
            if (node == null)
            {
                throw new QueryTalkException("Extensions", QueryTalkExceptionType.ArgumentNull,
                    "node = null", "Extensions.Columns");
            }

            return new DbColumns(node, node.NodeID, alias.ToString());
        }

        /// <summary>
        /// Represents all column of the specified node.
        /// </summary>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        /// <param name="node">The node that contains the columns.</param>
        /// <param name="alias">The alias of the columns.</param>
        public static DbColumns Columns<TNode>(this DbTable<TNode> node, string alias)
            where TNode : DbRow
        {
            if (node == null)
            {
                throw new QueryTalkException("Extensions", QueryTalkExceptionType.ArgumentNull,
                    "node = null", "Extensions.Columns");
            }

            return new DbColumns(node, node.NodeID, alias);
        }
    }
}
