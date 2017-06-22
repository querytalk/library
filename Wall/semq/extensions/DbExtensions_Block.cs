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
        /// Begins a block of predicates.
        /// </summary>
        /// <typeparam name="TRoot">The type of the node.</typeparam>
        /// <param name="node">The subject of the predicates.</param>
        public static DbTable<TRoot> Block<TRoot>(this DbTable<TRoot> node)
            where TRoot : DbRow
        {
            if (node == null)
            {
                throw new QueryTalkException(".Block", QueryTalkExceptionType.ArgumentNull, "node = null", Text.Method.Block);
            }
            ((IPredicate)node).SetPredicateGroup(PredicateGroupType.Begin);
            return node;
        }

        /// <summary>
        /// Ends a block of predicates.
        /// </summary>
        /// <typeparam name="TRoot">The type of the node.</typeparam>
        /// <param name="node">The subject of the predicates.</param>
        public static DbTable<TRoot> EndBlock<TRoot>(this DbTable<TRoot> node)
            where TRoot : DbRow
        {
            if (node == null)
            {
                throw new QueryTalkException(".EndBlock", QueryTalkExceptionType.ArgumentNull, "node = null", Text.Method.EndBlock);
            }
            ((IPredicate)node).SetPredicateGroup(PredicateGroupType.End);
            return node;
        }

    }
}
