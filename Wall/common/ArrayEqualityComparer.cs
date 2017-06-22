#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;
using System.Linq;

namespace QueryTalk.Wall
{
    /// <summary>
    /// The equality comparer class for array comparison.
    /// </summary>
    /// <typeparam name="T">The type of an array item.</typeparam>
    public class ArrayEqualityComparer<T> : IEqualityComparer<T[]>
    {
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public int GetHashCode(T[] obj)
        {
            if (obj != null)
            {
                unchecked
                {
                    int hash = 17;
                    foreach (var item in obj)
                    {
                        hash = hash * 23 + ((item != null) ? item.GetHashCode() : 0);
                    }

                    return hash;
                }
            }

            return 0;
        }

        /// <summary>
        /// Determines whether the two specified arguments are equal.
        /// </summary>
        /// <param name="x">The first argument to compare.</param>
        /// <param name="y">The first argument to compare.</param>
        public bool Equals(T[] x, T[] y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }

            if (x != null && y != null &&
                (x.Length == y.Length))
            {
                return x.SequenceEqual(y);
            }

            return false;
        }
    }
}
