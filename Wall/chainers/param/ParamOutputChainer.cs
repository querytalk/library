#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public class ParamOutputChainer : EndChainer, ISnippetDisallowed,
        IParam,
        IAny,
        IFrom,
        ICollect
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Out;
            }
        }

        internal ParamOutputChainer(Chainer prev) 
            : base(prev)
        {
            ParamChainer paramObj = prev.GetPrev<ParamChainer>();
            paramObj.Param.IsOutput = true; 
        }
    }
}
