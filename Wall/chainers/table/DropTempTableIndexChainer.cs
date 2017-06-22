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
    public sealed class DropTempTableIndexChainer : EndChainer, IBegin
    {
        internal override string Method
        {
            get
            {
                return Text.Method.DropTempTableIndex;
            }
        }

        internal DropTempTableIndexChainer(Chainer prev, string table, string index) 
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => table, table));
            CheckNullAndThrow(Arg(() => index, index));

            if (!GetRoot().TempTableExists(table))
            {
                Throw(QueryTalkExceptionType.UnknownTempTable, 
                    String.Format("temp table = {0}{1}   index = {2}", table, Environment.NewLine, index));
            } 

            if (Common.CheckIdentifier(index) != IdentifierValidity.RegularIdentifier)
            {
                Throw(QueryTalkExceptionType.InvalidIndexName, ArgVal(() => index, index));    
            }

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(70)
                    .NewLine(Text.DropIndex).S()
                    .Append(index).S()
                    .Append(Text.On).S()
                    .Append(table)
                    .Terminate()
                    .ToString();
            };        
        }
    }
}
