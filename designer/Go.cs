#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Reflection;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {

        /// <summary>
        /// Executes the multiple SQL batches in a single connection. The batches should be separated by the GO instruction. 
        /// </summary>
        /// <param name="sql">The SQL code.</param>
        public static void Go(string sql)
        {
            Call(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (sql == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "sql", Wall.Text.Method.Go);
                }

                Reader.Go(ca, sql, null);
            });
        }

    }
}