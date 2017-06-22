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
    public sealed class EndTableChainer : EndChainer, IStatement
    {
        internal override string Method
        {
            get
            {
                return Text.Method.End;
            }
        }

        internal EndTableChainer(Chainer prev) 
            : base(prev)
        {
            var beginTable = prev.GetPrev<BeginTableChainer>();
            if (!beginTable.HasColumns)
            {
                Throw(QueryTalkExceptionType.InvalidTableDesign,
                    String.Format("table = {0}", beginTable.TableName));
            }

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(100)
                    .NewLine(Text.RightBracket)
                    .Terminate();

                if (beginTable.IsFill)
                {
                    BeginTableChainer.Fill(sql, beginTable.TableName, beginTable.DataView);
                }

                return sql.ToString();
            };
        }
    }
}
