#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class RaiserrorChainer : EndChainer, IBegin, INonParameterizable
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Raiserror;
            }
        }

        internal RaiserrorChainer(Chainer prev, string message, int severity, byte state) 
            : base(prev)
        {
            var root = GetRoot();
            var message2 = message;

            CheckNullAndThrow(Arg(() => message, message));

            if (Common.CheckIdentifier(message) == IdentifierValidity.Variable)
            {
                SetChainer.CheckAndThrow(message, root, Method);
            }
            else
            {
                message2 = Filter.DelimitQuoteNonAsterix(message);
            }

            Build = (buildContext, buildArgs) =>
                {
                    return Text.GenerateSql(100)
                        .NewLine(Text.Raiserror)
                        .Append(Text.LeftBracket)
                        .Append(message2)
                        .Append(Text.Comma).S()
                        .Append(severity)
                        .Append(Text.Comma).S()
                        .Append(state)
                        .Append(Text.RightBracket)
                        .Terminate()
                        .ToString();
                };
        }
    }
}
