#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class TableParamChainer : ParamChainer, ISnippetDisallowed,
        IParam,
        IAny,
        IFrom,
        ICollect
    {
        internal override string Method
        {
            get
            {
                return Text.Method.TableParam;
            }
        }

        // ctor for user-defined table type
        internal TableParamChainer(Chainer prev, string paramName, DT dt, TableArgument nameType)
            : base(prev, paramName, dt)
        {
            var root = GetRoot();
            int ordinal = root.AllParams.Count;
            Param = new Variable(ordinal, paramName, dt, nameType, IdentifierType.Param);

            if (Param.Exception != null)
            {
                Param.Exception.Extra = Text.Free.UdttNameArgumentExtra;
                TryThrow(Param.Exception);
            }

            root.TryAddParamOrThrow(Param, false);
        }

        // ctor for temp table/table variable only
        internal TableParamChainer(Chainer prev, string table)
            : base(prev, table, DT.TempTable)   // DT.TempTable type is used to avoid base checking
        {
            var root = GetRoot();
            int ordinal = root.AllParams.Count;

            DT dt;
            if (Common.TryDetectTableType(root, table, Method) == TableType.TempTable)
            {
                dt = DT.TempTable;
            }
            else
            {
                dt = DT.TableVariable;
            }

            Param = new Variable(ordinal, table, dt, new TableArgument(table), IdentifierType.Param);

            if (Param.Exception != null)
            {
                Param.Exception.Extra = Text.Free.TempTableParamExtra;
                TryThrow(Param.Exception);
            }

            root.TryAddParamOrThrow(Param, false);
        }

        // ctor for bulk tables (bulk insert)
        internal TableParamChainer(Chainer prev, TableArgument bulkTable)
            : base(prev, bulkTable.Sql, DT.BulkTable)
        {
            CheckNullAndThrow(Arg(() => bulkTable, bulkTable));
            bulkTable.TryThrow();

            var root = GetRoot();
            int ordinal = root.AllParams.Count;

            Param = new Variable(ordinal, bulkTable.Sql, DT.BulkTable, bulkTable, IdentifierType.Param);
            if (Param.Exception != null)
            {
                Param.Exception.Extra = Text.Free.TempTableParamExtra;
                TryThrow(Param.Exception);
            }

            root.TryAddParamOrThrow(Param, false);
        }

    }
}
