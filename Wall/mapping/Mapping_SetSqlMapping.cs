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
        private static void SetSqlMapping()
        {
            SqlMapping = new Dictionary<DT, SqlMappingInfo>();

            SetSqlBigint();
            SetSqlBinary();
            SetSqlBit();
            SetSqlChar();
            SetSqlDate();
            SetSqlDatetime();
            SetSqlDatetime2();
            SetSqlDatetimeoffset();
            SetSqlSmalldatetime();
            SetSqlDecimal();
            SetSqlFloat();
            SetSqlInt();
            SetSqlMoney();
            SetSqlNChar();
            SetSqlNumeric();
            SetSqlNVarchar();
            SetSqlNVarcharMax();
            SetSqlReal();
            SetSqlSmallint();
            SetSqlSmallmoney();
            SetSqlVariant();
            SetSqlSysname();
            SetSqlTime();
            SetSqlTimestamp();
            SetSqlRowversion();
            SetSqlTinyint();
            SetSqlUniqueidentifier();
            SetSqlVarbinary();
            SetSqlVarbinaryMax();
            SetSqlVarchar();
            SetSqlVarcharMax();
            SetSqlXml();
            SetSqlText();
            SetSqlNText();
            SetSqlImage();
            SetSqlConcatenator();

        }

        private static void SetSqlVariant()
        {
            SqlMapping[DT.Sqlvariant] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Object),
                ClrSubTypes = new Type[] { },
                Sql = "[sql_variant]",
                SqlPlain = "sql_variant",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[sql_variant]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 100,
                DataTypeGroup = DataTypeGroup.Object,
                Build = (value, dataType) =>
                {
                    return BuildUnchecked(value);
                }
            };
        }

        private static void SetSqlTime()
        {
            SqlMapping[DT.Time] = new SqlMappingInfo()
            {
                ClrType = typeof(System.TimeSpan),
                ClrSubTypes = new Type[] { },
                Sql = "[time]",
                SqlPlain = "time",
                SizeType = SizeType.Length,
                MaxSize = 7,
                SqlByValue = (value) => { return "[time]"; },                       
                CheckSize = (value, length, precision, scale) => { return true; },  
                MaxSizeInChars = 16,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    return _BuildTimeSpan(value);
                }
            };
        }

        private static void SetSqlUniqueidentifier()
        {
            SqlMapping[DT.Uniqueidentifier] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Guid),
                ClrSubTypes = new Type[] { },
                Sql = "[uniqueidentifier]",
                SqlPlain = "uniqueidentifier",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[uniqueidentifier]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 36,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    return _BuildGuid(value);
                }
            };
        }

        private static void SetSqlSmallmoney()
        {
            SqlMapping[DT.Smallmoney] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Decimal),
                ClrSubTypes = new[] {
                    typeof(System.Byte),
                    typeof(System.Int16),
                    typeof(System.Int32),
                    typeof(System.Single),
                    typeof(System.Double)
                },
                Sql = "[smallmoney]",
                SqlPlain = "smallmoney",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[smallmoney]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 12,   // 10 + floating point + minus sign (negative values only)
                DataTypeGroup = DataTypeGroup.Number,
                Build = (value, dataType) =>
                {
                    return _BuildDecimal(value);
                }
            };
        }

        private static void SetSqlReal()
        {
            SqlMapping[DT.Real] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Single),
                ClrSubTypes = new[] {
                    typeof(System.Byte),
                    typeof(System.Int16),
                    typeof(System.Int32),
                    typeof(System.Int64)
                },
                Sql = "[real]",
                SqlPlain = "real",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[real]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 40,   // 38 + floating point + minus sign (negative values only)
                DataTypeGroup = DataTypeGroup.Number,
                Build = (value, dataType) =>
                {
                    return _BuildDouble(value);
                }
            };
        }

        private static void SetSqlMoney()
        {
            SqlMapping[DT.Money] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Decimal),
                ClrSubTypes = new[] {
                    typeof(System.Byte),
                    typeof(System.Int16),
                    typeof(System.Int32),
                    typeof(System.Int64),
                    typeof(System.Single),
                    typeof(System.Double)
                },
                Sql = "[money]",
                SqlPlain = "money",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[money]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 21,   // 19 + floating point + minus sign (negative values only)
                DataTypeGroup = DataTypeGroup.Number,
                Build = (value, dataType) =>
                {
                    return _BuildDecimal(value);
                }
            };
        }

        private static void SetSqlFloat()
        {
            SqlMapping[DT.Float] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Double),
                ClrSubTypes = new[] {
                    typeof(System.Byte),
                    typeof(System.Int16),
                    typeof(System.Int32),
                    typeof(System.Int64),
                    typeof(System.Single)
                },
                Sql = "[float]",
                SqlPlain = "float",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[float]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 40,   // 38 + floating point + minus sign (negative values only)
                DataTypeGroup = DataTypeGroup.Number,
                Build = (value, dataType) =>
                {
                    return _BuildDouble(value);
                }
            };
        }

        private static void SetSqlSmalldatetime()
        {
            SqlMapping[DT.Smalldatetime] = new SqlMappingInfo()
            {
                ClrType = typeof(System.DateTime),
                ClrSubTypes = new Type[] { },
                Sql = "[smalldatetime]",
                SqlPlain = "smalldatetime",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[smalldatetime]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 19,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    if (value == null)
                    {
                        return Text.Null;
                    }

                    return String.Format("'{0:yyyy'-'MM'-'dd'T'HH':'mm':'ss}'", value);
                }
            };
        }

        private static void SetSqlDatetimeoffset()
        {
            SqlMapping[DT.Datetimeoffset] = new SqlMappingInfo()
            {
                ClrType = typeof(System.DateTimeOffset),
                ClrSubTypes = new[] {
                    typeof(System.DateTime)
                },
                Sql = "[datetimeoffset]",
                SqlPlain = "datetimeoffset",
                SizeType = SizeType.Length,
                MaxSize = 7,
                SqlByValue = (value) => { return "[datetimeoffset]"; },             
                CheckSize = (value, length, precision, scale) => { return true; },  
                MaxSizeInChars = 33,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    if (value == null)
                    {
                        return Text.Null;
                    }

                    return String.Format("'{0:o}'", value);
                }
            };
        }

        private static void SetSqlDatetime2()
        {
            SqlMapping[DT.Datetime2] = new SqlMappingInfo()
            {
                ClrType = typeof(System.DateTime),
                ClrSubTypes = new Type[] { },
                Sql = "[datetime2]",
                SqlPlain = "datetime2",
                SizeType = SizeType.Length,
                MaxSize = 7,
                SqlByValue = (value) => { return "[datetime2]"; },              
                CheckSize = (value, length, precision, scale) => { return true; },  
                MaxSizeInChars = 27,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    if (value == null)
                    {
                        return Text.Null;
                    }

                    return String.Format("'{0:yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffff}'", value);
                }
            };
        }

        private static void SetSqlDatetime()
        {
            SqlMapping[DT.Datetime] = new SqlMappingInfo()
            {
                ClrType = typeof(System.DateTime),
                ClrSubTypes = new Type[] { },
                Sql = "[datetime]",
                SqlPlain = "datetime",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[datetime]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 23,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    if (value == null)
                    {
                        return Text.Null;
                    }

                    return String.Format("'{0:yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff}'", value);
                }
            };
        }

        private static void SetSqlDate()
        {
            SqlMapping[DT.Date] = new SqlMappingInfo()
            {
                ClrType = typeof(System.DateTime),
                ClrSubTypes = new Type[] { },
                Sql = "[date]",
                SqlPlain = "date",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[date]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 10,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    if (value == null)
                    {
                        return Text.Null;
                    }
                    return String.Format("'{0:yyyy'-'MM'-'dd}'", value);
                }
            };
        }

        private static void SetSqlConcatenator()
        {
            SqlMapping[DT.Concatenator] = new SqlMappingInfo()
            {
                ClrType = typeof(System.String),
                ClrSubTypes = new Type[] { },
                Sql = "[sysname]",
                SqlPlain = "sysname",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[sysname]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 128,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    return _BuildString(value, dataType);
                }
            };
        }

        private static void SetSqlNText()
        {
            SqlMapping[DT.NText] = new SqlMappingInfo()
            {
                ClrType = typeof(System.String),
                ClrSubTypes = new Type[] { },
                Sql = "[ntext]",
                SqlPlain = "ntext",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[ntext]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 4000,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    return _BuildString(value, dataType);
                }
            };
        }

        private static void SetSqlText()
        {
            SqlMapping[DT.Text] = new SqlMappingInfo()
            {
                ClrType = typeof(System.String),
                ClrSubTypes = new Type[] { },
                Sql = "[text]",
                SqlPlain = "text",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[text]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 4000,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    return _BuildString(value, dataType);
                }
            };
        }

        private static void SetSqlXml()
        {
            SqlMapping[DT.Xml] = new SqlMappingInfo()
            {
                ClrType = typeof(System.String),
                ClrSubTypes = new Type[] { },
                Sql = "[xml]",
                SqlPlain = "xml",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[xml]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 4000,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    return _BuildString(value, dataType);
                }
            };
        }

        private static void SetSqlVarcharMax()
        {
            SqlMapping[DT.VarcharMax] = new SqlMappingInfo()
            {
                ClrType = typeof(System.String),
                ClrSubTypes = new Type[] { },
                Sql = "[varchar](MAX)",
                SqlPlain = "varchar",
                SizeType = SizeType.None,
                SqlByValue = (value) => { return "[varchar](MAX)"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 4000,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    return _BuildString(value, dataType);
                }
            };
        }

        private static void SetSqlVarchar()
        {
            SqlMapping[DT.Varchar] = new SqlMappingInfo()
            {
                ClrType = typeof(System.String),
                ClrSubTypes = new Type[] { },
                Sql = "[varchar]",
                SqlPlain = "varchar",
                SizeType = SizeType.Length,
                MaxSize = 8000,
                SqlByValue = (value) =>
                {
                    if (value == null)
                    {
                        return "[varchar]";
                    }
                    else
                    {
                        return String.Format("[varchar]({0})", value.ToString().Length);
                    }
                },
                CheckSize = (value, length, precision, scale) =>
                {
                    if (value == null)
                    {
                        return true;
                    }
                    else
                    {
                        return length >= value.ToString().Length;
                    }
                },
                MaxSizeInChars = 0,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    return _BuildString(value, dataType);
                }
            };
        }

        private static void SetSqlSysname()
        {
            SqlMapping[DT.Sysname] = new SqlMappingInfo()
            {
                ClrType = typeof(System.String),
                ClrSubTypes = new Type[] { },
                Sql = "[sysname]",
                SqlPlain = "sysname",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[sysname]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 128,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    return _BuildString(value, dataType);
                }
            };
        }

        private static void SetSqlNVarcharMax()
        {
            SqlMapping[DT.NVarcharMax] = new SqlMappingInfo()
            {
                ClrType = typeof(System.String),
                ClrSubTypes = new Type[] { },
                Sql = "[nvarchar](MAX)",
                SqlPlain = "nvarchar",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[nvarchar](MAX)"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 4000,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    return _BuildString(value, dataType);
                }
            };
        }

        private static void SetSqlNVarchar()
        {
            SqlMapping[DT.NVarchar] = new SqlMappingInfo()
            {
                ClrType = typeof(System.String),
                ClrSubTypes = new Type[] { },
                Sql = "[nvarchar]",
                SqlPlain = "nvarchar",
                SizeType = SizeType.Length,
                MaxSize = 4000,
                SqlByValue = (value) =>
                {
                    if (value == null)
                    {
                        return "[nvarchar]";
                    }
                    else
                    {
                        return String.Format("[nvarchar]({0})", value.ToString().Length);
                    }
                },
                CheckSize = (value, length, precision, scale) =>
                {
                    if (value == null)
                    {
                        return true;
                    }
                    else
                    {
                        return length >= value.ToString().Length;
                    }
                },
                MaxSizeInChars = 0,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    return _BuildString(value, dataType);
                }
            };
        }

        private static void SetSqlNChar()
        {
            SqlMapping[DT.NChar] = new SqlMappingInfo()
            {
                ClrType = typeof(System.String),
                ClrSubTypes = new Type[] { },
                Sql = "[nchar]",
                SqlPlain = "nchar",
                SizeType = SizeType.Length,
                MaxSize = 4000,
                SqlByValue = (value) =>
                {
                    if (value == null)
                    {
                        return "[nchar]";
                    }
                    else
                    {
                        return String.Format("[nchar]({0})", value.ToString().Length);
                    }
                },
                CheckSize = (value, length, precision, scale) =>
                {
                    if (value == null)
                    {
                        return true;
                    }
                    else
                    {
                        return length >= value.ToString().Length;
                    }
                },
                MaxSizeInChars = 0,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    return _BuildString(value, dataType);
                }
            };
        }

        private static void SetSqlChar()
        {
            SqlMapping[DT.Char] = new SqlMappingInfo()
            {
                ClrType = typeof(System.String),
                ClrSubTypes = new Type[] { },
                Sql = "[char]",
                SqlPlain = "char",
                MaxSize = 8000,
                SizeType = SizeType.Length,
                SqlByValue = (value) =>
                {
                    if (value == null || value.GetType() == typeof(Value))
                    {
                        return "[char]";
                    }
                    else
                    {
                        return String.Format("[char]({0})", value.ToString().Length);
                    }
                },
                CheckSize = (value, length, precision, scale) =>
                {
                    if (value == null)
                    {
                        return true;
                    }
                    else
                    {
                        return length >= value.ToString().Length;
                    }
                },
                MaxSizeInChars = 0,
                DataTypeGroup = DataTypeGroup.Text,
                Build = (value, dataType) =>
                {
                    return _BuildString(value, dataType);
                }
            };
        }

        private static void SetSqlBit()
        {
            SqlMapping[DT.Bit] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Boolean),
                ClrSubTypes = new Type[] { },
                Sql = "[bit]",
                SqlPlain = "bit",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[bit]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 1,
                DataTypeGroup = DataTypeGroup.Number,
                Build = (value, dataType) =>
                {
                    return _BuildBoolean(value);
                }
            };
        }

        private static void SetSqlTinyint()
        {
            SqlMapping[DT.Tinyint] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Byte),
                ClrSubTypes = new Type[] { },
                Sql = "[tinyint]",
                SqlPlain = "tinyint",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[tinyint]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 4,   // 3 + minus sign (negative values only)
                DataTypeGroup = DataTypeGroup.Number,
                Build = (value, dataType) =>
                {
                    return _BuildInteger(value);
                }
            };
        }

        private static void SetSqlSmallint()
        {
            SqlMapping[DT.Smallint] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Int16),
                ClrSubTypes = new[] {
                    typeof(System.Byte)
                },
                Sql = "[smallint]",
                SqlPlain = "smallint",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[smallint]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 6,   // 5 + minus sign (negative values only)
                DataTypeGroup = DataTypeGroup.Number,
                Build = (value, dataType) =>
                {
                    return _BuildInteger(value);
                }
            };
        }

        private static void SetSqlInt()
        {
            SqlMapping[DT.Int] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Int32),
                ClrSubTypes = new[] {
                    typeof(System.Byte),
                    typeof(System.Int16)
                },
                Sql = "[int]",
                SqlPlain = "int",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[int]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 11,   // 10 + minus sign (negative values only)
                DataTypeGroup = DataTypeGroup.Number,
                Build = (value, dataType) =>
                {
                    return _BuildInteger(value);
                }
            };
        }

        private static void SetSqlBigint()
        {
            SqlMapping[DT.Bigint] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Int64),
                ClrSubTypes = new[] {
                    typeof(System.Byte),
                    typeof(System.Int16),
                    typeof(System.Int32)
                },
                Sql = "[bigint]",
                SqlPlain = "bigint",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[bigint]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 19,    // 18 + minus sign (negative values only)
                DataTypeGroup = DataTypeGroup.Number,
                Build = (value, dataType) =>
                {
                    return _BuildInteger(value);
                }
            };
        }

        private static void SetSqlDecimal()
        {
            SqlMapping[DT.Decimal] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Decimal),
                ClrSubTypes = new[] {
                    typeof(System.Byte),
                    typeof(System.Int16),
                    typeof(System.Int32),
                    typeof(System.Int64),
                    typeof(System.Single),
                    typeof(System.Double)
                },
                Sql = "[decimal]",
                SqlPlain = "decimal",
                SizeType = SizeType.Precision,
                MaxSize = 38,
                SqlByValue = (value) =>
                {
                    if (value == null)
                    {
                        return "[decimal]";
                    }
                    else
                    {
                        Decimal dec = Convert.ToDecimal(value);
                        int _precision = Regex.Replace(dec.ToString(), @"[\.,]", "").Length;
                        int _scale = _precision - ((long)Decimal.Truncate(dec)).ToString().Length;
                        return String.Format("[decimal]({0},{1})", _precision, _scale);
                    }
                },
                CheckSize = (value, length, precision, scale) =>
                {
                    if (value == null)
                    {
                        return true;
                    }
                    else
                    {
                        Decimal dec = Convert.ToDecimal(value);
                        int _precision = Regex.Replace(dec.ToString(), @"[\.,]", "").Length;
                        if (precision < _precision)
                        {
                            return false;
                        }
                        int _scale = _precision - ((long)Decimal.Truncate(dec)).ToString().Length;
                        if (scale < _scale)
                        {
                            return false;
                        }
                        return true;
                    }
                },
                MaxSizeInChars = 40,   // 38 + floating point + minus sign (negative values only)
                DataTypeGroup = DataTypeGroup.Number,
                Build = (value, dataType) =>
                {
                    return _BuildDecimal(value);
                }
            };
        }

        private static void SetSqlNumeric()
        {
            // numeric
            SqlMapping[DT.Numeric] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Decimal),
                ClrSubTypes = new[] {
                    typeof(System.Byte),
                    typeof(System.Int16),
                    typeof(System.Int32),
                    typeof(System.Int64),
                    typeof(System.Single),
                    typeof(System.Double)
                },
                Sql = "[numeric]",
                SqlPlain = "numeric",
                SizeType = SizeType.Precision,
                MaxSize = 38,
                SqlByValue = (value) =>
                {
                    if (value == null)
                    {
                        return "[numeric]";
                    }
                    else
                    {
                        Decimal dec = Convert.ToDecimal(value);
                        int _precision = Regex.Replace(dec.ToString(), @"[\.,]", "").Length;
                        int _scale = _precision - ((long)Decimal.Truncate(dec)).ToString().Length;
                        return String.Format("[numeric]({0},{1})", _precision, _scale);
                    }
                },
                CheckSize = (value, length, precision, scale) =>
                {
                    if (value == null)
                    {
                        return true;
                    }
                    else
                    {
                        Decimal dec = Convert.ToDecimal(value);
                        int _precision = Regex.Replace(dec.ToString(), @"[\.,]", "").Length;
                        if (precision < _precision)
                        {
                            return false;
                        }
                        int _scale = _precision - ((long)Decimal.Truncate(dec)).ToString().Length;
                        if (scale < _scale)
                        {
                            return false;
                        }
                        return true;
                    }
                },
                MaxSizeInChars = 40,   // 38 + floating point + minus sign (negative values only)
                DataTypeGroup = DataTypeGroup.Number,
                Build = (value, dataType) =>
                {
                    return _BuildDecimal(value);
                }
            };
        }

        private static void SetSqlBinary()
        {
            SqlMapping[DT.Binary] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Byte[]),
                ClrSubTypes = new Type[] { },
                Sql = "[binary]",
                SqlPlain = "binary",
                SizeType = SizeType.Length,
                MaxSize = 8000,
                SqlByValue = (value) =>
                {
                    if (value == null)
                    {
                        return "[binary]";
                    }
                    else
                    {
                        int length = 0;
                        if (value.GetType() == typeof(System.Data.Linq.Binary))
                        {
                            length = ((System.Data.Linq.Binary)value).Length;
                        }
                        else // value is System.Byte[]
                        {
                            length = ((byte[])value).Length;
                        }
                        return string.Format("[binary]({0})", length);
                    }
                },
                CheckSize = (value, length, precision, scale) =>
                {
                    if (value == null)
                    {
                        return true;
                    }
                    else
                    {
                        if (value.GetType() == typeof(System.Data.Linq.Binary))
                        {
                            return length >= ((System.Data.Linq.Binary)value).Length;
                        }
                        else // value is System.Byte[]
                        {
                            return length >= ((byte[])value).Length;
                        }
                    }
                },
                MaxSizeInChars = 0, 
                DataTypeGroup = DataTypeGroup.Hexadecimal,
                Build = (value, dataType) =>
                {
                    return _BuildByteArray(value);
                }
            };
        }

        private static void SetSqlTimestamp()
        {
            SqlMapping[DT.Timestamp] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Byte[]),
                ClrSubTypes = new Type[] { },
                Sql = "[timestamp]",
                SqlPlain = "timestamp",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[timestamp]"; },
                CheckSize = (value, length, precision, scale) => { return true; }, 
                MaxSizeInChars = 18,
                DataTypeGroup = DataTypeGroup.Hexadecimal,
                Build = (value, dataType) =>
                {
                    return _BuildByteArray(value);
                }
            };
        }

        private static void SetSqlRowversion()
        {
            SqlMapping[DT.Rowversion] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Byte[]),
                ClrSubTypes = new Type[] { },
                Sql = "[rowversion]",
                SqlPlain = "rowversion",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[rowversion]"; },
                CheckSize = (value, length, precision, scale) => { return true; }, 
                MaxSizeInChars = 18,
                DataTypeGroup = DataTypeGroup.Hexadecimal,
                Build = (value, dataType) =>
                {
                    return _BuildByteArray(value);
                }
            };
        }

        private static void SetSqlVarbinary()
        {
            SqlMapping[DT.Varbinary] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Byte[]),
                ClrSubTypes = new Type[] { },
                Sql = "[varbinary]",
                SqlPlain = "varbinary",
                SizeType = SizeType.Length,
                MaxSize = 8000,
                SqlByValue = (value) =>
                {
                    if (value == null)
                    {
                        return "[varbinary]";
                    }
                    else
                    {
                        int length = 0;
                        if (value.GetType() == typeof(System.Data.Linq.Binary))
                        {
                            length = ((System.Data.Linq.Binary)value).Length;
                        }
                        else // value is System.Byte[]
                        {
                            length = ((byte[])value).Length;
                        }
                        return string.Format("[varbinary]({0})", length);
                    }
                },
                CheckSize = (value, length, precision, scale) =>
                {
                    if (value == null)
                    {
                        return true;
                    }
                    else
                    {
                        if (value.GetType() == typeof(System.Data.Linq.Binary))
                        {
                            return length >= ((System.Data.Linq.Binary)value).Length;
                        }
                        else // value is System.Byte[]
                        {
                            return length >= ((byte[])value).Length;
                        }
                    }
                },
                MaxSizeInChars = 0,
                DataTypeGroup = DataTypeGroup.Hexadecimal,
                Build = (value, dataType) =>
                {
                    return _BuildByteArray(value);
                }
            };
        }

        private static void SetSqlVarbinaryMax()
        {
            SqlMapping[DT.VarbinaryMax] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Byte[]),
                ClrSubTypes = new Type[] { },
                Sql = "[varbinary](MAX)",
                SqlPlain = "varbinary",
                SizeType = SizeType.None,
                SqlByValue = (value) => { return "[varbinary](MAX)"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 4000,
                DataTypeGroup = DataTypeGroup.Hexadecimal,
                Build = (value, dataType) =>
                {
                    return _BuildByteArray(value);
                }
            };
        }

        private static void SetSqlImage()
        {
            SqlMapping[DT.Image] = new SqlMappingInfo()
            {
                ClrType = typeof(System.Byte[]),
                ClrSubTypes = new Type[] { },
                Sql = "[image]",
                SqlPlain = "image",
                SizeType = SizeType.None,
                MaxSize = 0,
                SqlByValue = (value) => { return "[image]"; },
                CheckSize = (value, length, precision, scale) => { return true; },
                MaxSizeInChars = 4000,
                DataTypeGroup = DataTypeGroup.Hexadecimal,
                Build = (value, dataType) =>
                {
                    return _BuildByteArray(value);
                }
            };
        }

    }
}
