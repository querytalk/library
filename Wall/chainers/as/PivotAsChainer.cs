#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class PivotAsChainer : AliasAs, ITableAlias, IQuery, IViewAllowed, 
        IWhere,
        IGroupBy,
        IOrderBy,
        ISelect
    {
        internal override string Method
        {
            get
            {
                return Text.Method.PivotAs;
            }
        }

        internal PivotAsChainer(Chainer prev, string alias)
            : base(prev, alias)
        {
            CheckNullOrEmptyAliasAndThrow(alias);
            ((PivotChainer)Prev).SetAlias(this);
        }
    }
}
