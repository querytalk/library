#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace QueryTalk.Wall
{
    internal static partial class Mapping
    {

        #region Checked (main internal checked method)

        // returns final output SQL formatted value
        internal static string Build(object value, DataType dataType)
        {
            var info = SqlMapping[dataType.DT];
            return info.Build(value, dataType);
        }

        internal static string Build(object value, Type clrType)
        {
            return Build(value, ClrMapping[clrType].DefaultDataType);
        }

        internal static string BuildCast(object value, DataType dataType)
        {
            var info = SqlMapping[dataType.DT];
            var sql = info.Build(value, dataType);
            return String.Format("CAST({0} AS {1})", sql, dataType.ToString());
        }

        internal static string BuildTestValue(string value, DataType dataType)
        {
            var info = SqlMapping[dataType.DT];

            if (value.ToUpperInvariant() == Text.Null.ToUpperInvariant())
            {
                return Text.Null;
            }

            var sql = Filter.Escape(value);

            if (dataType.DT.IsDateTime())
            {
                DateTime dateTime;
                if (DateTime.TryParse(value, out dateTime))
                {
                    sql = Build(dateTime, dataType);
                }
                else
                {
                    return sql;  
                }
            }
            else if (info.DataTypeGroup == DataTypeGroup.Text)
            {
                if (dataType.DT.IsUnicode())
                {
                    sql = String.Format("N'{0}'", sql);
                }
                else
                {
                    sql = String.Format("'{0}'", sql);
                }
            }
            else if (info.DataTypeGroup == DataTypeGroup.Number)
            {
                if (dataType.DT.IsDecimalNumber())
                {
                    sql = Regex.Replace(value, ",", ".");
                }
            }
            // sql_variant: treat as string
            else if (dataType.DT == DT.Sqlvariant)
            {
                return String.Format("N'{0}'", sql);
            }

            return sql;
        }

        #endregion

        #region Unchecked (main internal unchecked method)

        // builds unchecked value
        internal static string BuildUnchecked(object value)
        {
            QueryTalkException exception;

            var sql = BuildUnchecked(value, out exception);

            if (exception != null)
            {
                throw exception;
            }

            return sql;
        }

        internal static string BuildUnchecked(object value, out QueryTalkException exception)
        {
            exception = null;

            if (value == null)
            {
                return Text.Null;
            }

            Type type = value.GetType();

            if (type == typeof(DBNull) || type == typeof(System.Object))
            {
                return Text.Null;
            }

            if (type == typeof(View))
            {
                return ((View)value).Sql;
            }

            if (type == typeof(Value))
            {
                var data = (Value)value;

                if (data == Designer.Null)
                {
                    return Text.Null;
                }

                type = data.ClrType;
                value = data.Original;
            }

            Type clrType;
            if (Mapping.CheckClrCompliance(type, out clrType, out exception) == ClrTypeMatch.ClrMatch)
            {
                if (clrType == typeof(System.Object))
                {
                    return Text.Null;
                }

                return Build(value, clrType);  
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Clr builders

        internal static string BuildCast(bool value)
        {
            return BuildCast(value, DefaultBooleanType);
        }

        internal static string BuildCast(byte value)
        {
            return BuildCast(value, DefaultInt16Type);
        }

        internal static string BuildCast(int value)
        {
            return BuildCast(value, DefaultInt32Type);
        }

        internal static string BuildCast(long value)
        {
            return BuildCast(value, DefaultInt64Type);
        }

        internal static string BuildCast(decimal value)
        {
            return BuildCast(value, DefaultDecimalType);
        }

        internal static string BuildCast(float value)
        {
            return BuildCast(value, DefaultSingleType);
        }

        internal static string BuildCast(double value)
        {
            return BuildCast(value, DefaultDoubleType);
        }

        internal static string BuildCast(byte[] value)
        {
            return BuildCast(value, DefaultBinaryType);
        }

        internal static string BuildCast(string value)
        {
            return BuildCast(value, DefaultStringType);
        }

        internal static string BuildCast(DateTime value)
        {
            return BuildCast(value, DefaultDateTimeType);
        }

        internal static string BuildCast(DateTimeOffset value)
        {
            return BuildCast(value, DefaultDateTimeOffsetType);
        }

        internal static string BuildCast(Guid value)
        {
            return BuildCast(value, DefaultGuidType);
        }

        internal static string BuildCast(TimeSpan value)
        {
            return BuildCast(value, DefaultTimeSpanType);
        }

        #endregion

        #region Core builders
        // Contains methods specialized for each type of SQL serialization.
        // They actually build the serialized value.

        private static string _BuildString(object value, DataType dataType)
        {
            if (value.IsNullOrDbNull())
            {
                return Text.Null;
            }

            var sql = Filter.DelimitQuote(value.ToString()); 
            if (dataType.DT.IsUnicode())
            {
                return Text.N + sql;
            }
            else
            {
                return sql;
            }
        }

        private static string _BuildBoolean(object value)
        {
            if (value.IsNullOrDbNull())
            {
                return Text.Null;
            }

            return value.ToString().ToUpperInvariant() == "TRUE" ? "1" : "0";
        }

        private static string _BuildInteger(object value)
        {
            if (value.IsNullOrDbNull())
            {
                return Text.Null;
            }

            return Filter.Escape(value.ToString());
        }

        private static string _BuildDecimal(object value)
        {
            if (value.IsNullOrDbNull())
            {
                return Text.Null;
            }

            decimal value2 = 0;

            try
            {
                value2 = Convert.ToDecimal(value);
            }
            catch
            {
                throw ClrConversionFailed(value, typeof(decimal));
            }

            return Filter.Escape(value2.ToString(CultureInfo.InvariantCulture));
        }

        private static string _BuildDouble(object value)
        {
            if (value.IsNullOrDbNull())
            {
                return Text.Null;
            }

            double value2 = 0;

            try
            {
                value2 = Convert.ToDouble(value);
            }
            catch
            {
                throw ClrConversionFailed(value, typeof(double));
            }

            // SQL server incompatible values 
            if (Double.IsInfinity(value2) || Double.IsNaN(value2))
            {
                throw new QueryTalkException("_BuildDouble", QueryTalkExceptionType.InfiniteValueException,
                    "type = double");
            }

            return value2.ToString(CultureInfo.InvariantCulture);
        }

        private static string _BuildByteArray(object value)
        {
            if (value.IsNullOrDbNull())
            {
                return Text.Null;
            }

            byte[] bytes;

            if (value is byte[])
            {
                bytes = (byte[])value;
            }
            else if (value is System.Data.Linq.Binary)
            {
                bytes = ((System.Data.Linq.Binary)value).ToArray();
            }
            else
            {
                throw ClrConversionFailed(value, typeof(byte[]));
            }

            if (bytes.Length == 0)
            {
                return Text.Null;
            }

            StringBuilder hex = new StringBuilder("0x", bytes.Length * 2 + 2);
            foreach (byte b in bytes)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }

        private static string _BuildGuid(object value)
        {
            if (value.IsNullOrDbNull())
            {
                return Text.Null;
            }

            return Filter.DelimitQuote(value.ToString());   
        }

        private static string _BuildTimeSpan(object value)
        {
            if (value.IsNullOrDbNull())
            {
                return Text.Null;
            }

            if (value is TimeSpan)
            {
                var value2 = (TimeSpan)value;

                // SQL server incompatible values
                if (value2.TotalMilliseconds < 0 || value2.TotalMilliseconds >= 86400000)
                {
                    throw new QueryTalkException("_Build.TimeSpan", QueryTalkExceptionType.TimeOutOfRange,
                        String.Format("TimeSpan value = {0}", value2));
                }

                return String.Format(CultureInfo.InvariantCulture, "'{0:T}'", value);
            }

            throw ClrConversionFailed(value, typeof(TimeSpan));
        }

        #endregion

        #region Exceptions

        private static QueryTalkException ClrConversionFailed(object value, Type toType)
        {
            return new QueryTalkException("Mapping", QueryTalkExceptionType.ClrConversionFailed,
                String.Format("from value = {0}{1}   to type = {2}", value, Environment.NewLine, toType));
        }

        #endregion

    }
}
