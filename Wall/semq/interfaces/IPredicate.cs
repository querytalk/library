#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;

namespace QueryTalk.Wall
{
    internal interface IPredicate
    {
        List<Predicate> Predicates { get; }

        Predicate Predicate { get; set; }

        bool HasNot { get; set; }

        bool HasOr { get; set; }

        void AddPredicate(Predicate predicate);

        void AddPredicate(PredicateType predicateType, DbNode sentence);

        bool HasPredicate { get; }

        PredicateGroup PredicateGroup { get; set; }

        void SetPredicateGroup(PredicateGroupType predicateGroupType); 

        void ResetPredicateGroup();

        int PredicateGroupLevel { get; set; }
    }

}
