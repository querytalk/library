#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// Exposes the properties with basic information about the execution result.
    /// </summary>
    internal interface IAsyncStatus
    {
        /// <summary>
        /// Status of an asynchronous operation.
        /// </summary>
        AsyncStatus AsyncStatus { get; }

        /// <summary>
        /// The exception object if the operation has faulted.
        /// </summary>
        QueryTalkException Exception { get; }
    }
}
