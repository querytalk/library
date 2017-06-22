#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class ApplyAsChainer : AliasAs, ITableAlias, IQuery, INonSelectView, IViewAllowed,
        ISemqJoin,
        IInnerJoin,
        ILeftOuterJoin,
        IRightOuterJoin,
        IFullJoin,
        ICrossJoin,
        ICrossApply,
        IOuterApply,
        ISelect,
        IWhere,
        IGroupBy,
        IOrderBy
    {
        internal override string Method
        {
            get
            {
                return Text.Method.ApplyAs;
            }
        }

        internal ApplyAsChainer(Chainer prev, string alias)
            : base(prev, alias)
        {
            CheckNullOrEmptyAliasAndThrow(alias);
            ((TableChainer)prev).SetAliasByClient(this);    
        }
    }
}
