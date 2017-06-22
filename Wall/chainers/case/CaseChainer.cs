#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary> 
    public sealed class CaseChainer : Chainer, IQuery, INonPredecessor,
        ICaseInput,
        ICaseWhen
    {
        internal CaseChainer(Chainer prev) 
            : base(prev)
        {
            Build = (buildContext, buildArgs) =>
                {
                    return Text.Case;
                };
        }
    }
}
