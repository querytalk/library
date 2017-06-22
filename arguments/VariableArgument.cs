#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents a value of a variable in a query.
    /// </summary>
    public sealed class VariableArgument : Argument
    {

        #region Properties

        private bool _isVariable;

        internal DataType DataType { get; set; }

        #endregion

        // Main constructor that all other CLR constructors call. When an argument of a certain value CLR type is passed to this constructor, 
        // the boxing occurs. We choose this approach due to its simplicity.
        private VariableArgument(object arg)
            : base(arg)
        {
            if (arg is Chainer)
            {
                TryTakeAll((Chainer)arg);
            }
            else
            {
                Build = (buildContext, buildArgs) =>
                {
                    if (DataType != null)
                    {
                        return Mapping.Build(arg, DataType);
                    }
                    else
                    {
                        return Mapping.BuildUnchecked(arg, out chainException);
                    }
                };
            }
        }

        #region System.String

        internal VariableArgument(System.String arg)
            : this(arg as object)
        {
            Value arg2 = null;
            if (Common.CheckIdentifier(arg) == IdentifierValidity.Variable)
            {
                _isVariable = true;
            }
            else
            {
                // arg contains a value => convert it to Value object
                arg2 = new Value(arg);
                ArgType = arg2.ClrType;
            }

            Build = (buildContext, buildArgs) =>
            {
                if (_isVariable)
                {
                    return arg;
                }
                else
                {
                    return arg2.Build(buildContext, buildArgs);
                }
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(System.String arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region QueryTalk CLR Types

        #region System.Boolean

        internal VariableArgument(System.Boolean arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(System.Boolean arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region System.Byte

        internal VariableArgument(System.Byte arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(System.Byte arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region System.Byte[]

        internal VariableArgument(System.Byte[] arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(System.Byte[] arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region System.DateTime

        internal VariableArgument(System.DateTime arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(System.DateTime arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region System.DateTimeOffset

        internal VariableArgument(System.DateTimeOffset arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(System.DateTimeOffset arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region System.Decimal

        internal VariableArgument(System.Decimal arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(System.Decimal arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region System.Double

        internal VariableArgument(System.Double arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(System.Double arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region System.Guid

        internal VariableArgument(Guid arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(System.Guid arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region System.Int16

        internal VariableArgument(System.Int16 arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(System.Int16 arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region System.Int32

        internal VariableArgument(System.Int32 arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(System.Int32 arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region System.Int64

        internal VariableArgument(System.Int64 arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(System.Int64 arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region System.Single

        internal VariableArgument(System.Single arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(System.Single arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region System.TimeSpan

        internal VariableArgument(System.TimeSpan arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(System.TimeSpan arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #endregion

        #region Sys

        internal VariableArgument(SysFn arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(SysFn arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region Case

        internal VariableArgument(CaseExpressionChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(CaseExpressionChainer arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region View

        internal VariableArgument(View arg)
            : base(arg)
        {
            if (CheckNull(Arg(() => arg, arg)))
            {
                Build = (buildContext, buildArgs) =>
                {
                    return Filter.Enclose(arg.Build(buildContext, buildArgs));
                };
            }

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(View arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region Expression

        internal VariableArgument(Expression arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(Expression arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region Value

        internal VariableArgument(Value arg)
            : base(arg as Chainer)
        {
            if (arg.IsNullReference())
            {
                arg = Designer.Null;
            }

            SetArgType(arg);
            ArgType = arg.ClrType;  // overwrite the type 

            Build = arg.Build;
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(Value arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

        #region Udf

        internal VariableArgument(Udf arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of VariableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator VariableArgument(Udf arg)
        {
            return new VariableArgument(arg);
        }

        #endregion

    }
}
