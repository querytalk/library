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
        /// Begins the cursor block generating the SQL code that declares the cursor, opens it, and start the fetch-next loop.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="view">A cursor data source.</param>
        public static BeginCursorChainer BeginCursor(this IAny prev, View view)
        {
            return new BeginCursorChainer((Chainer)prev, view);
        }

        /// <summary>
        /// Stores column values into the specified variables.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="firstVariable">The first variable.</param>
        /// <param name="otherVariables">The other variables.</param>
        public static BeginCursorIntoVarsChainer IntoVars(this IBeginCursorWithVariables prev, 
            string firstVariable, params string[] otherVariables)
        {
            return new BeginCursorIntoVarsChainer(
                (Chainer)prev, Common.MergeArrays<string>(firstVariable, otherVariables));
        }

        /// <summary>
        /// Retrieves a specific row from a SQL Server cursor.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static BeginCursorFetchNextChainer FetchNext(this IAny prev)
        {
            return new BeginCursorFetchNextChainer((Chainer)prev);
        }

        /// <summary>
        /// Ends the cursor block generating the SQL code that closes the cursor loop, closes the cursor, and deallocates the cursor resources.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        public static EndCursorChainer EndCursor(this IAny prev)
        {
            return new EndCursorChainer((Chainer)prev);
        }
    }
}
