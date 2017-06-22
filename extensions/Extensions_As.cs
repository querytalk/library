#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the QueryTalk extension methods.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Specifies a column alias.
        /// </summary>
        /// <param name="identifier">A valid SQL identifier.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ColumnAsChainer As(this System.String identifier, string alias)
        {
            return new ColumnAsChainer(identifier, alias);
        }

        /// <summary>
        /// Specifies a column alias.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ColumnAsChainer As(this IAs prev, string alias)
        {
            return new ColumnAsChainer((Chainer)prev, alias);
        }

        /// <summary>
        /// Specifies a column alias.
        /// </summary>
        /// <param name="column">A mapped column object.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ColumnAsChainer As(this DbColumn column, string alias)
        {
            return new ColumnAsChainer(column, alias);
        }

        /// <summary>
        /// Specifies an alias of a view (subquery) used in a SELECT statement.
        /// </summary>
        /// <param name="prev">A view.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ViewAsChainer As(this IViewAs prev, string alias)
        {
            return new ViewAsChainer((Chainer)prev, alias);
        }

        #region Table Aliasing

        /// <summary>
        /// Specifies a table alias.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static FromAsChainer As(this IFromAs prev, string alias)
        {
            return new FromAsChainer((Chainer)prev, alias);
        }

        /// <summary>
        /// Specifies a table alias.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static JoinAsChainer As(this IJoinAs prev, string alias)
        {
            return new JoinAsChainer((Chainer)prev, alias);
        }

        /// <summary>
        /// Specifies a table alias.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ApplyAsChainer As(this IApplyAs prev, string alias)
        {
            return new ApplyAsChainer((Chainer)prev, alias);
        }

        /// <summary>
        /// Specifies a table alias.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static PivotAsChainer As(this IPivotAs prev, string alias)
        {
            return new PivotAsChainer((Chainer)prev, alias);
        }

        #endregion

        // for columns only
        #region Clr Data Types
        // As an argument of the extension method a CDT should only be used as is, without any implicit conversions.

        /// <summary>
        /// Specifies a column alias of a specified value.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ColumnAsChainer As(this System.Boolean value, string alias)
        {
            return new ColumnAsChainer(value, alias);
        }

        /// <summary>
        /// Specifies a column alias of a specified value.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ColumnAsChainer As(this System.Byte value, string alias)
        {
            return new ColumnAsChainer(value, alias);
        }

        /// <summary>
        /// Specifies a column alias of a specified value.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ColumnAsChainer As(this System.Byte[] value, string alias)
        {
            return new ColumnAsChainer(value, alias);
        }

        /// <summary>
        /// Specifies a column alias of a specified value.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ColumnAsChainer As(this System.DateTime value, string alias)
        {
            return new ColumnAsChainer(value, alias);
        }

        /// <summary>
        /// Specifies a column alias of a specified value.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ColumnAsChainer As(this System.DateTimeOffset value, string alias)
        {
            return new ColumnAsChainer(value, alias);
        }

        /// <summary>
        /// Specifies a column alias of a specified value.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ColumnAsChainer As(this System.Decimal value, string alias)
        {
            return new ColumnAsChainer(value, alias);
        }

        /// <summary>
        /// Specifies a column alias of a specified value.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ColumnAsChainer As(this System.Double value, string alias)
        {
            return new ColumnAsChainer(value, alias);
        }

        /// <summary>
        /// Specifies a column alias of a specified value.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ColumnAsChainer As(this System.Guid value, string alias)
        {
            return new ColumnAsChainer(value, alias);
        }

        /// <summary>
        /// Specifies a column alias of a specified value.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ColumnAsChainer As(this System.Int16 value, string alias)
        {
            return new ColumnAsChainer(value, alias);
        }

        /// <summary>
        /// Specifies a column alias of a specified value.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ColumnAsChainer As(this System.Int32 value, string alias)
        {
            return new ColumnAsChainer(value, alias);
        }

        /// <summary>
        /// Specifies a column alias of a specified value.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ColumnAsChainer As(this System.Int64 value, string alias)
        {
            return new ColumnAsChainer(value, alias);
        }

        /// <summary>
        /// Specifies a column alias of a specified value.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ColumnAsChainer As(this System.Single value, string alias)
        {
            return new ColumnAsChainer(value, alias);
        }

        /// <summary>
        /// Specifies a column alias of a specified value.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="alias">Is an alias. It is recommended that the alias follows the rules for regular SQL identifiers.</param>
        public static ColumnAsChainer As(this System.TimeSpan value, string alias)
        {
            return new ColumnAsChainer(value, alias);
        }

        #endregion

        #region Internal

        internal static FromAsChainer As(this IFromAs prev, int alias)
        {
            return new FromAsChainer((Chainer)prev, alias);
        }

        internal static JoinAsChainer As(this IJoinAs prev, int alias)
        {
            return new JoinAsChainer((Chainer)prev, alias);
        }

        #endregion

    }
}
