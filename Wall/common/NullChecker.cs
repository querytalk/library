#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Linq;

namespace QueryTalk.Wall
{
    internal static class NullChecker
    {
        // The general method used throughout the library to perform null check.
        //   note:
        //     Some classes can contain null value (Argument, Value).
        //     DBNull and empty object are considered as null.
        internal static bool IsUndefined(this object arg, bool allowValueNull = false)
        {
            if (arg == null)
            {
                return true;
            }

            if (allowValueNull && arg is Value)
            {
                return false;
            }

            if (arg is Argument)
            {
                arg = ((Argument)arg).Original;
            }

            if (arg == null)
            {
                return true;
            }

            if (arg is Value)
            {
                return (Value)arg == Designer.Null;
            }

            var type = arg.GetType();

            if (type == typeof(System.DBNull))
            {
                return true;
            }

            if (type == typeof(System.Object))
            {
                return true;
            }

            return false;
        }

        internal static bool IsNullOrDbNull(this object value)
        {
            return value == null || value == DBNull.Value;
        }

        // check grouping arguments against null
        internal static QueryTalkException IsGroupingNull(this GroupingArgument[] arguments, string method)
        {
            QueryTalkException exception = null;

            // check collection
            if (arguments == null || arguments.Length == 0)
            {
                exception = new QueryTalkException(
                    "Sys.GroupingId",
                    QueryTalkExceptionType.CollectionNullOrEmpty,
                    "arguments = undefined",
                    method);
            }

            // check each item
            if (arguments != null && arguments.Where(argument => argument == null).Any())
            {
                exception = new QueryTalkException(
                    "Sys.GroupingId",
                    QueryTalkExceptionType.CollectionNullOrEmpty,
                    "argument = undefined",
                    method);
            }

            return exception;
        }

        // Performs null reference check if the Argument object. 
        //   note:
        //     Argument cast to System.Object is necessary in order to avoid implicit casting operator ==.
        internal static bool IsNullReference(this Argument value)
        {
            return (object)value == null;
        }

    }
}
