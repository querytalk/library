#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public class CaseValueChainer : Chainer, IQuery,
        ICase,
        ICaseWhen
    {
        internal override string Method
        {
            get
            {
                return Text.Method.CaseValue;
            }
        }

        internal CaseValueChainer(Chainer prev, ScalarArgument checkValue) 
            : base(prev)
        {
            checkValue = checkValue ?? Designer.Null;
            TryTake(checkValue, TakeProperty.Database, TakeProperty.Build);
        }
    }
}
