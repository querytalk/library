#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// Represents a graph of a root node.
    /// </summary>
    /// <typeparam name="TRoot">The type of the root node.</typeparam>
    /// <typeparam name="T1">The type of the second node.</typeparam>
    public class DbGraph<TRoot, T1> : DbGraph<TRoot>, IGraph
        where TRoot : DbRow
        where T1 : DbRow
    {
        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="node">The node in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        public DbGraph(DbTable<TRoot> node, DbTable<T1> relatedNode)
            : base((DbNode)node, (DbNode)relatedNode)
        { }
    }

    /// <summary>
    /// Represents a graph of a root node.
    /// </summary>
    /// <typeparam name="TRoot">The type of the root node.</typeparam>
    /// <typeparam name="T1">The type of the second node.</typeparam>
    /// <typeparam name="T2">The type of the third node.</typeparam>
    public class DbGraph<TRoot, T1, T2> : DbGraph<TRoot>
        where TRoot : DbRow
        where T1 : DbRow
        where T2 : DbRow
    {
        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="node">The node in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbTable<TRoot> node, DbGraph<T1, T2> relatedGraph)
            : base((DbNode)node, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The node in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        internal DbGraph(DbGraph<TRoot, T1> graph, DbTable<T2> relatedNode)
            : base((IGraph)graph, (DbNode)relatedNode)
        { }
    }

    /// <summary>
    /// Represents a graph of a root node.
    /// </summary>
    /// <typeparam name="TRoot">The type of the root node.</typeparam>
    /// <typeparam name="T1">The type of the second node.</typeparam>
    /// <typeparam name="T2">The type of the third node.</typeparam>
    /// <typeparam name="T3">The type of the fourth node.</typeparam>
    public class DbGraph<TRoot, T1, T2, T3> : DbGraph<TRoot>
        where TRoot : DbRow
        where T1 : DbRow
        where T2 : DbRow
        where T3 : DbRow
    {
        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="node">The node in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbTable<TRoot> node, DbGraph<T1, T2, T3> relatedGraph)
            : base((DbNode)node, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1> graph, DbGraph<T2, T3> relatedGraph)
            : base((IGraph)graph, relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The node in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2> graph, DbTable<T3> relatedNode)
            : base((IGraph)graph, (DbNode)relatedNode)
        { }
    }

    /// <summary>
    /// Represents a graph of a root node.
    /// </summary>
    /// <typeparam name="TRoot">The type of the root node.</typeparam>
    /// <typeparam name="T1">The type of the second node.</typeparam>
    /// <typeparam name="T2">The type of the third node.</typeparam>
    /// <typeparam name="T3">The type of the fourth node.</typeparam>
    /// <typeparam name="T4">The type of the fifth node.</typeparam>
    public class DbGraph<TRoot, T1, T2, T3, T4> : DbGraph<TRoot>
        where TRoot : DbRow
        where T1 : DbRow
        where T2 : DbRow
        where T3 : DbRow
        where T4 : DbRow
    {
        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="node">The node in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbTable<TRoot> node, DbGraph<T1, T2, T3, T4> relatedGraph)
            : base((DbNode)node, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1> graph, DbGraph<T2, T3, T4> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2> graph, DbGraph<T3, T4> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The node in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2, T3> graph, DbTable<T4> relatedNode)
            : base((IGraph)graph, (DbNode)relatedNode)
        { }
    }

    /// <summary>
    /// Represents a graph of a root node.
    /// </summary>
    /// <typeparam name="TRoot">The type of the root node.</typeparam>
    /// <typeparam name="T1">The type of the second node.</typeparam>
    /// <typeparam name="T2">The type of the third node.</typeparam>
    /// <typeparam name="T3">The type of the fourth node.</typeparam>
    /// <typeparam name="T4">The type of the fifth node.</typeparam>
    /// <typeparam name="T5">The type of the sixth node.</typeparam>
    public class DbGraph<TRoot, T1, T2, T3, T4, T5> : DbGraph<TRoot>
        where TRoot : DbRow
        where T1 : DbRow
        where T2 : DbRow
        where T3 : DbRow
        where T4 : DbRow
        where T5 : DbRow
    {
        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="node">The node in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbTable<TRoot> node, DbGraph<T1, T2, T3, T4, T5> relatedGraph)
            : base((DbNode)node, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1> graph, DbGraph<T2, T3, T4, T5> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2> graph, DbGraph<T3, T4, T5> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2, T3> graph, DbGraph<T4, T5> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The node in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2, T3, T4> graph, DbTable<T5> relatedNode)
            : base((IGraph)graph, (DbNode)relatedNode)
        { }
    }

    /// <summary>
    /// Represents a graph of a root node.
    /// </summary>
    /// <typeparam name="TRoot">The type of the root node.</typeparam>
    /// <typeparam name="T1">The type of the second node.</typeparam>
    /// <typeparam name="T2">The type of the third node.</typeparam>
    /// <typeparam name="T3">The type of the fourth node.</typeparam>
    /// <typeparam name="T4">The type of the fifth node.</typeparam>
    /// <typeparam name="T5">The type of the sixth node.</typeparam>
    /// <typeparam name="T6">The type of the seventh node.</typeparam>
    public class DbGraph<TRoot, T1, T2, T3, T4, T5, T6> : DbGraph<TRoot>
        where TRoot : DbRow
        where T1 : DbRow
        where T2 : DbRow
        where T3 : DbRow
        where T4 : DbRow
        where T5 : DbRow
        where T6 : DbRow
    {
        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="node">The node in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbTable<TRoot> node, DbGraph<T1, T2, T3, T4, T5, T6> relatedGraph)
            : base((DbNode)node, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1> graph, DbGraph<T2, T3, T4, T5, T6> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2> graph, DbGraph<T3, T4, T5, T6> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2, T3> graph, DbGraph<T4, T5, T6> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2, T3, T4> graph, DbGraph<T5, T6> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The node in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2, T3, T4, T5> graph, DbTable<T6> relatedNode)
            : base((IGraph)graph, (DbNode)relatedNode)
        { }
    }

    /// <summary>
    /// Represents a graph of a root node.
    /// </summary>
    /// <typeparam name="TRoot">The type of the root node.</typeparam>
    /// <typeparam name="T1">The type of the second node.</typeparam>
    /// <typeparam name="T2">The type of the third node.</typeparam>
    /// <typeparam name="T3">The type of the fourth node.</typeparam>
    /// <typeparam name="T4">The type of the fifth node.</typeparam>
    /// <typeparam name="T5">The type of the sixth node.</typeparam>
    /// <typeparam name="T6">The type of the seventh node.</typeparam>
    /// <typeparam name="T7">The type of the eighth node.</typeparam>
    public class DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7> : DbGraph<TRoot>
        where TRoot : DbRow
        where T1 : DbRow
        where T2 : DbRow
        where T3 : DbRow
        where T4 : DbRow
        where T5 : DbRow
        where T6 : DbRow
        where T7 : DbRow
    {
        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="node">The node in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbTable<TRoot> node, DbGraph<T1, T2, T3, T4, T5, T6, T7> relatedGraph)
            : base((DbNode)node, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1> graph, DbGraph<T2, T3, T4, T5, T6, T7> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2> graph, DbGraph<T3, T4, T5, T6, T7> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2, T3> graph, DbGraph<T4, T5, T6, T7> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2, T3, T4> graph, DbGraph<T5, T6, T7> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2, T3, T4, T5> graph, DbGraph<T6, T7> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The node in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2, T3, T4, T5, T6> graph, DbTable<T7> relatedNode)
            : base((IGraph)graph, (DbNode)relatedNode)
        { }
    }

    /// <summary>
    /// Represents a graph of a root node.
    /// </summary>
    /// <typeparam name="TRoot">The type of the root node.</typeparam>
    /// <typeparam name="T1">The type of the second node.</typeparam>
    /// <typeparam name="T2">The type of the third node.</typeparam>
    /// <typeparam name="T3">The type of the fourth node.</typeparam>
    /// <typeparam name="T4">The type of the fifth node.</typeparam>
    /// <typeparam name="T5">The type of the sixth node.</typeparam>
    /// <typeparam name="T6">The type of the seventh node.</typeparam>
    /// <typeparam name="T7">The type of the eighth node.</typeparam>
    /// <typeparam name="T8">The type of the ninth node.</typeparam>
    public class DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7, T8> : DbGraph<TRoot>
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
        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="node">The node in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbTable<TRoot> node, DbGraph<T1, T2, T3, T4, T5, T6, T7, T8> relatedGraph)
            : base((DbNode)node, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1> graph, DbGraph<T2, T3, T4, T5, T6, T7, T8> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2> graph, DbGraph<T3, T4, T5, T6, T7, T8> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2, T3> graph, DbGraph<T4, T5, T6, T7, T8> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2, T3, T4> graph, DbGraph<T5, T6, T7, T8> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2, T3, T4, T5> graph, DbGraph<T6, T7, T8> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The graph in relation.</param>
        /// <param name="relatedGraph">The related graph.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2, T3, T4, T5, T6> graph, DbGraph<T7, T8> relatedGraph)
            : base((IGraph)graph, (IGraph)relatedGraph)
        { }

        /// <summary>
        /// Initializes a new instance of the graph.
        /// </summary>
        /// <param name="graph">The node in relation.</param>
        /// <param name="relatedNode">The related node.</param>
        internal DbGraph(DbGraph<TRoot, T1, T2, T3, T4, T5, T6, T7> graph, DbTable<T8> relatedNode)
            : base((IGraph)graph, (DbNode)relatedNode)
        { }
    }
}
