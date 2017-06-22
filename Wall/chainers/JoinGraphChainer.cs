#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    // class that performs node graph joining
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class JoinGraphChainer : Chainer, IQuery,

        ISemqJoin,
        IFromAs,
        IInnerJoin,
        ILeftOuterJoin,
        IRightOuterJoin,
        IFullJoin,
        ICrossJoin,
        ICrossApply,
        IOuterApply,
        IPivot,
        IWhere,
        IGroupBy,
        IOrderBy,
        ISelect,
        IDelete
    {
        internal JoinGraphChainer(ISemantic prev)
            : base(Translate.Join(((DbNode)prev).Root))
        { }
    }
}