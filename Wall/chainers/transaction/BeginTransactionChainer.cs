#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class BeginTransactionChainer : EndChainer, IBegin 
    {
        internal override string Method 
        { 
            get 
            { 
                return Text.Method.BeginTransaction; 
            } 
        }

        internal BeginTransactionChainer(Chainer prev, string nameOrVariable = null) 
            : base(prev)
        {
            var root = GetRoot();
            if (!Common.CheckTransactionName(nameOrVariable, root, out chainException))
            {
                TryThrow();
            }

            ++root.TranCounter;

            Build = (buildContext, buildArgs) =>
                {
                    if (root.IsEmbeddedTransaction)
                    {
                        Throw(QueryTalkExceptionType.TransactionalInterference, null);
                    }

                    if (nameOrVariable == null)
                    {
                        return Text.GenerateSql(20)
                            .NewLine(Text.BeginTransaction)
                            .Terminate()
                            .ToString();
                    }
                    else
                    {
                        return Text.GenerateSql(40)
                            .NewLine(Text.BeginTransaction)
                            .S()
                            .Append(nameOrVariable)
                            .Terminate()
                            .ToString();
                    }
                };
        }
    }
}
