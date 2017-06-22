#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;
using System.Data;
using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        #region AddRow

        /// <summary>
        /// Adds an item to the end of the generic ICollection and returns the collection reference. 
        /// </summary>
        /// <typeparam name="T1">The type of a column1 in a 1-column row object.</typeparam>
        /// <param name="collection">The generic collection.</param>
        /// <param name="column1">The first column.</param>
        public static ICollection<Row<T1>> AddRow<T1>(this ICollection<Row<T1>> collection, 
            T1 column1)
        {
            if (collection == null)
            {
                throw new QueryTalkException("Extensions_CLR.AddRow", QueryTalkExceptionType.ArgumentNull, "collection = null", Text.Method.AddRow);
            }

            collection.Add(new Row<T1>(column1));
            return collection;
        }

        /// <summary>
        /// Adds an item to the end of the generic ICollection and returns the collection reference. 
        /// </summary>
        /// <typeparam name="T1">The type of a column1 in a 2-column row object.</typeparam>
        /// <typeparam name="T2">The type of a column2 in a 2-column row object.</typeparam>
        /// <param name="collection">The generic collection.</param>
        /// <param name="column1">The first column.</param>
        /// <param name="column2">The second column.</param>
        public static ICollection<Row<T1, T2>> AddRow<T1, T2>(this ICollection<Row<T1, T2>> collection, 
            T1 column1, T2 column2)
        {
            if (collection == null)
            {
                throw new QueryTalkException("Extensions_CLR.AddRow", QueryTalkExceptionType.ArgumentNull, "collection = null", Text.Method.AddRow);
            }

            collection.Add(new Row<T1, T2>(column1, column2));
            return collection;
        }

        /// <summary>
        /// Adds an item to the end of the generic ICollection and returns the collection reference. 
        /// </summary>
        /// <typeparam name="T1">The type of a column1 in a 3-column row object.</typeparam>
        /// <typeparam name="T2">The type of a column2 in a 3-column row object.</typeparam>
        /// <typeparam name="T3">The type of a column3 in a 3-column row object.</typeparam>
        /// <param name="collection">The generic collection.</param>
        /// <param name="column1">The first column.</param>
        /// <param name="column2">The second column.</param>
        /// <param name="column3">The third column.</param>
        public static ICollection<Row<T1, T2, T3>> AddRow<T1, T2, T3>(this ICollection<Row<T1, T2, T3>> collection,
            T1 column1, T2 column2, T3 column3)
        {
            if (collection == null)
            {
                throw new QueryTalkException("Extensions_CLR.AddRow", QueryTalkExceptionType.ArgumentNull, "collection = null", Text.Method.AddRow);
            }

            collection.Add(new Row<T1, T2, T3>(column1, column2, column3));
            return collection;
        }

        /// <summary>
        /// Adds an item to the end of the generic ICollection and returns the collection reference. 
        /// </summary>
        /// <typeparam name="T1">The type of a column1 in a 4-column row object.</typeparam>
        /// <typeparam name="T2">The type of a column2 in a 4-column row object.</typeparam>
        /// <typeparam name="T3">The type of a column3 in a 4-column row object.</typeparam>
        /// <typeparam name="T4">The type of a column4 in a 4-column row object.</typeparam>
        /// <param name="collection">The generic collection.</param>
        /// <param name="column1">The first column.</param>
        /// <param name="column2">The second column.</param>
        /// <param name="column3">The third column.</param>
        /// <param name="column4">The fourth column.</param>
        public static ICollection<Row<T1, T2, T3, T4>> AddRow<T1, T2, T3, T4>(this ICollection<Row<T1, T2, T3, T4>> collection,
            T1 column1, T2 column2, T3 column3, T4 column4)
        {
            if (collection == null)
            {
                throw new QueryTalkException("Extensions_CLR.AddRow", QueryTalkExceptionType.ArgumentNull, "collection = null", Text.Method.AddRow);
            }

            collection.Add(new Row<T1, T2, T3, T4>(column1, column2, column3, column4));
            return collection;
        }

        /// <summary>
        /// Adds an item to the end of the generic ICollection and returns the collection reference. 
        /// </summary>
        /// <typeparam name="T1">The type of a column1 in a 5-column row object.</typeparam>
        /// <typeparam name="T2">The type of a column2 in a 5-column row object.</typeparam>
        /// <typeparam name="T3">The type of a column3 in a 5-column row object.</typeparam>
        /// <typeparam name="T4">The type of a column4 in a 5-column row object.</typeparam>
        /// <typeparam name="T5">The type of a column5 in a 5-column row object.</typeparam>
        /// <param name="collection">The generic collection.</param>
        /// <param name="column1">The first column.</param>
        /// <param name="column2">The second column.</param>
        /// <param name="column3">The third column.</param>
        /// <param name="column4">The fourth column.</param>
        /// <param name="column5">The fifth column.</param>
        public static ICollection<Row<T1, T2, T3, T4, T5>> AddRow<T1, T2, T3, T4, T5>(this ICollection<Row<T1, T2, T3, T4, T5>> collection,
            T1 column1, T2 column2, T3 column3, T4 column4, T5 column5)
        {
            if (collection == null)
            {
                throw new QueryTalkException("Extensions_CLR.AddRow", QueryTalkExceptionType.ArgumentNull, "collection = null", Text.Method.AddRow);
            }

            collection.Add(new Row<T1, T2, T3, T4, T5>(column1, column2, column3, column4, column5));
            return collection;
        }

        #endregion

        #region ToDataTable

        /// <summary>
        /// Converts a generic collection to the DataTable.
        /// </summary>
        /// <typeparam name="T">Is a type of a generic collection.</typeparam>
        /// <param name="collection">Is a collection object.</param>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
        {
            return ResultSet<T>.ToDataTable<T>(collection);
        }

        #endregion

    }
}
