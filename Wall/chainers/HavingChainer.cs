#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class HavingChainer : Chainer, IQuery, INonSelectView, IViewAllowed, 
        ISelect,
        IOrderBy,
        IDelete
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Having;
            }
        }

        internal HavingChainer(Chainer prev, ScalarArgument argument1, ScalarArgument argument2, bool equality)
            : this(prev, Expression.EqualitySimplifier(argument1, argument2, equality))
        {
            Query.Clause.Having = this;

            CheckNullAndThrow(Arg(() => argument1, argument1));

            Query.AddArgument(argument1);
            Query.AddArgument(argument2);
            TryTake(argument1);
            TryTake(argument2); 
        }

        internal HavingChainer(Chainer prev, Expression expression) 
            : base(prev)
        {
            Query.Clause.Having = this;

            CheckNullAndThrow(Arg(() => expression, expression));

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(50)
                    .NewLine(Text.Having).S()
                    .Append(expression.Build(buildContext, buildArgs))
                    .ToString();

                buildContext.TryTakeException(expression.Exception);
                TryThrow(buildContext);

                return sql;
            };
        }
    }
}
