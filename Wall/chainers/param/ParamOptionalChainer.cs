#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class ParamOptionalChainer : EndChainer, ISnippetDisallowed,
        IParamNotNull,
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
                return Text.Method.ParamOptional;
            }
        }

        internal ParamOptionalChainer(Chainer prev, ParameterArgument defaultArgument) 
            : base(prev)
        {
            if (defaultArgument.IsNullReference())
            {
                defaultArgument = Designer.Null;
            }

            TryTakeException(defaultArgument.Exception);
            TryThrow();

            ParamChainer paramObj = prev.GetPrev<ParamChainer>();
            paramObj.Param.IsOptional = true;
            paramObj.Param.Default = defaultArgument;

            if (defaultArgument.CheckType(paramObj.Param, defaultArgument.Value, out chainException))
            {
                paramObj.Param.Default.BuildArgument(GetRoot(), paramObj.Param.Name);
                TryThrow(paramObj.Param.Default.Exception);
            }
            else
            {
                chainException.Extra = "Argument is a default value of the parameter.";
                TryThrow();
            }    
        }
    }
}
