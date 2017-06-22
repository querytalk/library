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
        /// Order the result set of a query by the specified column list. The order in which rows are returned in a result set are not guaranteed unless an ORDER BY clause is specified.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="firstColumn">The first column in the column list.</param>
        /// <param name="otherColumns">Other columns in the column list.</param>
        public static OrderByChainer OrderBy(this IOrderBy prev,
            OrderingArgument firstColumn, params OrderingArgument[] otherColumns)
        {
            return new OrderByChainer((Chainer)prev,
                Common.MergeArrays<OrderingArgument>(
                    firstColumn, otherColumns ?? new OrderingArgument[] { null }));
        }

        #region AsAsc

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in ascending order. ASC is the default sort order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a string identifier of a column on which to sort the query result set.</param>
        public static OrderedChainer AsAsc(this string column)
        {
            return new OrderedChainer(column, SortOrder.Asc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in ascending order. ASC is the default sort order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is an identifier of a column on which to sort the query result set.</param>
        public static OrderedChainer AsAsc(this Identifier column)
        {
            return new OrderedChainer(column, SortOrder.Asc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in ascending order. ASC is the default sort order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a column expression of a built-in system function on which to sort the query result set.</param>
        public static OrderedChainer AsAsc(this SysFn column)
        {
            return new OrderedChainer(column, SortOrder.Asc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in ascending order. ASC is the default sort order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a column expression of a CASE clause on which to sort the query result set.</param>
        public static OrderedChainer AsAsc(this CaseExpressionChainer column)
        {
            return new OrderedChainer(column, SortOrder.Asc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in ascending order. ASC is the default sort order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a column expression consisting of a table alias and a mapped column on which to sort the query result set.</param>
        public static OrderedChainer AsAsc(this OfChainer column)
        {
            return new OrderedChainer(column, SortOrder.Asc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in ascending order. ASC is the default sort order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a column expression of a scalar user-defined function on which to sort the query result set.</param>
        public static OrderedChainer AsAsc(this Udf column)
        {
            return new OrderedChainer(column, SortOrder.Asc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in ascending order. ASC is the default sort order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a column expression of a scalar view on which to sort the query result set.</param>
        public static OrderedChainer AsAsc(this View column)
        {
            return new OrderedChainer(column, SortOrder.Asc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in ascending order. ASC is the default sort order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a column expression of a scalar expression on which to sort the query result set.</param>
        public static OrderedChainer AsAsc(this Expression column)
        {
            return new OrderedChainer(column, SortOrder.Asc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in ascending order. ASC is the default sort order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a column expression of a specified collation on which to sort the query result set.</param>
        public static OrderedChainer AsAsc(this CollateChainer column)
        {
            return new OrderedChainer(column, SortOrder.Asc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in ascending order. ASC is the default sort order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a column expression of a mapped column on which to sort the query result set.</param>
        public static OrderedChainer AsAsc(this DbColumn column)
        {
            return new OrderedChainer(column, SortOrder.Asc);
        }

        #endregion

        #region AsDesc

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in descending order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a string identifier of a column on which to sort the query result set.</param>
        public static OrderedChainer AsDesc(this System.String column)
        {
            return new OrderedChainer(column, SortOrder.Desc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in descending order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is an identifier of a column on which to sort the query result set.</param>
        public static OrderedChainer AsDesc(this Identifier column)
        {
            return new OrderedChainer(column, SortOrder.Desc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in descending order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a column expression of a built-in system function on which to sort the query result set.</param>
        public static OrderedChainer AsDesc(this SysFn column)
        {
            return new OrderedChainer(column, SortOrder.Desc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in descending order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a column expression of a CASE clause on which to sort the query result set.</param>
        public static OrderedChainer AsDesc(this CaseExpressionChainer column)
        {
            return new OrderedChainer(column, SortOrder.Desc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in descending order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a column expression consisting of a table alias and a mapped column on which to sort the query result set.</param>
        public static OrderedChainer AsDesc(this OfChainer column)
        {
            return new OrderedChainer(column, SortOrder.Desc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in descending order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a column expression of a scalar user-defined function on which to sort the query result set.</param>
        public static OrderedChainer AsDesc(this Udf column)
        {
            return new OrderedChainer(column, SortOrder.Desc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in descending order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a column expression of a scalar view on which to sort the query result set.</param>
        public static OrderedChainer AsDesc(this View column)
        {
            return new OrderedChainer(column, SortOrder.Desc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in descending order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a column expression of a scalar expression on which to sort the query result set.</param>
        public static OrderedChainer AsDesc(this Expression column)
        {
            return new OrderedChainer(column, SortOrder.Desc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in descending order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a column expression of a specified collation on which to sort the query result set.</param>
        public static OrderedChainer AsDesc(this CollateChainer column)
        {
            return new OrderedChainer(column, SortOrder.Desc);
        }

        /// <summary>
        /// Specifies that the values in the specified column should be sorted in descending order. Null values are treated as the lowest possible values.
        /// </summary>
        /// <param name="column">Is a column expression of a mapped column on which to sort the query result set.</param>
        public static OrderedChainer AsDesc(this DbColumn column)
        {
            return new OrderedChainer(column, SortOrder.Desc);
        }

        #endregion
    }
}
