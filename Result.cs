#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates dynamically created result sets.
    /// </summary>
    [DebuggerTypeProxy(typeof(Wall.ResultDebuggerProxy)),
     DebuggerDisplay("QueryTalk.Result | ReturnValue = {ReturnValue} | {TableCount} table(s)")]
    public sealed class Result : ResultSet<dynamic>, IResult, IAsyncStatus
    {

        #region Internal

        internal ResultSet<dynamic> GetTable(int index)
        {
            var name = Common.BuildTableName(index);

            try
            {
                if (index < 10)
                {
                    return (ResultSet<dynamic>)typeof(Result).GetProperty(name).GetValue(this, null);
                }
                else
                {
                    if (OtherTables != null)
                    {
                        return (ResultSet<dynamic>)((IDictionary<string, object>)OtherTables)[name];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                throw new QueryTalkException("Result.GetTable", QueryTalkExceptionType.InvalidResultSet,
                    String.Format("table = {0}", name));
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Connectable _connectable;

        #endregion

        #region Tables (Storage)

        /// <summary>
        /// The first table.
        /// </summary>
        public ResultSet<dynamic> Table1
        {
            get
            {
                return (ResultSet<dynamic>)this;
            }
        }

        /// <summary>
        /// The second table.
        /// </summary>
        public ResultSet<dynamic> Table2 { get; internal set; }

        /// <summary>
        /// The third table.
        /// </summary>
        public ResultSet<dynamic> Table3 { get; internal set; }

        /// <summary>
        /// The fourth table.
        /// </summary>
        public ResultSet<dynamic> Table4 { get; internal set; }

        /// <summary>
        /// The fifth table.
        /// </summary>
        public ResultSet<dynamic> Table5 { get; internal set; }

        /// <summary>
        /// The sixth table.
        /// </summary>
        public ResultSet<dynamic> Table6 { get; internal set; }

        /// <summary>
        /// The seventh table.
        /// </summary>
        public ResultSet<dynamic> Table7 { get; internal set; }

        /// <summary>
        /// The eight table.
        /// </summary>
        public ResultSet<dynamic> Table8 { get; internal set; }

        /// <summary>
        /// The ninth table.
        /// </summary>
        public ResultSet<dynamic> Table9 { get; internal set; }

        /// <summary>
        /// Other tables in the result sets (in case of more than 9 tables). 
        /// </summary>
        public dynamic OtherTables { get; internal set; }

        #endregion

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
                return _executed || Sql != null;    // to support CRUD
            }
            set
            {
                _executed = value;
            }
        }

        /// <summary>
        /// The RETURN_VALUE of the QueryTalk procedure or stored procedure.
        /// </summary>
        public int ReturnValue { get; internal set; }

        /// <summary>
        /// The number of tables in the result sets.
        /// </summary>
        public int TableCount { get; internal set; }

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

        /// <summary>
        /// The executive SQL code.
        /// </summary>
        public string Sql
        {
            get
            {
                if (_connectable != null)
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
        /// The reference of the compiled procedure object created implicitly or explicitly by .EndProc method.
        /// </summary>
        public Procedure Procedure
        {
            get
            {
                return _connectable.ToProc();
            }
        }

        #endregion

        #region Constructors

        internal Result(bool executed, int affectedCount)
            : base((IEnumerable<dynamic>)null)
        {
            Executed = executed;
            _affectedCount = affectedCount;
            Bag = new ExpandoObject();
        }

        internal Result(Connectable connectable, IEnumerable<dynamic> list)
            : base(list)
        {
            _connectable = connectable;
            Bag = new ExpandoObject();
        }

        internal Result(Connectable connectable)
            : this(connectable, (IEnumerable<dynamic>)null)
        { }

        internal Result(Connectable connectable, QueryTalkException ex)
            : this(connectable, (IEnumerable<dynamic>)null)
        {
            Exception = ex;
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            if (TableCount > 9)
            {
                return String.Format("QueryTalk.Result | ReturnValue = {0} | There are more than 9 tables. Check OtherTables property in the RawView.", ReturnValue);
            }
            else
            {
                return String.Format("QueryTalk.Result | ReturnValue = {0} | {1} table(s)", ReturnValue, TableCount);
            }
        }

        #endregion

    }

}
