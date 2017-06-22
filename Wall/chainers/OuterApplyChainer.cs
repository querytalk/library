#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class OuterApplyChainer : TableChainer, IQuery, INonSelectView, IViewAllowed, 
        IApplyAs,
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
        internal override string Keyword
        {
            get
            {
                return Text.OuterApply;
            }
        }

        internal override string Method
        {
            get
            {
                return Text.Method.OuterApply;
            }
        }

        internal OuterApplyChainer(Chainer prev, Table table) 
            : base(prev, table)
        { }

        internal OuterApplyChainer(Chainer prev, IOpenView table)
            : base(prev, table)
        { }

    }
}
