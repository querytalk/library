#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    // A build argument class that contains arguments of the building process.
    internal sealed class BuildArgs
    {
        // The executable object used with inlining.
        internal Executable Executable { get; private set; }

        // A body SQL code that is designed in a testing environment and is then forced to replace
        // the original code of the compilable object in the building process.
        internal string TestBody { get; set; }

        // True if building takes place in the testing environment.
        internal bool IsTesting
        {
            get
            {
                return TestBody != null;
            }
        }

        internal BuildArgs(Executable executable)
        {
            Executable = executable;
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("BuildArgs ({0})", Executable);
        }

    }
}
