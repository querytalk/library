#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    /// <summary>
    /// A unique 3-part identifier of a database, a database object, or column.
    /// </summary>
    public struct DB3
    {
        private int _dbX;

        /// <summary>
        /// Gets a database component.
        /// </summary>
        public int DbX
        {
            get
            {
                return _dbX;
            }
        }

        private int _tableY;

        /// <summary>
        /// Gets a table component.
        /// </summary>
        public int TableY
        {
            get
            {
                return _tableY;
            }
        }

        private int _columnZ;

        /// <summary>
        /// Gets a column component.
        /// </summary>
        public int ColumnZ
        {
            get
            {
                return _columnZ;
            }
        }

        /// <summary>
        /// Gets a default value.
        /// </summary>
        public static DB3 Default
        {
            get
            {
                return DB3.Get(0, 0, 0);
            }
        }

        /// <summary>
        /// Returns true if this instance has a default value.
        /// </summary>
        public bool IsDefault
        {
            get
            {
                return Equals(Default);
            }
        }

        /// <summary>
        /// Gets a database identifier of this instance.
        /// </summary>
        public DB3 DatabaseID
        {
            get
            {
                return DB3.Get(DbX);
            }
        }

        /// <summary>
        /// Gets a table identifier of this instance.
        /// </summary>
        public DB3 TableID
        {
            get
            {
                return DB3.Get(DbX, TableY);
            }
        }

        /// <summary>
        /// Returns a column identifier of this instance.
        /// </summary>
        /// <param name="columnZ">A column component.</param>
        public DB3 GetColumnID(int columnZ)
        {
            return DB3.Get(DbX, TableY, columnZ);
        }

        /// <summary>
        /// Initializes a new instance of DB3 struct.
        /// </summary>
        /// <param name="dbX">A database component.</param>
        /// <param name="tableY">A table component.</param>
        /// <param name="columnZ">A column component.</param>
        public DB3(int dbX, int tableY, int columnZ)
        {
            _dbX = dbX;
            _tableY = tableY;
            _columnZ = columnZ;
        }

        internal DB3 Coalesce(DB3 otherValue)
        {
            return IsDefault ? otherValue : this;
        }

        #region Static

        /// <summary>
        /// Returns a database identifier.
        /// </summary>
        /// <param name="dbX">A database component.</param>
        public static DB3 Get(int dbX)
        {
            return new DB3(dbX, 0, 0);
        }

        /// <summary>
        /// Returns a database-table identifier.
        /// </summary>
        /// <param name="dbX">A database component.</param>
        /// <param name="tableY">A table component.</param>
        public static DB3 Get(int dbX, int tableY)
        {
            return new DB3(dbX, tableY, 0);
        }

        /// <summary>
        /// Returns a database-table-column identifier.
        /// </summary>
        /// <param name="dbX">A database component.</param>
        /// <param name="tableY">A table component.</param>
        /// <param name="columnZ">A column component.</param>
        public static DB3 Get(int dbX, int tableY, int columnZ)
        {
            return new DB3(dbX, tableY, columnZ);
        }

        #endregion

        #region Overriden methods

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            var hash = 23;
            hash = (hash * 37) + _dbX;
            hash = (hash * 37) + _tableY;
            hash = (hash * 37) + _columnZ;
            return hash;
        }

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        public override bool Equals(object obj)
        {
            if (!(obj is DB3))
            {
                return false;
            }
            return Equals((DB3)obj);
        }

        /// <summary>
        /// Determines whether the specified argument is equal to this instance.
        /// </summary>
        /// <param name="other">The argument to compare with this instance.</param>
        public bool Equals(DB3 other)
        {
            if (other.DbX == _dbX && other.TableY == _tableY && other.ColumnZ == _columnZ)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the values are equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        public static bool operator ==(DB3 value1, DB3 value2)
        {
            return value1.Equals(value2);
        }

        /// <summary>
        /// Determines whether the values are not equal.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        public static bool operator !=(DB3 value1, DB3 value2)
        {
            return !value1.Equals(value2);
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("({0},{1},{2})", _dbX, _tableY, _columnZ);
        }

        #endregion

    }
}
