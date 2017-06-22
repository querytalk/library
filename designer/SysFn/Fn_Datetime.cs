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
        /// <para>ISDATE built-in function.</para>
        /// <para>Returns 1 if the expression is a valid date, time, or datetime value; otherwise, 0.</para>
        /// </summary>
        /// <param name="argument">Is a character string or expression that can be converted to a character string. The expression must be less than 4,000 characters. Date and time data types, except datetime and smalldatetime, are not allowed as the argument for ISDATE.</param>
        public static SysFn IsDate(ScalarArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "ISDATE({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.IsDate");
                    return sql;
                });
        }

        /// <summary>
        /// <para>SYSDATETIME built-in function.</para>
        /// <para>Returns a datetime2(7) value that contains the date and time of the computer on which the instance of SQL Server is running.</para>
        /// </summary>
        public static SysFn SysDatetime()
        {
            return new SysFn("SYSDATETIME()");
        }

        /// <summary>
        /// <para>SYSDATETIMEOFFSET built-in function.</para>
        /// <para>Returns a datetimeoffset(7) value that contains the date and time of the computer on which the instance of SQL Server is running. The time zone offset is included.</para>
        /// </summary>
        public static SysFn SysDatetimeOffset()
        {
            return new SysFn("SYSDATETIMEOFFSET()");
        }

        /// <summary>
        /// <para>SYSUTCDATETIME built-in function.</para>
        /// <para>Returns a datetime2 value that contains the date and time of the computer on which the instance of SQL Server is running. The date and time is returned as UTC time (Coordinated Universal Time). The fractional second precision specification has a range from 1 to 7 digits. The default precision is 7 digits.</para>
        /// </summary>
        public static SysFn SysUtcDatetime()
        {
            return new SysFn("SYSUTCDATETIME()");
        }

        /// <summary>
        /// <para>CURRENT_TIMESTAMP built-in function.</para>
        /// <para>Returns the current database system timestamp as a datetime value without the database time zone offset. This value is derived from the operating system of the computer on which the instance of SQL Server is running.</para>
        /// </summary>
        public static SysFn CurrentTimestamp
        {
            get
            {
                return new SysFn("CURRENT_TIMESTAMP");
            }
        }

        /// <summary>
        /// <para>GETDATE built-in function.</para>
        /// <para>Returns the current database system timestamp as a datetime value without the database time zone offset. This value is derived from the operating system of the computer on which the instance of SQL Server is running.</para>
        /// </summary>
        public static SysFn GetDate()
        {
            return new SysFn("GETDATE()");
        }

        /// <summary>
        /// <para>GETUTCDATE built-in function.</para>
        /// <para>Returns the current database system timestamp as a datetime value. The database time zone offset is not included. This value represents the current UTC time (Coordinated Universal Time). This value is derived from the operating system of the computer on which the instance of SQL Server is running.</para>
        /// </summary>
        public static SysFn GetUtcDate()
        {
            return new SysFn("GETUTCDATE()");
        }

        /// <summary>
        /// <para>DATENAME built-in function.</para>
        /// <para>Returns a character string that represents the specified datepart of the specified date.</para>
        /// </summary>
        /// <param name="datePart">Is the part of the date to return.</param>
        /// <param name="date">Is an expression that can be resolved to a time, date, smalldatetime, datetime, datetime2, or datetimeoffset value. date can be an expression, column expression, user-defined variable, or string literal.</param>
        public static SysFn Datename(Designer.PartOfDate datePart, ScalarArgument date)
        {
            date = date ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "DATENAME({0},{1})",
                        datePart.ToUpperCase(),
                        date.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(date.Exception, "Sys.Datename");
                    return sql;
                });
        }

        /// <summary>
        /// <para>DATEPART built-in function.</para>
        /// <para>Returns an integer that represents the specified datepart of the specified date.</para>
        /// </summary>
        /// <param name="partOfDate">Is the part of date (a date or time value) for which an integer will be returned.</param>
        /// <param name="date">Is an expression that can be resolved to a time, date, smalldatetime, datetime, datetime2, or datetimeoffset value. date can be an expression, column expression, user-defined variable, or string literal.</param>
        public static SysFn Datepart(Designer.PartOfDate partOfDate, ScalarArgument date)
        {
            date = date ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "DATEPART({0},{1})",
                        partOfDate.ToUpperCase(),
                        date.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(date.Exception, "Sys.Datepart");
                    return sql;
                });
        }

        /// <summary>
        /// <para>DATEDIFF built-in function.</para>
        /// <para>Returns an integer that represents the specified datepart of the specified date.</para>
        /// </summary>
        /// <param name="datePart">Is the part of date (a date or time value) for which an integer will be returned.</param>
        /// <param name="startDate">Is an expression that can be resolved to a time, date, smalldatetime, datetime, datetime2, or datetimeoffset value. date can be an expression, column expression, user-defined variable or string literal.</param>
        /// <param name="endDate">Is an expression that can be resolved to a time, date, smalldatetime, datetime, datetime2, or datetimeoffset value. date can be an expression, column expression, user-defined variable or string literal.</param>
        public static SysFn Datediff(Designer.PartOfDate datePart, ScalarArgument startDate, ScalarArgument endDate)
        {
            startDate = startDate ?? Designer.Null;
            endDate = endDate ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "DATEDIFF({0},{1},{2})",
                        datePart.ToUpperCase(),
                        startDate.Build(buildContext, buildArgs),
                        endDate.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(startDate.Exception, "Sys.Datediff");
                    buildContext.TryTakeException(endDate.Exception, "Sys.Datediff");
                    return sql;
                });
        }

        /// <summary>
        /// <para>DATEADD built-in function.</para>
        /// <para>Returns a specified date with the specified number interval (signed integer) added to a specified datepart of that date.</para>
        /// </summary>
        /// <param name="datePart">Is the part of date to which an integer number is added. </param>
        /// <param name="number">Is an expression that can be resolved to an int that is added to a datepart of date. User-defined variables are valid.</param>
        /// <param name="date">Is an expression that can be resolved to a time, date, smalldatetime, datetime, datetime2, or datetimeoffset value. date can be an expression, column expression, user-defined variable, or string literal. If the expression is a string literal, it must resolve to a datetime.</param>
        public static SysFn Dateadd(Designer.PartOfDate datePart, ScalarArgument number, ScalarArgument date)
        {
            number = number ?? Designer.Null; 
            date = date ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "DATEADD({0},{1},{2})",
                        datePart.ToUpperCase(),
                        number.Build(buildContext, buildArgs),
                        date.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(number.Exception, "Sys.Dateadd");
                    buildContext.TryTakeException(date.Exception, "Sys.Dateadd");
                    return sql;
                });
        }

        /// <summary>
        /// <para>DAY built-in function.</para>
        /// <para>Returns an integer representing the day (day of the month) of the specified date.</para>
        /// </summary>
        /// <param name="date">Is an expression that can be resolved to a time, date, smalldatetime, datetime, datetime2, or datetimeoffset value. The date argument can be an expression, column expression, user-defined variable or string literal.</param>
        public static SysFn Day(ScalarArgument date)
        {
            date = date ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "DAY({0})",
                        date.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(date.Exception, "Sys.Day");
                    return sql;
                });
        }

        /// <summary>
        /// <para>MONTH built-in function.</para>
        /// <para>Returns an integer that represents the month of the specified date.</para>
        /// </summary>
        /// <param name="date">Is an expression that can be resolved to a time, date, smalldatetime, datetime, datetime2, or datetimeoffset value. The date argument can be an expression, column expression, user-defined variable or string literal.</param>
        public static SysFn Month(ScalarArgument date)
        {
            date = date ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "MONTH({0})",
                        date.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(date.Exception, "Sys.Month");
                    return sql;
                });
        }

        /// <summary>
        /// <para>YEAR built-in function.</para>
        /// <para>Returns an integer that represents the year of the specified date.</para>
        /// </summary>
        /// <param name="date">Is an expression that can be resolved to a time, date, smalldatetime, datetime, datetime2, or datetimeoffset value. The date argument can be an expression, column expression, user-defined variable or string literal.</param>
        public static SysFn Year(ScalarArgument date)
        {
            date = date ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "YEAR({0})",
                        date.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(date.Exception, "Sys.Year");
                    return sql;
                });
        }

        /// <summary>
        /// <para>SWITCHOFFSET built-in function.</para>
        /// <para>Returns a datetimeoffset value that is changed from the stored time zone offset to a specified new time zone offset.</para>
        /// </summary>
        /// <param name="dateTimeOffset">Is an expression that can be resolved to a datetimeoffset(n) value.</param>
        /// <param name="timeZone">Is a character string in the format [+|-]TZH:TZM or a signed integer (of minutes) that represents the time zone offset, and is assumed to be daylight-saving aware and adjusted.</param>
        public static SysFn SwitchOffset(ScalarArgument dateTimeOffset, string timeZone)
        {
            dateTimeOffset = dateTimeOffset ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "SWITCHOFFSET({0},{1})",
                        dateTimeOffset.Build(buildContext, buildArgs),
                        Mapping.Build(timeZone, Mapping.DefaultStringType));
                    buildContext.TryTakeException(dateTimeOffset.Exception, "Sys.SwitchOffset");
                    return sql;
                });
        }

        /// <summary>
        /// <para>TODATETIMEOFFSET built-in function.</para>
        /// <para>Returns a datetimeoffset value that is translated from a datetime2 expression.</para>
        /// </summary>
        /// <param name="date">Is an expression that resolves to a datetime2 value.</param>
        /// <param name="timeZone">Is an expression that represents the time zone offset in minutes (if an integer), for example -120, or hours and minutes (if a string), for example ‘+13.00’. The range is +14 to -14 (in hours). The expression is interpreted in local time for the specified time_zone.</param>
        public static SysFn ToDatetimeOffset(ScalarArgument date, string timeZone)
        {
            date = date ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "TODATETIMEOFFSET({0},{1})",
                        date.Build(buildContext, buildArgs),
                        Mapping.Build(timeZone, Mapping.DefaultStringType));
                    buildContext.TryTakeException(date.Exception, "Sys.ToDatetimeOffset");
                    return sql;
                });
        }
      
    }
}
