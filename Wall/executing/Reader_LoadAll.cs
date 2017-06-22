#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace QueryTalk.Wall
{
    internal static partial class Reader
    {
        private static Result _LoadAll(Connectable connectable, SqlDataReader reader)
        {
            Result result = null;
            int tableIndex = 0;
            Type resultType = typeof(Result);

            try
            {
                do
                {
                    if (reader.GetName(0) == Text.Reserved.ReturnValueColumnName)
                    {

                        if (result == null)
                        {
                            result = new Result(connectable);
                        }

                        result.TableCount = tableIndex;
                        result.ReturnValue = Reader.ReadOutputDataOnFound(reader, connectable);

                        if (reader.NextResult())
                        {
                            Reader.ThrowQueryTalkReservedNameException();
                        }

                        return result;  // the code is supposed to always exit the method here
                    }

                    var table = _LoadTable<dynamic>(connectable, reader);
                    string tableName = String.Format("{0}{1}", Text.Free.Table, tableIndex + 1);

                    // Table1
                    if (tableIndex == 0)
                    {
                        result = new Result(connectable, table);
                    }
                    // Table2..
                    else
                    {
                        var ptable = new ResultSet<dynamic>(table);

                        // Table2..Table9
                        if (tableIndex < 9)
                        {
                            resultType.GetProperty(tableName).SetValue(result, ptable, null);
                        }
                        // Table10..
                        else
                        {
                            if (result.OtherTables == null)
                            {
                                result.OtherTables = new ExpandoObject();
                            }
                            ((IDictionary<string, object>)result.OtherTables)[tableName] = ptable;
                        }
                    }

                    ++tableIndex;
                }
                while (reader.NextResult());
            }
            catch (System.InvalidCastException)  // "Specified cast is not valid."
            {
                var mismatchData = Loader.AnalyseInvalidCastException(connectable, typeof(object), tableIndex);

                // throw exception with more accurate report
                if (mismatchData != null)
                {
                    var exception = Reader.TypeMismatchException(((IName)connectable.Executable).Name, Text.Method.Go, typeof(object), mismatchData);
                    throw exception;
                }
                // throw exception with less accurate report (not likely to happen)
                else
                {
                    var exception = Reader.TypeMismatchException(
                        ((IName)connectable.Executable).Name, Text.Method.Go, typeof(object), new Row<string, string, string>());
                    throw exception;
                }
            }

            return result;    // the code is not supposed to ever reach this line
        }

        internal static Result LoadAll(Connectable connectable)
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
                        return _LoadAll(connectable, reader);
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
            }
        }

        // asynchronously execute & load
        internal static Async LoadAllAsync(Connectable connectable)
        {
            var async = new Async(connectable);
            async.Result = new Result(connectable);  

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
                            async.Result = _LoadAll(connectable, reader);
                        }
                        catch (QueryTalkException ex)
                        {
                            async.SetResultException(ex);
                        }
                        catch (System.Exception ex)
                        {
                            async.SetClrException(ex, "LoadAllAsync.Loader", connectable, Text.Method.GoAsync);
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
                                    async.SetClrException(ex, "LoadAllAsync.AsyncResult2", connectable, Text.Method.GoAsync);
                                    async.EndAsync();
                                }
                            })),
                        null);
                }

                // Here we catch the exception that happend during the READER EXECUTION and is thrown by EndExecuteReader method.
                catch (QueryTalkException ex)
                {
                    async.SetResultException(ex);
                    async.EndAsync();
                }
                catch (System.Exception ex)
                {
                    async.SetClrException(ex, "LoadAllAsync.OnSqlRequestComplete", connectable, Text.Method.GoAsync);
                    async.EndAsync();
                }
            };
            
            async.BeginAsync(connectable, onSqlRequestComplete);
            return async;
        }
    }

}
