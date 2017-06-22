#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public class IntoVarsChainer : EndChainer, IQuery 
    {
        internal override string Method
        {
            get
            {
                return Text.Method.IntoVars;
            }
        }

        internal IntoVarsChainer(Chainer prev, string[] variables) 
            : base(prev)
        {
            CheckNullOrEmptyAndThrow(Argc(() => variables, variables));
            GetPrev<ISelectable>().Variables = variables;
        }
    }
}
