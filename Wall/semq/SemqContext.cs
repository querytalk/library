#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;

namespace QueryTalk.Wall
{
    /// <summary>
    /// A query context of the SEMQ. (Not intended for public use.)
    /// </summary>
    public sealed class SemqContext
    {
        // the most outer subject that starts the processing
        internal DbNode Subject { get; private set; }

        // the root subject of the query (the most outer subject of all nodes)
        //   It is given only for the internal node in order to preserve the link with the root subject inside the SQL querying processing
        //   in order to be able to control and recover all the nodes of a given query.
        internal DbNode RootSubject { get; private set; }

        internal IGraph RootGraph { get; private set; }

        internal EndChainer TranslatedGraph { get; set; }

        private List<GraphPair> _graphPairs = new List<GraphPair>();
        internal List<GraphPair> GraphPairs
        {
            get
            {
                return _graphPairs;
            }
        }

        internal Relation AddGraphPair(DB3 foreignKey, DbNode predecessorNode, DbNode relatedNode, Link link)
        {
            SetRX(relatedNode);

            // We need to replace foreignKey with the invoker's one.
            //   attention:
            //     This is needed because the default foreign key is the first column. Since it is allowed 
            //     that any column represents the foreign key, the user's foreign key must be replaced with 
            //     the default one in order to fit the graph invoker's default foreign key.
            DB3 invokerForeignKey;

            GraphPair pair;
            Relation relation;
            if (link.IsFKTable(predecessorNode.NodeID)) // => predecessor node is FK, related node is REFERENCE
            {
                invokerForeignKey = _GetInvokerForeignKey(link, foreignKey, out relation);
                pair = new GraphPair(invokerForeignKey, relatedNode.NodeID, predecessorNode.RX, relatedNode.RX);
            }
            else 
            {
                invokerForeignKey = _GetInvokerForeignKey(link, foreignKey, out relation);
                pair = new GraphPair(invokerForeignKey, predecessorNode.NodeID, relatedNode.RX, predecessorNode.RX);
            }

            _graphPairs.Add(pair);

            return relation;
        }

        private static DB3 _GetInvokerForeignKey(Link link, DB3 fkColumn, out Relation relation)
        {
            relation = link.TryGetRelation(fkColumn);
            return relation.ForeignKey;
        }

        // related index counter 
        private int _rx = 0;

        internal void SetRX(DbNode relatedNode)
        {
            _rx++;
            relatedNode.RX = _rx;
        }

        // a unique index of the current node in the SEMQ processing
        private int _index = 0;
        internal int GetNewIndex()
        {
            return ++_index;
        }

        // set the node's index
        internal void SetIndex(DbNode node)
        {
            // only assign new index if index is not already set
            if (node.Index == 0)
            {
                node.ChangeIndex(GetNewIndex());
            }
        }

        internal void SetGraphIndex(DbNode node)
        {
            if (node.Index == 0)
            {
                node.ChangeIndex(GetNewIndex());
            }
        }

        internal void AddNode(DbNode node)
        {
            if (!node.IsAdded)
            {
                if (RootSubject != null)
                {
                    RootSubject.Nodes.Add(node);
                }
                else
                {
                    Subject.Nodes.Add(node);
                }
            }

            node.IsAdded = true;
        }

        internal SemqContext(DbNode subject, DbNode rootSubject = null)
        {
            Subject = subject;
            RootSubject = rootSubject;
        }

        internal SemqContext(IGraph graph)
        {
            RootGraph = graph;
            Subject = graph.Subject; 
        }
    }
}
