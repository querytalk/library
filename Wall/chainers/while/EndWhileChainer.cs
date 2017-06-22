#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public class EndWhileChainer : EndChainer, IBegin
    {
        internal EndWhileChainer(Chainer prev) 
            : base(prev)
        {
            --GetRoot().WhileCounter;

            Build = (buildContext, buildArgs) =>
                {
                    return Text.GenerateSql(7)
                        .NewLine(Text.End)
                        .Terminate()
                        .ToString();
                };
        }
    }
}
