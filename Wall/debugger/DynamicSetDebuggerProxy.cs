#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Linq;
using System.Diagnostics;

namespace QueryTalk.Wall
{
    internal class DynamicSetDebuggerProxy
    {
        private ProxyDynamicSet _set;

        public DynamicSetDebuggerProxy(ProxyDynamicSet set)
        {
            if (set == null)
            {
                throw new ArgumentNullException("set");
            }

            _set = set;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public dynamic[] Items
        {
            get
            {
                if (_set.Count != 0)
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
