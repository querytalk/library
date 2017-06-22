#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk
{
    public static partial class Extensions
    {
        /// <summary>
        /// Returns true if the row status is New.
        /// </summary>
        /// <param name="status">The row status.</param>
        public static bool IsNew(this DbRowStatus status)
        {
            return status == DbRowStatus.New;
        }

        /// <summary>
        /// Returns true if the row status is Loaded.
        /// </summary>
        /// <param name="status">The row status.</param>
        public static bool IsLoaded(this DbRowStatus status)
        {
            return status == DbRowStatus.Loaded;
        }

        /// <summary>
        /// Returns true if the row status is Modified.
        /// </summary>
        /// <param name="status">The row status.</param>
        public static bool IsModified(this DbRowStatus status)
        {
            return status == DbRowStatus.Modified;
        }

        /// <summary>
        /// Returns true if the row status is Deleted.
        /// </summary>
        /// <param name="status">The row status.</param>
        public static bool IsDeleted(this DbRowStatus status)
        {
            return status == DbRowStatus.Deleted;
        }

        /// <summary>
        /// Returns true if the row is updatable.
        /// </summary>
        /// <param name="status">The row status.</param>
        public static bool IsUpdatable(this DbRowStatus status)
        {
            return status == DbRowStatus.Loaded || status == DbRowStatus.Modified;
        }
    }
}
