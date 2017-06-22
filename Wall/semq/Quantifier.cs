#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// Represents the quantification definition in semantic querying.
    /// </summary>
    public sealed class Quantifier
    {
        internal QuantifierType QuantifierType { get; private set; }

        internal long Cardinality { get; private set; }

        internal long MinCardinality { get; private set; }

        internal long MaxCardinality { get; private set; }

        internal Quantifier(QuantifierType quantifierType)
        {
            QuantifierType = quantifierType;
        }

        internal Quantifier(QuantifierType quantifierType, long cardinality)
        {
            QuantifierType = quantifierType;
            Cardinality = cardinality;
        }

        internal Quantifier(QuantifierType quantifierType, long minCardinality, long maxCardinality)
        {
            QuantifierType = quantifierType;
            MinCardinality = minCardinality;
            MaxCardinality = maxCardinality;
        }

        // build quantifier predicate with BIG_COUNT contained in the view
        //   attention:
        //      Most quantifier is not included.
        internal Expression CreateExpression(View view)
        {
            switch (QuantifierType)
            {
                case Wall.QuantifierType.AtLeastOne:
                    return view.GreaterOrEqualThan(((long)1).L());

                case Wall.QuantifierType.None:
                    return view.EqualTo(((long)0).L());

                case Wall.QuantifierType.Many:
                    return view.GreaterThan(((long)1).L());

                case Wall.QuantifierType.AtLeast:
                    return view.GreaterOrEqualThan(Cardinality.L());

                case Wall.QuantifierType.AtMost:
                    return view.LessOrEqualThan(Cardinality.L());

                case Wall.QuantifierType.MoreThan:
                    return view.GreaterThan(Cardinality.L());

                case Wall.QuantifierType.LessThan:
                    return view.LessThan(Cardinality.L());

                case Wall.QuantifierType.Exactly:
                    return view.EqualTo(Cardinality.L());

                case Wall.QuantifierType.Between:
                    return view.Between(MinCardinality.L(), MaxCardinality.L());

                default:
                    return view.GreaterOrEqualThan(((long)1).L());
            }
        }
    }
}
