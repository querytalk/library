#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class BeginCursorIntoVarsChainer : EndChainer, IStatement
    {
        internal override string Method
        {
            get
            {
                return Text.Method.BeginCursorWithVars;
            }
        }

        internal BeginCursorIntoVarsChainer(Chainer prev, string[] variables)
            : base(prev) 
        {
            CheckNullOrEmptyAndThrow(Argc(() => variables, variables));
            var beginCursor = GetPrev<BeginCursorChainer>();
            beginCursor.TrySetVariables(variables);
            SkipBuild = true;
        }
    }
}
