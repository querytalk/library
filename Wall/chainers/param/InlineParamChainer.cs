#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class InlineParamChainer : ParamChainer, ISnippetDisallowed,
        IParam,
        IAny,
        IFrom,
        ICollect
    {
        internal override string Method
        {
            get
            {
                return Text.Method.InlineParam;
            }
        }

        internal InlineParamChainer(Chainer prev, string paramName, DT inlineType)
            : base(prev, paramName, inlineType)
        {
            if (chainException == null)
            {
                var root = GetRoot();
                int ordinal = root.AllParams.Count;
                Param = new Variable(ordinal, paramName, inlineType, IdentifierType.Param);
                GetRoot().TryAddParamOrThrow(Param, false);
            }
        }
    }
}
