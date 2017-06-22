#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QueryTalk.Wall
{
    static partial class Crud
    {
        private static Procedure ReloadProc(DB3 nodeID, ColumnSelector selector)
        {
            return Designer.GetNewDesigner(Text.Method.ReloadGo, true, true)
                .ParamNodeColumns(nodeID, selector)
                .Param("@Table", Designer.Inliner.Table)
                .Param("@Where", Designer.Inliner.Expression)
                .From("@Table")
                .Where("@Where")
                .Select()
                .EndProc();
        }

        // isSilent:
        //   if true, then this method is used by other CRUD method (.UpdateGo/.InsertGo) which requires special treatment.
        internal static Result<T> ReloadGo<T>(Assembly client, DbRow row, bool forceMirroring, ConnectBy connectBy, 
            bool isSilent = false)
            where T : DbRow
        {
            var name = Text.NotAvailable;

            try
            {
                ColumnSelector selector = forceMirroring ? ColumnSelector.All : ColumnSelector.RK;
                if (isSilent)
                {
                    selector = ColumnSelector.RK;
                }

                if (row == null)
                {
                    throw new QueryTalkException("Crud.GoReload", QueryTalkExceptionType.ArgumentNull, "row = null", Text.Method.ReloadGo);
                }

                Crud.CheckTable(row, Text.Method.ReloadGo);

                List<ParameterArgument> args = new List<ParameterArgument>();

                var map = DbMapping.TryGetNodeMap(row);
                name = map.Name.Sql;

                if (forceMirroring)
                {
                    if (!row.GetStatus().IsUpdatable())
                    {
                        var arguments = map.BuildExceptionReport(row.GetOriginalRKValues(), row.GetOriginalRowversionValue());
                        throw new QueryTalkException("Crud.ReloadGo", QueryTalkExceptionType.InvalidMirroring,
                            arguments, Text.Method.ReloadGo).SetObjectName(map.Name.Sql);
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

                var cpass = ReloadProc(row.NodeID, selector).Pass(args.ToArray());
                cpass.SetRootMap(row.NodeID);   // important!
                Connectable connectable = Reader.GetConnectable(client, row, cpass, connectBy);
                var result = connectable.Go<T>();

                // success
                if (result.RowCount > 0)
                {
                    PropertyAccessor.SetValues(row, result.First().GetOriginalValues());
                    row.SetStatus(DbRowStatus.Loaded);
                    return new Result<T>(true, 0);
                }
                // not found
                else
                {
                    var arguments = map.BuildExceptionReport(row.GetOriginalRKValues(), row.GetOriginalRowversionValue());

                    if (!isSilent)
                    {
                        if (forceMirroring)
                        {
                            throw new QueryTalkException("Crud.ReloadGo", QueryTalkExceptionType.ConcurrencyViolation,
                                arguments, Text.Method.ReloadGo).SetObjectName(map.Name.Sql);
                        }
                        else
                        {
                            throw new QueryTalkException("Crud.ReloadGo", QueryTalkExceptionType.ReloadFailed,
                                arguments, Text.Method.ReloadGo).SetObjectName(map.Name.Sql);
                        }
                    }
                    else
                    {
                        row.SetStatus(DbRowStatus.Faulted);
                        return new Result<T>(true, 0);
                    }
                }
            }
            catch (QueryTalkException ex)
            {
                Loader.TryThrowInvalidSqlOperationException(ex, name, Text.Method.ReloadGo);
                throw;
            }
            catch (System.Exception ex)
            {
                var ex2 = Crud.ClrException(ex, name, Text.Method.ReloadGo);
                Loader.TryThrowInvalidSqlOperationException(ex2, name);
                throw ex2;
            } 
        }

    }
}
