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
        /// Generates the OUTPUT clause.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="columns">Are specified columns in OUTPUT clause. If null, then output clause is omitted.</param>
        public static OutputChainer Output(this IOutput prev, params Column[] columns)
        {
            return new OutputChainer((Chainer)prev, columns, null);
        }

        /// <summary>
        /// Generates the OUTPUT clause.
        /// </summary>
        /// <param name="prev">A predecessor table.</param>
        /// <param name="outputSource">Is a data source (Inserted or Deleted) of the OUTPUT clause.</param>
        /// <returns></returns>
        public static OutputChainer Output(this IOutput prev, OutputSource outputSource)
        {
            return new OutputChainer((Chainer)prev, null, outputSource);
        }

        /// <summary>
        /// Generates the OUTPUT INTO clause.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="target">Specifies a table that the returned rows are inserted into instead of being returned to the caller.</param>
        public static OutputIntoChainer Into(this OutputChainer prev, TableArgument target)
        {
            return new OutputIntoChainer((Chainer)prev, target);
        }
    }
}
