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
    public class CaseExpressionChainer : Chainer,
        IScalar,
        IAs
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Case;
            }
        }

        internal CaseExpressionChainer(Chainer prev)
            : base(prev)
        {
            Build = (buildContext, buildArgs) =>
                {
                    return Text.GenerateSql(200)
                        .Append(BuildCase(buildContext, buildArgs)).S()
                        .Append(Text.End)
                        .ToString();
                };
        }

        private string BuildCase(BuildContext buildContext, BuildArgs buildArgs)
        {
            var sql = new StringBuilder();
            Chainer node = GetPrev<CaseChainer>();
            while (node != this)
            {
                object append = null;
                append = node.Build(buildContext, buildArgs);
                if (append != null)
                {
                    sql.Append(append);
                    sql.S();
                }
                node = node.Next;
            }

            return sql.TrimEnd().ToString();
        }
    }
}
