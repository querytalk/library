#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents an argument of an aggregate built-in function.
    /// </summary> 
    public sealed class AggregateArgument : Argument
    {
        private AggregateArgument(Chainer arg)
            : base(arg)
        {
            TryTakeAll(arg);
        }

        #region Identifier

        internal AggregateArgument(Identifier arg)
            : this((Chainer)arg)
        {
            CtorBody(arg);
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of AggregateArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator AggregateArgument(Identifier arg)
        {
            return new AggregateArgument(arg);
        }

        #endregion

        #region System.String

        internal AggregateArgument(System.String arg)
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
        /// Implicitly converts an argument into the object of AggregateArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator AggregateArgument(System.String arg)
        {
            return new AggregateArgument(arg);
        }

        #endregion

        #region Value

        internal AggregateArgument(Value arg)
            : this(arg as Chainer)
        { 
            CheckNull(Arg(() => arg, arg));
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of AggregateArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator AggregateArgument(Value arg)
        {
            return new AggregateArgument(arg);
        }

        #endregion

        #region CLR Types (Numeric only)

        #region System.Byte

        internal AggregateArgument(System.Byte arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of AggregateArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator AggregateArgument(System.Byte arg)
        {
            return new AggregateArgument(arg);
        }

        #endregion

        #region System.Decimal

        internal AggregateArgument(System.Decimal arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of AggregateArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator AggregateArgument(System.Decimal arg)
        {
            return new AggregateArgument(arg);
        }

        #endregion

        #region System.Double

        internal AggregateArgument(System.Double arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of AggregateArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator AggregateArgument(System.Double arg)
        {
            return new AggregateArgument(arg);
        }

        #endregion

        #region System.Int16

        internal AggregateArgument(System.Int16 arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of AggregateArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator AggregateArgument(System.Int16 arg)
        {
            return new AggregateArgument(arg);
        }

        #endregion

        #region System.Int32

        internal AggregateArgument(System.Int32 arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of AggregateArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator AggregateArgument(System.Int32 arg)
        {
            return new AggregateArgument(arg);
        }

        #endregion

        #region System.Int64

        internal AggregateArgument(System.Int64 arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of AggregateArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator AggregateArgument(System.Int64 arg)
        {
            return new AggregateArgument(arg);
        }

        #endregion

        #region System.Single

        internal AggregateArgument(System.Single arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of AggregateArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator AggregateArgument(System.Single arg)
        {
            return new AggregateArgument(arg);
        }

        #endregion

        #endregion

        #region SysFn

        internal AggregateArgument(SysFn arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of AggregateArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator AggregateArgument(SysFn arg)
        {
            return new AggregateArgument(arg);
        }

        #endregion

        #region Case

        internal AggregateArgument(CaseExpressionChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of AggregateArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator AggregateArgument(CaseExpressionChainer arg)
        {
            return new AggregateArgument(arg);
        }

        #endregion

        #region Of

        internal AggregateArgument(OfChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of AggregateArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator AggregateArgument(OfChainer arg)
        {
            return new AggregateArgument(arg);
        }

        #endregion

        #region Udf

        internal AggregateArgument(Udf arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of AggregateArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator AggregateArgument(Udf arg)
        {
            return new AggregateArgument(arg);
        }

        #endregion

        #region Expression

        internal AggregateArgument(Expression arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of AggregateArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator AggregateArgument(Expression arg)
        {
            return new AggregateArgument(arg);
        }

        #endregion

        #region DbColumn

        internal AggregateArgument(DbColumn arg)
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
        /// Implicitly converts an argument into the object of AggregateArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator AggregateArgument(DbColumn arg)
        {
            return new AggregateArgument(arg);
        }

        #endregion

    }
}
