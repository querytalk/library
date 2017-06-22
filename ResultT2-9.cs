#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Diagnostics;
using System.Dynamic;
using QueryTalk.Wall;

namespace QueryTalk
{

    #region Result<T1, T2>

    /// <summary>
    /// Represents a generic 2-table result sets.
    /// </summary>
    [DebuggerTypeProxy(typeof(Wall.ResultDebuggerProxy<,>)),
     DebuggerDisplay("QueryTalk.Result | ReturnValue = {ReturnValue} | {TableCount} table(s)")]
    public sealed class Result<T1, T2> : IResult, IAsyncStatus, ICompilable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Connectable _connectable;

        #region Tables (Storage)

        /// <summary>
        /// The first table.
        /// </summary>
        public ResultSet<T1> Table1 { get; internal set; }

        /// <summary>
        /// The second table.
        /// </summary>
        public ResultSet<T2> Table2 { get; internal set; }

        #endregion

        #region Bag

        /// <summary>
        /// The dynamic bag.
        /// </summary>
        public dynamic Bag { get; set; }

        #endregion

        #region IResult

        /// <summary>
        /// A flag to indicate whether the execution took place.
        /// </summary>
        public bool Executed
        {
            get
            {
                return Sql != null;
            }
        }

        /// <summary>
        /// The return value of the QueryTalk procedure or stored procedure.
        /// </summary>
        public int ReturnValue { get; internal set; }

        /// <summary>
        /// The number of tables in the result sets.
        /// </summary>
        public int TableCount
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// The number of rows in the result set.
        /// </summary>
        public int RowCount
        {
            get
            {
                if (Table1 != null)
                {
                    return Table1.RowCount;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The number of affected rows. (Not used in this class.)
        /// </summary>
        public int AffectedCount
        {
            get
            {
                return 0;
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
        /// The reference of the procedure object created implicitly or explicitly by .EndProc method.
        /// </summary>
        public Procedure Procedure { get; private set; }

        #endregion

        #region Constructor

        internal Result(Connectable connectable)
        {
            _connectable = connectable;
            Bag = new ExpandoObject();
        }

        #endregion

    }

    #endregion

    #region Result<T1, T2, T3>

    /// <summary>
    /// Represents a generic 3-table result sets.
    /// </summary>
    [DebuggerTypeProxy(typeof(Wall.ResultDebuggerProxy<,,>)),
     DebuggerDisplay("QueryTalk.Result | ReturnValue = {ReturnValue} | {TableCount} table(s)")]
    public sealed class Result<T1, T2, T3> : IResult, IAsyncStatus, ICompilable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Connectable _connectable;

        #region Tables (Storage)

        /// <summary>
        /// The first table.
        /// </summary>
        public ResultSet<T1> Table1 { get; internal set; }

        /// <summary>
        /// The second table.
        /// </summary>
        public ResultSet<T2> Table2 { get; internal set; }

        /// <summary>
        /// The third table.
        /// </summary>
        public ResultSet<T3> Table3 { get; internal set; }

        #endregion

        #region Bag

        /// <summary>
        /// The dynamic bag.
        /// </summary>
        public dynamic Bag { get; set; }

        #endregion

        #region IResult

        /// <summary>
        /// A flag to indicate whether the execution took place.
        /// </summary>
        public bool Executed
        {
            get
            {
                return Sql != null;
            }
        }

        /// <summary>
        /// The return value of the QueryTalk procedure or stored procedure.
        /// </summary>
        public int ReturnValue { get; internal set; }

        /// <summary>
        /// The number of tables in the result sets.
        /// </summary>
        public int TableCount
        {
            get
            {
                return 3;
            }
        }

        /// <summary>
        /// The number of rows in the result set.
        /// </summary>
        public int RowCount
        {
            get
            {
                if (Table1 != null)
                {
                    return Table1.RowCount;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The number of affected rows. (Not used in this class.)
        /// </summary>
        public int AffectedCount
        {
            get
            {
                return 0;
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
        /// The reference of the procedure object created implicitly or explicitly by .EndProc method.
        /// </summary>
        public Procedure Procedure { get; private set; }

        #endregion

        #region Constructor

        internal Result(Connectable connectable)
        {
            _connectable = connectable;
            Bag = new ExpandoObject();
        }

        #endregion

    }

    #endregion

    #region Result<T1, T2, T3, T4>

    /// <summary>
    /// Represents a generic 4-table result sets.
    /// </summary>
    [DebuggerTypeProxy(typeof(Wall.ResultDebuggerProxy<,,,>)),
     DebuggerDisplay("QueryTalk.Result | ReturnValue = {ReturnValue} | {TableCount} table(s)")]
    public sealed class Result<T1, T2, T3, T4> : IResult, IAsyncStatus, ICompilable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Connectable _connectable;

        #region Tables (Storage)

        /// <summary>
        /// The first table.
        /// </summary>
        public ResultSet<T1> Table1 { get; internal set; }

        /// <summary>
        /// The second table.
        /// </summary>
        public ResultSet<T2> Table2 { get; internal set; }

        /// <summary>
        /// The third table.
        /// </summary>
        public ResultSet<T3> Table3 { get; internal set; }

        /// <summary>
        /// The fourth table.
        /// </summary>
        public ResultSet<T4> Table4 { get; internal set; }

        #endregion

        #region Bag

        /// <summary>
        /// The dynamic bag.
        /// </summary>
        public dynamic Bag { get; set; }

        #endregion

        #region IResult

        /// <summary>
        /// A flag to indicate whether the execution took place.
        /// </summary>
        public bool Executed
        {
            get
            {
                return Sql != null;
            }
        }

        /// <summary>
        /// The return value of the QueryTalk procedure or stored procedure.
        /// </summary>
        public int ReturnValue { get; internal set; }

        /// <summary>
        /// The number of tables in the result sets.
        /// </summary>
        public int TableCount
        {
            get
            {
                return 4;
            }
        }

        /// <summary>
        /// The number of rows in the result set.
        /// </summary>
        public int RowCount
        {
            get
            {
                if (Table1 != null)
                {
                    return Table1.RowCount;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The number of affected rows. (Not used in this class.)
        /// </summary>
        public int AffectedCount
        {
            get
            {
                return 0;
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
        /// The reference of the procedure object created implicitly or explicitly by .EndProc method.
        /// </summary>
        public Procedure Procedure { get; private set; }

        #endregion

        #region Constructor

        internal Result(Connectable connectable)
        {
            _connectable = connectable;
            Bag = new ExpandoObject();
        }

        #endregion

    }

    #endregion

    #region Result<T1, T2, T3, T4, T5>

    /// <summary>
    /// Represents a generic 5-table result sets.
    /// </summary>
    [DebuggerTypeProxy(typeof(Wall.ResultDebuggerProxy<,,,,>)),
     DebuggerDisplay("QueryTalk.Result | ReturnValue = {ReturnValue} | {TableCount} table(s)")]
    public sealed class Result<T1, T2, T3, T4, T5> : IResult, IAsyncStatus, ICompilable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Connectable _connectable;

        #region Tables (Storage)

        /// <summary>
        /// The first table.
        /// </summary>
        public ResultSet<T1> Table1 { get; internal set; }

        /// <summary>
        /// The second table.
        /// </summary>
        public ResultSet<T2> Table2 { get; internal set; }

        /// <summary>
        /// The third table.
        /// </summary>
        public ResultSet<T3> Table3 { get; internal set; }

        /// <summary>
        /// The fourth table.
        /// </summary>
        public ResultSet<T4> Table4 { get; internal set; }

        /// <summary>
        /// The fifth table.
        /// </summary>
        public ResultSet<T5> Table5 { get; internal set; }

        #endregion

        #region Bag

        /// <summary>
        /// The dynamic bag.
        /// </summary>
        public dynamic Bag { get; set; }

        #endregion

        #region IResult

        /// <summary>
        /// A flag to indicate whether the execution took place.
        /// </summary>
        public bool Executed
        {
            get
            {
                return Sql != null;
            }
        }

        /// <summary>
        /// The return value of the QueryTalk procedure or stored procedure.
        /// </summary>
        public int ReturnValue { get; internal set; }

        /// <summary>
        /// The number of tables in the result sets.
        /// </summary>
        public int TableCount
        {
            get
            {
                return 5;
            }
        }

        /// <summary>
        /// The number of rows in the result set.
        /// </summary>
        public int RowCount
        {
            get
            {
                if (Table1 != null)
                {
                    return Table1.RowCount;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The number of affected rows. (Not used in this class.)
        /// </summary>
        public int AffectedCount
        {
            get
            {
                return 0;
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
        /// The reference of the procedure object created implicitly or explicitly by .EndProc method.
        /// </summary>
        public Procedure Procedure { get; private set; }

        #endregion

        #region Constructor

        internal Result(Connectable connectable)
        {
            _connectable = connectable;
            Bag = new ExpandoObject();
        }

        #endregion

    }

    #endregion

    #region Result<T1, T2, T3, T4, T5, T6>

    /// <summary>
    /// Represents a generic 6-table result sets.
    /// </summary>
    [DebuggerTypeProxy(typeof(Wall.ResultDebuggerProxy<,,,,,>)),
     DebuggerDisplay("QueryTalk.Result | ReturnValue = {ReturnValue} | {TableCount} table(s)")]
    public sealed class Result<T1, T2, T3, T4, T5, T6> : IResult, IAsyncStatus, ICompilable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Connectable _connectable;

        #region Tables (Storage)

        /// <summary>
        /// The first table.
        /// </summary>
        public ResultSet<T1> Table1 { get; internal set; }

        /// <summary>
        /// The second table.
        /// </summary>
        public ResultSet<T2> Table2 { get; internal set; }

        /// <summary>
        /// The third table.
        /// </summary>
        public ResultSet<T3> Table3 { get; internal set; }

        /// <summary>
        /// The fourth table.
        /// </summary>
        public ResultSet<T4> Table4 { get; internal set; }

        /// <summary>
        /// The fifth table.
        /// </summary>
        public ResultSet<T5> Table5 { get; internal set; }

        /// <summary>
        /// The sixth table.
        /// </summary>
        public ResultSet<T6> Table6 { get; internal set; }

        #endregion

        #region Bag

        /// <summary>
        /// The dynamic bag.
        /// </summary>
        public dynamic Bag { get; set; }

        #endregion

        #region IResult

        /// <summary>
        /// A flag to indicate whether the execution took place.
        /// </summary>
        public bool Executed
        {
            get
            {
                return Sql != null;
            }
        }

        /// <summary>
        /// The return value of the QueryTalk procedure or stored procedure.
        /// </summary>
        public int ReturnValue { get; internal set; }

        /// <summary>
        /// The number of tables in the result sets.
        /// </summary>
        public int TableCount
        {
            get
            {
                return 6;
            }
        }

        /// <summary>
        /// The number of rows in the result set.
        /// </summary>
        public int RowCount
        {
            get
            {
                if (Table1 != null)
                {
                    return Table1.RowCount;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The number of affected rows. (Not used in this class.)
        /// </summary>
        public int AffectedCount
        {
            get
            {
                return 0;
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
        /// The reference of the procedure object created implicitly or explicitly by .EndProc method.
        /// </summary>
        public Procedure Procedure { get; private set; }

        #endregion

        #region Constructor

        internal Result(Connectable connectable)
        {
            _connectable = connectable;
            Bag = new ExpandoObject();
        }

        #endregion

    }

    #endregion

    #region Result<T1, T2, T3, T4, T5, T6, T7>

    /// <summary>
    /// Represents a generic 7-table result sets.
    /// </summary>
    [DebuggerTypeProxy(typeof(Wall.ResultDebuggerProxy<,,,,,,>)),
     DebuggerDisplay("QueryTalk.Result | ReturnValue = {ReturnValue} | {TableCount} table(s)")]
    public sealed class Result<T1, T2, T3, T4, T5, T6, T7> : IResult, IAsyncStatus, ICompilable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Connectable _connectable;

        #region Tables (Storage)

        /// <summary>
        /// The first table.
        /// </summary>
        public ResultSet<T1> Table1 { get; internal set; }

        /// <summary>
        /// The second table.
        /// </summary>
        public ResultSet<T2> Table2 { get; internal set; }

        /// <summary>
        /// The third table.
        /// </summary>
        public ResultSet<T3> Table3 { get; internal set; }

        /// <summary>
        /// The fourth table.
        /// </summary>
        public ResultSet<T4> Table4 { get; internal set; }

        /// <summary>
        /// The fifth table.
        /// </summary>
        public ResultSet<T5> Table5 { get; internal set; }

        /// <summary>
        /// The sixth table.
        /// </summary>
        public ResultSet<T6> Table6 { get; internal set; }

        /// <summary>
        /// The seventh table.
        /// </summary>
        public ResultSet<T7> Table7 { get; internal set; }

        #endregion

        #region Bag

        /// <summary>
        /// The dynamic bag.
        /// </summary>
        public dynamic Bag { get; set; }

        #endregion

        #region IResult

        /// <summary>
        /// A flag to indicate whether the execution took place.
        /// </summary>
        public bool Executed
        {
            get
            {
                return Sql != null;
            }
        }

        /// <summary>
        /// The return value of the QueryTalk procedure or stored procedure.
        /// </summary>
        public int ReturnValue { get; internal set; }

        /// <summary>
        /// The number of tables in the result sets.
        /// </summary>
        public int TableCount
        {
            get
            {
                return 7;
            }
        }

        /// <summary>
        /// The number of rows in the result set.
        /// </summary>
        public int RowCount
        {
            get
            {
                if (Table1 != null)
                {
                    return Table1.RowCount;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The number of affected rows. (Not used in this class.)
        /// </summary>
        public int AffectedCount
        {
            get
            {
                return 0;
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
        /// The reference of the procedure object created implicitly or explicitly by .EndProc method.
        /// </summary>
        public Procedure Procedure { get; private set; }

        #endregion

        #region Constructor

        internal Result(Connectable connectable)
        {
            _connectable = connectable;
            Bag = new ExpandoObject();
        }

        #endregion

    }

    #endregion

    #region Result<T1, T2, T3, T4, T5, T6, T7, T8>

    /// <summary>
    /// Represents a generic 8-table result sets.
    /// </summary>
    [DebuggerTypeProxy(typeof(Wall.ResultDebuggerProxy<,,,,,,,>)),
     DebuggerDisplay("QueryTalk.Result | ReturnValue = {ReturnValue} | {TableCount} table(s)")]
    public sealed class Result<T1, T2, T3, T4, T5, T6, T7, T8> : IResult, IAsyncStatus, ICompilable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Connectable _connectable;

        #region Tables (Storage)

        /// <summary>
        /// The first table.
        /// </summary>
        public ResultSet<T1> Table1 { get; internal set; }

        /// <summary>
        /// The second table.
        /// </summary>
        public ResultSet<T2> Table2 { get; internal set; }

        /// <summary>
        /// The third table.
        /// </summary>
        public ResultSet<T3> Table3 { get; internal set; }

        /// <summary>
        /// The fourth table.
        /// </summary>
        public ResultSet<T4> Table4 { get; internal set; }

        /// <summary>
        /// The fifth table.
        /// </summary>
        public ResultSet<T5> Table5 { get; internal set; }

        /// <summary>
        /// The sixth table.
        /// </summary>
        public ResultSet<T6> Table6 { get; internal set; }

        /// <summary>
        /// The seventh table.
        /// </summary>
        public ResultSet<T7> Table7 { get; internal set; }

        /// <summary>
        /// The eighth table.
        /// </summary>
        public ResultSet<T8> Table8 { get; internal set; }

        #endregion

        #region Bag

        /// <summary>
        /// The dynamic bag.
        /// </summary>
        public dynamic Bag { get; set; }

        #endregion

        #region IResult

        /// <summary>
        /// A flag to indicate whether the execution took place.
        /// </summary>
        public bool Executed
        {
            get
            {
                return Sql != null;
            }
        }

        /// <summary>
        /// The return value of the QueryTalk procedure or stored procedure.
        /// </summary>
        public int ReturnValue { get; internal set; }

        /// <summary>
        /// The number of tables in the result sets.
        /// </summary>
        public int TableCount
        {
            get
            {
                return 8;
            }
        }

        /// <summary>
        /// The number of rows in the result set.
        /// </summary>
        public int RowCount
        {
            get
            {
                if (Table1 != null)
                {
                    return Table1.RowCount;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The number of affected rows. (Not used in this class.)
        /// </summary>
        public int AffectedCount
        {
            get
            {
                return 0;
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
        /// The reference of the procedure object created implicitly or explicitly by .EndProc method.
        /// </summary>
        public Procedure Procedure { get; private set; }

        #endregion

        #region Constructor

        internal Result(Connectable connectable)
        {
            _connectable = connectable;
            Bag = new ExpandoObject();
        }

        #endregion

    }

    #endregion

    #region Result<T1, T2, T3, T4, T5, T6, T7, T8, T9>

    /// <summary>
    /// Represents a generic 9-table result sets.
    /// </summary>
    [DebuggerTypeProxy(typeof(Wall.ResultDebuggerProxy<,,,,,,,,>)),
     DebuggerDisplay("QueryTalk.Result | ReturnValue = {ReturnValue} | {TableCount} table(s)")]
    public sealed class Result<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IResult, IAsyncStatus, ICompilable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Connectable _connectable;

        #region Tables (Storage)

        /// <summary>
        /// The first table.
        /// </summary>
        public ResultSet<T1> Table1 { get; internal set; }

        /// <summary>
        /// The second table.
        /// </summary>
        public ResultSet<T2> Table2 { get; internal set; }

        /// <summary>
        /// The third table.
        /// </summary>
        public ResultSet<T3> Table3 { get; internal set; }

        /// <summary>
        /// The fourth table.
        /// </summary>
        public ResultSet<T4> Table4 { get; internal set; }

        /// <summary>
        /// The fifth table.
        /// </summary>
        public ResultSet<T5> Table5 { get; internal set; }

        /// <summary>
        /// The sixth table.
        /// </summary>
        public ResultSet<T6> Table6 { get; internal set; }

        /// <summary>
        /// The seventh table.
        /// </summary>
        public ResultSet<T7> Table7 { get; internal set; }

        /// <summary>
        /// The eighth table.
        /// </summary>
        public ResultSet<T8> Table8 { get; internal set; }

        /// <summary>
        /// The ninth table.
        /// </summary>
        public ResultSet<T9> Table9 { get; internal set; }

        #endregion

        #region Bag

        /// <summary>
        /// The dynamic bag.
        /// </summary>
        public dynamic Bag { get; set; }

        #endregion

        #region IResult

        /// <summary>
        /// A flag to indicate whether the execution took place.
        /// </summary>
        public bool Executed
        {
            get
            {
                return Sql != null;
            }
        }

        /// <summary>
        /// The return value of the QueryTalk procedure or stored procedure.
        /// </summary>
        public int ReturnValue { get; internal set; }

        /// <summary>
        /// The number of tables in the result sets.
        /// </summary>
        public int TableCount
        {
            get
            {
                return 9;
            }
        }

        /// <summary>
        /// The number of rows in the result set.
        /// </summary>
        public int RowCount
        {
            get
            {
                if (Table1 != null)
                {
                    return Table1.RowCount;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// The number of affected rows. (Not used in this class.)
        /// </summary>
        public int AffectedCount
        {
            get
            {
                return 0;
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
        /// The reference of the procedure object created implicitly or explicitly by .EndProc method.
        /// </summary>
        public Procedure Procedure { get; private set; }

        #endregion

        #region Constructor

        internal Result(Connectable connectable)
        {
            _connectable = connectable;
            Bag = new ExpandoObject();
        }

        #endregion

    }

    #endregion

}
