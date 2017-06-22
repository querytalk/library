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
        /// Inserts the rows in a table.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is a table argument.</param>
        public static InsertChainer Insert(this IInsert prev, Table table)
        {
            return new InsertChainer((Chainer)prev, table);
        }

        /// <summary>
        /// Inserts the rows in a table.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is a table node.</param>
        public static InsertChainer Insert(this IInsert prev, DbNode table)
        {
            return new InsertChainer((Chainer)prev, table);
        }

        /// <summary>
        /// Specifies the columns to be used in the INSERT statement.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="firstColumn">The first column to insert into.</param>
        /// <param name="otherColumns">Other columns to insert into.</param>
        public static ColumnsChainer IntoColumns(this IColumns prev,
            NonSelectColumnArgument firstColumn, params NonSelectColumnArgument[] otherColumns)
        {
            return new ColumnsChainer((Chainer)prev,
                Common.MergeArrays(firstColumn, otherColumns));
        }

        /// <summary>
        /// Allows explicit values to be inserted into the identity column of a table. At any time, only one table in a session can have the IDENTITY_INSERT property set to ON. If a table already has this property set to ON, and a SET IDENTITY_INSERT ON statement is issued for another table, SQL Server returns an error message that states SET IDENTITY_INSERT is already ON.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is a table argument.</param>
        /// <param name="setOn">Specifies whether the IDENTITY_INSERT property is set to ON (TRUE) or OFF (FALSE).</param>
        public static SetIdentityInsertChainer SetIdentityInsert(this IAny prev, TableArgument table, bool setOn)
        {
            return new SetIdentityInsertChainer((Chainer)prev, table, setOn);
        }
    }
}
