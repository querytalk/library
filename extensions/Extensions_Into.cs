#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {
        /// <summary>
        /// Stores single values into the variables.
        /// </summary>
        /// <param name="prev">A predecessor object in a object chain.</param>
        /// <param name="firstVariable">The first variable in a collection of variables.</param>
        /// <param name="otherVariables">A collection of other variables.</param>
        public static IntoVarsChainer IntoVars(this IIntoVars prev, 
            string firstVariable, params string[] otherVariables)
        {
            return new IntoVarsChainer((Chainer)prev,
                Common.MergeArrays<string>(firstVariable, otherVariables));
        }

        /// <summary>
        /// Creates a new temporary table and inserts the resulting rows from the query into it.
        /// </summary>
        /// <param name="prev">A predecessor object in a object chain.</param>
        /// <param name="tempTableName">A temporary table name. Should comply with the rules for regular SQL identifiers (starting with # sign).</param>
        public static IntoTempTableChainer IntoTempTable(this IIntoTempTable prev, string tempTableName)
        {
            return new IntoTempTableChainer((Chainer)prev, tempTableName);
        }
    }
}
