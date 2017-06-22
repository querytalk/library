#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Text;

namespace QueryTalk.Wall
{
    internal static partial class Translate
    {
        internal static void TranslateGraph(IGraph graph, SemqContext context, IGraph predecessor)
        {
            if (predecessor == null)
            {
                Translate.TranslateGraphNode(graph, context, null, context.Subject);
            }

            while (graph != null)
            {
                ((IGraph)graph).Translate(context, predecessor);

                graph = graph.Next;
            }
        }

        internal static void TranslateGraphNode(IGraph graph, SemqContext context, DbNode predecessor, DbNode node, bool isLeafNode = false)
        {
            var isSubject = (predecessor == null);

            node = node.Root;
            context.SetGraphIndex(node);

            Chainer translatedNode;
            Designer root;
            if (isSubject)
            {
                root = new InternalRoot(node);
            }
            else
            {
                root = context.TranslatedGraph.GetRoot();
                node.IsFinalizeable = false;   
            }

            // translate node only
            //   important:
            //     The node should be translated before the graph processing begins in order that to obtain the appropriate index.
            translatedNode = ((ISemantic)node).Translate(context, predecessor);

            StringBuilder nodeTableVar = null;

            // create table variable storage except for the last leaf node which does not need a table variable
            if (!isLeafNode)
            {
                TranslateGraphDeclareTable(node, root, out nodeTableVar);
            }

            // finish the node query translation as part of a graph 
            if (isSubject)
            {
                TranslateGraphSubject(context, node, root, translatedNode, nodeTableVar);
            }
            else
            {
                TranslateGraphRelated(graph, context, predecessor, node, translatedNode, nodeTableVar);
            }
        }

        private static void TranslateGraphDeclareTable(DbNode node, Designer root, out StringBuilder nodeTableVar)
        {
            var nodeTableVarName = GetTableVarName(node.Index);

            nodeTableVar = Text.GenerateSql(200)
                .Append(Text.Declare).S()
                .Append(nodeTableVarName).Append(Text._As_).Append(Text.Table).S()
                .Append(Text.LeftBracket);

            // add columns of all keys
            //   note: 
            //     We add all keys in order to assure that all join or where relations will be properly built.
            bool first = true;
            foreach (var column in node.Map.GetKeys())
            {
                var nullableDef = column.IsNullable ? Text.Null : Text.NotNull;

                if (!first)
                {
                    nodeTableVar.AppendComma().S();
                }

                nodeTableVar.Append(column.Name.Sql).S().Append(column.DataType.Build()).S().Append(nullableDef);
                first = false;
            }

            nodeTableVar.Append(Text.RightBracket)
                .Terminate();

            root.TryAddVariableOrThrow(new Variable(new DataType(DT.TableVariable), nodeTableVarName), Text.Method.DesignTable, false);
        }

        private static void TranslateGraphSubject(SemqContext context, DbNode node, Designer root,
            Chainer translatedNode, StringBuilder nodeTableVar)
        {
            var ix1 = node.Index;
            var ix2 = context.GetNewIndex();

            var columnArguments = node.Map.GetKeyColumns(node.Index);

            var nodeTableVarName = GetTableVarName(ix1);

            var subjectQuery = ((ISelect)translatedNode)
                .Select(columnArguments) 
                .Insert(nodeTableVarName)
                .EndSnipInternal();

            context.TranslatedGraph = root.GetDesigner()
                .Inject(nodeTableVar.ToString())
                .Inject(subjectQuery)
                .From(node.Map.Name).As(ix1)
                .InnerJoin(nodeTableVarName).As(ix2).On(node.Map.BuildSelfRelation(ix1, ix2))
                .Select(node.Map.GetColumns(ix1))
                .End();
        }

        // translate related table
        private static void TranslateGraphRelated(IGraph graph, SemqContext context, DbNode predecessor, DbNode related,
            Chainer translatedNode, StringBuilder relatedTableVar)
        {
            // try find link
            var link = DbMapping.TryFindPredicateLink(predecessor, related.Root, Text.Method.With);
            if (link.HasIntermediate)
            {
                ThrowWithRelatedNotFound(graph, related.Root);
            }

            // get FK
            var foreignKey = related.GetFK();                          
            foreignKey = foreignKey.Coalesce(link.DefaultForeignKey);   

            // set RX
            var relation = context.AddGraphPair(foreignKey, predecessor, related, link);

            // add comment
            context.TranslatedGraph.Comment(String.Format("{0} {1} of {2} ({3}->{4})",
                Text.Free.GraphComment, related.Name, predecessor.Name, predecessor.RX, related.RX));
   
            var predecessorTableVarName = GetTableVarName(predecessor.Index);
            var relatedTableVarName = GetTableVarName(related.Index);
            var keyColumns = related.Map.GetKeyColumns(related.Index);
            var ix1 = context.GetNewIndex();
            var ix2 = context.GetNewIndex();
            var ix3 = context.GetNewIndex();

            // append the predicate
            if (translatedNode is ConditionChainer)
            {
                translatedNode = ((IWhereAnd)translatedNode)
                    .AndExists(new InternalRoot(related).GetDesigner()
                        .From(predecessorTableVarName).As(ix1)
                        .Where(relation.BuildRelation(related.NodeID, related.Index, ix1)));
            }
            else
            {
                translatedNode = ((IWhere)translatedNode)
                    .WhereExists(new InternalRoot(related).GetDesigner()
                        .From(predecessorTableVarName).As(ix1)
                        .Where(relation.BuildRelation(related.NodeID, related.Index, ix1)));
            }

            // inject related table query into the subject query
            if (relatedTableVar != null)
            {
                context.TranslatedGraph
                    .Inject(relatedTableVar.ToString())
                    .Inject((((ISelect)translatedNode)
                        .Select(keyColumns)
                        .Insert(GetTableVarName(related.Index)))
                        .EndSnipInternal())
                    .From(related.Map.Name).As(ix2)
                    .InnerJoin(relatedTableVarName).As(ix3).On(related.Map.BuildSelfRelation(ix2, ix3))
                    .Select(related.Map.GetColumns(ix2));
            }
            // leaf nodes:
            else
            {
                var allColumns = related.Map.GetColumns(related.Index);
                context.TranslatedGraph
                    .Inject((((ISelect)translatedNode).Select(allColumns)).EndSnipInternal());
            }
        }

        private static string GetTableVarName(int index)
        {
            return String.Format("{0}{1}", Text.Free.GraphTablePrefix, index);
        }

        private static void ThrowWithRelatedNotFound(IGraph graph, DbNode node)
        {
            throw new QueryTalkException("With.ctor", QueryTalkExceptionType.WithRelatedNotFound,
                String.Format("related = {0}", node.Name), Text.Method.With).SetObjectName(graph.Subject.Name);
        }

    }

}
