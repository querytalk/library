#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Linq;
using System.Text;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class FromManyChainer : EndChainer, IBegin
    {
        internal override string Method
        {
            get
            {
                return Text.Method.FromMany;
            }
        }

        internal FromManyChainer(Chainer prev, 
            TableArgument firstTable,
            TableArgument secondTable, 
            TableArgument[] otherTables)
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => firstTable, firstTable));
            CheckNullAndThrow(Arg(() => secondTable, secondTable));
            CheckNullAndThrow(Arg(() => otherTables, otherTables));

            if (otherTables.Where(table => table == null).Any())
            {
                Throw(QueryTalkExceptionType.ArgumentNull, "table = null");
            }

            Build = (buildContext, buildArgs) =>
            {
                StringBuilder sql = Text.GenerateSql(200);
                ProcessTable(buildContext, buildArgs, firstTable, sql);
                ProcessTable(buildContext, buildArgs, secondTable, sql);

                Array.ForEach(otherTables, table =>
                {
                    ProcessTable(buildContext, buildArgs, table, sql);
                });

                TryThrow(buildContext);

                return sql.ToString();
            };
        }

        private void ProcessTable(
            BuildContext buildContext,
            BuildArgs buildArgs,
            TableArgument table, 
            StringBuilder sql)
        {
            sql.AppendLine()
                .Append(Text.Select).S()
                .Append(Text.Asterisk).S()
                .Append(Text.From).S()
                .Append(table.Build(buildContext, buildArgs))
                .Terminate();

            TryThrow(table.Exception);
        }
    }
}
