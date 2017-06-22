#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Text;

namespace QueryTalk.Wall
{
    internal static class Testing
    {
        internal static void AppendTestValue(StringBuilder builder, ParameterArgument argument)
        {
            // table-valued test argument
            if (argument.TestValue is View)
            {
                return;
            }

            // scalar test argument (is always a string)
            string value = (string)argument.TestValue;

            if (value.EqualsCS(Text.Null, false))
            {
                builder.Append(Text.Null);
            }
            else
            {
                // if CLR type is not given => original value has been passed as d.Null
                if (argument.ArgType == null)
                {
                    if (argument.DT.IsDefined())
                    {
                        builder.Append(value);
                    }
                    else
                    {
                        builder.Append(Mapping.BuildUnchecked(argument.Value));
                    }
                }
                else
                {
                    builder.Append(value);
                }
            }
        }

    }
}
