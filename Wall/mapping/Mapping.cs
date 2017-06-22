#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;

namespace QueryTalk.Wall
{
    internal static partial class Mapping
    {

        internal static Dictionary<DT, SqlMappingInfo> SqlMapping;
        internal static Dictionary<Type, ClrMappingInfo> ClrMapping;
        internal static Dictionary<DT, Type[]> InlineMapping;
        internal static Dictionary<DT, Type[]> TableParamMapping;

        // default data types
        internal static DataType DefaultBooleanType = new DataType(DT.Bit, false);
        internal static DataType DefaultStringType = new DataType(DT.NVarcharMax, false);
        internal static DataType DefaultInt16Type = new DataType(DT.Tinyint, false);
        internal static DataType DefaultInt32Type = new DataType(DT.Int, false);
        internal static DataType DefaultInt64Type = new DataType(DT.Bigint, false);
        internal static DataType DefaultDecimalType = new DataType(DT.Decimal, 38, 16, false);
        internal static DataType DefaultBinaryType = new DataType(DT.VarbinaryMax, false);
        internal static DataType DefaultDateTimeType = new DataType(DT.Datetime2, false);
        internal static DataType DefaultDateTimeOffsetType = new DataType(DT.Datetimeoffset, false);
        internal static DataType DefaultSingleType = new DataType(DT.Real, false);
        internal static DataType DefaultDoubleType = new DataType(DT.Float, false);
        internal static DataType DefaultGuidType = new DataType(DT.Uniqueidentifier, false);
        internal static DataType DefaultTimeSpanType = new DataType(DT.Time, false);

        internal enum SizeType : int { None = 0, Length = 1, Precision = 2 }
        internal enum ClrTypeMatch : int
        {
            ClrMatch = 0,
            NodeMatch = 1,
            Mismatch = 2
        }

        internal enum DataTypeGroup : int
        {
            // number group (bit, int, decimal...)
            Number = 0,

            // text group (char, nvarchar, datetime...)
            Text = 1,

            // hexadecimal group (binary, rowversion, image...)
            Hexadecimal = 2,

            // object group (sql_variant)
            Object = 3,
        }

        private const string _runtimeType = "System.RuntimeType";

        static Mapping()
        {
            SetClrMapping();
            SetSqlMapping();
            SetTableParamMapping();
            SetInlineMapping();
        }

        internal static DataType ProvideDataType(DataType dataType, Type clrType)
        {
            if (dataType != null)
            {
                return dataType;
            }
            else
            {
                return ClrMapping[clrType].DefaultDataType;
            }
        }

    }
}
