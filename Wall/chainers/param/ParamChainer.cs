#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public class ParamChainer : EndChainer
    {
        private string _paramName;

        internal override string Method
        {
            get
            {
                return Text.Method.Param;
            }
        }

        internal Variable Param { get; set; }

        internal ParamChainer(Chainer prev, string paramName, DT dt)
            : base(prev)
        {
            _paramName = paramName;

            if (dt.IsScalar() || dt.HasAtSign())
            {
                CheckParamAndThrow();
            }
        }

        private void CheckParamAndThrow()
        {
            CheckNullAndThrow(Arg(() => _paramName, _paramName));

            var check = Variable.TryValidateName(_paramName, out chainException);
            TryThrow();

            if (!check)
            {
                Throw(QueryTalkExceptionType.InvalidVariableName, ArgVal(() => _paramName, _paramName));
            }
        }
    }
}
