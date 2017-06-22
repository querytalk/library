#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {
        /// <summary>
        /// Converts a value into the Value object.
        /// </summary>
        /// <param name="argument">The value to convert.</param>
        /// <param name="parameterized">If true then the value is parameterized; if false then the value is used as literal.</param>
        public static Value Value(System.Object argument, bool parameterized = true)
        {
            return new Value(argument, parameterized ? Parameterization.Value : Parameterization.None);
        }

        /// <summary>
        /// Converts a value into the Value object.
        /// </summary>
        /// <param name="argument">The value to convert.</param>
        /// <param name="dataType">A data type of an argument.</param>
        /// <param name="parameterized">If true then the value is parameterized; if false then the value is used as literal.</param>
        public static Value Value(System.Object argument, DataType dataType, bool parameterized = true)
        {
            return new Value(argument, parameterized ? Parameterization.Value : Parameterization.None, dataType);
        }

    }
}