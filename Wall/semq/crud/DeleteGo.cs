#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Reflection;

namespace QueryTalk.Wall
{
    static partial class Crud
    {
        private static Procedure DeleteProc(DB3 nodeID, ColumnSelector selector)
        {
            return Designer.GetNewDesigner(Text.Method.DeleteGo, true, true)
                .ParamNodeColumns(nodeID, selector)
                .Param("@Table", Designer.Inliner.Table)
                .Param("@Where", Designer.Inliner.Expression)
                .From("@Table")
                .Where("@Where")
                .Delete()
                .Declare<int>("@r").Set(Designer.RowCount)
                .Return("@r")
                .EndProc();
        }

        private static Connectable GetDeleteBody(Assembly client, DbRow row, bool forceMirroring, ConnectBy connectBy,
            string method, ref string name, out NodeMap map)
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
                    throw new QueryTalkException("Crud.DeleteGo", QueryTalkExceptionType.InvalidMirroring,
                        arguments, Text.Method.DeleteGo).SetObjectName(map.Name.Sql);
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

            args.Add(map.Name);

            if (selector == ColumnSelector.All)
            {
                args.Add(new ParameterArgument(map.BuildOptimisticPredicate(originalValues, 1)));
            }
            else
            {
                args.Add(new ParameterArgument(map.BuildRKPredicate(originalValues, 1)));
            }

            var cpass = DeleteProc(row.NodeID, selector).Pass(args.ToArray());
            cpass.SetRootMap(row.NodeID);   // important!
            return Reader.GetConnectable(client, row, cpass, connectBy);
        }

        internal static Result<T> DeleteGo<T>(Assembly client, DbRow row, bool forceMirroring, ConnectBy connectBy)
            where T : DbRow
        {
            var name = Text.NotAvailable;
            NodeMap map; 

            try
            {
                var connectable = GetDeleteBody(client, row, forceMirroring, connectBy, Text.Method.DeleteGo, ref name, out map);
                var result = connectable.Go();
                if (result.ReturnValue == 1)
                {
                    row.SetStatus(DbRowStatus.Deleted);
                    return new Result<T>(true, 1);
                }
                // concurrency violation:
                else
                {
                    if (forceMirroring)
                    {
                        var arguments = map.BuildExceptionReport(row.GetOriginalRKValues(), row.GetOriginalRowversionValue());
                        throw new QueryTalkException("Crud.DeleteGo", QueryTalkExceptionType.ConcurrencyViolation,
                            arguments, Text.Method.DeleteGo).SetObjectName(map.Name.Sql);
                    }
                    else
                    {
                        return new Result<T>(true, 0);
                    }
                }
            }
            catch (QueryTalkException ex)
            {
                Loader.TryThrowInvalidSqlOperationException(ex, name, Text.Method.DeleteGo);
                throw;
            }
            catch (System.Exception ex)
            {
                var ex2 = Crud.ClrException(ex, name, Text.Method.DeleteGo);
                Loader.TryThrowInvalidSqlOperationException(ex2, name);
                throw ex2;
            } 
        }

        internal static Async<Result<T>> DeleteGoAsync<T>(Assembly client, DbRow row, bool forceMirroring, ConnectBy connectBy,
            Action<Result<T>> onCompleted)
            where T : DbRow
        {
            var name = Text.NotAvailable;
            NodeMap map;

            try
            {
                var connectable = GetDeleteBody(client, row, forceMirroring, connectBy, Text.Method.DeleteGoAsync, ref name, out map);

                // ignore table T load
                connectable.IgnoreLoad = true;

                return connectable.GoAsync<T>(result =>
                {
                    if (result.ReturnValue == 1)
                    {
                        row.SetStatus(DbRowStatus.Deleted);
                    }
                    // concurrency violation:
                    else
                    {
                        if (forceMirroring)
                        {
                            var arguments = map.BuildExceptionReport(row.GetOriginalRKValues(), row.GetOriginalRowversionValue());
                            throw new QueryTalkException("Crud.DeleteGoAsync", QueryTalkExceptionType.ConcurrencyViolation,
                                arguments, Text.Method.DeleteGo).SetObjectName(map.Name.Sql);
                        }
                    }

                    result.FinalizeCrud();
                    onCompleted?.Invoke(result);
                });
            }
            catch (QueryTalkException ex)
            {
                Loader.TryThrowInvalidSqlOperationException(ex, name, Text.Method.DeleteGoAsync);
                throw;
            }
            catch (System.Exception ex)
            {
                var ex2 = Crud.ClrException(ex, name, Text.Method.DeleteGoAsync);
                Loader.TryThrowInvalidSqlOperationException(ex2, name);
                throw ex2;
            }
        }

        internal static SubResult DeleteRowGo<T>(Assembly client, DbRow row, bool forceMirroring, ConnectBy connectBy)
            where T : DbRow
        {
            var result = DeleteGo<T>(client, row, forceMirroring, connectBy);
            return new SubResult(result.Executed, result.AffectedCount);
        }

    }
}
