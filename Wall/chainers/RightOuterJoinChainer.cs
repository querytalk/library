#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>  
    public sealed class RightOuterJoinChainer : JoinChainer, IQuery, INonSelectView, IViewAllowed
    {
        internal override string Keyword
        {
            get
            {
                return Text.RightOuterJoin;
            }
        }

        internal override string Method
        {
            get
            {
                return Text.Method.RightOuterJoin;
            }
        }

        internal RightOuterJoinChainer(Chainer prev, Table table) 
            : base(prev, table)
        { }

        internal RightOuterJoinChainer(Chainer prev, IOpenView table)
            : base(prev, table)
        { }
    }
}
