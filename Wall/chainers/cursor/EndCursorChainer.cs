#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class EndCursorChainer : EndChainer, IBegin
    {
        internal override string Method
        {
            get
            {
                return Text.Method.EndCursor;
            }
        }

        internal EndCursorChainer(Chainer prev) 
            : base(prev)
        {
            var root = GetRoot();
            --root.CursorCounter;
            var cursor = BeginCursorChainer.TryGetBeginCursor(this);

            CheckAndThrow();

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(50)
                    .NewLine(Text.End).Terminate()
                    .NewLine(Text.Close).S().Append(cursor.CursorName).Terminate()
                    .NewLine(Text.Deallocate).S().Append(cursor.CursorName).Terminate()
                    .ToString();
            };
        }

        // check if exists the FetchNext object between EndCursor and BeginCursor object
        private void CheckAndThrow()
        {
            var node = Prev;
            while (node != null && !(node is BeginCursorChainer))
            {
                if (node is BeginCursorFetchNextChainer)
                {
                    return;
                }
                node = node.Prev;
            }

            Throw(QueryTalkExceptionType.MissingFetchNextMethod, null, Text.Method.EndCursor);
        }
    }
}
