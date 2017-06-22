#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace QueryTalk.Wall
{
    internal static partial class ViewConverter
    {
        internal static View ConvertCollection<T>(IEnumerable data)
        {
            var ctype = typeof(T);                                                      
            var type = Common.TryGetSerializationItemType(ctype, data, Text.Method.ToView); 

            if (type == null)
            {
                throw new QueryTalkException("Common.TryGetSerializationItemType",
                    QueryTalkExceptionType.EmptyDynamicResult, String.Format("type = {0}", ctype), Text.Method.ToView);
            }

            Type clrType = null;                                                      
            QueryTalkException exception;
            List<ViewColumnInfo> columns = new List<ViewColumnInfo>();

            bool isScalar = (Mapping.CheckClrCompliance(type, out clrType, out exception) == Mapping.ClrTypeMatch.ClrMatch);

            bool isDataValue = type == typeof(Value);
            if (isDataValue)
            {
                isScalar = true;
                clrType = typeof(System.Object);
            }

            bool isEmpty = _CheckIfEmpty(data, isScalar);
            var properties = type.GetReadableProperties();
            List<IPropertyAccessor> getters = null;
            int numberOfProperties = 0;
            var sqlOuter = Text.GenerateSql(500);

            var sqlEmpty = Text.GenerateSql(200)
                .NewLineIndent(Text.Select).S();

            // outer select:
            if (isScalar)
            {
                _BuildScalarOuterSelect(type, clrType, columns, sqlOuter);
            }
            else 
            {
                _BuildClassOuterSelect(type, ref clrType, ref exception, columns, isEmpty, properties, out getters,
                    out numberOfProperties, sqlOuter, sqlEmpty);
            }

            // inner select:
            var sqlInner = Text.GenerateSql(500);
            int rowCount = 0;
            if (!isEmpty)
            {
                bool firstRow = true;
                foreach (var row in data)
                {
                    if (!isScalar && row == null)
                    {
                        continue;
                    }

                    _BuildSelect(sqlInner, firstRow);

                    if (!isScalar)
                    {
                        _BuildClassValues(columns, properties, getters, numberOfProperties, sqlInner, firstRow, row);
                    }
                    else
                    {
                        AppendColumn(sqlInner, Mapping.BuildUnchecked(row), Text.SingleColumnShortName);
                    }

                    firstRow = false;
                    ++rowCount;
                }
            }
            else
            {
                if (isScalar)
                {
                    AppendColumn(sqlEmpty, Text.Null, Text.SingleColumnShortName);
                }

                sqlInner.Append(sqlEmpty.ToString());
            }

            return Finalizer(type, sqlOuter, sqlInner, columns.ToArray(), rowCount, isEmpty);
        }

        private static void _BuildSelect(StringBuilder sqlInner, bool firstRow)
        {
            if (!firstRow)
            {
                sqlInner.NewLineIndent(Text.UnionAll).S()
                    .Append(Text.Select).S();
            }
            else
            {
                sqlInner.NewLineIndent(Text.Select).S();
            }
        }

        private static void _BuildClassValues(List<ViewColumnInfo> columns, PropertyInfo[] properties, List<IPropertyAccessor> getters,
            int numberOfProperties, StringBuilder sqlInner, bool firstRow, object row)
        {
            for (int j = 0; j < numberOfProperties; ++j)
            {
                if (j != 0)
                {
                    sqlInner.Append(Text.Comma);
                }

                var value = getters[j].GetValue(row);
                var column = columns[j];
                bool isSqlVariantFirstRow = firstRow && properties[j].PropertyType == typeof(object);  
                AppendColumnValue(sqlInner, value, j + 1, column.DataType, isSqlVariantFirstRow);
            }
        }

        private static void _BuildScalarOuterSelect(Type type, Type clrType, List<ViewColumnInfo> columns, StringBuilder sqlOuter)
        {
            var info = Mapping.ClrMapping[clrType];
            var castAs = String.Format("CAST ([C1] AS {0})", info.DefaultDataType.Build());
            AppendColumn(sqlOuter, castAs, Text.SingleColumnName);
            ViewColumnInfo column = new ViewColumnInfo(Text.SingleColumnName, type, clrType);
            columns.Add(column);
        }

        private static void _BuildClassOuterSelect(Type type, ref Type clrType, ref QueryTalkException exception, List<ViewColumnInfo> columns,
            bool isEmpty, PropertyInfo[] properties, out List<IPropertyAccessor> getters, out int numberOfProperties,
            StringBuilder sqlOuter, StringBuilder sqlEmpty)
        {
            numberOfProperties = properties.Length;
            if (numberOfProperties == 0)
            {
                throw new QueryTalkException("ViewConverter.ToView<T>", QueryTalkExceptionType.InvalidDataClass,
                    String.Format("data class = {0}", type));
            }

            bool cached = Cache.PropertyAccessors.TryGetValue(type, out getters);
            if (!cached)
            {
                getters = new List<IPropertyAccessor>();
            }

            NodeMap rowMap = null;
            bool isDbRow = type.IsDbRow();
            if (isDbRow)
            {
                rowMap = DbMapping.GetNodeMap(type);
                if (rowMap.ID.Equals(DB3.Default))
                {
                    DbRow.ThrowInvalidDbRowException(type);
                }
            }

            // outer select:
            int i = 0;
            foreach (var property in properties)
            {
                string column;

                var clrTypeMatch = Mapping.CheckClrCompliance(property.PropertyType, out clrType, out exception);
                if (clrTypeMatch != Mapping.ClrTypeMatch.ClrMatch)
                {
                    continue;
                }

                ViewColumnInfo columnInfo;
                if (isDbRow)
                {
                    var rowColumn = rowMap.Columns.Where(a => a.ID.ColumnZ == i + 1).First();
                    column = rowColumn.Name.Part1;
                    columnInfo = new ViewColumnInfo(column, rowColumn.DataType, rowColumn.IsNullable);
                    columns.Add(columnInfo);
                }
                else
                {
                    column = property.Name;
                    columnInfo = new ViewColumnInfo(column, property.PropertyType, clrType);
                    columns.Add(columnInfo);
                }

                if (i != 0)
                {
                    sqlOuter.NewLineIndent(Text.Comma);
                    sqlEmpty.Append(Text.Comma);
                }

                var dataType = Mapping.ProvideDataType(columnInfo.DataType, clrType);
                AppendOuterColumn(sqlOuter, dataType, i + 1, column);

                if (isEmpty)
                {
                    AppendNullValueColumn(sqlEmpty, i + 1);
                }

                if (!cached)
                {
                    getters.Add(PropertyAccessor.Create(type, property));
                }

                ++i;
            }

            numberOfProperties = i;
            if (numberOfProperties == 0)
            {
                ThrowInvalidDataClassException(type);
            }

            if (!cached)
            {
                Cache.PropertyAccessors[type] = getters;
            }
        }

        private static bool _CheckIfEmpty(IEnumerable data, bool isScalar)
        {
            bool isEmpty = true;
            if (data != null)
            {
                foreach (var item in data)
                {
                    if (isScalar)
                    {
                        isEmpty = false;
                        break;
                    }

                    // class:
                    if (!item.IsUndefined())
                    {
                        isEmpty = false;
                        break;
                    }
                }
            }

            return isEmpty;
        }

    }
}
