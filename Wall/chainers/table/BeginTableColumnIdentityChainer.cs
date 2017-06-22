#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class BeginTableColumnIdentityChainer : Chainer, IStatement,
        IBeginTableColumn,
        IBeginTablePrimaryKey,
        IBeginTableUniqueKey,
        IEndTable
    {
        internal override string Method
        {
            get
            {
                return Text.Method.TablePrimaryKey;
            }
        }

        internal BeginTableColumnIdentityChainer(Chainer prev, long seed = 1, long increment = 1)
            : base(prev)
        {
            if (increment == 0)
            {
                Throw(QueryTalkExceptionType.InvalidIdentityIncrement, "increment = 0");
            }

            GetPrev<BeginTableColumnChainer>().Identity = new TableIdentity(seed, increment);
            GetPrev<BeginTableColumnChainer>().IsNullable = false;  // set NOT NULL
        }

    }
}
