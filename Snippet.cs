#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Diagnostics;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents a snippet object which becomes a part of a procedure after the placement into the procedure body using .Inject method. (This class has no public constructor.)
    /// </summary>
    [DebuggerTypeProxy(typeof(Wall.SnippetDebuggerProxy))]  
    public sealed class Snippet : Compilable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal override string Method
        {
            get
            {
                return Text.Method.EndSnippet;
            }
        }

        internal Snippet(Chainer prev, bool isInternal = false)
            : base(prev, ObjectType.Snippet, isInternal)
        {
            CheckAndThrow();

            compiled = false;

            Build = (buildContext, buildArgs) =>
            {
                // build with own build context but take ParamRoot and exec context (buildArgs) from the caller
                return BuildChain(new BuildContext(this, buildContext.ParamRoot), buildArgs);
            };
        }

        private void CheckAndThrow()
        {
            var root = GetRoot();

            if (root.HasParams)
            {
                Throw(QueryTalkExceptionType.InvalidSnippet, null, Text.Method.Param);
            }

            if (root.IfCounter != 0)
            {
                Throw(QueryTalkExceptionType.InvalidIfBlock, null, Text.Method.If);
            }

            if (root.WhileCounter != 0)
            {
                Throw(QueryTalkExceptionType.InvalidWhileBlock, null, Text.Method.While);
            }

            if (root.TryCatchCounter != 0)
            {
                Throw(QueryTalkExceptionType.InvalidTryCatchBlock, null, Text.Method.TryCatch);
            }

            if (root.CursorCounter != 0)
            {
                Throw(QueryTalkExceptionType.InvalidCursorBlock, null, Text.Method.EndCursor);
            }
        }

    }
}
