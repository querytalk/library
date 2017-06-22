#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.ComponentModel;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Represents all columns of a database object.
    /// </summary>
    public class DbColumns
    {
        internal DbColumn Column { get; private set; }

        internal Func<BuildContext, BuildArgs, string> Build
        {
            get
            {
                return Column.Build;
            }
        }

        internal DbNode Parent
        {
            get
            {
                return Column.Parent;
            }
        }

        internal string Alias
        {
            get
            {
                return Column.OfAlias;
            }
        }

        internal DbColumns(DbNode parent, DB3 columnID)
        {
            Column = new DbColumn(parent, columnID);
        }

        internal DbColumns(DbNode parent, DB3 columnID, string alias)
        {
            Column = new DbColumn(parent, columnID, alias);
        }

        #region Not browsable

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType()
        {
            return base.GetType();
        }

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool Equals(object objA, object objB)
        {
            return Equals(objA, objB);
        }

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool ReferenceEquals(object objA, object objB)
        {
            return ReferenceEquals(objA, objB);
        }

        #endregion


    }
}
