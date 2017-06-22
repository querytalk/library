#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;

namespace QueryTalk.Wall
{
    /// <summary>
    /// The comparer class that implements IComparer&lt;T&gt; interface.
    /// </summary>
    public sealed class StringComparer : IComparer<System.String>
    {
        /// <summary>
        /// Compares two specified values and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="x">The first string to compare.</param>
        /// <param name="y">The second string to compare.</param>
        public int Compare(string x, string y)
        {
            return String.CompareOrdinal(x, y);
        }
    }

    /// <summary>
    /// The comparer class that implements IComparer&lt;T&gt; interface.
    /// </summary>
    public sealed class GuidComparer : IComparer<System.Guid>
    {
        /// <summary>
        /// Compares two specified values and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="x">The first string to compare.</param>
        /// <param name="y">The second string to compare.</param>
        public int Compare(Guid x, Guid y)
        {
            return x.CompareTo(y);
        }
    }

    /// <summary>
    /// The comparer class that implements IComparer&lt;T&gt; interface.
    /// </summary>
    public sealed class ByteArrayComparer : IComparer<System.Byte[]>
    {
        /// <summary>
        /// Compares two specified values and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="x">The first string to compare.</param>
        /// <param name="y">The second string to compare.</param>
        public int Compare(byte[] x, byte[] y)
        {
            return Comparers.ByteArrayComparer(x, y);
        }
    }

    /// <summary>
    /// Encapsulates various public comparer methods.
    /// </summary>
    public static class Comparers
    {

        #region System.Guid

        /// <summary>
        /// Determines whether the first argument is greater than the second one.
        /// </summary>
        /// <param name="x">The first argument to compare.</param>
        /// <param name="y">The second argument to compare.</param>
        public static bool GuidGreaterThan(Guid x, Guid y)
        {
            return x.CompareTo(y) > 0;
        }

        /// <summary>
        /// Determines whether the first argument is smaller than the second one.
        /// </summary>
        /// <param name="x">The first argument to compare.</param>
        /// <param name="y">The second argument to compare.</param>
        public static bool GuidLessThan(Guid x, Guid y)
        {
            return x.CompareTo(y) < 0;
        }

        /// <summary>
        /// Determines whether the two arguments are equal.
        /// </summary>
        /// <param name="x">The first argument to compare.</param>
        /// <param name="y">The second argument to compare.</param>
        public static bool GuidEqual(Guid x, Guid y)
        {
            return x.CompareTo(y) == 0;
        }

        #endregion

        #region System.Byte[]

        /// <summary>
        /// Compares two byte arrays and returns an integer that indicates their relative position in the sort order.
        /// </summary>
        /// <param name="x">The first byte array to compare.</param>
        /// <param name="y">The second byte array to compare.</param>
        public static int ByteArrayComparer(byte[] x, byte[] y)
        {
            if (x == null && y == null)
            {
                return 0;  
            }

            if (x == null)
            {
                return -1; 
            }

            if (y == null)
            {
                return 1;  
            }

            var cx = x.Length;
            var cy = y.Length;

            for (int i = 0; i < ((cx < cy) ? cx : cy); ++i)
            {
                if (x[i] > y[i]) { return 1; }  
                if (x[i] < y[i]) { return -1; } 
            }

            if (cx == cy)
            {
                return 0;
            }

            if (cx < cy)
            {
                return -1;
            }

            return 1;
        }

        /// <summary>
        /// Determines whether the first byte array is greater than the second one.
        /// </summary>
        /// <param name="x">The first byte array to compare.</param>
        /// <param name="y">The second byte array to compare.</param>
        public static bool ByteArrayGreaterThan(byte[] x, byte[] y)
        {
            return ByteArrayComparer(x, y) > 0;
        }

        /// <summary>
        /// Determines whether the first byte array is smaller than the second one.
        /// </summary>
        /// <param name="x">The first byte array to compare.</param>
        /// <param name="y">The second byte array to compare.</param>
        public static bool ByteArrayLessThan(byte[] x, byte[] y)
        {
            return ByteArrayComparer(x, y) < 0;
        }

        /// <summary>
        /// Determines whether the two byte arrays are equal.
        /// </summary>
        /// <param name="x">The first byte array to compare.</param>
        /// <param name="y">The second byte array to compare.</param>
        public static bool ByteArrayEqual(byte[] x, byte[] y)
        {
            return ByteArrayComparer(x, y) == 0;
        }

        #endregion

        #region System.Object/[sql_variant]

        /// <summary>
        /// Determines whether the two values given as objects are equal.
        /// </summary>
        /// <param name="x">The first value to compare.</param>
        /// <param name="y">The second value to compare.</param>
        public static bool SqlVariantEqual(object x, object y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            // numeric
            Nullable<decimal> xdec = _TryGetDecimal(x);
            Nullable<decimal> ydec = _TryGetDecimal(y);
            if (xdec != null && ydec != null)
            {
                return xdec == ydec;
            }
            
            var xtype = x.GetType();

            // check other types: type mismatch
            if (xtype != y.GetType())
            {
                return false;
            }

            if (xtype == typeof(string))
            {
                return (string)x == (string)y;
            }

            if (xtype == typeof(byte[]))
            {
                var byteComparer = new QueryTalk.Wall.ArrayEqualityComparer<byte>();
                return byteComparer.Equals((byte[])x, (byte[])y);
            }

            if (xtype == typeof(DateTime))
            {
                return (DateTime)x == (DateTime)y;
            }

            if (xtype == typeof(DateTimeOffset))
            {
                return (DateTimeOffset)x == (DateTimeOffset)y;
            }

            if (xtype == typeof(TimeSpan))
            {
                return (TimeSpan)x == (TimeSpan)y;
            }

            if (xtype == typeof(Guid))
            {
                return (Guid)x == (Guid)y;
            }

            return false;
        }

        private static Nullable<decimal> _TryGetDecimal(object value)
        {
            // special numeric case: SQL Server treats bool as numeric
            if (value is bool)
            {
                return ((bool)value) ? 1 : 0;
            }

            // regular numeric values
            if (value is byte
                || value is short
                || value is int
                || value is long
                || value is float
                || value is double
                || value is decimal)
            {
                return Convert.ToDecimal(value);
            }

            return null;
        }

        #endregion

    }
}
