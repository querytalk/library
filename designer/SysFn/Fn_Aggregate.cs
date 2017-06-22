#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {
        /// <summary>
        /// <para>AVG built-in function.</para>
        /// <para>Returns the average of the values in a group. Null values are ignored.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the exact numeric or approximate numeric data type category, except for the bit data type. Aggregate functions and subqueries are not permitted.</param>
        public static SysFn Avg(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "AVG({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Avg");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>AVG built-in function.</para>
        /// <para>Returns the average of unique values in a group. Null values are ignored.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the exact numeric or approximate numeric data type category, except for the bit data type. Aggregate functions and subqueries are not permitted.</param>
        public static SysFn AvgDistinct(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "AVG(DISTINCT {0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.AvgDistinct");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>CHECKSUM_AGG built-in function.</para>
        /// <para>Returns the checksum of the values in a group. Null values are ignored.</para>
        /// </summary>
        /// <param name="argument">Is an integer expression. Aggregate functions and subqueries are not allowed.</param>
        public static SysFn ChecksumAgg(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "CHECKSUM_AGG({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.ChecksumAgg");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>CHECKSUM_AGG built-in function.</para>
        /// <para>Returns the checksum of unique values in a group. Null values are ignored.</para>
        /// </summary>
        /// <param name="argument">Is an integer expression. Aggregate functions and subqueries are not allowed.</param>
        public static SysFn ChecksumAggDistinct(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "CHECKSUM_AGG(DISTINCT {0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.ChecksumAggDistinct");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>COUNT built-in function.</para>
        /// <para>Returns the number of items in a group.</para>
        /// </summary>
        public static SysFn Count()
        {
            return new SysFn(
                String.Format("COUNT({0})", Wall.Text.Asterisk)
            );
        }

        /// <summary>
        /// <para>COUNT built-in function.</para>
        /// <para>Returns the number of items in a group. COUNT always returns an int data type value.</para>
        /// </summary>
        /// <param name="argument">Is an expression of any type except text, image, or ntext.</param>
        public static SysFn Count(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "COUNT({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Count");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>COUNT built-in function.</para>
        /// <para>Returns the number of unique non-null values in a group.</para>
        /// </summary>
        /// <param name="argument">Is an expression of any type except text, image, or ntext.</param>
        public static SysFn CountDistinct(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "COUNT(DISTINCT {0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.CountDistinct");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>COUNT_BIG built-in function.</para>
        /// <para>Returns the number of items in a group. COUNT_BIG always returns a bigint data type value.</para>
        /// </summary>
        public static SysFn CountBig()
        {
            return new SysFn(
                String.Format("COUNT_BIG({0})", Wall.Text.Asterisk)
            );
        }

        /// <summary>
        /// <para>COUNT_BIG built-in function.</para>
        /// <para>Returns the number of items in a group. COUNT_BIG always returns a bigint data type value.</para>
        /// </summary>
        /// <param name="argument">Is an expression of any type.</param>
        public static SysFn CountBig(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "COUNT_BIG({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.CountBig");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>COUNT_BIG built-in function.</para>
        /// <para>Returns the number of unique non-null values in a group.</para>
        /// </summary>
        /// <param name="argument">Is an expression of any type.</param>
        public static SysFn CountBigDistinct(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "COUNT_BIG(DISTINCT {0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.CountBigDistinct");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>GROUPING built-in function.</para>
        /// <para>Indicates whether a specified column expression in a GROUP BY list is aggregated or not. GROUPING returns 1 for aggregated or 0 for not aggregated in the result set.</para>
        /// </summary>
        /// <param name="argument">Is a column or an expression that contains a column in a GROUP BY clause.</param>
        public static SysFn Grouping(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "GROUPING({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Grouping");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>GROUPING_ID built-in function.</para>
        /// <para>Is a function that computes the level of grouping.</para>
        /// </summary>
        /// <param name="columns">Are columns in a GROUP BY clause.</param>
        public static SysFn GroupingId(params GroupingArgument[] columns)
        {
            var exception = columns.IsGroupingNull("Sys.GroupingId");

            return new SysFn((buildContext, buildArgs) =>
                {
                    return String.Format(
                        "GROUPING_ID({0})",
                        GroupingArgument.Concatenate(columns, buildContext, buildArgs, false));
                }, 
                exception);
        }

        /// <summary>
        /// <para>MAX built-in function.</para>
        /// <para>Returns the maximum value in the expression.</para>
        /// </summary>
        /// <param name="argument">Is a constant, column name, or function, and any combination of arithmetic, bitwise, and string operators. MAX can be used with numeric, character, uniqueidentifier, and datetime columns, but not with bit columns. Aggregate functions and subqueries are not permitted.</param>
        public static SysFn Max(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "MAX({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Max");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>MAX built-in function.</para>
        /// <para>Returns the maximum unique value in the expression. DISTINCT is not meaningful with MAX and is available for ISO compatibility only.</para>
        /// </summary>
        /// <param name="argument">Is a constant, column name, or function, and any combination of arithmetic, bitwise, and string operators. MAX can be used with numeric, character, uniqueidentifier, and datetime columns, but not with bit columns. Aggregate functions and subqueries are not permitted.</param>
        public static SysFn MaxDistinct(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "MAX(DISTINCT {0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.MaxDistinct");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>MIN built-in function.</para>
        /// <para>Returns the minimum value in the expression.</para>
        /// </summary>
        /// <param name="argument">Is a constant, column name, or function, and any combination of arithmetic, bitwise, and string operators. MIN can be used with numeric, character, uniqueidentifier, and datetime columns, but not with bit columns. Aggregate functions and subqueries are not permitted.</param>
        public static SysFn Min(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "MIN({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Min");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>MIN built-in function.</para>
        /// <para>Returns the minimum unique value in the expression. DISTINCT is not meaningful with MIN and is available for ISO compatibility only.</para>
        /// </summary>
        /// <param name="argument">Is a constant, column name, or function, and any combination of arithmetic, bitwise, and string operators. MIN can be used with numeric, character, uniqueidentifier, and datetime columns, but not with bit columns. Aggregate functions and subqueries are not permitted.</param>
        public static SysFn MinDistinct(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "MIN(DISTINCT {0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.MinDistinct");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>ROLLUP built-in function.</para>
        /// <para>Generates a result set that shows aggregates for a hierarchy of values in the selected columns.</para>
        /// </summary>
        /// <param name="columns">Are selected columns in a ROLLUP function.</param>
        public static SysFn Rollup(params GroupingArgument[] columns)
        {
            // check null
            var exception = columns.IsGroupingNull("Sys.Rollup");

            return new SysFn((buildContext, buildArgs) =>
                {
                    return String.Format(
                        "ROLLUP({0})",
                        GroupingArgument.Concatenate(columns, buildContext, buildArgs, false));
                },
                exception);
        }

        /// <summary>
        /// <para>CUBE built-in function.</para>
        /// <para>Generates a result set that shows aggregates for all combinations of values in the selected columns.</para>
        /// </summary>
        /// <param name="columns">Are selected columns in a CUBE function.</param>
        public static SysFn Cube(params GroupingArgument[] columns)
        {
            // check null
            var exception = columns.IsGroupingNull("Sys.Cube");

            return new SysFn((buildContext, buildArgs) =>
                {
                    return String.Format(
                        "CUBE({0})",
                        GroupingArgument.Concatenate(columns, buildContext, buildArgs, false));
                },
                exception);
        }

        /// <summary>
        /// <para>STDEV built-in function.</para>
        /// <para>Returns the statistical standard deviation of all values in the specified expression.</para>
        /// </summary>
        /// <param name="argument">Is a numeric expression. Aggregate functions and subqueries are not permitted. argument is an expression of the exact numeric or approximate numeric data type category, except for the bit data type.</param>
        public static SysFn Stdev(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "STDEV({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Stdev");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>STDEV built-in function.</para>
        /// <para>Returns the statistical standard deviation of all unique values in the specified expression.</para>
        /// </summary>
        /// <param name="argument">Is a numeric expression. Aggregate functions and subqueries are not permitted. argument is an expression of the exact numeric or approximate numeric data type category, except for the bit data type.</param>
        public static SysFn StdevDistinct(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "STDEV(DISTINCT {0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.StdevDistinct");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>STDEVP built-in function.</para>
        /// <para>Returns the statistical standard deviation for the population for all values in the specified expression.</para>
        /// </summary>
        /// <param name="argument">Is a numeric expression. Aggregate functions and subqueries are not permitted. argument is an expression of the exact numeric or approximate numeric data type category, except for the bit data type.</param>
        public static SysFn Stdevp(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "STDEVP({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Stdevp");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>STDEVP built-in function.</para>
        /// <para>Returns the statistical standard deviation for the population for all unique values in the specified expression.</para>
        /// </summary>
        /// <param name="argument">Is a numeric expression. Aggregate functions and subqueries are not permitted. argument is an expression of the exact numeric or approximate numeric data type category, except for the bit data type.</param>
        public static SysFn StdevpDistinct(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "STDEVP(DISTINCT {0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.StdevpDistinct");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>SUM built-in function.</para>
        /// <para>Returns the sum of all the values in the expression. SUM can be used with numeric columns only. Null values are ignored.</para>
        /// </summary>
        /// <param name="argument">Is a constant, column, or function, and any combination of arithmetic, bitwise, and string operators. argument is an expression of the exact numeric or approximate numeric data type category, except for the bit data type. Aggregate functions and subqueries are not permitted.</param>
        public static SysFn Sum(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "SUM({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Sum");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>SUM built-in function.</para>
        /// <para>Returns the sum of all the unique values in the expression. SUM can be used with numeric columns only. Null values are ignored.</para>
        /// </summary>
        /// <param name="argument">Is a constant, column, or function, and any combination of arithmetic, bitwise, and string operators. argument is an expression of the exact numeric or approximate numeric data type category, except for the bit data type. Aggregate functions and subqueries are not permitted.</param>
        public static SysFn SumDistinct(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "SUM(DISTINCT {0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.SumDistinct");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>VAR built-in function.</para>
        /// <para>Returns the statistical variance of all values in the specified expression.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the exact numeric or approximate numeric data type category, except for the bit data type. Aggregate functions and subqueries are not permitted.</param>
        public static SysFn Var(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "VAR({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Var");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>VAR built-in function.</para>
        /// <para>Returns the statistical variance of all unique values in the specified expression.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the exact numeric or approximate numeric data type category, except for the bit data type. Aggregate functions and subqueries are not permitted.</param>
        public static SysFn VarDistinct(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "VAR(DISTINCT {0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.VarDistinct");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>VARP built-in function.</para>
        /// <para>Returns the statistical variance for the population for all values in the specified expression.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the exact numeric or approximate numeric data type category, except for the bit data type. Aggregate functions and subqueries are not permitted.</param>
        public static SysFn Varp(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "VARP({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Varp");
                    return sql;
                },
                argument.chainException);
        }

        /// <summary>
        /// <para>VARP built-in function.</para>
        /// <para>Returns the statistical variance for the population for all unique values in the specified expression.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the exact numeric or approximate numeric data type category, except for the bit data type. Aggregate functions and subqueries are not permitted.</param>
        public static SysFn VarpDistinct(AggregateArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "VARP(DISTINCT {0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.VarpDistinct");
                    return sql;
                },
                argument.chainException);
        }

    }
}
