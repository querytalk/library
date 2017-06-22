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
    internal class ProcedureDebuggerProxy
    {
        private Procedure _procedure;

        public ProcedureDebuggerProxy(Procedure procedure)
        {
            if (procedure == null)
            {
                throw new ArgumentNullException("procedure");
            }

            _procedure = procedure;
        }

        public string Sql
        {
            get
            {
                return _procedure.Sql;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public string[] Params
        {
            get
            {
                var exparams = _procedure.GetRoot().ExplicitParams;
                if (exparams.Count > 0)
                {
                    return exparams.Select(p => p.ToString()).ToArray();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
