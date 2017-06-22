#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class OrderByChainer : EndChainer, IQuery, IViewAllowed, IOpenView, 
        ISelect
    {
        internal override string Method
        {
            get
            {
                return Text.Method.OrderBy;
            }
        }

        internal OrderByChainer(Chainer prev, OrderingArgument[] columns)
            : base(prev)
        {
            _Body(columns);
        }

        internal OrderByChainer(ISemantic prev, OrderingArgument[] columns)
            : base(prev.Translate(new SemqContext(((DbNode)prev).Root), null))
        {
            _Body(columns);
        }

        private void _Body(OrderingArgument[] columns)
        {
            Query.Clause.OrderBy = this;

            CheckNullOrEmptyAndThrow(Argc(() => columns, columns));

            Query.AddArguments(columns);

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(100)
                    .NewLine(Text.OrderBy).S()
                    .Append(OrderingArgument.Concatenate(columns, buildContext, buildArgs, false))
                    .ToString();

                TryThrow(buildContext);

                return sql;
            };
        }
    }
}
