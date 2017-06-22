#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace QueryTalk.Wall
{
    internal partial class JsonConverter
    {
        private static string ConvertDataTable(DataTable dataTable)
        {
            if (dataTable == null)
            {
                throw new QueryTalkException("JsonConverter.ConvertDataTable", QueryTalkExceptionType.ArgumentNull,
                    "dataTable = null", Text.Method.ToJson);
            }

            if (dataTable.Columns.Count == 0)
            {
                throw new QueryTalkException("JsonConverter.ConvertDataTable", QueryTalkExceptionType.InvalidDataTable,
                    "type = DataTable", Text.Method.ToJson);
            }

            var columns = new List<Tuple<string, Type>>();
            int i = 0;
            var names = new HashSet<string>();
            foreach (DataColumn column in dataTable.Columns)
            {
                Type clrType;
                QueryTalkException exception;
                if (Mapping.CheckClrCompliance(column.DataType, out clrType, out exception) != Mapping.ClrTypeMatch.ClrMatch)
                {
                    continue;   
                }

                var name = Naming.GetClrColumnName(column.ColumnName, names, null, null);
                names.Add(name);

                columns.Add(Tuple.Create(name, clrType));
                ++i;
            }

            var json = new StringBuilder("[");
            bool first = true;
            foreach (var row in dataTable.AsEnumerable())
            {
                if (!first)
                {
                    json.AppendComma();
                }

                json.Append("{");
                for (int ix = 0; ix < i; ++ix)
                {
                    if (ix != 0)
                    {
                        json.AppendComma();
                    }

                    var value = row[ix];
                    var column = columns[ix];

                    if (value == DBNull.Value)
                    {
                        value = null;
                    }

                    json.AppendFormat("\"{0}\":{1}", column.Item1, Mapping.ClrMapping[column.Item2].ToJson(value));
                }
                json.Append("}");

                first = false;
            }
            json.Append("]");

            return json.ToString();
        }

    }
}
