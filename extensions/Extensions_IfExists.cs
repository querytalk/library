#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        #region If Exists

        /// <summary>
        /// Returns TRUE if a query contains any rows.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        /// <param name="view">Is a view object.</param>
        public static IfChainer IfExists(this IAny prev, View view)
        {
            return new IfChainer((Chainer)prev, view, true);
        }

        /// <summary>
        /// Returns TRUE if a query contains any rows.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        /// <param name="openView">Is a subquery.</param>
        public static IfChainer IfExists(this IAny prev, IOpenView openView)
        {
            return new IfChainer((Chainer)prev, openView, true);
        }

        /// <summary>
        /// Returns TRUE if a query contains any rows.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        /// <param name="nonSelectView">Is a subquery without the .Select method.</param>
        public static IfChainer IfExists(this IAny prev, INonSelectView nonSelectView)
        {
            return new IfChainer((Chainer)prev, nonSelectView, true);
        }

        /// <summary>
        /// Returns TRUE if a query does not contains any rows.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        /// <param name="view">Is a view object.</param>
        public static IfChainer IfNotExists(this IAny prev, View view)
        {
            return new IfChainer((Chainer)prev, view, false);
        }

        /// <summary>
        /// Returns TRUE if a query does not contains any rows.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        /// <param name="openView">Is a subquery.</param>
        public static IfChainer IfNotExists(this IAny prev, IOpenView openView)
        {
            return new IfChainer((Chainer)prev, openView, false);
        }

        /// <summary>
        /// Returns TRUE if a query does not contains any rows.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        /// <param name="nonSelectView">Is a subquery without the .Select method.</param>
        public static IfChainer IfNotExists(this IAny prev, INonSelectView nonSelectView)
        {
            return new IfChainer((Chainer)prev, nonSelectView, false);
        }

        #endregion

        #region Else If Exists

        /// <summary>
        /// Returns TRUE if a query contains any rows.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        /// <param name="view">Is a view object.</param>
        public static ElseIfChainer ElseIfExists(this IAny prev, View view)
        {
            return new ElseIfChainer((Chainer)prev, view, true);
        }

        /// <summary>
        /// Returns TRUE if a query contains any rows.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        /// <param name="openView">Is a subquery.</param>
        public static ElseIfChainer ElseIfExists(this IAny prev, IOpenView openView)
        {
            return new ElseIfChainer((Chainer)prev, openView, true);
        }

        /// <summary>
        /// Returns TRUE if a query contains any rows.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        /// <param name="nonSelectView">Is a subquery without the .Select method.</param>
        public static ElseIfChainer ElseIfExists(this IAny prev, INonSelectView nonSelectView)
        {
            return new ElseIfChainer((Chainer)prev, nonSelectView, true);
        }

        /// <summary>
        /// Returns TRUE if a query does not contains any rows.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        /// <param name="view">Is a view object.</param>
        public static ElseIfChainer ElseIfNotExists(this IAny prev, View view)
        {
            return new ElseIfChainer((Chainer)prev, view, false);
        }

        /// <summary>
        /// Returns TRUE if a query does not contains any rows.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        /// <param name="openView">Is a subquery.</param>
        public static ElseIfChainer ElseIfNotExists(this IAny prev, IOpenView openView)
        {
            return new ElseIfChainer((Chainer)prev, openView, false);
        }

        /// <summary>
        /// Returns TRUE if a query does not contains any rows.
        /// </summary>
        /// <param name="prev">Is a predecessor object.</param>
        /// <param name="nonSelectView">Is a subquery without the .Select method.</param>
        public static ElseIfChainer ElseIfNotExists(this IAny prev, INonSelectView nonSelectView)
        {
            return new ElseIfChainer((Chainer)prev, nonSelectView, false);
        }

        #endregion

    }
}
