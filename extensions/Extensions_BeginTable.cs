#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {
        /// <summary>
        /// Begins the declaration block of a table variable.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">The name of a table variable or temp table.</param>
        public static BeginTableChainer DesignTable(this IAny prev, string table)
        {
            return new BeginTableChainer((Chainer)prev, table, null, false, false);
        }

        /// <summary>
        /// Adds a new column in the table declaration block.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="column">A column argument.</param>
        /// <param name="dataTypeDef">A column data type definition.</param>
        public static BeginTableColumnChainer AddColumn(this IBeginTableColumn prev, NonSelectColumnArgument column,
            DataType dataTypeDef)
        {
            return new BeginTableColumnChainer((Chainer)prev, column, dataTypeDef);
        }

        /// <summary>
        /// Adds a new column in the table declaration block.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="column">A column argument.</param>
        /// <typeparam name="T">A type of a column.</typeparam>
        public static BeginTableColumnChainer AddColumn<T>(this IBeginTableColumn prev, NonSelectColumnArgument column)
        {
            return new BeginTableColumnChainer((Chainer)prev, column, typeof(T));
        }

        /// <summary>
        /// Defines an identity column in the table declaration block.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="seed">Is the value that is used for the very first row loaded into the table.</param>
        /// <param name="increment">Is the incremental value that is added to the identity value of the previous row that was loaded.</param>
        public static BeginTableColumnIdentityChainer Identity(this IBeginTableColumn prev, long seed = 1, long increment = 1)
        {
            return new BeginTableColumnIdentityChainer((Chainer)prev, seed, increment);
        }

        /// <summary>
        /// Defines a NULL property of the column in the table declaration block.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static BeginTableColumnNullableChainer Null(this IBeginTableColumn prev)
        {
            return new BeginTableColumnNullableChainer((Chainer)prev, true);
        }

        /// <summary>
        /// Defines a NOT NULL property of the column in the table declaration block.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static BeginTableColumnNullableChainer NotNull(this IBeginTableColumn prev)
        {
            return new BeginTableColumnNullableChainer((Chainer)prev, false);
        }

        /// <summary>
        /// Defines a CHECK constraint on the column in the table declaration block.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="expression">An expression of the CHECK constraint.</param>
        public static BeginTableColumnCheckChainer Check(this IBeginTableColumn prev, Expression expression)
        {
            return new BeginTableColumnCheckChainer((Chainer)prev, expression);
        }

        /// <summary>
        /// Defines a DEFAULT value for a column in the table declaration block.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="value">A DEFAULT value.</param>
        public static BeginTableColumnDefaultChainer Default(this IBeginTableColumn prev, Value value)
        {
            return new BeginTableColumnDefaultChainer((Chainer)prev, value);
        }

        /// <summary>
        /// Defines a PRIMARY KEY constraint for a table the table declaration block.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="columns">Columns in the PRIMARY KEY constraint definition.</param>
        public static BeginTablePrimaryKeyChainer PrimaryKey(this IBeginTablePrimaryKey prev, params OrderedColumnArgument[] columns)
        {
            return new BeginTablePrimaryKeyChainer((Chainer)prev, columns);
        }

        /// <summary>
        /// Defines a NONCLUSTERED PRIMARY KEY constraint for a table in the table declaration block.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="columns">Columns in the PRIMARY KEY constraint definition.</param>
        public static BeginTablePrimaryKeyChainer PrimaryKeyNonclustered(this IBeginTablePrimaryKey prev, params OrderedColumnArgument[] columns)
        {
            return new BeginTablePrimaryKeyChainer((Chainer)prev, columns, true);
        }

        /// <summary>
        /// Defines a UNIQUE KEY constraint for a table in the table declaration block.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="columns">Columns in the UNIQUE KEY constraint definition.</param>
        public static BeginTableUniqueKeyChainer UniqueKey(this IBeginTableUniqueKey prev, params OrderedColumnArgument[] columns)
        {
            return new BeginTableUniqueKeyChainer((Chainer)prev, columns);
        }

        /// <summary>
        /// Ends the definition block of a table variable or temporary table.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static EndTableChainer EndDesign(this IEndTable prev)
        {
            return new EndTableChainer((Chainer)prev);
        }

        #region DesignTable<T>

        /// <summary>
        /// Declares a table variable using the data class type.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">The name of a table variable.</param>
        /// <param name="skipTimestampColumn">If true, then timestamp column is not included. (Used only with DbRow types.)</param>
        /// <typeparam name="T">A type of a data class.</typeparam>
        public static BeginTableChainer DesignTable<T>(this IAny prev, string table, bool skipTimestampColumn = false)
        {
            return BeginTableChainer.DesingByType<T>((Chainer)prev, table, skipTimestampColumn);
        }

        /// <summary>
        /// Declares a table variable using the data class instance.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">The name of a table variable.</param>
        /// <param name="data">Is a data view.</param>
        /// <param name="fill">A flag to indicate whether the table variable is filled with data.</param>
        public static BeginTableChainer DesignTable(this IAny prev, string table, View data, bool fill = true)
        {
            return new BeginTableChainer((Chainer)prev, table, data, fill, false);
        }

        #endregion

        #region Extra operation on temp tables

        /// <summary>
        /// Creates a relational INDEX on a specified temporary table.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">The name of a temporary table.</param>
        /// <param name="index">The name of the relational index.</param>
        /// <param name="indexType">Is a type of the index.</param>
        public static CreateTempTableIndexChainer CreateTempTableIndex(this IAny prev, 
            string table, string index, Designer.IndexType indexType = Designer.IndexType.Nonclustered)
        {
            return new CreateTempTableIndexChainer((Chainer)prev, table, index, indexType);
        }

        /// <summary>
        /// Defines the columns of a relational index.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="columns">Columns on which the INDEX is based.</param>
        public static CreateTempTableIndexColumnsChainer AddColumns(this ITempTableIndexColumns prev, 
            params OrderedColumnArgument[] columns)
        {
            return new CreateTempTableIndexColumnsChainer((Chainer)prev, columns);
        }

        /// <summary>
        /// Drops the temporary table index.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">The name of a temporary table.</param>
        /// <param name="index">The name of the relational index.</param>
        public static DropTempTableIndexChainer DropTempTableIndex(this IAny prev, string table, string index)
        {
            return new DropTempTableIndexChainer((Chainer)prev, table, index);
        }

        /// <summary>
        /// Drops the temporary table.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">The name of a temporary table.</param>
        public static DropTempTableChainer DropTempTable(this IAny prev, string table)
        {
            return new DropTempTableChainer((Chainer)prev, table);
        }

        /// <summary>
        /// Truncates the table.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">A table name argument.</param>
        public static TruncateTableChainer TruncateTable(this IAny prev, TableArgument table)
        {
            return new TruncateTableChainer((Chainer)prev, table);
        }

        #endregion

    }
}
