#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class CatchChainer : EndChainer, IBegin
    {
        internal CatchChainer(Chainer prev) 
            : base(prev)
        {
            ++GetRoot().TryCatchCounter;

            Build = (buildContext, buildArgs) =>
                {
                    return Text.GenerateSql(30)
                        .NewLine(Text.EndTry).S()
                        .NewLine(Text.BeginCatch).S()
                        .ToString();
                };
        }
    }
}
