#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class BeginTableColumnNullableChainer : Chainer, IStatement,
        IBeginTableColumn,
        IBeginTablePrimaryKey,
        IBeginTableUniqueKey,
        IEndTable
    {
        internal BeginTableColumnNullableChainer(Chainer prev, bool isNullable)
            : base(prev)
        {
            GetPrev<BeginTableColumnChainer>().IsNullable = isNullable;
        }
    }
}
