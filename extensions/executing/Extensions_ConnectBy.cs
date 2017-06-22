#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Reflection;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates all public extension methods of the QueryTalk library.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Specifies a connection to be used in execution.
        /// </summary>
        /// <typeparam name="T">The IConnectable type.</typeparam>
        /// <param name="connectable">A predecessor object.</param>+
        /// <param name="connectionKey">A connection key that determines the connection.</param>
        public static T ConnectBy<T>(this T connectable, ConnectionKey connectionKey)
            where T : IConnectable
        {
                    if (connectable is DbNode)
                    {
                        // the connection key of a node graph should be bound to the subject
                        ((IConnectable)((DbNode)(object)connectable).Root).SetConnectionKey(connectionKey);
                    }
                    else if (connectable is DbRow)
                    {
                        var row = (DbRow)(object)connectable;
                        row.TrySetNode(); 
                        connectable.SetConnectionKey(connectionKey);
                    }
                    else
                    {
                        connectable.SetConnectionKey(connectionKey);
                    }

                    return connectable;
        }

        #region Internal

        internal static Connectable ConnectByInternal(this IConnectBy prev, Assembly client, string connectionString)
        {
            return Reader.GetConnectable(client, (Chainer)prev, connectionString);
        }

        #endregion

    }
}
