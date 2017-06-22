#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// Provides an interface for using the OUTPUT clause.
    /// </summary>
    public interface IOutput
    {
        /// <summary>
        /// The output columns.
        /// </summary>
        Column[] OutputColumns { get; set; }

        /// <summary>
        /// The output target table.
        /// </summary>
        TableArgument OutputTarget { get; set; }
    }
}
