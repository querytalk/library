#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    // Represents a SQL Server built-in function or global variable.
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class SysFn : Chainer, INonPredecessor,
        IScalar,
        IAs
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Sys;
            }
        }

        internal SysFn(string sql, QueryTalkException exception = null)
            : base(null)
        {
            chainException = exception;
            Build = (buildContext, buildArgs) =>
                {
                    return sql;
                };

            DebugValue = sql;
        }

        internal SysFn(Func<BuildContext, BuildArgs, string> buildMethod, 
            QueryTalkException exception = null)
            : base(null)
        {
            chainException = exception;
            Build = buildMethod;
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return (string)DebugValue ?? Text.Free.DebuggingValueNotAvailable;
        }
    }
}
