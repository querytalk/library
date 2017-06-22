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

namespace QueryTalk.Wall
{
    // Converts .NET data to QueryTalk's data view.
    internal static partial class ViewConverter
    {

        internal static View ToView<T>(T data)
        {
            Type type = typeof(T);
            QueryTalkException exception;

            if (data != null && type == typeof(System.Object))
            {
                type = data.GetType();
            }

            if (data is ResultSet<DataTable>)
            {
                return ViewConverter.ConvertDataTable(((ResultSet<DataTable>)(object)data).ToDataTable());
            }

            if (type == typeof(Result<DataTable>) && data == null)
            {
                return ViewConverter.ConvertDataTable(null);
            }

            if (type == typeof(DataTable))
            {
                return ViewConverter.ConvertDataTable((DataTable)(object)data);
            }

            if (type.IsIResult(typeof(IResult)))
            {
                var result = (IResult)(object)data;
                object table = null;

                if (result != null)
                {
                    if (result.TableCount == 1)
                    {
                        table = data;
                    }
                    else
                    {
                        table = ((dynamic)data).Table1;
                    }
                }

                if (result == null || table == null)
                {
                    throw new QueryTalkException(
                        "ViewConverter.ToView<T>", QueryTalkExceptionType.EmptyDynamicResult,
                        String.Format("type = {0}", typeof(T)), Text.Method.ToView);
                }

                return ViewConverter.ConvertCollection<T>((IEnumerable)table);
            }

            if (type == typeof(Value))
            {
                return ViewConverter.ConvertValue((Value)(object)data);
            }

            Type clrType;
            if (QueryTalk.Wall.Mapping.CheckClrCompliance(type, out clrType, out exception)
                == QueryTalk.Wall.Mapping.ClrTypeMatch.ClrMatch)
            {
                return ViewConverter.ConvertScalar<T>(data, clrType);
            }

            if (type.GetInterfaces().Where(i => i == typeof(IEnumerable)).Any())
            {
                return ViewConverter.ConvertCollection<T>((IEnumerable)data);
            }

            if (type.IsClass)
            {
                return ViewConverter.ConvertCollection<IEnumerable<T>>((IEnumerable)(new HashSet<T>(new[] { data })));
            }

            throw new QueryTalkException("ViewConverter.ToView<T>", QueryTalkExceptionType.InvalidSerializationData,
                String.Format("type = {0}", typeof(T)), Text.Method.ToView);
        }

        private static View Finalizer(
            Type type,
            StringBuilder sqlOuter,
            StringBuilder sqlInner,
            ViewColumnInfo[] columns,
            int rowCount,
            bool isEmptyCollection)
        {
            var sql = Text.GenerateSql(1000)
                .Append(Text.Select).S();

            if (isEmptyCollection)
            {
                sql.Append(Text.TopZero).S();
            }

            sql.Append(sqlOuter.ToString());
            sql.S()
                .NewLine(Text.From).S()
                .Append(Text.LeftBracket).S();
            sql.Indent(sqlInner.ToString());
            sql.NewLine(Text.RightBracket).S()
                .Append(Text.As).S()
                .Append(Text.DelimitedTargetAlias);

            return new View(sql.ToString(), type, columns.ToArray(), rowCount);
        }

        #region Helper Methods

        private static void AppendColumn(StringBuilder sql, string argument, string alias)
        {
            sql.Append(argument)
                .Append(Text._As_)
                .Append(Text.LeftSquareBracket)
                .Append(alias)
                .Append(Text.RightSquareBracket);
        }

        private static void AppendColumnAsSqlVariant(StringBuilder sql, string argument, string alias)
        {
            sql.Append(Text.Cast)
                .Append(Text.LeftBracket)
                .Append(argument)
                .Append(Text._As_)
                .Append(Mapping.SqlMapping[DT.Sqlvariant].Sql)
                .Append(Text.RightBracket)
                .Append(Text._As_)
                .Append(alias);
        }

        private static void AppendColumnValue(StringBuilder sql, object value, int index, bool isSqlVariantFirstRow = false)
        {
            if (isSqlVariantFirstRow)
            {
                AppendColumnAsSqlVariant(sql,
                    Mapping.BuildUnchecked(value),
                    String.Format("{0}{1}", Text.ColumnShortName, index));
            }
            else
            {
                AppendColumn(sql,
                    Mapping.BuildUnchecked(value),
                    String.Format("{0}{1}", Text.ColumnShortName, index));
            }
        }

        private static void AppendColumnValue(StringBuilder sql, object value, int index, DataType dataType, bool isSqlVariantFirstRow = false)
        {
            if (isSqlVariantFirstRow)
            {
                AppendColumnAsSqlVariant(sql,
                    Mapping.Build(value, dataType),
                    String.Format("{0}{1}", Text.ColumnShortName, index));
            }
            else
            {
                AppendColumn(sql,
                    Mapping.Build(value, dataType),
                    String.Format("{0}{1}", Text.ColumnShortName, index));
            }
        }

        private static void AppendScalarColumn(StringBuilder sql, object value, Type clrType)
        {
            AppendColumn(sql,
                Text.GenerateSql(100)
                    .Append(Text.Cast)
                    .Append(Text.LeftBracket)
                    .Append(Mapping.BuildUnchecked(value ?? Designer.Null))
                    .Append(Text._As_)
                    .Append(Mapping.ClrMapping[clrType].DefaultDataType.Build())
                    .Append(Text.RightBracket)
                    .ToString(),
                Text.SingleColumnName);
        }

        private static void AppendOuterColumn(StringBuilder sql, DataType dataType, int index, string columnName)
        {
            AppendColumn(sql, String.Format("CAST({0}{1} AS {2})", Text.ColumnShortName, index,
                dataType.Build()), columnName);
        }

        private static void AppendNullValueColumn(StringBuilder sql, int index)
        {
            AppendColumn(sql, Text.Null, String.Format("{0}{1}", Text.ColumnShortName, index));
        }

        private static void AppendNullColumn(StringBuilder sql)
        {
            AppendColumn(sql, Text.Null, Text.SingleColumnName);
        }

        private static Type StoreColumnInfo(List<ViewColumnInfo> columns, string columnName, Type columnType, bool? isNullable = null)
        {
            Type clrType;
            QueryTalkException exception;
            var clrMatch = Mapping.CheckClrCompliance(columnType, out clrType, out exception);
            if (clrMatch != Mapping.ClrTypeMatch.ClrMatch)
            {
                return null;
            }

            ViewColumnInfo columnInfo = new ViewColumnInfo(columnName, columnType, clrType, isNullable);
            columns.Add(columnInfo);

            return clrType;
        }

        private static void ThrowInvalidDataClassException(Type classType)
        {
            throw new QueryTalkException("ViewConverter.ThrowInvalidDataViewException", QueryTalkExceptionType.InvalidDataClass,
                String.Format("data class = {0}", classType), Text.Method.ToView);
        }

        #endregion

    }
}
