#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class BeginTableColumnCheckChainer : Chainer, IStatement,
        IBeginTableColumn,
        IBeginTablePrimaryKey,
        IBeginTableUniqueKey,
        IEndTable
    {
        internal override string Method
        {
            get
            {
                return Text.Method.TableCheck;
            }
        }

        internal BeginTableColumnCheckChainer(Chainer prev, Expression expression)
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => expression, expression));
            GetPrev<BeginTableColumnChainer>().Check = expression;
        }

    }
}
