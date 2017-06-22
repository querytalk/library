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
        internal static Result<T> LoadDataTable<T>(Connectable connectable, bool allowEmpty = false) 
        {
            DataSet dataSet = new DataSet();
            dataSet.Locale = System.Globalization.CultureInfo.InvariantCulture;
            int returnValue = 0;
            Result<T> result = null;

            try
            {
                using (SqlConnection cn = new SqlConnection(connectable.ConnectionString))
                {
                    cn.Open();

                    Importer.ExecuteBulkInsert(connectable, cn);
                    var cmd = new SqlCommand(connectable.Sql, cn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.SelectCommand.CommandTimeout = connectable.CommandTimeout;
                    da.Fill(dataSet);

                    int tableCount = dataSet.Tables.Count;

                    DataTable firstTable = null;
                    if (tableCount != 0)
                    {
                        firstTable = dataSet.Tables[0];
                        if (tableCount <= 1)
                        {
                            if (!allowEmpty)
                            {
                                throw new QueryTalkException("Reader.LoadDataTable", QueryTalkExceptionType.NoMoreResultset,
                                    "table = DataTable", Text.Method.Go);
                            }
                        }

                        // get return value
                        DataTable lastTable = dataSet.Tables[tableCount - 1];
                        var value = lastTable.Rows[0][Text.Reserved.ReturnValueColumnName];
                        if (value != null && value.GetType() == typeof(System.Int32))
                        {
                            returnValue = (int)value;
                        }
                    }

                    result = new Result<T>(connectable, firstTable);
                    result.ReturnValue = returnValue;
                }
            }
            catch (QueryTalkException ex)
            {
                ex.ObjectName = ((IName)connectable.Executable).Name;
                ex.Method = Text.Method.Go;
                throw;
            }

            return result;
        }

        // asynchronously execute & load
        internal static Async<Result<T>> LoadDataTableAsync<T>(Connectable connectable)
        {
            var async = new Async<Result<T>>(connectable);
            async.Result = new Result<T>(connectable);  
            int returnValue = 0;
            DataTable table = null;

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
                            table = LoadDataTable<T>(connectable, readerLoader);
                            returnValue = Reader.ReadOutputData(readerLoader, connectable);
                            async.Result = new Result<T>(connectable, table, returnValue);
                        }
                        catch (QueryTalkException ex)
                        {
                            if (async.SetResultException(ex))
                            {
                                async.Exception.Method = Text.Method.GoAsync;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            async.SetClrException(ex, "LoadDataTableAsync.Loader", connectable, Text.Method.GoAsync);
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
                                catch (System.Exception ex)
                                {
                                async.SetClrException(ex, "LoadDataTable.AsyncResult2", connectable, Text.Method.GoAsync);
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
                    async.SetClrException(ex, "LoadDataTableAsync<T>.OnSqlRequestComplete", connectable, Text.Method.GoAsync);
                    async.EndAsync();
                }
            };

            async.BeginAsync(connectable, onSqlRequestComplete);
            return async;
        }

        internal static DataTable LoadDataTable<T>(Connectable connectable, IDataReader reader) 
        {
            DataTable table = new DataTable();
            table.Locale = System.Globalization.CultureInfo.InvariantCulture;

            try
            {
                DataTable schema = reader.GetSchemaTable();
                foreach (DataRow row in schema.Rows)
                {
                    DataColumn column = new DataColumn();
                    column.ColumnName = row["ColumnName"].ToString();
                    column.DataType = Type.GetType(row["DataType"].ToString());
                    table.Columns.Add(column);
                }

                if (reader.Read())
                {
                    if (reader.GetName(0) == Text.Reserved.ReturnValueColumnName)
                    {
                        throw new QueryTalkException("Reader.LoadDataTable", QueryTalkExceptionType.NoMoreResultset,
                            "table = DataTable", Text.Method.Go);
                    }
                }
                else
                {
                    return table;
                }

                int rowix = 1;
                do
                {
                    DataRow row = table.NewRow();
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        row[i] = reader[i];
                    }
                    table.Rows.Add(row);

                    if (connectable.AsyncCanceled)
                    {
                        ThrowOperationCancelledByUserException<T>(connectable, rowix);
                    }
                    ++rowix;
                }
                while (reader.Read());
            }
            catch (QueryTalkException ex)
            {
                ex.ObjectName = ((IName)connectable.Executable).Name;
                ex.Method = Text.Method.Go;
                throw;
            }

            return table;
        }

    }
}
