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
        /// Removes the rows from a table or view. The target table is specified in the FROM clause.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static DeleteChainer Delete(this IDelete prev)
        {
            return new DeleteChainer((Chainer)prev, null);
        }

        /// <summary>
        /// Removes the rows from a table or view. The target table or view with an alias is specified in the FROM clause.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="alias">The alias of the table in the FROM clause from which the rows are to be deleted.</param>
        public static DeleteChainer Delete(this IDelete prev, string alias)
        {
            return new DeleteChainer((Chainer)prev, alias);
        }

        /// <summary>
        /// Removes the rows from a table or view. The target table or view with an alias is specified in the FROM clause.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="alias">The alias of the table in the FROM clause from which the rows are to be deleted.</param>
        public static DeleteChainer Delete(this IDelete prev, int alias)
        {
            return new DeleteChainer((Chainer)prev, alias.ToString());
        }
    }
}
