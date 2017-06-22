#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {
        /// <summary>
        /// Specifies that only the first set of rows will be returned from the query result.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="rows">Is the numeric bigint expression that specifies the number of rows to be returned.</param>
        public static TopChainer Top(this ITop prev, long rows)
        {
            if (prev is ISemantic)
            {
                return new TopChainer((ISemantic)prev, rows, false);
            }
            else
            {
                return new TopChainer((Chainer)prev, rows, false);
            }
        }

        /// <summary>
        /// Specifies that only the first set of rows will be returned from the query result.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="rows">Is the numeric float expression that specifies the percent of rows from the result set.</param>
        public static TopChainer Top(this ITop prev, double rows)
        {
            if (prev is ISemantic)
            {
                return new TopChainer((ISemantic)prev, rows, false);
            }
            else
            {
                return new TopChainer((Chainer)prev, rows, false);
            }
        }

        /// <summary>
        /// Specifies that only the first row will be returned from the query result.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static TopChainer TopOne(this ITop prev)
        {
            if (prev is ISemantic)
            {
                return new TopChainer((ISemantic)prev, 1, false);
            }
            else
            {
                return new TopChainer((Chainer)prev, 1, false);
            }
        }

        /// <summary>
        /// Specifies that all the rows with the same value in the ORDER BY columns appearing as the last of the TOP n (PERCENT) rows will be returned from the query result.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="rows">Is the numeric bigint expression that specifies the number of rows to be returned.</param>
        public static TopChainer TopWithTies(this ITop prev, long rows)
        {
            if (prev is ISemantic)
            {
                return new TopChainer((ISemantic)prev, rows, true);
            }
            else
            {
                return new TopChainer((Chainer)prev, rows, true);
            }
        }

        /// <summary>
        /// Specifies that all the rows with the same value in the ORDER BY columns appearing as the last of the TOP n (PERCENT) rows will be returned from the query result.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="rows">Is the numeric float expression that specifies the percent of rows from the result set.</param>
        public static TopChainer TopWithTies(this ITop prev, double rows)
        {
            if (prev is ISemantic)
            {
                return new TopChainer((ISemantic)prev, rows, true);
            }
            else
            {
                return new TopChainer((Chainer)prev, rows, true);
            }
        }

        #region Variable

        /// <summary>
        /// Specifies that only the first set of rows will be returned from the query result where the numeric expression is passed as variable.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="variable">Is the name of the variabler of the TOP value.</param>
        public static TopChainer Top(this ITop prev, string variable)
        {
            return new TopChainer((Chainer)prev, variable, false);
        }

        /// <summary>
        /// Specifies that all the rows with the same value in the ORDER BY columns appearing as the last of the TOP n (PERCENT) rows will be returned from the query result, where the numeric expression is passed as variable.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="variable">Is the name of the variable of the TOP value.</param>
        public static TopChainer TopWithTies(this ITop prev, string variable)
        {
            return new TopChainer((Chainer)prev, variable, true);
        }

        #endregion

        #region Internal

        internal static TopChainer Top(this ITop prev, Nullable<long> top, int overloader)
        {
            if (prev is ISemantic)
            {
                return new TopChainer((ISemantic)prev, top, false, 0);
            }
            else
            {
                return new TopChainer((Chainer)prev, top, false, true);
            }
        }

        #endregion
    }
}
