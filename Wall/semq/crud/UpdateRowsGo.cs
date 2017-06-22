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
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private static Procedure _updateRowsProc =
            Designer.GetNewDesigner(Text.Method.UpdateRowsGo, true, true)

                .TableParam("#Rows")                                // data collection
                .Param("@Table", Designer.Inliner.Table)            // target table
                .Param("@SourceColumns", Designer.Inliner.Column)   // source columns to insert
                .Param("@TargetColumns", Designer.Inliner.Column)   // target columns to insert
                .Param("@OnUpdate", Designer.Inliner.Expression)    // join condition (in update)
                .Param("@OnSelect", Designer.Inliner.Expression)    // join condition (in select)
                .Param("@AllColumns", Designer.Inliner.Column)      // all columns to return in output (with RowID)

                .DesignTable("@RID")
                    .AddColumn<int>("RowID")
                .EndDesign()

                // update
                .From("@Table").As(1)
                .InnerJoin("#Rows").As(2).On("@OnUpdate")
                .Select("@SourceColumns")
                .IntoColumns("@TargetColumns")
                .Update("1")
                .Output(String.Format("2.{0}", Text.Reserved.QtRowIDColumnName).As("RowID")).Into("@RID")

                .Declare<int>("@r").Set(Designer.RowCount)

                // output #1
                .From("@RID").Select("RowID")

                // output #2
                .From("@Table").As(1)
                .InnerJoin("#Rows").As(2).On("@OnSelect")
                .OrderBy(Designer.Identifier("2", Text.Reserved.QtRowIDColumnName))
                .Select("@AllColumns")

                // return affected rows count
                .Return("@r")

                .EndProc();

        internal static Result<T> UpdateRowsGo<T>(Assembly client, IEnumerable<T> rows, bool forceMirroring, ConnectBy connectBy)
            where T : DbRow
        {
            SubResult subResult;

            if (forceMirroring)
            {
                subResult = GoMirroring<T>(client, rows, Crud.UpdateRowGo<T>, connectBy);
            }
            else
            {
                subResult = GoNonMirroring<T>(client, rows, ProcessPackage_UpdateRows, connectBy, null);
            }

            return new Result<T>(subResult.Executed, subResult.AffectedCount);
        }

        private static SubResult ProcessPackage_UpdateRows<T>(
            Assembly client,
            IEnumerable<T> package,
            NodeMap map,
            ConnectBy connectBy,
            Nullable<bool> identityInsert)
            where T : DbRow
        {
            List<ParameterArgument> args = new List<ParameterArgument>();
            args.Add(ViewConverter.ConvertDbRowData(package, false, true)); // #Rows
            args.Add(map.Name);                                             // @Table
            args.Add(map.GetColumnsForUpdate(2));                           // @SourceColumns
            args.Add(map.GetColumnsForUpdate(null));                        // @TargetColumns
            args.Add(map.BuildSelfRelationWithOriginalValues(1, 2).E());    // @OnUpdate
            args.Add(map.BuildSelfRelation(1, 2).E());                      // @OnSelect
            args.Add(map.GetColumns(1, 2));                                 // @AllColumns (with RowID)

            PassChainer cpass = _updateRowsProc.Pass(args.ToArray());
            cpass.SetRootMap(map.ID);   // important!
            var connectable = Reader.GetConnectable(client, cpass, connectBy);
            var result = connectable.Go();
            var data = result.Table2.ToList();

            var computedIndexes = map.SortedComputedColumns.Select(a => a.ID.ColumnZ).ToArray();
            var hasComputedColumns = computedIndexes.Length > 0;

            var i = 0;
            var count = result.RowCount;
            var updated = result.Table1.ToList();

            foreach (var row in package)
            {
                if (i >= count) { break; }

                if (((IRow)row).RowID == updated[i].RowID)
                {
                    if (hasComputedColumns)
                    {
                        PropertyAccessor.SetValues(row, PropertyAccessor.GetValues(data[i], Text.Reserved.QtRowIDColumnName),
                            computedIndexes);
                    }

                    row.SetStatus(DbRowStatus.Loaded);

                    ++i;
                }
            }

            return new SubResult(true, result.ReturnValue);
        }

    }
}
