#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk
{
    /// <summary>
    /// Represents the testing options of a testing environment.
    /// </summary>
    public enum TestingOption : int
    {
        /// <summary>
        /// Open the testing environment window without executing the code.
        /// </summary>
        Open = 0,

        /// <summary>
        /// Open the testing environment window and execute the code (default setting).
        /// </summary>
        OpenAndExecute = 1,

        /// <summary>
        /// Ignore the .Test method. No testing environment window is shown.
        /// </summary>
        Skip = 2
    }
}
