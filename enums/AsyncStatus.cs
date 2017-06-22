#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk
{
    /// <summary>
    /// Status of an asynchronous operation.
    /// </summary>
    public enum AsyncStatus : int
    {
        /// <summary>
        /// The operation is still running.
        /// </summary>
        Running = 0,

        /// <summary>
        /// The operation is completed.
        /// </summary>
        Completed = 1,

        /// <summary>
        /// The operation has failed.
        /// </summary>
        Faulted = 2,

        /// <summary>
        /// The operation has been canceled.
        /// </summary>
        Canceled = 3,

        /// <summary>
        /// No asynchronous operation.
        /// </summary>
        None = 5,

    }
}
