#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class ElseChainer : EndChainer, IBegin
    {
        internal ElseChainer(Chainer prev) 
            : base(prev)
        {
            Build = (buildContext, buildArgs) =>
                {
                    return Text.GenerateSql(20)
                        .NewLine(Text.End).S()
                        .NewLine(Text.ElseBegin)
                        .ToString();
                };
        }
    }
}
