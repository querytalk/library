#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;

namespace QueryTalk.Wall
{
    internal class Joiner
    {

        #region Fields

        private QueryPart _queryPart;
        private BuildContext _buildContext;
        private BuildArgs _buildArgs;

        private List<TableChainer> _tables
        {
            get
            {
                return _queryPart.Tables;
            }
        }

        // collection of all target tables for auto-joining
        private List<TableChainer> _targets;

        private TableChainer _lastJoinTable;

        #endregion

        #region Ctor

        internal Joiner(QueryPart queryPart, BuildContext buildContext, BuildArgs buildArgs)
        {
            _queryPart = queryPart;
            _buildContext = buildContext;
            _buildArgs = buildArgs;
            _targets = new List<TableChainer>();
        }

        #endregion

        #region Join

        internal void ProcessJoin()
        {
            if (_tables.Count < 2) 
            {
                if (_tables.Count == 1)
                {
                    _lastJoinTable = _tables[0];
                }
                return; 
            }

            for (int i = 1; i < _tables.Count; ++i)
            {
                _targets.Add(_tables[i - 1]);

                if (!(_tables[i] is JoinChainer))
                {
                    continue;
                }

                var source = (JoinChainer)_tables[i];

                if (source.Correlation == JoinCorrelation.On)
                {
                    continue;
                }
                else if (source.Correlation == JoinCorrelation.By)
                {
                    ByJoin(source);
                }
                else
                {
                    AutoJoin(source);
                }

                _lastJoinTable = source;
            }
        }

        #region ByJoin

        private void ByJoin(JoinChainer source)
        {
            Expression expression = null;
            Expression part = null;

            var i = 0;
            foreach (var arg in source.ByArguments)
            {

                switch (arg.JoinType)
                {
                    case ByType.Alias:
                        part = AliasByJoin(source, arg);
                        break;

                    case ByType.NonMappedColumn:
                        part = NonMappedByJoin(source, arg);
                        break;

                    case ByType.MappedColumn:
                        part = MappedByJoin(source, arg);
                        break;
                }

                if (i == 0)
                {
                    expression = part;
                }
                else
                {
                    expression = expression.And(part);
                }
            
                ++i;
            }

            _InjectOn(source, expression);
        }

        private Expression NonMappedByJoin(JoinChainer source, ByArgument arg)
        {
            _GetTargetByAlias(source, arg.Alias, false);
            return _GetNonMappedExpression(arg, source.Alias.Name);
        }

        private Expression MappedByJoin(JoinChainer source, ByArgument arg)
        {
            var target = _GetTargetByAlias(source, arg.Alias, true);

            if (!source.HasNode || !target.HasNode)
            {
                throw JoinerException(source, target, QueryTalkExceptionType.InvalidJoinTable);
            }

            var expression = _GetMappedExpression(source, target, arg.DbColumn.ColumnID);

            source.AppendJoinGraph(target.JoinGraph);

            return expression;
        }

        private Expression AliasByJoin(JoinChainer source, ByArgument arg)
        {
            var target = _GetTargetByAlias(source, arg.Alias, true);

            if (!source.HasNode || !target.HasNode)
            {
                throw JoinerException(source, target, QueryTalkExceptionType.InvalidJoinTable);
            }

            var expression = _GetMappedExpression(source, target, DB3.Default);

            source.AppendJoinGraph(target.JoinGraph);

            return expression;
        }

        #endregion

        #region AutoJoin

        private void AutoJoin(JoinChainer source)
        {
            if (!source.HasNode)
            {
                throw JoinerException(source, (TableChainer)null, QueryTalkExceptionType.InvalidJoinTable);
            }

            bool found = false;
            TableChainer target = null;
            var ix = _targets.Count - 1;
            while (ix >= 0)
            {
                target = _targets[ix];
                if (CheckAutoTarget(source, target))
                {
                    found = true;
                    break;
                }
                --ix;
            }

            if (!found)
            {
                throw JoinerAutoJoinFailedException(source);
            }

            var expression = _GetMappedExpression(source, target, DB3.Default);
            _InjectOn(source, expression);
            source.AppendJoinGraph(target.JoinGraph);
        }

        private static bool CheckAutoTarget(TableChainer source, TableChainer target)
        {
            // skip the non-mapped tables
            if (!target.HasNode)
            {
                return false;
            }

            var link = DbMapping.GetLink(source.Node.SynonymOrThis.NodeID, target.Node.SynonymOrThis.NodeID);
            if (link == null 
                || !link.Intermediate.IsDefault)    // intermediate links are not allowed
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Supporting Methods

        private TableChainer _GetTargetByAlias(JoinChainer source, string alias, bool hasNodeCheck)
        {
            foreach (var target in _targets)
            {
                if (target.Alias.Name.EqualsCS(alias))
                {
                    if (hasNodeCheck && !target.HasNode)
                    {
                        throw JoinerException(source, alias, QueryTalkExceptionType.JoinTargetNotMapped);
                    }

                    return target;  
                }
            }

            throw JoinerException(source, alias, QueryTalkExceptionType.JoinTargetNotFound);
        }

        private static Expression _GetNonMappedExpression(ByArgument byArg, string sourceAlias)
        {
            return String.Format("{0}.{1} = {2}.{3}",
                Filter.Delimit(sourceAlias), Filter.Delimit(byArg.Column),
                Filter.Delimit(byArg.Alias), Filter.Delimit(byArg.Column)).E();
        }

        private static Expression _GetMappedExpression(TableChainer source, TableChainer target, DB3 foreignKey)
        {
            if (foreignKey.IsDefault)
            {
                foreignKey = source.Node.GetFK();
            }

            var joinRelation = DbMapping.GetRelation(target.Node.Root.SynonymOrThis.NodeID, source.Node.Root.SynonymOrThis.NodeID,
                foreignKey, Text.Method.By);

            if (foreignKey.IsDefault)
            {
                foreignKey = joinRelation.ForeignKey;
            }

            // determine join operator (for .Join method only)
            if (source is SemqJoinChainer && !joinRelation.IsInnerJoin(target.Node.Root.SynonymOrThis.NodeID))
            {
                ((SemqJoinChainer)source).IsLeftOuterJoin = true;
            }

            // foreign key table = this
            if (foreignKey.TableID.Equals(source.Node.NodeID.TableID))
            {
                return joinRelation.BuildRelation(foreignKey, source.Alias.Name, target.Alias.Name).E();
            }
            // foreign key table = target
            else
            {
                return joinRelation.BuildRelation(foreignKey, target.Alias.Name, source.Alias.Name).E();
            }
        }

        private static void _InjectOn(JoinChainer source, Expression expression)
        {
            Chainer prevOfOn = source;
            Chainer nextOfOn;

            // skip .As
            if (source.Next is AliasAs)
            {
                prevOfOn = source.Next;
            }

            nextOfOn = prevOfOn.Next;

            // skip .By
            if (nextOfOn is ByChainer)
            {
                nextOfOn = nextOfOn.Next;
            }

            // inject .On
            var on = ((IOn)prevOfOn).On(expression);
            on.SetNext(nextOfOn);
        }

        #endregion

        #endregion

        #region Column

        internal string ProcessColumn(DbColumn column)
        {
            // check: column graph allowed only in .Select method
            if (!column.Parent.NodeID.Equals(column.Parent.Root.NodeID) && !(_buildContext.Current is SelectChainer))
            {
                throw new QueryTalkException(this, QueryTalkExceptionType.ColumnGraphDisallowed, 
                    column.ToString(), _buildContext.Current.Method);
            }

            // .Of
            if (column.OfAlias != null)
            {
                return ProcessColumnOf(column);
            }
            // without .Of
            else
            {
                return ProcessColumnAuto(column);
            }
        }

        private string ProcessColumnOf(DbColumn column)
        {
            // skip non .Select methods
            if (!(_buildContext.Current is SelectChainer))
            {
                return column.OfAlias;
            }

            bool found = false;
            TableChainer target = null;
            foreach (var table in _tables)
            {
                if (table.Alias.Name.EqualsCS(column.OfAlias))
                {
                    if (!table.HasNode)
                    {
                        throw JoinerColumnNotBoundException(column);
                    }

                    found = true;
                    target = table;
                    break;
                }
            }

            if (!found)
            {
                throw JoinerColumnNotBoundException(column);
            }

            if (!column.Parent.NodeID.Equals(target.Node.NodeID))
            {
                throw JoinerColumnNotBoundException(column);
            }

            return target.Alias.Name;
        }

        private string ProcessColumnAuto(DbColumn column)
        {
            TableChainer bestMatchTable = null;
            int bestMatchIndex = 0; 
            foreach (var table in _tables)
            {
                if (!table.HasNode)
                {
                    continue;
                }

                // node comparison
                if (column.JoinGraph == table.Node.NodeID.ToString())
                {
                    return table.Alias.Name;
                }

                // join graph comparison
                if (column.JoinGraph == table.JoinGraph)
                {
                    return table.Alias.Name;
                }

                // check the partial match
                var tableJoinParts = table.JoinGraph.Split(new char[] { Text.DotChar });
                var columnJoinParts = column.JoinGraph.Split(new char[] { Text.DotChar });
                if (tableJoinParts.Length < columnJoinParts.Length)
                {
                    if (column.JoinGraph.IndexOf(table.JoinGraph) == 0)
                    {
                        // best match condition
                        if (bestMatchIndex < table.JoinGraphNodeCount)
                        {
                            bestMatchTable = table;
                            bestMatchIndex = table.JoinGraphNodeCount;
                        }
                    }
                }
            }

            // perfect match is not found:

            // allow nested db columns that are part of the SEMQ
            //   example: 
            //      ...Whose(s.Address.City.Country.Name.NotEqualTo(s.Country.Name)) 
            if (column.RootSubject != null)
            {
                return column.RootSubject.Index.ToString();
            }

            // inject column graph
            if (bestMatchIndex != 0)
            {
                return _InjectColumnGraph(column, bestMatchTable, bestMatchIndex);
            }

            // no match is found (& bestMatchIndex == 0)
            throw JoinerColumnNotBoundException(column);
        }

        private string _InjectColumnGraph(DbColumn column, TableChainer bestMatchTable, int bestMatchIndex)
        {
            if (_lastJoinTable != null)
            {
                _targets.Add(_lastJoinTable);
                _lastJoinTable = null;  // do reset after the use
            }

            // move to the best match table in the column graph
            var node = column.Parent.Root;
            while (bestMatchIndex > 1)
            {
                node = node.Next;
                --bestMatchIndex;
            }

            // target table to join with
            var target = bestMatchTable;

            // loop through the rest of the unmatched tables in the column graph
            while (node != null)
            {
                node = node.Next;
                if (node != null)
                {
                    target = _InjectJoinBy(node, target.Alias.Name);
                }
            }

            return target.Alias.Name;
        }

        private TableChainer _InjectJoinBy(DbNode node, string alias)
        {
            Chainer last = _tables[_tables.Count - 1];
            Chainer next; 

            // skip .As
            if (last.Next is AliasAs)
            {
                last = last.Next;
            }

            // skip .On/.By
            if (last.Next is OnChainer || last.Next is ByChainer)
            {
                last = last.Next;
            }

            next = last.Next;

            // inject .Join.By
            var nodeToInsert = new InternalNode(node);

            // pass FK to support multiple relations in column graph - !
            ((IRelation)nodeToInsert).FK = node.GetFK();

            var by = ((ISemqJoin)last).Join(nodeToInsert).By(alias);
            by.SetNext(next);

            // add injected 
            JoinChainer source = (JoinChainer)_tables[_tables.Count - 1];
            _targets.Add(source);
            ByJoin(source);

            return source;
        }

        #endregion

        #region Joiner Exception

        private QueryTalkException JoinerException(TableChainer source, string alias, QueryTalkExceptionType exceptionType)
        {
            return new QueryTalkException(this, exceptionType,
                String.Format("joining table = {0} AS {1}{2}   alias of target table = {3}",
                    source.TableArgument.Build(_buildContext, _buildArgs), Filter.Delimit(source.Alias.Name),
                    Environment.NewLine, Filter.Delimit(alias)),
                Text.Method.By);
        }

        private QueryTalkException JoinerException(TableChainer source, TableChainer target, QueryTalkExceptionType exceptionType)
        {
            if (target != null)
            {
                return new QueryTalkException(this, exceptionType,
                    String.Format("joining table {5}= {0} AS {1}{2}   target table {6}= {3} AS {4}",
                        source.TableArgument.Build(_buildContext, _buildArgs), Filter.Delimit(source.Alias.Name),
                        Environment.NewLine, target.TableArgument.Build(_buildContext, _buildArgs), Filter.Delimit(target.Alias.Name),
                        (source.Node == null) ? "(INVALID) " : null, (target != null && target.Node == null) ? "(INVALID) " : null
                        ),
                    Text.Method.By);
            }
            else
            {
                return new QueryTalkException(this, exceptionType,
                    String.Format("joining table = {0} AS {1}",
                        source.TableArgument.Build(_buildContext, _buildArgs), Filter.Delimit(source.Alias.Name)),
                    Text.Method.By);
            }
        }

        private QueryTalkException JoinerAutoJoinFailedException(TableChainer source)
        {
            if (source.HasNode)
            {
                return new QueryTalkException(this, QueryTalkExceptionType.AutoJoinFailed,
                    String.Format("joining table = {0} AS {1}",
                        source.Node.Root.Name, Filter.Delimit(source.Alias.Name)),
                    Text.Method.Join);
            }
            else
            {
                return new QueryTalkException(this, QueryTalkExceptionType.AutoJoinFailed,
                    String.Format("joining table = {0} AS {1}",
                        source.TableArgument.Build(_buildContext, _buildArgs), Filter.Delimit(source.Alias.Name)),
                    Text.Method.Join);
            }
        }

        private QueryTalkException JoinerColumnNotBoundException(DbColumn column)
        {
            QueryTalkException exception;
            var col = (column.IsAuto) ? Text.Asterisk : column.ColumnMap.Name.Sql;

            if (column.OfAlias != null)
            {
                exception = new QueryTalkException(this, QueryTalkExceptionType.ColumnNotBound,
                    String.Format("mapped column = {0}.{1}.Of({2})",
                        column.Parent.Name, col, column.OfAlias), Text.Method.Of);
            }
            else
            {
                exception = new QueryTalkException(this, QueryTalkExceptionType.ColumnNotBound,
                    String.Format("mapped column = {0}.{1}",
                        column.Parent.Name, col));
            }

            if (_buildContext.Current is SelectChainer && ((SelectChainer)_buildContext.Current).IsCollect)
            {
                exception.SetExtra("Mapped columns cannot be used in the .Collect method.");
            }

            return exception;
        }

        #endregion

    }

}
