#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        #region From

        /// <summary>
        /// Specifies the tables, views, derived tables, and joined tables used in SELECT, DELETE, and UPDATE statements.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is a table argument.</param>
        public static FromChainer From(this IFrom prev, Table table)
        {
            return new FromChainer((Chainer)prev, table);
        }

        /// <summary>
        /// Specifies the tables, views, derived tables, and joined tables used in SELECT, DELETE, and UPDATE statements.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is an open view object.</param>
        public static FromChainer From(this IFrom prev, IOpenView table)
        {
            return new FromChainer((Chainer)prev, table);
        }

        // internal method used to translate SEMQ code into SQL code
        internal static FromChainer From(this IFrom prev, Table table, DbNode node)
        {
            var cfrom = new FromChainer((Chainer)prev, table);
            cfrom.SetNode(node);
            return cfrom;
        }
        // twin method
        internal static FromChainer From(this IFrom prev, IOpenView table, DbNode node)
        {
            var cfrom = new FromChainer((Chainer)prev, table);
            cfrom.SetNode(node);
            return cfrom;
        }

        #endregion

        #region InnerJoin

        /// <summary>
        /// Specifies the inner join operation between two tables.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is a table argument.</param>
        public static InnerJoinChainer InnerJoin(this IInnerJoin prev, Table table)
        {
            return new InnerJoinChainer((Chainer)prev, table);
        }

        /// <summary>
        /// Specifies the inner join operation between two tables.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is an open view object.</param>
        public static InnerJoinChainer InnerJoin(this IInnerJoin prev, IOpenView table)
        {
            return new InnerJoinChainer((Chainer)prev, table);
        }

        #endregion

        #region LeftOuterJoin

        /// <summary>
        /// Specifies the left outer join operation between two tables.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is a table argument.</param>
        public static LeftOuterJoinChainer LeftOuterJoin(this ILeftOuterJoin prev, Table table)
        {
            return new LeftOuterJoinChainer((Chainer)prev, table);
        }

        /// <summary>
        /// Specifies the left outer join operation between two tables.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is an open view object.</param>
        public static LeftOuterJoinChainer LeftOuterJoin(this ILeftOuterJoin prev, IOpenView table)
        {
            return new LeftOuterJoinChainer((Chainer)prev, table);
        }

        #endregion

        #region RightOuterJoin

        /// <summary>
        /// Specifies the right outer join operation between two tables.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is a table argument.</param>
        public static RightOuterJoinChainer RightOuterJoin(this IRightOuterJoin prev, Table table)
        {
            return new RightOuterJoinChainer((Chainer)prev, table);
        }

        /// <summary>
        /// Specifies the right outer join operation between two tables.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is an open view object.</param>
        public static RightOuterJoinChainer RightOuterJoin(this IRightOuterJoin prev, IOpenView table)
        {
            return new RightOuterJoinChainer((Chainer)prev, table);
        }

        #endregion

        #region FullJoin

        /// <summary>
        /// Specifies the full join operation between two tables.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is a table argument.</param>
        public static FullJoinChainer FullJoin(this IFullJoin prev, Table table)
        {
            return new FullJoinChainer((Chainer)prev, table);
        }

        /// <summary>
        /// Specifies the full join operation between two tables.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is an open view object.</param>
        public static FullJoinChainer FullJoin(this IFullJoin prev, IOpenView table)
        {
            return new FullJoinChainer((Chainer)prev, table);
        }

        #endregion

        #region CrossJoin

        /// <summary>
        /// Specifies the cross join (Cartesian product) operation between two tables.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is a table argument.</param>
        public static CrossJoinChainer CrossJoin(this IFullJoin prev, Table table)
        {
            return new CrossJoinChainer((Chainer)prev, table);
        }

        /// <summary>
        /// Specifies the cross join (Cartesian product) operation between two tables.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is an open view object.</param>
        public static CrossJoinChainer CrossJoin(this IFullJoin prev, IOpenView table)
        {
            return new CrossJoinChainer((Chainer)prev, table);
        }

        #endregion

        #region CrossApply

        /// <summary>
        /// Specifies the cross apply operation between two table expressions.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is a table argument.</param>
        public static CrossApplyChainer CrossApply(this ICrossApply prev, Table table)
        {
            return new CrossApplyChainer((Chainer)prev, table);
        }

        /// <summary>
        /// Specifies the cross apply operation between two table expressions.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is an open view object.</param>
        public static CrossApplyChainer CrossApply(this ICrossApply prev, IOpenView table)
        {
            return new CrossApplyChainer((Chainer)prev, table);
        }

        #endregion

        #region OuterApply

        /// <summary>
        /// Specifies the outer apply operation between two table expressions.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is a table argument.</param>
        public static OuterApplyChainer OuterApply(this IOuterApply prev, Table table)
        {
            return new OuterApplyChainer((Chainer)prev, table);
        }

        /// <summary>
        /// Specifies the outer apply operation between two table expressions.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="table">Is an open view object.</param>
        public static OuterApplyChainer OuterApply(this IOuterApply prev, IOpenView table)
        {
            return new OuterApplyChainer((Chainer)prev, table);
        }

        #endregion

    }
}
