#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    internal enum Parameterization : int
    {
        // Value remains the literal.
        None = 0,

        // Value is converted into the parameter with the given value.
        Value  = 1,   

        // Value is converted into the parameter without the given value. Its value has to be explicitly passed using .Pass method.
        Param = 2
    }
}
