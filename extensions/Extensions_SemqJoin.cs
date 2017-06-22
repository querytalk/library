#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        /// <summary>
        /// Specifies the auto join operation between two tables.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is an open view object.</param>
        public static SemqJoinChainer Join(this ISemqJoin prev, ISemantic table)
        {
            return new SemqJoinChainer((Chainer)prev, table);
        }

    }
}
