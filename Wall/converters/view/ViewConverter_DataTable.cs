#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace QueryTalk.Wall
{
    internal static partial class ViewConverter
    {
        internal static View ConvertDataTable(DataTable dataTable)
        {
            int rowCount = 0;

            if (dataTable == null)
            {
                throw new QueryTalkException("ViewConverter.ToView", QueryTalkExceptionType.ArgumentNull,
                    "dataTable = null", Text.Method.ToView);
            }

            if (dataTable.Columns.Count == 0)
            {
                throw new QueryTalkException("ViewConverter.ToView", QueryTalkExceptionType.InvalidDataTable,
                    "type = DataTable", Text.Method.ToView);
            }

            var columns = new List<ViewColumnInfo>();
            var rows = dataTable.AsEnumerable();
            int numberOfColumns = dataTable.Columns.Count;

            var sqlOuter = Text.GenerateSql(500);
            var sqlEmpty = Text.GenerateSql(200)
                .NewLineIndent(Text.Select).S();

            int i = 0;
            i = _BuildDataTableOuterSelect(dataTable, columns, sqlOuter, i);
            numberOfColumns = i;

            if (numberOfColumns == 0)
            {
                ThrowInvalidDataClassException(typeof(DataTable));
            }

            var sqlInner = Text.GenerateSql(500);
            bool isEmpty = rows.Count() == 0;
            bool firstRow = true;

            if (!isEmpty)
            {
                _BuildDataTableValues(ref rowCount, columns, rows, sqlInner, ref firstRow);
            }
            else
            {
                _BuildDataTableEmpty(dataTable, sqlEmpty);
                sqlInner.Append(sqlEmpty);
            }

            return Finalizer(typeof(DataTable), sqlOuter, sqlInner, columns.ToArray(), rowCount, isEmpty);
        }

        private static void _BuildDataTableEmpty(DataTable dataTable, StringBuilder sqlEmpty)
        {
            int e = 0;
            foreach (DataColumn column in dataTable.Columns)
            {
                if (e != 0)
                {
                    sqlEmpty.Append(Text.Comma);
                }

                AppendNullValueColumn(sqlEmpty, e + 1);
                ++e;
            }
        }

        private static void _BuildDataTableValues(ref int rowCount, List<ViewColumnInfo> columns, EnumerableRowCollection<DataRow> rows, StringBuilder sqlInner, ref bool firstRow)
        {
            foreach (var row in rows)
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

                var j = 0;
                foreach (var column in columns)
                {
                    if (j != 0)
                    {
                        sqlInner.Append(Text.Comma);
                    }

                    var value = row[column.ColumnName];
                    AppendColumnValue(sqlInner, value, j + 1, column.DataType);
                    ++j;
                }

                firstRow = false;
                ++rowCount;
            }
        }

        private static int _BuildDataTableOuterSelect(DataTable dataTable, List<ViewColumnInfo> columns, StringBuilder sqlOuter, int i)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                string columnName = column.ColumnName;
                var clrType = StoreColumnInfo(columns, columnName, column.DataType, true);
                if (clrType == null)
                {
                    continue;
                }

                if (i != 0)
                {
                    sqlOuter.NewLineIndent(Text.Comma);
                }

                var info = Mapping.ClrMapping[clrType];
                AppendOuterColumn(sqlOuter, info.DefaultDataType, i + 1, columnName);
                ++i;
            }

            return i;
        }
    }
}
