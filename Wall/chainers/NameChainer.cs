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
    public sealed class NameChainer : Chainer, IViewAllowed, 
        INonTran,
        IParam,
        IAny,
        IFrom,
        ICollect
    {
        internal override string Method
        {
            get
            {
                return Text.Method.NameAs;
            }
        }

        internal NameChainer(Chainer prev, 
            Designer.IsolationLevel embeddedTransactionIsolationLevel = Designer.IsolationLevel.Default)
            : base(prev)
        {
            // check root reuse
            var root = prev.GetRoot();
            if (root.IsUsed)
            {
                if (root.Node != null && root.Node.IsUsed) 
                {
                    throw new QueryTalkException("NameChainer.ctor",
                        QueryTalkExceptionType.RootReuseDisallowed, String.Format("root = {0}", root.Name));
                }

                root.ClearForReuse();
            }

            root.SetAsUsed();
            root.EmbeddedTransactionIsolationLevel = embeddedTransactionIsolationLevel;
        }

    }
}
