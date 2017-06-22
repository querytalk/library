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
    internal class SetDebuggerProxy<T>
        
    {
        private ResultSet<T> _set;

        public SetDebuggerProxy(ResultSet<T> set)
        {
            if (set == null)
            {
                throw new ArgumentNullException("set");
            }

            _set = set;
        }

        public DataTable DataTable
        {
            get
            {
                return _set.DataTable;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                if (_set.RowCount != 0)
                {
                    return _set.ToArray();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
