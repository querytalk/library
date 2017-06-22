#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;

namespace QueryTalk.Wall
{
    internal partial class JsonConverter
    {
        private static string ConvertResult(IResult data, int indent)
        {
            var result = (IResult)(object)data;
            var jdata = new StringBuilder(JsonConverter.StringBuilderCapacity);
            if (indent > 1)
            {
                jdata.NewLineIndent("{", indent - 1);
            }
            else
            {
                jdata.Append("{");
            }

            if (result == null)
            {
                throw new QueryTalkException("JsonConverter.ToJson<T>",
                    QueryTalkExceptionType.ArgumentNull, "type = Result", Text.Method.ToJson)
                    .SetExtra("An undefined dynamic Result object cannot be serialized.");
            }

            jdata.NewLineIndent(indent).AppendFormat("\"ReturnValue\": {0}", result.ReturnValue);

            if (result.TableCount == 1)
            {
                _ConvertTable(jdata, result, 1, indent);
            }
            else
            {
                var properties = data.GetType().GetProperties();
                int ix = 1;
                foreach (var prop in properties.Where(a => Regex.IsMatch(a.Name, @"^Table\d")).OrderBy(a => a.Name))
                {
                    var table = prop.GetValue(result, null);
                    if (table == null) { continue; }
                    _ConvertTable(jdata, table, ix, indent);
                    ++ix;
                }

                if (result.TableCount > 9)
                {
                    for (int i = 10; i <= result.TableCount; ++i)
                    {
                        var table = ((Result)result).GetTable(i);
                        if (table == null) { continue; }
                        _ConvertTable(jdata, table, i, indent);
                    }
                }
            }

            if (indent > 0)
            {
                jdata.NewLineIndent("}", indent - 1);
            }
            else
            {
                jdata.Append("}");
            }

            return jdata.ToString();
        }

        private static void _ConvertTable(StringBuilder jdata, object table, int ix, int indent)
        {
            string data;
            var type = table.GetType();

            jdata.AppendComma();

            if (type == typeof(ResultSet<DataTable>))
            {
                data = ConvertDataTable(((ResultSet<DataTable>)table).ToDataTable());
            }
            else if (type.GetInterfaces().Where(i => i == typeof(IEnumerable)).Any())
            {
                data = ConvertCollection(type, (IEnumerable)table);
            }
            else
            {
                throw new QueryTalkException("ToJson", QueryTalkExceptionType.InvalidResult,
                    String.Format("table type = {0}", type));
            }

            jdata.NewLineIndent(indent).AppendFormat("\"Table{0}\": {1}", ix, data);
        }

    }
}
