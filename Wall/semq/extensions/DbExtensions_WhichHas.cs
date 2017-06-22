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
        /// <para>Provides a predicate of a subject. A subject with a predicate returns equal or less rows than a subject without a predicate.</para>
        /// <para>All predicate methods are logically identical.</para>
        /// </summary>
        /// <typeparam name="T">The type of the subject node.</typeparam>
        /// <param name="node">A subject node.</param>
        /// <param name="sentence">A predicate sentence.</param>
        public static T WhichHas<T>(this T node, ISemantic sentence)
            where T : ISemantic
        {
            return RelatedTo(node, sentence);
        }

        /// <summary>
        /// <para>Provides a predicate of a subject. A subject with a predicate returns equal or less rows than a subject without a predicate.</para>
        /// <para>All predicate methods are logically identical.</para>
        /// </summary>
        /// <typeparam name="T">The type of the subject node.</typeparam>
        /// <param name="node">A subject node.</param>
        /// <param name="expression">A predicate expression.</param>
        public static T WhichHas<T>(this T node, Expression expression)
            where T : ISemantic
        {
            return RelatedTo(node, expression);
        }

        /// <summary>
        /// <para>Provides a predicate of a subject. A subject with a predicate returns equal or less rows than a subject without a predicate.</para>
        /// <para>All predicate methods are logically identical.</para>
        /// </summary>
        /// <typeparam name="T">The type of the subject node.</typeparam>
        /// <param name="node">A subject node.</param>
        /// <param name="argument1">The first argument in the equality expression.</param>
        /// <param name="argument2">The second argument in the equality expression.</param>
        public static T WhichHas<T>(this T node, ScalarArgument argument1, ValueScalarArgument argument2)
            where T : ISemantic
        {
            return RelatedTo(node, argument1, argument2);
        }

        /// <summary>
        /// <para>Provides a predicate of a subject. A subject with a predicate returns equal or less rows than a subject without a predicate.</para>
        /// <para>All predicate methods are logically identical.</para>
        /// </summary>
        /// <typeparam name="T">The type of the subject node.</typeparam>
        /// <param name="node">A subject node.</param>
        /// <param name="quantifier">A quantifier of a predicate.</param>
        /// <param name="sentence">A predicate sentence.</param>
        public static T WhichHas<T>(this T node, Quantifier quantifier, ISemantic sentence)
            where T : ISemantic
        {
            return RelatedTo(node, quantifier, sentence);
        }

        /// <summary>
        /// <para>Provides a predicate of a subject. A subject with a predicate returns equal or less rows than a subject without a predicate.</para>
        /// <para>All predicate methods are logically identical.</para>
        /// </summary>
        /// <typeparam name="T">The type of the subject node.</typeparam>
        /// <param name="node">A subject node.</param>
        /// <param name="quantifier">A quantifier of a predicate.</param>
        /// <param name="expression">A predicate expression.</param>
        public static T WhichHas<T>(this T node, Quantifier quantifier, Expression expression)
            where T : ISemantic
        {
            return RelatedTo(node, quantifier, expression);
        }

        /// <summary>
        /// <para>Provides a predicate of a subject. A subject with a predicate returns equal or less rows than a subject without a predicate.</para>
        /// <para>All predicate methods are logically identical.</para>
        /// </summary>
        /// <typeparam name="T">The type of the subject node.</typeparam>
        /// <param name="node">A subject node.</param>
        /// <param name="quantifier">A quantifier of a predicate.</param>
        /// <param name="argument1">The first argument in the equality expression.</param>
        /// <param name="argument2">The second argument in the equality expression.</param>
        public static T WhichHas<T>(this T node, Quantifier quantifier, ScalarArgument argument1, ValueScalarArgument argument2)
            where T : ISemantic
        {
            return RelatedTo(node, quantifier, argument1, argument2);
        }

    }
}
