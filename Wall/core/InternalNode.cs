#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace QueryTalk.Wall
{
    internal class InternalNode : DbNode, ISemantic, IPredicate, IRelation, IOpenView
    {
        private List<Predicate> _predicates = new List<Predicate>();

        #region Constructors

        internal InternalNode(DbNode subject)
            : base((DbNode)null)
        {
            NodeID = subject.NodeID;  
        }

        internal InternalNode(DB3 id)
            : base((DbNode)null)
        {
            NodeID = id;
        }

        #endregion

        #region IPredicate

        List<Predicate> IPredicate.Predicates 
        {
            get
            {
                return _predicates;
            }
        }

        Predicate IPredicate.Predicate { get; set; }

        bool IPredicate.HasNot { get; set; }

        bool IPredicate.HasOr { get; set; }

        void IPredicate.AddPredicate(Predicate predicate)
        {
            _predicates.Add(predicate);
        }

        void IPredicate.AddPredicate(PredicateType predicateType, DbNode sentence)
        {
            _predicates.Add(new Predicate(this, predicateType, true, LogicalOperator.And, (ISemantic)sentence));
        }

        bool IPredicate.HasPredicate
        {
            get
            {
                return _predicates.Count > 0;
            }
        }

        PredicateGroup IPredicate.PredicateGroup { get; set; }

        void IPredicate.SetPredicateGroup(PredicateGroupType predicateGroupType)
        {
            PredicateGroup.SetPredicateGroup(this, predicateGroupType);
        }

        void IPredicate.ResetPredicateGroup()
        {
            ((IPredicate)this).PredicateGroup = null;
        }

        int IPredicate.PredicateGroupLevel { get; set; }

        #endregion

        #region IRelation

        DB3 IRelation.FK { get; set; }
        
        #endregion

        #region ISemantic

        bool ISemantic.IsQuery
        {
            get
            {
                return Prev != null
                    || Next != null
                    || ((IPredicate)this).HasPredicate;
            }
        }

        DbNode ISemantic.Subject
        {
            get
            {
                return this;
            }
        }

        DbNode ISemantic.RootSubject { get; set; }

        Chainer ISemantic.Translate(SemqContext context, DbNode predecessor)
        {
            if (context == null)
            {
                throw new QueryTalkException(this, QueryTalkExceptionType.NullArgumentInnerException,
                    "context = null", "InternalNode.ISemantic.Translate");
            }

            if (predecessor == null)
            {
                throw new QueryTalkException(this, QueryTalkExceptionType.NullArgumentInnerException,
                    "predecessor = null", "InternalNode.ISemantic.Translate");
            }

            context.SetIndex(this);

            var from = GetDesigner().From(new Table(Map.Name, new AliasAs(Index)));

            if (!((IPredicate)this).HasPredicate)
            {
                return from;
            }

            var predicate = ((IPredicate)this).Predicates[0];
            var relation = DbMapping.GetRelation(predecessor.SynonymOrThis.NodeID, NodeID, DB3.Default);
            var relationExpression = relation.BuildRelation(predecessor.SynonymOrThis.NodeID, predecessor.Index, Index).E();

            if (predicate.PredicateType == PredicateType.Cartesian)
            {
                var innerNode = ((ISemantic)predicate).Subject;
                context.SetIndex(innerNode);               
                innerNode.QueryRoot = context.Subject;

                var innerJoinAs = from
                    .InnerJoin((IOpenView)innerNode)
                    .As(innerNode.Index);

                var joinRelation = DbMapping.GetRelation(NodeID, innerNode.SynonymOrThis.NodeID, DB3.Default);
                var joinExpression = joinRelation.BuildRelation(NodeID, Index, innerNode.Index).E(); 
                var on = innerJoinAs   
                    .On(joinExpression);

                if (IsFinalizeable)
                {
                    return on.Where(relationExpression);
                }
                else
                {
                    return on;
                }
            }
            else
            {
                return (Chainer)from.WhereExists(
                    (INonSelectView)
                        ((ISemantic)predicate).Translate(context, this), true)
                    .AndWhere(relationExpression, true);
            }
        }

        #endregion

        #region Not browsable

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType()
        {
            return base.GetType();
        }

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool Equals(object objA, object objB)
        {
            return Equals(objA, objB);
        }

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool ReferenceEquals(object objA, object objB)
        {
            return ReferenceEquals(objA, objB);
        }

        #endregion

    }
}
