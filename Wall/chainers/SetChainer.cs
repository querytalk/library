#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Linq;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class SetChainer : EndChainer, IBegin, IStringAsValue, INonParameterizable
    {
        internal override string Method 
        { 
            get 
            { 
                return Text.Method.Set; 
            } 
        }

        // direct: If true then .Set method follows .Declare method.
        internal SetChainer(Chainer prev, string variableName, VariableArgument argument, bool direct = false)
            : base(prev)
        {
            var root = GetRoot();

            if (argument == null)
            {
                argument = Designer.Null;
            }

            if (direct && prev is DeclareChainer)
            {
                variableName = ((DeclareChainer)prev).VariableName;
            }

            CheckAndThrow(variableName, root, Method);
            TryThrow(argument.Exception);

            Build = (buildContext, buildArgs) =>
                {
                    var variable = buildContext.TryGetVariable(variableName, out chainException, Variable.SearchType.Variable);
                    if (chainException != null)
                    {
                        root.TryThrow(chainException, Method);
                    }
                    argument.DataType = variable.DataType;

                    return Text.GenerateSql(20)
                        .NewLine(Text.Set).S()
                        .Append(variableName)
                        .Append(Text._Equal_)
                        .Append(argument.Build(buildContext, buildArgs))
                        .Terminate()
                        .ToString();
                };
        }

        internal static void CheckAndThrow(string variable, Designer root, string method, string[] variables = null)
        {
            QueryTalkException exception;

            root.CheckNull(Chainer.Arg(() => variable, variable), method);
            if (root.chainException != null)
            {
                if (variables != null)
                {
                    root.chainException.Arguments = String.Format("variable name = null{0}   variables = ({1})",
                        Environment.NewLine,
                        String.Join(", ", variables.Select(v => v ?? Text.ClrNull)));
                }
                else
                {
                    root.chainException.Arguments = "variable name = null";
                }

                root.TryThrow(root.chainException, method);
            }

            bool check = Variable.TryValidateName(variable, out exception);
            root.TryThrow(exception, method);

            if (!check)
            {
                exception = new QueryTalkException("SetChainer.CheckAndThrow",
                    QueryTalkExceptionType.InvalidVariableName,
                    Chainer.ArgVal(() => variable, variable));
                root.TryThrow(exception, method);
            }

            if (root.VariableExists(variable) == false)
            {
                root.Throw(QueryTalkExceptionType.ParamOrVariableNotDeclared,
                    Chainer.ArgVal(() => variable, variable),
                    method);
            }
        }

    }
}
