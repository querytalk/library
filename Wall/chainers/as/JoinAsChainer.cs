#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class JoinAsChainer : AliasAs, ITableAlias, IQuery, INonSelectView, IViewAllowed, 
        IOn,
        ISemqJoin,
        IInnerJoin,
        ILeftOuterJoin,
        IRightOuterJoin,
        IFullJoin,
        ICrossJoin,
        ICrossApply,
        IOuterApply,
        IWhere,
        IGroupBy,
        IOrderBy,
        ISelect,
        IDelete
    {
        internal override string Method
        {
            get
            {
                return Text.Method.JoinAs;
            }
        }

        internal JoinAsChainer(Chainer prev, string alias)
            : base(prev, alias)
        {
            CheckNullOrEmptyAliasAndThrow(alias);
            ((TableChainer)prev).SetAliasByClient(this);     
        }

        internal JoinAsChainer(Chainer prev, int index)
            : base(prev, index)
        {
            ((TableChainer)prev).SetAlias(this);
        }
    }
}
