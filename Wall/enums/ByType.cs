#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    internal enum ByType : int
    {
        NonMappedColumn = 0,   
        MappedColumn = 1,     
        Alias = 2,             
    }
}
