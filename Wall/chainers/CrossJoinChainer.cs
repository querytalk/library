#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class CrossJoinChainer : TableChainer, IQuery, INonSelectView, IViewAllowed, 
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
        IOrderBy,
        ISelect,
        IDelete
    {
        internal override string Keyword
        {
            get
            {
                return Text.CrossJoin;
            }
        }

        internal override string Method
        {
            get
            {
                return Text.Method.CrossJoin;
            }
        }

        internal CrossJoinChainer(Chainer prev, Table table) 
            : base(prev, table)
        { }

        internal CrossJoinChainer(Chainer prev, IOpenView table)
            : base(prev, table)
        { }
    }
}
