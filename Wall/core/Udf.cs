#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    // Represents a user-defined function.
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class Udf : Chainer, INonPredecessor,
        IScalar,
        IAs
    {
        internal override string Method
        {
            get
            {
                return Text.Method.AsUdf;
            }
        }

        private Udf(Chainer prev)
            : base(null)
        { }

        internal Udf(Table table, FunctionArgument[] arguments)
            : this(null)
        {
            Build = (buildContext, buildArgs) =>
            {
                if (arguments == null)
                {
                    arguments = new FunctionArgument[] { Designer.Null };
                }

                foreach (var argument in arguments)
                {
                    if (!argument.IsNullReference() && argument.Original is View)
                    {
                        var arg = ((View)argument.Original).Parameterize(buildContext);
                        argument.SetStringBuildMethod(arg);
                        argument.SetArgType(arg);
                    }
                }

                var sql = Text.GenerateSql(50)
                    .Append(table.Build(buildContext, buildArgs))
                    .Append(Text.LeftBracket)
                    .Append(FunctionArgument.Concatenate(arguments, buildContext, buildArgs))
                    .Append(Text.RightBracket)
                    .ToString();

                TryThrow(buildContext);

                return sql;
            };
        }

    }
}
