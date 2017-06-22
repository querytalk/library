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
    public sealed class Concatenator : Chainer, INonPredecessor,
        IAs
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Concat;
            }
        }

        internal Variable Variable { get; private set; }

        internal Concatenator(Variable variable)
            : base(null)
        {
            var variableName = variable.Name;
            CheckNullAndThrow(Arg(() => variableName, variable));

            Variable = variable;

            Build = (buildContext, buildArgs) =>
            {
                string concatVar = variable.IsConcatenator() ?
                    variable.Name : String.Format(Text.Free.CastConcatenator, variable.Name);

                return Text.GenerateSql(30)
                    .Append(Text.SingleQuote)
                    .Append(Text._Plus_)
                    .Append(concatVar)
                    .Append(Text._Plus_)
                    .Append(Text.SingleQuote)
                    .ToString();
            };
        }

    }
}
