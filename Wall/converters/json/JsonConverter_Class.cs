#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Text;
using System.Collections.Generic;

namespace QueryTalk.Wall
{
    internal partial class JsonConverter
    {
        internal static string ConvertClass(object data, List<JsonProperty> jsonProperties, int indent = 0)
        {
            var json = new StringBuilder(JsonConverter.StringBuilderCapacity);
            if (indent > 1)
            {
                json.NewLineIndent("{", indent - 1);
            }
            else
            {
                json.Append("{");
            }

            var ix = 0;
            foreach (var prop in jsonProperties)
            {
                if (ix > 0)
                {
                    json.AppendComma();
                }
                var value = prop.Accessor.GetValue(data);
                var info = Mapping.ClrMapping[prop.ClrType];

                if (indent > 0)
                {
                    json.NewLineIndent(indent);
                }

                _AppendValue(json, value, info, prop.PropertyName, indent);
                ++ix;
            }

            if (indent > 0)
            {
                json.NewLineIndent("}", indent - 1);
            }
            else
            {
                json.Append("}");
            }

            return json.ToString();
        }

        private static void _AppendValue(StringBuilder json, object value, ClrMappingInfo info, string propertyName, int indent)
        {
            json.AppendFormat("{0}:{1}{2}", propertyName.DelimitJsonString(), indent > 0 ? " " : "", info.ToJson(value));
        }


    }
}
