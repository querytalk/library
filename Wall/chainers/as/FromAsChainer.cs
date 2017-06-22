#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class FromAsChainer : AliasAs, ITableAlias, IQuery, INonSelectView, IViewAllowed, 
        IFrom,
        ISemqJoin,
        ISelect,
        IInnerJoin,
        ILeftOuterJoin,
        IRightOuterJoin,
        IFullJoin,
        ICrossJoin,
        ICrossApply,
        IOuterApply,
        IPivot,
        IWhere,
        IDelete,
        IGroupBy,
        IOrderBy
    {
        internal override string Method
        {
            get
            {
                return Text.Method.FromAs;
            }
        }

        internal FromAsChainer(Chainer prev, string alias)
            : base(prev, alias)
        {
            CheckNullOrEmptyAliasAndThrow(alias);
            ((TableChainer)prev).SetAliasByClient(this);     
        }

        internal FromAsChainer(Chainer prev, int alias)
            : base(prev, alias)
        {
            ((TableChainer)prev).SetAlias(this);
        }
    }
}
