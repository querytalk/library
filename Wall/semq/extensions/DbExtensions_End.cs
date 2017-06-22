#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {
        private static Chainer _Translate(DbNode node)
        {
            var subject = node.Root;
            var context = new SemqContext(subject);
            var query = ((ISemantic)subject).Translate(context, null);
            return ((ISelect)query).Select().Top(subject.Top, 0);
        }

        /// <summary>
        /// Ends the semantic sentence creating the Procedure object.
        /// </summary>
        /// <param name="sentence">A semantic sentence.</param>
        public static Procedure EndProc(this ISemantic sentence)
        {
            var query = _Translate((DbNode)sentence);
            var proc = ((IEndProc)query).EndProcInternal();
            ((IConnectable)sentence).ResetConnectionKey();
            return proc;
        }

        /// <summary>
        /// Ends the semantic sentence creating the View object.
        /// </summary>
        /// <param name="sentence">A semantic sentence.</param>
        public static View EndView(this ISemantic sentence)
        {
            var query = _Translate((DbNode)sentence);
            return ((IEndView)query).EndView();
        }

        /// <summary>
        /// Ends the semantic sentence creating the Snippet object.
        /// </summary>
        /// <param name="sentence">A semantic sentence.</param>
        public static Snippet EndSnip(this ISemantic sentence)
        {
            var query = _Translate((DbNode)sentence);
            return ((IEndProc)query).EndSnip();
        }

    }
}
