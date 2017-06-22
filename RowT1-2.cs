#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace QueryTalk
{
    /// <summary>
    /// Represents a 2-column row object.
    /// </summary>
    /// <typeparam name="T1">The type of the column1.</typeparam>
    /// <typeparam name="T2">The type of the column2.</typeparam>
    public sealed class Row<T1, T2> : IEquatable<Row<T1, T2>>, ICloneable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T1 _column1;
        /// <summary>
        /// The first column.
        /// </summary>
        public T1 Column1
        {
            get
            {
                return _column1;
            }
            set
            {
                _column1 = value;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T2 _column2;
        /// <summary>
        /// The second column.
        /// </summary>
        public T2 Column2
        {
            get
            {
                return _column2;
            }
            set
            {
                _column2 = value;
            }
        }

        /// <summary>
        /// Initializes a default 2-column row.
        /// </summary>
        public Row()
        { }

        /// <summary>
        /// Initializes a new 2-column row.
        /// </summary>
        /// <param name="column1">The value of the column1.</param>
        /// <param name="column2">The value of the column2.</param>
        public Row(T1 column1, T2 column2)
        {
            _column1 = column1;
            _column2 = column2;
        }

        #region IEquatable

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Type objType = obj.GetType();
            var properties = objType.GetProperties();
            if (properties.Length == 2)
            {
                Type colType1 = properties[0].PropertyType.GetValueType();
                Type colType2 = properties[1].PropertyType.GetValueType();
                return
                    Row.ColumnEquality<T1>(_column1, colType1, properties[0].GetValue(obj, null))
                    && Row.ColumnEquality<T2>(_column2, colType2, properties[1].GetValue(obj, null));
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified Row object is equal to this instance.
        /// </summary>
        /// <param name="other">The Row object to compare with this instance.</param>
        public bool Equals(Row<T1, T2> other)
        {
            if (other == null)
            {
                return false;
            }

            return Row.ColumnEquality<T1>(_column1, other.Column1)
                && Row.ColumnEquality<T2>(_column2, other.Column2);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return Value.CombineHashCodes(
                Value.GetCrossTypeHashCode(typeof(T1), Row.GetHashCode(_column1)),
                Value.GetCrossTypeHashCode(typeof(T2), Row.GetHashCode(_column2)));
        }

        #endregion

        #region ICloneable

        /// <summary>
        /// Performs the deep cloning of the current Row object.
        /// </summary>
        public Row<T1, T2> Clone()
        {
            return new Row<T1, T2>(_column1, _column2);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Performs the shallow cloning of the current Row object with default values.
        /// </summary>
        public Row<T1, T2> New()
        {
            return new Row<T1, T2>();
        }

        #endregion

        #region AddRow

        /// <summary>
        /// Creates a generic ICollection instance and adds this row and a new 2-column row.
        /// </summary>
        /// <param name="column1">The value of the column1.</param>
        /// <param name="column2">The value of the column2.</param>
        public ICollection<Row<T1, T2>> AddRow(T1 column1, T2 column2)
        {
            return new List<Row<T1, T2>>(new Row<T1, T2>[] { this, new Row<T1, T2>(column1, column2) });
        }

        #endregion

        /// <summary>
        /// Returns a string that represents the value of this Row instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("({0}), ({1})",
                _column1 != null ? _column1.ToString() : "null",
                _column2 != null ? _column2.ToString() : "null");
        }
    }

}
