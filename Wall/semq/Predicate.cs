#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Represents a predicate with its attribute in the semantic querying. (Not intended for public use.)
    /// </summary>
    public sealed class Predicate : ISemantic
    {
        internal DbNode Subject { get; private set; }

        internal PredicateType PredicateType { get; private set; }

        internal bool Sign { get; private set; }

        internal LogicalOperator LogicalOperator { get; private set; }

        private Quantifier _quantifier;
        internal Quantifier Quantifier
        {
            get
            {
                return _quantifier;
            }
            private set
            {
                if (value == null)
                {
                    return;
                }

                if (value.QuantifierType == QuantifierType.None)
                {
                    Sign = !Sign;
                    PredicateType = Wall.PredicateType.Existential;  
                }
                // since AtLeastOne is the default quantifier of every predicate, it can be omitted
                else if (value.QuantifierType == QuantifierType.AtLeastOne)
                {
                    PredicateType = Wall.PredicateType.Existential;  
                }
                else
                {
                    _quantifier = value;
                }
            }
        }

        internal ISemantic Sentence { get; set; }

        internal bool HasExpression
        {
            get
            {
                return Sentence is Expression;
            }
        }

        internal Expression Expression
        {
            get
            {
                if (HasExpression)
                {
                    return (Expression)Sentence;
                }
                else
                {
                    return null;
                }
            }
        }

        internal bool IsFinalizeable { get; private set; }

        #region Maximized

        internal OrderingArgument MaximizedExpression { get; private set; }

        internal MaximizationType MaximizedType { get; private set; }

        internal long MaximizedTop { get; private set; }

        #endregion

        internal PredicateGroup PredicateGroup { get; private set; }

        // set predicate group (END backwards)
        internal void SetPredicateGroup(PredicateGroup predicateGroup)
        {
            // check Maximized
            if (PredicateType == Wall.PredicateType.Maximized)
            {
                throw new QueryTalkException("Predicate.SetPredicateGroup", QueryTalkExceptionType.InvalidHavingPredicatePosition, null,
                    Text.Method.HavingMaxMin);
            }

            PredicateGroup = predicateGroup;
        }

        internal bool IsDefault { get; set; }

        #region Constructors

        // ctor: Existential, Quantified, TopQuantified
        internal Predicate(
            DbNode subject,
            PredicateType predicateType,
            bool sign,                        
            LogicalOperator logicalOperator,  
            ISemantic sentence,
            Quantifier quantifier = null,
            bool isDefault = false  // default predicate is made by broken chain
            )
        {
            DbMapping.CheckNullAndThrow(subject);

            if (sentence == null)
            {
                throw new QueryTalkException("Predicate.ctor", QueryTalkExceptionType.ArgumentNull,
                    "predicate argument = null", "predicate method");
            }

            if (((IPredicate)subject).HasOr)
            {
                logicalOperator = Wall.LogicalOperator.Or;
                ((IPredicate)subject).HasOr = false;    // reset the flag - !
            }
            if (((IPredicate)subject).HasNot)
            {
                sign = false;
                ((IPredicate)subject).HasNot = false;   // reset the flag - !
            }

            Subject = subject;
            Sign = sign;
            LogicalOperator = logicalOperator;
            Sentence = sentence;    
            IsFinalizeable = true;
            IsDefault = isDefault;

            // set quantifier type:
            PredicateType = predicateType;
            if (quantifier != null && quantifier.QuantifierType == QuantifierType.Most)
            {
                if (sentence.Subject == null)
                {
                    string predicateExpression = Text.NotAvailable;
                    if (sentence is Expression)
                    {
                        predicateExpression = ((Expression)sentence).Build(BuildContext.CreateAdHoc(), new BuildArgs(null));
                    }

                    if (sentence is Expression)
                    {
                        Translate.ThrowInvalidQuantifierException(this, null, sentence, null, predicateExpression);
                    }

                    throw new QueryTalkException("Predicate.ctor", QueryTalkExceptionType.InvalidPredicate,
                        String.Format("predicate = {0}", predicateExpression), "predicate method");
                }

                PredicateType = Wall.PredicateType.TopQuantified;
                sentence.Subject.IsFinalizeable = false;
                IsFinalizeable = false;
            }

            SetPredicateGroup(((IPredicate)subject).PredicateGroup);
            ((IPredicate)subject).ResetPredicateGroup();
            Quantifier = quantifier;

            // pass parent
            sentence.RootSubject = Subject;
        }

        // ctor: Maximized
        internal Predicate(
            DbNode subject,
            MaximizationType maximizationType,
            bool sign,
            LogicalOperator logicalOperator,
            OrderingArgument maximizedExpression,
            long top = 1)
        {
            DbMapping.CheckNullAndThrow(subject);

            if (maximizedExpression == null)
            {
                throw new QueryTalkException("Predicate.ctor", QueryTalkExceptionType.ArgumentNull,
                    "maximized expression = null", Text.Method.HavingMaxMin);
            }

            // check expression DbColumn type
            if (maximizedExpression.ArgType == typeof(DbColumn))
            {
                var column = (DbColumn)maximizedExpression.Original;
                if (column.Parent != null && column.Parent.Prev != null)
                {
                    throw new QueryTalkException("Predicate.ctor", QueryTalkExceptionType.InvalidHavingPredicateArgument,
                        String.Format("node = {0}", column.Parent), Text.Method.HavingMaxMin);
                }

                // column parent must be the same as the subject
                if (!column.Parent.Equals(subject))
                {
                    throw new QueryTalkException("Predicate.ctor", QueryTalkExceptionType.InvalidHavingPredicateArgument,
                        String.Format("node = {0}{1}   subject = {2}", column.Parent, Environment.NewLine, subject),
                        Text.Method.HavingMaxMin);
                }

            }

            Subject = subject;
            Sign = sign;
            LogicalOperator = logicalOperator;
            PredicateType = Wall.PredicateType.Maximized;
            MaximizedType = maximizationType;
            MaximizedExpression = maximizedExpression;
            MaximizedTop = top;
            IsFinalizeable = true;
            subject.IsFinalizeable = true;
        }

        #endregion

        #region ISemantic

        DbNode ISemantic.Subject
        {
            get
            {
                if (Sentence == null)
                {
                    return null;
                }

                var subject = ((ISemantic)Sentence).Subject;

                // if a predicate is an expression, then move the expression from the subject to the last node in the sentence node chain
                if (HasExpression && subject != null)
                {
                    var last = subject.GetLast();
                    last.Expression = Expression;

                    // convert the "expression sentence" of a predicate into "a node sentence"
                    Sentence = (ISemantic)subject;
                }

                return subject;
            }
        }

        DbNode ISemantic.RootSubject { get; set; }

        bool ISemantic.IsQuery
        {
            get
            {
                return true;
            }
        }

        Chainer ISemantic.Translate(SemqContext context, DbNode predecessor)
        {
            var innerNode = ((ISemantic)this).Subject;
            return ((ISemantic)innerNode).Translate(context, predecessor);
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("{0}:{1}", Subject, (Sentence is DbNode) ? ((DbNode)Sentence).Root.ToString() : "(expr)");
        }

        #endregion

    }
}
