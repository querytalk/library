#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    internal sealed class InternalRoot : Designer, IFrom
    {
        internal bool IsMapper { get; private set; }

        internal InternalRoot()
        { }

        internal InternalRoot(bool isEmbeddedTransaction)
        {
            IsEmbeddedTransaction = isEmbeddedTransaction;
        }

        internal InternalRoot(DbNode node)
        {
            IsMapper = true;
            Name = node.Name;
            IsEmbeddedTransaction = false; 
            Node = node;
        }
    }
}