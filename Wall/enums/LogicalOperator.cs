#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// Specifies the logical operator.
    /// </summary>
    public enum LogicalOperator : int
    {
        /// <summary>
        /// The logical operator is not given.
        /// </summary>
        None = 0,

        /// <summary>
        /// AND logical operator.
        /// </summary>
        And = 1,

        /// <summary>
        /// OR logical operator.
        /// </summary>
        Or = 2,
    }
}
