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
    /// Represents a 5-column row object.
    /// </summary>
    /// <typeparam name="T1">The type of the column1.</typeparam>
    /// <typeparam name="T2">The type of the column2.</typeparam>
    /// <typeparam name="T3">The type of the column3.</typeparam>
    /// <typeparam name="T4">The type of the column4.</typeparam>
    /// <typeparam name="T5">The type of the column5.</typeparam>
    public sealed class Row<T1, T2, T3, T4, T5> : IEquatable<Row<T1, T2, T3, T4, T5>>, ICloneable
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

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T4 _column4;
        /// <summary>
        /// The fourth column.
        /// </summary>
        public T4 Column4
        {
            get
            {
                return _column4;
            }
            set
            {
                _column4 = value;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private T5 _column5;
        /// <summary>
        /// The fifth column.
        /// </summary>
        public T5 Column5
        {
            get
            {
                return _column5;
            }
            set
            {
                _column5 = value;
            }
        }

        /// <summary>
        /// Initializes a default 5-column row.
        /// </summary>
        public Row()
        { }

        /// <summary>
        /// Initializes a new 5-column row.
        /// </summary>
        /// <param name="column1">The value of the column1.</param>
        /// <param name="column2">The value of the column2.</param>
        /// <param name="column3">The value of the column3.</param>
        /// <param name="column4">The value of the column4.</param>
        /// <param name="column5">The value of the column5.</param>
        public Row(T1 column1, T2 column2, T3 column3, T4 column4, T5 column5)
        {
            _column1 = column1;
            _column2 = column2;
            _column3 = column3;
            _column4 = column4;
            _column5 = column5;
        }

        #region IEquatable

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The specified object to compare with this instance.</param>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Type objType = obj.GetType();
            var properties = objType.GetProperties();
            if (properties.Length == 5)
            {
                Type colType1 = properties[0].PropertyType.GetValueType();
                Type colType2 = properties[1].PropertyType.GetValueType();
                Type colType3 = properties[2].PropertyType.GetValueType();
                Type colType4 = properties[3].PropertyType.GetValueType();
                Type colType5 = properties[4].PropertyType.GetValueType();
                return
                    Row.ColumnEquality<T1>(_column1, colType1, properties[0].GetValue(obj, null))
                    && Row.ColumnEquality<T2>(_column2, colType2, properties[1].GetValue(obj, null))
                    && Row.ColumnEquality<T3>(_column3, colType3, properties[2].GetValue(obj, null))
                    && Row.ColumnEquality<T4>(_column4, colType4, properties[3].GetValue(obj, null))
                    && Row.ColumnEquality<T5>(_column5, colType5, properties[4].GetValue(obj, null));
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified Row object is equal to this instance.
        /// </summary>
        /// <param name="other">The Row object to compare with this instance.</param>
        public bool Equals(Row<T1, T2, T3, T4, T5> other)
        {
            if (other == null)
            {
                return false;
            }

            return Row.ColumnEquality<T1>(_column1, other.Column1)
                && Row.ColumnEquality<T2>(_column2, other.Column2)
                && Row.ColumnEquality<T3>(_column3, other.Column3)
                && Row.ColumnEquality<T4>(_column4, other.Column4)
                && Row.ColumnEquality<T5>(_column5, other.Column5);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return
                Value.CombineHashCodes(
                    Value.CombineHashCodes(
                        Value.CombineHashCodes(
                            Value.CombineHashCodes(
                                Value.GetCrossTypeHashCode(typeof(T1), Row.GetHashCode(_column1)),
                                Value.GetCrossTypeHashCode(typeof(T2), Row.GetHashCode(_column2))),
                            Value.GetCrossTypeHashCode(typeof(T3), Row.GetHashCode(_column3))),
                        Value.GetCrossTypeHashCode(typeof(T4), Row.GetHashCode(_column4))),
                    Value.GetCrossTypeHashCode(typeof(T5), Row.GetHashCode(_column5)));
        }

        #endregion

        #region ICloneable

        /// <summary>
        /// Performs the deep cloning of the current Row object.
        /// </summary>
        public Row<T1, T2, T3, T4, T5> Clone()
        {
            return new Row<T1, T2, T3, T4, T5>(_column1, _column2, _column3, _column4, _column5);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Performs the shallow cloning of the current Row object with default values.
        /// </summary>
        public Row<T1, T2, T3, T4, T5> New()
        {
            return new Row<T1, T2, T3, T4, T5>();
        }

        #endregion

        #region AddRow

        /// <summary>
        /// Creates a generic ICollection instance and adds this row and a new 5-column row.
        /// </summary>
        /// <param name="column1">The value of the column1.</param>
        /// <param name="column2">The value of the column2.</param>
        /// <param name="column3">The value of the column3.</param>
        /// <param name="column4">The value of the column4.</param>
        /// <param name="column5">The value of the column5.</param>
        public ICollection<Row<T1, T2, T3, T4, T5>> AddRow(T1 column1, T2 column2, T3 column3, T4 column4, T5 column5)
        {
            return new List<Row<T1, T2, T3, T4, T5>>(new Row<T1, T2, T3, T4, T5>[] { this, 
                new Row<T1, T2, T3, T4, T5>(column1, column2, column3, column4, column5) });
        }

        #endregion

        /// <summary>
        /// Returns a string that represents the value of this Row instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("({0}), ({1}), ({2}), ({3}), ({4})",
                _column1 != null ? _column1.ToString() : "null",
                _column2 != null ? _column2.ToString() : "null",
                _column3 != null ? _column3.ToString() : "null",
                _column4 != null ? _column4.ToString() : "null",
                _column5 != null ? _column5.ToString() : "null");
        }
    }

}
