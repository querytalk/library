#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public abstract class ConditionChainer : Chainer, IQuery, INonSelectView
    {
        internal override string Method
        {
            get
            {
                return Text.Method.OnOrWhere;
            }
        }

        internal PredicateGroup PredicateGroup { get; private set; }

        internal ConditionChainer(Chainer prev, PredicateGroup predicateGroup)
            : base(prev)
        {
            PredicateGroup = predicateGroup;
        }

        internal ConditionChainer(Chainer prev, Expression expression, 
            PredicateGroup predicateGroup = null) 
            : this(prev, predicateGroup)
        {
            CheckNullAndThrow(Arg(() => expression, expression));

            TryTake(expression);
            TryThrow();

            Query.AddArguments(expression.Arguments);

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(50)
                    .NewLine(predicateGroup.Build(PredicateGroupType.Begin, Keyword)).S()
                    .Append(prev.ExpressionGroupType.IsBegin() ? Text.LeftBracket : null)
                    .Append(expression.Build(buildContext, buildArgs))
                    .Append(prev.ExpressionGroupType.IsEnd() ? Text.RightBracket : null)
                    .Append(predicateGroup.Build(PredicateGroupType.End, null))
                    .S();

                buildContext.TryTakeException(expression.Exception);
                TryThrow(buildContext);

                return sql.ToString();
            };
        }

        internal ConditionChainer(Chainer prev, ScalarArgument argument1, ScalarArgument argument2, bool equality,
            PredicateGroup predicateGroup = null) 
            : this(prev, predicateGroup)
        {
            CheckNullAndThrow(Arg(() => argument1, argument1));

            TryTake(argument1);
            TryTake(argument2);

            var expression = Expression.EqualitySimplifier(argument1, argument2, equality);
            Query.AddArguments(expression.Arguments);

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(50)
                    .NewLine(predicateGroup.Build(PredicateGroupType.Begin, Keyword)).S()
                    .Append(prev.ExpressionGroupType.IsBegin() ? Text.LeftBracket : null)
                    .Append(expression.Build(buildContext, buildArgs))
                    .Append(prev.ExpressionGroupType.IsEnd() ? Text.RightBracket : null)
                    .Append(predicateGroup.Build(PredicateGroupType.End, null))
                    .S();

                buildContext.TryTakeException(expression.Exception);
                TryThrow(buildContext);

                return sql.ToString();
            };
        }

        // a ctor for .WhereExists support
        internal ConditionChainer(Chainer prev, INonSelectView nonSelectView, bool exists,
            PredicateGroup predicateGroup = null) 
            : this(prev, predicateGroup)
        {
            CheckNullAndThrow(Arg(() => nonSelectView, nonSelectView));

            SelectChainer select = new SelectChainer((Chainer)nonSelectView, new Column[] { Designer.Null }, false);
            var view = new View(select, Query);

            Query.AddArguments(view.Query.Arguments.ToArray());

            Build = (buildContext, buildArgs) =>
            {
                string keyword = exists ? Text.Exists : Text.NotExists;

                var sql = Text.GenerateSql(100)
                    .NewLine(predicateGroup.Build(PredicateGroupType.Begin, Keyword)).S()
                    .Append(prev.ExpressionGroupType.IsBegin() ? Text.LeftBracket : null)
                    .Append(keyword).S()
                    .EncloseLeft()
                    .Append(view.Build(buildContext, buildArgs))
                    .EncloseRight()
                    .Append(prev.ExpressionGroupType.IsEnd() ? Text.RightBracket : null)
                    .Append(predicateGroup.Build(PredicateGroupType.End, null))
                    .S();

                TryThrow(buildContext);

                return sql.ToString();
            };  
        }

    }
}
