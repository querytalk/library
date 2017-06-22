#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Reflection;

namespace QueryTalk.Wall
{
    // method wrappers to manage CLR exceptions
    internal static class PublicInvoker
    {

        internal static T Call<T>(Func<T> method)
        {
            try
            {
                return method();
            }
            catch (QueryTalkException ex)
            {
                if (ex.ClrException != null)
                {
                    throw ex.ClrException;
                }

                throw;
            }
        }

        internal static T Call<T>(Assembly callingAssembly, Func<Assembly, T> method)
        {
            try
            {
                return method(callingAssembly);
            }
            catch (QueryTalkException ex)
            {
                if (ex.ClrException != null)
                {
                    throw ex.ClrException;
                }

                throw;
            }
        }

        internal static void Call(Action method)
        {
            try
            {
                method();
            }
            catch (QueryTalkException ex)
            {
                if (ex.ClrException != null)
                {
                    throw ex.ClrException;
                }

                throw;
            }
        }

        internal static void Call(Assembly callingAssembly, Action<Assembly> method)
        {
            try
            {
                method(callingAssembly);
            }
            catch (QueryTalkException ex)
            {
                if (ex.ClrException != null)
                {
                    throw ex.ClrException;
                }

                throw;
            }
        }

    }
}
