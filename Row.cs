#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Data.Linq;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Provides static methods for creating Row objects.
    /// </summary>
    public static class Row
    {

        #region Create

        /// <summary>
        /// Creates a 1-column row object.
        /// </summary>
        /// <typeparam name="T1">The type of the column1.</typeparam>
        /// <param name="column1">The value of the column1.</param>
        public static Row<T1> Create<T1>(T1 column1)
        {
            return new Row<T1>(column1);
        }

        /// <summary>
        /// Creates a 2-column row object.
        /// </summary>
        /// <typeparam name="T1">The type of the column1.</typeparam>
        /// <typeparam name="T2">The type of the column2.</typeparam>
        /// <param name="column1">The value of the column1.</param>
        /// <param name="column2">The value of the column2.</param>
        public static Row<T1, T2> Create<T1, T2>(T1 column1, T2 column2)
        {
            return new Row<T1, T2>(column1, column2);
        }

        /// <summary>
        /// Creates a 3-column row object.
        /// </summary>
        /// <typeparam name="T1">The type of the column1.</typeparam>
        /// <typeparam name="T2">The type of the column2.</typeparam>
        /// <typeparam name="T3">The type of the column3.</typeparam>
        /// <param name="column1">The value of the column1.</param>
        /// <param name="column2">The value of the column2.</param>
        /// <param name="column3">The value of the column3.</param>
        public static Row<T1, T2, T3> Create<T1, T2, T3>(T1 column1, T2 column2, T3 column3)
        {
            return new Row<T1, T2, T3>(column1, column2, column3);
        }

        /// <summary>
        /// Creates a 4-column row object.
        /// </summary>
        /// <typeparam name="T1">The type of the column1.</typeparam>
        /// <typeparam name="T2">The type of the column2.</typeparam>
        /// <typeparam name="T3">The type of the column3.</typeparam>
        /// <typeparam name="T4">The type of the column4.</typeparam>
        /// <param name="column1">The value of the column1.</param>
        /// <param name="column2">The value of the column2.</param>
        /// <param name="column3">The value of the column3.</param>
        /// <param name="column4">The value of the column4.</param>
        public static Row<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 column1, T2 column2, T3 column3, T4 column4)
        {
            return new Row<T1, T2, T3, T4>(column1, column2, column3, column4);
        }

        /// <summary>
        /// Creates a 5-column row object.
        /// </summary>
        /// <typeparam name="T1">The type of the column1.</typeparam>
        /// <typeparam name="T2">The type of the column2.</typeparam>
        /// <typeparam name="T3">The type of the column3.</typeparam>
        /// <typeparam name="T4">The type of the column4.</typeparam>
        /// <typeparam name="T5">The type of the column5.</typeparam>
        /// <param name="column1">The value of the column1.</param>
        /// <param name="column2">The value of the column2.</param>
        /// <param name="column3">The value of the column3.</param>
        /// <param name="column4">The value of the column4.</param>
        /// <param name="column5">The value of the column5.</param>
        public static Row<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 column1, T2 column2, T3 column3, T4 column4, T5 column5)
        {
            return new Row<T1, T2, T3, T4, T5>(column1, column2, column3, column4, column5);
        }

        #endregion

        #region IsRow

        internal static bool IsRow(Type type)
        {
            if (!type.IsGenericType)
            {
                return false;
            }

            if (type.IsGenericType)
            {
                var typedef = type.GetGenericTypeDefinition();
                if (typedef == typeof(Row<>)
                    || typedef == typeof(Row<,>)
                    || typedef == typeof(Row<,,>)
                    || typedef == typeof(Row<,,,>)
                    || typedef == typeof(Row<,,,,>))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Equality

        internal static int GetHashCode<T>(T column)
        {
            if (column == null)
            {
                return 0;
            }

            if (column is System.Byte[])
            {
                return new Binary((System.Byte[])(object)column).GetHashCode();
            }
            else
            {
                return column.GetHashCode();
            }
        }

        internal static bool ColumnEquality<T>(T column, T other)
        {
            if (column == null && other == null)
            {
                return true;
            }

            else if (column == null || other == null)
            {
                return false;
            }

            if (Mapping.BuildUnchecked(column) != Mapping.BuildUnchecked(other))
            {
                return false;
            }

            return true;
        }

        internal static bool ColumnEquality<T>(T column, Type otherType, object otherValue)
        {
            Type type = typeof(T);
            if (type.IsNullable())
            {
                type = Nullable.GetUnderlyingType(type);
            }

            if (type != otherType)
            {
                return false;
            }

            if (column == null && otherValue == null)
            {
                return true;
            }

            else if (column == null || otherValue == null)
            {
                return false;
            }

            if (Mapping.BuildUnchecked(column) != Mapping.BuildUnchecked(otherValue))
            {
                return false;
            }

            return true;
        }

        #endregion

    }

}
