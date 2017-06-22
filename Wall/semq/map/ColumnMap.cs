#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    /// <summary>
    /// The mapping data of a column. (This class is intended to be used by the mapper application.)
    /// </summary>
    public sealed class ColumnMap : Map
    {
        /// <summary>
        /// A name of a column.
        /// </summary>
        public Identifier Name { get; internal set; } 

        /// <summary>
        /// A data type.
        /// </summary>
        public DataType DataType { get; private set; }

        /// <summary>
        /// A flag to indicate if a column is part of a row key.
        /// </summary>
        public bool IsRK { get; private set; }

        /// <summary>
        /// A flag to indicate if a column is part of a unique key.
        /// </summary>
        public bool IsUK { get; private set; }

        /// <summary>
        /// A flag to indicate if a column is part of a foreign key.
        /// </summary>
        public bool IsFK { get; private set; }

        /// <summary>
        /// A flag to indicate if a column is nullable.
        /// </summary>
        public bool IsNullable { get; private set; }

        /// <summary>
        /// A column type.
        /// </summary>
        public ColumnType ColumnType { get; private set; }

        /// <summary>
        /// A flag to indicate if a column has a default constraint.
        /// </summary>
        public bool HasDefault { get; private set; }

        /// <summary>
        /// A flag to indicate if a column represents all columns.
        /// </summary>
        internal bool IsAllColumns { get; private set; }

        /// <summary>
        /// A flag to indicate if a column is part of a key.
        /// </summary>
        internal bool IsKey
        {
            get
            {
                return IsRK || IsUK || IsFK;
            }
        }

        internal string FullName
        {
            get
            {
                var nodeMap = DbMapping.GetNodeMap(ID.TableID);
                return String.Format("{0}.{1}", nodeMap.Name.Sql, Name.Sql);
            }
        }

        internal string ClrName
        {
            get
            {
                return Naming.GetClrName(Name.Part1);
            }
        }

        internal int ColumnOrdinal { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ColumnMap class.
        /// </summary>
        /// <param name="id">A DB3 identifier of a column.</param>
        /// <param name="columnOrdinal">A column ordinal.</param>
        /// <param name="name">A name of a column.</param>
        /// <param name="dataType">A data type.</param>
        /// <param name="isNullable">A flag to indicate if a column is nullable.</param>
        /// <param name="isRK">A flag to indicate if a column is part of a row key.</param>
        /// <param name="isUK">A flag to indicate if a column is part of a unique key.</param>
        /// <param name="isFK">A flag to indicate if a column is part of a foreign key.</param>
        /// <param name="columnType">A column type.</param>
        /// <param name="hasDefault">A flag to indicate if a column has a default constraint.</param>
        public ColumnMap(
            DB3 id, 
            int columnOrdinal,
            Identifier name, 
            DataType dataType, 
            bool isNullable,
            bool isRK = false, 
            bool isUK = false,
            bool isFK = false, 
            ColumnType columnType = Wall.ColumnType.Regular,
            bool hasDefault = false)
            : base(id)
        {
            ColumnOrdinal = columnOrdinal;
            Name = name;
            DataType = dataType;
            IsNullable = isNullable;
            IsRK = isRK;
            IsUK = isUK;
            IsFK = isFK;
            ColumnType = columnType;
            HasDefault = hasDefault;
        }

        /// <summary>
        /// Initializes a new instance of the ColumnMap class.
        /// </summary>
        /// <param name="id">A DB3 identifier of a column.</param>
        public ColumnMap(DB3 id)
            : base(id)
        {
            IsAllColumns = true;
            Name = Identifier.GetAsterisk();
        }

    }
}
