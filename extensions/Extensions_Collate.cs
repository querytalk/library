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
        /// Defines the column collation.
        /// </summary>
        /// <param name="column">A column.</param>
        /// <param name="collation">A collaction name.</param>
        public static CollateChainer Collate(this string column, string collation = "DATABASE_DEFAULT")
        {
            return new CollateChainer(column, collation);
        }

        /// <summary>
        /// Defines the column collation.
        /// </summary>
        /// <param name="column">A column.</param>
        /// <param name="collation">A collaction name.</param>
        public static CollateChainer Collate(this DbColumn column, string collation = "DATABASE_DEFAULT")
        {
            return new CollateChainer(column, collation);
        }

        /// <summary>
        /// Defines the column collation.
        /// </summary>
        /// <param name="column">A column.</param>
        /// <param name="collation">A collaction name.</param>
        public static CollateChainer Collate(this Identifier column, string collation = "DATABASE_DEFAULT")
        {
            return new CollateChainer(column, collation);
        }

        /// <summary>
        /// Defines the column collation.
        /// </summary>
        /// <param name="column">A column.</param>
        /// <param name="collation">A collaction name.</param>
        public static CollateChainer Collate(this SysFn column, string collation = "DATABASE_DEFAULT")
        {
            return new CollateChainer(column, collation);
        }

        /// <summary>
        /// Defines the column collation.
        /// </summary>
        /// <param name="column">A column.</param>
        /// <param name="collation">A collaction name.</param>
        public static CollateChainer Collate(this CaseExpressionChainer column, string collation = "DATABASE_DEFAULT")
        {
            return new CollateChainer(column, collation);
        }

        /// <summary>
        /// Defines the column collation.
        /// </summary>
        /// <param name="column">A column.</param>
        /// <param name="collation">A collaction name.</param>
        public static CollateChainer Collate(this OfChainer column, string collation = "DATABASE_DEFAULT")
        {
            return new CollateChainer(column, collation);
        }

        /// <summary>
        /// Defines the column collation.
        /// </summary>
        /// <param name="column">A column.</param>
        /// <param name="collation">A collation name.</param>
        public static CollateChainer Collate(this Udf column, string collation = "DATABASE_DEFAULT")
        {
            return new CollateChainer(column, collation);
        }

        /// <summary>
        /// Defines the column collation.
        /// </summary>
        /// <param name="column">A column.</param>
        /// <param name="collation">A collaction name.</param>
        public static CollateChainer Collate(this View column, string collation = "DATABASE_DEFAULT")
        {
            return new CollateChainer(column, collation);
        }

        /// <summary>
        /// Defines the column collation.
        /// </summary>
        /// <param name="column">A column.</param>
        /// <param name="collation">A collaction name.</param>
        public static CollateChainer Collate(this Expression column, string collation = "DATABASE_DEFAULT")
        {
            return new CollateChainer(column, collation);
        }

    }
}
