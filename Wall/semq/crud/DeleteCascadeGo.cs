#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QueryTalk.Wall
{
    static partial class Crud
    {
        internal static Result<T> DeleteCascadeGo<T>(Assembly client, DbRow row, int maxLevels, ConnectBy connectBy)
            where T : DbRow
        {
            var name = Text.NotAvailable;

            try
            {
                if (row == null)
                {
                    throw new QueryTalkException("Crud.GoDeleteCascade", QueryTalkExceptionType.ArgumentNull, "row = null",
                        Text.Method.DeleteCascadeGo);
                }

                Crud.CheckTable(row, Text.Method.DeleteCascadeGo);

                CrudProcedure cached = row.NodeID.TryGetProcedure(CrudProcedureType.DeleteCascadeGo);
                Procedure proc;

                if (cached == null)
                {
                    var map = DbMapping.TryGetNodeMap(row);

                    var where = map.BuildRKPredicate(row.GetOriginalRKValues(), 0);
                    Chainer workingProc = Designer.GetNewDesigner(Text.Method.DeleteCascadeGo, true, true)
                        .ParamNodeColumns(row.NodeID, ColumnSelector.RK);

                    // collect all linked nodes using GetChildren method
                    List<NodeTree> tree = new List<NodeTree>();
                    ((ITable)((INode<T>)row).Node).LoadChildren(ref tree, 1, maxLevels);

                    // loop through all leaves
                    foreach (var node in tree.OrderByDescending(a => a.Level))
                    {
                        workingProc = BuildQuery(tree, node, node, node.Level, workingProc, where, true);
                    }

                    // delete root row & create Procedure object
                    proc = ((IFrom)workingProc)
                        .From(map.Name).As(0)
                        .Where(where)
                        .Delete(Text.Zero)
                        .EndProcInternal();

                    Cache.CrudProcedures.Add(new CrudProcedure(row.NodeID, CrudProcedureType.DeleteCascadeGo, proc));
                }
                else
                {
                    proc = cached.Procedure;
                }

                var rkValues = row.GetOriginalRKValues();
                var args = new List<ParameterArgument>();
                foreach (var value in rkValues)
                {
                    args.Add(new ParameterArgument(new Value(value)));
                }

                var cpass = proc.Pass(args.ToArray());
                cpass.SetRootMap(row.NodeID);  
                var connectable = Reader.GetConnectable(client, row, cpass, connectBy);
                var result = connectable.Go();

                return new Result<T>(true, -1);
            }
            catch (QueryTalkException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw Crud.ClrException(ex, name, Text.Method.DeleteCascadeGo);
            }
        }

        // builds the delete query recursively (with a zero-based root node)
        private static Chainer BuildQuery(
            List<NodeTree> tree,        // collection of all related nodes
            NodeTree leaf,              // leaf node
            NodeTree node,              // current node 
            int level,                  // current node's level
            Chainer workingProc,        // proc in building progress 
            Expression where,           // where clause
            bool isLeaf                 // is leaf level
          )  
        {
            if (level == 0 || node == null)
            {
                return workingProc;
            }

            var link = DbMapping.TryFindLink(node.Child, node.Parent, Text.Method.DeleteCascadeGo);

            foreach (var relation in link.Relations)
            {
                if (isLeaf)
                {
                    workingProc = ((IFrom)workingProc)
                        .From(node.Child.GetNodeName())
                        .As(level); 
                }

                // get on condition
                var joinConditionSql = relation.BuildRelation(relation.FKTable, level, level - 1);

                // build join
                workingProc = ((IInnerJoin)workingProc)
                    .InnerJoin(relation.RefTable.GetNodeName()).As(level - 1)
                    .On(joinConditionSql.E());
                
                // move up along the parent chain
                var parent = tree.Where(a => a.Child.Equals(node.Parent) && a.Level == level - 1).FirstOrDefault();

                if (parent == null && level - 1 != 0)
                {
                    throw new QueryTalkException("DeleteCascadeGo.BuildQuery", QueryTalkExceptionType.DeleteCascadeInnerException, null);
                }

                workingProc = BuildQuery(tree, leaf, parent, level - 1, workingProc, where, false);

                if (level - 1 == 0)
                {
                    workingProc = ((IWhere)workingProc)
                        .Where(where)
                        .Delete(leaf.Level)
                        .Comment(""); 
                }
            }

            return workingProc; 
        }

    }
}
