#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Reflection;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {

        #region From

        /// <summary>
        /// Specifies the tables, views, derived tables, and joined tables used in SELECT, DELETE, and UPDATE statements.
        /// </summary>
        /// <param name="table">Is a table argument.</param>
        public static FromChainer From(Table table)
        {
            return Call<FromChainer>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (table == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "table", Wall.Text.Method.From);
                }

                var root = new d();
                return new FromChainer(root, table);
            });
        }

        /// <summary>
        /// Specifies the tables, views, derived tables, and joined tables used in SELECT, DELETE, and UPDATE statements.
        /// </summary>
        /// <param name="table">Is a subquery.</param>
        public static FromChainer From(IOpenView table)
        {
            return Call<FromChainer>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (table == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "table", Wall.Text.Method.From);
                }

                var root = new d();
                return new FromChainer(root, table);
            });
        }

        #endregion

        #region FromSelect

        /// <summary>
        /// Retrieves rows from the database with all columns from the specified table.
        /// </summary>
        /// <param name="table">Is a table argument.</param>
        public static SelectChainer FromSelect(Table table)
        {
            if (table == null)
            {
                _Throw(QueryTalkExceptionType.ArgumentNull, "table", Wall.Text.Method.FromSelect);
            }

            var root = new d();
            return new FromChainer(root, table).Select();
        }

        /// <summary>
        /// Retrieves rows from the database with all columns from the specified table.
        /// </summary>
        /// <param name="table">Is a subquery.</param>
        public static SelectChainer FromSelect(IOpenView table)
        {
            return Call<SelectChainer>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (table == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "table", Wall.Text.Method.FromSelect);
                }

                var root = new d();
                return new FromChainer(root, table).Select();
            });
        }

        #endregion

        #region FromMany

        /// <summary>
        /// Retrieves multiple result sets from the specified tables.
        /// </summary>
        /// <param name="firstTable">The first table to select from.</param>
        /// <param name="secondTable">The second table to select from.</param>
        /// <param name="otherTables">Other tables to select from.</param>
        public static FromManyChainer FromMany(TableArgument firstTable, TableArgument secondTable, params TableArgument[] otherTables)
        {
            return Call<FromManyChainer>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (firstTable == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "firstTable", Wall.Text.Method.FromMany);
                }

                if (secondTable == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "secondTable", Wall.Text.Method.FromSelect);
                }

                if (otherTables == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "otherTables", Wall.Text.Method.FromSelect);
                }

                var root = new d();
                return new FromManyChainer(root, firstTable, secondTable, otherTables);
            });
        }

        #endregion

    }
}