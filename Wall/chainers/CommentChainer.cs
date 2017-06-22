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
    public sealed class CommentChainer : EndChainer, IBegin
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Comment;
            }
        }

        internal CommentChainer(Chainer prev, string comment) 
            : base(prev)
        {
            comment = comment ?? String.Empty;

            if (comment.Length > 100)
            {
                Throw(QueryTalkExceptionType.CommentTooLong, String.Format("comment = {0}...", comment.Substring(0, 30)));
            }

            Build = (buildContext, buildArgs) =>
                {
                    if (!String.IsNullOrEmpty(comment))
                    {
                        return Text.GenerateSql(30)
                            .NewLine()
                            .NewLine(Text.TwoHyphens).S()
                            .Append(comment)
                            .NewLine()
                            .ToString();
                    }
                    // empty line
                    else
                    {
                        return Text.GenerateSql(10)
                            .NewLine()
                            .ToString();
                    }
                };
        }
    }
}
