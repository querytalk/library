#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents a key of the connection data.
    /// </summary>
    public sealed class ConnectionKey : IEquatable<ConnectionKey>
    {
        private string _sKey;
        private Nullable<int> _iKey;

        private bool IsUndefined
        {
            get
            {
                return _sKey == null && _iKey == null;
            }
        }

        private static bool IsNull(ConnectionKey key)
        {
            return (object)key == null || key.IsUndefined;
        }

        #region System.String

        internal ConnectionKey(System.String arg)
        {
            _sKey = arg;
        }

        /// <summary>
        /// Implicitly converts an argument into a ConnectionKey.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ConnectionKey(System.String arg)
        {
            return new ConnectionKey(arg);
        }

        #endregion

        #region System.Int32

        internal ConnectionKey(System.Int32 arg)
        {
            // check negative values - not allowed (reserved for databases map keys)
            if (arg < 0)
            {
                throw new QueryTalkException(this, QueryTalkExceptionType.ConnectionKeyDisallowed,
                    String.Format("connectionKey = {0}", arg));
            }

            _iKey = arg;
        }

        internal static ConnectionKey CreateMapKey(DB3 databaseID)
        {
            var connKey = new ConnectionKey(0);
            connKey._iKey = -databaseID.DbX;
            return connKey;
        }

        /// <summary>
        /// Implicitly converts an argument into a ConnectionKey.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ConnectionKey(System.Int32 arg)
        {
            return new ConnectionKey(arg);
        }

        #endregion

        #region Comparators

        /// <summary>
        /// Determines whether the specified argument is equal to this instance.
        /// </summary>
        /// <param name="other">The argument to compare with this instance.</param>
        public bool Equals(ConnectionKey other)
        {
            if ((object)other == null)
            {
                return false;
            }

            if (other._sKey != _sKey || other._iKey != _iKey)
            {
                return false;
            }

            return true;
        }

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

            return (this as IEquatable<ConnectionKey>)
                .Equals(obj as ConnectionKey);
        }

        /// <summary>
        /// Determines whether the values are equal.
        /// </summary>
        /// <param name="key1">The first value.</param>
        /// <param name="key2">The second value.</param>
        public static bool operator ==(ConnectionKey key1, ConnectionKey key2)
        {
            if (System.Object.ReferenceEquals(key1, key2) || (ConnectionKey.IsNull(key1) && ConnectionKey.IsNull(key2)))
            {
                return true;
            }

            if ((object)key1 == null && (object)key2 != null || (object)key1 != null && (object)key2 == null)
            {
                return false;
            }

            return (key1 as IEquatable<ConnectionKey>)
                .Equals(key2);
        }

        /// <summary>
        /// Determines whether the values are not equal.
        /// </summary>
        /// <param name="key1">The first value.</param>
        /// <param name="key2">The second value.</param>
        public static bool operator !=(ConnectionKey key1, ConnectionKey key2)
        {
            return !(key1 == key2);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            if (_sKey != null)
            {
                return _sKey.GetHashCode();
            }
            else if (_iKey != null)
            {
                return _iKey.GetHashCode();
            }
            else return 0;
        }

        #endregion

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            if (_sKey != null)
            {
                return _sKey;
            }
            else if (_iKey != null)
            {
                return _iKey.ToString() + " (int)";
            }
            else
            {
                return Text.ClrNull;
            }
        }

    }
}
