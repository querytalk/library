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
        /// Exits unconditionally from a batch.
        /// </summary>
        public static ReturnChainer Return()
        {
            return Call<ReturnChainer>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var root = new d();
                return new ReturnChainer((Chainer)root);
            });
        }

        /// <summary>
        /// Exits unconditionally from a batch.
        /// </summary>
        /// <param name="returnValue">Is a return value.</param>
        public static ReturnChainer Return(int returnValue)
        {
            return Call<ReturnChainer>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var root = new d();
                return new ReturnChainer((Chainer)root, returnValue);
            });
        }

    }
}