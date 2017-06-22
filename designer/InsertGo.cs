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
        /// Inserts the specified row in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to insert.</param>
        /// <param name="identityInsert">If true, then the explicit value can be inserted into the identity column of a table.</param>
        public static Result<T> InsertGo<T>(T row, bool identityInsert = false)
            where T : DbRow
        {
            return Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (row == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "row", Wall.Text.Method.InsertGo);
                }

                return Crud.InsertGo<T>(ca, row, identityInsert, null);
            });
        }

        /// <summary>
        /// Inserts the specified rows in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="rows">The rows to insert.</param>
        /// <param name="identityInsert">If true, then the explicit value can be inserted into the identity column of a table.</param>
        public static Result<T> InsertRowsGo<T>(IEnumerable<T> rows, bool identityInsert = false)
            where T : DbRow
        {
            return Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (rows == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "row", Wall.Text.Method.InsertRowsGo);
                }

                return Crud.InsertRowsGo(ca, (IEnumerable<T>)rows, identityInsert, null);
            });
        }

        /// <summary>
        /// Inserts the specified row in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to insert.</param>
        /// <param name="identityInsert">If true, then the explicit value can be inserted into the identity column of a table.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public static Async<Result<T>> InsertGoAsync<T>(T row, bool identityInsert = false, Action<Result<T>> onCompleted = null)
            where T : DbRow
        {
            return Call<Async<Result<T>>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (row == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "row", Wall.Text.Method.InsertGoAsync);
                }

                return Crud.InsertGoAsync<T>(ca, row, identityInsert, null, onCompleted);
            });
        }

        /// <summary>
        /// Inserts the specified row in the database asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to insert.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public static Async<Result<T>> InsertGoAsync<T>(T row, Action<Result<T>> onCompleted)
            where T : DbRow
        {
            return Call<Async<Result<T>>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (row == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "row", Wall.Text.Method.InsertGoAsync);
                }

                return Crud.InsertGoAsync<T>(ca, row, false, null, onCompleted);
            });
        }

    }
}