#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    // Represents the wrapper around a variable specifying that it should be passed as output variable.
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class OutputVar : Chainer, INonPredecessor,
        IAs
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Out;
            }
        }

        internal string Variable { get; private set; }

        internal string Value
        {
            get
            {
                return Build(null, null);
            }
        }

        internal OutputVar(string variable)
            : base(null)
        {
            CheckNullAndThrow(Arg(() => variable, variable));

            // check
            if (Common.CheckIdentifier(variable) != IdentifierValidity.Variable)
            {
                CreateException(QueryTalkExceptionType.InvalidVariableName, ArgVal(() => variable, variable));
            }

            Variable = variable;

            Build = (buildContext, buildArgs) =>
            {
                return variable;
            };
        }

        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return Variable;
        }

    }
}
