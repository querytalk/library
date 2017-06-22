#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk
{
    public static partial class Extensions
    {
        /// <summary>
        /// Returns true if an asynchronous operation is completed.
        /// </summary>
        /// <param name="status">A status of an asynchronous operation.</param>
        public static bool IsCompleted(this AsyncStatus status)
        {
            return status == AsyncStatus.Completed;
        }

        /// <summary>
        /// Returns true if an asynchronous operation is running.
        /// </summary>
        /// <param name="status">A status of an asynchronous operation.</param>
        public static bool IsRunning(this AsyncStatus status)
        {
            return status == AsyncStatus.Running;
        }

        /// <summary>
        /// Returns true if an asynchronous operation has failed.
        /// </summary>
        /// <param name="status">A status of an asynchronous operation.</param>
        public static bool IsFaulted(this AsyncStatus status)
        {
            return status == AsyncStatus.Faulted;
        }

        /// <summary>
        /// Returns true if an asynchronous operation has been canceled.
        /// </summary>
        /// <param name="status">A status of an asynchronous operation.</param>
        public static bool IsCanceled(this AsyncStatus status)
        {
            return status == AsyncStatus.Canceled;
        }

    }
}
