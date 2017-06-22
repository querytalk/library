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

        #region Go

        /// <summary>
        /// Executes the batch returning a dynamic result with all the result sets from the data source.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static Result Go(this IGo prev)
        {
            return PublicInvoker.Call<Result>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                return Reader.LoadAll(connectable);
            }); 
        }

        #endregion

        #region Go<T>

        /// <summary>
        /// Executes the batch returning a single strongly typed result set.
        /// </summary>
        /// <typeparam name="T">Is the type of the result set. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="prev">A predecessor object.</param>
        public static Result<T> Go<T>(this IGo prev)
        {
            return PublicInvoker.Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
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
        /// Executes the batch returning a single strongly typed result set. The result can be fetched through the callback method.
        /// </summary>
        /// <typeparam name="T">Is the type of the enumerated row. Use dynamic or object type for dynamic result set. DataTable type is not supported.</typeparam>
        /// <param name="rowHandler">Is a delegate method that processes the row of a type T as an input.</param>
        /// <param name="prev">A predecessor object.</param>
        public static Result<T> Go<T>(this IGo prev, Action<T> rowHandler) 
        {
            return PublicInvoker.Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                var result = Reader.LoadTable<T>(connectable, rowHandler);
                return result;
            });
        }

        #endregion

        #region Go<T1..T10>

        /// <summary>
        /// Executes the batch returning 2 strongly typed result sets.
        /// </summary>
        /// <typeparam name="T1">Is the type of the Table1 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T2">Is the type of the Table2 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="prev">A predecessor object.</param>
        public static Result<T1, T2> Go<T1, T2>(this IGo prev)
        {
            return PublicInvoker.Call<Result<T1, T2>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                return Reader.LoadMany<T1, T2>(connectable);
            });
        }

        /// <summary>
        /// Executes the batch returning 3 strongly typed result sets.
        /// </summary>
        /// <typeparam name="T1">Is the type of the Table1 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T2">Is the type of the Table2 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T3">Is the type of the Table3 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="prev">A predecessor object.</param>
        public static Result<T1, T2, T3> Go<T1, T2, T3>(this IGo prev)
        {
            return PublicInvoker.Call<Result<T1, T2, T3>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                return Reader.LoadMany<T1, T2, T3>(connectable);
            });
        }

        /// <summary>
        /// Executes the batch returning 4 strongly typed result sets.
        /// </summary>
        /// <typeparam name="T1">Is the type of the Table1 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T2">Is the type of the Table2 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T3">Is the type of the Table3 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T4">Is the type of the Table4 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="prev">A predecessor object.</param>
        public static Result<T1, T2, T3, T4> Go<T1, T2, T3, T4>(this IGo prev)
        {
            return PublicInvoker.Call<Result<T1, T2, T3, T4>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                return Reader.LoadMany<T1, T2, T3, T4>(connectable);
            });
        }

        /// <summary>
        /// Executes the batch returning 5 strongly typed result sets.
        /// </summary>
        /// <typeparam name="T1">Is the type of the Table1 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T2">Is the type of the Table2 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T3">Is the type of the Table3 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T4">Is the type of the Table4 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T5">Is the type of the Table5 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="prev">A predecessor object.</param>
        public static Result<T1, T2, T3, T4, T5> Go<T1, T2, T3, T4, T5>(this IGo prev)
        {
            return PublicInvoker.Call<Result<T1, T2, T3, T4, T5>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                return Reader.LoadMany<T1, T2, T3, T4, T5>(connectable);
            });
        }

        /// <summary>
        /// Executes the batch returning 6 strongly typed result sets.
        /// </summary>
        /// <typeparam name="T1">Is the type of the Table1 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T2">Is the type of the Table2 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T3">Is the type of the Table3 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T4">Is the type of the Table4 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T5">Is the type of the Table5 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T6">Is the type of the Table6 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="prev">A predecessor object.</param>
        public static Result<T1, T2, T3, T4, T5, T6> Go<T1, T2, T3, T4, T5, T6>(this IGo prev)
        {
            return PublicInvoker.Call<Result<T1, T2, T3, T4, T5, T6>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                return Reader.LoadMany<T1, T2, T3, T4, T5, T6>(connectable);
            });
        }

        /// <summary>
        /// Executes the batch returning 7 strongly typed result sets.
        /// </summary>
        /// <typeparam name="T1">Is the type of the Table1 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T2">Is the type of the Table2 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T3">Is the type of the Table3 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T4">Is the type of the Table4 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T5">Is the type of the Table5 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T6">Is the type of the Table6 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T7">Is the type of the Table7 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="prev">A predecessor object.</param>
        public static Result<T1, T2, T3, T4, T5, T6, T7> Go<T1, T2, T3, T4, T5, T6, T7>(this IGo prev)
        {
            return PublicInvoker.Call<Result<T1, T2, T3, T4, T5, T6, T7>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                return Reader.LoadMany<T1, T2, T3, T4, T5, T6, T7>(connectable);
            });
        }

        /// <summary>
        /// Executes the batch returning 8 strongly typed result sets.
        /// </summary>
        /// <typeparam name="T1">Is the type of the Table1 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T2">Is the type of the Table2 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T3">Is the type of the Table3 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T4">Is the type of the Table4 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T5">Is the type of the Table5 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T6">Is the type of the Table6 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T7">Is the type of the Table7 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <typeparam name="T8">Is the type of the Table8 in the result. Use dynamic or object type for dynamic result set.</typeparam>
        /// <param name="prev">A predecessor object.</param>
        public static Result<T1, T2, T3, T4, T5, T6, T7, T8> Go<T1, T2, T3, T4, T5, T6, T7, T8>(this IGo prev)
        {
            return PublicInvoker.Call<Result<T1, T2, T3, T4, T5, T6, T7, T8>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                return Reader.LoadMany<T1, T2, T3, T4, T5, T6, T7, T8>(connectable);
            });
        }

        /// <summary>
        /// Executes the batch returning 9 strongly typed result sets.
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
        /// <param name="prev">A predecessor object.</param>
        public static Result<T1, T2, T3, T4, T5, T6, T7, T8, T9> Go<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this IGo prev)
        {
            return PublicInvoker.Call<Result<T1, T2, T3, T4, T5, T6, T7, T8, T9>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                return Reader.LoadMany<T1, T2, T3, T4, T5, T6, T7, T8, T9>(connectable);
            });
        }

        #endregion

    }
}
