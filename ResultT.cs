#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates returning result set.
    /// </summary>
    [DebuggerTypeProxy(typeof(Wall.ResultDebuggerProxy<>)),
     DebuggerDisplay("QueryTalk.Result | ReturnValue = {ReturnValue} | {TableCount} table(s)")]
    public sealed class Result<T> : ResultSet<T>, IResult, IAsyncStatus, ICompilable
        
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Connectable _connectable;

        #region Bag

        /// <summary>
        /// The dynamic bag.
        /// </summary>
        public dynamic Bag { get; set; }

        #endregion

        #region IResult

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _executed;

        /// <summary>
        /// A flag to indicate whether the execution took place.
        /// </summary>
        public bool Executed
        {
            get
            {
                return _executed || Sql != null;   
            }
            internal set
            {
                _executed = value;
            }
        }

        /// <summary>
        /// The RETURN_VALUE of the QueryTalk procedure or stored procedure.
        /// </summary>
        public int ReturnValue { get; internal set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _tableCount = 1;    

        /// <summary>
        /// The number of tables in the result sets.
        /// </summary>
        public int TableCount
        {
            get
            {
                return _tableCount;
            }
        }

        /// <summary>
        /// The number of rows in the result set.
        /// </summary>
        public new int RowCount
        {
            get
            {
                return base.RowCount;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _affectedCount;

        /// <summary>
        /// The number of rows affected by the CRUD operation.
        /// </summary>
        public int AffectedCount
        {
            get
            {
                return _affectedCount;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _showSQL = true;  

        /// <summary>
        /// The executive SQL code.
        /// </summary>
        public string Sql
        {
            get
            {
                if (_showSQL && _connectable != null)
                {
                    return _connectable.Sql;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region IAsyncStatus

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AsyncStatus _asyncStatus = AsyncStatus.None;

        /// <summary>
        /// Status of an asynchronous operation.
        /// </summary>
        public AsyncStatus AsyncStatus
        {
            get
            {
                if (Exception != null)
                {
                    return AsyncStatus.Faulted;
                }
                else
                {
                    return _asyncStatus;
                }
            }
            internal set
            {
                _asyncStatus = value;
            }
        }

        /// <summary>
        /// The exception object if the operation has faulted.
        /// </summary>
        public QueryTalkException Exception { get; internal set; }

        #endregion

        #region ICompilable

        /// <summary>
        /// The reference of the procedure object created implicitly or explicitly by .EndProc method.
        /// </summary>
        public Procedure Procedure { get; private set; }

        #endregion

        #region Constructors

        internal Result(bool executed, int affectedCount)
            : base((IEnumerable<T>)null)
        {
            Executed = executed;
            _affectedCount = affectedCount;
            _showSQL = false;
            _tableCount = 0;
            Bag = new ExpandoObject();
        }

        internal Result(Connectable connectable)
            : base((IEnumerable<T>)null)
        {
            _connectable = connectable;
            Bag = new ExpandoObject();
        }

        internal Result(Connectable connectable, IEnumerable<T> data)
            : base(data)
        {
            _connectable = connectable;
            Bag = new ExpandoObject();
        }

        internal Result(Connectable connectable, IEnumerable<T> data, int returnValue)
            : base(data)
        {
            ReturnValue = returnValue;
            _connectable = connectable;
            Bag = new ExpandoObject();
        }

        internal Result(Connectable connectable, DataTable dataTable)
            : base(dataTable)
        {
            _connectable = connectable;
            Bag = new ExpandoObject();
        }

        internal Result(Connectable connectable, DataTable dataTable, int returnValue)
            : base(dataTable)
        {
            ReturnValue = returnValue;
            _connectable = connectable;
            Bag = new ExpandoObject();
        }

        #endregion

        #region CRUD

        // finalizes CRUD method execution by cleaning the result set and setting AffectedCount
        internal void FinalizeCrud()
        {
            _executed = true;
            _affectedCount = ReturnValue;
            ReturnValue = 0;
            _tableCount = 0;
            _showSQL = false;
            Clear();
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            if (typeof(T) == typeof(System.Object))
            {
                return String.Format("QueryTalk.Result | ReturnValue = {0} | {1} row(s)", ReturnValue, RowCount);
            }
            else
            {
                return String.Format("QueryTalk.Result | ReturnValue = {0} | {1} row(s) of {2}", ReturnValue, RowCount, typeof(T));
            }
        }

        #endregion

    }

}
