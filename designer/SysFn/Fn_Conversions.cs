#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {

        #region Cast

        /// <summary>
        /// <para>CAST AS built-in function.</para>
        /// <para>Converts an expression of one data type to another.</para>
        /// </summary>
        /// <param name="column">A column expression.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn Cast(Column column, DataType dataType)
        {
            column = column ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                if (dataType == null)
                {
                    throw new QueryTalkException("Cast", 
                        QueryTalkExceptionType.ArgumentNull, "dataTypeDef = null", Wall.Text.Method.SysConvert);
                }

                var sql = String.Format(
                    "CAST({0} AS {1})",
                    column.Build(buildContext, buildArgs),
                    dataType.Build());
                buildContext.TryTakeException(column.Exception, "Sys.Cast");
                return sql;

            });
        }

        /// <summary>
        /// <para>CAST AS built-in function.</para>
        /// <para>Converts an expression of one data type to another.</para>
        /// </summary>
        /// <param name="argument">A scalar expression.</param>
        /// <param name="dataTypeDef">A data type definition.</param>
        public static SysFn Cast(IScalar argument, DataType dataTypeDef)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                if (dataTypeDef == null)
                {
                    throw new QueryTalkException("Cast",
                        QueryTalkExceptionType.ArgumentNull, "dataTypeDef = null", Wall.Text.Method.SysConvert);
                }

                var sql = String.Format(
                    "CAST({0} AS {1})",
                    ((Chainer)argument).Build(buildContext, buildArgs),
                    dataTypeDef.Build());
                buildContext.TryTakeException(((Chainer)argument).Exception, "Sys.Cast");
                return sql;
            });
        }
     
        #endregion

        #region Convert

        /// <summary>
        /// <para>CONVERT built-in function.</para>
        /// <para>Converts an expression of one data type to another.</para>
        /// </summary>
        /// <param name="argument">Any expression argument.</param>
        /// <param name="dataTypeDef">The target data type definition.</param>
        /// <param name="style">Is an integer expression that specifies how the CONVERT function is to translate expression. If style is NULL, NULL is returned. The range is determined by dataTypeDef.</param>
        public static SysFn Convert(FunctionArgument argument, DataType dataTypeDef, int style = 0)
        {
            var convert = new ConvertChainer(null, argument, dataTypeDef, style);
            return convert.GetSys();
        }

        #endregion

    }
}
