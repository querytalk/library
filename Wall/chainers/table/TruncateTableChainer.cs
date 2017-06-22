#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class TruncateTableChainer : EndChainer, IBegin 
    {
        internal override string Method
        {
            get
            {
                return Text.Method.TruncateTable;
            }
        }

        internal TruncateTableChainer(Chainer prev, TableArgument table) 
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => table, table));

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(50)
                    .NewLine(Text.TruncateTable).S()
                    .Append(table.Build(buildContext, buildArgs))
                    .Terminate()
                    .ToString();
            };
        }
    }
}
