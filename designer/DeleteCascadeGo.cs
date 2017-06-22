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
        /// Deletes the row with the associated children rows.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to delete.</param>
        /// <param name="maxLevels">Specifies the maximum number of cascading levels that can be included in the deletion. If more levels than maxLevels are needed to complete the cascade deletion, an exception is thrown.</param>
        public static Result<T> DeleteCascadeGo<T>(T row, int maxLevels = 5)
            where T : DbRow
        {
            return Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (row == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "row", Wall.Text.Method.DeleteCascadeGo);
                }

                return Crud.DeleteCascadeGo<T>(ca, row, maxLevels, null);
            });
        }

    }
}