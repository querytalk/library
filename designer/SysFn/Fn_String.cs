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
        /// <para>ASCII built-in function.</para>
        /// <para>Returns the ASCII code value of the leftmost character of a character expression.</para>
        /// </summary>
        /// <param name="argument">Is an expression of the type char or varchar.</param>
        public static SysFn Ascii(FunctionArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "ASCII({0})",
                    argument.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument.Exception, "Sys.Ascii");
                return sql;
            });
        }

        /// <summary>
        /// <para>CHAR built-in function.</para>
        /// <para>Converts an int ASCII code to a character.</para>
        /// </summary>
        /// <param name="argument">Is an integer from 0 through 255. NULL is returned if the integer expression is not in this range.</param>
        public static SysFn Char(ScalarArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "CHAR({0})",
                    argument.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument.Exception, "Sys.Char");
                return sql;
            });
        }

        /// <summary>
        /// <para>CHARINDEX built-in function.</para>
        /// <para>Searches an expression for another expression and returns its starting position if found.</para>
        /// </summary>
        /// <param name="toFind">Is a character expression that contains the sequence to be found. toFind is limited to 8000 characters.</param>
        /// <param name="toSearch">Is a character expression to be searched.</param>
        /// <param name="startLocation">Is an integer or bigint expression at which the search starts. If startLocation is not specified, is a negative number, or is 0, the search starts at the beginning of toSearch.</param>
        public static SysFn Charindex(ScalarArgument toFind, ScalarArgument toSearch, ScalarArgument startLocation = null)
        {
            toFind = toFind ?? Designer.Null;
            toSearch = toSearch ?? Designer.Null; 

            if (startLocation == null)
            {
                return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "CHARINDEX({0},{1})",
                        toFind.Build(buildContext, buildArgs),
                        toSearch.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(toFind.Exception, "Sys.CharIndex");
                    buildContext.TryTakeException(toSearch.Exception, "Sys.CharIndex");
                    return sql;
                });
            }
            else
            {
                return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "CHARINDEX({0},{1},{2})",
                        toFind.Build(buildContext, buildArgs),
                        toSearch.Build(buildContext, buildArgs),
                        startLocation.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(toFind.Exception, "Sys.CharIndex");
                    buildContext.TryTakeException(toSearch.Exception, "Sys.CharIndex");
                    buildContext.TryTakeException(startLocation.Exception, "Sys.CharIndex");
                    return sql;
                });
            }
        }

        /// <summary>
        /// <para>DIFFERENCE built-in function.</para>
        /// <para>Returns an integer value that indicates the difference between the SOUNDEX values of two character expressions.</para>
        /// </summary>
        /// <param name="argument1">Is the first alphanumeric expression of character data. argument1 can be a constant, variable, or column.</param>
        /// <param name="argument2">Is the second alphanumeric expression of character data. argument2 can be a constant, variable, or column.</param>
        public static SysFn Difference(FunctionArgument argument1, FunctionArgument argument2)
        {
            argument1 = argument1 ?? Designer.Null;
            argument2 = argument2 ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "DIFFERENCE({0},{1})",
                    argument1.Build(buildContext, buildArgs),
                    argument2.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument1.Exception, "Sys.Difference");
                buildContext.TryTakeException(argument2.Exception, "Sys.Difference");
                return sql;
            });
        }

        /// <summary>
        /// <para>LEFT built-in function.</para>
        /// <para>Returns the left part of a character string with the specified number of characters.</para>
        /// </summary>
        /// <param name="argument1">Is an expression of character or binary data. textual can be a constant, variable, or column. argument1 can be of any data type, except text or ntext, that can be implicitly converted to varchar or nvarchar.</param>
        /// <param name="argument2">Is a positive integer that specifies how many characters of the argument1 will be returned. </param>
        public static SysFn Left(ScalarArgument argument1, ScalarArgument argument2)
        {
            argument1 = argument1 ?? Designer.Null;
            argument2 = argument2 ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "LEFT({0},{1})",
                    argument1.Build(buildContext, buildArgs),
                    argument2.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument1.Exception, "Sys.Left");
                buildContext.TryTakeException(argument2.Exception, "Sys.Left");
                return sql;
            });
        }

        /// <summary>
        /// <para>LEN built-in function.</para>
        /// <para>Returns the number of characters of the specified string expression, excluding trailing blanks.</para>
        /// </summary>
        /// <param name="argument">Is the string expression to be evaluated. argument can be a constant, variable, or column of either character or binary data.</param>
        public static SysFn Len(ScalarArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "LEN({0})",
                    argument.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument.Exception, "Sys.Len");
                return sql;
            });
        }

        /// <summary>
        /// <para>LOWER built-in function.</para>
        /// <para>Returns a character expression after converting uppercase character data to lowercase.</para>
        /// </summary>
        /// <param name="argument">Is an expression of character or binary data. argument can be a constant, variable, or column. argument must be of a data type that is implicitly convertible to varchar.</param>
        public static SysFn Lower(ScalarArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "LOWER({0})",
                    argument.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument.Exception, "Sys.Lower");
                return sql;
            });
        }

        /// <summary>
        /// <para>NCHAR built-in function.</para>
        /// <para>Returns a character expression after it removes leading blanks.</para>
        /// </summary>
        /// <param name="argument">Is an expression of character or binary data. argument can be a constant, variable, or column. argument must be of a data type, except text, ntext, and image, that is implicitly convertible to varchar.</param>
        public static SysFn LTrim(FunctionArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "LTRIM({0})",
                    argument.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument.Exception, "Sys.LTrim");
                return sql;
            });
        }

        /// <summary>
        /// <para>NCHAR built-in function.</para>
        /// <para>Returns the Unicode character with the specified integer code, as defined by the Unicode standard.</para>
        /// </summary>
        /// <param name="argument">When the collation of the database does not contain the supplementary character (SC) flag, this is a positive whole number from 0 through 65535 (0 through 0xFFFF). If a value outside this range is specified, NULL is returned. When the collation of the database supports the supplementary character (SC) flag, this is a positive whole number from 0 through 1114111 (0 through 0x10FFFF). If a value outside this range is specified, NULL is returned.</param>
        public static SysFn NChar(ScalarArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "NCHAR({0})",
                    argument.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument.Exception, "Sys.NChar");
                return sql;
            });
        }

        /// <summary>
        /// <para>PATINDEX built-in function.</para>
        /// <para>Returns the starting position of the first occurrence of a pattern in a specified expression, or zeros if the pattern is not found, on all valid text and character data types.</para>
        /// </summary>
        /// <param name="pattern">Is a character expression that contains the sequence to be found. Wildcard characters can be used; however, the % character must come before and follow pattern (except when you search for first or last characters). pattern is an expression of the character string data type category. pattern is limited to 8000 characters.</param>
        /// <param name="argument">Is an expression, typically a column that is searched for the specified pattern. argument is of the character string data type category.</param>
        public static SysFn Patindex(ScalarArgument pattern, ScalarArgument argument)
        {
            pattern = pattern ?? Designer.Null;
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "PATINDEX({0},{1})",
                    pattern.Build(buildContext, buildArgs),
                    argument.Build(buildContext, buildArgs));
                buildContext.TryTakeException(pattern.Exception, "Sys.Patindex");
                buildContext.TryTakeException(argument.Exception, "Sys.Patindex");
                return sql;
            });
        }

        /// <summary>
        /// <para>QUOTENAME built-in function.</para>
        /// <para>Returns a Unicode string with the delimiters added to make the input string a valid SQL Server delimited identifier.</para>
        /// </summary>
        /// <param name="argument">Is a string of Unicode character data. argument is sysname and is limited to 128 characters. Inputs greater than 128 characters return NULL.</param>
        /// <param name="quoteCharacter">Is a one-character string to use as the delimiter. Can be a single quotation mark ( ' ), a left or right bracket ( [ ] ), or a double quotation mark ( " ). If quote_character is not specified, brackets are used.</param>
        public static SysFn Quotename(FunctionArgument argument, Designer.QuoteCharacter quoteCharacter = Designer.QuoteCharacter.Brackets)
        {
            argument = argument ?? Designer.Null;

            switch (quoteCharacter)
            {
                case Designer.QuoteCharacter.Brackets:
                    return new SysFn((buildContext, buildArgs) =>
                    {
                        var sql = String.Format(
                            "QUOTENAME({0},'[]')",
                            argument.Build(buildContext, buildArgs));
                        buildContext.TryTakeException(argument.Exception, "Sys.Quotename");
                        return sql;
                    });
                case Designer.QuoteCharacter.SingleQuotationMark:
                   return new SysFn((buildContext, buildArgs) =>
                    {
                        var sql = String.Format(
                            "QUOTENAME({0},'''')",
                            argument.Build(buildContext, buildArgs));
                        buildContext.TryTakeException(argument.Exception, "Sys.Quotename");
                        return sql;
                    });
                case Designer.QuoteCharacter.DoubleQuotationMark:
                   return new SysFn((buildContext, buildArgs) =>
                    {
                        var sql = String.Format(
                            "QUOTENAME({0},'\"')",
                            argument.Build(buildContext, buildArgs));
                        buildContext.TryTakeException(argument.Exception, "Sys.Quotename");
                        return sql;
                    });
                default:
                   return new SysFn((buildContext, buildArgs) =>
                   {
                       var sql = String.Format(
                           "QUOTENAME({0},'[]')",
                           argument.Build(buildContext, buildArgs));
                       buildContext.TryTakeException(argument.Exception, "Sys.Quotename");
                       return sql;
                   });
            }
        }

        /// <summary>
        /// <para>REPLACE built-in function.</para>
        /// <para>Replaces all occurrences of a specified string value with another string value.</para>
        /// </summary>
        /// <param name="toSearch">Is the string expression to be searched. toSearch can be of a character or binary data type.</param>
        /// <param name="pattern">Is the substring to be found. pattern can be of a character or binary data type.</param>
        /// <param name="replaceWith">Is the replacement string. replaceWith can be of a character or binary data type.</param>
        public static SysFn Replace(
            ScalarArgument toSearch, ScalarArgument pattern, ScalarArgument replaceWith)
        {
            toSearch = toSearch ?? Designer.Null;
            pattern = pattern ?? Designer.Null;
            replaceWith = replaceWith ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "REPLACE({0},{1},{2})",
                    toSearch.Build(buildContext, buildArgs),
                    pattern.Build(buildContext, buildArgs),
                    replaceWith.Build(buildContext, buildArgs));
                buildContext.TryTakeException(toSearch.Exception, "Sys.Replace");
                buildContext.TryTakeException(pattern.Exception, "Sys.Replace");
                buildContext.TryTakeException(replaceWith.Exception, "Sys.Replace");
                return sql;
            });
        }

        /// <summary>
        /// <para>REPLICATE built-in function.</para>
        /// <para>Repeats a string value a specified number of times.</para>
        /// </summary>
        /// <param name="argument1">Is an expression of a character string or binary data type. argument1 can be either character or binary data.</param>
        /// <param name="argument2">Is an expression of any integer type, including bigint.</param>
        public static SysFn Replicate(FunctionArgument argument1, FunctionArgument argument2)
        {
            argument1 = argument1 ?? Designer.Null;
            argument2 = argument2 ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "REPLICATE({0},{1})",
                    argument1.Build(buildContext, buildArgs),
                    argument2.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument1.Exception, "Sys.Replicate");
                buildContext.TryTakeException(argument2.Exception, "Sys.Replicate");
                return sql;
            });
        }

        /// <summary>
        /// <para>REVERSE built-in function.</para>
        /// <para>Returns the reverse order of a string value.</para>
        /// </summary>
        /// <param name="argument">Is an expression of a string or binary data type. textual can be a constant, variable, or column of either character or binary data.</param>
        public static SysFn Reverse(ScalarArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "REVERSE({0})",
                    argument.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument.Exception, "Sys.Reverse");
                return sql;
            });
        }

        /// <summary>
        /// <para>RIGHT built-in function.</para>
        /// <para>Returns the right part of a character string with the specified number of characters.</para>
        /// </summary>
        /// <param name="argument1">Is an expression of character or binary data. argument1 can be a constant, variable, or column. argument1 can be of any data type, except text or ntext, that can be implicitly converted to varchar or nvarchar.</param>
        /// <param name="argument2">Is a positive integer that specifies how many characters of argument1 will be returned.</param>
        public static SysFn Right(ScalarArgument argument1, ScalarArgument argument2)
        {
            argument1 = argument1 ?? Designer.Null;
            argument2 = argument2 ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "RIGHT({0},{1})",
                    argument1.Build(buildContext, buildArgs),
                    argument2.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument1.Exception, "Sys.Right");
                buildContext.TryTakeException(argument2.Exception, "Sys.Right");
                return sql;
            });
        }

        /// <summary>
        /// <para>RTRIM built-in function.</para>
        /// <para>Returns a character string after truncating all trailing blanks.</para>
        /// </summary>
        /// <param name="argument">Is an expression of character data. argument can be a constant, variable, or column of either character or binary data.</param>
        public static SysFn RTrim(FunctionArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "RTRIM({0})",
                    argument.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument.Exception, "Sys.RTrim");
                return sql;
            });
        }

        /// <summary>
        /// <para>SOUNDEX built-in function.</para>
        /// <para>Returns a four-character (SOUNDEX) code to evaluate the similarity of two strings.</para>
        /// </summary>
        /// <param name="argument">Is an alphanumeric expression of character data. argument can be a constant, variable, or column.</param>
        public static SysFn Soundex(FunctionArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "SOUNDEX({0})",
                    argument.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument.Exception, "Sys.Soundex");
                return sql;
            });
        }

        /// <summary>
        /// <para>SPACE built-in function.</para>
        /// <para>Returns a string of repeated spaces.</para>
        /// </summary>
        /// <param name="argument">Is a positive integer that indicates the number of spaces. If argument is negative, a null string is returned.</param>
        public static SysFn Space(ScalarArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "SPACE({0})",
                    argument.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument.Exception, "Sys.Space");
                return sql;
            });
        }

        /// <summary>
        /// <para>STR built-in function.</para>
        /// <para>Returns character data converted from numeric data.</para>
        /// </summary>
        /// <param name="argument">Is an expression of approximate numeric (float) data type with a decimal point.</param>
        /// <param name="length">Is the total length. This includes decimal point, sign, digits, and spaces.</param>
        /// <param name="decimalPrecision">Is the number of places to the right of the decimal point. decimalPrecision must be less than or equal to 16.</param>
        public static SysFn Str(ScalarArgument argument, ScalarArgument length, ScalarArgument decimalPrecision)
        {
            argument = argument ?? Designer.Null;
            length = length ?? Designer.Null;
            decimalPrecision = decimalPrecision ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "STR({0},{1},{2})",
                    argument.Build(buildContext, buildArgs),
                    length.Build(buildContext, buildArgs),
                    decimalPrecision.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument.Exception, "Sys.Str");
                buildContext.TryTakeException(length.Exception, "Sys.Str");
                buildContext.TryTakeException(decimalPrecision.Exception, "Sys.Str");
                return sql;
            });
        }

        /// <summary>
        /// <para>STUFF built-in function.</para>
        /// <para>The STUFF function inserts a string into another string. It deletes a specified length of characters in the first string at the start position and then inserts the second string into the first string at the start position.</para>
        /// </summary>
        /// <param name="argument">Is an expression of character data. textual can be a constant, variable, or column of either character or binary data.</param>
        /// <param name="start">Is an integer value that specifies the location to start deletion and insertion.</param>
        /// <param name="length">Is an integer that specifies the number of characters to delete.</param>
        /// <param name="replaceWith">Is an expression of character data. replaceWith can be a constant, variable, or column of either character or binary data.</param>
        public static SysFn Stuff(ScalarArgument argument, ScalarArgument start, ScalarArgument length, ScalarArgument replaceWith)
        {
            argument = argument ?? Designer.Null;
            start = start ?? Designer.Null;
            length = length ?? Designer.Null;
            if (replaceWith == null) { replaceWith = Designer.Null; }

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "STUFF({0},{1},{2},{3})",
                    argument.Build(buildContext, buildArgs),
                    start.Build(buildContext, buildArgs),
                    length.Build(buildContext, buildArgs),
                    replaceWith.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument.Exception, "Sys.Stuff");
                buildContext.TryTakeException(start.Exception, "Sys.Stuff");
                buildContext.TryTakeException(length.Exception, "Sys.Stuff");
                buildContext.TryTakeException(replaceWith.Exception, "Sys.Stuff");
                return sql;
            });
        }

        /// <summary>
        /// <para>SUBSTRING built-in function.</para>
        /// <para>Returns part of a character, binary, text, or image expression.</para>
        /// </summary>
        /// <param name="argument">Is a character, binary, text, ntext, or image expression.</param>
        /// <param name="start">Is an integer or bigint expression that specifies where the returned characters start.</param>
        /// <param name="length">Is a positive integer or bigint expression that specifies how many characters of the expression will be returned.</param>
        public static SysFn Substring(ScalarArgument argument, ScalarArgument start, ScalarArgument length)
        {
            argument = argument ?? Designer.Null;
            start = start ?? Designer.Null;
            length = length ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "SUBSTRING({0},{1},{2})",
                    argument.Build(buildContext, buildArgs),
                    start.Build(buildContext, buildArgs),
                    length.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument.Exception, "Sys.Substring");
                buildContext.TryTakeException(start.Exception, "Sys.Substring");
                buildContext.TryTakeException(length.Exception, "Sys.Substring");
                return sql;
            });
        }

        /// <summary>
        /// <para>UNICODE built-in function.</para>
        /// <para>Returns the integer value, as defined by the Unicode standard, for the first character of the input expression.</para>
        /// </summary>
        /// <param name="argument">Is an nchar or nvarchar expression.</param>
        public static SysFn Unicode(ScalarArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "UNICODE({0})",
                    argument.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument.Exception, "Sys.Unicode");
                return sql;
            });
        }

        /// <summary>
        /// <para>UPPER built-in function.</para>
        /// <para>Returns a character expression with lowercase character data converted to uppercase.</para>
        /// </summary>
        /// <param name="argument">Is an expression of character data. textual can be a constant, variable, or column of either character or binary data.</param>
        public static SysFn Upper(ScalarArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
            {
                var sql = String.Format(
                    "UPPER({0})",
                    argument.Build(buildContext, buildArgs));
                buildContext.TryTakeException(argument.Exception, "Sys.Upper");
                return sql;
            });
        }

    }
}
