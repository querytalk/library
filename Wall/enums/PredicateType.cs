#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    internal enum PredicateType : int
    {
        // A default predicate which represents an inner relationship between a subject and its own attribute 
        // or a relationship between a subject and an outer subject with the cardinality 'at least one'.
        Existential = 0,

        // A subject is joined with an outer subject.
        Cartesian = 1,

        // A subject is in quantified relationship with an outer subject.
        Quantified = 2,

        // A subject is in top quantified relationship with an outer subject.
        TopQuantified = 3,

        // A subject is in self-relation with its maximum attribute.
        Maximized = 4,
    }
}
