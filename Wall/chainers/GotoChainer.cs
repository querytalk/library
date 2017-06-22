#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class GotoChainer : EndChainer, IBegin 
    {
        internal override string Method 
        { 
            get 
            {
                return Text.Method.Goto;
            } 
        }

        internal GotoChainer(Chainer prev, string label) 
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => label, label));
            
            Build = (buildContext, buildArgs) =>
                {
                    return Text.GenerateSql(20)
                        .NewLine(Text.Goto).S()
                        .Append(label)
                        .Terminate()
                        .ToString();
                };
        }
    }
}
