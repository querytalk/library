#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace QueryTalk.Wall
{
    internal class Importer
    {
        internal static void ExecuteBulkInsert(Connectable connectable, SqlConnection cn)
        {
            var bulkArguments = connectable.Executable.Arguments
                .Where(argument => argument.IsBulk)
                .ToList();

            if (bulkArguments.Count == 0)
            {
                return;
            }

            var empty = bulkArguments.Where(argument => Value.IsNull(argument.Value))
                .FirstOrDefault();
            if (empty != null)
            {
                throw new QueryTalkException("Importer.ExecuteBulkInsert", QueryTalkExceptionType.ParamArgumentNull,
                    String.Format("param = {0}", empty.ParamName), Text.Method.Pass); 
            }

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cn))
            {
                bulkArguments.ForEach(argument =>
                {
                    if (((DataTable)argument.Value).Rows.Count > 0)
                    {
                        bulkCopy.DestinationTableName = argument.ParamName;
                        bulkCopy.WriteToServer((DataTable)argument.Value);
                    }
                });
            }
        }

        internal static Result ExecuteBulkInsert(Assembly client, DataTable data, TableArgument table, ConnectBy connectBy)
        {
            table.TryThrow(Text.Method.BulkInsertGo);

            if (data == null)
            {
                throw new QueryTalkException("Crud.GoBulkInsert", QueryTalkExceptionType.ArgumentNull, "data = null", Text.Method.BulkInsertGo)
                    .SetObjectName(table.Sql);
            }

            if (data.Rows.Count == 0)
            {
                return new Result(false, 0);
            }   

            try
            {
                ConnectionKey connKey = null;
                if (connectBy != null)
                {
                    connKey = ((IConnectable)connectBy).ConnectionKey;
                }

                var connectionString = ConnectionManager.InvokeConnectionFunc(client, connKey).ConnectionString; 
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();
 
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cn))
                    {
                        bulkCopy.DestinationTableName = table.Sql;
                        bulkCopy.WriteToServer(data);
                    }
                }

                return new Result(true, data.Rows.Count);
            }
            catch (QueryTalkException ex)
            {
                ex.Method = Text.Method.BulkInsertGo;
                throw;
            }
        }

        // mapped bulk insert
        internal static Result<T> ExecuteBulkInsert<T>(Assembly client, IEnumerable<T> rows, ConnectBy connectBy)
            where T : DbRow
        {
            if (rows == null)
            {
                throw new QueryTalkException("Importer.ExecuteBulkInsert<T>",
                    QueryTalkExceptionType.ArgumentNull, "rows = null", Text.Method.BulkInsertGo);
            }

            if (rows.Count() == 0)
            {
                return new Result<T>(false, 0);
            }

            Crud.CheckTable(rows.First(), Text.Method.BulkInsertGo);

            var first = rows.First();
            var nodeID = ((DbRow)first).NodeID;
            var nodeMap = DbMapping.GetNodeMap(nodeID);
            var table = nodeMap.Name.ToString();
            var data = rows.ToDataTable();

            try
            {
                ConnectionKey connKey = null;
                if (connectBy != null)
                {
                    connKey = ((IConnectable)connectBy).ConnectionKey;
                }

                var connectionString = ConnectionManager.InvokeConnectionFunc(client, connKey).ConnectionString;
                using (SqlConnection cn = new SqlConnection(connectionString))
                {
                    cn.Open();

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cn))
                    {
                        bulkCopy.DestinationTableName = table;
                        foreach (var column in nodeMap.GetInsertableColumns(false))
                        {
                            bulkCopy.ColumnMappings.Add(column.Name.Part1, column.Name.Part1);
                        }

                        bulkCopy.WriteToServer(data);
                    }
                }

                return new Result<T>(true, rows.Count());
            }
            catch (QueryTalkException ex)
            {
                ex.Method = Text.Method.BulkInsertGo;
                throw;
            }
        }
        
    }
}
