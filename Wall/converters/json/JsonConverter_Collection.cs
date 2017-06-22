#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace QueryTalk.Wall
{
    internal partial class JsonConverter
    {
        private static string ConvertCollection(Type ctype, IEnumerable data)
        {
            var json = new StringBuilder("[", JsonConverter.StringBuilderCapacity);                                               // collection type
            var type = Common.TryGetSerializationItemType(ctype, data, Text.Method.ToJson);     // item type

            CollectionType collectionType;
            ClrMappingInfo clrInfo = null;
            Type clrType;
            QueryTalkException exception;
            List<JsonProperty> jsonProperties = null;

            // empty collection
            if (type == null)
            {
                collectionType = CollectionType.Object;
                data = null;
            }
            else if (type == typeof(System.Object))
            {
                collectionType = CollectionType.Object;
            }
            else if (type == typeof(Value))
            {
                collectionType = CollectionType.Value;
            }
            // CLR compliant scalar type
            else if (Mapping.CheckClrCompliance(type, out clrType, out exception) == Mapping.ClrTypeMatch.ClrMatch)
            {
                collectionType = CollectionType.Clr;
                clrInfo = Mapping.ClrMapping[clrType];
            }
            // class
            else
            {
                collectionType = CollectionType.Class;
                jsonProperties = ReflectClass(type);
            }

            var first = true;
            if (data != null)
            {
                foreach (var row in data)
                {
                    if (!first)
                    {
                        json.Append(",");
                    }

                    if (row.IsUndefined())
                    {
                        json.Append(Text.ClrNull);
                        first = false;
                        continue;
                    }

                    switch (collectionType)
                    {
                        case CollectionType.Object:
                            var rowType = row.GetType();
                            if (Mapping.CheckClrCompliance(rowType, out clrType, out exception) != Mapping.ClrTypeMatch.ClrMatch)
                            {
                                throw exception;
                            }
                            clrInfo = Mapping.ClrMapping[rowType];
                            json.Append(clrInfo.ToJson(row));
                            break;

                        case CollectionType.Value:
                            var o = ((Value)row).Original;
                            clrInfo = Mapping.ClrMapping[o.GetType()];
                            json.Append(clrInfo.ToJson(o));
                            break;

                        case CollectionType.Clr:                 
                            json.Append(clrInfo.ToJson(row));
                            break;

                        // class
                        default:
                            json.Append(ConvertClass(row, jsonProperties));
                            break;
                    }

                    first = false;
                }
            }

            json.Append("]");
            return json.ToString();
        }

    }
}
