#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class UnionChainer : Chainer, IQuery, IViewAllowed,
        IAny,
        IFrom,
        ICollect
    {
        internal override string Keyword
        {
            get
            {
                return chainKeyword;
            }
        }

        internal enum UnionType { Union, UnionAll, Except, Intersect }

        internal UnionChainer(Chainer prev, UnionType unionType) 
            : base(prev)
        {
            Query.SetUnion(this);
            chainKeyword = GetType().ToString();

            Build = (buildContext, buildArgs) =>
                {
                    switch (unionType)
                    {
                        case UnionType.UnionAll:
                            chainKeyword = Text.UnionAll;
                            chainMethod = Text.Method.UnionAll;
                            break;
                        case UnionType.Except:
                            chainKeyword = Text.Except;
                            chainMethod = Text.Method.Except;
                            break;
                        case UnionType.Intersect:
                            chainKeyword = Text.Intersect;
                            chainMethod = Text.Method.Intersect;
                            break;
                        default:
                            chainKeyword = Text.Union;
                            chainMethod = Text.Method.Union;
                            break;
                    }

                    return Text.GenerateSql(15)
                        .NewLine(chainKeyword)
                        .ToString();
                };
        }
    }
}
