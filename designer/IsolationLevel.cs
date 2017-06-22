#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {
        /// <summary>
        /// An isolation level defines the degree to which one transaction must be isolated from resource or data modifications made by other transactions.
        /// </summary>
        public enum IsolationLevel : int
        {
            /// <summary>
            /// Default isolation level of the SQL server (ReadCommitted). 
            /// </summary>
            Default = 0,

            /// <summary>
            /// Specifies that statements cannot read data that has been modified but not committed by other transactions.
            /// </summary>
            ReadCommitted = 1,

            /// <summary>
            /// Specifies that statements can read rows that have been modified by other transactions but not yet committed.
            /// </summary>
            ReadUncommitted = 2,

            /// <summary>
            /// Specifies that statements cannot read data that has been modified but not yet committed by other transactions and that no other transactions can modify data that has been read by the current transaction until the current transaction completes.
            /// </summary>
            RepeatableRead = 3,

            /// <summary>
            /// Specifies that data read by any statement in a transaction will be the transactionally consistent version of the data that existed at the start of the transaction. 
            /// </summary>
            Snapshot = 4,

            /// <summary>
            /// <para>Specifies the following:</para>
            /// <para>- Statements cannot read data that has been modified but not yet committed by other transactions.</para>
            /// <para>- No other transactions can modify data that has been read by the current transaction until the current transaction completes.</para>
            /// <para>- Other transactions cannot insert new rows with key values that would fall in the range of keys read by any statements in the current transaction until the current transaction completes.</para>
            /// </summary>
            Serializable = 5
        }
    }
}