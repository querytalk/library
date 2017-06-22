#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Linq;

namespace QueryTalk.Wall
{
    // for .Insert & .Update
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class ColumnsChainer : Chainer, IQuery,
        IInsert,
        IUpdate
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Columns;
            }
        }

        private Column[] _columns;

        internal ColumnsChainer(Chainer prev, NonSelectColumnArgument[] columns) 
            : base(prev)
        {
            CheckNullOrEmptyAndThrow(Argc(() => columns, columns));
            _columns = new Column[columns.Length];
            for (int i = 0; i < columns.Length; ++i)
            {
                if (columns[i].IsUndefined())
                {
                    Throw(
                        QueryTalkExceptionType.ArgumentNull,
                        String.Format("column = null"));
                }

                _columns[i] = Column.ToColumn(columns[i]);
            }

            Build = (buildContext, buildArgs) =>
                {
                    if (buildArgs.Executable == null)
                    {
                        var selectable = GetPrev<ISelectable>();
                        if (selectable.IsEmpty)
                        {
                            Throw(QueryTalkExceptionType.UndefinedColumnsDisallowed, null, Text.Method.Select);
                        }

                        if (columns.Count() != selectable.ColumnCount)
                        {
                            Throw(QueryTalkExceptionType.InvalidColumnCount,
                                String.Format("value count = {0}{1}   column count = {2}{3}   columns = {4}",
                                    selectable.ColumnCount, Environment.NewLine,
                                    columns.Count(), Environment.NewLine,
                                    ToString()));
                        }
                    }

                    // build is done in the next WriteChainer object
                    ((WriteChainer)Next).Columns = _columns;  
                    return null;   
                };
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            if (_columns != null)
            {
                return String.Format("{0}",
                    String.Join(",", _columns.Where(c => c != null)
                        .Select(c =>
                        {
                            if (c.Original != null)
                            {
                                return Filter.Delimit((string)c.Original);
                            }

                            return Text.Null;
                        })));
            }

            return Text.NotAvailable;
        }
    }
}
