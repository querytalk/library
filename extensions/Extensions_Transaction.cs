#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {
        /// <summary>
        /// Controls the locking and row versioning behavior of Transact-SQL statements issued by a connection to SQL Server.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="isolationLevel">A type of the isolation level.</param>
        public static SetIsolationLevelChainer SetIsolationLevel(this IAny prev, Designer.IsolationLevel isolationLevel)
        {
            return new SetIsolationLevelChainer((Chainer)prev, isolationLevel);
        }

        /// <summary>
        /// Marks the starting point of an explicit, local transaction.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="nameOrVariable">Is the optional name assigned to the transaction or the name of a user-defined variable containing a valid transaction name.</param>
        public static BeginTransactionChainer BeginTransaction(this IAny prev, string nameOrVariable = null)
        {
            return new BeginTransactionChainer((Chainer)prev, nameOrVariable);
        }

        /// <summary>
        /// Sets a savepoint within a transaction.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="nameOrVariable">Is the name assigned to the savepoint or the name of a user-defined variable containing a valid savepoint name.</param>
        public static SaveTransactionChainer SaveTransaction(this IAny prev, string nameOrVariable)
        {
            return new SaveTransactionChainer((Chainer)prev, nameOrVariable);
        }

        /// <summary>
        /// Marks the end of a successful implicit or explicit transaction.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="nameOrVariable">Is the optional name assigned to the transaction or the name of a user-defined variable containing a valid transaction name. Is ignored by the SQL Server Database Engine.</param>
        public static CommitTransactionChainer CommitTransaction(this IAny prev, string nameOrVariable = null)
        {
            return new CommitTransactionChainer((Chainer)prev, nameOrVariable);
        }

        /// <summary>
        /// Rolls back an explicit or implicit transaction to the beginning of the transaction, or to a savepoint inside the transaction.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="nameOrVariable">Is the name assigned to the transaction or the name of a user-defined variable containing a valid transaction name.</param>
        public static RollbackTransactionChainer RollbackTransaction(this IAny prev, string nameOrVariable = null)
        {
            return new RollbackTransactionChainer((Chainer)prev, nameOrVariable);
        }
    }
}
