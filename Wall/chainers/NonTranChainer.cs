#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class NonTranChainer : Chainer,
        IParam,
        IAny
    {
        internal NonTranChainer(Chainer prev) 
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => prev, prev));
            prev.GetRoot().IsEmbeddedTransaction = false;
        }
    }
}
