#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
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
        /// Sets a default client connection within the scope of a client assembly.
        /// </summary>
        /// <param name="connectionString">
        /// Is a default connection string.
        /// </param>
        /// <param name="global">If true, then the scope of the connection definition is the AppDomain.</param>
        public static void SetConnection(string connectionString, bool @global = false)
        {
            Call(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (connectionString == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "connectionString", Wall.Text.Method.SetConnection);
                }

                ConnectionManager.SetConnectionFunc(ca, (Func<ConnectionKey, ConnectionData>)(connKey => { return connectionString; }), @global);
            });
        }

        /// <summary>
        /// Sets a default client connection within the scope of a client assembly.
        /// </summary>
        /// <param name="connectionFunc">
        /// Is a delegate method that receives a connection key as an argument and returns the connection string.
        /// </param>
        /// <param name="global">If true, then the scope of the connection definition is the AppDomain.</param>
        public static void SetConnection(Func<ConnectionKey, ConnectionData> connectionFunc, bool @global = false)
        {
            Call(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (connectionFunc == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "connectionFunc", Wall.Text.Method.SetConnection);
                }

                ConnectionManager.SetConnectionFunc(ca, connectionFunc, @global);
            });
        }

    }
}