#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Represents an object that is used to invoke the graph creator of the related node.
    /// </summary>
    public sealed class GraphInvoker
    {
        internal DB3 ForeignKeyID { get; private set; }

        internal DB3 ReferenceID { get; private set; }

        /// <summary>
        /// A delegate method whose call causes the node's static constructor to execute.
        /// </summary>
        public Action<IEnumerable, IEnumerable> Invoke { get; private set; }

        /// <summary>
        /// Initializes an instance of the NodeInvoker class.
        /// </summary>
        /// <param name="referenceID">Is a specified identifier of the predecessor node.</param>
        /// <param name="foreignKeyID">Is a specified identifier of the related node.</param>
        /// <param name="invokeMethod">Is a delegate method whose call causes the graph creator to execute.</param>
        public GraphInvoker(DB3 foreignKeyID, DB3 referenceID, Action<IEnumerable, IEnumerable> invokeMethod)
        {
            ForeignKeyID = foreignKeyID;
            ReferenceID = referenceID;
            Invoke = invokeMethod;
        }
    }
}
