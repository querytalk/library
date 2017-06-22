#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>  
    public sealed class CommitTransactionChainer : EndChainer, IBegin 
    {
        internal override string Method 
        { 
            get 
            { 
                return Text.Method.CommitTransaction; 
            } 
        }

        internal CommitTransactionChainer(Chainer prev, string nameOrVariable = null) 
            : base(prev)
        {
            var root = GetRoot();
            if (!Common.CheckTransactionName(nameOrVariable, root, out chainException))
            {
                TryThrow();
            }

            if (root.TranCounter == 0)
            {
                Throw(QueryTalkExceptionType.BeginTransactionMissing, null);
            }

            Build = (buildContext, buildArgs) =>
                {
                    if (root.IsEmbeddedTransaction)
                    {
                        Throw(QueryTalkExceptionType.TransactionalInterference, null);
                    }

                    if (nameOrVariable == null)
                    {
                        return Text.GenerateSql(25)
                            .NewLine(Text.CommitTransaction)
                            .Terminate()
                            .ToString();
                    }
                    else
                    {
                        return Text.GenerateSql(50)
                            .NewLine(Text.CommitTransaction).S()
                            .Append(nameOrVariable)
                            .Terminate()
                            .ToString();
                    }
                };
        }
    }
}
