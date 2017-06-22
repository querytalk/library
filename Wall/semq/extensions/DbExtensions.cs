#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        private static T AddPredicate<T>(T node, ISemantic sentence, bool sign, Quantifier quantifier = null)
            where T : ISemantic
        {
            if (quantifier == null)
            {
                ((IPredicate)node).AddPredicate(
                    new Predicate((DbNode)(object)node, PredicateType.Existential, sign, LogicalOperator.And, sentence));
            }
            else
            {
                ((IPredicate)node).AddPredicate(
                    new Predicate((DbNode)(object)node, PredicateType.Quantified, sign, LogicalOperator.And, sentence, quantifier));
            }

            return node;
        }

        private static T AddPredicate<T>(T node, Expression expression, bool sign, Quantifier quantifier = null)
            where T : ISemantic
        {
            if (quantifier == null)
            {
                ((IPredicate)node).AddPredicate(
                    new Predicate((DbNode)(object)node, PredicateType.Existential, sign, LogicalOperator.And, expression));
            }
            else
            {
                ((IPredicate)node).AddPredicate(
                    new Predicate((DbNode)(object)node, PredicateType.Quantified, sign, LogicalOperator.And, expression, quantifier));
            }

            return node;
        }

        private static T AddPredicate<T>(T node, ScalarArgument arg1, ValueScalarArgument arg2, bool sign, Quantifier quantifier = null)
            where T : ISemantic
        {
            if (quantifier == null)
            {
                ((IPredicate)node).AddPredicate(
                    new Predicate((DbNode)(object)node, PredicateType.Existential, true, LogicalOperator.And,
                        Expression.EqualitySimplifier(arg1, arg2, sign)));
            }
            else
            {
                ((IPredicate)node).AddPredicate(
                    new Predicate((DbNode)(object)node, PredicateType.Quantified, true, LogicalOperator.And,
                        Expression.EqualitySimplifier(arg1, arg2, sign), quantifier));
            }

            return node;
        }

        internal static WhereChainer Where(this IWhere prev, Expression condition, bool sign, PredicateGroup predicateGroup = null)
        {
            if (sign)
            {
                return new WhereChainer((Chainer)prev, condition, predicateGroup);
            }
            else
            {
                return new WhereChainer((Chainer)prev, condition.Not(), predicateGroup);
            }
        }

        internal static WhereChainer WhereExists(this IWhere prev, INonSelectView nonSelectView, bool sign,
            PredicateGroup predicateGroup = null)
        {
            return new WhereChainer((Chainer)prev, nonSelectView, sign, predicateGroup);
        }

        internal static ConditionChainer WhereExists(this ConditionChainer prev, INonSelectView nonSelectView, bool sign, Predicate predicate)
        {
            if (predicate.LogicalOperator == LogicalOperator.And)
            {
                return new WhereAndChainer((Chainer)prev, nonSelectView, sign, predicate.PredicateGroup);
            }
            else
            {
                return new WhereOrChainer((Chainer)prev, nonSelectView, sign, predicate.PredicateGroup);
            }
        }

        internal static WhereChainer WhereQuantified(this IWhere prev, INonSelectView nonSelectView, bool sign, Predicate predicate)
        {
            var expression = predicate.Quantifier
                .CreateExpression(((ISelect)nonSelectView).SelectCountBig().EndView());
            return Where(prev, expression, sign, predicate.PredicateGroup);
        }

        internal static ConditionChainer WhereQuantified(this ConditionChainer prev, INonSelectView nonSelectView, bool sign, Predicate predicate)
        {
            var expression = predicate.Quantifier
                .CreateExpression(((ISelect)nonSelectView).SelectCountBig().EndView());

            if (predicate.LogicalOperator == LogicalOperator.And)
            {
                return AndWhere(prev, expression, sign, predicate.PredicateGroup);
            }
            else
            {
                return OrWhere(prev, expression, sign, predicate.PredicateGroup);
            }
        }

        internal static WhereAndChainer AndWhere(this ConditionChainer prev, Expression condition, bool sign, PredicateGroup predicateGroup = null)
        {
            if (sign)
            {
                return new WhereAndChainer((Chainer)prev, condition, predicateGroup);
            }
            else
            {
                return new WhereAndChainer((Chainer)prev, condition.Not(), predicateGroup);
            }
        }

        internal static WhereOrChainer OrWhere(this ConditionChainer prev, Expression condition, bool sign, PredicateGroup predicateGroup = null)
        {
            if (sign)
            {
                return new WhereOrChainer((Chainer)prev, condition, predicateGroup);
            }
            else
            {
                return new WhereOrChainer((Chainer)prev, condition.Not(), predicateGroup);
            }
        }

        internal static SelectChainer SelectCountBig(this ISelect prev)
        {
            return new SelectChainer((Chainer)prev, new Column[] { Designer.CountBig() }, false);
        }

        internal static DB3 GetFK(this DbNode table)
        {
            return ((IRelation)table).FK;
        }

        internal static ConnectBy GetConnectBy(this DbRow row)
        {
            var connectionKey = ((IConnectable)row).ConnectionKey;
            if (connectionKey != null)
            {
                row.CheckLoadableAndThrow(null);  
                var connectBy = new ConnectBy(row.Node.Mapper.GetRoot(), connectionKey);
                ((IConnectable)row).ResetConnectionKey();
                return connectBy;
            }

            return null;
        }

    }
}
