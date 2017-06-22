#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    internal class ArgumentDebuggerProxy
    {
        private Argument _arg;

        public ArgumentDebuggerProxy(Argument arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException("arg");
            }

            _arg = arg;
        }

        public object Value
        {
            get
            {
                return _arg.DebugValue;
            }
        }

    }
}
