#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Reflection;
using System.Data;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {

        /// <summary>
        /// Executes a stored procedure or SQL batch.
        /// </summary>
        /// <param name="procOrBatch">Is a stored procedure or SQL batch code.</param>
        public static Result ExecGo(ExecArgument procOrBatch)
        {
            return Call<Result>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (procOrBatch == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "procOrBatch", Wall.Text.Method.ExecGo);
                }

                var root = new d();
                var cpass = PassChainer.Create(root, procOrBatch);
                var connectable = Reader.GetConnectable(ca, cpass);
                return Reader.LoadAll(connectable);
            });
        }

        /// <summary>
        /// <para>Executes a stored procedure or SQL batch.</para>
        /// </summary>
        /// <param name="procOrBatch">Is a stored procedure or SQL batch code.</param>
        public static Result<T> ExecGo<T>(ExecArgument procOrBatch)

        {
            return Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (procOrBatch == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "procOrBatch", Wall.Text.Method.ExecGo);
                }

                var root = new d();
                var cpass = PassChainer.Create(root, procOrBatch);
                var connectable = Reader.GetConnectable(ca, cpass);
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
        /// <param name="procOrBatch">Is a stored procedure or SQL batch.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public static Async ExecGoAsync(ExecArgument procOrBatch, Action<Result> onCompleted = null)
        {
            return Call<Async>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (procOrBatch == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "procOrBatch", Wall.Text.Method.ExecGoAsync);
                }

                var root = new d();
                var cpass = PassChainer.Create(root, procOrBatch);
                var connectable = Reader.GetConnectable(ca, cpass);
                connectable.OnAsyncCompleted = onCompleted;
                return Reader.LoadAllAsync(connectable);
            });
        }

        /// <summary>
        /// Executes a stored procedure or SQL batch asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the result set that is returned by the execution.</typeparam>
        /// <param name="procOrBatch">Is a stored procedure or SQL batch.</param>
        /// <param name="onCompleted">A delegate method that is called when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public static Async<Result<T>> ExecGoAsync<T>(ExecArgument procOrBatch, Action<Result<T>> onCompleted = null)

        {
            return Call<Async<Result<T>>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (procOrBatch == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "procOrBatch", ".ExecGoAsync");
                }

                var root = new d();
                var cpass = PassChainer.Create(root, procOrBatch);
                var connectable = Reader.GetConnectable(ca, cpass);
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


    }
}