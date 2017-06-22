#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>   
    public sealed class BeginTableColumnDefaultChainer : Chainer, IStatement,
        IBeginTableColumn,
        IBeginTablePrimaryKey,
        IBeginTableUniqueKey,
        IEndTable
    {
        internal BeginTableColumnDefaultChainer(Chainer prev, Value value)
            : base(prev)
        {
            value = value ?? Designer.Null;
            var nonParameterizedValue = new Value(value.Original, Parameterization.None);
            GetPrev<BeginTableColumnChainer>().Default = nonParameterizedValue;
        }
    }
}
