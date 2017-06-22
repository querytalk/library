#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class ViewAsChainer : ColumnAsChainer, IQuery
    {
        internal override string Method
        {
            get
            {
                return Text.Method.ViewAs;
            }
        }

        internal ViewAsChainer(Chainer prev, string alias) 
            : base(prev, alias)
        {
            if (!CheckNull(Arg(() => prev, prev)))
            {
                chainException.Extra = Text.Free.ChainObjectNullExtra;
                return;
            }

            if (!CheckNullOrEmptyAlias(alias))
            {
                return;
            }

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(200)
                    .EncloseLeft()
                    .Append(((View)prev).Build(buildContext, buildArgs))
                    .EncloseRight()
                    .Append(Text._As_)
                    .Append(Filter.Delimit(alias))
                    .ToString();
            }; 
        }
    }
}
