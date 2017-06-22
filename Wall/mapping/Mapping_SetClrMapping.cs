#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QueryTalk.Wall
{
    internal static partial class Mapping
    {
        private static void SetClrMapping()
        {

            ClrMapping = new Dictionary<Type, ClrMappingInfo>();

            SetClrBoolean();
            SetClrByte();
            SetClrByteArray();
            SetClrBinary();
            SetClrDateTime();
            SetClrDateTimeOffset();
            SetClrDecimal();
            SetClrDouble();
            SetClrGuid();
            SetClrInt16();
            SetClrInt32();
            SetClrInt64();
            SetClrSingle();
            SetClrString();
            SetClrTimeSpan();
            SetClrObject();
        }

        private static void SetClrObject()
        {
            ClrMapping[typeof(System.Object)] = new ClrMappingInfo()
            {
                ClrType = typeof(System.Object),
                DTypes = new[] {
                    DT.Sqlvariant
                },
                DefaultDataType = new DataType(DT.Sqlvariant, false),
                SqlDataReaderGetMethodName = "GetValue",
                IsUnbox = false,
                NullableType = typeof(System.Object),
                ToJson = (value) =>
                {
                    if (value == null) { return Text.ClrNull; }
                    var type = value.GetType();
                    if (type == typeof(System.Object)) { return Text.ClrNull; }   // just in case: to avoid circular reference
                    if (ClrMapping.ContainsKey(type))
                    {
                        return ClrMapping[type].ToJson(value);
                    }
                    return Text.ClrNull;  // non-supported CLR type => null
                }
            };
        }

        private static void SetClrTimeSpan()
        {
            ClrMapping[typeof(System.TimeSpan)] = new ClrMappingInfo()
            {
                ClrType = typeof(System.TimeSpan),
                DTypes = new[] {
                    DT.Time
                },
                DefaultDataType = new DataType(DT.Time, false),
                SqlDataReaderGetMethodName = "GetValue",    // note: "GetDateTime" method causes "Specified cast is not valid" exception.
                IsUnbox = true,
                NullableType = typeof(Nullable<System.TimeSpan>),
                ToJson = (value) =>
                {
                    if (value == null) { return Text.ClrNull; }
                    return String.Format("\"{0}\"", (System.TimeSpan)value);
                }
            };
        }

        private static void SetClrGuid()
        {
            ClrMapping[typeof(System.Guid)] = new ClrMappingInfo()
            {
                ClrType = typeof(System.Guid),
                DTypes = new[] {
                    DT.Uniqueidentifier
                },
                DefaultDataType = new DataType(DT.Uniqueidentifier, false),
                SqlDataReaderGetMethodName = "GetGuid",
                IsUnbox = false,
                NullableType = typeof(Nullable<System.Guid>),
                ToJson = (value) =>
                {
                    if (value == null) { return Text.ClrNull; }
                    return String.Format("\"{0}\"", value.ToString());
                }
            };
        }

        private static void SetClrSingle()
        {
            ClrMapping[typeof(System.Single)] = new ClrMappingInfo()
            {
                ClrType = typeof(System.Single),
                DTypes = new[] {
                    DT.Real
                },
                DefaultDataType = new DataType(DT.Real, false),
                SqlDataReaderGetMethodName = "GetFloat",
                IsUnbox = false,
                NullableType = typeof(Nullable<System.Single>),
                ToJson = (value) =>
                {
                    if (value == null) { return Text.ClrNull; }
                    return (Convert.ToDouble(value)).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
            };
        }

        private static void SetClrDouble()
        {
            ClrMapping[typeof(System.Double)] = new ClrMappingInfo()
            {
                ClrType = typeof(System.Double),
                DTypes = new[] {
                    DT.Float
                },
                DefaultDataType = new DataType(DT.Float, false),
                SqlDataReaderGetMethodName = "GetDouble",
                IsUnbox = false,
                NullableType = typeof(Nullable<System.Double>),
                ToJson = (value) =>
                {
                    if (value == null) { return Text.ClrNull; }
                    return ((System.Double)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
            };
        }

        private static void SetClrDateTimeOffset()
        {
            ClrMapping[typeof(System.DateTimeOffset)] = new ClrMappingInfo()
            {
                ClrType = typeof(System.DateTimeOffset),
                DTypes = new[] {
                    DT.Datetimeoffset
                },
                DefaultDataType = new DataType(DT.Datetimeoffset, false),
                SqlDataReaderGetMethodName = "GetDateTimeOffset",
                IsUnbox = false,
                NullableType = typeof(Nullable<System.DateTimeOffset>),
                ToJson = (value) =>
                {
                    if (value == null) { return Text.ClrNull; }
                    return String.Format("\"{0:o}\"", value);
                }
            };
        }

        private static void SetClrDateTime()
        {
            ClrMapping[typeof(System.DateTime)] = new ClrMappingInfo()
            {
                ClrType = typeof(System.DateTime),
                DTypes = new[] {
                    DT.Date,
                    DT.Datetime,
                    DT.Datetime2,
                    DT.Smalldatetime
                },
                DefaultDataType = new DataType(DT.Datetime2, false),
                SqlDataReaderGetMethodName = "GetDateTime",
                IsUnbox = false,
                NullableType = typeof(Nullable<System.DateTime>),
                ToJson = (value) =>
                {
                    if (value == null) { return Text.ClrNull; }
                    return String.Format("\"{0:yyyy-MM-ddTHH:mm:ss.fffffff}\"", value);
                }
            };
        }

        private static void SetClrInt32()
        {
            ClrMapping[typeof(System.Int32)] = new ClrMappingInfo()
            {
                ClrType = typeof(System.Int32),
                DTypes = new[] {
                    DT.Int
                },
                DefaultDataType = new DataType(DT.Int, false),
                SqlDataReaderGetMethodName = "GetInt32",
                IsUnbox = false,
                NullableType = typeof(Nullable<System.Int32>),
                ToJson = (value) =>
                {
                    if (value == null) { return Text.ClrNull; }
                    return value.ToString();
                }
            };
        }

        private static void SetClrInt16()
        {
            ClrMapping[typeof(System.Int16)] = new ClrMappingInfo()
            {
                ClrType = typeof(System.Int16),
                DTypes = new[] {
                    DT.Smallint
                },
                DefaultDataType = new DataType(DT.Smallint, false),
                SqlDataReaderGetMethodName = "GetInt16",
                IsUnbox = false,
                NullableType = typeof(Nullable<System.Int16>),
                ToJson = (value) =>
                {
                    if (value == null) { return Text.ClrNull; }
                    return value.ToString();
                }
            };
        }

        private static void SetClrInt64()
        {
            // System.Int64
            ClrMapping[typeof(System.Int64)] = new ClrMappingInfo()
            {
                ClrType = typeof(System.Int64),
                DTypes = new[] {
                    DT.Bigint
                },
                DefaultDataType = new DataType(DT.Bigint, false),
                SqlDataReaderGetMethodName = "GetInt64",
                IsUnbox = false,
                NullableType = typeof(Nullable<System.Int64>),
                ToJson = (value) =>
                {
                    if (value == null) { return Text.ClrNull; }
                    return value.ToString();
                }
            };
        }

        private static void SetClrDecimal()
        {
            // System.Decimal
            ClrMapping[typeof(System.Decimal)] = new ClrMappingInfo()
            {
                ClrType = typeof(System.Decimal),
                DTypes = new[] {
                    DT.Decimal,
                    DT.Numeric,
                    DT.Money,
                    DT.Smallmoney
                },
                DefaultDataType = new DataType(DT.Decimal, 38, 16, false),
                SqlDataReaderGetMethodName = "GetDecimal",
                IsUnbox = false,
                NullableType = typeof(Nullable<System.Decimal>),
                ToJson = (value) =>
                {
                    if (value == null) { return Text.ClrNull; }
                    return ((System.Decimal)value).ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
            };
        }

        private static void SetClrBinary()
        {
            ClrMapping[typeof(System.Data.Linq.Binary)] = new ClrMappingInfo()
            {
                ClrType = typeof(System.Data.Linq.Binary),
                DTypes = new[] {
                    DT.Binary,
                    DT.Varbinary,
                    DT.VarbinaryMax,
                    DT.Timestamp,
                    DT.Image
                },
                DefaultDataType = new DataType(DT.VarbinaryMax, false),
                SqlDataReaderGetMethodName = "GetValue",    // note: "GetBytes" takes 4 arguments, also buffer size which cannot be provided in runtime
                IsUnbox = false,
                CtorParameterType = typeof(System.Byte[]),
                NullableType = typeof(System.Data.Linq.Binary),
                ToJson = (value) =>
                {
                    if (value == null) { return Text.ClrNull; }
                    return ((System.Data.Linq.Binary)value).ToString();
                }
            };
        }

        private static void SetClrByteArray()
        {
            // System.Byte[]
            ClrMapping[typeof(System.Byte[])] = new ClrMappingInfo()
            {
                ClrType = typeof(System.Byte[]),
                DTypes = new[] {
                    DT.Binary,
                    DT.Varbinary,
                    DT.VarbinaryMax,
                    DT.Timestamp,
                    DT.Image
                },
                DefaultDataType = new DataType(DT.VarbinaryMax, false),
                SqlDataReaderGetMethodName = "GetValue",    // note: "GetBytes" takes 4 arguments, also buffer size which cannot be provided in runtime
                IsUnbox = false,
                NullableType = typeof(System.Byte[]),
                ToJson = (value) =>
                {
                    if (value == null) { return Text.ClrNull; }
                    return new System.Data.Linq.Binary((System.Byte[])value).ToString();
                }
            };
        }

        private static void SetClrBoolean()
        {
            ClrMapping[typeof(System.Boolean)] = new ClrMappingInfo()
            {
                ClrType = typeof(System.Boolean),
                DTypes = new[] {
                        DT.Bit
                    },
                DefaultDataType = Mapping.DefaultBooleanType,
                SqlDataReaderGetMethodName = "GetBoolean",
                IsUnbox = false,
                NullableType = typeof(Nullable<System.Boolean>),
                ToJson = (value) =>
                {
                    if (value == null) { return Text.ClrNull; }
                    return value.ToString().ToLower();
                }
            };
        }

        private static void SetClrByte()
        {
            ClrMapping[typeof(System.Byte)] = new ClrMappingInfo()
            {
                ClrType = typeof(System.Byte),
                DTypes = new[] {
                    DT.Tinyint
                },
                DefaultDataType = new DataType(DT.Tinyint, false),
                SqlDataReaderGetMethodName = "GetByte",
                IsUnbox = false,
                NullableType = typeof(Nullable<System.Byte>),
                ToJson = (value) =>
                {
                    if (value == null) { return Text.ClrNull; }
                    return value.ToString();
                }
            };
        }

        private static void SetClrString()
        {
            ClrMapping[typeof(System.String)] = new ClrMappingInfo()
            {
                ClrType = typeof(System.String),
                DTypes = new[] {
                    DT.Char,
                    DT.NChar,
                    DT.Varchar,
                    DT.VarcharMax,
                    DT.NVarchar,
                    DT.NVarcharMax,
                    DT.Text,
                    DT.NText,
                    DT.Xml
                },
                DefaultDataType = DefaultStringType,
                SqlDataReaderGetMethodName = "GetString",
                IsUnbox = false,
                NullableType = typeof(System.String),
                ToJson = (value) =>
                {
                    if (value == null) { return Text.ClrNull; }
                    var json = Regex.Replace(value.ToString(), "([\\\\\\\"])", "\\$1");
                    json = Regex.Replace(json, "\b", "\\b");
                    json = Regex.Replace(json, "\f", "\\f");
                    json = Regex.Replace(json, "\n", "\\n");
                    json = Regex.Replace(json, "\r", "\\r");
                    json = Regex.Replace(json, "\t", "\\t");
                    return String.Format("\"{0}\"", json);
                }
            };
        }

    }
}
