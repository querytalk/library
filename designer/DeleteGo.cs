#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Reflection;
using System.Collections.Generic;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {

        /// <summary>
        /// Deletes the specified row in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to delete.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        public static Result<T> DeleteGo<T>(T row, bool forceMirroring = true)
            where T : DbRow
        {
            return Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (row == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "row", Wall.Text.Method.DeleteGo);
                }

                return Crud.DeleteGo<T>(ca, row, forceMirroring, null);
            });
        }

        /// <summary>
        /// Deletes the specified rows in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="rows">The rows to delete.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        public static Result<T> DeleteRowsGo<T>(IEnumerable<T> rows, bool forceMirroring = true)
            where T : DbRow
        {
            return Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (rows == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "row", Wall.Text.Method.DeleteRowsGo);
                }

                return Crud.DeleteRowsGo(ca, (IEnumerable<T>)rows, forceMirroring, null);
            });
        }

        /// <summary>
        /// Deletes the specified row in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to delete.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public static Async<Result<T>> DeleteGoAsync<T>(T row, bool forceMirroring = true, Action<Result<T>> onCompleted = null)
            where T : DbRow
        {
            return Call<Async<Result<T>>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (row == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "row", Wall.Text.Method.DeleteGoAsync);
                }

                return Crud.DeleteGoAsync<T>(ca, row, forceMirroring, null, onCompleted);
            });
        }

        /// <summary>
        /// Deletes the specified row in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to delete.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public static Async<Result<T>> DeleteGoAsync<T>(T row, Action<Result<T>> onCompleted)
            where T : DbRow
        {
            return Call<Async<Result<T>>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (row == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "row", Wall.Text.Method.DeleteGoAsync);
                }

                return Crud.DeleteGoAsync<T>(ca, row, true, null, onCompleted);
            });
        }

    }
}