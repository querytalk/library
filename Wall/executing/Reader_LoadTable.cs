#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QueryTalk.Wall
{
    internal static partial class Reader
    {
        // core load method for sync/async load
        private static IEnumerable<T> _load<T>(Connectable connectable, IDataReader reader, Action<T> rowHandler, bool allowEmpty = false) 
        {
            if (connectable.IgnoreLoad)
            {
                return null;
            }

            if (rowHandler == null)
            {
                var table = _LoadTable<T>(connectable, reader, allowEmpty);
                return table;
            }
            else
            {
                _EnumerateTable<T>(connectable, reader, rowHandler);
                return null;
            }
        }

        internal static Result<T> LoadTable<T>(Connectable connectable, Action<T> rowHandler, bool allowEmpty = false) 
        {
            IEnumerable<T> table = null;
            int returnValue = 0;
            Type type = typeof(T);

            if (type == typeof(DataTable))
            {
                throw new QueryTalkException("Reader.LoadTable<T>", QueryTalkExceptionType.DataTableCannotEnumerate,
                    ((IName)connectable.Executable).Name, ".Go<DataTable>", null);
            }

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
                            table = _load<T>(connectable, reader, rowHandler, allowEmpty);
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
                var mismatchData = Loader.AnalyseInvalidCastException(connectable, type, 0);
                // throw exception with accurate report
                if (mismatchData != null)
                {
                    var exception = Reader.TypeMismatchException(((IName)connectable.Executable).Name, Text.Method.Go, type, mismatchData);
                    throw exception;
                }
                // throw exception with less accurate report (not likely to happen)
                else
                {
                    var exception = Reader.TypeMismatchException(
                        ((IName)connectable.Executable).Name, Text.Method.Go, type, new Row<string, string, string>());
                    throw exception;
                }
            }
            catch (System.ArgumentNullException ex)
            {
                var exception = ClrException(ex, "Load", connectable,
                    Text.Method.Go, String.Format("data class = {0}", typeof(T)));
                exception.Extra = Text.Free.DataClassNoConstructorExtra;
                throw exception;
            }

            var result = new Result<T>(connectable, table);
            result.ReturnValue = returnValue;
            return result;
        }

        // asynchronously execute & load
        internal static Async<Result<T>> LoadTableAsync<T>(Connectable connectable, Action<T> rowHandler)
        {
            Type type = typeof(T);
            if (type == typeof(DataTable))
            {
                throw new QueryTalkException("Reader.LoadTableAsync", QueryTalkExceptionType.DataTableCannotEnumerate,
                    ((IName)connectable.Executable).Name, ".GoAsync<DataTable>", null);
            }

            var async = new Async<Result<T>>(connectable);
            async.Result = new Result<T>(connectable);  
            int returnValue = 0;
            IEnumerable<T> table = null;

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
                            table = _load<T>(connectable, reader, rowHandler);
                            returnValue = Reader.ReadOutputData(readerLoader, connectable);
                            async.Result = new Result<T>(connectable, table, returnValue);
                        }
                        catch (QueryTalkException ex)
                        {
                            async.SetResultException(ex);
                        }
                        catch (System.InvalidCastException)  // "Specified cast is not valid."
                        {
                            var mismatchData = Loader.AnalyseInvalidCastException(connectable, type, 0);

                            // throw exception with accurate report
                            QueryTalkException exception;
                            if (mismatchData != null)
                            {
                                exception = Reader.TypeMismatchException(((IName)connectable.Executable).Name, Text.Method.GoAsync, type, mismatchData);
                            }
                            // throw exception with less accurate report (not likely to happen)
                            else
                            {
                                exception = Reader.TypeMismatchException(
                                    ((IName)connectable.Executable).Name, Text.Method.GoAsync, type, new Row<string, string, string>());
                            }

                            async.SetResultException(exception);
                        }
                        catch (System.ArgumentNullException ex)
                        {
                            var exception = ClrException(ex, "Load", connectable,
                                Text.Method.GoAsync, String.Format("data class = {0}", type));
                            async.SetResultException(exception);
                            async.Exception.Extra = Text.Free.DataClassNoConstructorExtra;
                        }
                        catch (System.Exception ex)
                        {
                            async.SetClrException(ex, "LoadTableAsync.Loader", connectable, Text.Method.GoAsync);
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
                    asyncBegin = (Async)iAsyncResult.AsyncState;                // Async object passed from BeginAsync method
                    reader = async.Command.EndExecuteReader(iAsyncResult);      // notifies main thread that SQL request has completed

                    // -------------------------------------------------------------------------------------------
                    // here is a gap between two async calls: first is finished, the second is about to begin...
                    // -------------------------------------------------------------------------------------------

                    // begin async2
                    async.AsyncResult2 = loader.BeginInvoke(reader, 
                        new AsyncCallback(
                            new Action<IAsyncResult>((iAsyncResult2) =>
                            {
                                try
                                {
                                    loader.EndInvoke(iAsyncResult2);  
                                    async.EndAsync();
                                }
                                catch (System.Exception ex)
                                {
                                    async.SetClrException(ex, "LoadTable.AsyncResult2", connectable, Text.Method.GoAsync);
                                    async.EndAsync();
                                }
                            })),
                        null);
                }
                catch (QueryTalkException ex)
                {
                    async.SetResultException(ex);
                    async.EndAsync();
                }
                catch (System.Exception ex)
                {
                    async.SetClrException(ex, "LoadTableAsync.OnSqlRequestComplete", connectable, Text.Method.GoAsync);
                    async.EndAsync();
                }
            };

            async.BeginAsync(connectable, onSqlRequestComplete);
            return async;
        }

        private static IEnumerable<T> _LoadTable<T>(
            Connectable connectable,
            IDataReader reader,
            bool allowEmpty = false)
        {
            Type type = typeof(T);
            var table = new HashSet<T>();
            Func<IDataRecord, T> loader = null;

            if (!reader.Read())
            {
                return table;
            }

            if (reader.GetName(0) == Text.Reserved.ReturnValueColumnName)
            {
                if (reader.NextResult())
                {
                    Reader.ThrowQueryTalkReservedNameException();
                }

                if (!allowEmpty)
                {
                    throw new QueryTalkException("Reader._LoadTable<T>", QueryTalkExceptionType.NoMoreResultset,
                        String.Format("table = {0}", type), Text.Method.Go);
                }
                else
                {
                    return null;
                }
            }

            QueryTalkException exception;
            loader = Loader.ProvideLoader<T>(reader, out exception);
            Loader.TryThrowException(exception, connectable);

            int rowix = 1;
            do
            {
                T row = loader(reader);
                table.Add(row);

                if (connectable.AsyncCanceled)
                {
                    ThrowOperationCancelledByUserException<T>(connectable, rowix);
                }
                ++rowix;
            }
            while (reader.Read());

            Loader.SetTableAsLoaded(table);

            return table;
        }

        private static void _EnumerateTable<T>(Connectable connectable, IDataReader reader, Action<T> rowHandler)

        {
            Type type = typeof(T);
            Func<IDataRecord, T> loader = null;

            if (!reader.Read())
            {
                return;
            }

            if (reader.GetName(0) == Text.Reserved.ReturnValueColumnName)
            {
                throw new QueryTalkException("Reader._EnumerateTable<T>", QueryTalkExceptionType.NoMoreResultset,
                    String.Format("table = {0}", type), Text.Method.Go);
            }

            QueryTalkException exception;
            loader = Loader.ProvideLoader<T>(reader, out exception);
            Loader.TryThrowException(exception, connectable);

            int rowix = 1;
            do
            {
                T row = loader(reader);
                Loader.SetRowAsLoaded(row);
                rowHandler(row);

                if (connectable.AsyncCanceled)
                {
                    ThrowOperationCancelledByUserException<T>(connectable, rowix);
                }
                ++rowix;
            }
            while (reader.Read());
        }     

    }
}
