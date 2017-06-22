#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Reflection;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Encapsulates the CRUD methods.
    /// </summary>
    public static partial class Crud
    {
        private const int PackageSize = 200;

        // checks if row is a table
        internal static void CheckTable(DbRow row, string method)
        {
            if (row is INode)
            {
                return;
            }

            throw new QueryTalkException("Crud.CheckTable", QueryTalkExceptionType.InvalidCrudOperationException,
                String.Format("node = {0}", row.NodeID.GetNodeName()), method);
        }

        internal static SubResult GoMirroring<T>(
            Assembly client,
            IEnumerable<T> rows,      
            Func<Assembly, T, bool, ConnectBy, SubResult> processMethod,
            ConnectBy connectBy)      
            where T : DbRow
        {
            var name = Text.NotAvailable;

            try
            {

                #region Check

                var subResult = new SubResult(); 

                if (rows == null)
                {
                    throw new QueryTalkException("Crud.GoMirroring", QueryTalkExceptionType.ArgumentNull, 
                        "rows = null", Text.Method.CrudRowsGo);
                }

                if (rows.Count() == 0)
                {
                    return subResult;
                }

                // non-null rows 
                var rows2 = new List<T>();
                foreach (var row in rows)
                {
                    if (row != null)
                    {
                        rows2.Add(row);
                    }
                }

                if (rows2.Count() == 0)
                {
                    return subResult;
                }

                Crud.CheckTable(rows.First(), Text.Method.CrudRowsGo);

                #endregion

                var map = DbMapping.TryGetNodeMap(rows2.First());
                name = map.Name.Sql;
                var scopeOption = GetTransactionScopeOption(client, map, connectBy);

                // check if ambient transaction exists and use its isolation level
                if (Transaction.Current != null)
                {
                    scopeOption.IsolationLevel = Transaction.Current.IsolationLevel;
                }

                using (var scope = new TransactionScope(TransactionScopeOption.Required, scopeOption))
                {
                    // loop
                    foreach (var row in rows)
                    {
                        var sub = processMethod(client, row, true, connectBy);
                        if (sub.Executed)
                        {
                            subResult
                                .SetExecuted()
                                .AddAffectedCount(sub.AffectedCount);
                        }
                    }

                    scope.Complete();
                }

                return subResult;
            }
            catch (QueryTalkException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw Crud.ClrException(ex, name, Text.NotAvailable);
            }
        }

        internal static SubResult GoNonMirroring<T>(
            Assembly client,
            IEnumerable<T> rows,        
            Func<Assembly, IEnumerable<T>, NodeMap, ConnectBy, Nullable<bool>, SubResult> processMethod,
            ConnectBy connectBy,
            Nullable<bool> identityInsert)  
            where T : DbRow
        {
            var name = Text.NotAvailable;

            try
            {

                #region Check

                var subResult = new SubResult(); 

                if (rows == null)
                {
                    throw new QueryTalkException("Crud.GoNonMirroring", QueryTalkExceptionType.ArgumentNull, 
                        "rows = null", Text.Method.CrudRowsGo);
                }

                if (rows.Count() == 0)
                {
                    return subResult;
                }

                Crud.CheckTable(rows.First(), Text.Method.CrudRowsGo); 

                // non-null rows 
                var rows2 = new List<T>();
                foreach (var row in rows)
                {
                    if (row != null)
                    {
                        rows2.Add(row);
                    }
                }

                if (rows2.Count() == 0)
                {
                    return subResult;
                }

                #endregion

                var map = DbMapping.TryGetNodeMap(rows2.First());
                name = map.Name.Sql;

                if (!map.HasIdentity)
                {
                    identityInsert = false;
                }

                // set RowID
                var i = 0;
                foreach (var row in rows2)
                {
                    ((IRow)row).RowID = i++;
                }

                var packageIndex = 0;
                var scopeOption = GetTransactionScopeOption(client, map, connectBy);

                // check if ambient transaction exists and use its isolation level
                if (Transaction.Current != null)
                {
                    scopeOption.IsolationLevel = Transaction.Current.IsolationLevel;
                }

                using (var scope = new TransactionScope(TransactionScopeOption.Required, scopeOption))
                {
                    var count = rows2.Count();   // entire volume of data to be processed
                    var ix = 0;                  // pointer to the first row in the package

                    do
                    {
                        var package = rows2.Where(row => ((IRow)row).RowID >= ix && ((IRow)row).RowID < ix + Crud.PackageSize).ToList();

                        var sub = processMethod(client, package, map, connectBy, identityInsert);

                        if (sub.Executed)
                        {
                            subResult
                                .SetExecuted()
                                .AddAffectedCount(sub.AffectedCount);
                        }

                        ix += Crud.PackageSize;
                        ++packageIndex;

                    } while (ix < count);

                    scope.Complete();
                }

                return subResult;
            }
            catch (QueryTalkException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                throw Crud.ClrException(ex, name, Text.NotAvailable);
            }
        }

        internal static TransactionOptions GetTransactionScopeOption(Assembly client, Map map, ConnectBy connectBy)
        {
            var tempNode = new InternalNode(map.ID);    
            var connectionData = ConnectionManager.GetConnectionData(client, connectBy, tempNode, null);
            var scopeOption = new TransactionOptions();
            scopeOption.Timeout = TimeSpan.FromSeconds(connectionData.CommandTimeout);
            scopeOption.IsolationLevel = connectionData.MassDataOperationIsolationLevel.ToSystemEnum();
            return scopeOption;
        }

        internal static QueryTalkException ClrException(System.Exception ex, string name, string method, string arguments = null)
        {
            QueryTalkException exception = new QueryTalkException("Crud",
                QueryTalkExceptionType.ClrException, name, method, arguments);
            exception.ClrException = ex;
            return exception;
        }

    }
}
