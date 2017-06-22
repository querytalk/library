#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryTalk.Wall
{
    /// <summary>
    /// The mapping data of a database object. (This class is intended to be used by the mapper application.)
    /// </summary>
    public sealed class NodeMap : Map
    {
        /// <summary>
        /// A name of a database object.
        /// </summary>
        public Identifier Name { get; private set; }

        /// <summary>
        /// Columns of a database object.
        /// </summary>
        public HashSet<ColumnMap> Columns { get; private set; }

        /// <summary>
        /// Parameters of a database object.
        /// </summary>
        public HashSet<ParamMap> Params { get; private set; }

        /// <summary>
        /// A flag to indicate whether a database object has a genuine row key (PK or UK).
        /// </summary>
        internal bool HasGenuineRK { get; private set; }

        /// <summary>
        /// Initializes a new instance of the NodeMap class.
        /// </summary>
        /// <param name="id">A DB3 identifier of a database object.</param>
        /// <param name="name">A name of a database object.</param>
        /// <param name="hasGenuineRK">A flag to indicate whether a database object has a genuine row key (PK or UK).</param>
        public NodeMap(DB3 id, Identifier name, bool hasGenuineRK = true)
            : base(id)
        {
            Name = name;
            Columns = new HashSet<ColumnMap>(new MapEqualityComparer());
            Params = new HashSet<ParamMap>(new MapEqualityComparer());
            HasGenuineRK = hasGenuineRK;
        }

        internal ColumnMap[] SortedColumns
        {
            get
            {
                return Columns.OrderBy(c => c.ID.ColumnZ).ToArray();
            }
        }

        internal ColumnMap[] SortedColumnsByDatabaseOrder
        {
            get
            {
                return Columns.OrderBy(c => c.ColumnOrdinal).ToArray();
            }
        }

        internal ColumnMap[] GetInsertableColumns(bool identityInsert)
        {
            if (identityInsert)
            {
                return Columns
                    .Where(c => c.ColumnType == ColumnType.Regular || c.ColumnType == ColumnType.Identity)
                    .OrderBy(c => c.ID.ColumnZ).ToArray();
            }
            else
            {
                return Columns
                    .Where(c => c.ColumnType == ColumnType.Regular)
                    .OrderBy(c => c.ID.ColumnZ).ToArray();
            }
        }

        internal ColumnMap[] GetInsertableColumns(DbRow row, bool identityInsert)
        {
            if (identityInsert)
            {
                return Columns
                    .Where(c => c.ColumnType == ColumnType.Regular || c.ColumnType == ColumnType.Identity)
                    .Where(c => row.SetColumns.Contains(c.ID.ColumnZ) || !c.HasDefault) 
                    .OrderBy(c => c.ID.ColumnZ).ToArray();
            }
            else
            {
                return Columns
                    .Where(c => c.ColumnType == ColumnType.Regular)
                    .Where(c => row.SetColumns.Contains(c.ID.ColumnZ) || !c.HasDefault)  
                    .OrderBy(c => c.ID.ColumnZ).ToArray();
            }
        }

        internal ColumnMap[] SortedRKColumns
        {
            get
            {
                return Columns.Where(c => c.IsRK).OrderBy(c => c.ID.ColumnZ).ToArray();
            }
        }

        internal ColumnMap[] SortedComputedColumns
        {
            get
            {
                return Columns.Where(c => c.ColumnType != ColumnType.Regular)
                    .OrderBy(c => c.ID.ColumnZ).ToArray();
            }
        }

        internal bool HasIdentity
        {
            get
            {
                return Columns.Where(a => a.ColumnType == ColumnType.Identity && a.IsRK).Any();
            }
        }

        internal bool HasRowversion
        {
            get
            {
                return Columns.Where(a => a.ColumnType == ColumnType.Rowversion).Any();
            }
        }

        private ColumnMap _rowversionColumn;
        internal ColumnMap RowversionColumn
        {
            get
            {
                if (HasRowversion && _rowversionColumn == null)
                {
                    _rowversionColumn = Columns.Where(a => a.DataType.DT.IsRowversion())
                        .FirstOrDefault();
                }
                return _rowversionColumn;
            }
        }

        internal static NodeMap Default
        {
            get
            {
                return new NodeMap(DB3.Default, Designer.Identifier("DefaultNode"));
            }
        }

        internal ColumnMap[] GetKeys()
        {
            return Columns.Where(a => a.IsKey).ToArray();
        }


        internal ColumnMap TryGetIdentityPK()
        {
            return Columns.Where(a => a.IsRK && a.ColumnType == ColumnType.Identity).FirstOrDefault();
        }

        #region Columns

        internal Column[] GetColumns(int alias, Nullable<int> aliasOfRowID = null)
        {
            List<Column> columns = new List<Column>();
            foreach (var map in SortedColumns)
            {
                columns.Add(Designer.Identifier(alias.ToString(), map.Name.Part1));
            }

            // add RowID
            if (aliasOfRowID != null)
            {
                columns.Add(Designer.Identifier(aliasOfRowID.ToString(), Text.Reserved.QtRowIDColumnName));
            }

            return columns.ToArray();
        }

        internal Column[] GetColumnsByDatabaseOrder(string alias)
        {
            List<Column> columns = new List<Column>();
            foreach (var map in SortedColumnsByDatabaseOrder)
            {
                if (alias != null)
                {
                    columns.Add(Designer.Identifier(alias.ToString(), map.Name.Part1));
                }
                else
                {
                    columns.Add(Designer.Identifier(map.Name.Part1));
                }
            }

            return columns.ToArray();
        }

        internal Column[] GetColumnsByDatabaseOrder(string alias, DbNode node)
        {
            List<Column> columns = new List<Column>();
            foreach (var map in SortedColumnsByDatabaseOrder)
            {
                if (alias != null)
                {
                    columns.Add(new DbColumn(node, map.ID, true).Of(alias));
                }
                else
                {
                    columns.Add(new DbColumn(node, map.ID, true));
                }
            }

            return columns.ToArray();
        }

        internal Column[] GetColumnsForUpdate(Nullable<int> alias)
        {
            List<Column> columns = new List<Column>();
            foreach (var map in Columns.Where(c => c.ColumnType == ColumnType.Regular)
                .OrderBy(c => c.ID.ColumnZ))
            {
                if (alias != null)
                {
                    columns.Add(Designer.Identifier(alias.ToString(), map.Name.Part1));
                }
                else
                {
                    columns.Add(map.Name);
                }
            }
            return columns.ToArray();
        }

        internal Column[] GetRKColumns(Nullable<int> alias)
        {
            List<Column> columns = new List<Column>();
            foreach (var map in SortedRKColumns)
            {
                if (alias != null)
                {
                    columns.Add(Designer.Identifier(alias.ToString(), map.Name.Part1));
                }
                else
                {
                    columns.Add(map.Name);
                }
            }
            return columns.ToArray();
        }

        internal Column[] GetKeyColumns(int alias)
        {
            List<Column> columns = new List<Column>();
            foreach (var map in Columns.Where(c => c.IsKey))
            {
                columns.Add(Designer.Identifier(alias.ToString(), map.Name.Part1));
            }
            return columns.ToArray();
        }

        #endregion

        internal GroupingArgument[] GetGroupBy(int alias)
        {
            List<GroupingArgument> columns = new List<GroupingArgument>();
            foreach (var columnMap in Columns.Where(c => c.IsRK))
            {
                columns.Add(Designer.Identifier(alias.ToString(), columnMap.Name.Part1));
            }
            return columns.ToArray();
        }

        internal string BuildSelfRelation(int indexA, int indexB)
        {
            List<string> columns = new List<string>();
            foreach (var columnMap in Columns.Where(c => c.IsRK))
            {
                // regular non-nullable RK
                if (!columnMap.IsNullable)
                {
                    columns.Add(String.Format("([{0}].{1} = [{2}].{3})",
                        indexA, columnMap.Name.Sql,
                        indexB, columnMap.Name.Sql));
                }
                // nullable RK (not very common) 
                else
                {
                    columns.Add(String.Format("([{0}].{1} = [{2}].{3} OR ([{0}].{1} IS NULL AND [{2}].{3} IS NULL))",
                        indexA, columnMap.Name.Sql,
                        indexB, columnMap.Name.Sql));
                }
            }

            return String.Join(Text._And_, columns);
        }

        internal string BuildSelfRelationWithOriginalValues(int indexA, int indexB)
        {
            List<string> columns = new List<string>();
            foreach (var columnMap in Columns.Where(c => c.IsRK))
            {
                // regular non-nullable RK
                if (!columnMap.IsNullable)
                {
                    columns.Add(String.Format("([{0}].{1} = [{2}].{3})",
                        indexA, columnMap.Name.Sql,
                        indexB, Filter.Delimit(Common.ProvideOriginalColumnCRUD(columnMap.Name.Part1))));
                }
                // nullable RK (not very common) 
                else
                {
                    columns.Add(String.Format("([{0}].{1} = [{2}].{3} OR ([{0}].{1} IS NULL AND [{2}].{3} IS NULL))",
                        indexA, columnMap.Name.Sql,
                        indexB, Filter.Delimit(Common.ProvideOriginalColumnCRUD(columnMap.Name.Part1))));
                }
            }

            return String.Join(Text._And_, columns);
        }

        internal Expression BuildOptimisticPredicate(object[] values, int index)
        {
            StringBuilder builder = null;
            int i = 0;

            if (HasRowversion)
            {
                builder = Text.GenerateSql(30)
                    .Append(String.Format("({0} = @o1)", _rowversionColumn.Name.Sql));
            }
            else
            {
                foreach (var column in SortedColumns)
                {
                    string expressionStr;
                    if (values[i] == null)
                    {
                        expressionStr = String.Format("([{0}].{1} IS NULL)",
                            index, column.Name);
                    }
                    else
                    {
                        expressionStr = String.Format("([{0}].{1} = @o{2})",
                            index, column.Name, i+1);
                    }

                    if (builder == null)
                    {
                        builder = Text.GenerateSql(200);
                    }
                    else
                    {
                        builder.Append(Text._And_);
                    }

                    builder.Append(expressionStr);
                    ++i;
                }
            }

            return builder.ToString().E();
        }

        internal Expression BuildRKPredicate(object[] rkValues, int index, bool parameterization = true)
        {
            StringBuilder builder = null;
            int i = 0;

            foreach (var column in SortedRKColumns)
            {
                string expressionStr;
                if (rkValues[i] == null)
                {
                    expressionStr = String.Format("([{0}].{1} IS NULL)",
                        index, column.Name);
                }
                else
                {
                    if (parameterization)
                    {
                        expressionStr = String.Format("([{0}].{1} = @o{2})",
                            index, column.Name, i + 1);
                    }
                    else
                    {
                        var value = rkValues[i];
                        expressionStr = String.Format("([{0}].{1} = {2})",
                            index, column.Name, Mapping.Build(value, column.DataType));
                    }
                }

                if (builder == null)
                {
                    builder = Text.GenerateSql(200);
                }
                else
                {
                    builder.Append(Text._And_);
                }

                builder.Append(expressionStr);
                ++i;
            }

            return builder.ToString().E();
        }

        internal Expression BuildRKExpression(object[] rkValues, int index)
        {
            Expression builder = null;
            Expression prev = null; 
            Expression current = null;
            ScalarArgument arg = null; 
            int i = 0;

            foreach (var column in SortedRKColumns)
            {
                arg = new Identifier(index.ToString(), column.Name.Part1);

                // a IS NULL
                if (rkValues[i] == null)
                {
                    current = arg.IsNull();
                }
                // a = b
                else
                {
                    current = arg.EqualTo(new Value(rkValues[i], column.DataType));
                }

                // (E)
                if (builder == null)
                {
                    builder = current;
                }
                // (E1) AND (E2)
                else
                {
                    builder = prev.And(current);
                }

                prev = builder;
                ++i;
            }

            return builder;
        }

        internal string BuildExceptionReport(object[] rkValues, object rowversion)
        {
            if (HasRowversion && rowversion == null)
            {
                return String.Format("{0} = null", RowversionColumn.Name);
            }

            var builder = new StringBuilder();
            var i = 0;
            foreach (var column in SortedRKColumns)
            {
                var value = Mapping.Build(rkValues[i++], column.DataType);
                if (i == 1)
                {
                    builder.Append(String.Format("{0} = {1}", column.Name.Sql, value));
                }
                else
                {
                    builder.NewLine(String.Format("   {0} = {1}", column.Name.Sql, value));
                }
            }

            return builder.ToString();
        }

    }
}
