#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    // represents a group of predicates logically separated from other group of predicates
    internal class PredicateGroup
    {
        internal PredicateGroupType PredicateGroupType { get; set; }
        internal LogicalOperator LogicalOperator { get; private set; }
        internal bool Sign { get; private set; }

        internal PredicateGroup(PredicateGroupType predicateGroupType, LogicalOperator logicalOperator, bool sign)
        {
            PredicateGroupType = predicateGroupType;
            LogicalOperator = logicalOperator;
            Sign = sign;
        }

        internal static void SetPredicateGroup(IPredicate node, PredicateGroupType predicateGroupType)
        {
            if (predicateGroupType == PredicateGroupType.Begin)
            {
                ++node.PredicateGroupLevel;
            }
            else
            {
                --node.PredicateGroupLevel;
            }

            if (node.PredicateGroup != null)
            {
                if (node.PredicateGroup.PredicateGroupType == predicateGroupType)
                {
                    ThrowInvalidPredicateGroupingException((DbNode)node);
                }
                else
                {
                    node.ResetPredicateGroup();
                    return;
                }
            }
            else if (node.PredicateGroupLevel > 1) 
            {
                ThrowInvalidPredicateGroupingException((DbNode)node);
            }

            LogicalOperator op = node.HasPredicate ? LogicalOperator.And : LogicalOperator.None;  // default
            bool sign = true;  
            if (node.HasOr)
            {
                op = Wall.LogicalOperator.Or;
                node.HasOr = false;    // reset
            }
            if (node.HasNot)
            {
                sign = false;
                node.HasNot = false;   // reset 
            }

            node.PredicateGroup = new PredicateGroup(predicateGroupType, op, sign);

            if (predicateGroupType == PredicateGroupType.End)
            {
                if (node.HasPredicate)
                {
                    if (node.Predicates[node.Predicates.Count - 1].PredicateGroup != null)
                    {
                        node.Predicates[node.Predicates.Count - 1].PredicateGroup.PredicateGroupType = PredicateGroupType.BeginEnd;
                    }
                    else
                    {
                        // non-first predicate takes the node's predicate ending group 
                        node.Predicates[node.Predicates.Count - 1].SetPredicateGroup(node.PredicateGroup);
                    }
                }

                node.ResetPredicateGroup();
            }
        }

        internal static void ThrowInvalidPredicateGroupingException(DbNode subject)
        {
            throw new QueryTalkException("PredicateGroup.ThrowInvalidPredicateGroupingException",
                QueryTalkExceptionType.InvalidPredicateGrouping, null, Text.Method.PredicateGrouping).SetObjectName(subject.Name);
        }

    }
}
