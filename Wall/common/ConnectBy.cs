#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Represents the object that stores the connection data. (Not intended for public use.)
    /// </summary>
    public class ConnectBy : IConnectable
    {
        private Designer _root;

        #region IConnectable

        private ConnectionKey _connectionKey;
        ConnectionKey IConnectable.ConnectionKey
        {
            get
            {
                return _connectionKey;
            }
        }

        void IConnectable.SetConnectionKey(ConnectionKey connectionKey)
        {
            _connectionKey = connectionKey;
        }

        // not needed
        void IConnectable.ResetConnectionKey()
        { }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ConnectBy()
        { }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="root">The root of the designer.</param>
        /// <param name="connectionKey">The connection key.</param>
        internal ConnectBy(Designer root, ConnectionKey connectionKey)
        {
            _root = root;
            _connectionKey = connectionKey;
        }

        #endregion

        #region Go

        /// <summary>
        /// Executes the multiple SQL batches in a single connection. The batches should be separated by the GO instruction. 
        /// </summary>
        /// <param name="sql">The SQL code.</param>
        public void Go(string sql)
        {
            PublicInvoker.Call(Assembly.GetCallingAssembly(), (ca) =>
            {
                Reader.Go(ca, sql, this);
            });
        }

        #endregion

        #region ExecGo

        /// <summary>
        /// Executes a stored procedure or SQL batch.
        /// </summary>
        /// <param name="procOrBatch">Is a stored procedure or SQL batch.</param>
        public Result ExecGo(ExecArgument procOrBatch)
        {
            return PublicInvoker.Call<Result>(Assembly.GetCallingAssembly(), (ca) => 
            {
                var cpass = PassChainer.Create(_root, procOrBatch);
                var connectable = Reader.GetConnectable(ca, cpass, this);
                return Reader.LoadAll(connectable);
            });
        }

        /// <summary>
        /// Executes a stored procedure or SQL batch asynchronously.
        /// </summary>
        /// <param name="procOrBatch">Is a stored procedure or SQL batch.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public Async ExecGoAsync(ExecArgument procOrBatch, Action<Result> onCompleted = null)
        {
            return PublicInvoker.Call<Async>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var cpass = PassChainer.Create(_root, procOrBatch);
                var connectable = Reader.GetConnectable(ca, cpass, this);
                connectable.OnAsyncCompleted = onCompleted;
                return Reader.LoadAllAsync(connectable);
            });
        }

        /// <summary>
        /// Executes a stored procedure or SQL batch.
        /// </summary>
        /// <typeparam name="T">The type of the result set that is returned by the execution.</typeparam>
        /// <param name="procOrBatch">Is a stored procedure or SQL batch.</param>
        public Result<T> ExecGo<T>(ExecArgument procOrBatch)
            
        {
            return PublicInvoker.Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var cpass = PassChainer.Create(_root, procOrBatch);
                var connectable = Reader.GetConnectable(ca, cpass, this);

                Result<T> result;

                if (typeof(T) == typeof(DataTable))
                {
                    result = Reader.LoadDataTable<T>(connectable);
                }
                else
                {
                    result = Reader.LoadTable<T>(connectable, null);
                }

                return result;
            });
        }

        /// <summary>
        /// Executes a stored procedure or SQL batch asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the result set that is returned by the execution.</typeparam>
        /// <param name="procOrBatch">Is a stored procedure or SQL batch.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public Async<Result<T>> ExecGoAsync<T>(ExecArgument procOrBatch, Action<Result<T>> onCompleted = null)
            
        {
            return PublicInvoker.Call<Async<Result<T>>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var cpass = PassChainer.Create(_root, procOrBatch);
                var connectable = Reader.GetConnectable(ca, cpass, this);
                connectable.OnAsyncCompleted = onCompleted;

                if (typeof(T) == typeof(DataTable))
                {
                    return Reader.LoadDataTableAsync<T>(connectable);
                }
                else
                {
                    return Reader.LoadTableAsync<T>(connectable, null);
                }
            });
        }

        #endregion

        #region ReloadGo

        /// <summary>
        /// Reloads the specified row from the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to reload.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        public Result<T> ReloadGo<T>(T row, bool forceMirroring = true)
            where T : DbRow
        {
            return PublicInvoker.Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) => 
                Crud.ReloadGo<T>(ca, row, forceMirroring, this));
        }

        #endregion

        #region DeleteGo

        /// <summary>
        /// Deletes the specified row in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to delete.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        public Result<T> DeleteGo<T>(T row, bool forceMirroring = true)
            where T : DbRow
        {
            return PublicInvoker.Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) => 
                Crud.DeleteGo<T>(ca, row, forceMirroring, this));
        }

        /// <summary>
        /// Deletes the specified row in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to delete.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public Async<Result<T>> DeleteGoAsync<T>(T row, bool forceMirroring = true, Action<Result<T>> onCompleted = null)
            where T : DbRow
        {
            return PublicInvoker.Call<Async<Result<T>>>(Assembly.GetCallingAssembly(), (ca) => 
                Crud.DeleteGoAsync<T>(ca, row, forceMirroring, this, onCompleted));
        }

        /// <summary>
        /// Deletes the specified row in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to delete.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public Async<Result<T>> DeleteGoAsync<T>(T row, Action<Result<T>> onCompleted )
            where T : DbRow
        {
            return DeleteGoAsync(row, true, onCompleted);
        }

        /// <summary>
        /// Deletes the specified rows in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="rows">The rows to delete.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        public Result<T> DeleteRowsGo<T>(IEnumerable<T> rows, bool forceMirroring = true)
            where T : DbRow
        {
            return PublicInvoker.Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) => 
                Crud.DeleteRowsGo(ca, rows, forceMirroring, this));
        }

        #endregion

        #region UpdateGo

        /// <summary>
        /// Updates the specified row in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to update.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        public Result<T> UpdateGo<T>(T row, bool forceMirroring = true)
            where T : DbRow
        {
            return PublicInvoker.Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) =>
                Crud.UpdateGo<T>(ca, row, forceMirroring, this));
        }

        /// <summary>
        /// Updates the specified row in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to update.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public Async<Result<T>> UpdateGoAsync<T>(T row, bool forceMirroring = true, Action<Result<T>> onCompleted = null)
            where T : DbRow
        {
            return PublicInvoker.Call<Async<Result<T>>>(Assembly.GetCallingAssembly(), (ca) => 
                Crud.UpdateGoAsync<T>(ca, row, forceMirroring, this, onCompleted));
        }

        /// <summary>
        /// Updates the specified row in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to update.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public Async<Result<T>> UpdateGoAsync<T>(T row, Action<Result<T>> onCompleted)
            where T : DbRow
        {
            return UpdateGoAsync(row, true, onCompleted);
        }

        /// <summary>
        /// Updates the specified rows in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="rows">The rows to update.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        public Result<T> UpdateRowsGo<T>(IEnumerable<T> rows, bool forceMirroring = true)
            where T : DbRow
        {
            return PublicInvoker.Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) => 
                Crud.UpdateRowsGo(ca, rows, forceMirroring, this));
        }

        #endregion

        #region InsertGo

        /// <summary>
        /// Inserts the specified row in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to insert.</param>
        /// <param name="identityInsert">If true, then the explicit value can be inserted into the identity column of a table.</param>
        public Result<T> InsertGo<T>(T row, bool identityInsert = false)
            where T : DbRow
        {
            return PublicInvoker.Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) => 
                Crud.InsertGo<T>(ca, row, identityInsert, this));
        }

        /// <summary>
        /// Inserts the specified row in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to insert.</param>
        /// <param name="identityInsert">If true, then the explicit value can be inserted into the identity column of a table.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public Async<Result<T>> InsertGoAsync<T>(T row, bool identityInsert = false, Action<Result<T>> onCompleted = null)
            where T : DbRow
        {
            return PublicInvoker.Call<Async<Result<T>>>(Assembly.GetCallingAssembly(), (ca) => 
                Crud.InsertGoAsync<T>(ca, row, identityInsert, this, onCompleted));
        }

        /// <summary>
        /// Inserts the specified row in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to insert.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public Async<Result<T>> InsertGoAsync<T>(T row, Action<Result<T>> onCompleted)
            where T : DbRow
        {
            return InsertGoAsync(row, false, onCompleted);
        }

        /// <summary>
        /// Inserts the specified rows in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="rows">The rows to insert.</param>
        /// <param name="identityInsert">If true, then the explicit value can be inserted into the identity column of a table.</param>
        public Result<T> InsertRowsGo<T>(IEnumerable<T> rows, bool identityInsert = false)
            where T : DbRow
        {
            return PublicInvoker.Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) => 
                Crud.InsertRowsGo(ca, rows, identityInsert, this));
        }

        #endregion

        #region DeleteCascadeGo

        /// <summary>
        /// Deletes the row with the associated children rows.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to delete.</param>
        /// <param name="maxLevels">Specifies the maximum number of cascading levels that can be included in the deletion. If more levels than maxLevels are needed to complete the cascade deletion, an exception is thrown.</param>
        public void DeleteCascadeGo<T>(T row, int maxLevels = 5)
            where T : DbRow
        {
            PublicInvoker.Call(Assembly.GetCallingAssembly(), (ca) => 
                Crud.DeleteCascadeGo<T>(ca, row, maxLevels, this));
        }

        #endregion

        #region InsertCascadeGo

        /// <summary>
        /// Inserts the row with the associated children rows.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to insert.</param>
        public void InsertCascadeGo<T>(T row)
            where T : DbRow
        {
            PublicInvoker.Call(Assembly.GetCallingAssembly(), (ca) =>
                Crud.InsertCascadeGo<T>(ca, row, this));    
        }

        #endregion

        #region BulkInsertGo

        /// <summary>
        /// Executes a bulk insert.
        /// </summary>
        /// <param name="data">The data to be inserted.</param>
        /// <param name="table">Is a target table where the data is inserted.</param>
        public void BulkInsertGo(DataTable data, TableArgument table)
        {
            PublicInvoker.Call(Assembly.GetCallingAssembly(), (ca) => 
                Importer.ExecuteBulkInsert(ca, data, table, this));         
        }

        /// <summary>
        /// Executes a bulk insert.
        /// </summary>
        /// <param name="data">The data to be inserted.</param>
        /// <param name="table">Is a target table where the data is inserted.</param>
        public void BulkInsertGo(Result<DataTable> data, TableArgument table)
        {
            PublicInvoker.Call(Assembly.GetCallingAssembly(), (ca) => 
                Importer.ExecuteBulkInsert(ca, data.ToDataTable(), table, this));
        }

        /// <summary>
        /// Executes a bulk insert.
        /// </summary>
        /// <param name="rows">The data to be inserted.</param>
        public void BulkInsertGo<T>(IEnumerable<T> rows)
            where T : DbRow
        {
            PublicInvoker.Call(Assembly.GetCallingAssembly(), (ca) =>
                Importer.ExecuteBulkInsert(ca, rows, this));
        }

        #endregion

    }
}
