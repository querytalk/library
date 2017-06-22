#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        #region Pivot/Unpivot

        /// <summary>
        /// PIVOT rotates a table-valued expression by turning the unique values from one column in the expression into multiple columns in the output, and performs aggregations where they are required on any remaining column values that are wanted in the final output.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="aggregateValueColumn">Is a specified Sys aggregation method that takes a column whose values are to be aggregated and stored into rotated columns.</param>
        /// <param name="rotatingColumn">Is a rotating column whose values will become the rotated column names in the pivot table.</param>
        /// <param name="firstRotatedColumn">The first rotated column in the pivot table.</param>
        /// <param name="otherRotatedColumns">Other rotated columns in the pivot table.</param>
        public static PivotChainer Pivot(this IPivot prev, 
            SysFn aggregateValueColumn, 
            NonSelectColumnArgument rotatingColumn, 
            string firstRotatedColumn, params string[] otherRotatedColumns)
        {
            return new PivotChainer((Chainer)prev, aggregateValueColumn, rotatingColumn,
                Common.MergeArrays<string>(firstRotatedColumn, otherRotatedColumns));
        }

        /// <summary>
        /// UNPIVOT performs the opposite operation to PIVOT by rotating columns of a table-valued expression into column values. Note that UNPIVOT is not the exact reverse of PIVOT. PIVOT performs an aggregation and, therefore, merges possible multiple rows into a single row in the output. UNPIVOT does not reproduce the original table-valued expression result because rows have been merged.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="valueColumn">Is a column that will hold the rotated values.</param>
        /// <param name="rotatingColumn">Is a rotating column whose values will become the rotated column names in the pivot table.</param>
        /// <param name="firstRotatedColumn">The first rotated column in the pivot table.</param>
        /// <param name="otherRotatedColumns">Other rotated columns in the pivot table.</param>
        public static PivotChainer Unpivot(this IPivot prev, 
            NonSelectColumnArgument valueColumn, 
            NonSelectColumnArgument rotatingColumn,
            string firstRotatedColumn, params string[] otherRotatedColumns)
        {
            return new PivotChainer((Chainer)prev, valueColumn, rotatingColumn,
                Common.MergeArrays<string>(firstRotatedColumn, otherRotatedColumns));
        }

        #endregion

    }
}
