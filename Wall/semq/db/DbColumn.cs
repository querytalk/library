#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Represents a database column.
    /// </summary>
    [DebuggerTypeProxy(typeof(Wall.DbColumnDebuggerProxy))]
    public sealed class DbColumn : IScalar, IColumnName 
    {
        internal DB3 ColumnID { get; set; }

        internal DbNode RootSubject { get; set; }

        internal bool IsAllColumns
        {
            get
            {
                return ColumnID.ColumnZ == 0;
            }
        }

        internal bool IsAuto { get; private set; }

        private ColumnMap _columnMap;
        internal ColumnMap ColumnMap
        {
            get
            {
                if (_columnMap == null)
                {
                    if (!IsAllColumns)
                    {
                        _columnMap = DbMapping.GetColumnMap(ColumnID);
                    }
                    else
                    {
                        _columnMap = new ColumnMap(ColumnID);
                    }
                }

                return _columnMap;
            }
        }

        internal string ColumnName
        {
            get
            {
                return ColumnMap.Name.Part1;
            }
        }

        #region IColumnName

        string IColumnName.ColumnName
        {
            get
            {
                return ColumnName;
            }
        }

        #endregion

        private DbNode _parent;
        internal DbNode Parent 
        {
            get
            {
                return _parent;
            }
            private set
            {
                _parent = value;
            }
        }

        internal string OfAlias { get; set; }

        internal Func<BuildContext, BuildArgs, string> Build { get; private set; }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DbColumn class.
        /// </summary>
        /// <param name="parent">A parent.</param>
        /// <param name="columnID">A DB3 identifier of a column.</param>
        public DbColumn(DbNode parent, DB3 columnID)
        {
            _parent = parent;
            ColumnID = columnID;

            Build = (buildContext, buildArgs) =>
            {
                _parent.CheckReuseAndThrow();

                // note: 
                //   If the current object is a PivotChainer, then the previous FromChainer is not accessable 
                //   by the select statement any more
                if (buildContext.Current is PivotChainer)
                {
                    return ColumnMap.Name.Sql;
                }

                string alias = Text.Zero;

                if (buildContext.Current.QueryPart != null && buildContext.Current.QueryPart.Joiner != null)
                {
                    alias = buildContext.Current.QueryPart.Joiner.ProcessColumn(this);
                }

                if (alias != Text.ZeroAlias)
                {
                    return String.Format("{0}.{1}",
                        Filter.Delimit(alias),
                        IsAllColumns ? Text.Asterisk : ColumnMap.Name.Sql);
                }
                else
                {
                    return String.Format("{0}",
                        IsAllColumns ? Text.Asterisk : ColumnMap.Name.Sql);
                }
            };
        }

        /// <summary>
        /// Initializes a new instance of the DbColumn class.
        /// </summary>
        /// <param name="parent">A parent.</param>
        /// <param name="columnID">A DB3 identifier of a column.</param>
        /// <param name="alias">An alias of a column.</param>
        public DbColumn(DbNode parent, DB3 columnID, string alias)
            : this(parent, columnID)
        {
            OfAlias = alias;
        }

        internal DbColumn(DbNode parent, DB3 columnID, bool isAuto)
            : this(parent, columnID)
        {
            IsAuto = isAuto;
        }

        #endregion

        #region Column Processing

        private string _joinGraph;
        internal string JoinGraph
        {
            get
            {
                if (_joinGraph == null)
                {
                    List<Tuple<DB3, int>> reversed = new List<Tuple<DB3, int>>();
                    int i = 0;

                    var node = _parent;
                    while (node != null)
                    {
                        reversed.Add(Tuple.Create(node.NodeID, i++));
                        node = node.Prev;
                    }

                    _joinGraph = String.Join(Text.Dot, reversed
                        .OrderByDescending(a => a.Item2)
                        .Select(a => a.Item1.ToString())
                        .ToArray());
                }

                return _joinGraph;
            }
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return ColumnMap.Name.ToString();
        }

        #endregion

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
