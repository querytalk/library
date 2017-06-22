#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Reflection;

namespace QueryTalk.Wall
{
    internal static partial class Reader
    {
        // This method is used to execute the raw SQL. It can execute many batches separated by the GO command.
        internal static void Go(Assembly client, string sql, ConnectBy connectBy)
        {
            if (String.IsNullOrWhiteSpace(sql))
            {
                return;
            }

            var connectionData = ConnectionManager.GetConnectionData(client, connectBy, null, null);
            var batches = GetBatches(sql);

            using (SqlConnection cn = new SqlConnection(connectionData.ConnectionString))
            {
                cn.Open();

                foreach (var batch in batches)
                {
                    if (String.IsNullOrWhiteSpace(batch))
                    {
                        continue;
                    }

                    var batchSql = Text.GenerateSql(200)
                        .Append(Text.Free.QueryTalkCode)  
                        .AppendLine(batch);

                    using (var cmd = new SqlCommand(batchSql.ToString(), cn))
                    {
                        cmd.CommandTimeout = connectionData.CommandTimeout;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // return batchs of SQL code split by GO instruction
        private static string[] GetBatches(string sql)
        {
            var batches = Regex.Split(sql, @"^\s*GO\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            for (int i = 0; i < batches.Length; ++i)
            {
                var batch = batches[i];
                batches[i] = Regex.Replace(batch, @"(^\s+)|(\s+$)", "");
            }
            return batches;
        }

    }
}
