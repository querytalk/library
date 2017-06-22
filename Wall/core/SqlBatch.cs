#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QueryTalk.Wall
{
    // Represents a stored procedure or SQL batch.
    internal sealed class SqlBatch : Compilable, 
        IStoredProc, IExecutable,
        IGo,
        IConnectBy
    {
        internal override string Method
        {
            get
            {
                return Text.Method.QtSql;
            }
        }

        private List<string> _batchParameters;
        internal List<string> BatchParameters
        {
            get
            {
                if (_batchParameters == null)
                {
                    _batchParameters = new List<string>();
                }
                return _batchParameters;
            }
        }

        internal SqlBatch(Chainer prev, ExecArgument procOrBatch)
            : base(prev, procOrBatch.CompilableType)
        {
            CheckNullAndThrow(Arg(() => procOrBatch, procOrBatch));
            TryThrow();

            var root = GetRoot();
            if (root.Name == null)
            {
                root.Name = base.CompilableType == ObjectType.StoredProc ?
                    Text.Free.StoredProcRootName : Text.Free.SqlBatchRootName;
            }
            
            if (base.CompilableType == ObjectType.SqlBatch && !procOrBatch.Arguments.IsEmpty())
            {
                _batchParameters = ParseBatch((string)procOrBatch.Original, out chainException);
                TryThrow();
            }

            compiled = true;

            Build = (buildContext, buildArgs) =>
                {
                    var sql = procOrBatch.Build(buildContext, buildArgs);

                    if (base.CompilableType == ObjectType.StoredProc)
                    {
                        root.Name = sql;
                    }

                    if (Sql == null)
                    {
                        Sql = sql;
                    }

                    return Sql;
                };
        }

        private static List<string> ParseBatch(string batch, out QueryTalkException exception)
        {
            exception = null;
            var prms = new List<string>();
            Regex reg = new Regex(@"(?<=[^'])(@[a-zA-Z0-9_]+)(?<=[^'])");
            MatchCollection matches = reg.Matches(batch);

            foreach (Match match in matches)
            {
                var hit = match.Value;

                if (!Variable.CheckNameFormat(hit, out exception))
                {
                    return null;
                }

                if (!prms.Contains(hit))
                {
                    prms.Add(hit);
                }
            }

            return prms;
        }

    }
}
