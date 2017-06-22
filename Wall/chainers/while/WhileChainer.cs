#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class WhileChainer : EndChainer, IBegin
    {
        internal override string Method
        {
            get
            {
                return Text.Method.While;
            }
        }

        internal WhileChainer(Chainer prev, ScalarArgument argument1, ValueScalarArgument argument2, bool equality)
            : this(prev, Expression.EqualitySimplifier(argument1, argument2, equality))
        {
            CheckNullAndThrow(Arg(() => argument1, argument1));
            TryTake(argument1);
            TryTake(argument2); 
        }

        internal WhileChainer(Chainer prev, Expression expression) 
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => expression, expression));

            ++GetRoot().WhileCounter;

            Build = (buildContext, buildArgs) =>
                {
                    var sql = Text.GenerateSql(100)
                        .NewLine(Text.While).S()
                        .Append(expression.Build(buildContext, buildArgs))
                        .NewLine(Text.Begin).S()
                        .ToString();

                    buildContext.TryTakeException(expression.Exception);
                    TryThrow(buildContext);

                    return sql;
                };
        }
    }
}
