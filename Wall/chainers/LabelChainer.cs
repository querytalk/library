#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class LabelChainer : EndChainer, IBegin 
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Label;
            }
        }

        internal LabelChainer(Chainer prev, string label) 
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => label, label));

            if (Common.CheckIdentifier(label) != IdentifierValidity.RegularIdentifier)
            {
               Throw(QueryTalkExceptionType.InvalidLabel, ArgVal(() => label, label));
            }

            GetRoot().TryAddLabelOrThrow(label);

            Build = (buildContext, buildArgs) =>
                {
                    return Text.GenerateSql(20)
                        .NewLine(label)
                        .Append(Text.SemiColon).S()
                        .ToString();
                };
        }
    }
}
