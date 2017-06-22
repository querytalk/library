#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class EndTryCatchChainer : EndChainer, IBegin 
    {
        internal EndTryCatchChainer(Chainer prev) 
            : base(prev)
        {
            // decrease the counter by 2 including the catch block
            GetRoot().TryCatchCounter -= 2;  

            Build = (buildContext, buildArgs) =>
                {
                    return Text.GenerateSql(15)
                        .NewLine(Text.EndCatch)
                        .Terminate()
                        .ToString();
                };
        }
    }
}
