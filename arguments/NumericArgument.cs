#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents a numeric argument of a built-in function.
    /// </summary>
    public sealed class NumericArgument : Argument
    {
        private NumericArgument(Chainer arg)
            : base(arg)
        {
            TryTakeAll(arg);
        }

        #region Identifier

        internal NumericArgument(Identifier arg)
            : this((Chainer)arg)
        {
            CtorBody(arg);
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(Identifier arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

        #region Value

        internal NumericArgument(Value arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(Value arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

        #region System.String
        // column identifier has higher precedence over System.String arg

        internal NumericArgument(System.String arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                string sql;
                if (ProcessVariable(buildContext, buildArgs, out sql))
                {
                    return sql;
                }

                // process string as value
                if (buildContext.IsCurrentStringAsValue)
                {
                    return buildContext.BuildString(arg);
                } 

                sql = Filter.DelimitMultiPartOrParam(arg, IdentifierType.ColumnOrParam, out chainException);
                buildContext.TryTakeException(chainException);
                return sql;
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(System.String arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

        #region CLR Types (Numeric only)

        #region System.Byte

        internal NumericArgument(System.Byte arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(System.Byte arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

        #region System.Decimal

        internal NumericArgument(System.Decimal arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(System.Decimal arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

        #region System.Double

        internal NumericArgument(System.Double arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(System.Double arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

        #region System.Int16

        internal NumericArgument(System.Int16 arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(System.Int16 arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

        #region System.Int32

        internal NumericArgument(System.Int32 arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(System.Int32 arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

        #region System.Int64

        internal NumericArgument(System.Int64 arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(System.Int64 arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

        #region System.Single

        internal NumericArgument(System.Single arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(System.Single arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

        #endregion

        #region Sys

        internal NumericArgument(SysFn arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(SysFn arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

        #region Case

        internal NumericArgument(CaseExpressionChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(CaseExpressionChainer arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

        #region Of

        internal NumericArgument(OfChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(OfChainer arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

        #region Udf

        internal NumericArgument(Udf arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(Udf arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

        #region Expression

        internal NumericArgument(Expression arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(Expression arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

        #region Concatenator

        internal NumericArgument(Concatenator arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildConcatenator(buildContext, buildArgs, arg);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(Concatenator arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

        #region DbColumn

        internal NumericArgument(DbColumn arg)
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
        /// Implicitly converts an argument into the object of NumericArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NumericArgument(DbColumn arg)
        {
            return new NumericArgument(arg);
        }

        #endregion

    }
}
