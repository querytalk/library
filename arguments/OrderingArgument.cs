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
    /// Represents a column of ordering and ranking methods.
    /// </summary>
    public sealed class OrderingArgument : Argument
    {
        private OrderingArgument(Chainer arg)
            : base(arg)
        {
            TryTakeAll(arg);
        }

        #region System.String

        internal OrderingArgument(System.String arg)
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
        /// Implicitly converts an argument into the object of OrderingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator OrderingArgument(System.String arg)
        {
            return new OrderingArgument(arg);
        }

        #endregion

        #region Identifier

        internal OrderingArgument(Identifier arg)
            : this((Chainer)arg)
        {
            CtorBody(arg);
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of OrderingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator OrderingArgument(Identifier arg)
        {
            return new OrderingArgument(arg);
        }

        #endregion

        #region Sys

        internal OrderingArgument(SysFn arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of OrderingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator OrderingArgument(SysFn arg)
        {
            return new OrderingArgument(arg);
        }

        #endregion

        #region Case

        internal OrderingArgument(CaseExpressionChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of OrderingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator OrderingArgument(CaseExpressionChainer arg)
        {
            return new OrderingArgument(arg);
        }

        #endregion

        #region Of

        internal OrderingArgument(OfChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of OrderingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator OrderingArgument(OfChainer arg)
        {
            return new OrderingArgument(arg);
        }

        #endregion

        #region Udf

        internal OrderingArgument(Udf arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of OrderingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator OrderingArgument(Udf arg)
        {
            return new OrderingArgument(arg);
        }

        #endregion

        #region View

        internal OrderingArgument(View arg)
            : base(arg)
        {
            if (CheckNull(Arg(() => arg, arg)))
            {
                Build = (buildContext, buildArgs) =>
                {
                    return Filter.Enclose(arg.Build(buildContext, buildArgs));
                };
            }
            else
            {
                chainException.Extra = Text.Free.ScalarViewNullExtra;
            }

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of OrderingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator OrderingArgument(View arg)
        {
            return new OrderingArgument(arg);
        }

        #endregion

        #region Expression

        internal OrderingArgument(Expression arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of OrderingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator OrderingArgument(Expression arg)
        {
            return new OrderingArgument(arg);
        }

        #endregion

        #region Ordered

        internal OrderingArgument(OrderedChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of OrderingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator OrderingArgument(OrderedChainer arg)
        {
            return new OrderingArgument(arg);
        }

        #endregion

        #region Concatenator

        internal OrderingArgument(Concatenator arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildConcatenator(buildContext, buildArgs, arg);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of OrderingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator OrderingArgument(Concatenator arg)
        {
            return new OrderingArgument(arg);
        }

        #endregion

        #region DbColumn

        internal OrderingArgument(DbColumn arg)
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
        /// Implicitly converts an argument into the object of OrderingArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator OrderingArgument(DbColumn arg)
        {
            return new OrderingArgument(arg);
        }

        #endregion 

        internal static string Concatenate(
            OrderingArgument[] arguments,
            BuildContext buildContext, 
            BuildArgs buildArgs,
            bool isEnclosed)
        {
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
                            "OrderingArgument.Concatenate",
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

                    var sql = argument.Build(buildContext, buildArgs);

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

                    return isEnclosed ? String.Format("({0})", sql) : sql;
                            
                }).ToArray());
        }

    }
}
