#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        #region Reload

        /// <summary>
        /// Reloads the specified row from the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to reload.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        public static Result<T> ReloadGo<T>(this T row, bool forceMirroring = true)
            where T : DbRow
        {
            if (row == null)
            {
                throw new QueryTalkException("extensions.ReloadGo<T>", QueryTalkExceptionType.ArgumentNull, "row = null", Text.Method.ReloadGo);
            }

            return Crud.ReloadGo<T>(Assembly.GetCallingAssembly(), row, forceMirroring, null);
        }

        #endregion

        #region Insert

        /// <summary>
        /// Inserts the specified row in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to insert.</param>
        /// <param name="identityInsert">If true, then the explicit value can be inserted into the identity column of a table.</param>
        public static Result<T> InsertGo<T>(this T row, bool identityInsert = false)
            where T : DbRow
        {
            if (row == null)
            {
                throw new QueryTalkException("extensions.InsertGo<T>", QueryTalkExceptionType.ArgumentNull, "row = null", Text.Method.InsertGo);
            }

            return Crud.InsertGo<T>(Assembly.GetCallingAssembly(), row, identityInsert, null);
        }

        /// <summary>
        /// Inserts the specified row in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to insert.</param>
        /// <param name="identityInsert">If true, then the explicit value can be inserted into the identity column of a table.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public static Async<Result<T>> InsertGoAsync<T>(this T row, bool identityInsert = false, Action<Result<T>> onCompleted = null)
            where T : DbRow
        {
            if (row == null)
            {
                throw new QueryTalkException("extensions.InsertGoAsync<T>", QueryTalkExceptionType.ArgumentNull, "row = null", Text.Method.InsertGoAsync);
            }

            return Crud.InsertGoAsync<T>(Assembly.GetCallingAssembly(), row, identityInsert, null, onCompleted);
        }

        /// <summary>
        /// Inserts the specified row in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to insert.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public static Async<Result<T>> InsertGoAsync<T>(this T row, Action<Result<T>> onCompleted)
            where T : DbRow
        {
            if (row == null)
            {
                throw new QueryTalkException("extensions.InsertGoAsync<T>", QueryTalkExceptionType.ArgumentNull, "row = null", Text.Method.InsertGoAsync);
            }

            return Crud.InsertGoAsync<T>(Assembly.GetCallingAssembly(), row, false, null, onCompleted);
        }

        /// <summary>
        /// Inserts the row with the associated children rows.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to insert.</param>
        public static Result<T> InsertCascadeGo<T>(this T row)
            where T : DbRow
        {
            if (row == null)
            {
                throw new QueryTalkException("extensions.InsertCascadeGo<T>", QueryTalkExceptionType.ArgumentNull, "row = null", Text.Method.InsertCascadeGo);
            }

            return Crud.InsertCascadeGo<T>(Assembly.GetCallingAssembly(), row, null);
        }

        /// <summary>
        /// Inserts the specified rows in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="rows">The rows to insert.</param>
        /// <param name="identityInsert">If true, then the explicit value can be inserted into the identity column of a table.</param>
        public static Result<T> InsertRowsGo<T>(this IEnumerable<T> rows, bool identityInsert = true)
            where T : DbRow
        {
            if (rows == null)
            {
                throw new QueryTalkException("extensions.InsertRowsGo<T>", QueryTalkExceptionType.ArgumentNull, "rows = null", Text.Method.InsertRowsGo);
            }

            return Crud.InsertRowsGo<T>(Assembly.GetCallingAssembly(), rows, identityInsert, null);
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the specified row in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to update.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        public static Result<T> UpdateGo<T>(this T row, bool forceMirroring = true)
            where T : DbRow
        {
            if (row == null)
            {
                throw new QueryTalkException("extensions.UpdateGo<T>", QueryTalkExceptionType.ArgumentNull, "row = null", Text.Method.UpdateGo);
            }

            return Crud.UpdateGo<T>(Assembly.GetCallingAssembly(), row, forceMirroring, null);
        }

        /// <summary>
        /// Updates the specified row in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to update.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public static Async<Result<T>> UpdateGoAsync<T>(this T row, bool forceMirroring = true, Action<Result<T>> onCompleted = null)
            where T : DbRow
        {
            if (row == null)
            {
                throw new QueryTalkException("extensions.UpdateGoAsync<T>", QueryTalkExceptionType.ArgumentNull, "row = null", Text.Method.UpdateGoAsync);
            }

            return Crud.UpdateGoAsync<T>(Assembly.GetCallingAssembly(), row, forceMirroring, null, onCompleted);
        }

        /// <summary>
        /// Updates the specified row in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to update.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public static Async<Result<T>> UpdateGoAsync<T>(this T row, Action<Result<T>> onCompleted)
            where T : DbRow
        {
            if (row == null)
            {
                throw new QueryTalkException("extensions.UpdateGoAsync<T>", QueryTalkExceptionType.ArgumentNull, "row = null", Text.Method.UpdateGoAsync);
            }

            return Crud.UpdateGoAsync<T>(Assembly.GetCallingAssembly(), row, true, null, onCompleted);
        }

        /// <summary>
        /// Updates the specified rows in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="rows">The rows to update.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        public static Result<T> UpdateRowsGo<T>(this IEnumerable<T> rows, bool forceMirroring = true)
            where T : DbRow
        {
            if (rows == null)
            {
                throw new QueryTalkException("extensions.UpdateRowsGo<T>", QueryTalkExceptionType.ArgumentNull, "rows = null", Text.Method.UpdateRowsGo);
            }

            return Crud.UpdateRowsGo<T>(Assembly.GetCallingAssembly(), rows, forceMirroring, null);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the specified row in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to delete.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        public static Result<T> DeleteGo<T>(this T row, bool forceMirroring = true)
            where T : DbRow
        {
            if (row == null)
            {
                throw new QueryTalkException("extensions.DeleteGo<T>", QueryTalkExceptionType.ArgumentNull, "row = null", Text.Method.DeleteGo);
            }

            return Crud.DeleteGo<T>(Assembly.GetCallingAssembly(), row, forceMirroring, null);
        }

        /// <summary>
        /// Deletes the specified row in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to delete.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public static Async<Result<T>> DeleteGoAsync<T>(this T row, bool forceMirroring = true, Action<Result<T>> onCompleted = null)
            where T : DbRow
        {
            if (row == null)
            {
                throw new QueryTalkException("extensions.DeleteGoAsync<T>", QueryTalkExceptionType.ArgumentNull, "row = null", Text.Method.DeleteGoAsync);
            }

            return Crud.DeleteGoAsync<T>(Assembly.GetCallingAssembly(), row, forceMirroring, null, onCompleted);
        }

        /// <summary>
        /// Deletes the specified row in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to delete.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public static Async<Result<T>> DeleteGoAsync<T>(this T row, Action<Result<T>> onCompleted)
            where T : DbRow
        {
            if (row == null)
            {
                throw new QueryTalkException("extensions.DeleteGoAsync<T>", QueryTalkExceptionType.ArgumentNull, "row = null", Text.Method.DeleteGoAsync);
            }

            return Crud.DeleteGoAsync<T>(Assembly.GetCallingAssembly(), row, true, null, onCompleted);
        }

        /// <summary>
        /// Deletes the specified rows in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="rows">The rows to delete.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        public static Result<T> DeleteRowsGo<T>(this IEnumerable<T> rows, bool forceMirroring = true)
            where T : DbRow
        {
            if (rows == null)
            {
                throw new QueryTalkException("extensions.DeleteRowsGo<T>", QueryTalkExceptionType.ArgumentNull, "row = null", Text.Method.DeleteRowsGo);
            }

            return Crud.DeleteRowsGo<T>(Assembly.GetCallingAssembly(), rows, forceMirroring, null);
        }

        /// <summary>
        /// Deletes the row with the associated children rows.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to delete.</param>
        /// <param name="maxLevels">Specifies the maximum number of cascading levels that can be included in the deletion. If more levels than maxLevels are needed to complete the cascade deletion, an exception is thrown.</param>
        public static Result<T> DeleteCascadeGo<T>(this T row, int maxLevels = 5)
            where T : DbRow
        {
            // check null
            if (row == null)
            {
                throw new QueryTalkException("extensions.DeleteCascadeGo<T>", QueryTalkExceptionType.ArgumentNull, "row = null", Text.Method.DeleteCascadeGo);
            }

            return Crud.DeleteCascadeGo<T>(Assembly.GetCallingAssembly(), row, maxLevels, null);
        }

        #endregion

        #region BulkInsert

        /// <summary>
        /// Executes a bulk insert.
        /// </summary>
        /// <param name="rows">The data to be inserted.</param>
        public static Result<T> BulkInsertGo<T>(this IEnumerable<T> rows)
            where T : DbRow
        {
            if (rows == null)
            {
                throw new QueryTalkException("extensions.BulkInsertGo<T>", QueryTalkExceptionType.ArgumentNull, "rows = null", Text.Method.BulkInsertGo);
            }

            return Importer.ExecuteBulkInsert<T>(Assembly.GetCallingAssembly(), rows, null);
        }

        /// <summary>
        /// Executes a bulk insert.
        /// </summary>
        /// <param name="data">The data to be inserted.</param>
        /// <param name="table">Is a target table where the data is inserted.</param>
        public static Result BulkInsertGo(this DataTable data, TableArgument table)
        {
            if (data == null)
            {
                throw new QueryTalkException("extensions.BulkInsertGo<T>", QueryTalkExceptionType.ArgumentNull, "rows = null", Text.Method.BulkInsertGo);
            }

            return Importer.ExecuteBulkInsert(Assembly.GetCallingAssembly(), data, table, null);
        }

        /// <summary>
        /// Executes a bulk insert.
        /// </summary>
        /// <param name="data">The data to be inserted.</param>
        /// <param name="table">Is a target table where the data is inserted.</param>
        public static Result BulkInsertGo(this Result<DataTable> data, TableArgument table)
        {
            if (data == null)
            {
                throw new QueryTalkException("extensions.BulkInsertGo<T>", QueryTalkExceptionType.ArgumentNull, "result = null", Text.Method.BulkInsertGo);
            }

            return Importer.ExecuteBulkInsert(Assembly.GetCallingAssembly(), data.ToDataTable(), table, null);
        }

        #endregion

    }
}
