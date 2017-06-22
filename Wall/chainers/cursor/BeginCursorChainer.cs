#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class BeginCursorChainer : Chainer, IBegin, 
        IBeginCursorWithVariables
    {
        internal override string Method
        {
            get
            {
                return Text.Method.BeginCursor;
            }
        }

        internal string CursorName { get; private set; }

        private string _variables;
        internal string Variables 
        {
            get
            {
                return _variables;
            }
        }
        internal void TrySetVariables(string[] variables)
        {
            // check variable names
            Array.ForEach(variables, variable =>
                {
                    SetChainer.CheckAndThrow(variable, GetRoot(), Text.Method.BeginCursorWithVars);
                });

            _variables = String.Join(Text.Comma, variables);
        }

        internal BeginCursorChainer(Chainer prev, View view)
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => view, view));
            view.TryThrow(Method);

            var root = GetRoot();

            CheckNullAndThrow(Arg(() => view, view));

            CursorName = String.Format("{0}{1}", 
                Text.Reserved.CursorNameBody, root.GetUniqueIndex());

            ++root.CursorCounter;

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(500)
                    .NewLine(Text.Declare).S().Append(CursorName).S().Append(Text.CursorLocalFastForward).S()
                    .NewLine(Text.For).S()
                    .Append(view.Build(buildContext, buildArgs)).Terminate()
                    .NewLine(Text.Open).S().Append(CursorName).Terminate()
                    .NewLine(Text.FetchNext).S().Append(Text.From).S().Append(CursorName).S()
                    .Append(Text.Into).S().Append(_variables).Terminate()
                    .NewLine(Text.Free.WhileFetchStatus).S()
                    .NewLine(Text.Begin).S()
                    .ToString();
            };
        }

        internal static BeginCursorChainer TryGetBeginCursor(Chainer fetchOrEndCursorObject)
        {
            var isFetchNext = fetchOrEndCursorObject is BeginCursorFetchNextChainer;
            int skipCounter = 0;

            // anticipated: 
            //   fetchOrEndCursorObject should not be null
            var node = fetchOrEndCursorObject;
            while (true)
            {
                node = node.Prev;
                if (node == null)
                {
                    throw new QueryTalkException("CBeginCursor.TryGetBeginCursor", QueryTalkExceptionType.InvalidCursorBlock,
                        null, isFetchNext ? Text.Method.BeginCursorFetchNext : Text.Method.EndCursor)
                            .SetObjectName(fetchOrEndCursorObject.GetRoot().Name);
                }

                if (node is EndCursorChainer)
                {
                    ++skipCounter;
                    continue;
                }

                if (node is BeginCursorChainer)
                {
                    if (skipCounter > 0)
                    {
                        --skipCounter;
                        continue;
                    }

                    // match
                    return (BeginCursorChainer)node;
                }
            }
        }

    }
}
