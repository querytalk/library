#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Diagnostics;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class ByChainer : Chainer, IJoinCond, IQuery, INonSelectView, IViewAllowed,
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
        internal override string Method
        {
            get
            {
                return Text.Method.By;
            }
        }

        internal ByChainer(Chainer prev, ByArgument[] byArguments)
            : base(prev)
        {
            CheckNullOrEmptyAndThrow(Argc(() => byArguments, byArguments));
            Array.ForEach(byArguments, a =>
                {
                    if (a == null)
                    {
                        Throw(QueryTalkExceptionType.ArgumentNull, null);
                    }

                    TryThrow(a.Exception);
                });

            Query.AddArguments(byArguments);
            GetPrev<JoinChainer>().SetJoinCorrelation(JoinCorrelation.By, byArguments);
            SkipBuild = true;
        }

    }
}
