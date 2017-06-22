#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public class TopChainer : EndChainer, IQuery, IOpenView, IViewAllowed,
        IUnion,
        IIntoVars,
        IIntoTempTable,
        IInsert,
        IColumns,
        IEndView
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Top;
            }
        }

        internal TopChainer(Chainer prev, long top, bool isWithTies)
            : base(prev)
        {
            prev.SetTop(top, isWithTies);
        }

        internal TopChainer(Chainer prev, double top, bool isWithTies)
            : base(prev)
        {
            prev.SetTop(top, isWithTies);
        }

        internal TopChainer(Chainer prev, string topVariable, bool isWithTies)
            : base(prev)
        {
            var variable = GetRoot().TryGetVariable(topVariable, out chainException);

            // check inliner: disallowed
            if (variable.IsInliner())
            {
                chainException = new QueryTalkException("Ctop..ctor", QueryTalkExceptionType.InvalidInliner,
                    String.Format("inliner = {0}", topVariable));
            }

            TryThrow();
            GetPrev<SelectChainer>().SetTop(variable, isWithTies);
        }

        internal TopChainer(Chainer prev, Nullable<long> top, bool isWithTies, bool overloader)
            : base(prev)
        {
            if (top != null)
            {
                prev.SetTop((long)top, isWithTies);
            }
            else
            {
                SkipBuild = true;
            }
        }

        internal TopChainer(ISemantic prev, Nullable<long> top, bool isWithTies, int overloader)
            : base(((ISelect)prev.Translate(new SemqContext(((DbNode)prev).Root), null)).Select())
        {
            if (top != null)
            {
                Prev.SetTop((long)top, isWithTies);
            }
            else
            {
                SkipBuild = true;
            }
        }

        internal TopChainer(ISemantic prev, double top, bool isWithTies)
            : base(((ISelect)prev.Translate(new SemqContext(((DbNode)prev).Root), null)).Select())
        {
            Prev.SetTop(top, isWithTies);
        }

    }
}
