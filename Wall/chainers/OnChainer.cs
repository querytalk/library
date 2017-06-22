#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class OnChainer : ConditionChainer, IJoinCond, IQuery, INonSelectView, IViewAllowed,
        IOnAnd,
        IOnOr,
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
                return Text.OnWithIndent;
            }
        }

        internal override string Method
        {
            get
            {
                return Text.Method.On;
            }
        }

        internal OnChainer(Chainer prev, Expression expression) 
            : base(prev, expression)
        {
            GetPrev<JoinChainer>().SetJoinCorrelation(JoinCorrelation.On, null);
        }

        internal OnChainer(Chainer prev, ScalarArgument argument1, ScalarArgument argument2, bool equality) 
            : base(prev, argument1, argument2, equality)
        {
            chainMethod = equality ? Text.Method.On : Text.Method.OnNot;
            GetPrev<JoinChainer>().SetJoinCorrelation(JoinCorrelation.On, null);
        }
    }
}
