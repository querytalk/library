#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    // base class for all join chainers (InnerJoinChainer, LeftOuterJoinChainer, RightOuterJoinChainer, FullJoinChainer, SemqJoinChainer)
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public abstract class JoinChainer : TableChainer, IQuery, INonSelectView, IViewAllowed, 
        IJoinAs,
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
        internal ByArgument[] ByArguments { get; set; }  

        private JoinCorrelation _correlation = JoinCorrelation.Auto;
        internal JoinCorrelation Correlation 
        {
            get 
            { 
                return _correlation; 
            }
        }

        internal void SetJoinCorrelation(JoinCorrelation correlation, ByArgument[] byArguments)
        {
            _correlation = correlation;
            ByArguments = byArguments;
        }

        internal JoinChainer(Chainer prev, Table table) 
            : base(prev, table)
        { }

        internal JoinChainer(Chainer prev, IOpenView table)
            : base(prev, table)
        { }

    }
}
