#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;

namespace QueryTalk.Wall
{
    internal static class Common
    {
        internal static string ToUpperCS(this string value, bool isCaseSensitive = true)
        {
            if (isCaseSensitive)
            {
                return value;
            }
            else
            {
                return value.ToUpperInvariant();
            }
        }

        internal static bool EqualsCS(this string value1, string value2, bool isCaseSensitive = true)
        {
            if (value1 == null)
            {
                return value2 == null;
            }

            if (isCaseSensitive)
            {
                return String.Compare(value1, value2, StringComparison.Ordinal) == 0;
            }
            else
            {
                return String.Compare(value1, value2, StringComparison.OrdinalIgnoreCase) == 0;
            }
        }

        internal const string DatetimeMinSql = "1753-01-01T00:00:00.000";
        internal const string SmalldatetimeMinSql = "1900-01-01T00:00:00";
        internal const string DateTimeMin = "0001-01-01";

        internal static Value[] DefaultReturnValue
        {
            get
            {
                return new Value[] { 0 };
            }
        }

        internal static string AsParam(this string name, Nullable<int> index = null)
        {
            return String.Format(@"@{0}{1}", name, index);
        }

        internal static Type GetClrType(this Type type)
        {
            Type clrType;
            QueryTalkException exception;
            var match = Mapping.CheckClrCompliance(type, out clrType, out exception);
            if (match != Mapping.ClrTypeMatch.ClrMatch)
            {
                return null; 
            }

            return clrType;
        }

        internal static IdentifierValidity CheckIdentifier(string identifier)
        {
            // check length
            if (identifier == null || identifier.Length == 0 || identifier.Length > 128)
            {
                return IdentifierValidity.InvalidLength;
            }

            // check character set
            if (Regex.IsMatch(identifier, @"[^\p{L}\p{Nd}@$#_]", RegexOptions.IgnoreCase))
            {
                return IdentifierValidity.InvalidChars;
            }

            // check the first character
            string first = identifier.Substring(0, 1);
            if (Regex.IsMatch(first, @"[\p{Nd}$]", RegexOptions.IgnoreCase))
            {
                return IdentifierValidity.InvalidChars;
            }

            // check @@ and ##
            if (identifier.Length >= 2
                && (identifier.Substring(0, 2) == "@@" || identifier.Substring(0, 2) == "##"))
            {
                return IdentifierValidity.Reserved;
            }

            // check underscore (_)
            if (first == "_")
            {
                return IdentifierValidity.Reserved;
            }

            // check @
            if (first == "@")
            {
                return IdentifierValidity.Variable;
            }

            // check #
            if (first == "#")
            {
                return IdentifierValidity.TempTable;
            }

            return IdentifierValidity.RegularIdentifier;
        }

        // return true if the first letter of an identifier is the number sign (#)
        internal static bool IsTempTable(string identifier)
        {
            // check length
            if (identifier == null || identifier.Length == 0 || identifier.Length > 128)
            {
                return false;
            }

            return identifier.Substring(0, 1) == "#";
        }

        internal static TableType TryDetectTableType(Designer root, string tableName, string method = null)
        {
            root.CheckNullAndThrow(Chainer.Arg(() => tableName, tableName), method);
            var identifier = Common.CheckIdentifier(tableName);

            if (identifier == IdentifierValidity.Variable)
            {
                return TableType.Variable;
            }
            else if (identifier == IdentifierValidity.TempTable)
            {
                root.TryAddTempTable(tableName);
                return TableType.TempTable;
            }
            else
            {
                root.Throw(QueryTalkExceptionType.InvalidTableIdentifier,
                    String.Format("identifier = {0}", tableName), method);
            }

            return TableType.None;
        }

        internal static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        internal static bool CheckReservedName(object exceptionCreator, string paramName, out QueryTalkException exception)
        {
            exception = null;

            if (Regex.IsMatch(Regex.Replace(paramName, "^@+", "").ToLower(System.Globalization.CultureInfo.InvariantCulture), 
                "^" + Text.Reserved.ReservedPreffix))
            {
                exception = new QueryTalkException(exceptionCreator, QueryTalkExceptionType.ReservedNamePrefix,
                    String.Format("name = {0}", paramName));
                return false;
            }

            return true;
        }

        internal static T[] MergeArrays<T>(T firstColumn, IEnumerable<T> otherColumns)
        {
            if (otherColumns == null)
            {
                otherColumns = new T[] { default(T) };
            }

            T[] merged = new T[otherColumns.Count() + 1];
            merged[0] = firstColumn;
            otherColumns.ToArray<T>().CopyTo(merged, 1);
            return merged;
        }

        internal static bool CheckTransactionName(string nameOrVariable, Designer root, out QueryTalkException exception)
        {
            exception = null;
            if (nameOrVariable != null)
            {
                var check = Common.CheckIdentifier(nameOrVariable);
                if ((check != IdentifierValidity.RegularIdentifier
                    && check != IdentifierValidity.Variable) || nameOrVariable.Length > 32)
                {
                    exception = new QueryTalkException("Common.CheckTransactionName", QueryTalkExceptionType.InvalidTransactionName,
                        String.Format("nameOrVariable = {0}", nameOrVariable));
                    return false;
                }

                if (check == IdentifierValidity.Variable)
                {
                    if (root.VariableExists(nameOrVariable) == false)
                    {
                        exception = new QueryTalkException("Common.CheckTransactionName", QueryTalkExceptionType.ParamOrVariableNotDeclared,
                            String.Format("nameOrvariable = {0}", nameOrVariable));
                        return false;
                    }
                }
                else
                {
                    if (!CheckReservedName("Common.CheckTransactionName", nameOrVariable, out exception))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        internal static bool IsIResult(this Type type, Type @interface)
        {
            return type.GetInterfaces().Where(t => t == @interface).Any();
        }

        internal static RichTextBox AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
            return box;
        }

        internal static string BuildTableName(int index)
        {
            return String.Format("{0}{1}", Text.Free.Table, index);
        }

        internal static void CheckNullAndThrow(this object arg, string argument, string method)
        {
            if (arg == null)
            {
                throw new QueryTalkException("Common.CheckNullAndThrow", QueryTalkExceptionType.ArgumentNull,
                    String.Format("{0} = null", argument), method);
            }
        }

        internal static void CheckWithAndThrow(this DbNode node, string argument, string method)
        {
            if (node == null)
            {
                throw new QueryTalkException("Common.CheckTable", QueryTalkExceptionType.ArgumentNull,
                    String.Format("{0} = null", argument), method);
            }

            if (!(node is ITable))
            {
                throw new QueryTalkException("Common.CheckTable", QueryTalkExceptionType.InvalidWith,
                    String.Format("{0} = {1}", argument, DbMapping.GetNodeName(node.NodeID)), method);
            }
        }

        internal static bool IsTable(this DbNode node)
        {
            return node is ITable;
        }

        internal static bool FindDuplicates(IEnumerable<string> items, out IEnumerable<string> duplicates)
        {
            duplicates =
                (from a in items
                 where a != null 
                    && a != Wall.Text.Asterisk  
                 group a by a into g
                 where g.Count() > 1
                 select g.Key).ToList();

            return duplicates.Count() > 0;
        }

        internal static bool IsDbRow(this Type type)
        {
            return type.BaseType == typeof(DbRow);
        }

        internal static string ProvideOriginalColumnCRUD(string columnName)
        {
            return String.Format("{0}_{1}", columnName, Text.Reserved.QtRowIDColumnName);
        }

        internal static bool IsBegin(this ExpressionGroupType expressionGroupType)
        {
            return expressionGroupType == ExpressionGroupType.Begin || expressionGroupType == ExpressionGroupType.BeginEnd;
        }

        internal static bool IsEnd(this ExpressionGroupType expressionGroupType)
        {
            return expressionGroupType == ExpressionGroupType.End || expressionGroupType == ExpressionGroupType.BeginEnd;
        }

        internal static bool HasDefaultQuantifier(this Predicate predicate)
        {
            return predicate.PredicateType == Wall.PredicateType.Existential;
        }

        internal static bool IsPureSubject(this DbNode node)
        {
            return (!(node.HasExpression || node.Next != null || node.Prev != null || ((IPredicate)node).HasPredicate));
        }

        internal static object TryCorrectMinWeakDatetime(this ColumnMap column, object value, DbRow row)
        {
            if (Admin.IsDateTimeMinCorrectionOn                   
                && value != null                                  
                && column.DataType.DT.IsWeakDateTime()                  
                && (DateTime)value == DateTime.MinValue        
                && !row.SetColumns.Contains(column.ID.ColumnZ))    
            {
                // datetime:
                if (column.DataType.DT == DT.Datetime)
                {
                    return Common.DatetimeMinSql;
                }
                // smalldatetime:
                else
                {
                    return Common.DatetimeMinSql;
                }
            }
            else
            {
                return value;
            }
        }

        internal static string DelimitJsonString(this string value)
        {
            return String.Format("\"{0}\"", value);
        }

        internal static Type TryGetSerializationItemType(Type ctype, IEnumerable data, string method)
        {
            if (ctype.IsArray)
            {
                return ctype.GetElementType();
            }
            else if (ctype.IsGenericType)
            {
                var type = ctype.GetGenericArguments()[0];
                if (type == typeof(System.Object))
                {
                    type = _GetItemType(data);
                    if (type == null)
                    {
                        return null;
                    }
                }

                return type;
            }
            else if (ctype == typeof(Result))
            {
                var type = _GetItemType(data);
                if (type == null)
                {
                    return null;
                }

                return type;
            }

            throw new QueryTalkException("Common.TryGetSerializationItemType",
                QueryTalkExceptionType.InvalidSerializationData, String.Format("type = {0}", ctype), method);
        }

        private static Type _GetItemType(IEnumerable data)
        {
            object nonNullItem = null;
            if (data != null)
            {
                foreach (var item in data)
                {
                    nonNullItem = item;
                    if (!item.IsUndefined())
                    {
                        break;
                    }
                }
            }

            return (nonNullItem != null) ? nonNullItem.GetType() : null;
        }

    }
}
