#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Reflection;

namespace QueryTalk.Wall
{
    // A static class with methods to manage connections.
    internal static class ConnectionManager
    {
        internal static void SetConnectionFunc(Assembly client, Func<ConnectionKey, ConnectionData> connectionFunc, bool @global)
        {
            if (connectionFunc == null)
            {
                throw new QueryTalkException("ConnectionManager.SetConnectionFunc",
                    QueryTalkExceptionType.ArgumentNull, "connectionFunc = null",
                    Text.Method.SetConnection);
            }

            var settings = Admin.GetSettings(client);
            settings.ConnectionFunc = connectionFunc;

            // manage global setting
            if (@global && Admin.CheckConnectionFuncGlobalSetting())
            {
                settings.IsConnectionFuncGlobal = true;
            }
        }

        // invokes connection function
        internal static ConnectionData InvokeConnectionFunc(Assembly client, ConnectionKey connectionKey)
        {
            try
            {
                // search global setting
                var globalConnectionFunc = Admin.GetGlobalConnectionFunc();
                if (globalConnectionFunc != null)
                {
                    return globalConnectionFunc(connectionKey);
                }

                // no global settings:

                var settings = Admin.GetSettings(client);

                // search client setting
                if (settings.ConnectionFunc != null)
                {
                    return settings.ConnectionFunc(connectionKey); // always provide connection args object
                }

                // no client settings:

                throw MissingConnectionFuncException(client);
            }
            catch (QueryTalkException)
            {
                throw;
            }
            catch (System.Exception ex)
            {
                var exception = new QueryTalkException("ConnectionManager.InvokeConnectionFunc",
                    QueryTalkExceptionType.ClrException, null, Text.Method.InvokeConnectionManager, String.Format("client = {0}", client));
                exception.ClrException = ex;
                throw exception;
            }
        }

        // common method for processing the connection data 
        internal static ConnectionData GetConnectionData(Assembly client, 
            ConnectBy connectBy,        // explicitly given query key
            DbNode node,                // database object bound to database map key (very likely to be given)
            Chainer chainObject)        // chain object that is very likely to have no connection data (except when .ConnectBy is called inside the compilable)
        {
            ConnectionKey connectionKey = null;

            if (connectBy != null)
            {
                connectionKey = ((IConnectable)connectBy).ConnectionKey;
            }
            else if (node != null)
            {
                connectionKey = ((IConnectable)node).ConnectionKey;
                if (connectionKey != null)
                {
                    ((IConnectable)node).ResetConnectionKey();   // always clean it up after the getter provide the ConnectionKey
                }
            }
            else if (chainObject != null)
            {
                var root = chainObject.GetRoot();

                // root: in cases when connection data is stored into compilable object
                //   example:
                //     s.River.Select().ConnectBy().Go()
                if (root.ConnectionKey != null)
                {
                    connectionKey = root.ConnectionKey;
                    root.ConnectionKey = null;   
                }

                // root.Node: in cases when the compilable is based on the mapped object
                //   example:
                //     s.River.Select().Go()
                else if (root.Node != null)
                {
                    connectionKey = ((IConnectable)root.Node).ConnectionKey;
                    if (connectionKey != null)
                    {
                        ((IConnectable)root.Node).ResetConnectionKey();   // always clean it up after the getter provide the ConnectionKey
                    }
                }
            }

            return ConnectionManager.InvokeConnectionFunc(client, connectionKey);
        }

        private static QueryTalkException MissingConnectionFuncException(Assembly client)
        {
            var exception = new QueryTalkException(
                "ConnectionManager.InvokeConnectionFunc",
                QueryTalkExceptionType.MissingConnectionFunc,
                String.Format("assembly = {0}", client),
                Text.Method.SetConnection);
            throw exception;
        }

    }
}
