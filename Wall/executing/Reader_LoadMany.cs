#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Data;
using System.Data.SqlClient;

namespace QueryTalk.Wall
{
    internal static partial class Reader
    {
        internal class TableSetLoaderArgs
        {
            internal System.Exception Exception { get; set; }   
            internal Type TableType { get; set; }            
            internal int TableIndex { get; set; }             

            internal TableSetLoaderArgs(System.Exception exception, Type tableType, int tableIndex)
            {
                Exception = exception;
                TableType = tableType;
                TableIndex = tableIndex;
            }
        }

        #region Sync

        #region Body

        private static void _LoadTableSetBody(
            Connectable connectable,
            Func<IDataReader, TableSetLoaderArgs> tableSetLoader,   
            Type currentTableType,
            out int returnValue)
        {
            int tableIndex = 0;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectable.ConnectionString))
                {
                    cn.Open();

                    Importer.ExecuteBulkInsert(connectable, cn);
                    using (SqlCommand cmd = new SqlCommand(connectable.Sql, cn))
                    {
                        cmd.CommandTimeout = connectable.CommandTimeout;
                        SqlDataReader reader = cmd.ExecuteReader();

                        try
                        {
                            var loaderException = tableSetLoader(reader);
                            if (loaderException != null)
                            {
                                currentTableType = loaderException.TableType;
                                tableIndex = loaderException.TableIndex;
                                throw loaderException.Exception;
                            }

                            returnValue = Reader.ReadOutputData(reader, connectable);
                        }
                        finally
                        {
                            reader.Close();
                        }
                    }
                }
            }
            catch (QueryTalkException ex)
            {
                ex.ObjectName = ((IName)connectable.Executable).Name;
                ex.Method = Text.Method.Go;
                throw;
            }
            catch (System.InvalidCastException)  // "Specified cast is not valid."
            {
                var mismatchData = Loader.AnalyseInvalidCastException(connectable, currentTableType, tableIndex);

                // throw exception with more accurate report
                if (mismatchData != null)
                {
                    throw Reader.TypeMismatchException(
                        ((IName)connectable.Executable).Name, Text.Method.Go, currentTableType, mismatchData, 
                        tableIndex);
                }
                // throw exception with less accurate report (not likely to happen)
                else
                {
                    throw Reader.TypeMismatchException(
                        ((IName)connectable.Executable).Name, Text.Method.Go, currentTableType, new Row<string, string, string>(),
                        tableIndex);
                }
            }
        }

        #endregion

        internal static Result<T1,T2> LoadMany<T1,T2>(Connectable connectable)
        {
            int returnValue;
            Type currentTableType = typeof(T1);
            var result = new Result<T1, T2>(connectable);
            int tableIndex = 0;

            Func<IDataReader, TableSetLoaderArgs> tableSetLoader = (reader) =>
                {
                    result.Table1 = ReadFirstOrNextTable<T1>(reader, connectable, ref tableIndex);
                    currentTableType = typeof(T2);
                    result.Table2 = ReadFirstOrNextTable<T2>(reader, connectable, ref tableIndex);
                    return null;
                };

            _LoadTableSetBody(connectable, tableSetLoader, currentTableType, out returnValue);
            result.ReturnValue = returnValue;
            return result;
        }

        internal static Result<T1, T2, T3> LoadMany<T1, T2, T3>(Connectable connectable)
        {
            int returnValue;
            var result = new Result<T1, T2, T3>(connectable);
            Type currentTableType = typeof(T1);
            int tableIndex = 0;

            Func<IDataReader, TableSetLoaderArgs> tableSetLoader = (reader) =>
            {
                result.Table1 = ReadFirstOrNextTable<T1>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T2);
                result.Table2 = ReadFirstOrNextTable<T2>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T3);
                result.Table3 = ReadFirstOrNextTable<T3>(reader, connectable, ref tableIndex);
                return null;
            };

            _LoadTableSetBody(connectable, tableSetLoader, currentTableType, out returnValue);
            result.ReturnValue = returnValue;
            return result;
        }

        internal static Result<T1, T2, T3, T4> LoadMany<T1, T2, T3, T4>(Connectable connectable)
        {
            int returnValue;
            var result = new Result<T1, T2, T3, T4>(connectable);
            Type currentTableType = typeof(T1);
            int tableIndex = 0;

            Func<IDataReader, TableSetLoaderArgs> tableSetLoader = (reader) =>
            {
                result.Table1 = ReadFirstOrNextTable<T1>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T2);
                result.Table2 = ReadFirstOrNextTable<T2>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T3);
                result.Table3 = ReadFirstOrNextTable<T3>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T4);
                result.Table4 = ReadFirstOrNextTable<T4>(reader, connectable, ref tableIndex);
                return null;
            };

            _LoadTableSetBody(connectable, tableSetLoader, currentTableType, out returnValue);
            result.ReturnValue = returnValue;
            return result;
        }

        internal static Result<T1, T2, T3, T4, T5> LoadMany<T1, T2, T3, T4, T5>(Connectable connectable)
        {
            int returnValue;
            var result = new Result<T1, T2, T3, T4, T5>(connectable);
            Type currentTableType = typeof(T1);
            int tableIndex = 0;

            Func<IDataReader, TableSetLoaderArgs> tableSetLoader = (reader) =>
            {
                result.Table1 = ReadFirstOrNextTable<T1>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T2);
                result.Table2 = ReadFirstOrNextTable<T2>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T3);
                result.Table3 = ReadFirstOrNextTable<T3>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T4);
                result.Table4 = ReadFirstOrNextTable<T4>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T5);
                result.Table5 = ReadFirstOrNextTable<T5>(reader, connectable, ref tableIndex);
                return null;
            };

            _LoadTableSetBody(connectable, tableSetLoader, currentTableType, out returnValue);
            result.ReturnValue = returnValue;
            return result;
        }

        internal static Result<T1, T2, T3, T4, T5, T6> LoadMany<T1, T2, T3, T4, T5, T6>(Connectable connectable)
        {
            int returnValue;
            var result = new Result<T1, T2, T3, T4, T5, T6>(connectable);
            Type currentTableType = typeof(T1);
            int tableIndex = 0;

            Func<IDataReader, TableSetLoaderArgs> tableSetLoader = (reader) =>
            {
                result.Table1 = ReadFirstOrNextTable<T1>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T2);
                result.Table2 = ReadFirstOrNextTable<T2>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T3);
                result.Table3 = ReadFirstOrNextTable<T3>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T4);
                result.Table4 = ReadFirstOrNextTable<T4>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T5);
                result.Table5 = ReadFirstOrNextTable<T5>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T6);
                result.Table6 = ReadFirstOrNextTable<T6>(reader, connectable, ref tableIndex);
                return null;
            };

            _LoadTableSetBody(connectable, tableSetLoader, currentTableType, out returnValue);
            result.ReturnValue = returnValue;
            return result;
        }

        internal static Result<T1, T2, T3, T4, T5, T6, T7> LoadMany<T1, T2, T3, T4, T5, T6, T7>(Connectable connectable)
        {
            int returnValue;
            var result = new Result<T1, T2, T3, T4, T5, T6, T7>(connectable);
            Type currentTableType = typeof(T1);
            int tableIndex = 0;

            Func<IDataReader, TableSetLoaderArgs> tableSetLoader = (reader) =>
            {
                result.Table1 = ReadFirstOrNextTable<T1>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T2);
                result.Table2 = ReadFirstOrNextTable<T2>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T3);
                result.Table3 = ReadFirstOrNextTable<T3>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T4);
                result.Table4 = ReadFirstOrNextTable<T4>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T5);
                result.Table5 = ReadFirstOrNextTable<T5>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T6);
                result.Table6 = ReadFirstOrNextTable<T6>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T7);
                result.Table7 = ReadFirstOrNextTable<T7>(reader, connectable, ref tableIndex);
                return null;
            };

            _LoadTableSetBody(connectable, tableSetLoader, currentTableType, out returnValue);
            result.ReturnValue = returnValue;
            return result;
        }

        internal static Result<T1, T2, T3, T4, T5, T6, T7, T8> LoadMany<T1, T2, T3, T4, T5, T6, T7, T8>(Connectable connectable)
        {
            int returnValue;
            var result = new Result<T1, T2, T3, T4, T5, T6, T7, T8>(connectable);
            Type currentTableType = typeof(T1);
            int tableIndex = 0;

            Func<IDataReader, TableSetLoaderArgs> tableSetLoader = (reader) =>
            {
                result.Table1 = ReadFirstOrNextTable<T1>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T2);
                result.Table2 = ReadFirstOrNextTable<T2>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T3);
                result.Table3 = ReadFirstOrNextTable<T3>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T4);
                result.Table4 = ReadFirstOrNextTable<T4>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T5);
                result.Table5 = ReadFirstOrNextTable<T5>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T6);
                result.Table6 = ReadFirstOrNextTable<T6>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T7);
                result.Table7 = ReadFirstOrNextTable<T7>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T8);
                result.Table8 = ReadFirstOrNextTable<T8>(reader, connectable, ref tableIndex);
                return null;
            };

            _LoadTableSetBody(connectable, tableSetLoader, currentTableType, out returnValue);
            result.ReturnValue = returnValue;
            return result;
        }

        internal static Result<T1, T2, T3, T4, T5, T6, T7, T8, T9> LoadMany<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Connectable connectable)
        {
            int returnValue;
            var result = new Result<T1, T2, T3, T4, T5, T6, T7, T8, T9>(connectable);
            Type currentTableType = typeof(T1);
            int tableIndex = 0;

            Func<IDataReader, TableSetLoaderArgs> tableSetLoader = (reader) =>
            {
                result.Table1 = ReadFirstOrNextTable<T1>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T2);
                result.Table2 = ReadFirstOrNextTable<T2>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T3);
                result.Table3 = ReadFirstOrNextTable<T3>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T4);
                result.Table4 = ReadFirstOrNextTable<T4>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T5);
                result.Table5 = ReadFirstOrNextTable<T5>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T6);
                result.Table6 = ReadFirstOrNextTable<T6>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T7);
                result.Table7 = ReadFirstOrNextTable<T7>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T8);
                result.Table8 = ReadFirstOrNextTable<T8>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T9);
                result.Table9 = ReadFirstOrNextTable<T9>(reader, connectable, ref tableIndex);
                return null;
            };

            _LoadTableSetBody(connectable, tableSetLoader, currentTableType, out returnValue);
            result.ReturnValue = returnValue;
            return result;
        }

        #endregion

        #region Async

        #region TableSet Body

        private static void _LoadTableSetBodyAsync<T>(
            Connectable connectable,
            Func<IDataReader, TableSetLoaderArgs> tableSetLoader,    // main loader delegate method
            Async<T> async,
            Type currentTableType) 
        {
            int loaderReturnValue = 0;
            int tableIndex = 0;

            // callback1: a method that is invoked when SQL processing has completed
            AsyncCallback onSqlRequestComplete = (iAsyncResult) =>
            {
                Async asyncBegin = null;
                SqlDataReader reader = null;

                // async2: data loader processing reader asynchronously
                var loader =
                    new Action<IDataReader>((readerLoader) =>
                    {
                        try
                        {
                            var loaderException = tableSetLoader(reader);
                            if (loaderException != null)
                            {
                                currentTableType = loaderException.TableType;
                                tableIndex = loaderException.TableIndex;
                                throw loaderException.Exception;
                            }

                            // load return value
                            loaderReturnValue = Reader.ReadOutputData(reader, connectable);
                            ((dynamic)async.Result).ReturnValue = loaderReturnValue;
                        }
                        catch (QueryTalkException ex)
                        {
                            if (async.SetResultException(ex))
                            {
                                async.Exception.ObjectName = ((IName)connectable.Executable).Name;
                                async.Exception.Method = Text.Method.GoAsync;
                            }
                        }
                        catch (System.InvalidCastException)  // "Specified cast is not valid."
                        {
                            var mismatchData = Loader.AnalyseInvalidCastException(connectable, currentTableType, tableIndex);

                            // throw exception with accurate report
                            if (mismatchData != null)
                            {
                                throw Reader.TypeMismatchException(
                                    ((IName)connectable.Executable).Name, Text.Method.GoAsync, currentTableType, mismatchData,
                                    tableIndex);
                            }
                            // throw exception with less accurate report (not likely to happen)
                            else
                            {
                                throw Reader.TypeMismatchException(
                                    ((IName)connectable.Executable).Name, Text.Method.GoAsync, currentTableType, new Row<string, string, string>(),
                                    tableIndex);
                            }
                        }
                        catch (System.Exception ex)
                        {
                            QueryTalkException exception = new QueryTalkException("Reader._LoadTableSetBodyAsync.Loader",
                                QueryTalkExceptionType.ClrException, ((IName)connectable.Executable).Name,
                                Text.Method.GoAsync, String.Format("table = {0}", currentTableType));
                            exception.ClrException = ex;
                            async.SetResultException(exception);
                        }
                        finally
                        {
                            if (readerLoader != null)
                            {
                                readerLoader.Close();
                            }
                        }
                    });

                try
                {
                    asyncBegin = (Async)iAsyncResult.AsyncState;          
                    reader = async.Command.EndExecuteReader(iAsyncResult);     

                    // -------------------------------------------------------------------------------------------
                    // here is a gap between two async calls: first is finished, the second is about to begin...
                    // -------------------------------------------------------------------------------------------

                    async.AsyncResult2 = loader.BeginInvoke(reader,
                        new AsyncCallback(
                            new Action<IAsyncResult>((iAsyncResult2) =>
                            {
                                try
                                {
                                    loader.EndInvoke(iAsyncResult2);
                                    async.EndAsync();
                                }
                                catch (QueryTalkException ex)
                                {
                                    ex.Method = Text.Method.GoAsync;
                                    async.SetResultException(ex);
                                    async.EndAsync();
                                }
                                catch (System.Exception ex)
                                {
                                    async.SetClrException(ex, "LoadTableSet.AsyncResult2", connectable, Text.Method.GoAsync);
                                    async.EndAsync();
                                }
                            })),
                        null);
                }
                catch (QueryTalkException ex)
                {
                    ex.ObjectName = ((IName)connectable.Executable).Name;
                    ex.Method = Text.Method.GoAsync;
                    async.SetResultException(ex);
                    async.EndAsync();
                }
                catch (System.InvalidCastException)  // "Specified cast is not valid."
                {
                    var mismatchData = Loader.AnalyseInvalidCastException(connectable, currentTableType, tableIndex);

                    // throw exception with accurate report
                    QueryTalkException exception;
                    if (mismatchData != null)
                    {
                        exception = Reader.TypeMismatchException(
                            ((IName)connectable.Executable).Name, Text.Method.GoAsync, currentTableType, mismatchData);
                    }
                    // throw exception with less accurate report (not likely to happen)
                    else
                    {
                        exception = Reader.TypeMismatchException(
                            ((IName)connectable.Executable).Name, Text.Method.GoAsync, currentTableType, new Row<string, string, string>());
                    }
                    async.SetResultException(exception);
                    async.EndAsync();
                }
                catch (System.Exception ex)
                {
                    QueryTalkException exception = new QueryTalkException("Reader._LoadTableSetBodyAsync.OnSqlRequestComplete",
                        QueryTalkExceptionType.ClrException, ((IName)connectable.Executable).Name,
                        Text.Method.GoAsync, String.Format("table = {0}", currentTableType));
                    exception.ClrException = ex;
                    async.SetResultException(exception);
                    async.EndAsync();
                }
            };

            async.BeginAsync(connectable, onSqlRequestComplete);
        }

        #endregion

        internal static Async<Result<T1, T2>> LoadManyAsync<T1, T2>(Connectable connectable)
        {
            var async = new Async<Result<T1, T2>>(connectable);
            async.Result = new Result<T1, T2>(connectable);   
            Type currentTableType = typeof(T1);
            int tableIndex = 0;

            Func<IDataReader, TableSetLoaderArgs> tableSetLoader = (reader) =>
            {
                async.Result.Table1 = ReadFirstOrNextTable<T1>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T2);
                async.Result.Table2 = ReadFirstOrNextTable<T2>(reader, connectable, ref tableIndex);
                return null;
            };

            _LoadTableSetBodyAsync<Result<T1, T2>>(connectable, tableSetLoader, async, currentTableType);
            return async;
        }

        internal static Async<Result<T1, T2, T3>> LoadManyAsync<T1, T2, T3>(Connectable connectable)
        {
            var async = new Async<Result<T1, T2, T3>>(connectable);
            async.Result = new Result<T1, T2, T3>(connectable);
            Type currentTableType = typeof(T1);
            int tableIndex = 0;

            Func<IDataReader, TableSetLoaderArgs> tableSetLoader = (reader) =>
            {
                async.Result.Table1 = ReadFirstOrNextTable<T1>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T2);
                async.Result.Table2 = ReadFirstOrNextTable<T2>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T3);
                async.Result.Table3 = ReadFirstOrNextTable<T3>(reader, connectable, ref tableIndex);
                return null;
            };

            _LoadTableSetBodyAsync(connectable, tableSetLoader, async, currentTableType);
            return async;
        }

        internal static Async<Result<T1, T2, T3, T4>> LoadManyAsync<T1, T2, T3, T4>(Connectable connectable)
        {
            var async = new Async<Result<T1, T2, T3, T4>>(connectable);
            async.Result = new Result<T1, T2, T3, T4>(connectable);
            Type currentTableType = typeof(T1);
            int tableIndex = 0;

            Func<IDataReader, TableSetLoaderArgs> tableSetLoader = (reader) =>
            {
                async.Result.Table1 = ReadFirstOrNextTable<T1>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T2);
                async.Result.Table2 = ReadFirstOrNextTable<T2>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T3);
                async.Result.Table3 = ReadFirstOrNextTable<T3>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T4);
                async.Result.Table4 = ReadFirstOrNextTable<T4>(reader, connectable, ref tableIndex);
                return null;
            };

            _LoadTableSetBodyAsync(connectable, tableSetLoader, async, currentTableType);
            return async;
        }

        internal static Async<Result<T1, T2, T3, T4, T5>> LoadManyAsync<T1, T2, T3, T4, T5>(Connectable connectable)
        {
            var async = new Async<Result<T1, T2, T3, T4, T5>>(connectable);
            async.Result = new Result<T1, T2, T3, T4, T5>(connectable);
            Type currentTableType = typeof(T1);
            int tableIndex = 0;

            Func<IDataReader, TableSetLoaderArgs> tableSetLoader = (reader) =>
            {
                async.Result.Table1 = ReadFirstOrNextTable<T1>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T2);
                async.Result.Table2 = ReadFirstOrNextTable<T2>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T3);
                async.Result.Table3 = ReadFirstOrNextTable<T3>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T4);
                async.Result.Table4 = ReadFirstOrNextTable<T4>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T5);
                async.Result.Table5 = ReadFirstOrNextTable<T5>(reader, connectable, ref tableIndex);
                return null;
            };

            _LoadTableSetBodyAsync(connectable, tableSetLoader, async, currentTableType);
            return async;
        }

        internal static Async<Result<T1, T2, T3, T4, T5, T6>> LoadManyAsync<T1, T2, T3, T4, T5, T6>(Connectable connectable)
        {
            var async = new Async<Result<T1, T2, T3, T4, T5, T6>>(connectable);
            async.Result = new Result<T1, T2, T3, T4, T5, T6>(connectable);
            Type currentTableType = typeof(T1);
            int tableIndex = 0;

            Func<IDataReader, TableSetLoaderArgs> tableSetLoader = (reader) =>
            {
                async.Result.Table1 = ReadFirstOrNextTable<T1>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T2);
                async.Result.Table2 = ReadFirstOrNextTable<T2>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T3);
                async.Result.Table3 = ReadFirstOrNextTable<T3>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T4);
                async.Result.Table4 = ReadFirstOrNextTable<T4>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T5);
                async.Result.Table5 = ReadFirstOrNextTable<T5>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T6);
                async.Result.Table6 = ReadFirstOrNextTable<T6>(reader, connectable, ref tableIndex);
                return null;
            };

            _LoadTableSetBodyAsync(connectable, tableSetLoader, async, currentTableType);
            return async;
        }

        internal static Async<Result<T1, T2, T3, T4, T5, T6, T7>> LoadManyAsync<T1, T2, T3, T4, T5, T6, T7>(Connectable connectable)
        {
            var async = new Async<Result<T1, T2, T3, T4, T5, T6, T7>>(connectable);
            async.Result = new Result<T1, T2, T3, T4, T5, T6, T7>(connectable);
            Type currentTableType = typeof(T1);
            int tableIndex = 0;

            Func<IDataReader, TableSetLoaderArgs> tableSetLoader = (reader) =>
            {
                async.Result.Table1 = ReadFirstOrNextTable<T1>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T2);
                async.Result.Table2 = ReadFirstOrNextTable<T2>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T3);
                async.Result.Table3 = ReadFirstOrNextTable<T3>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T4);
                async.Result.Table4 = ReadFirstOrNextTable<T4>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T5);
                async.Result.Table5 = ReadFirstOrNextTable<T5>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T6);
                async.Result.Table6 = ReadFirstOrNextTable<T6>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T7);
                async.Result.Table7 = ReadFirstOrNextTable<T7>(reader, connectable, ref tableIndex);
                return null;
            };

            _LoadTableSetBodyAsync(connectable, tableSetLoader, async, currentTableType);
            return async;
        }

        internal static Async<Result<T1, T2, T3, T4, T5, T6, T7, T8>> LoadManyAsync<T1, T2, T3, T4, T5, T6, T7, T8>(Connectable connectable)
        {
            var async = new Async<Result<T1, T2, T3, T4, T5, T6, T7, T8>>(connectable);
            async.Result = new Result<T1, T2, T3, T4, T5, T6, T7, T8>(connectable);
            Type currentTableType = typeof(T1);
            int tableIndex = 0;

            Func<IDataReader, TableSetLoaderArgs> tableSetLoader = (reader) =>
            {
                async.Result.Table1 = ReadFirstOrNextTable<T1>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T2);
                async.Result.Table2 = ReadFirstOrNextTable<T2>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T3);
                async.Result.Table3 = ReadFirstOrNextTable<T3>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T4);
                async.Result.Table4 = ReadFirstOrNextTable<T4>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T5);
                async.Result.Table5 = ReadFirstOrNextTable<T5>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T6);
                async.Result.Table6 = ReadFirstOrNextTable<T6>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T7);
                async.Result.Table7 = ReadFirstOrNextTable<T7>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T8);
                async.Result.Table8 = ReadFirstOrNextTable<T8>(reader, connectable, ref tableIndex);
                return null;
            };

            _LoadTableSetBodyAsync(connectable, tableSetLoader, async, currentTableType);
            return async;
        }

        internal static Async<Result<T1, T2, T3, T4, T5, T6, T7, T8, T9>> LoadManyAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            Connectable connectable)
        {
            var async = new Async<Result<T1, T2, T3, T4, T5, T6, T7, T8, T9>>(connectable);
            async.Result = new Result<T1, T2, T3, T4, T5, T6, T7, T8, T9>(connectable);
            Type currentTableType = typeof(T1);
            int tableIndex = 0;

            Func<IDataReader, TableSetLoaderArgs> tableSetLoader = (reader) =>
            {
                async.Result.Table1 = ReadFirstOrNextTable<T1>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T2);
                async.Result.Table2 = ReadFirstOrNextTable<T2>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T3);
                async.Result.Table3 = ReadFirstOrNextTable<T3>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T4);
                async.Result.Table4 = ReadFirstOrNextTable<T4>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T5);
                async.Result.Table5 = ReadFirstOrNextTable<T5>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T6);
                async.Result.Table6 = ReadFirstOrNextTable<T6>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T7);
                async.Result.Table7 = ReadFirstOrNextTable<T7>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T8);
                async.Result.Table8 = ReadFirstOrNextTable<T8>(reader, connectable, ref tableIndex);
                currentTableType = typeof(T9);
                async.Result.Table9 = ReadFirstOrNextTable<T9>(reader, connectable, ref tableIndex);
                return null;
            };

            _LoadTableSetBodyAsync(connectable, tableSetLoader, async, currentTableType);
            return async;
        }

        #endregion

        #region Supporting methods

        private static ResultSet<T> ReadFirstOrNextTable<T>(
            IDataReader reader,  
            Connectable connectable,            
            ref int tableIndex
            ) 
        {
            return _Read<T>(reader, connectable, null, ref tableIndex, tableIndex == 0);
        }

        // reads first or next single table from multi-table datasource
        private static ResultSet<T> _Read<T>(
            IDataReader reader, 
            Connectable connectable, 
            Action<T> handler, 
            ref int tableIndex, 
            bool isFirstRead)
            
        {
            ResultSet<T> table = null;

            var isFirstOrHasNextTable = isFirstRead ? true : reader.NextResult();

            if (isFirstOrHasNextTable)
            {
                if (handler == null)
                {
                    if (typeof(T) == typeof(DataTable))
                    {
                        table = new ResultSet<T>(LoadDataTable<T>(connectable, reader));
                    }
                    else
                    {
                        table = new ResultSet<T>(_LoadTable<T>(connectable, reader));
                    }
                }
                else
                {
                    _EnumerateTable<T>(connectable, reader, handler);
                }
                ++tableIndex;
            }
            else
            {
                throw Reader.NoMoreResultsetException<T>(connectable);
            }

            return table;
        }

        #endregion

    }
}
