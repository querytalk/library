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
        /// <para>ISNULL built-in function.</para>
        /// <para>Replaces NULL with the specified replacement value.</para>
        /// </summary>
        /// <param name="argument">Is the expression to be checked for NULL. The argument can be of any type.</param>
        /// <param name="replacement">Is the expression to be returned if check expression is NULL. replacement value must be of a type that is implicitly convertible to the type of check expresssion.</param>
        public static SysFn IsNull(ScalarArgument argument, ScalarArgument replacement)
        {
            argument = argument ?? Designer.Null;
            replacement = replacement ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "ISNULL({0},{1})",
                        argument.Build(buildContext, buildArgs),
                        replacement.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.IsNull");
                    buildContext.TryTakeException(replacement.Exception, "Sys.IsNull");
                    return sql;
                });
        }

        /// <summary>
        /// <para>CHECKSUM built-in function.</para>
        /// <para>Returns the checksum value computed over a row of a table, or over a list of expressions. CHECKSUM is intended for use in building hash indexes.</para>
        /// </summary>
        public static SysFn Checksum()
        {
            return new SysFn("CHECKSUM(*)");
        }

        /// <summary>
        /// <para>CHECKSUM built-in function.</para>
        /// <para>Returns the checksum value computed over a row of a table, or over a list of expressions. CHECKSUM is intended for use in building hash indexes.</para>
        /// </summary>
        /// <param name="argument">Is an expression of any type except a noncomparable data type.</param>
        public static SysFn Checksum(ScalarArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "CHECKSUM({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.Checksum");
                    return sql;
                });
        }

        /// <summary>
        /// <para>BINARY_CHECKSUM built-in function.</para>
        /// <para>Returns the binary checksum value computed over a row of a table or over a list of expressions. BINARY_CHECKSUM can be used to detect changes to a row of a table.</para>
        /// </summary>
        public static SysFn BinaryChecksum()
        {
            return new SysFn("BINARY_CHECKSUM(*)");
        }

        /// <summary>
        /// <para>BINARY_CHECKSUM built-in function.</para>
        /// <para>Returns the binary checksum value computed over a row of a table or over a list of expressions. BINARY_CHECKSUM can be used to detect changes to a row of a table.</para>
        /// </summary>
        /// <param name="argument">Is an expression of any type. BINARY_CHECKSUM ignores expressions of noncomparable data types in its computation.</param>
        public static SysFn BinaryChecksum(ScalarArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "BINARY_CHECKSUM({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.BinaryChecksum");
                    return sql;
                });
        }

        /// <summary>
        /// <para>ISNUMERIC built-in function.</para>
        /// <para>Determines whether an expression is a valid numeric type.</para>
        /// </summary>
        /// <param name="argument">Is the expression to be evaluated.</param>
        public static SysFn IsNumeric(ScalarArgument argument)
        {
            argument = argument ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "ISNUMERIC({0})",
                        argument.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument.Exception, "Sys.IsNumeric");
                    return sql;
                });
        }

        /// <summary>
        /// <para>NEWID built-in function.</para>
        /// <para>Creates a unique value of type uniqueidentifier.</para>
        /// </summary>
        public static SysFn NewId()
        {
            return new SysFn("NEWID()");
        }

        /// <summary>
        /// <para>NULLIF built-in function.</para>
        /// <para>Returns a null value if the two specified expressions are equal.</para>
        /// </summary>
        /// <param name="argument1">Is the first scalar expression.</param>
        /// <param name="argument2">Is the second scalar expression.</param>
        public static SysFn NullIf(ScalarArgument argument1, ScalarArgument argument2)
        {
            argument1 = argument1 ?? Designer.Null;
            argument2 = argument2 ?? Designer.Null;

            return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "NULLIF({0},{1})",
                        argument1.Build(buildContext, buildArgs),
                        argument2.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument1.Exception, "Sys.NullIf");
                    buildContext.TryTakeException(argument2.Exception, "Sys.NullIf");
                    return sql;
                });
        }

        /// <summary>
        /// <para>COAELSCE built-in function.</para>
        /// <para>Returns the first non-null expression among its arguments.</para>
        /// </summary>
        /// <param name="argument1">Is the first expression of any type.</param>
        /// <param name="argument2">Is the second expression of any type.</param>
        /// <param name="otherArguments">Are all other expressions of any type.</param>
        public static SysFn Coalesce(FunctionArgument argument1, FunctionArgument argument2, 
            params FunctionArgument[] otherArguments)
        {
            if (argument1 == null) { argument1 = Designer.Null; }
            if (argument2 == null) { argument2 = Designer.Null; }

            if (otherArguments == null || otherArguments.Length == 0)
            {
                return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "COALESCE({0},{1})",
                        argument1.Build(buildContext, buildArgs),
                        argument2.Build(buildContext, buildArgs));
                    buildContext.TryTakeException(argument1.Exception, "Sys.Coalesce");
                    buildContext.TryTakeException(argument2.Exception, "Sys.Coalesce");
                    return sql;
                });
            }
            else
            {
                return new SysFn((buildContext, buildArgs) =>
                {
                    var sql = String.Format(
                        "COALESCE({0},{1},{2})",
                        argument1.Build(buildContext, buildArgs),
                        argument2.Build(buildContext, buildArgs),
                        FunctionArgument.Concatenate(otherArguments, buildContext, buildArgs));
                    buildContext.TryTakeException(argument1.Exception, "Sys.Coalesce");
                    buildContext.TryTakeException(argument2.Exception, "Sys.Coalesce");
                    return sql;
                });
            }
        }

        /// <summary>
        /// <para>XACT_STATE built-in function.</para>
        /// <para>Is a scalar function that reports the user transaction state of a current running request. XACT_STATE indicates whether the request has an active user transaction, and whether the transaction is capable of being committed.</para>
        /// </summary>
        public static SysFn XactState()
        {
            return new SysFn("XACT_STATE()");
        }

        /// <summary>
        /// <para>SCOPE_IDENTITY built-in function.</para>
        /// <para>Returns the last identity value inserted into an identity column in the same scope. A scope is a module: a stored procedure, trigger, function, or batch. Therefore, two statements are in the same scope if they are in the same stored procedure, function, or batch.</para>
        /// </summary>
        public static SysFn ScopeIdentity()
        {
            return new SysFn("SCOPE_IDENTITY()");
        }
    }
}
