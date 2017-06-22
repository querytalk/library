#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class DataParamChainer : ParamChainer, ISnippetDisallowed,
        IParamOptional,
        IParamNotNull,
        IParamOutput,
        IParam,
        IAny,
        IFrom,
        ICollect
    {
        internal override string Method 
        { 
            get 
            { 
                return Text.Method.Param;
            } 
        }

        internal DataParamChainer(Chainer prev, string paramName, DataType dbt)
            : base(prev, paramName, dbt.DT)
        {
            var root = GetRoot();
            int ordinal = root.AllParams.Count;
            Param = new Variable(ordinal, paramName, dbt, IdentifierType.Param);
            GetRoot().TryAddParamOrThrow(Param, false);
        }

        internal DataParamChainer(Chainer prev, string paramName, DT dt, TableArgument udt)
            : base(prev, paramName, dt)
        {
            var root = GetRoot();
            int ordinal = root.AllParams.Count;
            Param = new Variable(ordinal, paramName, dt, udt, IdentifierType.Param);
            GetRoot().TryAddParamOrThrow(Param, false);
        }

        internal DataParamChainer(Chainer prev, string paramName, Type type)
            : base(prev, paramName, DT.None) 
        {
            var root = GetRoot();
            int ordinal = root.AllParams.Count;
            Param = new Variable(ordinal, paramName, type, IdentifierType.Param);
            Param.Exception.TryThrow(Text.Method.Param);
            GetRoot().TryAddParamOrThrow(Param, false);
        }

        #region CRUD (internal)

        internal static DataParamChainer AddNodeColumns(Chainer prev, ColumnMap[] columns, string prefix)
        {
            int i = 1;
            DataParamChainer cparam = null;
            foreach (var column in columns)
            {
                if (cparam == null)
                {
                    cparam = ((IParam)prev).Param(prefix.AsParam(i), column.DataType);
                }
                else
                {
                    cparam = cparam.Param(prefix.AsParam(i), column.DataType);
                }

                ++i;
            }

            if (cparam == null)
            {
                throw new QueryTalkException("DataParam.AddNodeColumns", QueryTalkExceptionType.ParamNotFoundInnerException, 
                    String.Format("prefix = {0}", prefix));
            }

            return cparam;
        }

        internal static DataParamChainer AddNodeColumns(Chainer prev, DB3 nodeID, ColumnSelector selector, string prefix)
        {
            var node = DbMapping.GetNodeMap(nodeID);     
            ColumnMap[] columns;

            if (selector == ColumnSelector.All)
            {
                if (node.HasRowversion)
                {
                    return ((IParam)prev).Param(prefix.AsParam(1), node.RowversionColumn.DataType);
                }

                columns = node.SortedColumns;
            }
            else if (selector == ColumnSelector.RK)
            {
                columns = node.SortedRKColumns;
            }
            else if (selector == ColumnSelector.InsertableWithIdentity)
            {
                columns = node.GetInsertableColumns(true);
            }
            else
            {
                columns = node.GetInsertableColumns(false);
            }

            return AddNodeColumns(prev, columns, prefix);
        }

        #endregion
    }
}
