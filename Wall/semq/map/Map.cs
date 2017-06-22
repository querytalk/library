#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;

namespace QueryTalk.Wall
{
    /// <summary>
    /// A base class of all map objects. (Not intended for public use.)
    /// </summary>
    public abstract class Map
    {
        /// <summary>
        /// A unique identifier of the database object.
        /// </summary>
        public DB3 ID { get; private set; }

        private ConnectionKey _connectionKey = null;

        /// <summary>
        ///  Gets the key of the connection data.
        /// </summary>
        public ConnectionKey ConnectionKey
        {
            get
            {
                if ((object)_connectionKey == null)
                {
                    _connectionKey = ConnectionKey.CreateMapKey(ID);
                }
                return _connectionKey;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Map class.
        /// </summary>
        /// <param name="id">Is a unique identifier of the database object.</param>
        protected Map(DB3 id)
        {
            ID = id;
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return ID.ToString();
        }
    }

    /// <summary>
    /// IEqualityComparer implementation class of the Map class. 
    /// </summary>
    public class MapEqualityComparer : IEqualityComparer<Map>
    {
        /// <summary>
        /// Determines whether the values are equal.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        public bool Equals(Map x, Map y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.ID.Equals(y.ID);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public int GetHashCode(Map obj)
        {
            if (obj == null)
            {
                return 0;
            }

            return obj.ID.GetHashCode();
        }
    }
}
