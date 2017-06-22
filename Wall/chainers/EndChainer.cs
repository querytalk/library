#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    // Link object between two sql statements. A base clase for all chain objects that represents an ending.
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public class EndChainer : Chainer,
        IAny,
        IFrom,
        ICollect,
        IEndProc,
        IGo,
        IConnectBy
    {
        internal override string Method
        {
            get
            {
                return Text.Method.End;
            }
        }

        /// <summary>
        /// Ends the statement. Not explicitly needed.
        /// </summary>
        /// <param name="include">
        /// If false then the statement is excluded from the SQL build and will not get executed.
        /// </param>
        public EndChainer End(bool include = true)
        {
            if (Statement != null)
            {
                Statement.SkipBuild = !include;
            }
            return this;
        }

        internal EndChainer(Chainer prev) 
            : base(prev)
        { }
    }
}
