#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class IfChainer : EndChainer, IBegin
    {
        internal override string Method
        {
            get
            {
                return chainMethod;
            }
        }

        internal IfChainer(Chainer prev, ScalarArgument argument1, ValueScalarArgument argument2, bool equality)
            : this(prev, Expression.EqualitySimplifier(argument1, argument2, equality))
        {
            CheckNullAndThrow(Arg(() => argument1, argument1));
            TryTake(argument1);
            TryTake(argument2); 
        }

        internal IfChainer(Chainer prev, Expression expression) 
            : base(prev) 
        {
            chainMethod = Text.Method.If;
            CheckNullAndThrow(Arg(() => expression, expression));
            ++GetRoot().IfCounter;

            Build = (buildContext, buildArgs) =>
                {
                    var sql = Text.GenerateSql(50)
                        .NewLine(Text.If).S()
                        .Append(expression.Build(buildContext, buildArgs)).S()
                        .NewLine(Text.Begin)
                        .ToString();

                    buildContext.TryTakeException(expression.Exception);
                    TryThrow(buildContext);

                    return sql;
                };
        }

        #region If(Not)Exists

        internal IfChainer(Chainer prev, View view, bool sign)
            : base(prev)
        {
            chainMethod = Text.Method.IfExists;
            CheckNullAndThrow(Arg(() => view, view));
            ++GetRoot().IfCounter;
            Build = BuildViewMethod(view, sign);
        }

        internal IfChainer(Chainer prev, IOpenView openView, bool sign)
            : base(prev)
        {
            chainMethod = Text.Method.IfExists;
            CheckNullAndThrow(Arg(() => openView, openView));

            IOpenView query;
            if (openView is ISemantic)
            {
                query = DbMapping.SelectFromSemantic((ISemantic)openView);
            }
            else
            {
                query = openView;
            }

            var view = new View(query);

            if (Exception != null)
            {
                Exception.Extra = String.Format("Check {0} method.", Exception.Method);
                TryThrow();
            }

            ++GetRoot().IfCounter;
            Build = BuildViewMethod(view, sign);
        }

        internal IfChainer(Chainer prev, INonSelectView nonSelectView, bool sign)
            : base(prev)
        {
            chainMethod = Text.Method.IfExists;
            CheckNullAndThrow(Arg(() => nonSelectView, nonSelectView));

            SelectChainer select = new SelectChainer((Chainer)nonSelectView, new Column[] { Designer.Null }, false);
            var view = new View(select, select.Query);

            ++GetRoot().IfCounter;

            Build = BuildViewMethod(view, sign);
        }

        #endregion

        private Func<BuildContext, BuildArgs, string> BuildViewMethod(View view, bool sign)
        {
            return (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(50)
                    .NewLine(Text.If).S().Append(sign ? null : Text.Not_)
                        .Append(Text.Exists).S()
                    .Append(Text.LeftBracket)
                        .Append(view.Build(buildContext, buildArgs))
                    .Append(Text.RightBracket)
                    .NewLine(Text.Begin)
                    .ToString();

                TryThrow(buildContext);

                return sql;
            };
        }
    }
}
