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
        /// Specifies the connection which is to be used in the execution.
        /// </summary>
        /// <param name="connectionKey">Is a connection key as specified in the connection delegate method.</param>
        public static ConnectBy ConnectBy(ConnectionKey connectionKey)
        {
            return Call<ConnectBy>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (connectionKey == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "connectionKey", ".ConnectBy");
                }

                return new ConnectBy(new d(), connectionKey);
            });
        }

    }
}