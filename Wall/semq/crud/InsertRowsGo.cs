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

        #region Procedures

        private static Procedure _insertRowsIntoIdentityProc =
             Designer.GetNewDesigner(Text.Method.InsertRowsGo, true, true, Designer.IsolationLevel.Serializable)

                // params
                .TableParam("#Rows")                                // data collection
                .Param<long>("@Delta")                              // @Delta = package[0].__QtRowID
                .Param("@Identity", Designer.Inliner.Column)        // identity PK
                .Param("@MaxOfIdentity", Designer.Inliner.Column)   // ISNULL(MAX(identity), 0)
                .Param("@NewIdentity", Designer.Inliner.Column)     // __QTRowID - @Delta + @LastID + 1
                .Param("@Table", Designer.Inliner.Table)            // target table
                .Param("@Columns", Designer.Inliner.Column)         // columns 
                .Param("@AllColumns", Designer.Inliner.Column)      // all columns to return in output
                .Param("@On", Designer.Inliner.Expression)          // join condition

                // get LastID
                .Declare<long>("@LastID")
                .From("@Table").Select("@MaxOfIdentity").IntoVars("@LastID")

                // update #Rows
                .From("#Rows")
                .Select("@NewIdentity").IntoColumns("@Identity")
                .Update()

                // insert
                .SetIdentityInsert("@Table", true)
                .From("#Rows")
                .Select("@Columns")
                .IntoColumns("@Columns")
                .Insert("@Table")
                .SetIdentityInsert("@Table", false)

                // output
                .From("@Table").As(1)
                .InnerJoin("#Rows").As(2).On("@On")
                .OrderBy(Designer.Identifier("2", Text.Reserved.QtRowIDColumnName))
                .Select("@AllColumns")

                .EndProc();

        private static Procedure _identityInsertRowsProc =
             Designer.GetNewDesigner(Text.Method.InsertRowsGo, true, true)

                // params
                .TableParam("#Rows")                                // data collection
                .Param("@Table", Designer.Inliner.Table)            // target table
                .Param("@Columns", Designer.Inliner.Column)         // columns to insert
                .Param("@AllColumns", Designer.Inliner.Column)      // all columns to return in output
                .Param("@On", Designer.Inliner.Expression)          // join condition

                // insert 
                .SetIdentityInsert("@Table", true)
                .From("#Rows")
                .Select("@Columns")
                .IntoColumns("@Columns")
                .Insert("@Table")
                .SetIdentityInsert("@Table", false)

                // output
                .From("@Table").As(1)
                .InnerJoin("#Rows").As(2).On("@On")
                .OrderBy(Designer.Identifier("2", Text.Reserved.QtRowIDColumnName))
                .Select("@AllColumns")

                .EndProc();

        private static Procedure _insertRowsProc =
            Designer.GetNewDesigner(Text.Method.InsertRowsGo, true, true)

                // params
                .TableParam("#Rows")                                // data collection
                .Param("@Table", Designer.Inliner.Table)            // target table
                .Param("@Columns", Designer.Inliner.Column)         // columns to insert
                .Param("@AllColumns", Designer.Inliner.Column)      // all columns to return in output
                .Param("@On", Designer.Inliner.Expression)          // join condition

                // insert 
                .From("#Rows")
                .Select("@Columns")
                .IntoColumns("@Columns")
                .Insert("@Table")

                // output
                .From("@Table").As(1)
                .InnerJoin("#Rows").As(2).On("@On")
                .OrderBy(Designer.Identifier("2", Text.Reserved.QtRowIDColumnName))
                .Select("@AllColumns")

                .EndProc();

        #endregion

        /// <summary>
        /// Inserts the specified rows in the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="client">A client assembly.</param>
        /// <param name="rows">The rows to insert.</param>
        /// <param name="identityInsert">If true, then the explicit value can be inserted into the identity column of a table.</param>
        /// <param name="connectBy">A ConnectBy object.</param>
        public static Result<T> InsertRowsGo<T>(Assembly client, IEnumerable<T> rows, bool identityInsert, ConnectBy connectBy)
            where T : DbRow
        {
            var subResult = GoNonMirroring<T>(client, rows, ProcessPackage_InsertRows, connectBy, identityInsert);
            return new Result<T>(subResult.Executed, subResult.AffectedCount);
        }

        private static SubResult ProcessPackage_InsertRows<T>(Assembly client, 
            IEnumerable<T> package, NodeMap map, ConnectBy connectBy, Nullable<bool> identityInsert)
            where T : DbRow
        {
            if (package.Count() == 0)
            {
                return new SubResult();
            }

            PassChainer cpass;
            List<ParameterArgument> args = new List<ParameterArgument>();
            var view = ViewConverter.ConvertDbRowData(package, false, true);
            var columns = map.GetInsertableColumns(true).Select(a => new Column(a.Name)).ToArray();

            if (map.HasIdentity)
            {
                var identityPK = map.TryGetIdentityPK();
                if (identityInsert == false)
                {
                    var delta = ((IRow)package.First()).RowID;
                    args.Add(view);
                    args.Add(delta);                                                // @Delta
                    args.Add(identityPK.Name.Part1);                                // @Identity
                    args.Add(new Column(Designer.IsNull(                            // ISNULL(MAX(@Identity), 0)
                        Designer.Max(Designer.Identifier("1", identityPK.Name.Part1)), 0)));
                    args.Add(new Column(String.Format("[1].[{0}] - @Delta + @LastID + 1", Text.Reserved.QtRowIDColumnName).E()));
                    args.Add(map.Name);                                             // @Table
                    args.Add(columns);                                              // @Columns
                    args.Add(map.GetColumns(1));                                    // @AllColumns
                    args.Add(map.BuildSelfRelation(1, 2).E());                      // @On
                    cpass = _insertRowsIntoIdentityProc.Pass(args.ToArray());
                }
                else
                {
                    args.Add(ViewConverter.ConvertDbRowData(package, false, true)); // #Rows
                    args.Add(map.Name);                                             // @Table
                    args.Add(columns);                                              // @Columns
                    args.Add(map.GetColumns(1));                                    // @AllColumns
                    args.Add(map.BuildSelfRelation(1, 2).E());                      // @On
                    cpass = _identityInsertRowsProc.Pass(args.ToArray());
                }
            }
            // non-identity table
            else
            {
                // pass arguments
                args.Add(ViewConverter.ConvertDbRowData(package, false, true));     // #Rows
                args.Add(map.Name);                                                 // @Table
                args.Add(columns);                                                  // @Columns
                args.Add(map.GetColumns(1));                                        // @AllColumns
                args.Add(map.BuildSelfRelation(1, 2).E());                          // @On
                cpass = _insertRowsProc.Pass(args.ToArray());
            }

            // execute
            cpass.SetRootMap(map.ID);  
            var connectable = Reader.GetConnectable(client, cpass, connectBy);
            var result = connectable.Go<T>();
            var data = result.ToList();

            var computedIndexes = map.SortedComputedColumns.Select(a => a.ID.ColumnZ).ToArray();
            var j = 0;
            foreach (var row in package)
            {
                PropertyAccessor.SetValues(row, PropertyAccessor.GetValues(data[j++]), computedIndexes);
                row.SetStatus(DbRowStatus.Loaded);
            }

            // all rows inserted successfully or none
            return new SubResult(true, result.RowCount);
        }

    }
}
