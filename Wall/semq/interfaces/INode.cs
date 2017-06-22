#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Reflection;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Provides access to the node from the DbRow object.
    /// </summary>
    public interface INode
    {
        /// <summary>
        /// A node to be accessed.
        /// </summary>
        DbNode Node { get; }

        /// <summary>
        /// A method that appends children data to the row object.
        /// </summary>
        /// <param name="client">A client assembly.</param>
        /// <param name="connectBy">A ConnectBy object holding the connection key.</param>
        void InsertGraph(Assembly client, ConnectBy connectBy);
    }

    /// <summary>
    /// Provides access to the database table from the DbRow object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INode<T>
        where T : DbRow
    {
        /// <summary>
        /// A database table to be accessed.
        /// </summary>
        DbTable<T> Node { get; }    
    }
}
