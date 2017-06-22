#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {
        /// <summary>
        /// Converts params input into a collection.
        /// </summary>
        /// <typeparam name="T">The type of the item.</typeparam>
        /// <param name="items">Are individual items to be returned as a collection.</param>
        /// <returns></returns>
        public static IEnumerable<T> Rows<T>(params T[] items)
        {
            return items;
        }

    }
}