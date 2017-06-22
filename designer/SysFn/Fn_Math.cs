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
        /// <para>ABS built-in function.</para>
        /// <para>A mathematical function that returns the absolute (positive) value of the specified argument.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the exact numeric or approximate numeric data type category.</param>
        public static SysFn Abs(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "ABS({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Abs");
                    return sql;
                });
        }

        /// <summary>
        /// <para>ACOS built-in function.</para>
        /// <para>A mathematical function that returns the angle, in radians, whose cosine is the specified float expression; also called arccosine.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the type float or of a type that can be implicitly converted to float, with a value from -1 through 1.</param>
        public static SysFn Acos(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "ACOS({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Acos");
                    return sql;
                });
        }

        /// <summary>
        /// <para>ASIN built-in function.</para>
        /// <para>Returns the angle, in radians, whose sine is the specified float expression. This is also called arcsine.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the type float or of a type that can be implicitly converted to float, with a value from -1 through 1.</param>
        public static SysFn Asin(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "ASIN({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Asin");
                    return sql;
                });
        }

        /// <summary>
        /// <para>ATAN built-in function.</para>
        /// <para>Returns the angle in radians whose tangent is a specified float expression. This is also called arctangent.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the type float or of a type that can be implicitly converted to float.</param>
        public static SysFn Atan(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "ATAN({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Atan");
                    return sql;
                });
        }

        /// <summary>
        /// <para>ATN2 built-in function.</para>
        /// <para>Returns the angle, in radians, between the positive x-axis and the ray from the origin to the point (y, x), where x and y are the values of the two specified float expressions.</para>
        /// </summary>
        /// <param name="argument1">Is an expression of the float data type.</param>
        /// <param name="argument2">Is an expression of the float data type.</param>
        public static SysFn Atn2(NumericArgument argument1, NumericArgument argument2)
        {
            if (argument1 == null) { argument1 = Designer.Null; }
            if (argument2 == null) { argument2 = Designer.Null; }

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "ATN2({0},{1})",
                        argument1.Build(buildContext, buildArgs),
                        argument2.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument1.Exception, "Sys.Atn2");
                    buildContext.TryTakeException(argument2.Exception, "Sys.Atn2");
                    return sql;
                });
        }

        /// <summary>
        /// <para>CEILING built-in function.</para>
        /// <para>Returns the smallest integer greater than, or equal to, the specified argument.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the exact numeric or approximate numeric data type category, except for the bit data type.</param>
        public static SysFn Ceiling(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "CEILING({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Ceiling");
                    return sql;
                });
        }

        /// <summary>
        /// <para>COS built-in function.</para>
        /// <para>Is a mathematical function that returns the trigonometric cosine of the specified angle, in radians, in the specified expression.</para>
        /// </summary>
        /// <param name="argument">Is an expression of type float.</param>
        public static SysFn Cos(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "COS({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Cos");
                    return sql;
                });
        }

        /// <summary>
        /// <para>COT built-in function.</para>
        /// <para>A mathematical function that returns the trigonometric cotangent of the specified angle, in radians, in the specified float expression.</para>
        /// </summary>
        /// <param name="argument">Is an expression of type float or of a type that can be implicitly converted to float.</param>
        public static SysFn Cot(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "COT({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Cot");
                    return sql;
                });
        }

        /// <summary>
        /// <para>DEGREES built-in function.</para>
        /// <para>Returns the corresponding angle in degrees for an angle specified in radians.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the exact numeric or approximate numeric data type category, except for the bit data type.</param>
        public static SysFn Degrees(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "DEGREES({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Degrees");
                    return sql;
                });
        }

        /// <summary>
        /// <para>EXP built-in function.</para>
        /// <para>Returns the exponential value of the specified float expression.</para>
        /// </summary>
        /// <param name="argument">Is an expression of type float or of a type that can be implicitly converted to float.</param>
        public static SysFn Exp(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "EXP({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Exp");
                    return sql;
                });
        }

        /// <summary>
        /// <para>FLOOR built-in function.</para>
        /// <para>Returns the largest integer less than or equal to the specified argument.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the exact numeric or approximate numeric data type category, except for the bit data type.</param>
        public static SysFn Floor(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "FLOOR({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Floor");
                    return sql;
                });
        }

        /// <summary>
        /// <para>LOG built-in function.</para>
        /// <para>Returns the natural logarithm of the specified float expression.</para>
        /// </summary>
        /// <param name="argument">Is an expression of type float or of a type that can be implicitly converted to float.</param>
        public static SysFn Log(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "LOG({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Log");
                    return sql;
                });
        }

        /// <summary>
        /// <para>LOG10 built-in function.</para>
        /// <para>Returns the base-10 logarithm of the specified float expression.</para>
        /// </summary>
        /// <param name="argument">Is an expression of type float or of a type that can be implicitly converted to float.</param>
        public static SysFn Log10(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "LOG10({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Log10");
                    return sql;
                });
        }

        /// <summary>
        /// <para>PI built-in function.</para>
        /// <para>Returns the constant value of PI.</para>
        /// </summary>
        public static SysFn Pi()
        {
            return new SysFn("PI()");
        }

        /// <summary>
        /// <para>POWER built-in function.</para>
        /// <para>Returns the value of the specified expression to the specified power.</para>
        /// </summary>
        /// <param name="argument1">Is an expression of type float or of a type that can be implicitly converted to float.</param>
        /// <param name="argument2">Is the power to which to raise argument1. argument2 can be an expression of the exact numeric or approximate numeric data type category, except for the bit data type.</param>
        public static SysFn Power(NumericArgument argument1, NumericArgument argument2)
        {
            if (argument1 == null) { argument1 = Designer.Null; }
            if (argument2 == null) { argument2 = Designer.Null; }

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "POWER({0},{1})",
                        argument1.Build(buildContext, buildArgs),
                        argument2.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument1.Exception, "Sys.Power");
                    buildContext.TryTakeException(argument2.Exception, "Sys.Power");
                    return sql;
                });
        }

        /// <summary>
        /// <para>RADIANS built-in function.</para>
        /// <para>Returns radians when a argument, in degrees, is entered.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the exact numeric or approximate numeric data type category, except for the bit data type.</param>
        public static SysFn Radians(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "RADIANS({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Radians");
                    return sql;
                });
        }

        /// <summary>
        /// <para>RAND built-in function.</para>
        /// <para>Returns a pseudo-random float value from 0 through 1, exclusive.</para>
        /// </summary>
        public static SysFn Rand()
        {
            return new SysFn(
                String.Format(
                    "RAND()"
            ));
        }

        /// <summary>
        /// <para>RAND built-in function.</para>
        /// <para>Returns a pseudo-random float value from 0 through 1, exclusive.</para>
        /// </summary>
        /// <param name="seed">Is an integer expression (tinyint, smallint, or int) that gives the seed value. If seed is not specified, the SQL Server Database Engine assigns a seed value at random. For a specified seed value, the result returned is always the same.</param>
        public static SysFn Rand(NumericArgument seed)
        {
            seed = seed ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "RAND({0})",
                        seed.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(seed.Exception, "Sys.Rand");
                    return sql;
                });
        }

        /// <summary>
        /// <para>ROUND built-in function.</para>
        /// <para>Returns a numeric value, rounded to the specified length or precision.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the exact numeric or approximate numeric data type category, except for the bit data type.</param>
        /// <param name="length">Is the precision to which the argument is to be rounded. length must be an expression of type tinyint, smallint, or int.</param>
        public static SysFn Round(NumericArgument argument, NumericArgument length)
        {
            if (argument == null) { argument = Designer.Null; }
            if (length == null) { length = Designer.Null; }

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "ROUND({0},{1})",
                        argument.Build(buildContext, buildArgs),
                        length.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Round");
                    buildContext.TryTakeException(length.Exception, "Sys.Round");
                    return sql;
                });
        }

        /// <summary>
        /// <para>ROUND built-in function.</para>
        /// <para>Returns a numeric value, rounded to the specified length or precision.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the exact numeric or approximate numeric data type category, except for the bit data type.</param>
        /// <param name="length">Is the precision to which the argument is to be rounded. length must be an expression of type tinyint, smallint, or int.</param>
        /// <param name="function">Is the type of operation to perform. function must be tinyint, smallint, or int. When function is omitted or has a value of 0 (default), the argument is rounded.</param>
        public static SysFn Round(NumericArgument argument, NumericArgument length, NumericArgument function)
        {
            if (argument == null) { argument = Designer.Null; }
            if (length == null) { length = Designer.Null; }
            if (function == null) { function = Designer.Null; }

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "ROUND({0},{1},{2})",
                        argument.Build(buildContext, buildArgs),
                        length.Build(buildContext, buildArgs),
                        function.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Round");
                    buildContext.TryTakeException(length.Exception, "Sys.Round");
                    buildContext.TryTakeException(function.Exception, "Sys.Round");
                    return sql;
                });
        }

        /// <summary>
        /// <para>SIGN built-in function.</para>
        /// <para>Returns the positive (+1), zero (0), or negative (-1) sign of the specified expression.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the exact numeric or approximate numeric data type category, except for the bit data type.</param>
        public static SysFn Sign(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "SIGN({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Sign");
                    return sql;
                });
        }

        /// <summary>
        /// <para>SIN built-in function.</para>
        /// <para>Returns the trigonometric sine of the specified angle, in radians, and in an approximate numeric, float, expression.</para>
        /// </summary>
        /// <param name="argument">Is an expression of type float or of a type that can be implicitly converted to float.</param>
        public static SysFn Sin(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "SIN({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Sin");
                    return sql;
                });
        }

        /// <summary>
        /// <para>SQRT built-in function.</para>
        /// <para>Returns the square root of the specified float value.</para>
        /// </summary>
        /// <param name="argument">Is an expression of type float or of a type that can be implicitly converted to float.</param>
        public static SysFn Sqrt(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "SQRT({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Sqrt");
                    return sql;
                });
        }

        /// <summary>
        /// <para>SQUARE built-in function.</para>
        /// <para>Returns the square of the specified float value.</para>
        /// </summary>
        /// <param name="argument">Is an expression of type float or of a type that can be implicitly converted to float.</param>
        public static SysFn Square(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "SQUARE({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Square");
                    return sql;
                });
        }

        /// <summary>
        /// <para>TAN built-in function.</para>
        /// <para>Returns the tangent of the input expression.</para>
        /// </summary>
        /// <param name="argument">Is an expression of type float or of a type that can be implicitly converted to float, interpreted as number of radians.</param>
        public static SysFn Tan(NumericArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "TAN({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Tan");
                    return sql;
                });
        }
       
    }
}
