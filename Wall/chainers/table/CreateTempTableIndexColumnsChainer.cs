#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class CreateTempTableIndexColumnsChainer : EndChainer, IStatement 
    {
        internal override string Method
        {
            get
            {
                return Text.Method.CreateTempTableIndex;
            }
        }

        internal CreateTempTableIndexColumnsChainer(Chainer prev, OrderedColumnArgument[] columns) 
            : base(prev)
        {
            CheckNullOrEmptyAndThrow(Argc(() => columns, columns));

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(50)
                    .EncloseLeft().S()
                    .NewLineIndent(OrderedColumnArgument.Concatenate(columns, buildContext, buildArgs))
                    .NewLine(Text.RightBracket)
                    .Terminate()
                    .ToString();

                TryThrow(buildContext);

                return sql;
            };
        }
    }
}
