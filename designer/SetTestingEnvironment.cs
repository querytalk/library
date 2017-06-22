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
        /// <para>Shuts down the testing environment.</para>
        /// <para>This instruction cannot be reverted.</para>
        /// </summary>
        public static void ShutTestingEnvironmentDown(bool @global = false)
        {
            Call(Assembly.GetCallingAssembly(), (ca) =>
            {
                Admin.ShutTestingEnvironmentDown(ca, @global);
            });
        }

        /// <summary>
        /// Sets the testing environment. If true then the testing environment will be shown on .Test method, unless the .ShutTestingEnvironmentDown method was previously used. If false, the testing environment won't be shown.
        /// </summary>
        public static void SetTestingEnvironment(bool on)
        {
            Call(Assembly.GetCallingAssembly(), (ca) =>
            {
                Admin.SetTestingEnvironment(ca, on);
            });
        }

    }
}