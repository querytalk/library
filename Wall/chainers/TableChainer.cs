#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Text;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public class TableChainer : Chainer, IQuery
    {
        private Table _table;

        private AliasAs _alias = null;
        internal AliasAs Alias       
        {
            get
            {
                return _alias;
            }
        }

        private DbNode _node;
        internal DbNode Node 
        {
            get
            {
                return _node;
            }
        }

        internal void SetNode(DbNode node)
        {
            _node = node;
        }

        internal bool HasNode
        {
            get
            {
                return Node != null;
            }
        }

        #region JoinGraph

        private string _joinGraph;
        internal string JoinGraph 
        {
            get
            {
                if (_joinGraph == null && _node != null)
                {
                    _joinGraph = Node.NodeID.ToString();  
                }
                return _joinGraph;
            }
        }

        internal void AppendJoinGraph(string prevGraph)
        {
            if (_node != null)
            {
                _joinGraph = String.Format("{0}.{1}", prevGraph, Node.NodeID);
            }
        }

        private int _joinGraphNodeCount = 1;   
        internal int JoinGraphNodeCount 
        {
            get
            {
                if (_joinGraph != null)
                {
                    _joinGraphNodeCount = _joinGraph.Split(new char[] { Text.DotChar }).Length;
                }

                return _joinGraphNodeCount;
            }
        }

        #endregion

        // set table alias (internally)
        internal void SetAlias(AliasAs alias)
        {
            // if a table already has an alias => remove it
            if (_alias != null)
            {
                Statement.RemoveAlias(_alias);
                _alias = null;
            }

            _alias = alias;
        }

        // set table alias (by client)
        internal void SetAliasByClient(AliasAs alias)
        {
            // if a table already has an alias => remove it
            if (_alias != null)
            {
                Statement.RemoveAlias(_alias);
                _alias = null;
            }

            Statement.CheckAlias(alias);
            TryThrow(Statement.Exception);

            _alias = alias;
        }

        #region Constructors

        private TableChainer(Chainer prev)
            : base(prev)
        {
            // set default table alias
            _alias = new AliasAs(Query.GetIncrementalTableAlias());

            Build = (buildContext, buildArgs) =>
            {
                return BaseBuildMethod(buildContext, buildArgs);
            };
        }

        internal TableChainer(Chainer prev, Table table) 
            : this(prev)
        {
            ProcessTableArgument(table);
        }

        internal TableChainer(Chainer prev, IOpenView openView)
            : this(prev)
        {
            IOpenView query = null;  

            // SEMQ
            if (openView is ISemantic)
            {        
                SetNode(((DbNode)openView).Root); 

                if (((ISemantic)openView).IsQuery)
                {
                    query = DbMapping.SelectFromSemantic((ISemantic)openView);
                }
                else if (openView is IFunction)
                {
                    ProcessTableArgument(((IFunction)openView).GetUdf());
                    return;
                }
                // table/view identifier:
                else
                {
                    if (_node.IsSynonym)
                    {
                        if (_node.SynonymQuery is IOpenView)
                        {
                            query = (IOpenView)_node.SynonymQuery;
                        }
                        else
                        {
                            ProcessTableArgument((View)_node.SynonymQuery);
                            return;
                        }
                    }
                    else
                    {
                        ProcessTableArgument(_node.Map.Name);
                        return;
                    }
                }
            }
            // SQL query:
            else
            {
                query = openView;
            }

            Query.AddTableObject(this);

            var tableArgument = query;  
            CheckNullAndThrow(Arg(() => tableArgument, tableArgument));
            TryThrow(((Chainer)query).Exception);

            var view = new View(query, Query);
            _table = view;

            Query.AddArguments(view.Query.Arguments.ToArray());

            if (chainException != null)
            {
                chainException.Extra = String.Format("Check {0} method.", chainException.Method);
                TryThrow();
            }

            TableArgument = _table;
        }

        // ctor for Pivot/Unpivot
        internal TableChainer(Chainer prev, bool overload)
            : base(prev)
        {
            var cfrom = GetPrev<FromChainer>();
            if (cfrom.Node != null)
            {
                SetNode(cfrom.Node);
            }

            _alias = new AliasAs(Query.GetIncrementalTableAlias());
            Query.AddTableObject(this);

            // no build method: build is done by PivotChainer object
        }

        #endregion

        internal string BaseBuildMethod(BuildContext buildContext, BuildArgs buildArgs)
        {
            string tableSql = TableArgument.Build(buildContext, buildArgs);  

            TryThrow(buildContext);

            StringBuilder sql = Text.GenerateSql(30)
                .NewLine(Keyword);     

            // subquery:
            if (TableArgument.Original is View)
            {
                sql.S().Append(Text.LeftBracket)
                    .Append(tableSql)
                    .NewLine(Text.RightBracket);
            }
            // table:
            else
            {
                sql.S().Append(tableSql);
            }

            // table with alias
            if (_alias != null)
            {
                return sql
                    .S().Append(Text.As)
                    .S().Append(Filter.Delimit(_alias.Name))
                    .ToString();
            }
            else
            {
                return sql.ToString();
            }
        }

        private void ProcessTableArgument(Table table)
        {
            Query.AddTableObject(this);

            CheckNullAndThrow(Arg(() => table, table));
            TryThrow(table.Exception);
            
            Query.AddArgument(table);
            _table = table;
            _alias = table.Alias ?? _alias;

            if (chainException != null)
            {
                chainException.Extra = String.Format("Check {0} method.", chainException.Method);
                TryThrow();
            }

            TableArgument = table;
        }

    }
}
