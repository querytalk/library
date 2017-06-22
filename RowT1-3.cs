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
    /// Represents a 3-column row object.
    /// </summary>
    /// <typeparam name="T1">The type of the column1.</typeparam>
    /// <typeparam name="T2">The type of the column2.</typeparam>
    /// <typeparam name="T3">The type of the column3.</typeparam>
    public sealed class Row<T1, T2, T3> : IEquatable<Row<T1, T2, T3>>, ICloneable
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T3 _column3;
        /// <summary>
        /// The third column.
        /// </summary>
        public T3 Column3
        {
            get
            {
                return _column3;
            }
            set
            {
                _column3 = value;
            }
        }

        /// <summary>
        /// Initializes a default 3-column row.
        /// </summary>
        public Row()
        { }

        /// <summary>
        /// Initializes a new 3-column row.
        /// </summary>
        /// <param name="column1">The value of the column1.</param>
        /// <param name="column2">The value of the column2.</param>
        /// <param name="column3">The value of the column3.</param>
        public Row(T1 column1, T2 column2, T3 column3)
        {
            _column1 = column1;
            _column2 = column2;
            _column3 = column3;
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
            if (properties.Length == 3)
            {
                Type colType1 = properties[0].PropertyType.GetValueType();
                Type colType2 = properties[1].PropertyType.GetValueType();
                Type colType3 = properties[2].PropertyType.GetValueType();
                return
                    Row.ColumnEquality<T1>(_column1, colType1, properties[0].GetValue(obj, null))
                    && Row.ColumnEquality<T2>(_column2, colType2, properties[1].GetValue(obj, null))
                    && Row.ColumnEquality<T3>(_column3, colType3, properties[2].GetValue(obj, null));
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified Row object is equal to this instance.
        /// </summary>
        /// <param name="other">The Row object to compare with this instance.</param>

        public bool Equals(Row<T1, T2, T3> other)
        {
            if (other == null)
            {
                return false;
            }

            return Row.ColumnEquality<T1>(_column1, other.Column1)
                && Row.ColumnEquality<T2>(_column2, other.Column2)
                && Row.ColumnEquality<T3>(_column3, other.Column3);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return
                Value.CombineHashCodes(
                    Value.CombineHashCodes(
                        Value.GetCrossTypeHashCode(typeof(T1), Row.GetHashCode(_column1)),
                        Value.GetCrossTypeHashCode(typeof(T2), Row.GetHashCode(_column2))),
                    Value.GetCrossTypeHashCode(typeof(T3), Row.GetHashCode(_column3)));
        }

        #endregion

        #region ICloneable

        /// <summary>
        /// Performs the deep cloning of the current Row object.
        /// </summary>
        public Row<T1, T2, T3> Clone()
        {
            return new Row<T1, T2, T3>(_column1, _column2, _column3);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Performs the shallow cloning of the current Row object with default values.
        /// </summary>
        public Row<T1, T2, T3> New()
        {
            return new Row<T1, T2, T3>();
        }

        #endregion

        #region AddRow

        /// <summary>
        /// Creates a generic ICollection instance and adds this row and a new 3-column row.
        /// </summary>
        /// <param name="column1">The value of the column1.</param>
        /// <param name="column2">The value of the column2.</param>
        /// <param name="column3">The value of the column3.</param>
        public ICollection<Row<T1, T2, T3>> AddRow(T1 column1, T2 column2, T3 column3)
        {
            return new List<Row<T1, T2, T3>>(new Row<T1, T2, T3>[] { this, 
                new Row<T1, T2, T3>(column1, column2, column3) });
        }

        #endregion

        /// <summary>
        /// Returns a string that represents the value of this Row instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("({0}), ({1}), ({2})", 
                _column1 != null ? _column1.ToString() : "null", 
                _column2 != null ? _column2.ToString() : "null",
                _column3 != null ?_column3.ToString() : "null");
        }
    }

}
