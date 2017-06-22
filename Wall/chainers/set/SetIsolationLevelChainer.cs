#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public class SetIsolationLevelChainer : EndChainer, IBegin
    {
        internal SetIsolationLevelChainer(Chainer prev, Designer.IsolationLevel isolationLevel) 
            : base(prev)
        {
            Build = (buildContext, buildArgs) =>
                {
                    string isolation = isolationLevel.ToSql();
                    return Text.GenerateSql(50)
                        .NewLine(Text.SetTransactionIsolationLevel).S()
                        .Append(isolation)
                        .Terminate()
                        .ToString();
                };
        }
    }
}
