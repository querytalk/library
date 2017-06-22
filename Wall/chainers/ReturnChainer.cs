#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class ReturnChainer : EndChainer, IBegin 
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Return;
            }
        }

        // empty return
        internal ReturnChainer(Chainer prev) 
            : base(prev)
        {
            Build = (buildContext, buildArgs) =>
                {
                    return Text.GenerateSql(10)
                        .NewLine(GetRoot().IsEmbeddedTransaction ? Text.Free.ReturnWithCommit : Text.Return)
                        .Terminate()
                        .ToString();
                };
        }

        // return value
        internal ReturnChainer(Chainer prev, int returnValue)
            : base(prev)
        {
            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(40)
                    .NewLine(Text.Set).S()
                    .Append(Text.Reserved.ReturnValueInnerParam)
                    .Append(Text._Equal_)
                    .Append(returnValue)
                    .Terminate()
                    .NewLine(GetRoot().IsEmbeddedTransaction ? Text.Free.ReturnWithCommit : Text.Return)
                    .Terminate()
                    .ToString();
            };
        }

        // return variable
        internal ReturnChainer(Chainer prev, string returnVariable)
            : base(prev)
        {
            if (!Variable.TryValidateName(returnVariable, out chainException))
            {
                TryThrow();
            }

            var root = GetRoot();

            if (root.VariableExists(returnVariable) == false)
            {
                Throw(QueryTalkExceptionType.ParamOrVariableNotDeclared, ArgVal(() => returnVariable, returnVariable));
            }

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(40)
                    .NewLine(Text.Set).S()
                    .Append(Text.Reserved.ReturnValueInnerParam)
                    .Append(Text._Equal_)
                    .Append(returnVariable)
                    .Terminate()
                    .NewLine(GetRoot().IsEmbeddedTransaction ? Text.Free.ReturnWithCommit : Text.Return)
                    .Terminate()
                    .ToString();
            };
        }
    }
}
