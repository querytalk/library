#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {

        /// <summary>
        /// Generates the SELECT statement without the FROM clause returning the result set of the specified values.
        /// </summary>
        /// <param name="firstValue">Is the first value.</param>
        /// <param name="otherValues">Are the other values.</param>
        public static CollectChainer Collect(Column firstValue, params Column[] otherValues)
        {
            var values = MergeArrays(firstValue, otherValues);
            if (values == null || values.Length == 0)
            {
                _Throw(QueryTalkExceptionType.ArgumentNull, "columns", ".Collect");
            }

            var root = new d();
            return new CollectChainer(root, values);
        }

    }
}