#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// Provides an interface for asynchronous operation.
    /// </summary>
    public interface IAsync
    {
        /// <summary>
        /// Wait until the asynchronous operation completes.
        /// </summary>
        Result Await();

        /// <summary>
        /// Cancel then asynchronous operation.
        /// </summary>
        bool Cancel();
    }

    /// <summary>
    /// Provides a generic interface for asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the data class.</typeparam>
    public interface IAsync<T> where T : DbRow
    {
        /// <summary>
        /// Wait until the asynchronous operation completes.
        /// </summary>
        Result<T> Await();

        /// <summary>
        /// Cancel then asynchronous operation.
        /// </summary>
        bool Cancel();
    }
}
