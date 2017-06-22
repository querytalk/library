#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class OutputIntoChainer : EndChainer, IQuery,
        IAny
    {
        internal override string Method 
        { 
            get 
            {
                return Text.Method.OutputInto;
            } 
        }

        internal OutputIntoChainer(Chainer prev, TableArgument target)
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => target, target));
            ((IOutput)prev.Prev).OutputTarget = target;
        }
    }
}
