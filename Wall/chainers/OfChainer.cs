#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Diagnostics;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class OfChainer : Chainer, IQuery, INonPredecessor,
        IScalar,
        IAs
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Of;
            }
        }

        internal DbColumn Column { get; private set; }

        internal OfChainer(Chainer prev, DbColumn column, string tableAlias) 
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => column, column));
            CheckNullAndThrow(Arg(() => tableAlias, tableAlias));

            column.OfAlias = tableAlias;
            Build = column.Build;
            Column = column;
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("{0}.{1}", Filter.DelimitNonAsterix(Column.OfAlias), Filter.DelimitNonAsterix(Column.ColumnName));
        }

    }
}
