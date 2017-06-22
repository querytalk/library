#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;

namespace QueryTalk.Wall
{
    // Used only when SEMQ tries to find the intermediate node.
    /// <summary>
    /// Represents an object that is used to invoke the static constructor of a database node when appropriate.
    /// </summary>
    public sealed class NodeInvoker : Map
    {
        /// <summary>
        /// A delegate method whose call causes the node's static constructor to execute.
        /// </summary>
        public Action Invoke { get; private set; }

        /// <summary>
        /// Initializes an instance of the NodeInvoker class.
        /// </summary>
        /// <param name="id">Is a specified node identifier.</param>
        /// <param name="invokeMethod">Is a delegate method whose call causes the node's static constructor to execute.</param>
        public NodeInvoker(DB3 id, Action invokeMethod)
            : base(id)
        {
            Invoke = invokeMethod;
        }
    }

    /// <summary>
    /// Represents an object that is used to invoke the static constructor of a database node when appropriate.
    /// </summary>
    public class NodeInvoker<T, TMany> : Map
    {
        // identifier of a many-node
        internal DB3 ManyID { get; private set; }

        /// <summary>
        /// A delegate method whose call causes the node's static constructor to execute.
        /// </summary>
        public Action<IEnumerable<T>, IEnumerable<TMany>> Invoke { get; private set; }

        /// <summary>
        /// Initializes an instance of the NodeInvoker class.
        /// </summary>
        /// <param name="id">Is a specified node identifier.</param>
        /// <param name="manyID">Is a specified many-node identifier.</param>
        /// <param name="invokeMethod">Is a delegate method whose call causes the node's static constructor to execute.</param>
        public NodeInvoker(DB3 id, DB3 manyID, Action<IEnumerable<T>, IEnumerable<TMany>> invokeMethod)
            : base(id)
        {
            ManyID = manyID;
            Invoke = invokeMethod;
        }
    }
}
