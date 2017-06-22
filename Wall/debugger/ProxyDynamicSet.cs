#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Represents a HashSet collection of a dynamic values.
    /// </summary>
    public class ProxyDynamicSet : HashSet<dynamic> 
    {
        internal ProxyDynamicSet(HashSet<dynamic> set)
            : base(set)
        { }
    }
}
