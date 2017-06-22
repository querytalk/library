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
        // for RK with a rowversion column
        private static Procedure UpdateProc<T>(NodeMap map, ColumnSelector selector, Column[] valueParams, ColumnMap[] modifiedColumns,
            Column[] outputColumns, Column[] selectColumns)
            where T : DbRow
        {
            return Designer.GetNewDesigner(Text.Method.UpdateGo, true, true)
                .ParamNodeColumns(map.ID, selector)                 // rowMatch predicate columns
                .ParamNodeColumns(modifiedColumns, "v")             // values in .Select method
                .Param("@Table", Designer.Inliner.Table)            // target table
                .Param("@Where", Designer.Inliner.Expression)       // optimistic predicate
                .Param("@Columns", Designer.Inliner.Column)         // column in .IntoColumns method

                .DesignTable<T>("@Output", true).EndDesign()

                .From("@Table")
                .Where("@Where")
                .Select(valueParams)
                .IntoColumns("@Columns")
                .Update().Output(outputColumns).Into("@Output")

                .From("@Output").Select(selectColumns)

                .Declare<int>("@r").Set(Designer.RowCount)
                .Return("@r")
                .EndProc();
        }

        private static Connectable GetUpdateBody<T>(Assembly client, DbRow row, bool forceMirroring, ConnectBy connectBy,
            string method, ref string name, out NodeMap map)
            where T : DbRow
        {
            if (row == null)
            {
                throw new QueryTalkException("Crud", QueryTalkExceptionType.ArgumentNull, "row = null", method);
            }

            Crud.CheckTable(row, method);

            ColumnSelector selector = forceMirroring ? ColumnSelector.All : ColumnSelector.RK;

            List<ParameterArgument> args = new List<ParameterArgument>();
            map = DbMapping.TryGetNodeMap(row);
            name = map.Name.Sql;

            if (forceMirroring)
            {
                if (!row.GetStatus().IsUpdatable())
                {
                    var arguments = map.BuildExceptionReport(row.GetOriginalRKValues(), row.GetOriginalRowversionValue());
                    throw new QueryTalkException("Crud.UpdateGo", QueryTalkExceptionType.InvalidMirroring,
                        arguments, Text.Method.UpdateGo).SetObjectName(map.Name.Sql);
                }
            }

            object[] originalValues;

            if (selector == ColumnSelector.All)
            {
                if (map.HasRowversion)
                {
                    originalValues = new object[] { row.GetOriginalRowversionValue() };
                }
                else
                {
                    originalValues = row.GetOriginalValues();
                }
            }
            // RK selector
            else
            {
                originalValues = row.GetOriginalRKValues();
            }

            for (int i = 0; i < originalValues.Length; ++i)
            {
                args.Add(new ParameterArgument(new Value(originalValues[i])));
            }

            var currentValues = PropertyAccessor.GetValues(row);
            var updatableColumns = row.GetUpdatableColumns(forceMirroring);
            if (updatableColumns == null || updatableColumns.Length == 0)
            {
                return null;
            }

            var valueParams = new Column[updatableColumns.Length];
            int j = 0;
            foreach (var column in updatableColumns)
            {
                var value = new Value(currentValues[column.ID.ColumnZ - 1]);
                args.Add(new ParameterArgument(value));
                valueParams[j] = String.Format("@v{0}", j + 1);
                ++j;
            }

            args.Add(map.Name);
            if (selector == ColumnSelector.All)
            {
                args.Add(new ParameterArgument(map.BuildOptimisticPredicate(originalValues, 1)));
            }
            else
            {
                args.Add(new ParameterArgument(map.BuildRKPredicate(originalValues, 1)));
            }

            int[] modified = updatableColumns.Select(a => a.ID.ColumnZ).ToArray();

            args.Add(map.Columns
                .Where(a => modified.Contains(a.ID.ColumnZ))
                .OrderBy(a => a.ID.ColumnZ)
                .Select(a => new Column(a.Name))
                .ToArray());

            Column[] outputColumns;
            Column[] selectColumns;
            _getColumnsWithoutRowversion(map, out outputColumns, out selectColumns);

            var cpass = UpdateProc<T>(map, selector, valueParams, updatableColumns, outputColumns, selectColumns).Pass(args.ToArray());
            cpass.SetRootMap(row.NodeID);
            return Reader.GetConnectable(client, row, cpass, connectBy);
        }

        internal static Result<T> UpdateGo<T>(Assembly client, DbRow row, bool forceMirroring, ConnectBy connectBy)
            where T : DbRow
        {
            var name = Text.NotAvailable;
            NodeMap map;

            try
            {
                var connectable = GetUpdateBody<T>(client, row, forceMirroring, connectBy, Text.Method.UpdateGo, ref name, out map);

                // no columns to modify?
                if (connectable == null)
                {
                    return new Result<T>(false, 0);
                }

                var result = connectable.Go<T>();

                // success
                if (result.ReturnValue == 1)
                {
                    PropertyAccessor.SetValues(row, PropertyAccessor.GetValues(result.First()));

                    // has rowversion & it is not RK
                    if (row.HasNonRKRowversion)
                    {
                        row.SetStatus(DbRowStatus.Loaded);
                        Crud.ReloadGo<T>(client, row, true, connectBy, true);
                    }
                    else
                    {
                        // rowversion column is null:       
                        if (map.HasRowversion && row.GetRowversionValue() == null)
                        {
                            row.SetStatus(DbRowStatus.Faulted);
                        }
                        else
                        {
                            row.SetStatus(DbRowStatus.Loaded);
                        }
                    }

                    result.FinalizeCrud();
                    return result;
                }
                // optimistic concurrency violation
                else
                {
                    if (forceMirroring)
                    {
                        var arguments = map.BuildExceptionReport(row.GetOriginalRKValues(), row.GetOriginalRowversionValue());
                        throw new QueryTalkException("Crud.UpdateGo", QueryTalkExceptionType.ConcurrencyViolation,
                            arguments, Text.Method.UpdateGo).SetObjectName(map.Name.Sql);
                    }
                    else
                    {
                        result.FinalizeCrud();
                        return result;
                    }
                }
            }
            catch (QueryTalkException ex)
            {
                Loader.TryThrowInvalidSqlOperationException(ex, name, Text.Method.UpdateGo);
                throw;
            }
            catch (System.Exception ex)
            {
                var ex2 = Crud.ClrException(ex, name, Text.Method.UpdateGo);
                Loader.TryThrowInvalidSqlOperationException(ex2, name);
                throw ex2;
            }
        }

        internal static Async<Result<T>> UpdateGoAsync<T>(Assembly client, DbRow row, bool forceMirroring, ConnectBy connectBy,
            Action<Result<T>> onCompleted)
            where T : DbRow
        {
            var name = Text.NotAvailable;
            NodeMap map;

            Crud.CheckTable(row, Text.Method.UpdateGo);

            try
            {
                var connectable = GetUpdateBody<T>(client, row, forceMirroring, connectBy, Text.Method.UpdateGo, ref name, out map);

                // no columns to modify?
                if (connectable == null)
                {
                    return Async<Result<T>>.CreateDefault<T>();
                }

                return connectable.GoAsync<T>(result =>
                {
                    try
                    {
                        // success
                        if (result.ReturnValue == 1)
                        {
                            PropertyAccessor.SetValues(row, PropertyAccessor.GetValues(result.First()));
                            row.SetStatus(DbRowStatus.Loaded);
                        }
                        // concurrency violation
                        else
                        {
                            if (forceMirroring)
                            {
                                var arguments = map.BuildExceptionReport(row.GetOriginalRKValues(), row.GetOriginalRowversionValue());
                                throw new QueryTalkException("Crud.UpdateGoAsync", QueryTalkExceptionType.ConcurrencyViolation,
                                    arguments, Text.Method.UpdateGoAsync).SetObjectName(map.Name.Sql);
                            }
                        }

                        result.FinalizeCrud();
                        onCompleted?.Invoke(result);
                    }
                    catch (QueryTalkException)
                    {
                        throw;
                    }
                    catch (System.Exception ex)
                    {
                        throw Crud.ClrException(ex, name, Text.Method.InsertGoAsync);
                    }
                });
            }
            catch (QueryTalkException ex)
            {
                Loader.TryThrowInvalidSqlOperationException(ex, name, Text.Method.UpdateGo);
                throw;
            }
            catch (System.Exception ex)
            {
                var ex2 = Crud.ClrException(ex, name, Text.Method.UpdateGo);
                Loader.TryThrowInvalidSqlOperationException(ex2, name);
                throw ex2;
            }
        }

        internal static SubResult UpdateRowGo<T>(Assembly client, DbRow row, bool optimistic, ConnectBy connectBy)
            where T : DbRow
        {
            var result = UpdateGo<T>(client, row, optimistic, connectBy);
            return new SubResult(result.Executed, result.AffectedCount);
        }

    }
}
