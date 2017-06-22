#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class SaveTransactionChainer : EndChainer, IBegin
    {
        internal override string Method 
        { 
            get 
            { 
                return Text.Method.SaveTransaction; 
            } 
        }

        internal SaveTransactionChainer(Chainer prev, string nameOrVariable) 
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => nameOrVariable, nameOrVariable));

            var root = GetRoot();
            if (!Common.CheckTransactionName(nameOrVariable, root, out chainException))
            {
                TryThrow();
            }

            Build = (buildContext, buildArgs) =>
                {
                    if (root.IsEmbeddedTransaction)
                    {
                        Throw(QueryTalkExceptionType.TransactionalInterference, null);
                    }

                    return Text.GenerateSql(50)
                        .NewLine(Text.SaveTransaction).S()
                        .Append(nameOrVariable)
                        .Terminate()
                        .ToString();
                };
        }
    }
}
