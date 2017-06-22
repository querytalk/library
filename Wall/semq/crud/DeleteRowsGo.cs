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
        private static Procedure _deleteRowsProc =
            Designer.GetNewDesigner(Text.Method.DeleteRowsGo, true, true)

                .TableParam("#Rows")                           
                .Param("@Table", Designer.Inliner.Table)           
                .Param("@On", Designer.Inliner.Expression)

                .DesignTable("@RID")
                    .AddColumn<int>("RowID")
                .EndDesign()

                .From("@Table").As(1)
                .InnerJoin("#Rows").As(2).On("@On")
                .Delete("1")
                .Output(String.Format("2.{0}", Text.Reserved.QtRowIDColumnName).As("RowID")).Into("@RID")

                .Declare<int>("@r").Set(Designer.RowCount)

                // output
                .From("@RID").Select("RowID")

                .Return("@r")
                .EndProc();

        internal static Result<T> DeleteRowsGo<T>(Assembly client, IEnumerable<T> rows, bool forceMirroring, ConnectBy connectBy)
            where T : DbRow
        {
            SubResult subResult;

            if (forceMirroring)
            {
                subResult = GoMirroring<T>(client, rows, Crud.DeleteRowGo<T>, connectBy);
            }
            else
            {
                subResult = GoNonMirroring<T>(client, rows, ProcessPackage_DeleteRows, connectBy, null);
            }

            return new Result<T>(subResult.Executed, subResult.AffectedCount);
        }

        private static SubResult ProcessPackage_DeleteRows<T>(
            Assembly client,
            IEnumerable<T> package, 
            NodeMap map, 
            ConnectBy connectBy, 
            Nullable<bool> identityInsert)
            where T : DbRow
        {
            List<ParameterArgument> args = new List<ParameterArgument>();
            args.Add(ViewConverter.ConvertDbRowData(package, false, true));     // #Rows
            args.Add(map.Name);                                                 // @Table
            args.Add(map.BuildSelfRelationWithOriginalValues(1, 2).E());        // @On

            PassChainer cpass = _deleteRowsProc.Pass(args.ToArray());
            cpass.SetRootMap(map.ID);   // important!
            var connectable = Reader.GetConnectable(client, cpass, connectBy);
            var result = connectable.Go();

            var i = 0;
            var count = result.RowCount;
            var deleted = result.ToList();
            foreach (var row in package)
            {
                if (i >= count) { break; }
                if (((IRow)row).RowID == deleted[i].RowID)
                {
                    row.SetStatus(DbRowStatus.Deleted);
                    ++i;
                }
            }

            return new SubResult(true, result.ReturnValue);
        }

    }
}
