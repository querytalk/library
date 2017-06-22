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
        /// Reloads the specified row from the database.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to reload.</param>
        /// <param name="forceMirroring">If true, then the optimistic concurrency check is done which throws an exception if the check fails.</param>
        public static Result<T> ReloadGo<T>(T row, bool forceMirroring = true)
            where T : DbRow
        {
            return Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (row == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "row", Wall.Text.Method.ReloadGo);
                }

                return Crud.ReloadGo<T>(ca, row, forceMirroring, null);
            });
        }

    }
}