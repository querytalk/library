#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        /// <summary>
        /// Updates a table.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        public static UpdateChainer Update(this IUpdate prev)
        {
            return new UpdateChainer((Chainer)prev, null);
        }

        /// <summary>
        /// Updates a table.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        /// <param name="alias">The alias of the table in the FROM clause from which the rows are to be updated.</param>
        public static UpdateChainer Update(this IUpdate prev, string alias)
        {
            return new UpdateChainer((Chainer)prev, alias);
        }

        /// <summary>
        /// Updates a table.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        /// <param name="alias">The alias of the table in the FROM clause from which the rows are to be updated.</param>
        public static UpdateChainer Update(this IUpdate prev, int alias)
        {
            return new UpdateChainer((Chainer)prev, alias.ToString());
        }

    }
}
