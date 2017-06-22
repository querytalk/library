#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;

namespace QueryTalk.Wall
{
    internal static partial class ViewConverter
    {
        internal static View ConvertDbRowData(IEnumerable<DbRow> rows, bool rkOnly = false, bool withRowID = false)
        {
            var first = rows.Where(row => row != null).First();

            var nodeMap = DbMapping.GetNodeMap(first.NodeID);
            if (nodeMap.ID.Equals(DB3.Default))
            {
                DbRow.ThrowInvalidDbRowException(first.GetType());
            }

            var rowCount = 0;
            var columns = new List<ViewColumnInfo>();
            var type = first.GetType();
            var properties = type.GetSortedWritableProperties(withRowID);

            var numberOfProperties = properties.Length;
            if (numberOfProperties == 0)
            {
                throw new QueryTalkException("ViewConverter.ToView<T>", QueryTalkExceptionType.InvalidDataClass,
                    String.Format("data class = {0}", type));
            }

            List<IPropertyAccessor> getters;
            bool cached;

            if (withRowID)
            {
                cached = Cache.IRowAccessors.TryGetValue(type, out getters);
            }
            else
            {
                cached = Cache.PropertyAccessors.TryGetValue(type, out getters);
            }

            if (!cached)
            {
                getters = _GetDbRowsGetters(withRowID, type, properties);
            }

            var sql = Text.GenerateSql(500)
                .Append(Text.Select).S();

            int i = 0;
            i = _BuildDbRowsOuterSelect(rkOnly, withRowID, nodeMap, columns, sql, i);

            if (withRowID)
            {
                sql.NewLineIndent(Text.Comma);
                AppendColumn(sql,
                    String.Format("CAST({0} AS [int])", Filter.DelimitNonAsterix(String.Format("{0}{1}", Text.ColumnShortName, i + 1))),
                    Text.Reserved.QtRowIDColumnName);
            }

            sql.S()
                .NewLine(Text.From).S()
                .Append(Text.LeftBracket).S();

            bool firstRow = true;
            foreach (var row in rows)
            {
                if (row == null) { continue; }

                if (!firstRow)
                {
                    sql.NewLineIndent(Text.UnionAll).S()
                        .Append(Text.Select).S();
                }
                else
                {
                    sql.NewLineIndent(Text.Select).S();
                }

                var originalValues = row.GetOriginalRKValues();
                var gi = 0;  // getter index
                int j = 0;
                _BuildDbRowsValues(rkOnly, withRowID, nodeMap, getters, sql, firstRow, row, originalValues, ref j,
                    ref gi);

                if (withRowID)
                {
                    var value = getters[gi].GetValue(row);
                    sql.Append(Text.Comma);
                    AppendColumnValue(sql, value, j + 1);
                }

                firstRow = false;
                ++rowCount;
            }

            sql.NewLine(Text.RightBracket).S()
                .Append(Text.As).S()
                .Append(Text.DelimitedTargetAlias);

            return new View(sql.ToString(), typeof(DbRow), columns.ToArray(), rowCount);
        }

        private static void _BuildDbRowsValues(bool rkOnly, bool withRowID, NodeMap nodeMap, List<IPropertyAccessor> getters, StringBuilder sql,
            bool firstRow, DbRow row, object[] originalValues, ref int j,
            ref int gi)
        {
            int ov = 0;  // original values index
            foreach (var column in nodeMap.SortedColumns)
            {
                if (rkOnly && !column.IsRK)
                {
                    ++gi;
                    continue;
                }

                if (j != 0)
                {
                    sql.Append(Text.Comma);
                }

                var value = getters[gi].GetValue(row);
                value = column.TryCorrectMinWeakDatetime(value, row);
                AppendColumnValue(sql, value, j + 1, column.DataType, (firstRow && column.DataType.DT == DT.Sqlvariant));

                ++j;
                ++gi;

                if (withRowID && column.IsRK)
                {
                    if (j != 0) { sql.Append(Text.Comma); }
                    AppendColumnValue(sql, originalValues[ov], j + 1, column.DataType, (firstRow && column.DataType.DT == DT.Sqlvariant));
                    ++j;
                    ++ov;
                }
            }
        }

        private static int _BuildDbRowsOuterSelect(bool rkOnly, bool withRowID, NodeMap nodeMap, List<ViewColumnInfo> columns,
            StringBuilder sql, int i)
        {
            foreach (var column in nodeMap.SortedColumns)
            {
                if (rkOnly && !column.IsRK)
                {
                    continue;
                }

                var columnName = column.Name.Part1;
                columns.Add(new ViewColumnInfo(columnName, column.DataType, column.IsNullable));

                if (i != 0)
                {
                    sql.NewLineIndent(Text.Comma);
                }

                var argument = String.Format("{0}{1}", Text.ColumnShortName, i + 1);

                AppendColumn(sql,
                    String.Format("CAST({0} AS {1})", Filter.DelimitNonAsterix(argument), column.DataType.Build()),
                    columnName);

                ++i;

                if (withRowID && column.IsRK)
                {
                    if (i != 0) { sql.NewLineIndent(Text.Comma); }
                    var argumentOfOriginalValue = String.Format("{0}{1}", Text.ColumnShortName, i + 1);
                    AppendColumn(sql,
                        String.Format("CAST({0} AS {1})", Filter.DelimitNonAsterix(argumentOfOriginalValue), column.DataType.Build()),
                        Common.ProvideOriginalColumnCRUD(columnName));
                    ++i;
                }
            }

            return i;
        }

        private static List<IPropertyAccessor> _GetDbRowsGetters(bool withRowID, Type type, PropertyInfo[] properties)
        {
            List<IPropertyAccessor> getters = new List<IPropertyAccessor>();
            foreach (var property in properties)
            {
                getters.Add(PropertyAccessor.Create(type, property));
            }

            if (withRowID)
            {
                Cache.IRowAccessors[type] = getters;
            }
            else
            {
                Cache.PropertyAccessors[type] = getters;
            }

            return getters;
        }

    }
}
