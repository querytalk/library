#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class ParamNotNullChainer : EndChainer, ISnippetDisallowed,
        IParamOutput,
        IParam,
        IAny,
        IFrom,
        ICollect
    {
        internal override string Method
        {
            get
            {
                return Text.Method.ParamNotNull;
            }
        }

        internal ParamNotNullChainer(Chainer prev) 
            : base(prev)
        { 
            ParamChainer paramObj = prev.GetPrev<ParamChainer>();
            paramObj.Param.IsNullable = false;

            if (paramObj.Param.Default != null && paramObj.Param.Default.Value == null)
            {
                var paramName = paramObj.Param.Name;
                Throw(QueryTalkExceptionType.OptionalParamDefaultArgumentNull,
                    ArgVal(() => paramName, paramName),
                    Text.Method.ParamOptional);
            }
        }
    }
}
