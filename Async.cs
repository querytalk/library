#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Provides the object of an asynchronous operation. (This class has no public constructor.)
    /// </summary>
    public class Async : IAsyncStatus, IDisposable
    {
        /// <summary>
        /// The returning data.
        /// </summary>
        public dynamic Result { get; internal set; }

        private bool _success;
        /// <summary>
        /// True if the operation is successfully completed.
        /// </summary>
        public bool Success
        {
            get
            {
                return _success || AsyncStatus == AsyncStatus.Completed;
            }
            set
            {
                _success = value;
            }
        }

        /// <summary>
        /// The executive SQL code.
        /// </summary>
        public string Sql
        {
            get
            {
                return Connectable.Sql;
            }
        }

        #region IDisposable

        /// <summary>
        /// Releases unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
 
        /// <summary>
        /// A Finalizer method.
        /// </summary>
        ~Async()
        {
            Dispose(false);
        }

        /// <summary>
        /// Releases unmanaged and, optionally, managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_taskSource != null)
                {
                    _taskSource.Dispose();
                    _taskSource = null;
                }
            }
        }

        #endregion

        #region Internal

        internal SqlCommand Command { get; set; }           

        // SQL request async
        internal IAsyncResult AsyncResult1 { get; set; }    

        // reader async
        internal IAsyncResult AsyncResult2 { get; set; }    

        internal Connectable Connectable { get; set; }  

        private CancellationTokenSource _taskSource;
   
        // cancellation task token of the asynchronous operation
        private CancellationToken _taskToken;

        // true if .EndSync method has been called
        private bool _ended { get; set; }

        private object _locker = new object();

        #endregion

        #region Constructors

        internal Async(Connectable connectable)
        {
            Connectable = connectable;
        }

        internal Async(Connectable connectable, SqlCommand command)
        {
            Connectable = connectable;
            Command = command;
        }

        #endregion

        #region BeginAsync/EndAsync

        private void _beginAsync(Connectable connectable, AsyncCallback callback)
        {
            SqlConnection cn = new SqlConnection(connectable.ConnectionStringAsync);
            cn.Open();
            Importer.ExecuteBulkInsert(connectable, cn);
            SqlCommand cmd = new SqlCommand(connectable.Sql, cn);
            cmd.CommandTimeout = connectable.CommandTimeout;
            Command = cmd;

            AsyncResult1 = cmd.BeginExecuteReader(callback, this, CommandBehavior.CloseConnection);
        }

        // common begin async code 
        internal void BeginAsync(Connectable connectable, AsyncCallback callback)
        {
            _taskSource = new CancellationTokenSource();
            _taskToken = _taskSource.Token;
            
            // disallow async operations inside transactions (onlt TE is permitted)
            if (Transaction.Current != null && !Connectable.IsTesting)
            {
                throw new QueryTalkException("Async.BeginAsync", QueryTalkExceptionType.InvalidAsyncOperation, null, Text.Method.GoAsync);
            }

            // testing environment inside transaction => sync
            //   We need to make TE synchronous in order to be able to use the same ambient transaction with ease without need to 
            //   impose our own transaction management using dependent transactions which is not easy to adjust to all needs.
            if (Transaction.Current != null && Connectable.IsTesting)
            {
                _beginAsync(connectable, callback);
                return;
            }

            // no ambient transaction => start task
            //   We use multi-threading here in order to avoid blocking of the connection .Open method.
            Task.Factory.StartNew((o) =>
            {
                try
                {
                    _beginAsync(connectable, callback);
                }
                catch (QueryTalkException ex)
                {
                    SetResultException(ex);
                    EndAsync();
                }
                catch (System.Exception ex)
                {
                    SetClrException(ex, "Async.OnSqlRequestComplete", connectable, Text.Method.GoAsync);
                    EndAsync();
                }
            }, _taskToken);

        }

        // ends the asynchronous operation
        internal virtual void EndAsync()
        {
            lock (_locker)
            {
                if (_ended || AsyncStatus == QueryTalk.AsyncStatus.Canceled)
                {
                    return;
                }

                SetCompleted();

                _ended = true;

                // execute client action
                if (Connectable.HasOnAsyncCompleted)
                {
                    try
                    {
                        ((Action<Result>)Connectable.OnAsyncCompleted)((Result)Result);
                    }
                    catch (System.Exception ex)
                    {
                        SetClrException(ex, "Async.Await", Connectable, "OnAsyncCompleted");
                        throw Exception;
                    }
                }
            }
        }

        #endregion

        #region Await/Cancel

        /// <summary>
        /// Wait until the asynchronous operation completes.
        /// </summary>
        public Result Await()
        {
            // Check and exit, if the processing already finished.
            //   Lock is needed in order to access to the AsyncStatus AFTER the .EndAsync
            //   method is completed. If we access while .EndSync is being executed,
            //   we can read AsyncStatus just in the moment when it was changed to Completed
            //   and before the client code is executed which would leave the execution of  
            //   the client code outside the synchronization.
            lock (_locker)
            {
                if (AsyncStatus != AsyncStatus.Running)
                {
                    return (Result)Result;
                }
            }

            TryThrowException();

            int i = 0;
            if (AsyncResult1 == null)
            {
                // await until the connection is resolved 
                while (AsyncResult1 == null && i++ < Admin.Async1Timeout && !HasError)
                {
                    Thread.Sleep(1);
                }
            }

            TryThrowException();

            if (AsyncResult1 == null)
            {
                throw new QueryTalkException("Async.Await", QueryTalkExceptionType.Await1Failed, null, Text.Method.Await);
            }

            // the connection has been established successfully 
            // and .BeginExecuteReader has been called:
            if (AsyncResult1 != null)
            {
                // await until the SQL server finishes its job and the loader begins
                if (!AsyncResult1.IsCompleted)
                {
                    AsyncResult1.AsyncWaitHandle.WaitOne();
                }

                TryThrowException();

                // There is a small time gap between the SQL server execution operation and the loading operation (reader),
                // so we have to properly handle this time gap using:
                int j = 0;
                while (AsyncResult2 == null && j++ < Admin.Async2Timeout && !HasError)
                {
                    Thread.Sleep(1); 
                }

                TryThrowException();

                if (AsyncResult2 == null)
                {
                    throw new QueryTalkException("Async.Await", QueryTalkExceptionType.Await2Failed, null, Text.Method.Await);
                }

                if (AsyncResult2 != null && !AsyncResult2.IsCompleted)
                {
                    AsyncResult2.AsyncWaitHandle.WaitOne();
                    _asyncStatus = AsyncStatus.Completed;
                }

                TryThrowException();
            }
            else if (AsyncResult2 != null && !AsyncResult2.IsCompleted)
            {
                AsyncResult2.AsyncWaitHandle.WaitOne();

                TryThrowException();

                _asyncStatus = AsyncStatus.Completed;
            }

            TryThrowException();

            // ends the asynchronous operation 
            //   The call will wait until the .EndAsync method completes in case that 
            //   it was already called by the loader thread. If so, then it will immediately 
            //   exit because the ended flag will be set to true.
            EndAsync();

            TryThrowException();

            return (Result)Result;
        }

        /// <summary>
        /// Cancel the asynchronous operation.
        /// </summary>
        public bool Cancel()
        {
            try
            {
                lock (_locker)
                {
                    if (AsyncStatus != AsyncStatus.Running)
                    {
                        return false;
                    }
                }

                _asyncStatus = AsyncStatus.Canceled;

                if (Command != null && AsyncResult1 != null && !AsyncResult1.IsCompleted)
                {
                    Command.Cancel();
                }

                // important:
                //   Remove the connection from the pool in order to close all uncomitted transactions (rollback).
                if (Command != null && Command.Connection != null)
                {
                    SqlConnection.ClearPool(Command.Connection);
                }

                Connectable.AsyncCanceled = true;

                if (_taskSource != null)
                {
                    _taskSource.Cancel();
                }

                return true;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_taskSource != null)
                {
                    _taskSource.Dispose();
                }
            }
        }

        #endregion

        #region IAsyncStatus

        private AsyncStatus _asyncStatus = AsyncStatus.Running;

        /// <summary>
        /// Status of an asynchronous operation.
        /// </summary>
        public AsyncStatus AsyncStatus
        {
            get
            {
                if (HasError)
                {
                    return AsyncStatus.Faulted;   
                }
                else
                {
                    return _asyncStatus;
                }
            }
            internal set
            {
                _asyncStatus = value;
            }
        }

        internal void SetCompleted()
        {
            _asyncStatus = QueryTalk.AsyncStatus.Completed;
            Result.AsyncStatus = QueryTalk.AsyncStatus.Completed;
        }

        private QueryTalkException _exception;

        /// <summary>
        /// The exception object if the operation has faulted.
        /// </summary>
        public QueryTalkException Exception
        {
            get
            {
                return _exception;
            }
        }

        internal bool HasError
        {
            get
            {
                return Exception != null;
            }
        }

        #endregion

        #region Exception

        internal void TryThrowException()
        {
            if (HasError && Exception != null)
            {
                _asyncStatus = QueryTalk.AsyncStatus.Faulted;
                
                if (Exception.ClrException != null)
                {
                    throw Exception.ClrException;
                }

                throw Exception;
            }
        }

        internal virtual bool SetResultException(QueryTalkException ex)
        {
            if (AsyncStatus != AsyncStatus.Canceled)
            {
                _exception = ex;
                if (Result == null)
                {
                    Result = new Result(Connectable, ex);
                }
                else
                {
                    Result.Exception = ex;
                }
                return true;
            }
            else
            {
                if (Result == null)
                {
                    Result = new Result(Connectable);
                }

                Result.AsyncStatus = AsyncStatus.Canceled;
            }
            return false;
        }

        internal void SetClrException(
            System.Exception ex,
            string exceptionCreator,
            Connectable connectable,
            string method)
        {
            if (connectable.AsyncCanceled)
            {
                AsyncStatus = AsyncStatus.Canceled;
                Result = new Result(connectable);
                Result.AsyncStatus = AsyncStatus.Canceled;
                return;
            }

            QueryTalkException exception = new QueryTalkException(exceptionCreator, QueryTalkExceptionType.ClrException, ((IName)connectable).Name,
                method, null);
            exception.ClrException = ex;
            SetResultException(exception);
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("QueryTalk.Async | AsyncStatus = {0}", AsyncStatus);
        }

        #endregion

    }

    /// <summary>
    /// Represents the object of an asynchronous operation.
    /// </summary>
    /// <typeparam name="T">The type of the returning data.</typeparam>
    public sealed class Async<T> : Async
        
    {
        private T _result;

        /// <summary>
        /// The returning data.
        /// </summary>
        public new T Result
        {
            get
            {
                return _result;
            }
            internal set
            {
                _result = value;
                ((Async)this).Result = value;   // important!
            }
        }

        private bool _ended { get; set; }

        private object _locker = new object();

        internal Async(Connectable connectable)
            : base(connectable)
        { }

        internal static Async<T> CreateDefault<U>()
            where U : DbRow
        {
            var async = new Async<T>(null);
            async.Result = (T)(object)(new Result<U>(null));
            async.AsyncStatus = AsyncStatus.Completed;
            return async;
        }

        internal override bool SetResultException(QueryTalkException ex)
        {
            if (AsyncStatus != AsyncStatus.Canceled)
            {
                base.SetResultException(ex);
                ((dynamic)Result).Exception = ex;
                return true;
            }
            else
            {
                ((dynamic)Result).AsyncStatus = AsyncStatus.Canceled;
            }
            return false;
        }

        internal new void EndAsync()
        {
            lock (_locker)
            {
                if (_ended || AsyncStatus == QueryTalk.AsyncStatus.Canceled)
                {
                    return;
                }

                SetCompleted();

                _ended = true;

                if (Connectable.HasOnAsyncCompleted)
                {
                    try
                    {
                        ((Action<T>)Connectable.OnAsyncCompleted)(this.Result);
                    }
                    catch (System.Exception ex)
                    {
                        SetClrException(ex, "Async.Await", Connectable, "OnAsyncCompleted");
                        throw Exception;
                    }
                }
            }
        }

        /// <summary>
        /// Wait until the asynchronous operation completes.
        /// </summary>
        public new T Await()
        {
            lock (_locker)
            {
                if (AsyncStatus != AsyncStatus.Running)
                {
                    return Result;
                }
            }

            TryThrowException();

            int i = 0;
            if (AsyncResult1 == null)
            {
                while (AsyncResult1 == null && i++ < Admin.Async1Timeout && !HasError)
                {
                    Thread.Sleep(1);
                }
            }

            TryThrowException();

            if (AsyncResult1 == null)
            {
                throw new QueryTalkException("Async.Await", QueryTalkExceptionType.Await1Failed, null, Text.Method.Await);
            }

            if (AsyncResult1 != null)
            {
                if (!AsyncResult1.IsCompleted)
                {
                    AsyncResult1.AsyncWaitHandle.WaitOne();
                }

                TryThrowException();

                int j = 0;
                while (AsyncResult2 == null && j++ < Admin.Async2Timeout && !HasError)
                {
                    Thread.Sleep(1);    
                }

                TryThrowException();

                if (AsyncResult2 == null)
                {
                    throw new QueryTalkException("Async.Await", QueryTalkExceptionType.Await2Failed, null, Text.Method.Await);
                }

                if (AsyncResult2 != null && !AsyncResult2.IsCompleted)
                {
                    AsyncResult2.AsyncWaitHandle.WaitOne();
                    AsyncStatus = AsyncStatus.Completed;
                }

                TryThrowException();
            }
            else if (AsyncResult2 != null && !AsyncResult2.IsCompleted)
            {
                AsyncResult2.AsyncWaitHandle.WaitOne();

                TryThrowException();

                AsyncStatus = AsyncStatus.Completed;
            }

            TryThrowException();

            EndAsync();

            TryThrowException();

            return Result;
        }

    }
}
