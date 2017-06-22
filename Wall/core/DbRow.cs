#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Diagnostics;

namespace QueryTalk.Wall
{
    // base class for data db classes
    /// <summary>
    /// Represents a data class or a row of the selectable SQL objects (tables, views, table-valued functions). Note that the purpose of the DbRow object is to store data and not to design a SQL code.
    /// </summary>
    public class DbRow : IRow, IConnectable
    {

        #region IRow

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        long IRow.RowID { get; set; }

        #endregion

        #region IConnectable

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ConnectionKey IConnectable.ConnectionKey
        {
            get
            {
                if (Node != null)
                {
                    return Node.Mapper.GetRoot().ConnectionKey;
                }
                else
                {
                    return null;
                }
            }
        }

        void IConnectable.SetConnectionKey(ConnectionKey connectionKey)
        {
            if (Node == null)
            {
                TrySetNode();
            }
            Node.Mapper.GetRoot().ConnectionKey = connectionKey;
        }

        void IConnectable.ResetConnectionKey()
        {
            if (Node != null)
            {
                Node.Mapper.GetRoot().ConnectionKey = null;
            }
        }

        #endregion

        #region Node

        /// <summary>
        /// A DB3 identifier of this row.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected internal DB3 NodeID { get; protected set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private NodeMap _nodeMap;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private NodeMap NodeMap
        {
            get
            {
                if (_nodeMap == null)
                {
                    _nodeMap = DbMapping.GetNodeMap(NodeID);
                }
                return _nodeMap;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DbNode _node;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal DbNode Node
        {
            get
            {
                return _node;
            }
        }

        internal DbNode TrySetNode()
        {
            if (_node == null)
            {
                CheckLoadableAndThrow(Text.Method.ConnectBy);
                _node = ((INode)this).Node;    
                _node.Row = this;
            }

            // allow reusability
            _node.CanBeReused();  
            _node.ChangeIndex(0);
            _node.Mapper.GetRoot().ClearForReuse();
            return _node;
        }

        internal DbTable<T> TrySetNode<T>() 
            where T : DbRow
        {
            if (_node == null)
            {
                CheckLoadableAndThrow(Text.Method.With);
                _node = ((INode<T>)this).Node;   
                _node.Row = this;
            }

            // allow reusability
            _node.CanBeReused(); 
            _node.ChangeIndex(0);
            _node.Mapper.GetRoot().ClearForReuse();

            return (DbTable<T>)_node;
        }

        internal void CheckLoadableAndThrow(string method)
        {
            if (GetStatus().IsNew())
            {
                throw new QueryTalkException("DbRow", QueryTalkExceptionType.NewRowException,
                    String.Format("row = {0}{1}   status = {2}", 
                        NodeID.GetNodeName(), Environment.NewLine, GetStatus()),
                        method);
            }
        }

        #endregion

        #region OnModify

        // collection of original values (set on modify)
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ConcurrentDictionary<int, object> _originalValues;

        // collection of columns set by user
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HashSet<int> _setColumns;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal HashSet<int> SetColumns
        {
            get
            {
                if (_setColumns == null)
                {
                    _setColumns = new HashSet<int>();
                }

                return _setColumns;
            }
        }

        /// <summary>
        /// A method that is called from the mapper assembly when the property value is modified.
        /// </summary>
        /// <param name="columnZ">The column index.</param>
        /// <param name="value">The original value of the property.</param>
        protected internal void OnModify(int columnZ, object value)
        {
            _status = DbRowStatus.Modified;

            if (_originalValues == null)
            {
                _originalValues = new ConcurrentDictionary<int, object>();
            }

            _originalValues.TryAdd(columnZ, value);
        }

        /// <summary>
        /// A method that is called from the mapper assembly when the property value is set.
        /// </summary>
        /// <param name="columnZ">The column index.</param>
        protected internal void OnSet(int columnZ)
        {
            SetColumns.Add(columnZ);
        }

        #endregion

        #region GetOriginalValues

        internal object[] GetOriginalValues()
        {
            List<object> values = new List<object>();
            var currentValues = PropertyAccessor.GetValues(this);

            foreach (var column in NodeMap.SortedColumns)
            {
                object value;
                var columnZ = column.ID.ColumnZ;
                if (_originalValues != null && _originalValues.TryGetValue(columnZ, out value))
                {
                    values.Add(value);
                }
                else
                {
                    values.Add(currentValues[columnZ - 1]);
                }
            }

            return values.ToArray();
        }

        internal object[] GetOriginalRKValues()
        {
            List<object> values = new List<object>();
            var currentValues = PropertyAccessor.GetValues(this);

            foreach (var column in NodeMap.SortedRKColumns)
            {
                object value;
                var columnZ = column.ID.ColumnZ;
                if (_originalValues != null && _originalValues.TryGetValue(columnZ, out value))
                {
                    values.Add(value);
                }
                else
                {
                    values.Add(currentValues[columnZ - 1]);
                }
            }

            return values.ToArray();
        }

        private void ResetOriginalValues()
        {
            if (_originalValues != null)
            {
                _originalValues.Clear();
            }
        }

        internal ColumnMap[] GetUpdatableColumns(bool forceMirroring)
        {
            if (!forceMirroring)
            {
                return NodeMap.Columns
                    .Where(a => a.ColumnType == ColumnType.Regular)
                    .OrderBy(a => a.ID.ColumnZ)
                    .ToArray();
            }

            if (_originalValues != null)
            {
                return NodeMap.Columns
                    .Where(a => _originalValues.Select(b => b.Key).Contains(a.ID.ColumnZ)
                        && a.ColumnType == ColumnType.Regular)
                    .OrderBy(a => a.ID.ColumnZ)
                    .ToArray();
            }
            else
            {
                // Loaded, Modified:
                if (_status.IsUpdatable())
                {
                    return null;
                }
                // Deleted, New: 
                else
                {
                    return NodeMap.Columns
                        .Where(a => a.ColumnType == ColumnType.Regular)  
                        .OrderBy(a => a.ID.ColumnZ)
                        .ToArray();
                }
            }
        }

        #endregion

        #region GetCurrentRKValues

        // gets the current RK values (including the modifications)
        internal object[] GetCurrentRKValues()
        {
            List<object> values = new List<object>();
            var currentValues = PropertyAccessor.GetValues(this);

            foreach (var column in NodeMap.SortedRKColumns)
            {
                var columnZ = column.ID.ColumnZ;
                values.Add(currentValues[columnZ - 1]);
            }

            return values.ToArray();
        }

        #endregion

        #region Rowversion

        // returns true if DbRow contains a rowversion column 
        // & it is not part of the row key (RK)
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal bool HasNonRKRowversion
        {
            get
            {
                var map = NodeMap;

                if (!map.HasRowversion)
                {
                    return false;
                }

                if (map.RowversionColumn.IsRK)
                {
                    return false;
                }

                return true;
            }
        }

        // anticipated:
        //   Row has a rowversion column.
        internal object GetRowversionValue()
        {
            var columnZ = NodeMap.RowversionColumn.ID.ColumnZ;
            return PropertyAccessor.GetValues(this)[columnZ - 1];
        }

        internal object GetOriginalRowversionValue()
        {
            if (NodeMap.RowversionColumn == null)
            {
                return null;
            }

            object value;
            var columnZ = NodeMap.RowversionColumn.ID.ColumnZ;

            if (_originalValues != null && _originalValues.TryGetValue(columnZ, out value))
            {
                return value;
            }
            else
            {
                return PropertyAccessor.GetValues(this)[columnZ - 1];
            }
        }

        #endregion

        #region Status

        private DbRowStatus _status = DbRowStatus.New;

        /// <summary>
        /// Returns the status of the row object.
        /// </summary>
        public DbRowStatus GetStatus()
        {
            return _status;
        }

        /// <summary>
        /// True if this instance can modify.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected internal bool CanModify
        {
            get
            {
                return _status == DbRowStatus.Loaded
                    || _status == DbRowStatus.Modified;
            }
        }

        internal void SetStatus(DbRowStatus status)
        {
            _status = status;

            if (status == DbRowStatus.Loaded || status == DbRowStatus.Faulted)
            {
                ResetOriginalValues(); 
            }
        }

        internal void SetLoadedByLoader()
        {
            _status = DbRowStatus.Loaded;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the DbRow class.
        /// </summary>
        public DbRow()
        { }

        #endregion

        #region Static

        internal static void ThrowInvalidDbRowException(Type type)
        {
            throw new QueryTalkException("DbRow", QueryTalkExceptionType.InvalidDbRow,
                String.Format("data class = {0}", type.Name));
        }

        #endregion

    }

}
