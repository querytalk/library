#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class DropTempTableChainer : EndChainer, IBegin 
    {
        internal DropTempTableChainer(Chainer prev) : base(prev)
        { }

        internal override string Method
        {
            get
            {
                return Text.Method.DropTempTable;
            }
        }

        internal DropTempTableChainer(Chainer prev, string table) 
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => table, table));

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(100)
                    .NewLine(BuildDrop(table, GetRoot(), out chainException))
                    .ToString();

                TryThrow();

                return sql;
            };
        }

        internal static string BuildDrop(string table, Designer root, out QueryTalkException exception)
        {
            exception = null;

            // check if a temp table exists - prevent dropping persistent tables
            if (root != null && !root.TempTableExists(table, true))
            {
                exception = new QueryTalkException("DropTempTableChainer.BuildDrop",
                    QueryTalkExceptionType.UnknownTempTable, String.Format("temp table = {0}", table));
                return null;
            }

            return BuildDropNoCheck(table);
        }

        internal static string BuildDropNoCheck(string table)
        {
            return Text.GenerateSql(100)
                .AppendFormat("IF OBJECT_ID(N'tempdb..{0}')", table).S()
                .Append(Text.IsNotNull).S()
                .Append(Text.DropTempTable).S().Append(table)
                .Terminate()
                .ToString();
        }
    }
}
