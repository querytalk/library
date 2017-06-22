#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QueryTalk.Wall
{
    static partial class Crud
    {
        // attention:
        //   rowversion (timestamp) column is returned as NULL.
        private static Procedure InsertProc<T>(ColumnMap[] insertableColumns, Column[] valueParams, bool identityInsert, 
            Column[] outputColumns, Column[] selectColumns)
            where T : DbRow
        {
            return Designer.GetNewDesigner(Text.Method.InsertGo, true, true)
                .ParamNodeColumns(insertableColumns, "v")
                .Param("@Table", Designer.Inliner.Table)             
                .Param("@Columns", Designer.Inliner.Column)        

                .DesignTable<T>("@Output", true).EndDesign()

                .SetIdentityInsert("@Table", true).End(identityInsert)
                .Collect(valueParams)
                .IntoColumns("@Columns")
                .Insert("@Table").Output(outputColumns).Into("@Output")
                .SetIdentityInsert("@Table", false).End(identityInsert)

                .From("@Output").Select(selectColumns)

                .Return(1)
                .EndProc();
        }

        private static Procedure InsertProcDefaultValues<T>(Column[] outputColumns, Column[] selectColumns)
            where T : DbRow
        {
            return Designer.GetNewDesigner(Text.Method.InsertGo, true, true)
                .Param("@Table", Designer.Inliner.Table)

                .DesignTable<T>("@Output", true).EndDesign()

                .Collect(Designer.DefaultValues)
                .Insert("@Table").Output(outputColumns).Into("@Output")

                .From("@Output").Select(selectColumns)

                .Return(1)
                .EndProc();
        }

        private static Connectable GetInsertBody<T>(Assembly client, DbRow row, bool identityInsert, ConnectBy connectBy,
            string method, ref string name)
            where T : DbRow
        {
            if (row == null)
            {
                throw new QueryTalkException("Crud", QueryTalkExceptionType.ArgumentNull, "row = null", method);
            }

            Crud.CheckTable(row, method);

            List<ParameterArgument> args = new List<ParameterArgument>();

            var map = DbMapping.TryGetNodeMap(row);
            name = map.Name.Sql;

            if (!map.HasIdentity)
            {
                identityInsert = false;
            }

            var insertableColumns = map.GetInsertableColumns(row, identityInsert);
            Column[] outputColumns;
            Column[] selectColumns;
            _getColumnsWithoutRowversion(map, out outputColumns, out selectColumns);

            PassChainer cpass;
            if (insertableColumns.Length > 0)
            {
                var currentValues = PropertyAccessor.GetValues(row);
                var valueParams = new Column[insertableColumns.Length];
                int j = 0;
                foreach (var column in insertableColumns)
                {
                    var value = new Value(currentValues[column.ID.ColumnZ - 1]);
                    value.Original = column.TryCorrectMinWeakDatetime(value.Original, row);
                    args.Add(new ParameterArgument(value));
                    valueParams[j] = String.Format("@v{0}", j + 1);
                    ++j;
                }

                args.Add(map.Name);
                args.Add(insertableColumns
                    .Select(a => new Column(a.Name))
                    .ToArray());

                cpass = InsertProc<T>(insertableColumns, valueParams, identityInsert, outputColumns, selectColumns)
                    .Pass(args.ToArray());
            }
            // default values:
            else
            {
                cpass = InsertProcDefaultValues<T>(outputColumns, selectColumns).Pass(map.Name);
            }

            cpass.SetRootMap(row.NodeID);              
            return Reader.GetConnectable(client, row, cpass, connectBy);
        }

        internal static Result<T> InsertGo<T>(Assembly client, DbRow row, bool identityInsert, ConnectBy connectBy)
            where T : DbRow
        {
            var name = Text.NotAvailable;

            try
            {
                var connectable = GetInsertBody<T>(client, row, identityInsert, connectBy, Text.Method.InsertGo, ref name);
                var result = connectable.Go<T>();
                if (result.RowCount > 0)
                {
                    PropertyAccessor.SetValues(row, PropertyAccessor.GetValues(result.First()));
                    row.SetStatus(DbRowStatus.Loaded);

                    // has rowversion & it is not RK
                    if (row.HasNonRKRowversion)
                    {
                        Crud.ReloadGo<T>(client, row, false, connectBy, true);
                    }
                    else
                    {
                        // rowversion column is null: 
                        var map = DbMapping.TryGetNodeMap(row);
                        if (map.HasRowversion && row.GetRowversionValue() == null)
                        {
                            row.SetStatus(DbRowStatus.Faulted);
                        }
                        else
                        {
                            row.SetStatus(DbRowStatus.Loaded);
                        }
                    }
                }

                result.FinalizeCrud();
                return result;
            }
            catch (QueryTalkException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw Crud.ClrException(ex, name, Text.Method.InsertGo);
            }
        }

        internal static Async<Result<T>> InsertGoAsync<T>(Assembly client, DbRow row, bool identityInsert, ConnectBy connectBy,
            Action<Result<T>> onCompleted)
            where T : DbRow
        {
            var name = Text.NotAvailable;

            try
            {
                var connectable = GetInsertBody<T>(client, row, identityInsert, connectBy, Text.Method.InsertGoAsync, ref name);

                return connectable.GoAsync<T>(result =>
                {
                    if (result.RowCount > 0)
                    {
                        PropertyAccessor.SetValues(row, PropertyAccessor.GetValues(result.First()));
                        row.SetStatus(DbRowStatus.Loaded);
                    }

                    result.FinalizeCrud();
                    onCompleted?.Invoke(result);
                });
            }
            catch (QueryTalkException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw Crud.ClrException(ex, name, Text.Method.InsertGoAsync);
            }
        }

        private static void _getColumnsWithoutRowversion(NodeMap map, out Column[] outputColumns, out Column[] selectColumns)
        {
            List<Column> ocols = new List<Column>();
            List<Column> scols = new List<Column>();
            foreach (var column in map.SortedColumns)
            {
                // skip rowversion
                if (column.DataType.DT.IsRowversion())
                {
                    continue;
                }

                scols.Add(String.Format(column.Name.Part1));
                ocols.Add(String.Format("Inserted.{0}", column.Name.Part1));
            }

            outputColumns = ocols.ToArray();
            selectColumns = scols.ToArray();
        }

    }
}
