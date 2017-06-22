#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    internal enum IdentifierType : int
    {
        ColumnOrParam = 0, 
        Table = 1,          
        Param = 2,          
        SqlVariable = 3
    }
}
