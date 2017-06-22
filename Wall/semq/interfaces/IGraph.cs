#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    internal interface IGraph
    {
        IGraph Prev { get; }

        IGraph Next { get; set; }

        IGraph Root { get; }

        DbNode Subject { get; }

        Action<SemqContext, IGraph> Translate { get; set; }
    }
}
