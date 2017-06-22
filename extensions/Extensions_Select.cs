#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        #region Select

        /// <summary>
        /// Retrieves rows from the database with all columns from the base tables.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static SelectChainer Select(this ISelect prev)
        {
            return new SelectChainer((Chainer)prev, new Column[] { }, false); 
        }

        /// <summary>
        /// Retrieves rows from the database with columns from the specified list of columns.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="columns">
        /// Is a specified list of columns.
        /// </param>
        public static SelectChainer Select(this ISelect prev, params Column[] columns)
        {
            return new SelectChainer((Chainer)prev, columns, false); 
        }

        /// <summary>
        /// <para>Retrieves the number of rows returned by the query.</para>
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static SelectChainer SelectCount(this ISelect prev)
        {
            return new SelectChainer((Chainer)prev, new Column[] { Designer.Count().As(Text.Count) }, false);
        }

        /// <summary>
        /// <para>Retrieves rows from the database with all columns from the base tables.</para>
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static SelectChainer SelectDistinct(this ISelect prev)
        {
            return new SelectChainer((Chainer)prev, new Column[] { }, true);
        }

        /// <summary>
        /// <para>Retrieves unique rows from the database with columns from the specified list of columns.</para>
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="columns">
        /// Is a specified list of columns.
        /// </param>
        public static SelectChainer SelectDistinct(this ISelect prev, params Column[] columns)
        {
            return new SelectChainer((Chainer)prev, columns, true);
        }

        #endregion

        #region FromSelect

        /// <summary>
        /// Retrieves rows from the database with all columns from the specified table.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is a table argument.</param>
        public static SelectChainer FromSelect(this IFrom prev, Table table)
        {
            return new FromChainer((Chainer)prev, table).Select();
        }

        /// <summary>
        /// Retrieves rows from the database with all columns from the specified table.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is a subquery.</param>
        public static SelectChainer FromSelect(this IFrom prev, IOpenView table)
        {
            return new FromChainer((Chainer)prev, table).Select();
        }

        #endregion

    }
}
