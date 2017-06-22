#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {
        /// <summary>
        /// Defines the inlining parameter type used in dynamic parameterization of a batch.
        /// </summary>
        public enum Inliner : int
        {
            /// <summary>
            /// <para>Represents a table.</para>
            /// </summary>
            Table = 0,

            /// <summary>
            /// <para>Represents a column in .Select method.</para>
            /// </summary>
            Column = 1,

            /// <summary>
            /// <para>Represents an expression.</para>
            /// </summary>
            Expression = 2,

            /// <summary>
            /// <para>Represents a SQL code in .Inject method.</para>
            /// </summary>
            Sql = 3,

            /// <summary>
            /// <para>Represents a procedure.</para>
            /// </summary>
            Procedure = 4,

            /// <summary>
            /// <para>Represents a snippet.</para>
            /// </summary>
            Snippet = 5,

            /// <summary>
            /// <para>Represents a stored procedure.</para>
            /// </summary>
            StoredProcedure = 6,
        }
 
    }
}