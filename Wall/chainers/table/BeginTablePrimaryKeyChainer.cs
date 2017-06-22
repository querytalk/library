#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class BeginTablePrimaryKeyChainer : Chainer, IStatement,
        IBeginTableUniqueKey,
        IEndTable
    {
        internal override string Method
        {
            get
            {
                return Text.Method.TablePrimaryKey;
            }
        }

        internal BeginTablePrimaryKeyChainer(Chainer prev, OrderedColumnArgument[] columns, bool nonclustered = false)
            : base(prev)
        {
            CheckNullOrEmptyAndThrow(Argc(() => columns, columns));

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(70)
                    .NewLineIndent(Text.Comma)
                    .Append(nonclustered ? Text.PrimaryKeyNonclustered : Text.PrimaryKey).S()
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
