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
        /// Inserts the row with the associated children rows.
        /// </summary>
        /// <typeparam name="T">The type of the row object.</typeparam>
        /// <param name="row">The row to insert.</param>
        public static Result<T> InsertCascadeGo<T>(T row)
            where T : DbRow
        {
            return Call<Result<T>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (row == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "row", Wall.Text.Method.InsertCascadeGo);
                }

                return Crud.InsertCascadeGo<T>(ca, row, null);
            });
        }

    }
}