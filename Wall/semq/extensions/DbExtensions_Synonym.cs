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
        /// Instructs the builder to use the specified synonym for a node in the relational code. 
        /// (For example, a base table can be used as a synonym of a view.)
        /// </summary>
        /// <typeparam name="T">The type of a node.</typeparam>
        /// <param name="node">A node whose synonym is to be used.</param>
        /// <param name="synonym">A synonym of a node.</param>
        public static DbTable<T> UseAs<T>(this DbTable<T> node, ITable synonym)
            where T : DbRow
        {
            node.CheckNullAndThrow("node", Text.Method.UseAs);
            synonym.CheckNullAndThrow("synonym", Text.Method.UseAs);
            ((DbNode)node).Synonym = (DbNode)synonym;
            return node;
        }

        /// <summary>
        /// Instructs the builder to use the specified synonym for a query in the relational code. 
        /// </summary>
        /// <typeparam name="T">The type of a synonym node.</typeparam>
        /// <param name="prev">A SQL query.</param>
        /// <param name="synonym">A synonym of a query.</param>
        public static DbTable<T> UseAs<T>(this IEndView prev, DbTable<T> synonym)
            where T : DbRow
        {
            prev.CheckNullAndThrow("prev", Text.Method.UseAs);
            synonym.CheckNullAndThrow("synonym", Text.Method.UseAs);

            CheckSynonymAndThrow(synonym);
            if (((Chainer)prev).GetRoot().Statements.Count > 1)
            {
                throw new QueryTalkException(".UseAs", QueryTalkExceptionType.InvalidSynonym, null, Text.Method.UseAs);
            }

            ((DbNode)(object)synonym).SynonymQuery = (Chainer)prev;
            return synonym;
        }

        /// <summary>
        /// Instructs the builder to use the specified synonym for a View object in the relational code. 
        /// </summary>
        /// <typeparam name="T">The type of a synonym node.</typeparam>
        /// <param name="prev">A View object.</param>
        /// <param name="synonym">A synonym of a query.</param>
        public static DbTable<T> UseAs<T>(this View prev, DbTable<T> synonym)
            where T : DbRow
        {
            prev.CheckNullAndThrow("prev", Text.Method.UseAs);
            synonym.CheckNullAndThrow("synonym", Text.Method.UseAs);

            CheckSynonymAndThrow(synonym);

            ((DbNode)(object)synonym).SynonymQuery = (Chainer)prev;
            return synonym;
        }

        /// <summary>
        /// Joins the nodes in a graph instructing the builder to use the specified synonym for the join query in the relational code.
        /// </summary>
        /// <typeparam name="T">The type of a synonym node.</typeparam>
        /// <param name="graph">A graph.</param>
        /// <param name="synonym">A synonym of a graph.</param>
        public static DbTable<T> JoinAs<T>(this ISemantic graph, DbTable<T> synonym)
            where T : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.Join);
            synonym.CheckNullAndThrow("synonym", Text.Method.Join);
            
            CheckSynonymAndThrow(synonym);

            var node = graph.Subject;
            bool found = false;
            do
            {
                if (((DbNode)(object)synonym).Equals(node))
                {
                    found = true;
                    break;
                }
            } while ((node = node.Next) != null);
            if (!found)
            {
                throw new QueryTalkException(".JoinAs<T>", QueryTalkExceptionType.InvalidSynonym,
                    String.Format("synonym = {0}", synonym), Text.Method.JoinSynonym);
            }

            return ((IEndView)((ISemqToSql)graph).Join()
                .Select("1.*"))
                .UseAs(synonym);
        }

        private static void CheckSynonymAndThrow(DbNode synonym)
        {
            if (!synonym.IsTable())
            {
                throw new QueryTalkException(".UseAs", QueryTalkExceptionType.InvalidSynonym,
                    String.Format("synonym = {0}", synonym), Text.Method.JoinSynonym);
            }
        }

    }
}
