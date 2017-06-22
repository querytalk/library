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
        /// Specifies a table alias of a mapped column.
        /// </summary>
        /// <param name="column">Is a mapped column object that represents a column in the SQL Server.</param>
        /// <param name="tableAlias">Is a table alias that should belong to a table where the column is mapped to.</param>
        public static OfChainer Of(this DbColumn column, string tableAlias)
        {
            return new OfChainer(null, column, tableAlias);
        }

        /// <summary>
        /// Specifies a table alias of a mapped column.
        /// </summary>
        /// <param name="column">Is a mapped column object that represents a column in the SQL Server.</param>
        /// <param name="tableAlias">Is a table alias that should belong to a table where the column is mapped to.</param>
        public static OfChainer Of(this DbColumn column, int tableAlias)
        {
            return new OfChainer(null, column, tableAlias.ToString());
        }        

    }
}
