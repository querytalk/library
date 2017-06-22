#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        #region Scalar

        /// <summary>
        /// Declares a scalar parameter.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="parameter">Is the name of the parameter. The name must begin with the at sign (@) and should follows the rules for regular SQL identifiers.</param>
        /// <param name="dataType">Is a data type definition of a parameter. All data type definitions are available in the Sys class.</param>
        public static DataParamChainer Param(this IParam prev, string parameter, DataType dataType)
        {
            return new DataParamChainer((Chainer)prev, parameter, dataType);
        }

        /// <summary>
        /// Declares a scalar parameter.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="parameter">Is the name of a parameter. The name must begin with the at sign (@) and should follows the rules for regular SQL identifiers.</param>
        /// <param name="userDefinedType">Is a user-defined scalar type identifier.</param>
        public static DataParamChainer Param(this IParam prev, string parameter, TableArgument userDefinedType)
        {
            return new DataParamChainer((Chainer)prev, parameter, DT.Udt, userDefinedType);
        }

        /// <summary>
        /// Declares an inline parameter used for SQL code inlining rather than passing data values. A procedure with an inline parameter is recompiled on every execution.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="parameter">Is the name of a parameter. The name must begin with the at sign (@) and should follows the rules for regular SQL identifiers.</param>
        /// <param name="inlineType">Is an inline type of a parameter.</param>
        public static InlineParamChainer Param(this IParam prev, string parameter, Designer.Inliner inlineType)
        {
            return new InlineParamChainer((Chainer)prev, parameter, inlineType.ToDT());
        }

        /// <summary>
        /// Declares a scalar parameter using the CLR-SQL mapping.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="parameter">Is the name of the parameter. The name must begin with the at sign (@) and should follows the rules for regular SQL identifiers.</param>
        /// <typeparam name="T">A CLR type mapped to a certain SQL type.</typeparam>
        public static DataParamChainer Param<T>(this IParam prev, string parameter)
        {
            return new DataParamChainer((Chainer)prev, parameter, typeof(T));
        }

        #endregion

        #region Table

        /// <summary>
        /// Declares a table-valued parameter.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="parameter">Is the name of a table variable parameter. The name must begin with the at sign (@) and should follow the rules for regular SQL identifiers.</param>
        /// <param name="userDefinedTableType">Is a user-defined table type identifier.</param>
        public static TableParamChainer TableParam(this IParam prev, string parameter, TableArgument userDefinedTableType)
        {
            return new TableParamChainer((Chainer)prev, parameter, DT.Udtt, userDefinedTableType);
        }

        /// <summary>
        /// Declares a table-valued parameter.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">
        /// Is the name of a temporary table or table variable. The name must begin with the number sign (#) for temporary table or with the at sign (@) for table variable.
        /// </param>
        public static TableParamChainer TableParam(this IParam prev, string table)
        {
            return new TableParamChainer((Chainer)prev, table);
        }

        /// <summary>
        /// Declares a table-valued parameter.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="bulkTable">Specifies a bulk table identifier.</param>
        public static TableParamChainer TableParam(this IParam prev, Identifier bulkTable)
        {
            return new TableParamChainer((Chainer)prev, bulkTable);
        }

        #endregion

        #region Other

        /// <summary>
        /// Specifies that the parameter is optional.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static ParamOptionalChainer Optional(this IParamOptional prev)
        {
            return new ParamOptionalChainer((Chainer)prev, null);
        }

        /// <summary>
        /// Specifies that the parameter is optional.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="defaultValue">Is a default value assigned to the parameter in case that a parameter argument is not passed. All optional parameters have to be declared at the end of the parameter declaration.</param>
        public static ParamOptionalChainer Optional(this IParamOptional prev, ParameterArgument defaultValue)
        {
            return new ParamOptionalChainer((Chainer)prev, defaultValue);
        }

        /// <summary>
        /// Specifies that the parameter cannot receive a null argument.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static ParamNotNullChainer NotNull(this IParamNotNull prev)
        {
            return new ParamNotNullChainer((Chainer)prev);
        }

        /// <summary>
        /// <para>Specifies that the procedure returns the parameter value back to the caller.</para>
        /// <para>Note that a parameter argument has to be a Value object.</para>
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static ParamOutputChainer Output(this IParamOutput prev)
        {
            return new ParamOutputChainer((Chainer)prev);
        }

        #endregion

        #region CRUD (internal)

        // add columns with selector definition
        internal static DataParamChainer ParamNodeColumns(this IParam prev, DB3 nodeID, ColumnSelector selector, string prefix = null)
        {
            return DataParamChainer.AddNodeColumns((Chainer)prev, nodeID, selector, prefix ?? Text.OPrefix);
        }

        // add columns by custom selection
        internal static DataParamChainer ParamNodeColumns(this IParam prev, ColumnMap[] columns, string prefix = null)
        {
            return DataParamChainer.AddNodeColumns((Chainer)prev, columns, prefix ?? Text.OPrefix);
        }

        #endregion

    }
}
