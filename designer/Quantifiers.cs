#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {

        /// <summary>
        /// There should be at least one object to satisfy the quantifier. (The default quantifier.)
        /// </summary>
        public static Quantifier AtLeastOne
        {
            get
            {
                return new Quantifier(QuantifierType.AtLeastOne);
            }
        }

        /// <summary>
        /// No object satisfies the specified quantifier.
        /// </summary>
        public static Quantifier None
        {
            get
            {
                return new Quantifier(QuantifierType.None, 0);
            }
        }

        /// <summary>
        /// There should be exactly one object to satisfy the specified quantifier.
        /// </summary>
        public static Quantifier One
        {
            get
            {
                return new Quantifier(QuantifierType.Exactly, 1);
            }
        }

        /// <summary>
        /// There should be more than one object to satisfy the specified quantifier.
        /// </summary>
        public static Quantifier Many
        {
            get
            {
                return new Quantifier(QuantifierType.Many);
            }
        }

        /// <summary>
        /// There should be at least as many objects as specified in order to satisfy the specified quantifier.
        /// </summary>
        /// <param name="cardinality">Specifies the cardinality of a quantifier method.</param>
        public static Quantifier AtLeast(long cardinality)
        {
            return new Quantifier(QuantifierType.AtLeast, cardinality);
        }

        /// <summary>
        /// There should be at most as many objects as specified in order to satisfy the specified quantifier.
        /// </summary>
        /// <param name="cardinality">Specifies the cardinality of a quantifier method.</param>
        public static Quantifier AtMost(long cardinality)
        {
            return new Quantifier(QuantifierType.AtMost, cardinality);
        }

        /// <summary>
        /// There should be more than as many objects as specified in order to satisfy the specified quantifier.
        /// </summary>
        /// <param name="cardinality">Specifies the cardinality of a quantifier method.</param>
        public static Quantifier MoreThan(long cardinality)
        {
            return new Quantifier(QuantifierType.MoreThan, cardinality);
        }

        /// <summary>
        /// There should be less than as many objects as specified in order to satisfy the specified quantifier.
        /// </summary>
        /// <param name="cardinality">Specifies the cardinality of a quantifier method.</param>
        public static Quantifier LessThan(long cardinality)
        {
            return new Quantifier(QuantifierType.LessThan, cardinality);
        }

        /// <summary>
        /// There should be exactly as many objects as specified in order to satisfy the specified quantifier.
        /// </summary>
        /// <param name="cardinality">Specifies the cardinality of a quantifier method.</param>
        public static Quantifier Exactly(long cardinality)
        {
            return new Quantifier(QuantifierType.Exactly, cardinality);
        }

        /// <summary>
        /// There should be a specified range of objects in order to satisfy the quantifier.
        /// </summary>
        /// <param name="minCardinality">Specifies the minimum cardinality of a quantifier method.</param>
        /// <param name="maxCardinality">Specifies the maximum cardinality of a quantifier method.</param>
        public static Quantifier Between(long minCardinality, long maxCardinality)
        {
            return new Quantifier(QuantifierType.Between, minCardinality, maxCardinality);
        }

        /// <summary>
        /// A subset that contains the most objects of a certain kind as specified in the predicate satisfies the specified quantifier.
        /// </summary>
        public static Quantifier Most
        {
            get
            {
                return new Quantifier(QuantifierType.Most, 1);
            }
        }

        /// <summary>
        /// <para>A subset that contains the most objects of a certain kind as specified in the predicate satisfies the specified quantifier.</para>
        /// <para>More than one subset returned in descending order satisfies the quantifier if the top value is specified as greater than 1.</para>
        /// </summary>
        /// <param name="top">Is a specified top argument.</param>
        public static Quantifier TopMost(long top)
        {
            return new Quantifier(QuantifierType.Most, top);
        }

    }
}