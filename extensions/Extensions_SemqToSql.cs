#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    // Chain methods that form a bridge between SEMQ and SQL designer.
    public static partial class Extensions
    {

        #region Select

        /// <summary>
        /// Retrieves rows from the database with all columns from the base tables.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static SelectChainer Select(this ISemqToSql prev)
        {
            prev.CheckNullAndThrow(Text.Free.Sentence, Text.Method.Select);
            return new SelectChainer((ISemantic)prev, new Column[] { }, false);
        }

        /// <summary>
        /// Retrieves rows from the database with columns from the specified list of columns.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="columns">
        /// Is a specified list of columns.
        /// </param>
        public static SelectChainer Select(this ISemqToSql prev, params Column[] columns)
        {
            prev.CheckNullAndThrow(Text.Free.Sentence, Text.Method.Select);
            return new SelectChainer((ISemantic)prev, columns, false);
        }

        /// <summary>
        /// Retrieves the number of rows returned by the query.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static SelectChainer SelectCount(this ISemqToSql prev)
        {
            prev.CheckNullAndThrow(Text.Free.Sentence, Text.Method.SelectCount);
            return new SelectChainer((ISemantic)prev, new Column[] { Designer.Count().As(Text.Count) }, false);
        }

        /// <summary>
        /// Retrieves rows from the database with all columns from the base tables.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static SelectChainer SelectDistinct(this ISemqToSql prev)
        {
            prev.CheckNullAndThrow(Text.Free.Sentence, Text.Method.SelectDistinct);
            return new SelectChainer((ISemantic)prev, new Column[] { }, true);
        }

        /// <summary>
        /// Retrieves unique rows from the database with columns from the specified list of columns.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="columns">
        /// Is a specified list of columns.
        /// </param>
        public static SelectChainer SelectDistinct(this ISemqToSql prev, params Column[] columns)
        {
            prev.CheckNullAndThrow(Text.Free.Sentence, Text.Method.SelectDistinct);
            return new SelectChainer((ISemantic)prev, columns, true);
        }

        #endregion

        #region GroupBy

        /// <summary>
        /// Groups a selected set of rows into a set of summary rows by the values of one or more columns or expressions.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="columns">Are column expressions on which grouping is performed.</param>
        public static GroupByChainer GroupBy(this ISemqToSql prev, params GroupingArgument[] columns)
        {
            prev.CheckNullAndThrow(Text.Free.Sentence, Text.Method.GroupBy);
            return new GroupByChainer((ISemantic)prev, columns);
        }

        #endregion

        #region OrderBy

        /// <summary>
        /// Order the result set of a query by the specified column list. The order in which rows are returned in a result set are not guaranteed unless an ORDER BY clause is specified.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="firstColumn">The first column in the column list.</param>
        /// <param name="otherColumns">Other columns in the column list.</param>
        public static OrderByChainer OrderBy(this ISemqToSql prev,
            OrderingArgument firstColumn, params OrderingArgument[] otherColumns)
        {
            prev.CheckNullAndThrow(Text.Free.Sentence, Text.Method.OrderBy);
            return new OrderByChainer((ISemantic)prev,
                Common.MergeArrays<OrderingArgument>(firstColumn, otherColumns ?? new OrderingArgument[] { null }));
        }

        #endregion

        #region Join

        /// <summary>
        /// Joins the nodes in a graph.
        /// </summary>
        /// <param name="prev">A graph.</param>
        public static JoinGraphChainer Join(this ISemqToSql prev)
        {
            prev.CheckNullAndThrow(Text.Free.Graph, Text.Method.Join);
            return  new JoinGraphChainer((ISemantic)prev);
        }

        #endregion

    }
}
