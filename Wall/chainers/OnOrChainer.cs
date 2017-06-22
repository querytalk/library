#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class OnOrChainer : ConditionChainer, IJoinCond, IQuery, INonSelectView, IViewAllowed, 
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
        IUnion
    {
        internal override string Keyword
        {
            get
            {
                return Text.GenerateSql(10).Indent().Append(Text.Or).ToString();
            }
        }

        internal override string Method
        {
            get
            {
                return Text.Method.Or;
            }
        }

        internal OnOrChainer(Chainer prev, Expression expression) 
            : base(prev, expression)
        { }

        internal OnOrChainer(Chainer prev, ScalarArgument argument1, ScalarArgument argument2, bool equality) 
            : base(prev, argument1, argument2, equality)
        {
            chainMethod = equality ? Text.Method.Or : Text.Method.OrNot;
        }
    }
}
