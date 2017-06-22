#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {
        /// <summary>
        /// Sets a procedure argument as an output argument.
        /// </summary>
        /// <param name="argument">
        /// Is an output argument. 
        /// </param>
        public static Value Output(this Value argument)
        {
            if (argument.IsNullReference())
            {
                throw new QueryTalkException("Sys.Output", QueryTalkExceptionType.NullReferenceOutput,
                    "output variable = null", QueryTalk.Wall.Text.Method.Pass);
            }

            argument.IsOutput = true;
            return argument;  
        }

        /// <summary>
        /// Sets a procedure argument as an input argument (default).
        /// </summary>
        /// <param name="argument">
        /// Is an output argument. 
        /// </param>
        public static Value Input(this Value argument)
        {
            if (!argument.IsNullReference())
            {
                argument.IsOutput = false;
                return argument;
            }
            else
            {
                return null;
            }
        }
    }
}
