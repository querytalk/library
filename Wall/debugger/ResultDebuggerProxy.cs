#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Linq;
using System.Data;
using System.Diagnostics;

namespace QueryTalk.Wall
{

    #region Result

    internal class ResultDebuggerProxy
    {
        private Result _result;

        public ResultDebuggerProxy(Result result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }
            _result = result;
        }

        public ProxyDynamicSet Table1
        {
            get
            {
                if (_result.Table1 != null)
                {
                    return new ProxyDynamicSet(_result.Table1);
                }
                else
                {
                    return null;
                }
            }
        }

        public ProxyDynamicSet Table2
        {
            get
            {
                if (_result.Table2 != null)
                {
                    return new ProxyDynamicSet(_result.Table2);
                }
                else
                {
                    return null;
                }
            }
        }

        public ProxyDynamicSet Table3
        {
            get
            {
                if (_result.Table3 != null)
                {
                    return new ProxyDynamicSet(_result.Table3);
                }
                else
                {
                    return null;
                }
            }
        }

        public ProxyDynamicSet Table4
        {
            get
            {
                if (_result.Table4 != null)
                {
                    return new ProxyDynamicSet(_result.Table4);
                }
                else
                {
                    return null;
                }
            }
        }

        public ProxyDynamicSet Table5
        {
            get
            {
                if (_result.Table5 != null)
                {
                    return new ProxyDynamicSet(_result.Table5);
                }
                else
                {
                    return null;
                }
            }
        }

        public ProxyDynamicSet Table6
        {
            get
            {
                if (_result.Table6 != null)
                {
                    return new ProxyDynamicSet(_result.Table6);
                }
                else
                {
                    return null;
                }
            }
        }

        public ProxyDynamicSet Table7
        {
            get
            {
                if (_result.Table7 != null)
                {
                    return new ProxyDynamicSet(_result.Table7);
                }
                else
                {
                    return null;
                }
            }
        }

        public ProxyDynamicSet Table8
        {
            get
            {
                if (_result.Table8 != null)
                {
                    return new ProxyDynamicSet(_result.Table8);
                }
                else
                {
                    return null;
                }
            }
        }

        public ProxyDynamicSet Table9
        {
            get
            {
                if (_result.Table9 != null)
                {
                    return new ProxyDynamicSet(_result.Table9);
                }
                else
                {
                    return null;
                }
            }
        }
    }

    #endregion

    #region Result<T>

    internal class ResultDebuggerProxy<T>
        
    {
        private Result<T> _result;

        public ResultDebuggerProxy(Result<T> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            _result = result;
        }

        public DataTable DataTable
        {
            get
            {
                return _result.DataTable;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                if (_result.Count != 0)
                {
                    return _result.ToArray();
                }
                else
                {
                    return new T[] { };
                }
            }
        }
    }

    #endregion

    #region Result<T1,T2>

    internal class ResultDebuggerProxy<T1,T2>
    {
        private Result<T1,T2> _result;

        public ResultDebuggerProxy(Result<T1,T2> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            _result = result;
        }

        public ResultSet<T1> Table1 { get { return _result.Table1; } }
        public ResultSet<T2> Table2 { get { return _result.Table2; } }
    }

    #endregion

    #region Result<T1,T2,T3>

    internal class ResultDebuggerProxy<T1, T2, T3>
    {
        private Result<T1, T2, T3> _result;

        public ResultDebuggerProxy(Result<T1, T2, T3> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            _result = result;
        }

        public ResultSet<T1> Table1 { get { return _result.Table1; } }
        public ResultSet<T2> Table2 { get { return _result.Table2; } }
        public ResultSet<T3> Table3 { get { return _result.Table3; } }
    }

    #endregion

    #region Result<T1,T2,T3,T4>

    internal class ResultDebuggerProxy<T1, T2, T3, T4>
    {
        private Result<T1, T2, T3, T4> _result;

        public ResultDebuggerProxy(Result<T1, T2, T3, T4> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            _result = result;
        }

        public ResultSet<T1> Table1 { get { return _result.Table1; } }
        public ResultSet<T2> Table2 { get { return _result.Table2; } }
        public ResultSet<T3> Table3 { get { return _result.Table3; } }
        public ResultSet<T4> Table4 { get { return _result.Table4; } }
    }

    #endregion

    #region Result<T1,T2,T3,T4,T5>

    internal class ResultDebuggerProxy<T1, T2, T3, T4, T5>
    {
        private Result<T1, T2, T3, T4, T5> _result;

        public ResultDebuggerProxy(Result<T1, T2, T3, T4, T5> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            _result = result;
        }

        public ResultSet<T1> Table1 { get { return _result.Table1; } }
        public ResultSet<T2> Table2 { get { return _result.Table2; } }
        public ResultSet<T3> Table3 { get { return _result.Table3; } }
        public ResultSet<T4> Table4 { get { return _result.Table4; } }
        public ResultSet<T5> Table5 { get { return _result.Table5; } }
    }

    #endregion

    #region Result<T1,T2,T3,T4,T5,T6>

    internal class ResultDebuggerProxy<T1, T2, T3, T4, T5, T6>
    {
        private Result<T1, T2, T3, T4, T5, T6> _result;

        public ResultDebuggerProxy(Result<T1, T2, T3, T4, T5, T6> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            _result = result;
        }

        public ResultSet<T1> Table1 { get { return _result.Table1; } }
        public ResultSet<T2> Table2 { get { return _result.Table2; } }
        public ResultSet<T3> Table3 { get { return _result.Table3; } }
        public ResultSet<T4> Table4 { get { return _result.Table4; } }
        public ResultSet<T5> Table5 { get { return _result.Table5; } }
        public ResultSet<T6> Table6 { get { return _result.Table6; } }
    }

    #endregion

    #region Result<T1,T2,T3,T4,T5,T6,T7>

    internal class ResultDebuggerProxy<T1, T2, T3, T4, T5, T6, T7>
    {
        private Result<T1, T2, T3, T4, T5, T6, T7> _result;

        public ResultDebuggerProxy(Result<T1, T2, T3, T4, T5, T6, T7> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            _result = result;
        }

        public ResultSet<T1> Table1 { get { return _result.Table1; } }
        public ResultSet<T2> Table2 { get { return _result.Table2; } }
        public ResultSet<T3> Table3 { get { return _result.Table3; } }
        public ResultSet<T4> Table4 { get { return _result.Table4; } }
        public ResultSet<T5> Table5 { get { return _result.Table5; } }
        public ResultSet<T6> Table6 { get { return _result.Table6; } }
        public ResultSet<T7> Table7 { get { return _result.Table7; } }
    }

    #endregion

    #region Result<T1,T2,T3,T4,T5,T6,T7,T8>

    internal class ResultDebuggerProxy<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        private Result<T1, T2, T3, T4, T5, T6, T7, T8> _result;

        public ResultDebuggerProxy(Result<T1, T2, T3, T4, T5, T6, T7, T8> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            _result = result;
        }

        public ResultSet<T1> Table1 { get { return _result.Table1; } }
        public ResultSet<T2> Table2 { get { return _result.Table2; } }
        public ResultSet<T3> Table3 { get { return _result.Table3; } }
        public ResultSet<T4> Table4 { get { return _result.Table4; } }
        public ResultSet<T5> Table5 { get { return _result.Table5; } }
        public ResultSet<T6> Table6 { get { return _result.Table6; } }
        public ResultSet<T7> Table7 { get { return _result.Table7; } }
        public ResultSet<T8> Table8 { get { return _result.Table8; } }
    }

    #endregion

    #region Result<T1,T2,T3,T4,T5,T6,T7,T8,T9>

    internal class ResultDebuggerProxy<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    {
        private Result<T1, T2, T3, T4, T5, T6, T7, T8, T9> _result;

        public ResultDebuggerProxy(Result<T1, T2, T3, T4, T5, T6, T7, T8, T9> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            _result = result;
        }

        public ResultSet<T1> Table1 { get { return _result.Table1; } }
        public ResultSet<T2> Table2 { get { return _result.Table2; } }
        public ResultSet<T3> Table3 { get { return _result.Table3; } }
        public ResultSet<T4> Table4 { get { return _result.Table4; } }
        public ResultSet<T5> Table5 { get { return _result.Table5; } }
        public ResultSet<T6> Table6 { get { return _result.Table6; } }
        public ResultSet<T7> Table7 { get { return _result.Table7; } }
        public ResultSet<T8> Table8 { get { return _result.Table8; } }
        public ResultSet<T9> Table9 { get { return _result.Table9; } }
    }

    #endregion

}
