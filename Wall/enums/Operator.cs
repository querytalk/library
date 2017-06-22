#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    // expression operators
    internal enum Operator : int
    {
        None = 0,              
        Not = 1,                
        And = 2,                 
        AndNot = 3,
        Or = 4,
        OrNot = 5,
        IsNull = 6,
        IsNotNull = 7,
        Equal = 8,
        NotEqual = 9,
        Like = 10,
        NotLike = 11,
        GreaterThan = 12,
        GreaterOrEqualThan = 13,
        NotGreaterThan = 14,
        LessThan = 15,
        LessOrEqualThan = 16,
        NotLessThan = 17,
        Between = 18,
        NotBetween = 19,
        In = 20,
        NotIn = 21,
        Any = 22,
        Some = 23,
        All = 24,
        Exists = 25,
        NotExists = 26,
        Plus = 100,
        Minus = 101,
        MultiplyBy = 102,
        DivideBy = 103,
        Modulo = 104,
        AndBitwise = 105,
        OrBitwise = 106,
        ExclusiveOrBitwise = 107,
        BitwiseNot = 108,
    }
    
}
