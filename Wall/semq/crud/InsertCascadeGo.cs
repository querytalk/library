#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Reflection;
using System.Transactions;

namespace QueryTalk.Wall
{
    static partial class Crud
    {
        internal static Result<T> InsertCascadeGo<T>(Assembly client, DbRow row, ConnectBy connectBy)
            where T : DbRow
        {
            var name = Text.NotAvailable;

            if (row == null)
            {
                throw new QueryTalkException("Crud", QueryTalkExceptionType.ArgumentNull, "row = null", Text.Method.InsertCascadeGo);
            }

            Crud.CheckTable(row, Text.Method.InsertCascadeGo);

            try
            {
                var map = DbMapping.TryGetNodeMap(row);
                name = map.Name.Sql;

                var scopeOption = GetTransactionScopeOption(client, map, connectBy);
                if (Transaction.Current != null)
                {
                    scopeOption.IsolationLevel = Transaction.Current.IsolationLevel;
                }

                using (var scope = new TransactionScope(TransactionScopeOption.Required, scopeOption))
                {
                    Crud.InsertGo<T>(client, row, false, connectBy);
                    ((INode)row).InsertGraph(client, connectBy);
                    scope.Complete();
                }

                return new Result<T>(true, -1);
            }
            catch (QueryTalkException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw Crud.ClrException(ex, name, Text.Method.InsertCascadeGo);
            }
        }

    }
}
