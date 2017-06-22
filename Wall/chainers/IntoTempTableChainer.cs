#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public class IntoTempTableChainer : EndChainer, IQuery
    {
        internal override string Method
        {
            get
            {
                return Text.Method.IntoTempTable;
            }
        }

        internal IntoTempTableChainer(Chainer prev, string table) 
            : base(prev)
        {
            Query.Clause.IntoTempTable = this;

            CheckNullAndThrow(Arg(() => table, table));

            if (Common.CheckIdentifier(table) != IdentifierValidity.TempTable)
            {
                Throw(QueryTalkExceptionType.InvalidTempTableName, ArgVal(() => table, table));
            }

            GetRoot().TryAddTempTable(table);

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(30)
                    .NewLine(Text.Into).S()
                    .Append(table)
                    .ToString();
            };
        }
    }
}
