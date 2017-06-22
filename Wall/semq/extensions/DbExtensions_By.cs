#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {
        /// <summary>
        /// Specifies the foreign key that is to be used in relation. 
        /// The foreign key should be explicitly given when there is more than one relation between two tables.
        /// </summary>
        /// <typeparam name="T">The type of the node.</typeparam>
        /// <param name="node">The node to which belongs the foreign key.</param>
        /// <param name="column">The column that is part of the foreign key.</param>
        public static DbTable<T> By<T>(this DbTable<T> node, string column)
            where T : DbRow
        {
            // null check
            if (node == null)
            {
                throw new QueryTalkException(".By", QueryTalkExceptionType.ArgumentNull, "node", Text.Method.By);
            }
            if (column == null)
            {
                throw new QueryTalkException(".By", QueryTalkExceptionType.ArgumentNull, "column", Text.Method.By);
            }

            var nodeMap = DbMapping.GetNodeMap(node.NodeID);
            ColumnMap columnMap = null;
            foreach (var col in nodeMap.Columns)
            {
                if (Api.IsEqual(col.Name.Part1, column))
                {
                    columnMap = col;
                    break;
                }
            }

            if (columnMap == null)
            {
                throw new QueryTalkException(".By", QueryTalkExceptionType.ColumnNotFound,
                    String.Format("node = {0}{1}   column = {2}", node.Name, Environment.NewLine, column), Text.Method.By)
                    .SetObjectName(node.Name);
            }

            ((IRelation)node).FK = columnMap.ID;

            return node;
        }

    }
}
