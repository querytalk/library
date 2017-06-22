#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// An object that implements ISemantic can create a query in a free querying notation which means that its semantic has the capability to be translated and understood in the SQL world.
    /// </summary>
    public interface ISemantic
    {
        /// <summary>
        /// <para>Gets a subject of a semantic sentence.</para>
        /// <para>(Not intended for public use.)</para>
        /// </summary>
        DbNode Subject { get; }

        /// <summary>
        /// <para>Gets a parent subject of a semantic sentence.</para>
        /// <para>(Not intended for public use.)</para>
        /// </summary>
        DbNode RootSubject { get; set; }

        /// <summary>
        /// <para>Returns true if the node is part of a semantical query.</para>
        /// <para>(Not intended for public use.)</para>
        /// </summary>
        bool IsQuery { get; }

        /// <summary>
        /// <para>A method which translates the semantic query into the SQL query returning a chain object of a free designer.</para>
        /// <para>(Not intended for public use.)</para>
        /// </summary>
        /// <param name="context">A SEMQ context created on the initial call.</param>
        /// <param name="predecessor">A predecessor node that precedes the current node in the node chain or a subject in a predicate sentence.</param>
        Chainer Translate(SemqContext context, DbNode predecessor);
    }
}
