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
    /// Represents a column of a primary key/unique key definition.
    /// </summary>
    public sealed class OrderedColumnArgument : Argument,
        IScalar
    {
        internal OrderedColumnArgument(Chainer arg)
            : base(arg)
        {
            TryTakeAll(arg);
        }

        #region Identifier

        internal OrderedColumnArgument(Identifier arg)
            : this((Chainer)arg)
        {
            CtorBody(arg);
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of OrderedColumnArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator OrderedColumnArgument(Identifier arg)
        {
            return new OrderedColumnArgument(arg);
        }

        #endregion

        #region System.String

        internal OrderedColumnArgument(System.String arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                string sql;
                if (ProcessVariable(buildContext, buildArgs, out sql))
                {
                    return sql;
                }

                return Filter.DelimitColumnMultiPart(arg, out chainException);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of OrderedColumnArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator OrderedColumnArgument(System.String arg)
        {
            return new OrderedColumnArgument(arg);
        }

        #endregion

        #region Ordered

        internal OrderedColumnArgument(OrderedChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of OrderedColumnArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator OrderedColumnArgument(OrderedChainer arg)
        {
            return new OrderedColumnArgument(arg);
        }

        #endregion

        #region DbColumn

        internal OrderedColumnArgument(DbColumn arg)
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
        /// Implicitly converts an argument into the object of OrderedColumnArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator OrderedColumnArgument(DbColumn arg)
        {
            return new OrderedColumnArgument(arg);
        }

        #endregion

        internal static string Concatenate(
            OrderedColumnArgument[] arguments,
            BuildContext buildContext, 
            BuildArgs buildArgs)
        {
            return String.Join(Text.CommaWithSpace,
                arguments.Select(argument =>
                {
                    if (argument.IsUndefined())
                    {
                        var exception = new QueryTalkException("OrderedColumnArgument",
                            QueryTalkExceptionType.ArgumentNull,
                            "column = undefined");
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

                    return sql;

                }).ToArray());
        }

    }
}
