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
        /// Retrieves multiple result sets from the specified tables.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="firstTable">The first table to select from.</param>
        /// <param name="secondTable">The second table to select from.</param>
        /// <param name="otherTables">Other tables to select from.</param>
        public static FromManyChainer FromMany(this IAny prev,
            TableArgument firstTable,
            TableArgument secondTable,
            params TableArgument[] otherTables)
        {
            return new FromManyChainer((Chainer)prev, firstTable, secondTable, otherTables);
        }
    }
}
