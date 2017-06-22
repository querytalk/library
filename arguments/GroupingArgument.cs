#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Linq;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents an expression of aggregate methods.
    /// </summary> 
    public sealed class GroupingArgument : Argument
    {
        private GroupingArgument(Chainer arg)
            : base(arg)
        {
            TryTakeAll(arg);
        }

        #region System.String

        internal GroupingArgument(System.String arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                string sql;
                if (ProcessVariable(buildContext, buildArgs, out sql))
                {
                    return sql;
                }

                sql = Filter.DelimitMultiPartOrParam(arg, IdentifierType.ColumnOrParam, out chainException);
                buildContext.TryTakeException(chainException);
                return sql;
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of GroupingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator GroupingArgument(System.String arg)
        {
            return new GroupingArgument(arg);
        }

        #endregion

        #region Identifier

        internal GroupingArgument(Identifier arg)
            : this((Chainer)arg)
        {
            CtorBody(arg);
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of GroupingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator GroupingArgument(Identifier arg)
        {
            return new GroupingArgument(arg);
        }

        #endregion

        #region Sys

        internal GroupingArgument(SysFn arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of GroupingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator GroupingArgument(SysFn arg)
        {
            return new GroupingArgument(arg);
        }

        #endregion

        #region Case

        internal GroupingArgument(CaseExpressionChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of GroupingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator GroupingArgument(CaseExpressionChainer arg)
        {
            return new GroupingArgument(arg);
        }

        #endregion

        #region Of

        internal GroupingArgument(OfChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of GroupingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator GroupingArgument(OfChainer arg)
        {
            return new GroupingArgument(arg);
        }

        #endregion

        #region Udf

        internal GroupingArgument(Udf arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of GroupingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator GroupingArgument(Udf arg)
        {
            return new GroupingArgument(arg);
        }

        #endregion

        #region Expression

        internal GroupingArgument(Expression arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of GroupingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator GroupingArgument(Expression arg)
        {
            return new GroupingArgument(arg);
        }

        #endregion

        #region Concatenator

        internal GroupingArgument(Concatenator arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildConcatenator(buildContext, buildArgs, arg);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of GroupingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator GroupingArgument(Concatenator arg)
        {
            return new GroupingArgument(arg);
        }

        #endregion

        #region DbColumn

        internal GroupingArgument(DbColumn arg)
            : base(arg)
        {
            if (arg != null)
            {
                Build = arg.Build;
            }
            else
            {
                CheckNull(Arg(() => arg, arg));
            }

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of GroupingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator GroupingArgument(DbColumn arg)
        {
            return new GroupingArgument(arg);
        }

        #endregion

        internal static string Concatenate(
            GroupingArgument[] arguments,
            BuildContext buildContext, 
            BuildArgs buildArgs, 
            bool isEnclosed)
        {
            // check
            if (arguments == null || arguments.Length == 0)
            {
                return "()";
            }

            return String.Join(Text.CommaWithSpace,
                arguments.Select(argument =>
                {
                    if (argument.IsUndefined())
                    {
                        var exception = new QueryTalkException(
                            "GroupingArgument.Concatenate",
                            QueryTalkExceptionType.ArgumentNull,
                            String.Format("column = undefined"));
                        buildContext.TryTakeException(exception);
                        return null;
                    }

                    if (argument.Exception != null)
                    {
                        buildContext.TryTakeException(argument.Exception);
                        return null;
                    }

                    string sql = argument.Build(buildContext, buildArgs);

                    if (argument.Exception != null)
                    {
                        buildContext.TryTakeException(argument.Exception);
                        return null;
                    }

                    if (buildContext.Exception != null)
                    {
                        return null;
                    }

                    buildContext.TryTake(argument);

                    if (sql == "[]")
                    {
                        return "()";
                    }
                    else
                    {
                        return isEnclosed ?
                            String.Format("({0})", sql) : sql;
                    }
                }).ToArray());
        }

    }
}
