#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class SemqJoinChainer : JoinChainer, IQuery, INonSelectView, IViewAllowed
    {
        internal bool IsLeftOuterJoin { get; set; }

        internal override string Keyword
        {
            get
            {
                return IsLeftOuterJoin ? Text.LeftOuterJoin : Text.InnerJoin;
            }
        }

        internal override string Method
        {
            get
            {
                return Text.Method.Join;
            }
        }

        internal SemqJoinChainer(Chainer prev, ISemantic node) 
            : base(prev, (IOpenView)node)
        { }

    }
}
