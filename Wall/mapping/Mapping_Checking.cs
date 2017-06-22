#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryTalk.Wall
{
    internal static partial class Mapping
    {

        // Checks if a given type is QueryTalk compliant CLR type.
        internal static ClrTypeMatch CheckClrCompliance(Type type, out Type clrType, out QueryTalkException exception)
        {
            exception = null;
            clrType = type;

            if (clrType == typeof(System.DBNull))
            {
                clrType = typeof(System.Object);     
                return ClrTypeMatch.ClrMatch;
            }

            if (type == typeof(System.Object))
            {
                clrType = type.GetType(); 

                // object does not contain any value since it is a declared type (in the class)
                if (clrType.FullName == _runtimeType)
                {
                    clrType = typeof(System.Object);  
                    return ClrTypeMatch.ClrMatch;
                }
            }

            if (clrType.IsNullable())
            {
                clrType = Nullable.GetUnderlyingType(clrType);
            }

            if (!ClrMapping.Keys.Contains(clrType))
            {
                if (clrType.IsDbRow()
                    || (clrType.IsGenericType && clrType.GetGenericTypeDefinition() == typeof(HashSet<>)))
                {
                    exception = new QueryTalkException("TypeMapping.CheckClrCompliance",
                        QueryTalkExceptionType.NonCompliantClrType, String.Format("type = {0}", clrType));
                    return ClrTypeMatch.NodeMatch;
                }

                string arguments = String.Format("Non-supported CLR type = {0}", clrType);
                if (!clrType.Equals(type))
                {
                    arguments += String.Format("{0}   CLR type wrapper = {1}", Environment.NewLine, type);
                }

                exception = new QueryTalkException("TypeMapping.CheckClrCompliance",
                    QueryTalkExceptionType.NonCompliantClrType, arguments);

                exception.Extra =
                    "The supported CLR types:" + Environment.NewLine +
                    "    - System.Boolean" + Environment.NewLine +
                    "    - System.Byte" + Environment.NewLine +
                    "    - System.Byte[]" + Environment.NewLine +
                    "    - System.DateTime" + Environment.NewLine +
                    "    - System.DateTimeOffset" + Environment.NewLine +
                    "    - System.Decimal" + Environment.NewLine +
                    "    - System.Double" + Environment.NewLine +
                    "    - System.Guid" + Environment.NewLine +
                    "    - System.Int16" + Environment.NewLine +
                    "    - System.Int32" + Environment.NewLine +
                    "    - System.Int64" + Environment.NewLine +
                    "    - System.Object" + Environment.NewLine +
                    "    - System.Single" + Environment.NewLine +
                    "    - System.String" + Environment.NewLine +
                    "    - System.TimeSpan" + Environment.NewLine +
                    "    - System.Data.Linq.Binary (partial support)";

                return ClrTypeMatch.Mismatch;
            }

            return ClrTypeMatch.ClrMatch;
        }

        // Checks if a given type is QueryTalk compliant CLR type and if it matches the given SQL type.
        internal static bool CheckSqlCompliance(Type type, DT sqlType, out QueryTalkException exception)
        {
            exception = null;
            Type clrType;

            var clrCheck = CheckClrCompliance(type, out clrType, out exception);
            if (clrCheck != ClrTypeMatch.ClrMatch)
            {
                return false;
            }

            // sql_variant can map to any supported CLR type
            if (sqlType == DT.Sqlvariant)
            {
                return true;
            }

            if (Mapping.SqlMapping[sqlType].ClrSubTypes.Contains(clrType))
            {
                return true;
            }

            SqlMappingInfo info = SqlMapping[sqlType];
            if (info.ClrType != clrType)
            {
                exception = new QueryTalkException("TypeMapping.CheckSqlCompliance",
                    QueryTalkExceptionType.ParamArgumentTypeMismatch, null);
                return false;
            }

            return true;
        }

        // Checks if a given inline type corresponds to the CLR type.
        internal static bool CheckInlineType(Type type, DT inliner, out QueryTalkException exception)
        {
            exception = null;

            if (!inliner.IsInliner())
            {
                exception = new QueryTalkException("TypeMapping.CheckInlineType",
                    QueryTalkExceptionType.ParamArgumentTypeMismatch, null);
                return false;
            }

            if (InlineMapping[inliner].Contains(type))
            {
                return true;
            }

            exception = new QueryTalkException("TypeMapping.CheckInlineType",
                QueryTalkExceptionType.ParamArgumentTypeMismatch, null);
            return false;
        }

        // Checks if argument's type matches the parameter's data type.
        internal static bool CheckArgument(Variable param, Type argumentType, out QueryTalkException exception)
        {
            exception = null;

            if (param.DT.IsNameType())
            {
                return true;
            }

            if (argumentType == null)
            {
                return true;   
            }

            if (param.DT.IsInliner())
            {
                CheckInlineType(argumentType, param.DT, out exception);
            }

            else
            {
                CheckSqlCompliance(argumentType, param.DT, out exception);
            }

            return (exception == null);
        }

    }
}
