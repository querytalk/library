#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;
using System.Linq;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Represents the mapping data of a database relation.
    /// </summary>
    public sealed class Relation : Map
    {
        // a foreign key table columns (FK)
        internal DB3[] FKColumns { get; private set; }

        // a reference table columns (PK or UK)
        internal DB3[] RefColumns { get; private set; }

        internal DB3 RefTable
        {
            get
            {
                return RefColumns[0].TableID;
            }
        }

        internal DB3 FKTable
        {
            get
            {
                return FKColumns[0].TableID;
            }
        }

        internal DB3 ForeignKey
        {
            get
            {
                return base.ID;
            }
        }

        internal bool IsSelfRelation
        {
            get
            {
                return FKColumns[0].TableID.Equals(RefColumns[0].TableID);
            }
        }

        // returns true if the table is on reference side in the relationship
        internal bool IsRefTable(DB3 table)
        {
            return table.Equals(RefTable);
        }

        // returns true if the table is on foreign key side in the relationship
        internal bool IsFKTable(DB3 table)
        {
            return table.Equals(FKTable);
        }

        internal bool IsInnerJoin(DB3 firstTable)
        {
            if (IsRefTable(firstTable))
            {
                return true;
            }

            return !DbMapping.GetColumnMap(FKColumns[0]).IsNullable;
        }


        /// <summary>
        /// Initializes a new instance of the Relation class.
        /// </summary>
        /// <param name="foreignKey">The specified column of the relation which represents a unique identifier of the relation between the two tables.</param>
        /// <param name="foreignKeyColumns">The columns which are part of the key in table A.</param>
        /// <param name="referenceKeyColumns">The columns which are part of the key in table B.</param>
        public Relation(DB3 foreignKey, DB3[] foreignKeyColumns, DB3[] referenceKeyColumns)
            : base(foreignKey)
        {
            FKColumns = foreignKeyColumns;
            RefColumns = referenceKeyColumns;
        }
        internal string BuildRelation(DB3 table1, string alias1, string alias2)
        {
            string aliasA, aliasB;

            if (table1.TableID.Equals(ID.TableID) 
                && !IsSelfRelation) 
            {
                aliasA = alias1;
                aliasB = alias2;
            }
            else
            {
                aliasA = alias2;
                aliasB = alias1;
            }

            var sql = Text.GenerateSql(70);
            for (int i = 0; i < FKColumns.Count(); ++i)
            {
                if (i > 0)
                {
                    sql.Append(Text._And_);
                }

                var columnA = DbMapping.GetColumnMap(FKColumns[i]);
                var columnB = DbMapping.GetColumnMap(RefColumns[i]);

                sql
                    .AppendFormat("[{0}].{1} = [{2}].{3}",
                        aliasA, columnA.Name.Sql,
                        aliasB, columnB.Name.Sql);
            }

            return sql.ToString();
        }

        internal string BuildRelation(DB3 table1, int alias1, int alias2)
        {
            return BuildRelation(table1, alias1.ToString(), alias2.ToString());
        }

        internal Column[] GetFK(int alias)
        {
            List<Column> columns = new List<Column>();
            foreach (var columnID in FKColumns)
            {
                columns.Add(Designer.Identifier(alias.ToString(), DbMapping.GetColumnMap(columnID).Name.Part1));
            }
            return columns.ToArray();
        }

    }
}
