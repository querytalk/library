#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// Handles the connection data.
    /// </summary>
    public interface IConnectable
    {
        /// <summary>
        /// Gets the connection key.
        /// </summary>
        ConnectionKey ConnectionKey { get; }

        /// <summary>
        /// Sets the connection key.
        /// </summary>
        /// <param name="connectionKey">Is a connection key to set.</param>
        void SetConnectionKey(ConnectionKey connectionKey);

        /// <summary>
        /// Resets the connection key.
        /// </summary>
        void ResetConnectionKey(); 
    }
}
