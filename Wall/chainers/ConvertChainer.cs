#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class ConvertChainer : Chainer, IQuery, INonPredecessor,
        IConvertWithStyle
    {
        private FunctionArgument _argument;
        private DataType _def;
        private int _style;

        internal override string Method
        {
            get
            {
                return Text.Method.SysConvert;
            }
        }

        internal ConvertChainer(Chainer prev, FunctionArgument argument, DataType def, int style = 0) 
            : base(prev)
        {
            if (def.Exception != null)
            {
                def.Exception.Method = Method;
                chainException = def.Exception;
            }

            _argument = (argument == null) ? Designer.Null : argument;
            _def = def;
            _style = style;
        }

        internal SysFn GetSys()
        {
            if (_style == 0)
            {
                return new SysFn((buildContext, buildArgs) =>
                {

                    return Text.GenerateSql(30)
                        .Append(Text.Convert)
                        .EncloseLeft()
                        .Append(_def.Build())
                        .AppendComma()
                        .Append(_argument.Build(buildContext, buildArgs))
                        .EncloseRight()
                        .ToString();
                },
                chainException);
            }
            else
            {
                return new SysFn((buildContext, buildArgs) =>
                {

                    return Text.GenerateSql(30)
                        .Append(Text.Convert)
                        .EncloseLeft()
                        .Append(_def.Build())
                        .AppendComma()
                        .Append(_argument.Build(buildContext, buildArgs))
                        .AppendComma()
                        .Append(_style)
                        .EncloseRight()
                        .ToString();
                },
                chainException);
            }
        }
    }
}
