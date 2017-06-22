#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents a scalar expression with the string value preference. If a given argument is a string it will be treated as a value.
    /// </summary>
    public sealed class ValueScalarArgument : ScalarArgument
    {
        internal ValueScalarArgument(IScalar arg)
            : base(arg)
        { }

        #region Identifier

        internal ValueScalarArgument(Identifier arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(Identifier arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region Value

        internal ValueScalarArgument(Value arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(Value arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region System.String

        internal ValueScalarArgument(System.String arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(System.String arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region QueryTalk CLR Types

        #region System.Boolean

        internal ValueScalarArgument(System.Boolean arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(System.Boolean arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region System.Byte

        internal ValueScalarArgument(System.Byte arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(System.Byte arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region System.Byte[]

        internal ValueScalarArgument(System.Byte[] arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(System.Byte[] arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region System.DateTime

        internal ValueScalarArgument(System.DateTime arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(System.DateTime arg)
        {
            return new ValueScalarArgument(arg);  
        }

        #endregion

        #region System.DateTimeOffset

        internal ValueScalarArgument(System.DateTimeOffset arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(System.DateTimeOffset arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region System.Decimal

        internal ValueScalarArgument(System.Decimal arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(System.Decimal arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region System.Double

        internal ValueScalarArgument(System.Double arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(System.Double arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region System.Guid

        internal ValueScalarArgument(Guid arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(System.Guid arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region System.Int16

        internal ValueScalarArgument(System.Int16 arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(System.Int16 arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region System.Int32

        internal ValueScalarArgument(System.Int32 arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(System.Int32 arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region System.Int64

        internal ValueScalarArgument(System.Int64 arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(System.Int64 arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region System.Single

        internal ValueScalarArgument(System.Single arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(System.Single arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region System.TimeSpan

        internal ValueScalarArgument(System.TimeSpan arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(System.TimeSpan arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #endregion

        #region Sys

        internal ValueScalarArgument(SysFn arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(SysFn arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region Case

        internal ValueScalarArgument(CaseExpressionChainer arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(CaseExpressionChainer arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region Of

        internal ValueScalarArgument(OfChainer arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(OfChainer arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region Udf

        internal ValueScalarArgument(Udf arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(Udf arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region View

        internal ValueScalarArgument(View arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(View arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region Expression

        internal ValueScalarArgument(Expression arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(Expression arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region Collate

        internal ValueScalarArgument(CollateChainer arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(CollateChainer arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region Concatenator

        internal ValueScalarArgument(Concatenator arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(Concatenator arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

        #region DbColumn

        internal ValueScalarArgument(DbColumn arg)
            : base(arg)
        { }

        /// <summary>
        /// Implicitly converts an argument into the object of ValueScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ValueScalarArgument(DbColumn arg)
        {
            return new ValueScalarArgument(arg);
        }

        #endregion

    }
}
