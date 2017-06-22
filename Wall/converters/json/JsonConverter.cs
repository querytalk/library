#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace QueryTalk.Wall
{
    // Converts .NET data to JSON.
    internal partial class JsonConverter
    {
        internal static int StringBuilderCapacity = 300;

        internal static string ToJson<T>(T data)
        {
            Type type = typeof(T);
            QueryTalkException exception;

            if (data != null && type == typeof(System.Object))
            {
                type = data.GetType();
            }

            if (data is ResultSet<DataTable>)
            {
                return JsonConverter.ConvertDataTable(((ResultSet<DataTable>)(object)data).ToDataTable());
            }

            if (type == typeof(Result<DataTable>) && data == null)
            {
                return JsonConverter.ConvertDataTable(null);
            }

            if (type == typeof(DataTable))
            {
                return JsonConverter.ConvertDataTable((DataTable)(object)data);
            }

            if (type.IsIResult(typeof(IResult)))
            {
                return JsonConverter.ConvertResult((IResult)data, 1);
            }

            if (type == typeof(Value))
            {
                return JsonConverter.ConvertValue((Value)(object)data);
            }

            Type clrType;
            if (QueryTalk.Wall.Mapping.CheckClrCompliance(type, out clrType, out exception)
                == QueryTalk.Wall.Mapping.ClrTypeMatch.ClrMatch)
            {
                return JsonConverter.ConvertScalar(data, clrType);
            }

            if (type.GetInterfaces().Where(i => i == typeof(IEnumerable)).Any())
            {
                return JsonConverter.ConvertCollection(typeof(T), (IEnumerable)data);
            }

            if (type.IsClass)
            {
                var jsonProperties = ReflectClass(type);
                return ConvertClass(data, jsonProperties, 1);
            }

            throw new QueryTalkException("JsonConverter.ToJson<T>", QueryTalkExceptionType.InvalidSerializationData,
                String.Format("type = {0}", typeof(T)), Text.Method.ToJson);
        }

        private static List<JsonProperty> ReflectClass(Type type)
        {
            QueryTalkException exception;
            Type clrType;
            bool cached = false;

            List<JsonProperty> jsonCacheValue;
            cached = Cache.JsonCache.TryGetValue(type, out jsonCacheValue);
            if (!cached)
            {
                jsonCacheValue = new List<JsonProperty>();
                var properties = type.GetReadableProperties();
                var ix = 0;
                var valid = false;
                foreach (var property in properties)
                {
                    var clrTypeMatch = Mapping.CheckClrCompliance(property.PropertyType, out clrType, out exception);
                    if (clrTypeMatch != Mapping.ClrTypeMatch.ClrMatch)
                    {
                        continue;
                    }

                    if (!cached)
                    {
                        jsonCacheValue.Add(new JsonProperty(property.Name, clrType, PropertyAccessor.Create(type, property)));
                    }

                    ++ix;
                    valid = true;
                }

                if (!valid)
                {
                    throw new QueryTalkException("JsonConverter.ToJson<T>", QueryTalkExceptionType.InvalidDataClass,
                        String.Format("type = {0}", type), Text.Method.ToJson);
                }

                Cache.JsonCache[type] = jsonCacheValue;
            }

            return jsonCacheValue;
        }

    }
}
