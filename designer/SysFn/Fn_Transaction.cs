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
        /// Evaluates to True if the current request has an active user transaction which is committable.
        /// </summary>
        public static Expression IsTransactionCommittable
        {
            get
            {
                return new Expression("XACT_STATE() = 1");
            }
        }

        /// <summary>
        /// Evaluates to True if the current request has an active user transaction which is uncommittable. If an error has occurred that has caused the transaction to be classified as an uncommittable transaction, the request cannot commit the transaction or roll back to a savepoint; it can only request a full rollback of the transaction.
        /// </summary>
        public static Expression IsTransactionUncommittable
        {
            get
            {
                return new Expression("XACT_STATE() = -1");
            }
        }

        /// <summary>
        /// Evaluates to True if the current request has no active user transaction.
        /// </summary>
        public static Expression IsTransactionActive
        {
            get
            {
                return new Expression("XACT_STATE() <> 0");
            }
        }

        /// <summary>
        /// Evaluates to True if the @@TRANCOUNT function returns the number greater than zero.
        /// </summary>
        public static Expression IsAnyTransaction
        {
            get
            {
                return new Expression("@@TRANCOUNT > 0");
            }
        }

        /// <summary>
        /// Evaluates to True if the @@TRANCOUNT function returns a zero.
        /// </summary>
        public static Expression IsNoTransaction
        {
            get
            {
                return new Expression("@@TRANCOUNT = 0");
            }
        }

    }
}
