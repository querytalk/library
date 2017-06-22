#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {
        /// <summary>
        /// <para>CURRENT_USER built-in function.</para>
        /// <para>Returns the name of the current user. This function is equivalent to USER_NAME().</para>
        /// </summary>
        public static SysFn CurrentUser
        {
            get
            {
                return new SysFn("CURRENT_USER");
            }
        }

        /// <summary>
        /// <para>ORIGINAL_LOGIN built-in function.</para>
        /// <para>Returns the name of the login that connected to the instance of SQL Server. You can use this function to return the identity of the original login in sessions in which there are many explicit or implicit context switches.</para>
        /// </summary>
        public static SysFn OriginalLogin()
        {
            return new SysFn("ORIGINAL_LOGIN()");
        }

        /// <summary>
        /// <para>SESSION_USER built-in function.</para>
        /// <para>SESSION_USER returns the user name of the current context in the current database.</para>
        /// </summary>
        public static SysFn SessionUser
        {
            get
            {
                return new SysFn("SESSION_USER");
            }
        }

        /// <summary>
        /// <para>SUSER_ID built-in function.</para>
        /// <para>Returns the login identification number of the user.</para>
        /// </summary>
        public static SysFn SuserId()
        {
            return new SysFn("SUSER_ID()");
        }

        /// <summary>
        /// <para>SUSER_ID built-in function.</para>
        /// <para>Returns the login identification number of the user.</para>
        /// </summary>
        /// <param name="login">Is the login name of the user.</param>
        public static SysFn SuserId(string login)
        {
            return new SysFn(String.Format("SUSER_ID({0})", Filter.DelimitQuote(login)));
        }

        /// <summary>
        /// <para>SUSER_SID built-in function.</para>
        /// <para>Returns the security identification number (SID) for the specified login name.</para>
        /// </summary>
        public static SysFn SuserSid()
        {
            return new SysFn("SUSER_SID()");
        }

        /// <summary>
        /// <para>SUSER_SID built-in function.</para>
        /// <para>Returns the security identification number (SID) for the specified login name.</para>
        /// </summary>
        /// <param name="login">Is the login name of the user.</param>
        public static SysFn SuserSid(string login)
        {
            return new SysFn(String.Format("SUSER_SID({0})", Filter.DelimitQuote(login)));
        }

        /// <summary>
        /// <para>SUSER_SNAME built-in function.</para>
        /// <para>Returns the login identification name of the user.</para>
        /// </summary>
        public static SysFn SuserSname()
        {
            return new SysFn("SUSER_SNAME()");
        }

        /// <summary>
        /// <para>SUSER_SNAME built-in function.</para>
        /// <para>Returns the login identification name of the user.</para>
        /// </summary>
        /// <param name="serverUserId">Is the login identification number of the user.</param>
        public static SysFn SuserSname(int serverUserId)
        {
            return new SysFn(String.Format("SUSER_SNAME({0})", Mapping.BuildCast(serverUserId)));
        }

        /// <summary>
        /// <para>SUSER_NAME built-in function.</para>
        /// <para>Returns the login identification name of the user.</para>
        /// </summary>
        public static SysFn SuserName()
        {
            return new SysFn("SUSER_NAME()");
        }

        /// <summary>
        /// <para>SUSER_NAME built-in function.</para>
        /// <para>Returns the login identification name of the user.</para>
        /// </summary>
        /// <param name="serverUserId">Is the login identification number of the user.</param>
        public static SysFn SuserName(int serverUserId)
        {
            return new SysFn(String.Format("SUSER_NAME({0})",
                serverUserId));
        }

        /// <summary>
        /// <para>SYSTEM_USER built-in function.</para>
        /// <para>Allows a system-supplied value for the current login to be inserted into a table when no default value is specified.</para>
        /// </summary>
        public static SysFn SystemUser
        {
            get
            {
                return new SysFn("SYSTEM_USER");
            }
        }

        /// <summary>
        /// <para>USER_NAME built-in function.</para>
        /// <para>Returns a database user name from a specified identification number.</para>
        /// </summary>
        public static SysFn UserName()
        {
            return new SysFn("USER_NAME()");
        }

        /// <summary>
        /// <para>USER_NAME built-in function.</para>
        /// <para>Returns a database user name from a specified identification number.</para>
        /// </summary>
        /// <param name="id">Is the identification number associated with a database user. idis int.</param>
        public static SysFn UserName(int id)
        {
            return new SysFn(String.Format("USER_NAME({0})",
                id));
        }

        /// <summary>
        /// <para>DATABASE_PRINCIPAL_ID built-in function.</para>
        /// <para>Returns the ID number of a principal in the current database.</para>
        /// </summary>
        public static SysFn DatabasePrincipalId()
        {
            return new SysFn("DATABASE_PRINCIPAL_ID()");
        }

        /// <summary>
        /// <para>DATABASE_PRINCIPAL_ID built-in function.</para>
        /// <para>Returns the ID number of a principal in the current database.</para>
        /// </summary>
        /// <param name="principalName">Is an expression of type sysname that represents the principal.</param>
        public static SysFn DatabasePrincipalId(string principalName)
        {
            return new SysFn(String.Format("DATABASE_PRINCIPAL_ID({0})", Filter.DelimitQuote(principalName)));
        }
    }
}
