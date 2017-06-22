#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Data;
using System.Reflection;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates all public extension methods of the QueryTalk library.
    /// </summary>
    public static partial class Extensions
    {

        #region GoAsync

        /// <summary>
        /// Executes the batch asynchronously returning a dynamic result with all the result sets from the batch.
        /// </summary>
        /// <param name="onCompleted">Is a delegate method that is invoked when the asynchronous operation completes.</param>
        /// <param name="prev">A predecessor object.</param>
        public static Async GoAsync(this IGo prev, Action<Result> onCompleted = null)
        {
            return PublicInvoker.Call<Async>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                connectable.OnAsyncCompleted = onCompleted;
                return Reader.LoadAllAsync(connectable);
            });
        }

        #endregion

        #region GoAsync<T>

        /// <summary>
        /// Executes the batch asynchronously returning a single strongly typed result set.
        /// </summary>
        /// <typeparam name="T">Is the type of the result set. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="onCompleted">Is a delegate method that is invoked when the asynchronous operation completes.</param>
        /// <param name="prev">A predecessor object.</param>
        public static Async<Result<T>> GoAsync<T>(this IGo prev, Action<Result<T>> onCompleted = null) 
        {
            return PublicInvoker.Call<Async<Result<T>>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                connectable.OnAsyncCompleted = (dynamic)onCompleted;
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

        #region GoAsync<T1..T10>

        /// <summary>
        /// Executes the batch asynchronously returning 2 strongly typed result sets.
        /// </summary>
        /// <typeparam name="T1">Is the type of the Table1 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T2">Is the type of the Table2 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="onCompleted">Is a delegate method that is invoked when the asynchronous operation completes.</param>
        /// <param name="prev">A predecessor object.</param>
        public static Async<Result<T1, T2>> GoAsync<T1, T2>(
            this IGo prev, Action<Result<T1, T2>> onCompleted = null)
        {
            return PublicInvoker.Call<Async<Result<T1, T2>>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                connectable.OnAsyncCompleted = onCompleted;
                return Reader.LoadManyAsync<T1, T2>(connectable);
            });
        }

        /// <summary>
        /// Executes the batch asynchronously returning 3 strongly typed result sets.
        /// </summary>
        /// <typeparam name="T1">Is the type of the Table1 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T2">Is the type of the Table2 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T3">Is the type of the Table3 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="onCompleted">Is a delegate method that is invoked when the asynchronous operation completes.</param>
        /// <param name="prev">A predecessor object.</param>
        public static Async<Result<T1, T2, T3>> GoAsync<T1, T2, T3>(
            this IGo prev, Action<Result<T1, T2, T3>> onCompleted = null)
        {
            return PublicInvoker.Call<Async<Result<T1, T2, T3>>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                connectable.OnAsyncCompleted = onCompleted;
                return Reader.LoadManyAsync<T1, T2, T3>(connectable);
            });
        }

        /// <summary>
        /// Executes the batch asynchronously returning 4 strongly typed result sets.
        /// </summary>
        /// <typeparam name="T1">Is the type of the Table1 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T2">Is the type of the Table2 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T3">Is the type of the Table3 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T4">Is the type of the Table4 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="onCompleted">Is a delegate method that is invoked when the asynchronous operation completes.</param>
        /// <param name="prev">A predecessor object.</param>
        public static Async<Result<T1, T2, T3, T4>> GoAsync<T1, T2, T3, T4>(
            this IGo prev, Action<Result<T1, T2, T3, T4>> onCompleted = null)
        {
            return PublicInvoker.Call<Async<Result<T1, T2, T3, T4>>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                connectable.OnAsyncCompleted = onCompleted;
                return Reader.LoadManyAsync<T1, T2, T3, T4>(connectable);
            });
        }

        /// <summary>
        /// Executes the batch asynchronously returning 5 strongly typed result sets.
        /// </summary>
        /// <typeparam name="T1">Is the type of the Table1 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T2">Is the type of the Table2 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T3">Is the type of the Table3 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T4">Is the type of the Table4 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T5">Is the type of the Table5 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="onCompleted">Is a delegate method that is invoked when the asynchronous operation completes.</param>
        /// <param name="prev">A predecessor object.</param>
        public static Async<Result<T1, T2, T3, T4, T5>> GoAsync<T1, T2, T3, T4, T5>(
            this IGo prev, Action<Result<T1, T2, T3, T4, T5>> onCompleted = null)
        {
            return PublicInvoker.Call<Async<Result<T1, T2, T3, T4, T5>>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                connectable.OnAsyncCompleted = onCompleted;
                return Reader.LoadManyAsync<T1, T2, T3, T4, T5>(connectable);
            });
        }

        /// <summary>
        /// Executes the batch asynchronously returning 6 strongly typed result sets.
        /// </summary>
        /// <typeparam name="T1">Is the type of the Table1 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T2">Is the type of the Table2 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T3">Is the type of the Table3 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T4">Is the type of the Table4 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T5">Is the type of the Table5 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T6">Is the type of the Table6 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="onCompleted">Is a delegate method that is invoked when the asynchronous operation completes.</param>
        /// <param name="prev">A predecessor object.</param>
        public static Async<Result<T1, T2, T3, T4, T5, T6>> GoAsync<T1, T2, T3, T4, T5, T6>(
            this IGo prev, Action<Result<T1, T2, T3, T4, T5, T6>> onCompleted = null)
        {
            return PublicInvoker.Call<Async<Result<T1, T2, T3, T4, T5, T6>>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                connectable.OnAsyncCompleted = onCompleted;
                return Reader.LoadManyAsync<T1, T2, T3, T4, T5, T6>(connectable);
            });
        }

        /// <summary>
        /// Executes the batch asynchronously returning 7 strongly typed result sets.
        /// </summary>
        /// <typeparam name="T1">Is the type of the Table1 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T2">Is the type of the Table2 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T3">Is the type of the Table3 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T4">Is the type of the Table4 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T5">Is the type of the Table5 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T6">Is the type of the Table6 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T7">Is the type of the Table7 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="onCompleted">Is a delegate method that is invoked when the asynchronous operation completes.</param>
        /// <param name="prev">A predecessor object.</param>
        public static Async<Result<T1, T2, T3, T4, T5, T6, T7>> GoAsync<T1, T2, T3, T4, T5, T6, T7>(
            this IGo prev, Action<Result<T1, T2, T3, T4, T5, T6, T7>> onCompleted = null)
        {
            return PublicInvoker.Call<Async<Result<T1, T2, T3, T4, T5, T6, T7>>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                connectable.OnAsyncCompleted = onCompleted;
                return Reader.LoadManyAsync<T1, T2, T3, T4, T5, T6, T7>(connectable);
            });
        }

        /// <summary>
        /// Executes the batch asynchronously returning 8 strongly typed result sets.
        /// </summary>
        /// <typeparam name="T1">Is the type of the Table1 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T2">Is the type of the Table2 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T3">Is the type of the Table3 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T4">Is the type of the Table4 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T5">Is the type of the Table5 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T6">Is the type of the Table6 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T7">Is the type of the Table7 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T8">Is the type of the Table8 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="onCompleted">Is a delegate method that is invoked when the asynchronous operation completes.</param>
        /// <param name="prev">A predecessor object.</param>
        public static Async<Result<T1, T2, T3, T4, T5, T6, T7, T8>> GoAsync<T1, T2, T3, T4, T5, T6, T7, T8>(
            this IGo prev, Action<Result<T1, T2, T3, T4, T5, T6, T7, T8>> onCompleted = null)
        {
            return PublicInvoker.Call<Async<Result<T1, T2, T3, T4, T5, T6, T7, T8>>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                connectable.OnAsyncCompleted = onCompleted;
                return Reader.LoadManyAsync<T1, T2, T3, T4, T5, T6, T7, T8>(connectable);
            });
        }

        /// <summary>
        /// Executes the batch asynchronously returning 9 strongly typed result sets.
        /// </summary>
        /// <typeparam name="T1">Is the type of the Table1 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T2">Is the type of the Table2 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T3">Is the type of the Table3 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T4">Is the type of the Table4 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T5">Is the type of the Table5 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T6">Is the type of the Table6 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T7">Is the type of the Table7 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T8">Is the type of the Table8 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T9">Is the type of the Table9 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="onCompleted">Is a delegate method that is invoked when the asynchronous operation completes.</param>
        /// <param name="prev">A predecessor object.</param>
        public static Async<Result<T1, T2, T3, T4, T5, T6, T7, T8, T9>> GoAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this IGo prev, Action<Result<T1, T2, T3, T4, T5, T6, T7, T8, T9>> onCompleted = null)
        {
            return PublicInvoker.Call<Async<Result<T1, T2, T3, T4, T5, T6, T7, T8, T9>>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                connectable.OnAsyncCompleted = onCompleted;
                return Reader.LoadManyAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(connectable);
            });
        }

        #endregion

    }
}
