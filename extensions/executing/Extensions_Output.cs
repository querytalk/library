#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates all public extension methods of the QueryTalk library.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Specifies an output variable whose value can be modified by the execution.
        /// </summary>
        /// <param name="variable">Is a variable specified to return the value to the caller.</param>
        public static OutputVar Output(this string variable)
        {
            return new OutputVar(variable);
        }
    }
}
