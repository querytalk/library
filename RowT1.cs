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
    /// Represents a 1-column row object.
    /// </summary>
    /// <typeparam name="T1">The type of the column1.</typeparam>
    public sealed class Row<T1> : IEquatable<Row<T1>>, ICloneable
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

        /// <summary>
        /// Initializes a default 1-column row.
        /// </summary>
        public Row()
        { }

        /// <summary>
        /// Initializes a new 1-column row.
        /// </summary>
        /// <param name="column1">The value of the column1.</param>
        public Row(T1 column1)
        {
            _column1 = column1;
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
            if (properties.Length == 1)
            {
                Type colType1 = properties[0].PropertyType.GetValueType();
                return
                    Row.ColumnEquality<T1>(_column1, colType1, properties[0].GetValue(obj, null));
            }

            return false;
        }

        /// <summary>
        /// Determines whether the specified row object is equal to this instance.
        /// </summary>
        /// <param name="other">The Row object to compare with this instance.</param>
        public bool Equals(Row<T1> other)
        {
            if (other == null)
            {
                return false;
            }

            return Row.ColumnEquality<T1>(_column1, other.Column1);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return Value.GetCrossTypeHashCode(typeof(T1), Row.GetHashCode(_column1));
        }

        #endregion

        #region ICloneable

        /// <summary>
        /// Performs the deep cloning of the current Row object.
        /// </summary>
        public Row<T1> Clone()
        {
            return new Row<T1>(_column1);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Performs the shallow cloning of the current Row object with default values.
        /// </summary>
        public Row<T1> New()
        {
            return new Row<T1>();
        }

        #endregion

        #region AddC

        /// <summary>
        /// Creates a generic ICollection instance and adds this row and a new 1-column row.
        /// </summary>
        /// <param name="column1">The value of the column1.</param>
        public ICollection<Row<T1>> AddRow(T1 column1)
        {
            return new List<Row<T1>>(new Row<T1>[] { this, new Row<T1>(column1) });
        }

        #endregion

        /// <summary>
        /// Returns a string that represents the value of this Row instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("({0})",
                _column1 != null ? _column1.ToString() : "null");
        }
    }

}
