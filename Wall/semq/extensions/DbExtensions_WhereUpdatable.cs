#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;
using System.Linq;
using System.Data;
using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {
        /// <summary>
        /// Filters the specified rows returning the updatable rows only.
        /// </summary>
        /// <param name="rows">The rows to filter.</param>
        public static IEnumerable<DbRow> WhereUpdatable(this IEnumerable<DbRow> rows)
        {
            return (rows != null) ? rows.Where(r => r.GetStatus().IsUpdatable()) : null;
        }

    }
}
