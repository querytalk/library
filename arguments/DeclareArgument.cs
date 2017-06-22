#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using QueryTalk.Wall;

namespace QueryTalk
{
    // Used only for the declaration of the SQL variables by inferring the type from the given value (DeclareArgument).
    /// <summary>
    /// Represents a CLR value of a SQL variable.
    /// </summary>
    public sealed class DeclareArgument : Argument
    {

        // Main constructor that all other CLR constructors call. When an argument of a certain value CLR type is passed to this constructor, 
        // the boxing occurs. We choose this approach due to its simplicity.
        private DeclareArgument(object arg)
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
                    return Mapping.BuildUnchecked(arg, out chainException);
                };
            }
        }

        #region QueryTalk CLR Types (including System.String)

        #region System.String

        internal DeclareArgument(System.String arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of DeclareArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator DeclareArgument(System.String arg)
        {
            return new DeclareArgument(arg);
        }

        #endregion

        #region System.Boolean

        internal DeclareArgument(System.Boolean arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of DeclareArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator DeclareArgument(System.Boolean arg)
        {
            return new DeclareArgument(arg);
        }

        #endregion

        #region System.Byte

        internal DeclareArgument(System.Byte arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of DeclareArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator DeclareArgument(System.Byte arg)
        {
            return new DeclareArgument(arg);
        }

        #endregion

        #region System.Byte[]

        internal DeclareArgument(System.Byte[] arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of DeclareArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator DeclareArgument(System.Byte[] arg)
        {
            return new DeclareArgument(arg);
        }

        #endregion

        #region System.DateTime

        internal DeclareArgument(System.DateTime arg)
            : this(arg as object)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of DeclareArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator DeclareArgument(System.DateTime arg)
        {
            return new DeclareArgument(arg); 
        }

        #endregion

        #region System.DateTimeOffset

        internal DeclareArgument(System.DateTimeOffset arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of DeclareArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator DeclareArgument(System.DateTimeOffset arg)
        {
            return new DeclareArgument(arg);
        }

        #endregion

        #region System.Decimal

        internal DeclareArgument(System.Decimal arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of DeclareArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator DeclareArgument(System.Decimal arg)
        {
            return new DeclareArgument(arg);
        }

        #endregion

        #region System.Double

        internal DeclareArgument(System.Double arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of DeclareArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator DeclareArgument(System.Double arg)
        {
            return new DeclareArgument(arg);
        }

        #endregion

        #region System.Guid

        internal DeclareArgument(Guid arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of DeclareArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator DeclareArgument(System.Guid arg)
        {
            return new DeclareArgument(arg);
        }

        #endregion

        #region System.Int16

        internal DeclareArgument(System.Int16 arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of DeclareArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator DeclareArgument(System.Int16 arg)
        {
            return new DeclareArgument(arg);
        }

        #endregion

        #region System.Int32

        internal DeclareArgument(System.Int32 arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of DeclareArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator DeclareArgument(System.Int32 arg)
        {
            return new DeclareArgument(arg);
        }

        #endregion

        #region System.Int64

        internal DeclareArgument(System.Int64 arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of DeclareArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator DeclareArgument(System.Int64 arg)
        {
            return new DeclareArgument(arg);
        }

        #endregion

        #region System.Single

        internal DeclareArgument(System.Single arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of DeclareArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator DeclareArgument(System.Single arg)
        {
            return new DeclareArgument(arg);
        }

        #endregion

        #region System.TimeSpan

        internal DeclareArgument(System.TimeSpan arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of DeclareArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator DeclareArgument(System.TimeSpan arg)
        {
            return new DeclareArgument(arg);
        }

        #endregion

        #endregion

        #region Value

        internal DeclareArgument(Value arg)
            : base(arg as Chainer)
        {
            if (arg.IsNullReference())
            {
                arg = Designer.Null;
            }

            ArgType = arg.ClrType;
            Build = arg.Build;
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of DeclareArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator DeclareArgument(Value arg)
        {
            return new DeclareArgument(arg);
        }

        #endregion

    }
}
