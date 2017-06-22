#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates all public extension methods of the QueryTalk library.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Specifies a list of arguments which are to be passed to the procedure. The parameter values have to be passed in the order of parameters as declared in the procedure.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="arguments">Are the arguments to pass.</param>
        public static PassChainer Pass(this IPass prev, params ParameterArgument[] arguments)
        {
            return new PassChainer((Chainer)prev, arguments);
        }

        /// <summary>
        /// Specifies a list of arguments which are to be passed to the batch. The parameter values have to be passed in the order of parameters as declared in the batch.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="arguments">Are the arguments to pass.</param>
        public static PassChainer Pass(this IAny prev, params ParameterArgument[] arguments)
        {
            var proc = ((EndChainer)prev).EndProc();
            return new PassChainer(proc, arguments);
        }

        /// <summary>
        /// Specifies a list of parameter values which are to be passed to the procedure or SQL batch. The parameter values have to be passed in the order of parameters as declared in the procedure or SQL batch.
        /// </summary>
        /// <param name="execArgument">Is the identifier of a stored procedure or SQL batch code.</param>
        /// <param name="arguments">Are the arguments to pass.</param>
        public static ExecArgument Pass(this string execArgument, params ParameterArgument[] arguments)
        {
            return new ExecArgument(execArgument, arguments);
        }

    }
}
