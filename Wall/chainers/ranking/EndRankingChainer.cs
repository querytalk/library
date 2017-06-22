#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Text;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class EndRankingChainer : Chainer, IQuery
    {
        internal override string Method
        {
            get
            {
                return Text.Method.End;
            }
        }

        internal EndRankingChainer(Chainer prev) 
            : base(prev)
        {
            Build = (buildContext, buildArgs) =>
            {
                StringBuilder sql = new StringBuilder();
                Chainer node = GetPrev<RankingChainer>();

                while (node != this)
                {
                    var append = node.Build(buildContext, buildArgs);
                    TryThrow(buildContext);
                    if (append != null)
                    {
                        sql.Append(append);
                    }
                    node = node.Next;
                }

                return Text.GenerateSql(200)
                    .Append(sql.TrimEnd())
                    .Append(Text.RightBracket)
                    .ToString();
            };
        }
    }
}
