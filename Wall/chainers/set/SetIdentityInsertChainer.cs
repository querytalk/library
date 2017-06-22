#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class SetIdentityInsertChainer : EndChainer, IBegin
    {
        internal override string Keyword
        {
            get
            {
                return Text.SetIdentityInsert;
            }
        }

        internal override string Method
        {
            get
            {
                return Text.Method.SetIdentityInsert;
            }
        }

        internal SetIdentityInsertChainer(Chainer prev, TableArgument table, bool setOn) 
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => table, table));

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(70)
                    .NewLine(Text.SetIdentityInsert).S()
                    .Append(table.Build(buildContext, buildArgs)).S()
                    .Append((bool)setOn ? Text.On : Text.Off)
                    .Terminate()
                    .ToString();

                TryThrow(buildContext);

                return sql;
            };
        }
    }
}
