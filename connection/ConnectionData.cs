#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Diagnostics;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents a connection data needed to establish the connection to a SQL Server database.
    /// </summary>
    public sealed class ConnectionData 
    {
        /// <summary>
        /// A connection string definition.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Specifies the user of an execution context of a batch. 
        /// </summary>
        public string ExecuteAsUser { get; set; }

        // default command timeout
        private int _commandTimeout = 30;
        /// <summary>
        /// A command timeout.
        /// </summary>
        public int CommandTimeout 
        {
            get
            {
                return _commandTimeout;
            }
            set
            {
                _commandTimeout = value;
            }
        }

        // default isolation level
        private QueryTalk.Designer.IsolationLevel _massDataOperationIsolationLevel = QueryTalk.Designer.IsolationLevel.ReadCommitted;
        /// <summary>
        /// The isolation level used in CRUD mass data operations.
        /// </summary>
        public QueryTalk.Designer.IsolationLevel MassDataOperationIsolationLevel
        {
            get
            {
                return _massDataOperationIsolationLevel;
            }
            set
            {
                _massDataOperationIsolationLevel = value;
            }
        }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ConnectionData class.
        /// </summary>
        /// <param name="connectionString">A connection string.</param>
        /// <param name="commandTimeout">A command timeout.</param>
        /// <param name="massDataOperationIsolationLevel">An isolation level of a CRUD mass data operation.</param>
        public ConnectionData(string connectionString, 
            int commandTimeout = 30,
            QueryTalk.Designer.IsolationLevel massDataOperationIsolationLevel = QueryTalk.Designer.IsolationLevel.ReadCommitted)
        {
            if (connectionString == null)
            {
                throw ArgumentUndefinedException();
            }

            ConnectionString = connectionString;
            _commandTimeout = commandTimeout;
            _massDataOperationIsolationLevel = massDataOperationIsolationLevel;
        }

        /// <summary>
        /// Initializes a new instance of the ConnectionData class.
        /// </summary>
        /// <param name="connectionString">A connection string.</param>
        public ConnectionData(System.String connectionString)
            : this(connectionString, 30, Designer.IsolationLevel.ReadCommitted)
        { }

        #endregion

        /// <summary>
        /// Sets the user of an execution context of a batch.
        /// </summary>
        /// <param name="executeAsUser"></param>
        /// <returns></returns>
        public ConnectionData SetExecuteAs(string executeAsUser)
        {
            ExecuteAsUser = executeAsUser;
            return this;
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ConnectionData type.
        /// </summary>
        /// <param name="connectionString">A connection string argument to convert.</param>
        public static implicit operator ConnectionData(System.String connectionString)
        {
            return new ConnectionData(connectionString);
        }

        private QueryTalkException ArgumentUndefinedException()
        {
            return new QueryTalkException(this, QueryTalkExceptionType.ArgumentNull, "connectionString = null", 
                Text.Method.SetConnection);
        }

    }
}
