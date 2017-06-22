#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class BeginCursorFetchNextChainer : EndChainer, IBegin
    {
        internal override string Method
        {
            get
            {
                return Text.Method.BeginCursorFetchNext;
            }
        }

        internal BeginCursorFetchNextChainer(Chainer prev)
            : base(prev)
        {
            var cursor = BeginCursorChainer.TryGetBeginCursor(this);

            Build = (buildContext, buildArgs) =>
                {
                    return Text.GenerateSql(100)
                        .NewLine(Text.FetchNext).S()
                        .Append(Text.From).S()
                        .Append(cursor.CursorName).S()
                        .Append(Text.Into).S()
                        .Append(cursor.Variables).Terminate()
                        .ToString();
                };
        }

    }
}
