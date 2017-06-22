#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    // QueryTalkExceptionType annotation class
    internal sealed class ExceptionAttribute : Attribute
    {
        internal string Message { get; private set; }

        internal string Tip { get; private set; }

        internal ExceptionAttribute(string message, string tip)
        {
            Message = message;
            Tip = tip;
        }
    }
}
