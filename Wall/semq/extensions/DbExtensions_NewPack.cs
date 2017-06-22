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
        /// Creates a new row.
        /// </summary>
        /// <typeparam name="T">The type of the row.</typeparam>
        /// <param name="node">A node that is used to create a new row.</param>
        public static T New<T>(this DbTable<T> node)
            where T : DbRow, new()
        {
            return new T();
        }

        /// <summary>
        /// Copy data from the source object into the row.
        /// </summary>
        /// <typeparam name="T">The type of the row.</typeparam>
        /// <param name="target">The target row object.</param>
        /// <param name="source">The source object.</param>
        public static T Pack<T>(this T target, object source)
            where T : DbRow
        {
            if (target == null)
            {
                throw new QueryTalkException("extensions.Pack<T>", QueryTalkExceptionType.ArgumentNull, "target = null", Text.Method.Pack);
            }
            if (source == null)
            {
                throw new QueryTalkException("extensions.Pack<T>", QueryTalkExceptionType.ArgumentNull, "source = null", Text.Method.Pack);
            }

            if (source != null)
            {
                return ClassConverter.SetClass<T>(target, source);
            }

            return target;
        }
    }
}
