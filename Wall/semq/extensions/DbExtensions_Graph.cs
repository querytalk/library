#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Reflection;
using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        #region With

        #region 1-x

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="T1">The type of the related node.</typeparam>
        /// <param name="node">The node in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        public static DbGraph<TRoot, T1> With<TRoot, T1>(this DbTable<TRoot> node, DbTable<T1> relatedNode)
            where TRoot : DbRow
            where T1 : DbRow
        {
            node.CheckWithAndThrow(Text.Free.Subject, Text.Method.With);
        return new DbGraph<TRoot, T1>(node, relatedNode);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="T1">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T2">The type of the second node in related graph.</typeparam>
        /// <param name="node">The node in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2> With<TRoot, T1, T2>(this DbTable<TRoot> node, DbGraph<T1, T2> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
        {
            node.CheckWithAndThrow(Text.Free.Subject, Text.Method.With);
        return new DbGraph<TRoot, T1, T2>(node, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="T1">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T2">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the third node in related graph.</typeparam>
        /// <param name="node">The node in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3> With<TRoot, T1, T2, T3>(this DbTable<TRoot> node, DbGraph<T1, T2, T3> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
        {
            node.CheckWithAndThrow(Text.Free.Subject, Text.Method.With);
        return new DbGraph<TRoot, T1, T2, T3>(node, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="T1">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T2">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T4">The type of the fourth node in related graph.</typeparam>
        /// <param name="node">The node in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4> With<TRoot, T1, T2, T3, T4>(this DbTable<TRoot> node, DbGraph<T1, T2, T3, T4> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
        {
            node.CheckWithAndThrow(Text.Free.Subject, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4>(node, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="T1">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T2">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T4">The type of the fourth node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the fifth node in related graph.</typeparam>
        /// <param name="node">The node in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5> With<TRoot, T1, T2, T3, T4, T5>(this DbTable<TRoot> node, 
            DbGraph<T1, T2, T3, T4, T5> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
        {
            node.CheckWithAndThrow(Text.Free.Subject, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5>(node, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="T1">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T2">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T4">The type of the fourth node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the fifth node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the sixth node in related graph.</typeparam>
        /// <param name="node">The node in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6> With<TRoot, T1, T2, T3, T4, T5, T6>(this DbTable<TRoot> node,
            DbGraph<T1, T2, T3, T4, T5, T6> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
        {
            node.CheckWithAndThrow(Text.Free.Subject, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6>(node, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="T1">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T2">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T4">The type of the fourth node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the fifth node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the sixth node in related graph.</typeparam>
        /// <typeparam name="T7">The type of the seventh node in related graph.</typeparam>
        /// <param name="node">The node in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7> With<TRoot, T1, T2, T3, T4, T5, T6, T7>(this DbTable<TRoot> node,
            DbGraph<T1, T2, T3, T4, T5, T6, T7> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
        {
            node.CheckWithAndThrow(Text.Free.Subject, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7>(node, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="T1">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T2">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T4">The type of the fourth node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the fifth node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the sixth node in related graph.</typeparam>
        /// <typeparam name="T7">The type of the seventh node in related graph.</typeparam>
        /// <typeparam name="T8">The type of the eighth node in related graph.</typeparam>
        /// <param name="node">The node in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8> With<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(this DbTable<TRoot> node,
            DbGraph<T1, T2, T3, T4, T5, T6, T7, T8> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
            where T8 : DbRow
        {
            node.CheckWithAndThrow(Text.Free.Subject, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(node, relatedGraph);
        }

        #region DbRow<TRoot>

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="T1">The type of the related node.</typeparam>
        /// <param name="row">The row in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        public static DbGraph<TRoot, T1> With<TRoot, T1>(this INode<TRoot> row, DbTable<T1> relatedNode)
            where TRoot : DbRow
            where T1 : DbRow
        {
            row.CheckNullAndThrow(Text.Free.Row, Text.Method.With);
            return new DbGraph<TRoot, T1>(((DbRow)row).TrySetNode<TRoot>(), relatedNode);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="T1">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T2">The type of the second node in related graph.</typeparam>
        /// <param name="row">The row in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2> With<TRoot, T1, T2>(this INode<TRoot> row, DbGraph<T1, T2> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
        {
            row.CheckNullAndThrow(Text.Free.Row, Text.Method.With);
            return new DbGraph<TRoot, T1, T2>(((DbRow)row).TrySetNode<TRoot>(), relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="T1">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T2">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the third node in related graph.</typeparam>
        /// <param name="row">The row in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3> With<TRoot, T1, T2, T3>(this INode<TRoot> row, DbGraph<T1, T2, T3> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
        {
            row.CheckNullAndThrow(Text.Free.Row, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3>(((DbRow)row).TrySetNode<TRoot>(), relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="T1">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T2">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T4">The type of the fourth node in related graph.</typeparam>
        /// <param name="row">The row in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4> With<TRoot, T1, T2, T3, T4>(this INode<TRoot> row, DbGraph<T1, T2, T3, T4> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
        {
            row.CheckNullAndThrow(Text.Free.Row, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4>(((DbRow)row).TrySetNode<TRoot>(), relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="T1">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T2">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T4">The type of the fourth node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the fifth node in related graph.</typeparam>
        /// <param name="row">The row in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5> With<TRoot, T1, T2, T3, T4, T5>(this INode<TRoot> row,
            DbGraph<T1, T2, T3, T4, T5> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
        {
            row.CheckNullAndThrow(Text.Free.Row, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5>(((DbRow)row).TrySetNode<TRoot>(), relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="T1">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T2">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T4">The type of the fourth node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the fifth node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the sixth node in related graph.</typeparam>
        /// <param name="row">The row in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6> With<TRoot, T1, T2, T3, T4, T5, T6>(this INode<TRoot> row,
            DbGraph<T1, T2, T3, T4, T5, T6> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
        {
            row.CheckNullAndThrow(Text.Free.Row, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6>(((DbRow)row).TrySetNode<TRoot>(), relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="T1">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T2">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T4">The type of the fourth node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the fifth node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the sixth node in related graph.</typeparam>
        /// <typeparam name="T7">The type of the seventh node in related graph.</typeparam>
        /// <param name="row">The row in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7> With<TRoot, T1, T2, T3, T4, T5, T6, T7>(this INode<TRoot> row,
            DbGraph<T1, T2, T3, T4, T5, T6, T7> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
        {
            row.CheckNullAndThrow(Text.Free.Row, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7>(((DbRow)row).TrySetNode<TRoot>(), relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="T1">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T2">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T4">The type of the fourth node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the fifth node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the sixth node in related graph.</typeparam>
        /// <typeparam name="T7">The type of the seventh node in related graph.</typeparam>
        /// <typeparam name="T8">The type of the eighth node in related graph.</typeparam>
        /// <param name="row">The row in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8> With<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(this INode<TRoot> row,
            DbGraph<T1, T2, T3, T4, T5, T6, T7, T8> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
            where T8 : DbRow
        {
            row.CheckNullAndThrow(Text.Free.Row, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(((DbRow)row).TrySetNode<TRoot>(), relatedGraph);
        }

        #endregion

        #endregion

        #region 2-x

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the related node.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        public static DbGraph<TRoot, T1, T2> With<TRoot, T1, T2>(this DbGraph<TRoot, T1> graph, DbTable<T2> relatedNode)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2>(graph, relatedNode);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the second node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3> With<TRoot, T1, T2, T3>(this DbGraph<TRoot, T1> graph, DbGraph<T2, T3> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3>(graph, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T4">The type of the third node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4> With<TRoot, T1, T2, T3, T4>(this DbGraph<TRoot, T1> graph, DbGraph<T2, T3, T4> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4>(graph, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T4">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the fourth node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5> With<TRoot, T1, T2, T3, T4, T5>(this DbGraph<TRoot, T1> graph, 
            DbGraph<T2, T3, T4, T5> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5>(graph, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T4">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the fourth node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the fifth node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6> With<TRoot, T1, T2, T3, T4, T5, T6>(this DbGraph<TRoot, T1> graph,
            DbGraph<T2, T3, T4, T5, T6> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6>(graph, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T4">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the fourth node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the fifth node in related graph.</typeparam>
        /// <typeparam name="T7">The type of the sixth node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7> With<TRoot, T1, T2, T3, T4, T5, T6, T7>(this DbGraph<TRoot, T1> graph,
            DbGraph<T2, T3, T4, T5, T6, T7> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7>(graph, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T3">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T4">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the fourth node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the fifth node in related graph.</typeparam>
        /// <typeparam name="T7">The type of the sixth node in related graph.</typeparam>
        /// <typeparam name="T8">The type of the seventh node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8> With<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(this DbGraph<TRoot, T1> graph,
            DbGraph<T2, T3, T4, T5, T6, T7, T8> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
            where T8 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(graph, relatedGraph);
        }

        #endregion

        #region 3-x

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the related node.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3> With<TRoot, T1, T2, T3>(this DbGraph<TRoot, T1, T2> graph, DbTable<T3> relatedNode)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3>(graph, relatedNode);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the first node related graph.</typeparam>
        /// <typeparam name="T4">The type of the second node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4> With<TRoot, T1, T2, T3, T4>(this DbGraph<TRoot, T1, T2> graph, DbGraph<T3, T4> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4>(graph, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the first node related graph.</typeparam>
        /// <typeparam name="T4">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the third node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5> With<TRoot, T1, T2, T3, T4, T5>(this DbGraph<TRoot, T1, T2> graph, 
            DbGraph<T3, T4, T5> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5>(graph, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the first node related graph.</typeparam>
        /// <typeparam name="T4">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the fourth node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6> With<TRoot, T1, T2, T3, T4, T5, T6>(this DbGraph<TRoot, T1, T2> graph,
            DbGraph<T3, T4, T5, T6> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6>(graph, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the first node related graph.</typeparam>
        /// <typeparam name="T4">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the fourth node in related graph.</typeparam>
        /// <typeparam name="T7">The type of the fifth node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7> With<TRoot, T1, T2, T3, T4, T5, T6, T7>(this DbGraph<TRoot, T1, T2> graph,
            DbGraph<T3, T4, T5, T6, T7> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7>(graph, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the first node related graph.</typeparam>
        /// <typeparam name="T4">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the fourth node in related graph.</typeparam>
        /// <typeparam name="T7">The type of the fifth node in related graph.</typeparam>
        /// <typeparam name="T8">The type of the sixth node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8> With<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(this DbGraph<TRoot, T1, T2> graph,
            DbGraph<T3, T4, T5, T6, T7, T8> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
            where T8 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(graph, relatedGraph);
        }

        #endregion

        #region 4-x

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <typeparam name="T4">The type of the related node.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4> With<TRoot, T1, T2, T3, T4>(this DbGraph<TRoot, T1, T2, T3> graph, DbTable<T4> relatedNode)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4>(graph, relatedNode);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node graph.</typeparam>
        /// <typeparam name="T4">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the second node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5> With<TRoot, T1, T2, T3, T4, T5>(this DbGraph<TRoot, T1, T2, T3> graph, 
            DbGraph<T4, T5> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5>(graph, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node graph.</typeparam>
        /// <typeparam name="T4">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the third node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6> With<TRoot, T1, T2, T3, T4, T5, T6>(this DbGraph<TRoot, T1, T2, T3> graph,
            DbGraph<T4, T5, T6> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6>(graph, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node graph.</typeparam>
        /// <typeparam name="T4">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T7">The type of the fourth node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7> With<TRoot, T1, T2, T3, T4, T5, T6, T7>(this DbGraph<TRoot, T1, T2, T3> graph,
            DbGraph<T4, T5, T6, T7> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7>(graph, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node graph.</typeparam>
        /// <typeparam name="T4">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T5">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the third node in related graph.</typeparam>
        /// <typeparam name="T7">The type of the fourth node in related graph.</typeparam>
        /// <typeparam name="T8">The type of the fifth node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8> With<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(this DbGraph<TRoot, T1, T2, T3> graph,
            DbGraph<T4, T5, T6, T7, T8> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
            where T8 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(graph, relatedGraph);
        }

        #endregion

        #region 5-x

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <typeparam name="T4">The type of the fifth node in graph.</typeparam>
        /// <typeparam name="T5">The type of the related node.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5> With<TRoot, T1, T2, T3, T4, T5>(this DbGraph<TRoot, T1, T2, T3, T4> graph,
            DbTable<T5> relatedNode)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5>(graph, relatedNode);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <typeparam name="T4">The type of the fifth node in graph.</typeparam>
        /// <typeparam name="T5">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the second node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6> With<TRoot, T1, T2, T3, T4, T5, T6>(this DbGraph<TRoot, T1, T2, T3, T4> graph,
            DbGraph<T5, T6> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6>(graph, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <typeparam name="T4">The type of the fifth node in graph.</typeparam>
        /// <typeparam name="T5">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T7">The type of the third node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7> With<TRoot, T1, T2, T3, T4, T5, T6, T7>(this DbGraph<TRoot, T1, T2, T3, T4> graph,
            DbGraph<T5, T6, T7> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7>(graph, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <typeparam name="T4">The type of the fifth node in graph.</typeparam>
        /// <typeparam name="T5">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T6">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T7">The type of the fourth node in related graph.</typeparam>
        /// <typeparam name="T8">The type of the fourth node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8> With<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(this DbGraph<TRoot, T1, T2, T3, T4> graph,
            DbGraph<T5, T6, T7, T8> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
            where T8 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(graph, relatedGraph);
        }

        #endregion

        #region 6-x

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <typeparam name="T4">The type of the fifth node in graph.</typeparam>
        /// <typeparam name="T5">The type of the sixth node in graph.</typeparam>
        /// <typeparam name="T6">The type of the related node.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6> With<TRoot, T1, T2, T3, T4, T5, T6>(this DbGraph<TRoot, T1, T2, T3, T4, T5> graph,
            DbTable<T6> relatedNode)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6>(graph, relatedNode);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <typeparam name="T4">The type of the fifth node in graph.</typeparam>
        /// <typeparam name="T5">The type of the sixth node in graph.</typeparam>
        /// <typeparam name="T6">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T7">The type of the second node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7> With<TRoot, T1, T2, T3, T4, T5, T6, T7>(this DbGraph<TRoot, T1, T2, T3, T4, T5> graph,
            DbGraph<T6, T7> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7>(graph, relatedGraph);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <typeparam name="T4">The type of the fifth node in graph.</typeparam>
        /// <typeparam name="T5">The type of the sixth node in graph.</typeparam>
        /// <typeparam name="T6">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T7">The type of the second node in related graph.</typeparam>
        /// <typeparam name="T8">The type of the third node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8> With<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(this DbGraph<TRoot, T1, T2, T3, T4, T5> graph,
            DbGraph<T6, T7, T8> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
            where T8 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(graph, relatedGraph);
        }

        #endregion

        #region 7-x

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <typeparam name="T4">The type of the fifth node in graph.</typeparam>
        /// <typeparam name="T5">The type of the sixth node in graph.</typeparam>
        /// <typeparam name="T6">The type of the seventh node in graph.</typeparam>
        /// <typeparam name="T7">The type of the related node.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7> With<TRoot, T1, T2, T3, T4, T5, T6, T7>(this DbGraph<TRoot, T1, T2, T3, T4, T5, T6> graph,
            DbTable<T7> relatedNode)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7>(graph, relatedNode);
        }

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <typeparam name="T4">The type of the fifth node in graph.</typeparam>
        /// <typeparam name="T5">The type of the sixth node in graph.</typeparam>
        /// <typeparam name="T6">The type of the seventh node in related graph.</typeparam>
        /// <typeparam name="T7">The type of the first node in related graph.</typeparam>
        /// <typeparam name="T8">The type of the second node in related graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8> With<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(this DbGraph<TRoot, T1, T2, T3, T4, T5, T6> graph,
            DbGraph<T7, T8> relatedGraph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
            where T8 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(graph, relatedGraph);
        }

        #endregion

        #region 8-x

        /// <summary>
        /// Defines the graph of related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <typeparam name="T4">The type of the fifth node in graph.</typeparam>
        /// <typeparam name="T5">The type of the sixth node in graph.</typeparam>
        /// <typeparam name="T6">The type of the seventh node in graph.</typeparam>
        /// <typeparam name="T7">The type of the eighth node in graph.</typeparam>
        /// <typeparam name="T8">The type of the related node.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        public static DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8> With<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(this DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7> graph,
            DbTable<T8> relatedNode)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
            where T8 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.With);
            return new DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(graph, relatedNode);
        }

        #endregion

        #endregion

        #region Go

        /// <summary>
        /// Executes the semantic query with a graph definition returning the query result with the related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        public static Result<TRoot> Go<TRoot, T1>(this DbGraph<TRoot, T1> graph)
            where TRoot : DbRow
            where T1 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.Go);
            return PublicInvoker.Call<Result<TRoot>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var proc = EndProc<TRoot>(graph);  
                var connectable = Reader.GetConnectable(ca, proc);
                var result = Reader.LoadMany<TRoot, T1>(connectable);
                var row = ((IGraph)graph).Subject.Row;
                var rowTable = row.TryCollectRow<TRoot>();
                DbMapping.CreateGraph<TRoot>(proc.GraphPairs,
                    rowTable ?? result.Table1, result.Table2);

                return new Result<TRoot>(connectable, rowTable ?? result.Table1);
            });
        }

        /// <summary>
        /// Executes the semantic query with a graph definition returning the query result with the related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        public static Result<TRoot> Go<TRoot, T1, T2>(this DbGraph<TRoot, T1, T2> graph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.Go);
            return PublicInvoker.Call<Result<TRoot>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var proc = EndProc<TRoot>(graph);   
                var connectable = Reader.GetConnectable(ca, proc);
                var result = Reader.LoadMany<TRoot, T1, T2>(connectable);
                var row = ((IGraph)graph).Subject.Row;
                var rowTable = row.TryCollectRow<TRoot>();
                DbMapping.CreateGraph<TRoot>(proc.GraphPairs,
                    rowTable ?? result.Table1, result.Table2, result.Table3);

                return new Result<TRoot>(connectable, rowTable ?? result.Table1);
            });
        }

        /// <summary>
        /// Executes the semantic query with a graph definition returning the query result with the related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        public static Result<TRoot> Go<TRoot, T1, T2, T3>(this DbGraph<TRoot, T1, T2, T3> graph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.Go);
            return PublicInvoker.Call<Result<TRoot>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var proc = EndProc<TRoot>(graph);   
                var connectable = Reader.GetConnectable(ca, proc);
                var result = Reader.LoadMany<TRoot, T1, T2, T3>(connectable);
                var row = ((IGraph)graph).Subject.Row;
                var rowTable = row.TryCollectRow<TRoot>();
                DbMapping.CreateGraph<TRoot>(proc.GraphPairs,
                    rowTable ?? result.Table1, result.Table2, result.Table3, result.Table4);

                return new Result<TRoot>(connectable, rowTable ?? result.Table1);
            });
        }

        /// <summary>
        /// Executes the semantic query with a graph definition returning the query result with the related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <typeparam name="T4">The type of the fifth node in graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        public static Result<TRoot> Go<TRoot, T1, T2, T3, T4>(this DbGraph<TRoot, T1, T2, T3, T4> graph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.Go);
            return PublicInvoker.Call<Result<TRoot>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var proc = EndProc<TRoot>(graph);  
                var connectable = Reader.GetConnectable(ca, proc);
                var result = Reader.LoadMany<TRoot, T1, T2, T3, T4>(connectable);
                var row = ((IGraph)graph).Subject.Row;
                var rowTable = row.TryCollectRow<TRoot>();
                DbMapping.CreateGraph<TRoot>(proc.GraphPairs,
                    rowTable ?? result.Table1, result.Table2, result.Table3, result.Table4,
                        result.Table5);

                return new Result<TRoot>(connectable, rowTable ?? result.Table1);
            });
        }

        /// <summary>
        /// Executes the semantic query with a graph definition returning the query result with the related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <typeparam name="T4">The type of the fifth node in graph.</typeparam>
        /// <typeparam name="T5">The type of the sixth node in graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        public static Result<TRoot> Go<TRoot, T1, T2, T3, T4, T5>(this DbGraph<TRoot, T1, T2, T3, T4, T5> graph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.Go);
            return PublicInvoker.Call<Result<TRoot>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var proc = EndProc<TRoot>(graph);   
                var connectable = Reader.GetConnectable(ca, proc);
                var result = Reader.LoadMany<TRoot, T1, T2, T3, T4, T5>(connectable);
                var row = ((IGraph)graph).Subject.Row;
                var rowTable = row.TryCollectRow<TRoot>();
                DbMapping.CreateGraph<TRoot>(proc.GraphPairs,
                    rowTable ?? result.Table1, result.Table2, result.Table3, result.Table4,
                        result.Table5, result.Table6);

                return new Result<TRoot>(connectable, rowTable ?? result.Table1);
            });
        }

        /// <summary>
        /// Executes the semantic query with a graph definition returning the query result with the related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <typeparam name="T4">The type of the fifth node in graph.</typeparam>
        /// <typeparam name="T5">The type of the sixth node in graph.</typeparam>
        /// <typeparam name="T6">The type of the seventh node in graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        public static Result<TRoot> Go<TRoot, T1, T2, T3, T4, T5, T6>(this DbGraph<TRoot, T1, T2, T3, T4, T5, T6> graph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.Go);
            return PublicInvoker.Call<Result<TRoot>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var proc = EndProc<TRoot>(graph);   
                var connectable = Reader.GetConnectable(ca, proc);
                var result = Reader.LoadMany<TRoot, T1, T2, T3, T4, T5, T6>(connectable);
                var row = ((IGraph)graph).Subject.Row;
                var rowTable = row.TryCollectRow<TRoot>();
                DbMapping.CreateGraph<TRoot>(proc.GraphPairs,
                    rowTable ?? result.Table1, result.Table2, result.Table3, result.Table4,
                        result.Table5, result.Table6, result.Table7);

                return new Result<TRoot>(connectable, rowTable ?? result.Table1);
            });
        }

        /// <summary>
        /// Executes the semantic query with a graph definition returning the query result with the related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <typeparam name="T4">The type of the fifth node in graph.</typeparam>
        /// <typeparam name="T5">The type of the sixth node in graph.</typeparam>
        /// <typeparam name="T6">The type of the seventh node in graph.</typeparam>
        /// <typeparam name="T7">The type of the eighth node in graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        public static Result<TRoot> Go<TRoot, T1, T2, T3, T4, T5, T6, T7>(this DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7> graph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.Go);
            return PublicInvoker.Call<Result<TRoot>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var proc = EndProc<TRoot>(graph);   
                var connectable = Reader.GetConnectable(ca, proc);
                var result = Reader.LoadMany<TRoot, T1, T2, T3, T4, T5, T6, T7>(connectable);
                var row = ((IGraph)graph).Subject.Row;
                var rowTable = row.TryCollectRow<TRoot>();
                DbMapping.CreateGraph<TRoot>(proc.GraphPairs,
                    rowTable ?? result.Table1, result.Table2, result.Table3, result.Table4,
                        result.Table5, result.Table6, result.Table7, result.Table8);

                return new Result<TRoot>(connectable, rowTable ?? result.Table1);
            });
        }

        /// <summary>
        /// Executes the semantic query with a graph definition returning the query result with the related data.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node in graph.</typeparam>
        /// <typeparam name="T1">The type of the second node in graph.</typeparam>
        /// <typeparam name="T2">The type of the third node in graph.</typeparam>
        /// <typeparam name="T3">The type of the fourth node in graph.</typeparam>
        /// <typeparam name="T4">The type of the fifth node in graph.</typeparam>
        /// <typeparam name="T5">The type of the sixth node in graph.</typeparam>
        /// <typeparam name="T6">The type of the seventh node in graph.</typeparam>
        /// <typeparam name="T7">The type of the eighth node in graph.</typeparam>
        /// <typeparam name="T8">The type of the ninth node in graph.</typeparam>
        /// <param name="graph">The graph in relation.</param>
        public static Result<TRoot> Go<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(this DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8> graph)
            where TRoot : DbRow
            where T1 : DbRow
            where T2 : DbRow
            where T3 : DbRow
            where T4 : DbRow
            where T5 : DbRow
            where T6 : DbRow
            where T7 : DbRow
            where T8 : DbRow
        {
            graph.CheckNullAndThrow(Text.Free.Graph, Text.Method.Go);
            return PublicInvoker.Call<Result<TRoot>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var proc = EndProc<TRoot>(graph);  
                var connectable = Reader.GetConnectable(ca, proc);
                var result = Reader.LoadMany<TRoot, T1, T2, T3, T4, T5, T6, T7, T8>(connectable);
                var row = ((IGraph)graph).Subject.Row;
                var rowTable = row.TryCollectRow<TRoot>();
                DbMapping.CreateGraph<TRoot>(proc.GraphPairs,
                    rowTable ?? result.Table1, result.Table2, result.Table3, result.Table4,
                        result.Table5, result.Table6, result.Table7, result.Table8, result.Table9);

                return new Result<TRoot>(connectable, rowTable ?? result.Table1);
            });
        }

        #endregion
 
        #region EndProc<TRoot>

        private static Procedure EndProc<TRoot>(DbGraph<TRoot> prev)
            where TRoot : DbRow
        {
            var graphRoot = ((IGraph)prev).Root;
            var context = new SemqContext(graphRoot);
            Translate.TranslateGraph(graphRoot, context, null);
            var proc = ((IEndProc)context.TranslatedGraph).EndProcInternal();
            proc.GraphPairs = context.GraphPairs.ToArray();
            return proc;
        }

        #endregion

    }
}
