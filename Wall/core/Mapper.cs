#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    // chainable wrapper around the SQL code used in SEMQ
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class Mapper : Compilable, 
        IExecutable,
        IGo,
        IConnectBy
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Mapper;
            }
        }

        internal void SetSql(string sql)
        {
            Sql = sql;
        }

        internal Mapper(Designer root)
            : base(root, ObjectType.Mapper)
        {
            compiled = true;     
            Sql = root.Name;

            Build = (buildContext, buildArgs) =>
            {
                return Sql;
            };
        }

    }
}
