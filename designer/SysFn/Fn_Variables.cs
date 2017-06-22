#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {
        /// <summary>
        /// Returns the number of qualifying rows currently in the last cursor opened on the connection.
        /// </summary>
        public static SysFn CursorRows
        {
            get
            {
                return new SysFn("@@CURSOR_ROWS");
            }
        }

        /// <summary>
        /// Returns the error number for the last Transact-SQL statement executed.
        /// </summary>
        public static SysFn Error
        {
            get
            {
                return new SysFn("@@ERROR");
            }
        }

        /// <summary>
        /// Returns the status of the last cursor FETCH statement issued against any cursor currently opened by the connection.
        /// </summary>
        public static SysFn FetchStatus
        {
            get
            {
                return new SysFn("@@FETCH_STATUS");
            }
        }

        /// <summary>
        /// Returns the last-inserted identity value.
        /// </summary>
        public static SysFn Identity
        {
            get
            {
                return new SysFn("@@IDENTITY");
            }
        }

        /// <summary>
        /// Returns the number of rows affected by the last statement. If the number of rows is more than 2 billion, use ROWCOUNT_BIG function.
        /// </summary>
        public static SysFn RowCount
        {
            get
            {
                return new SysFn("@@ROWCOUNT");
            }
        }

        /// <summary>
        /// Returns the name of the local SQL Server.
        /// </summary>
        public static SysFn ServerName
        {
            get
            {
                return new SysFn("@@SERVERNAME");
            }
        }

        /// <summary>
        /// Returns the number of BEGIN TRANSACTION statements that have occurred on the current connection.
        /// </summary>
        public static SysFn TranCount
        {
            get
            {
                return new SysFn("@@TRANCOUNT");
            }
        }
    }
}
