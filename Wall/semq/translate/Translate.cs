#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryTalk.Wall
{
    internal static partial class Translate
    {

        #region TranslateNode

        // General translation method for the database (table) node.
        internal static Chainer TranslateNode(SemqContext context, DbNode predecessor, DbNode node)
        {
            var currentNode = node.Root;

            if (context == null)
            {
                context = new SemqContext(currentNode);
            }

            context.AddNode(currentNode);
            currentNode.CheckReuseAndThrow();
            context.SetIndex(currentNode);

            Chainer query = Translate.CreateQueryFromSubject(currentNode);

            if (currentNode.HasExpression)
            {
                query.ExpressionGroupType = ExpressionGroupType.BeginEnd;  
                Translate.ProcessExpression(currentNode, null, ref query);
            }
            else
            {
                currentNode.SetAsUsed();
            }

            Translate.ConvertChainIntoPredicate(currentNode);
           
            if (((IPredicate)currentNode).HasPredicate)
            {
                Predicate prevPredicate = null;   
                var predicates = ((IPredicate)currentNode).Predicates;
                var pix = 0;  

                foreach (var predicate in predicates)
                {
                    SetExpressionGrouping(query, predicates, predicate, ref pix);

                    CheckHavingPredicateAndThrow(predicate, ref prevPredicate);

                    var innerNode = ((ISemantic)predicate).Subject;
                    if (innerNode != null)
                    {
                        context.AddNode(innerNode);

                        if (CheckSelfPredicate(currentNode, innerNode))
                        {
                            innerNode = innerNode.BreakChain();
                            predicate.Sentence = (ISemantic)innerNode;
                        }
                        else if (innerNode.Equals(currentNode) && innerNode.HasExpression)
                        {
                            // let the predicate subject have the same level as its caller
                            innerNode.ChangeIndex(currentNode.Index);

                            Translate.ProcessExpression(innerNode, predicate, ref query);

                            continue;
                        }

                        // get link
                        var link = DbMapping.TryFindPredicateLink(currentNode, innerNode);

                        // process intermediate node
                        DbNode intermediate = null;
                        if (link.HasIntermediate)
                        {
                            intermediate = Translate.ProcessIntermediate(context, predicate, innerNode, link);
                        }

                        // process a simple foreign key (if possible)
                        else if (Translate.ProcessSimpleForeignKey(currentNode, innerNode, predicate, link, ref query))
                        {
                            CheckQuantifierAndThrow(currentNode, innerNode, intermediate, predicate);
                            continue;
                        }

                        CheckQuantifierAndThrow(currentNode, innerNode, intermediate, predicate);

                        Translate.ProcessPredicate(context, predicate, ref query);

                    }
                    else
                    {
                        Translate.ProcessSelfPredicate(context, currentNode, predicate, ref query);
                    }
                }
            }

            if (predecessor != null)
            {
                Translate.Finalize(predecessor, currentNode, ref query);
            }

            if (((IPredicate)currentNode).PredicateGroupLevel != 0)
            {
                PredicateGroup.ThrowInvalidPredicateGroupingException((DbNode)currentNode);
            }

            return query;
        }

        private static bool CheckSelfPredicate(DbNode currentNode, DbNode innerNode)
        {
            if (!(innerNode.Equals(currentNode) && innerNode.Next != null))
            {
                return false;
            }

            return true;
        }

        private static void SetExpressionGrouping(Chainer query, List<Predicate> predicates, Predicate predicate, ref int pix)
        {
            if (predicate.PredicateType == PredicateType.Maximized)
            {
                return;
            }

            if (predicates == null)
            {
                query.ExpressionGroupType = ExpressionGroupType.BeginEnd;
                return;
            }

            // exclude maximized
            var predicatesWithoutMaximized = predicates.Where(p => p.PredicateType != PredicateType.Maximized).ToList();
            
            if (pix == 0)
            {
                query.ExpressionGroupType = (predicatesWithoutMaximized.Count == 1) ? ExpressionGroupType.BeginEnd : ExpressionGroupType.Begin;
            }
            else if (pix == predicatesWithoutMaximized.Count - 1)
            {
                query.ExpressionGroupType = ExpressionGroupType.End;
            }

            ++pix;
        }

        // check the position of .HavingXXX predicate
        private static void CheckHavingPredicateAndThrow(Predicate current, ref Predicate prev)
        {
            if (prev == null)
            {
                prev = current;
                return;
            }

            if (prev.PredicateType == PredicateType.Maximized)
            {
                throw new QueryTalkException("Translate.CheckHavingPredicateAndThrow", QueryTalkExceptionType.InvalidHavingPredicatePosition, 
                    null, Text.Method.HavingMaxMin);
            }

            prev = current;
        }

        internal static void CheckQuantifierAndThrow(DbNode currentNode, DbNode innerNode, DbNode intermediate, Predicate predicate)
        {
            if (currentNode == null || innerNode == null) 
            {
                return;  
            }

            Relation relation;

            if (intermediate != null)
            {
                relation = DbMapping.GetRelation(intermediate.NodeID, currentNode.SynonymOrThis.NodeID, intermediate.GetFK());
            }
            else
            {
                relation = DbMapping.GetRelation(innerNode.SynonymOrThis.NodeID, currentNode.SynonymOrThis.NodeID, innerNode.SynonymOrThis.GetFK());
            }

            // one-to-many is ok
            if (relation.IsRefTable(currentNode.SynonymOrThis.NodeID))
            {
                return;
            }

            // many-to-one is not ok if a predicate type is not existential
            if (!predicate.HasDefaultQuantifier())
            {
                ThrowInvalidQuantifierException(predicate, currentNode, (ISemantic)innerNode, intermediate, null);
            }
        }

        #endregion

        internal static Chainer CreateQueryFromSubject(DbNode currentNode)
        {
            if (currentNode is IFunction)
            {
                var arguments = ((IFunction)currentNode).Arguments;

                DbMapping.PrepareFunctionArguments(arguments, currentNode);

                return currentNode.GetDesigner()
                    .From(currentNode.Map.Name.PassUdf(
                        ((IFunction)currentNode).Arguments), currentNode)
                    .As(currentNode.Index);
            }
            // table
            else
            {
                if (currentNode.IsSynonym)
                {
                    if (currentNode.SynonymQuery is IOpenView)
                    {
                        return currentNode.GetDesigner().From((IOpenView)currentNode.SynonymQuery, currentNode)
                            .As(currentNode.Index);
                    }
                    // View
                    else
                    {
                        return currentNode.GetDesigner().From((View)currentNode.SynonymQuery, currentNode)
                            .As(currentNode.Index);
                    }
                }
                else
                {
                    if (currentNode.Row != null)
                    {
                        var row = currentNode.Row;
                        return currentNode.GetDesigner()
                            .From(currentNode.Map.Name, currentNode).As(currentNode.Index)
                            .Where(DbMapping.GetNodeMap(row.NodeID)
                                .BuildRKExpression(row.GetOriginalRKValues(), currentNode.Index));
                    }
                    else
                    {
                        return currentNode.GetDesigner()
                            .From(currentNode.Map.Name, currentNode)
                            .As(currentNode.Index);
                    }
                }
            }
        }

        internal static void ProcessExpression(DbNode currentNode, Predicate predicate, ref Chainer query)
        {
            bool sign = true;
            var logicalOperator = LogicalOperator.And;

            if (predicate != null)
            {
                sign = predicate.Sign;
                logicalOperator = predicate.LogicalOperator;

                // check predicate type - no quantifiers allowed - !
                if (!predicate.HasDefaultQuantifier())
                {
                    ThrowInvalidQuantifierException(predicate, currentNode, null, null, null);
                }

            }
            // else subject expression (S.a) with positive sign

            if (query is ConditionChainer)
            {
                if (logicalOperator == LogicalOperator.And)
                {
                    query = ((ConditionChainer)query)
                        .AndWhere(currentNode.Expression, sign, predicate != null ? predicate.PredicateGroup : null);
                }
                else
                {
                    query = ((ConditionChainer)query)
                        .OrWhere(currentNode.Expression, sign, predicate != null ? predicate.PredicateGroup : null);
                }
            }
            else
            {
                query = ((IWhere)query)
                    .Where(currentNode.Expression, sign, predicate != null ? predicate.PredicateGroup : null);
            }
        }

        // Process a node chain S.S2...Sn by placing the consecutive node S2 into the predicate of a subject S: 
        //     S.S2 => S.P(S2)
        internal static void ConvertChainIntoPredicate(DbNode currentNode)
        {
            if (currentNode.Next == null)
            {
                return;
            }

            var next = currentNode.BreakChain();

            var predicate = new Predicate(currentNode, PredicateType.Existential, true, LogicalOperator.And, (ISemantic)next, null, true);
            ((IPredicate)currentNode).Predicates.Add(predicate);
        }

        internal static DbNode ProcessIntermediate(SemqContext context, Predicate predicate, DbNode innerNode, Link link)
        {
            var intermediate = new InternalNode(link.Intermediate);

            context.AddNode(intermediate);
            intermediate.IsFinalizeable = predicate.IsFinalizeable;

            if (predicate.PredicateType == PredicateType.Quantified
                || predicate.PredicateType == PredicateType.TopQuantified)
            {
                ((IPredicate)intermediate).AddPredicate(PredicateType.Cartesian, innerNode);
            }
            else
            {
                ((IPredicate)intermediate).AddPredicate(PredicateType.Existential, innerNode);
            }

            predicate.Sentence = (ISemantic)intermediate;

            return intermediate;
        }

        // When a subject S2 of a predicate is a single node without the chaining and the predicates (+expression)
        // and it represents a simple foreign key (with a single column) then it is translated as an expression 
        // where FK attribute is checked by NOT NULL operator.
        //   note: 
        //     If a FK is NOT NULLABLE then the .Where method is omitted.
        //   returns: 
        //     True if a simple foreign key has been processed.
        internal static bool ProcessSimpleForeignKey(
            DbNode currentNode, 
            DbNode innerNode, 
            Predicate predicate, 
            Link link,
            ref Chainer query)
        {
            if (!(
                innerNode.IsLast                            // subject is a single non-chained node
                && !((IPredicate)innerNode).HasPredicate    // without a predicate
                && !innerNode.HasExpression                 // without expression      
            ))               
            {
                return false;
            }

            var foreignKey = innerNode.GetFK();
            var relation = link.TryGetRelation(foreignKey);

            // is relation many-to-one?
            if (relation.IsRefTable(currentNode.SynonymOrThis.NodeID))
            {
                return false;
            }

            // is simple foreign key?
            if (relation.FKColumns.Count() > 1)
            {
                return false;
            }

            // assure that foreign key column is given
            if (foreignKey.IsDefault)
            {
                foreignKey = relation.ForeignKey;
            }

            var fk = DbMapping.GetColumnMap(foreignKey);
            var column = String.Format("{0}.{1}", predicate.Subject.Index, fk.Name.Part1);

            // prepare expression
            Expression expression = column.IsNotNull();

            if (!fk.IsNullable)
            {
                expression = "1=1";
            }

            if (query is ConditionChainer)
            {
                if (predicate.LogicalOperator == LogicalOperator.Or)
                {
                    query = ((ConditionChainer)query)
                        .OrWhere(expression, predicate.Sign, predicate.PredicateGroup);
                }
                else
                {
                    query = ((ConditionChainer)query)
                        .AndWhere(expression, predicate.Sign, predicate.PredicateGroup);
                }
            }
            else
            {
                query = ((IWhere)query)
                    .Where(expression, predicate.Sign, predicate.PredicateGroup);
            }

            return true;
        }

        internal static void ProcessSelfPredicate(SemqContext context, DbNode currentNode, Predicate predicate, ref Chainer query)
        {
            if (predicate.PredicateType == PredicateType.Maximized)
            {
                ProcessMaximized(context, predicate, ref query);
            }
            else
            {
                ProcessExpressionSQL(currentNode, predicate, ref query);
            }
        }

        // processes SQL expression
        internal static void ProcessExpressionSQL(DbNode currentNode, Predicate predicate, ref Chainer query)
        {
            if (!predicate.HasDefaultQuantifier())
            {
                ThrowInvalidQuantifierException(predicate, currentNode, null, null, null);
            }

            if (query is ConditionChainer)
            {
                if (predicate.LogicalOperator == LogicalOperator.Or)
                {
                    query = ((ConditionChainer)query)
                        .OrWhere(predicate.Expression, predicate.Sign, predicate.PredicateGroup);
                }
                else
                {
                    query = ((ConditionChainer)query)
                        .AndWhere(predicate.Expression, predicate.Sign, predicate.PredicateGroup);
                }
            }
            else
            {
                query = ((IWhere)query)
                    .Where(predicate.Expression, predicate.Sign, predicate.PredicateGroup);
            }
        }

        #region Process Predicate

        internal static void ProcessPredicate(SemqContext context, Predicate predicate, ref Chainer query)
        {
            switch (predicate.PredicateType)
            {
                case PredicateType.Existential:
                    ProcessExistential(context, predicate, ref query);
                    break;

                case PredicateType.Quantified:
                    ProcessQuantified(context, predicate, ref query);
                    break;

                case PredicateType.TopQuantified:
                    ProcessTopQuantified(context, predicate, ref query);
                    break;

                case PredicateType.Maximized:
                    ProcessMaximized(context, predicate, ref query);
                    break;
            }
        }

        private static INonSelectView TranslatePredicate(SemqContext context, Predicate predicate)
        {
            return (INonSelectView)((ISemantic)predicate).Translate(context, predicate.Subject);
        }

        #endregion

        #region Existential

        private static void ProcessExistential(SemqContext context, Predicate predicate, ref Chainer query)
        {
            var body = TranslatePredicate(context, predicate);

            if (query is IWhere)
            {
                query = ((IWhere)query).WhereExists(body, predicate.Sign, predicate.PredicateGroup);
            }
            else
            {
                query = ((ConditionChainer)query).WhereExists(body, predicate.Sign, predicate);
            }
        }

        #endregion

        #region Quantified

        private static void ProcessQuantified(SemqContext context, Predicate predicate, ref Chainer query)
        {
            var body = TranslatePredicate(context, predicate);

            if (query is IWhere)
            {
                query = ((IWhere)query).WhereQuantified(body, predicate.Sign, predicate);
            }
            else
            {
                query = ((ConditionChainer)query).WhereQuantified(body, predicate.Sign, predicate);
            }
        }

        #endregion

        #region TopQuantified

        private static void ProcessTopQuantified(SemqContext context, Predicate predicate, ref Chainer query)
        {
            // manually create new table indexes
            var ix1 = context.GetNewIndex();
            var ix2 = context.GetNewIndex();
            var ix3 = context.GetNewIndex();
            var ix4 = context.GetNewIndex();

            // get relation between the subject of the predicate and its inner node
            var subject = predicate.Subject;    
            var self = subject.SynonymOrThis.Map.Name;
            var innerNode = ((ISemantic)predicate).Subject;
            var link = DbMapping.TryFindPredicateLink(subject, innerNode);

            var foreignKey = innerNode.GetFK();
            var relation = link.TryGetRelation(foreignKey);
            var relationExpression = relation.BuildRelation(subject.NodeID, ix1, ix2).E();
            var count = Designer.Identifier(ix4.ToString(), Text.Reserved.CountColumnReserved);

            // get columns of inner table A1 
            List<Column> columnsA1 = new List<Column>();
            columnsA1.AddRange(subject.SynonymOrThis.Map.GetRKColumns(ix4));
            columnsA1.Add(count);

            // get columns of inner table A2 
            List<Column> columnsA2 = new List<Column>();
            columnsA2.AddRange(subject.SynonymOrThis.Map.GetRKColumns(ix1));
            columnsA2.Add(Designer.CountBig().As(Text.Reserved.CountColumnReserved));

            // build self-relation of a Subject
            var selfRelation = subject.SynonymOrThis.Map.BuildSelfRelation(subject.Index, ix3);

            // translate predicate (without finalization)
            // attention:
            //   Do not move this line upper, before tableB assignment because after the translation 
            //   the node chain gets split and tableB is not assigned the correct node if the predicate 
            //   is translated before the assignment takes place.
            var translatedPredicate = ((ISelect)TranslatePredicate(context, predicate));

            // get columns of inner table B2 (subject of a predicate)
            List<Column> columnsB2 = new List<Column>();
            
            if (relation.IsRefTable(innerNode.NodeID))
            {
                columnsB2.AddRange(innerNode.Map.GetRKColumns(innerNode.Index));
            }
            else
            {
                columnsB2.AddRange(relation.GetFK(innerNode.Index));
            }

            // create TopQuantified body
            var body = Designer.GetNewDesigner()
                .From(Designer.GetNewDesigner()
                    .From(Designer.GetNewDesigner()
                        .From(self).As(ix1)
                        .InnerJoin(translatedPredicate.Select(columnsB2.ToArray())).As(ix2)  // diff
                        .On(relationExpression)
                        .GroupBy(subject.SynonymOrThis.Map.GetGroupBy(ix1))
                        .Select(columnsA2.ToArray())).As(ix4)
                    .OrderBy(count.AsDesc())
                    .Select(columnsA1.ToArray()).TopWithTies(predicate.Quantifier.Cardinality)
                , subject).As(ix3)
                .Where(selfRelation, predicate.Sign, null);

            if (query is IWhere)
            {
                query = ((IWhere)query).WhereExists(body, predicate.Sign, predicate.PredicateGroup);
            }
            else
            {
                query = ((ConditionChainer)query).WhereExists(body, predicate.Sign, predicate);
            }
        }

        #endregion

        #region Maximized

        private static void ProcessMaximized(SemqContext context, Predicate predicate, ref Chainer query)
        {
            var ix1 = context.GetNewIndex();
            var ix2 = context.GetNewIndex();

            var self = predicate.Subject;
            context.SetIndex(self);   

            var maximizedExpression = predicate.MaximizedExpression;

            var ordered = predicate.MaximizedType == MaximizationType.Min ?
                new OrderedChainer(maximizedExpression, SortOrder.Asc) :
                new OrderedChainer(maximizedExpression, SortOrder.Desc);

            var root = new InternalNode(self);

            query = root.GetDesigner()
                .From(Designer.GetNewDesigner()
                    .From(((ISelect)query).Select(), root).As(ix1)
                    .OrderBy(ordered)
                    .Select().TopWithTies(predicate.MaximizedTop)
                    , root
                ).As(ix2);

            self.ChangeIndex(ix2);
        }

        #endregion

        #region Finalizer

        internal static void Finalize(DbNode predecessor, DbNode currentNode, ref Chainer query)
        {
            if (!currentNode.IsFinalizeable)
            {
                return;
            }

            predecessor = predecessor.SetSynonymOrThis();
            currentNode = currentNode.SetSynonymOrThis();

            var foreignKey = currentNode.GetFK();
            var relation = DbMapping.GetRelation(currentNode.NodeID, predecessor.NodeID, foreignKey);
            var relationExpression = relation.BuildRelation(currentNode.NodeID, currentNode.Index, predecessor.Index).E();

            if (query is ConditionChainer)
            {
                query = ((ConditionChainer)query).AndWhere(relationExpression, true);
            }
            else
            {
                query = ((IWhere)query).Where(relationExpression, true);
            }
        }

        #endregion

        #region Join

        // Applies the joining of the nodes along the joinable node chain.
        //   note:
        //     Joining can be applied only on pure nodes without the predicates.
        //     Otherwise the predicates could contain the joinable node chains
        //     which would lead into the mixture of semantical and SQL query language.
        internal static Chainer Join(DbNode rootNode)
        {
            Chainer query = null;
            var node = rootNode;
            int ix = 1;  // index of a table in joining

            // move along the graph
            while (node != null)
            {
                // check for predicates
                if (((IPredicate)node).HasPredicate)
                {
                    node.Mapper.Throw(QueryTalkExceptionType.InvalidJoin, null, Text.Method.NodeChainJoin);
                }

                // check if node has a FK
                var fk = ((IRelation)node).FK;

                // isolate node & get next node
                var next = node.BreakChain();

                // .From
                if (query == null)
                {
                    query = Designer.GetNewDesigner(rootNode.Name, true, true)
                        .From((IOpenView)node).As(ix);
                }
                // .InnerJoin
                else
                {
                    // add .By for FK
                    if (!fk.IsDefault)
                    {
                        var fkColumn = new DbColumn(new InternalNode(fk.TableID), fk);
                        query = ((IInnerJoin)query).InnerJoin((IOpenView)node).As(ix).By(fkColumn.Of(ix-1));
                    }
                    else
                    {
                        query = ((IInnerJoin)query).InnerJoin((IOpenView)node);
                    }
                }

                node = next;
                ++ix;
            };

            return query;
        }

        #endregion

        #region InvalidQuantifierException

        internal static void ThrowInvalidQuantifierException(
            Predicate predicate, 
            DbNode subject, 
            ISemantic predicateSentence, 
            DbNode intermediate, 
            string predicateExpression)
        {
            DbNode predicateNode = null;
            if (predicateSentence != null)
            {
                predicateNode = (predicateSentence.Subject != null) ? predicateSentence.Subject : null;
            }

            if (intermediate != null)
            {
                throw new QueryTalkException("Translate.ThrowInvalidQuantifierException", QueryTalkExceptionType.QuantifierDisallowed,
                    String.Format("subject = {0}{1}   predicate node (intermediate) = {5}{1}   given predicate node = {2}{1}   predicate expression = {3}{1}   quantifier = {4}",
                        subject != null ? DbMapping.GetNodeName(subject.NodeID).Sql : Text.NotAvailable,
                        Environment.NewLine,
                        predicateNode != null ? DbMapping.GetNodeName(predicateNode.NodeID).Sql : Text.NotAvailable,
                        predicateExpression != null ? predicateExpression : Text.NotAvailable,
                        predicate.Quantifier != null ? predicate.Quantifier.QuantifierType.ToString() : Text.NotAvailable,
                        intermediate.Name),
                    Text.Method.PredicateMethod).SetExtra("Pay attention to the intermediate predicate node which was provided by the library since the subject and the given predicate node are not related directly to each other.");
            }
            else
            {
                throw new QueryTalkException("Translate.ThrowInvalidQuantifierException", QueryTalkExceptionType.QuantifierDisallowed,
                    String.Format("subject = {0}{1}   predicate node = {2}{1}   predicate expression = {3}{1}   quantifier = {4}",
                        subject != null ? DbMapping.GetNodeName(subject.NodeID).Sql : Text.NotAvailable,
                        Environment.NewLine,
                        predicateNode != null ? DbMapping.GetNodeName(predicateNode.NodeID).Sql : Text.NotAvailable,
                        predicateExpression != null ? predicateExpression : Text.NotAvailable,
                        predicate.Quantifier != null ? predicate.Quantifier.QuantifierType.ToString() : Text.NotAvailable),
                    Text.Method.PredicateMethod);
            }
        }

        #endregion

    }
}
