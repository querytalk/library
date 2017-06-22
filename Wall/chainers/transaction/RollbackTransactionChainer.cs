#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class RollbackTransactionChainer : EndChainer, IBegin 
    {
        internal override string Method 
        { 
            get 
            { 
                return Text.Method.RollbackTransaction; 
            } 
        }

        internal RollbackTransactionChainer(Chainer prev, string nameOrVariable = null) 
            : base(prev)
        {
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

                    if (root.TranCounter == 0)
                    {
                        Throw(QueryTalkExceptionType.BeginTransactionMissing, null);
                    }

                    if (nameOrVariable == null)
                    {
                        return Text.GenerateSql(25)
                            .NewLine(Text.RollbackTransaction)
                            .Terminate()
                            .ToString();
                    }
                    else
                    {
                        return Text.GenerateSql(50)
                            .NewLine(Text.RollbackTransaction).S()
                            .Append(nameOrVariable)
                            .Terminate()
                            .ToString();
                    }
                };
        }
    }
}
