#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace QueryTalk.Wall
{
    internal static class DbMapping
    {
        private static int _databaseID = 0;

        internal static int NewDatabaseID
        {
            get
            {
                return System.Threading.Interlocked.Increment(ref _databaseID);
            }
        }

        private static HashSet<DatabaseMap> _databases;
        private static HashSet<NodeMap> _nodes;
        private static ConcurrentDictionary<Type, DB3> _rowTypes;

        internal static void AddRowType(Type type, DB3 nodeID)
        {
            _rowTypes.TryAdd(type, nodeID);
        }

        // collection of all links between the mapped tables
        private static HashSet<Link> _links;

        // links between tables with ordered rows (for graph building only)
        private static HashSet<OrderedLink> _orderedLinks;

        #region Invokers

        // collection of delegate methods that invoke table's static ctor
        private static HashSet<NodeInvoker> _invokers;

        // Adds a node invoker to the node invoker collection.
        internal static void AddNodeInvoker(NodeInvoker invoker)
        {
            _invokers.Add(invoker);
        }

        // invoke a specified node's static ctor
        private static void _Invoke(DB3 nodeID)
        {
            var invoker = _invokers.Where(a => a.ID.Equals(nodeID)).FirstOrDefault();
            if (invoker == null)
            {
                ThrowNodeNotFoundException(nodeID);
            }

            invoker.Invoke();
        }

        #endregion

        #region GraphInvokers

        // collection of delegate methods that invoke graph creators
        private static HashSet<GraphInvoker> _graphInvokers;

        // adds graph invoker to the collection
        internal static void AddGraphInvoker(GraphInvoker invoker)
        {
            if (!_graphInvokers.Where(a => a.ForeignKeyID.Equals(invoker.ForeignKeyID) 
                && a.ReferenceID.Equals(invoker.ReferenceID)).Any())
            {
                _graphInvokers.Add(invoker);
            }
        }

        // invoke certain graph creator
        internal static void GraphInvoke(DB3 foreignkeyID, DB3 referenceID, IEnumerable foreignKeyTable, IEnumerable referenceTable)
        {
            var invoker = _graphInvokers.Where(a => a.ForeignKeyID.Equals(foreignkeyID) &&
                a.ReferenceID.Equals(referenceID)).FirstOrDefault();

            if (invoker != null)
            {
                invoker.Invoke(foreignKeyTable, referenceTable);
            }
            else
            {
                throw new QueryTalkException("DbMapping.GraphInvoke", QueryTalkExceptionType.InvokerNotFoundInnerException,
                    String.Format("foreignKeyID = {0}{1}   referenceID = {2}", foreignkeyID, Environment.NewLine, referenceID));
            }
        }

        #endregion

        internal static Link GetLink(DB3 a, DB3 b)
        {
            return _links.Where(link => link.Equals(a, b)).FirstOrDefault();
        }

        internal static DatabaseMap AddDatabase(DatabaseMap map)
        {
            _databases.Add(map);
            return map;
        }

        internal static void AddNode(NodeMap map)
        {
            _nodes.Add(map);
        }

        internal static Link AddLink(Link link)
        { 
            _links.Add(link);
            _orderedLinks.Add(new OrderedLink(link.TableA, link.TableB, link));
            _orderedLinks.Add(new OrderedLink(link.TableB, link.TableA, link));
            return link;
        }

        internal static NodeMap GetNodeMap(DB3 nodeID)
        {
            var node = _nodes.Where(a => a.ID.Equals(nodeID))
                .SingleOrDefault();

            if (node == null)
            {
                _Invoke(nodeID);
                node = _nodes.Where(a => a.ID.Equals(nodeID))
                    .SingleOrDefault();
            }

            if (node == null)
            {
                ThrowNodeNotFoundException(nodeID);
            }

            return node;
        }

        internal static NodeMap GetNodeMap(Type rowType)
        {
            DB3 nodeID;
            if (!_rowTypes.TryGetValue(rowType, out nodeID))
            {
                // failure => create object in order to trigger the static ctor
                Activator.CreateInstance(rowType);

                _rowTypes.TryGetValue(rowType, out nodeID);
            }

            return GetNodeMap(nodeID);
        }

        internal static NodeMap TryGetNodeMap(DbRow row)
        {
            var map = GetNodeMap(row.NodeID);

            if (map.ID.Equals(DB3.Default))
            {
                DbRow.ThrowInvalidDbRowException(row.GetType());
            }

            return map;
        }

        internal static CrudProcedure TryGetProcedure(this DB3 nodeID, CrudProcedureType procedureType)
        {
            return Cache.CrudProcedures.Where(p => p.Equals(nodeID, procedureType)).FirstOrDefault();
        }

        public static Identifier GetNodeName(this DB3 nodeID)
        {
            return GetNodeMap(nodeID).Name;
        }

        internal static ColumnMap GetColumnMap(DB3 columnID)
        {
            var tableMap = GetNodeMap(columnID.TableID);
            return tableMap.Columns.Where(a => a.ID.Equals(columnID))
                .Single();
        }

        #region Relations

        // builds a relation SQL clause
        internal static Relation GetRelation(DB3 tableA, DB3 tableB, DB3 foreignKey, string method = null)     
        {
            var link = TryFindLink(tableA, tableB, method);

            if (foreignKey.Equals(DB3.Default))
            {
                if (link.HasManyRelations)
                {
                    throw new QueryTalkException("DbMapping.BuildRelation", QueryTalkExceptionType.MissingForeignKey,
                        String.Format("linked tables = {0}:{1}", GetNodeMap(tableA).Name.Sql, GetNodeMap(tableB).Name.Sql), method);
                }
                else
                {
                    foreignKey = link.DefaultForeignKey;
                }
            }

            return link.TryGetRelation(foreignKey, method);
        }

        internal static string GetForeignKeyName(DB3 foreignKey)
        {
            ColumnMap foreignKeyMap;
            string foreignKeyFullName = Text.Unknown;
            if (!foreignKey.Equals(DB3.Default))
            {
                foreignKeyMap = GetColumnMap(foreignKey);
                foreignKeyFullName = foreignKeyMap.FullName;
            }

            return foreignKeyFullName;
        }

        internal static Link TryFindLink(DB3 tableA, DB3 tableB, string method = null)
        {
            var link = GetLink(tableA, tableB);

            if (link == null && !tableA.Equals(tableB))
            {
                link = TryFindIntermediate(tableA, tableB, method);  
            }

            // second check
            if (link == null)
            {
                throw new QueryTalkException("DbMapping.TryFindLink", QueryTalkExceptionType.LinkNotFound,
                    String.Format("table A = {0}{1}   table B = {2}",
                        DbMapping.GetNodeMap(tableA).Name.Sql, Environment.NewLine,
                        DbMapping.GetNodeMap(tableB).Name.Sql),
                        method);
            }

            return link;
        }

        internal static Link TryFindPredicateLink(DbNode outerNode, DbNode innerNode, string method = null)
        {
            return TryFindLink(outerNode.SynonymOrThis.NodeID, innerNode.SynonymOrThis.NodeID, method);
        }

        // try find intermediate table & cache it
        private static Link TryFindIntermediate(DB3 tableA, DB3 tableB, string method = null)
        {
            // do not seek for link between the equal tables
            if (!tableA.Equals(tableB))
            {
                // find all (ordered) links of tableA
                var linksA = _orderedLinks.Where(a => a.TableA.Equals(tableA));

                // find all (ordered) links of tableB
                var linksB = _orderedLinks.Where(a => a.TableA.Equals(tableB));

                // find all intermediate tables
                var intermediates = 
                    (from a in linksA
                    join b in linksB on a.TableB equals b.TableB
                    select a.TableB).ToList();

                // enumerate through each intermediate table and evaluate it
                DB3 c = DB3.Default;    // first intermediate
                DB3 c2 = DB3.Default;   // superfluous intermediate (should not exists)
                foreach (var intermediate in intermediates)
                {
                    // make sure that the table has been initialized
                    _Invoke(intermediate);

                    // get link C to A
                    var linkToA = _orderedLinks
                        .Where(a => a.TableA.Equals(intermediate)
                            && a.TableB.Equals(tableA))
                        .Select(a => a.Link)
                        .Distinct()
                        .FirstOrDefault();

                    // get link C to B
                    var linkToB = _orderedLinks
                        .Where(a => a.TableA.Equals(intermediate)
                            && a.TableB.Equals(tableB))
                        .Select(a => a.Link)
                        .Distinct()
                        .FirstOrDefault();

                    // exclude intermediates that have links with many relations
                    if (linkToA.HasManyRelations || linkToB.HasManyRelations)
                    {
                        continue;
                    }
 
                    // There are 4 combinations of A.C.B relationship:
                    // --------------------------------------------------
                    //   A << C >> B    (allowed)
                    //   A << C << B    (allowed)
                    //   A >> C >> B    (allowed)
                    //   A >> C << B    (disallowed)
                    // -------------------------------------------------------------------------
                    // The intermediate tables that on ONE side of both relations are excluded.
                    // -------------------------------------------------------------------------
                    if (linkToA.IsRefTable(intermediate) && linkToB.IsRefTable(intermediate))
                    {
                        continue;
                    }

                    if (c.IsDefault)
                    {
                        c = intermediate;
                    }
                    else
                    {
                        c2 = intermediate;
                    }
                }

                // hit
                if (!c.IsDefault)
                {
                    // only a single intermediate table is allowed
                    if (c2.IsDefault)
                    {
                        // create and cache the link A:B
                        var link = new Link(tableA, tableB, c);
                        _links.Add(link);
                        return link;
                    }
                    else
                    {
                        // link ambiguity
                        throw new QueryTalkException("DbMapping.TryFindIntermediate", QueryTalkExceptionType.LinkAmbiguity,
                            String.Format("table A = {0}{1}   table B = {2}{3}   intermediate 1 = {4}{5}   intermediate 2 = {6}",
                                GetNodeMap(tableA).Name.Sql, Environment.NewLine, 
                                GetNodeMap(tableB).Name.Sql, Environment.NewLine, 
                                GetNodeMap(c).Name.Sql, Environment.NewLine, 
                                GetNodeMap(c2).Name.Sql),
                                method).SetObjectName(DbMapping.GetNodeMap(tableA).Name.Sql);
                    }
                }
            }

            // link has not been found
            throw new QueryTalkException("DbMapping.TryFindIntermediate",
                QueryTalkExceptionType.LinkNotFound,
                String.Format("table A = {0}{1}   table B = {2}",
                    GetNodeMap(tableA).Name.Sql, Environment.NewLine, GetNodeMap(tableB).Name.Sql),
                    method).SetObjectName(DbMapping.GetNodeMap(tableA).Name.Sql);
        }

        #endregion

        #region Convertions

        internal static IOpenView SelectFromSemantic(ISemantic query)
        {
            var subject = ((ISemantic)query).Subject;
            var context = new SemqContext(subject, subject.QueryRoot);
            return ((ISelect)query.Translate(context, null)).Select();
        }

        #endregion

        #region Graph Creation

        // creates entire graph
        internal static void CreateGraph<TRoot>(GraphPair[] graphPairs, params IEnumerable[] tables)
            where TRoot : DbRow
        {
            foreach (var pair in graphPairs)
            {
                GraphInvoke(pair.ForeignKeyID, pair.ReferenceID, (IEnumerable)tables[pair.ForeignKeyRX], (IEnumerable)tables[pair.ReferenceRX]);
            }
        }

        #endregion

        #region for Procedure + Func

        internal static void CreateParams(Designer root, DbNode node)
        {
            int i = 0;
            foreach (var param in node.Map.Params)
            {
                long index;

                // user-defined table types need to be index by Guid
                if (param.DataType.DT == DT.Udtt)
                {
                    index = Designer.GetVariableGuid();
                }
                // non-Udtt types (regular index)
                else
                {
                    index = root.GetVariableIndex();
                }

                var variable = new Variable(param.DataType, String.Format("@{0}", index));
                variable.IsOutput = param.IsOutput;  
                root.TryAddParamOrThrow(variable, true);
                ++i;
            }
        }

        // replace string argument(s) with Value object (string treated as values)
        internal static void PrepareFunctionArguments(FunctionArgument[] arguments, DbNode node)
        {
            if (node.Map.Params.Count > 0)
            {
                if (arguments.IsEmpty())
                {
                    node.Mapper.Throw(QueryTalkExceptionType.MissingFunctionArguments, null, Text.NotAvailable);
                }
            }
            else
            {
                return;  
            }

            for (int i = 0; i < arguments.Length; ++i)
            {
                var arg = arguments[i];

                if (!arg.IsNullReference() && arg.Original is System.String)
                {
                    if (Variable.Detect(arg.Original))
                    {
                        continue;
                    }

                    arguments[i] = new FunctionArgument(((string)arguments[i].Original).V());
                }
            }
        }

        #endregion

        static DbMapping()
        {
            _databases = new HashSet<DatabaseMap>(new MapEqualityComparer());
            _nodes = new HashSet<NodeMap>(new MapEqualityComparer());
            _links = new HashSet<Link>(new LinkEqualityComparer());
            _orderedLinks = new HashSet<OrderedLink>(new OrderedLinkEqualityComparer());
            _invokers = new HashSet<NodeInvoker>(new MapEqualityComparer());
            _graphInvokers = new HashSet<GraphInvoker>();
            _rowTypes = new ConcurrentDictionary<Type, DB3>();

            // add default node - !
            _nodes.Add(NodeMap.Default);
        }

        #region Exception(s)

        internal static void CheckNullAndThrow(object node)
        {
            if (node == null)
            {
                throw new QueryTalkException("DbMapping.Throw",
                    QueryTalkExceptionType.SubjectNull, null, Text.Method.PredicateMethod);
            }
        }

        internal static void ThrowForeignKeyNotFoundException(DB3 foreignKey, DB3 table)
        {
            var foreignKeyFullName = DbMapping.GetForeignKeyName(foreignKey);
            throw new QueryTalkException("Link.TryGetRelation", QueryTalkExceptionType.ForeignKeyNotFound,
                String.Format("table = {0}{1}   foreign key = {2}",
                    DbMapping.GetNodeMap(table).Name.Sql,
                    Environment.NewLine,
                    foreignKeyFullName), Text.Method.By);
        }

        private static void ThrowNodeNotFoundException(DB3 nodeID)
        {
            // try get database
            var database = _databases.Where(a => a.ID == nodeID.DatabaseID).FirstOrDefault();

            if (database != null)
            {
                throw new QueryTalkException("DbMapping.GetNodeMap", QueryTalkExceptionType.NodeNotFoundInnerException,
                    String.Format("database = {0}{1}   requested node ID = {2}", database.Name, Environment.NewLine, nodeID));
            }
            else
            {
                throw new QueryTalkException("DbMapping.GetNodeMap", QueryTalkExceptionType.NodeNotFoundInnerException,
                    String.Format("requested node ID = {0}", nodeID));
            }
        }

        #endregion
    }
}
