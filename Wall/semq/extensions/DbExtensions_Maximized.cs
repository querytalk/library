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
        /// <para>A maximized MAX predicate of a subject.</para>
        /// <para>Note that a maximized predicate should be the last predicate in a sentence.</para>
        /// </summary>
        /// <typeparam name="T">The type of the subject.</typeparam>
        /// <param name="node">A subject node.</param>
        /// <param name="maximizedExpression">A maximized expression.</param>
        public static T HavingMax<T>(this T node, OrderingArgument maximizedExpression)
            where T : ISemantic
        {
            ((IPredicate)node).AddPredicate(
                new Predicate((DbNode)(object)node, MaximizationType.Max, true, LogicalOperator.And, maximizedExpression));
            return node;
        }

        /// <summary>
        /// <para>A maximized MAX predicate of a subject.</para>
        /// <para>Note that a maximized predicate should be the last predicate in a sentence.</para>
        /// </summary>
        /// <typeparam name="T">The type of the subject.</typeparam>
        /// <param name="node">A subject node.</param>
        /// <param name="top">The top value that will limit the rows.</param>
        /// <param name="maximizedExpression">A maximized expression.</param>
        public static T HavingMax<T>(this T node, long top, OrderingArgument maximizedExpression)
            where T : ISemantic
        {
            ((IPredicate)node).AddPredicate(
                new Predicate((DbNode)(object)node, MaximizationType.Max, true, LogicalOperator.And, maximizedExpression, top));
            return node;
        }

        /// <summary>
        /// <para>A maximized MIN predicate of a subject.</para>
        /// <para>Note that a maximized predicate should be the last predicate in a sentence.</para>
        /// </summary>
        /// <typeparam name="T">The type of the subject.</typeparam>
        /// <param name="node">A subject node.</param>
        /// <param name="maximizedExpression">A maximized expression.</param>
        public static T HavingMin<T>(this T node, OrderingArgument maximizedExpression)
            where T : ISemantic
        {
            ((IPredicate)node).AddPredicate(
                new Predicate((DbNode)(object)node, MaximizationType.Min, true, LogicalOperator.And, maximizedExpression));
            return node;
        }

        /// <summary>
        /// <para>A maximized MIN predicate of a subject.</para>
        /// <para>Note that a maximized predicate should be the last predicate in a sentence.</para>
        /// </summary>
        /// <typeparam name="T">The type of the subject.</typeparam>
        /// <param name="node">A subject node.</param>
        /// <param name="top">The top value that will limit the rows.</param>
        /// <param name="maximizedExpression">A maximized expression.</param>
        public static T HavingMin<T>(this T node, long top, OrderingArgument maximizedExpression)
            where T : ISemantic
        {
            ((IPredicate)node).AddPredicate(
                new Predicate((DbNode)(object)node, MaximizationType.Min, true, LogicalOperator.And, maximizedExpression, top));
            return node;
        }

    }
}
