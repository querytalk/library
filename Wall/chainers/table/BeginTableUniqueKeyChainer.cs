#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class BeginTableUniqueKeyChainer : Chainer, IStatement,
        IBeginTableUniqueKey,
        IEndTable
    {
        internal override string Method
        {
            get
            {
                return Text.Method.TableUniqueKey;
            }
        }

        internal BeginTableUniqueKeyChainer(Chainer prev, OrderedColumnArgument[] columns)
            : base(prev)
        {
            // check column
            CheckNullOrEmptyAndThrow(Argc(() => columns, columns));

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(70)
                    .NewLineIndent(Text.Comma)
                        .Append(Text.Unique).S()
                        .EncloseLeft()
                        .Append(OrderedColumnArgument.Concatenate(columns, buildContext, buildArgs))
                        .EncloseRight()
                        .ToString();

                TryThrow(buildContext);

                return sql;
            };
        }

    }
}
