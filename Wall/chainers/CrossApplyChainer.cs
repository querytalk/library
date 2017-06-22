#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class CrossApplyChainer : TableChainer, IQuery, INonSelectView, IViewAllowed, 
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
                return Text.CrossApply;
            } 
        }

        internal override string Method 
        { 
            get 
            {
                return Text.Method.CrossApply;
            } 
        }

        internal CrossApplyChainer(Chainer prev, Table table) 
            : base(prev, table)
        { }

        internal CrossApplyChainer(Chainer prev, IOpenView table)
            : base(prev, table)
        { }

    }
}
