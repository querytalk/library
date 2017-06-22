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
    public sealed class CreateTempTableIndexChainer : Chainer, IBegin,
        ITempTableIndexColumns
    {
        internal override string Method
        {
            get
            {
                return Text.Method.CreateTempTableIndex;
            }
        }

        internal CreateTempTableIndexChainer(Chainer prev, string table, string index, Designer.IndexType indexType) 
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
                var sql = Text.GenerateSql(200)
                    .NewLine(Text.Create).S();

                switch (indexType)
                {
                    case Designer.IndexType.Nonclustered: sql.Append(Text.Nonclustered).S(); break;
                    case Designer.IndexType.Clustered: sql.Append(Text.Clustered).S(); break;
                    case Designer.IndexType.UniqueNonclustered: sql.Append(Text.NonclusteredUnique).S(); break;
                    case Designer.IndexType.UniqueClustered: sql.Append(Text.ClusteredUnique).S(); break;
                }

                sql.Append(Text.Index).S()
                    .Append(index).S()
                    .Append(Text.On).S()
                    .Append(table);

                return sql.ToString();
            };        
        }
    }
}
