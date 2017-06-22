#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Reflection;
using System.Data;
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
        /// Executes a bulk insert.
        /// </summary>
        /// <param name="data">The data to be inserted.</param>
        /// <param name="table">Is a target table where the data is inserted.</param>
        public static Result BulkInsertGo(DataTable data, TableArgument table)
        {
            return Call<Result>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (table == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "table", Wall.Text.Method.BulkInsertGo);
                }
                if (data == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "data", Wall.Text.Method.BulkInsertGo);
                }

                return Importer.ExecuteBulkInsert(ca, data, table, null);
            });
        }

        /// <summary>
        /// Executes a bulk insert.
        /// </summary>
        /// <param name="data">The data to be inserted.</param>
        /// <param name="table">Is a target table where the data is inserted.</param>
        public static Result BulkInsertGo(Result<DataTable> data, TableArgument table)
        {
            return Call<Result>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (table == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "table", Wall.Text.Method.BulkInsertGo);
                }
                if (data == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "data", Wall.Text.Method.BulkInsertGo);
                }

                return Importer.ExecuteBulkInsert(ca, data.ToDataTable(), table, null);
            });
        }

        /// <summary>
        /// Executes a bulk insert.
        /// </summary>
        /// <param name="rows">The data to be inserted.</param>
        public static Result<T> BulkInsertGo<T>(IEnumerable<T> rows)
            where T : DbRow
        {
            return Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (rows == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "rows", Wall.Text.Method.BulkInsertGo);
                }

                return Importer.ExecuteBulkInsert(ca, rows, null);
            });
        }

    }
}