#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;
using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {
        /// <summary>
        /// Determines whether a specified value matches any value in a view or a list.
        /// </summary>
        /// <param name="test">Is a string identifier of a column.</param>
        /// <param name="viewOrFirstValue">Is a one-column (scalar) view or is the first value to compare.</param>
        /// <param name="otherValues">Are other values from the list.</param>
        public static Expression In(this string test, ValueScalarArgument viewOrFirstValue, params ValueScalarArgument[] otherValues)
        {
            return new Expression(Operator.In, test,
                Common.MergeArrays<ValueScalarArgument>(viewOrFirstValue, otherValues));
        }

        /// <summary>
        /// Determines whether a specified value matches any value in a view or a list.
        /// </summary>
        /// <param name="test">Is a test expression.</param>
        /// <param name="viewOrFirstValue">Is a one-column (scalar) view or is the first value to compare.</param>
        /// <param name="otherValues">Are other values from the list.</param>
        public static Expression In(this IScalar test, ValueScalarArgument viewOrFirstValue, params ValueScalarArgument[] otherValues)
        {
            return new Expression(Operator.In, new ValueScalarArgument(test),
                Common.MergeArrays<ValueScalarArgument>(viewOrFirstValue, otherValues));
        }

        /// <summary>
        /// Determines whether a specified value matches any value in a view or a list.
        /// </summary>
        /// <param name="test">Is a test expression.</param>
        /// <param name="collectionOfValues">Specified list of scalar values. The list is any collection. The value is any QueryTalk compliant CLR type.</param>
        public static Expression In<T>(this string test, IEnumerable<T> collectionOfValues)
        {
            QueryTalkException exception;
            return new Expression(Operator.In, test,
                Expression.BuildSequence<T>(collectionOfValues, out exception), exception);
        }

        /// <summary>
        /// Determines whether a specified value matches any value in a view or a list.
        /// </summary>
        /// <param name="test">Is a test expression.</param>
        /// <param name="collectionOfValues">Specified list of scalar values. The list is any collection. The value is any QueryTalk compliant CLR type.</param>
        public static Expression In<T>(this IScalar test, IEnumerable<T> collectionOfValues)
        {
            QueryTalkException exception;
            return new Expression(Operator.In, new ScalarArgument(test),
                Expression.BuildSequence<T>(collectionOfValues, out exception), exception);
        }

        /// <summary>
        /// Determines whether a specified value does NOT match any value in a view or a list.
        /// </summary>
        /// <param name="test">Is a string identifier of a column.</param>
        /// <param name="viewOrFirstValue">Is a one-column (scalar) view or is the first value to compare.</param>
        /// <param name="otherValues">Are other values from the list.</param>
        public static Expression NotIn(this string test, ValueScalarArgument viewOrFirstValue, params ValueScalarArgument[] otherValues)
        {
            return new Expression(Operator.NotIn, test,
                Common.MergeArrays<ValueScalarArgument>(viewOrFirstValue, otherValues));
        }

        /// <summary>
        /// Determines whether a specified value does NOT match any value in a view or a list.
        /// </summary>
        /// <param name="test">Is a test expression.</param>
        /// <param name="viewOrFirstValue">Is a one-column (scalar) view or is the first value to compare.</param>
        /// <param name="otherValues">Are other values from the list.</param>
        public static Expression NotIn(this IScalar test, ValueScalarArgument viewOrFirstValue, params ValueScalarArgument[] otherValues)
        {
            return new Expression(Operator.NotIn, new ScalarArgument(test),
                Common.MergeArrays<ValueScalarArgument>(viewOrFirstValue, otherValues));
        }

        /// <summary>
        /// Determines whether a specified value does NOT match any value in a view or a list.
        /// </summary>
        /// <param name="test">Is a test expression.</param>
        /// <param name="collectionOfValues">Specified list of scalar values. The list is any collection. The value is any QueryTalk compliant CLR type.</param>
        public static Expression NotIn<T>(this string test, IEnumerable<T> collectionOfValues)
        {
            QueryTalkException exception;
            return new Expression(Operator.NotIn, test,
                Expression.BuildSequence<T>(collectionOfValues, out exception), exception);
        }

        /// <summary>
        /// Determines whether a specified value does NOT match any value in a view or a list.
        /// </summary>
        /// <param name="test">Is a test expression.</param>
        /// <param name="collectionOfValues">Specified list of scalar values. The list is any collection. The value is any QueryTalk compliant CLR type.</param>
        public static Expression NotIn<T>(this IScalar test, IEnumerable<T> collectionOfValues)
        {
            QueryTalkException exception;
            return new Expression(Operator.NotIn, new ScalarArgument(test),
                Expression.BuildSequence<T>(collectionOfValues, out exception), exception);
        }
    }
}
