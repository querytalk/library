#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        #region Procedure, DbProcedure, CPass

        /// <summary>
        /// Executes the QueryTalk procedure.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="procedure">Is a QueryTalk procedure with or without the passed parameters.</param>
        /// <param name="returnValueToVariable">Is a name of the variable where the RETURN VALUE returned by the stored procedure will be stored.</param>
        public static ExecChainer Exec(this IAny prev, IExecutable procedure, string returnValueToVariable = null)
        {
            return new ExecChainer((Chainer)prev, procedure, returnValueToVariable);
        }

        #endregion

        #region Stored procedure or SQL batch (string representation)

        /// <summary>
        /// Executes the stored procedure or SQL batch.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="procOrBatch">Is a stored procedure identifier, SQL batch code, or inliner of a stored procedure.</param>
        /// <param name="returnValueToVariable">Is a name of the variable where the RETURN VALUE returned by the stored procedure will be stored.</param>
        public static ExecChainer Exec(this IAny prev, ExecArgument procOrBatch, string returnValueToVariable = null) 
        {
            if (procOrBatch == null)
            {
                throw new QueryTalkException("Exec", QueryTalkExceptionType.ArgumentNull, "procOrBatch = null", Text.Method.Exec);
            }
            return ExecChainer.Create((Chainer)prev, procOrBatch, returnValueToVariable, procOrBatch.Arguments);
        }

        #endregion

    }
}
