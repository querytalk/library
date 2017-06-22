#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Diagnostics;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents a data value of a CLR-SQL type that can be passed to or loaded from the SQL server.
    /// </summary>
    [DebuggerTypeProxy(typeof(Wall.ValueDebuggerProxy))]
    public sealed class Value : Argument, IEquatable<Value>,
        IScalar,
        IAs
    {

        #region Properties

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal override string Method
        {
            get
            {
                return Text.Method.Value;
            }
        }

        // if true then this instance contains a special defined value (e.g. d.Default or d.DefaultValues).
        private bool _isSpecialValue = false;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal bool IsSpecialValue
        {
            get
            {
                return _isSpecialValue;
            }
        }

        internal void SetValue(object value)
        {
            Original = value;

            if (value != null)
            {
                _clrType = value.GetType();
                _hashCode = GetCrossTypeHashCode(_clrType, value.GetHashCode());
                Build = (buildContext, buildArgs) =>
                {
                    return Mapping.BuildUnchecked(value);
                };
            }
            else
            {
                SetAsNull();
            }
        }

        internal void SetSpecialValue(string value)
        {
            Original = value;
            _isSpecialValue = true;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Type _clrType;

        /// <summary>
        /// Gets the type of the value stored in this instance.
        /// </summary>
        public Type ClrType
        {
            get
            {
                return _clrType;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DataType _dbt;
        internal DataType DbT
        {
            get
            {
                return _dbt;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _isOutput = false;
        internal bool IsOutput
        {
            get
            {
                return _isOutput;
            }
            set
            {
                _isOutput = value;
            }
        }

        #endregion

        private int _hashCode = 0;  

        #region Constructors

        private void SetAsNull()
        {
            _clrType = null;
            Original = null;
            Build = (buildContext, buildArgs) =>
            {
                return (string)Original ?? Text.Null;
            };
        }

        internal Value()
            : base(null)
        {
            SetAsNull();
        }

        internal Value(System.Boolean value, Parameterization p = Parameterization.Value)
            : base(value)
        {
            Original = value;
            _clrType = typeof(System.Boolean);
            _hashCode = GetCrossTypeHashCode(_clrType, value.GetHashCode());

            Build = (buildContext, buildArgs) =>
            {
                return (p != Parameterization.None) ? (value.Parameterize(buildContext, p) ?? 
                    Mapping.BuildCast(value)) : Mapping.BuildCast(value);
            };
        }

        internal Value(System.Byte value, Parameterization p = Parameterization.Value)
            : base(value)
        {
            Original = value;
            _clrType = typeof(System.Byte);
            _hashCode = GetCrossTypeHashCode(_clrType, value.GetHashCode());

            Build = (buildContext, buildArgs) =>
            {
                return (p != Parameterization.None) ? (value.Parameterize(buildContext, p) ?? 
                    Mapping.BuildCast(value)) : Mapping.BuildCast(value);
            };
        }

        internal Value(System.Byte[] value, Parameterization p = Parameterization.Value)
            : base(value)
        {
            Original = value;
            _clrType = typeof(System.Byte[]);

            // use Binary type for byte[] hash code calculation
            _hashCode = GetCrossTypeHashCode(_clrType, new System.Data.Linq.Binary(value).GetHashCode());

            Build = (buildContext, buildArgs) =>
            {
                // literal:
                return (p != Parameterization.None) ? 
                    (value.Parameterize(buildContext, p) ?? Mapping.Build(value, Mapping.DefaultBinaryType)) : 
                        Mapping.Build(value, Mapping.DefaultBinaryType);
            };
        }

        internal Value(System.DateTime value, Parameterization p = Parameterization.Value, DataType dataType = null)
            : base(value)
        {
            Original = value;
            _clrType = typeof(System.DateTime);
            _hashCode = GetCrossTypeHashCode(_clrType, value.GetHashCode());
            dataType = dataType ?? Mapping.DefaultDateTimeType;

            // build
            Build = (buildContext, buildArgs) =>
            {
                return (p != Parameterization.None) ?
                    (value.Parameterize(buildContext, dataType, p) ?? 
                        Mapping.BuildCast(value, dataType)) : Mapping.BuildCast(value, dataType);
            };
        }

        internal Value(System.DateTimeOffset value, Parameterization p = Parameterization.Value)
            : base(value)
        {
            Original = value;
            _clrType = typeof(System.DateTimeOffset);
            _hashCode = GetCrossTypeHashCode(_clrType, value.GetHashCode());

            Build = (buildContext, buildArgs) =>
            {
                return (p != Parameterization.None) ? (value.Parameterize(buildContext, p) ?? Mapping.BuildCast(value)) : Mapping.BuildCast(value);
            };
        }

        internal Value(System.Decimal value, Parameterization p = Parameterization.Value)
            : base(value)
        {
            Original = value;
            _clrType = typeof(System.Decimal);
            _hashCode = GetCrossTypeHashCode(_clrType, value.GetHashCode());

            Build = (buildContext, buildArgs) =>
            {
                return (p != Parameterization.None) ? (value.Parameterize(buildContext, p) ?? 
                    Mapping.BuildCast(value)) : Mapping.BuildCast(value);
            };
        }

        internal Value(System.Double value, Parameterization p = Parameterization.Value)
            : base(value)
        {
            Original = value;
            _clrType = typeof(System.Double);
            _hashCode = GetCrossTypeHashCode(_clrType, value.GetHashCode());

            Build = (buildContext, buildArgs) =>
            {
                return (p != Parameterization.None) ? (value.Parameterize(buildContext, p) ?? Mapping.BuildCast(value)) : Mapping.BuildCast(value);
            };
        }

        internal Value(System.Guid value, Parameterization p = Parameterization.Value)
            : base(value)
        {
            Original = value;
            _clrType = typeof(System.Guid);
            _hashCode = GetCrossTypeHashCode(_clrType, value.GetHashCode());

            Build = (buildContext, buildArgs) =>
            {
                return (p != Parameterization.None) ? (value.Parameterize(buildContext, p) ?? Mapping.BuildCast(value)) : Mapping.BuildCast(value);
            };
        }

        internal Value(System.Int16 value, Parameterization p = Parameterization.Value)
            : base(value)
        {
            Original = value;
            _clrType = typeof(System.Int16);
            _hashCode = GetCrossTypeHashCode(_clrType, value.GetHashCode());

            Build = (buildContext, buildArgs) =>
            {
                return (p != Parameterization.None) ? (value.Parameterize(buildContext, p) ?? Mapping.BuildCast(value)) : Mapping.BuildCast(value);
            };
        }

        internal Value(System.Int32 value, Parameterization p = Parameterization.Value)
            : base(value)
        {
            Original = value;
            _clrType = typeof(System.Int32);
            _hashCode = GetCrossTypeHashCode(_clrType, value.GetHashCode());

            Build = (buildContext, buildArgs) =>
            {
                // literal:
                return (p != Parameterization.None) ? 
                    (value.Parameterize(buildContext, p) ?? Mapping.Build(value, Mapping.DefaultInt32Type)) : 
                        Mapping.Build(value, Mapping.DefaultInt32Type);
            };
        }

        internal Value(System.Int64 value, Parameterization p = Parameterization.Value)
            : base(value)
        {
            Original = value;
            _clrType = typeof(System.Int64);
            _hashCode = GetCrossTypeHashCode(_clrType, value.GetHashCode());

            Build = (buildContext, buildArgs) =>
            {
                return (p != Parameterization.None) ? (value.Parameterize(buildContext, p) ?? Mapping.BuildCast(value)) : Mapping.BuildCast(value);
            };
        }

        internal Value(System.Single value, Parameterization p = Parameterization.Value)
            : base(value)
        {
            Original = value;
            _clrType = typeof(System.Single);
            _hashCode = GetCrossTypeHashCode(_clrType, value.GetHashCode());

            Build = (buildContext, buildArgs) =>
            {
                return (p != Parameterization.None) ? (value.Parameterize(buildContext, p) ?? Mapping.BuildCast(value)) : Mapping.BuildCast(value);
            };
        }

        internal Value(System.String value, Parameterization p = Parameterization.Value, DataType dataType = null)
            : base(value)
        {
            Original = value;
            _clrType = typeof(System.String);

            if (dataType == null)
            {
                dataType = Mapping.DefaultStringType;
            }

            if (value != null)
            {
                _hashCode = GetCrossTypeHashCode(_clrType, value.GetHashCode());
            }

            Build = (buildContext, buildArgs) =>
            {
                string sql = null;
                if (p != Parameterization.None)
                {
                    sql = value.Parameterize(buildContext, dataType, p);
                }

                if (sql == null)
                {
                    // literal:
                    sql = Mapping.Build(value, dataType);

                    // if node contains concatenator than all string values should be escaped twice
                    if (buildContext.Current.IsQuery)
                    {
                        if (buildContext.Current.Query.Master.IsConcatenated)
                        {
                            sql = Filter.Escape(sql);
                        }
                    }
                }

                return sql;
            };

        }

        internal Value(System.TimeSpan value, Parameterization p = Parameterization.Value)
            : base(value)
        {
            Original = value;
            _clrType = typeof(System.TimeSpan);
            _hashCode = GetCrossTypeHashCode(_clrType, value.GetHashCode());

            Build = (buildContext, buildArgs) =>
            {
                return (p != Parameterization.None) ? (value.Parameterize(buildContext, p) ?? Mapping.BuildCast(value)) : Mapping.BuildCast(value);
            };
        }

        #region System.Object

        internal Value(object value, Parameterization p, DataType dataType = null)
            : base(value)
        {
            if (value == null)
            {
                SetAsNull();
                return;
            }

            Original = value;
            var clrType = value.GetType();  

            if (Mapping.CheckClrCompliance(clrType, out _clrType, out chainException) != Mapping.ClrTypeMatch.ClrMatch)
            {
                TryThrow();
            }

            DebugValue = value;

            if (_clrType == typeof(DBNull) || _clrType == typeof(System.Object))
            {
                SetAsNull();
                return;
            }

            _hashCode = GetCrossTypeHashCode(_clrType, value.GetHashCode());

            Build = (buildContext, buildArgs) =>
            {
                string sql = null;
                if (p != Parameterization.None)
                {
                    sql = value.Parameterize(buildContext, dataType, p);
                }

                if (sql == null)
                {
                    if (dataType != null)
                    {
                        sql = String.Format("{0}({1} AS {2})", Text.Cast, Mapping.Build(value, dataType), dataType.Build());
                    }
                    else
                    {
                        sql = Mapping.BuildUnchecked(value, out chainException);
                    }
                }

                return sql;
            };        
        }

        /// <summary>
        /// Converts an object into a Value.
        /// </summary>
        /// <param name="value">An object to convert.</param>
        /// <param name="parameterized">If true than the value will be represented in the SQL code by the variable.</param>
        public Value(object value, bool parameterized = true)
            : this(value, parameterized ? Parameterization.Value : Parameterization.None)
        { }

        /// <summary>
        /// Represents a CLR value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dataType">A DateType instance. Use d.(db_type) method.</param>
        /// <param name="parameterized"></param>
        public Value(object value, DataType dataType, bool parameterized = true)
            : this(value, parameterized ? Parameterization.Value : Parameterization.None, dataType)
        {
            _dbt = dataType;
        }

        #endregion

        #endregion

        #region Implicit conversions: to Value

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Value(System.Boolean arg)
        {
            return new Value(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Value(System.Byte arg)
        {
            return new Value(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Value(System.Byte[] arg)
        {
            return new Value(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Value(System.DateTime arg)
        {
            return new Value(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Value(System.DateTimeOffset arg)
        {
            return new Value(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Value(System.Decimal arg)
        {
            return new Value(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Value(System.Double arg)
        {
            return new Value(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Value(System.Guid arg)
        {
            return new Value(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Value(System.Int16 arg)
        {
            return new Value(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Value(System.Int32 arg)
        {
            return new Value(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Value(System.Int64 arg)
        {
            return new Value(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Value(System.Single arg)
        {
            return new Value(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Value(System.String arg)
        {
            return new Value(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Value(System.TimeSpan arg)
        {
            return new Value(arg);
        }

        #endregion

        #region Implicit conversions: to CLR types

        #region System.Boolean

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator System.Boolean(Value arg)
        {
            try
            {
                return (System.Boolean)arg.Original;
            }
            catch (System.NullReferenceException)
            {
                throw CreateNullCannotCastException(typeof(System.Boolean));
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Boolean));
            }
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Nullable<System.Boolean>(Value arg)
        {
            try
            {
                if (arg == null || arg.Original == null)
                {
                    return null;
                }
                else
                {
                    return (System.Boolean)arg.Original;
                }
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Boolean));
            }
        }

        #endregion

        #region System.Byte

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator System.Byte(Value arg)
        {
            try
            {
                return (System.Byte)arg.Original;
            }
            catch (System.NullReferenceException)
            {
                throw CreateNullCannotCastException(typeof(System.Byte));
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Byte));
            }
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Nullable<System.Byte>(Value arg)
        {
            try
            {
                if (arg == null || arg.Original == null)
                {
                    return null;
                }
                else
                {
                    return (System.Byte)arg.Original;
                }
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Byte));
            }
        }

        #endregion

        #region System.Byte[]

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator System.Byte[](Value arg)
        {
            try
            {
                return (System.Byte[])(arg ?? Designer.Null).Original;
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Byte[]));
            }
        }

        #endregion

        #region System.DateTime

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator System.DateTime(Value arg)
        {
            try
            {
                return (System.DateTime)arg.Original;
            }
            catch (System.NullReferenceException)
            {
                throw CreateNullCannotCastException(typeof(System.DateTime));
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.DateTime));
            }
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Nullable<System.DateTime>(Value arg)
        {
            try
            {
                if (arg == null || arg.Original == null)
                {
                    return null;
                }
                else
                {
                    return (System.DateTime)arg.Original;
                }
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.DateTime));
            }
        }

        #endregion

        #region System.DateTimeOffset

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator System.DateTimeOffset(Value arg)
        {
            try
            {
                return (System.DateTimeOffset)arg.Original;
            }
            catch (System.NullReferenceException)
            {
                throw CreateNullCannotCastException(typeof(System.DateTimeOffset));
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.DateTimeOffset));
            }
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Nullable<System.DateTimeOffset>(Value arg)
        {
            try
            {
                if (arg == null || arg.Original == null)
                {
                    return null;
                }
                else
                {
                    return (System.DateTimeOffset)arg.Original;
                }
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.DateTimeOffset));
            }
        }

        #endregion

        #region System.Decimal

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator System.Decimal(Value arg)
        {
            try
            {
                return (System.Decimal)arg.Original;
            }
            catch (System.NullReferenceException)
            {
                throw CreateNullCannotCastException(typeof(System.Decimal));
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Decimal));
            }
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Nullable<System.Decimal>(Value arg)
        {
            try
            {
                if (arg == null || arg.Original == null)
                {
                    return null;
                }
                else
                {
                    return (System.Decimal)arg.Original;
                }
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Decimal));
            }
        }

        #endregion

        #region System.Double

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator System.Double(Value arg)
        {
            try
            {
                return (System.Double)arg.Original;
            }
            catch (System.NullReferenceException)
            {
                throw CreateNullCannotCastException(typeof(System.Double));
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Double));
            }
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Nullable<System.Double>(Value arg)
        {
            try
            {
                if (arg == null || arg.Original == null)
                {
                    return null;
                }
                else
                {
                    return (System.Double)arg.Original;
                }
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Double));
            }
        }

        #endregion

        #region System.Guid

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator System.Guid(Value arg)
        {
            try
            {
                return (System.Guid)arg.Original;
            }
            catch (System.NullReferenceException)
            {
                throw CreateNullCannotCastException(typeof(System.Guid));
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Guid));
            }
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Nullable<System.Guid>(Value arg)
        {
            try
            {
                if (arg == null || arg.Original == null)
                {
                    return null;
                }
                else
                {
                    return (System.Guid)arg.Original;
                }
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Guid));
            }
        }

        #endregion

        #region System.Int16

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator System.Int16(Value arg)
        {
            try
            {
                return (System.Int16)arg.Original;
            }
            catch (System.NullReferenceException)
            {
                throw CreateNullCannotCastException(typeof(System.Int16));
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Int16));
            }
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Nullable<System.Int16>(Value arg)
        {
            try
            {
                if (arg == null || arg.Original == null)
                {
                    return null;
                }
                else
                {
                    return (System.Int16)arg.Original;
                }
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Int16));
            }
        }

        #endregion

        #region System.Int32

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator System.Int32(Value arg)
        {
            try
            {
                return (System.Int32)arg.Original;
            }

            catch (System.NullReferenceException)
            {
                throw CreateNullCannotCastException(typeof(System.Int32));
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Int32));
            }
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Nullable<System.Int32>(Value arg)
        {
            try
            {
                if (arg == null || arg.Original == null)
                {
                    return null;
                }
                else
                {
                    return (System.Int32)arg.Original;
                }
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Int32));
            }
        }

        #endregion

        #region System.Int64

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator System.Int64(Value arg)
        {
            try
            {
                return (System.Int64)arg.Original;
            }
            catch (System.NullReferenceException)
            {
                throw CreateNullCannotCastException(typeof(System.Int64));
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Int64));
            }
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Nullable<System.Int64>(Value arg)
        {
            try
            {
                if (arg == null || arg.Original == null)
                {
                    return null;
                }
                else
                {
                    return (System.Int64)arg.Original;
                }
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Int64));
            }
        }

        #endregion

        #region System.Single

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator System.Single(Value arg)
        {
            try
            {
                return (System.Single)arg.Original;
            }
            catch (System.NullReferenceException)
            {
                throw CreateNullCannotCastException(typeof(System.Single));
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Single));
            }
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Nullable<System.Single>(Value arg)
        {
            try
            {
                if (arg == null || arg.Original == null)
                {
                    return null;
                }
                else
                {
                    return (System.Single)arg.Original;
                }
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.Single));
            }
        }

        #endregion

        #region System.String

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator System.String(Value arg)
        {
            try
            {
                return (System.String)(arg ?? Designer.Null).Original;
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.String));
            }
        }

        #endregion

        #region System.TimeSpan

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator System.TimeSpan(Value arg)
        {
            try
            {
                return (System.TimeSpan)arg.Original;
            }
            catch (System.NullReferenceException)
            {
                throw CreateNullCannotCastException(typeof(System.TimeSpan));
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.TimeSpan));
            }
        }

        /// <summary>
        /// Implicitly converts an argument into a Value object.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Nullable<System.TimeSpan>(Value arg)
        {
            try
            {
                if (arg == null || arg.Original == null)
                {
                    return null;
                }
                else
                {
                    return (System.TimeSpan)arg.Original;
                }
            }
            catch (System.InvalidCastException)
            {
                throw CreateInvalidCastException(arg, typeof(System.TimeSpan));
            }
        }

        #endregion

        #region Create exceptions

        private static QueryTalkException CreateInvalidCastException(Value arg, Type targetType)
        {
            return new QueryTalkException(typeof(Value).Name, QueryTalkExceptionType.InvalidCast,
                String.Format("value = {0} ({1}){2}   CLR type = {3}", arg.Original, arg.Original.GetType(), Environment.NewLine, targetType),
                Text.Method.ImplicitConversion);
        }

        private static QueryTalkException CreateNullCannotCastException(Type targetType)
        {
            return new QueryTalkException(typeof(Value).Name, QueryTalkExceptionType.NullCannotCast,
                String.Format("value = null{0}   type = {1}", Environment.NewLine, targetType),
                Text.Method.ImplicitConversion);
        }

        #endregion

        #endregion

        #region IsNull

        /// <summary>
        /// Returns true if a value evaluates to null.
        /// </summary>
        /// <param name="value">A value to evaluate.</param>
        /// <returns></returns>
        public static bool IsNull(object value)
        {
            if (value == null)
            {
                return true;
            }

            var type = value.GetType();

            if (type == typeof(DBNull) || type == typeof(System.Object))
            {
                return true;
            }

            if (type == typeof(Value))
            {
                return (Value)value == Designer.Null;
            }

            return false;
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Determines whether the specified argument is equal to this instance.
        /// </summary>
        /// <param name="other">The argument to compare with this instance.</param>
        public bool Equals(Value other)
        {
            if (other == null)
            {
                return this.Original == null;
            }

            if (this._clrType != other._clrType)
            {
                return false;
            }

            if (Mapping.BuildUnchecked(Original) != Mapping.BuildUnchecked(other.Original))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Value))
            {
                Type clrType;
                if (Mapping.CheckClrCompliance(obj.GetType(), out clrType, out chainException) == Mapping.ClrTypeMatch.ClrMatch)
                {
                    obj = new Value(obj);
                }
                else
                {
                    return false;
                }
            }

            return (this as IEquatable<Value>)
                .Equals(obj as Value);
        }

        /// <summary>
        /// Returns a value indicating whether the two specified Value values are equal.
        /// </summary>
        /// <param name="pd1">The first value in a comparison.</param>
        /// <param name="pd2">The second value in a comparison.</param>
        public static bool operator ==(Value pd1, Value pd2)
        {
            if (System.Object.ReferenceEquals(pd1, pd2))
            {
                return true;
            }

            if ((object)pd1 == null && (object)pd2 != null && pd2.Original == null)
            {
                return true;
            }

            if ((object)pd2 == null && (object)pd1 != null && pd1.Original == null)
            {
                return true;
            }

            if (pd1.Original == null && pd2.Original == null)
            {
                return true;
            }

            if (((object)pd1 == null) || ((object)pd2 == null))
            {
                return false;
            }

            return (pd1 as IEquatable<Value>)
                .Equals(pd2);
        }

        /// <summary>
        /// Returns a value indicating whether the two specified Value values are not equal.
        /// </summary>
        /// <param name="pd1">The first value in a comparison.</param>
        /// <param name="pd2">The second value in a comparison.</param>
        public static bool operator !=(Value pd1, Value pd2)
        {
            return !(pd1 == pd2);
        }

        /// <summary>
        /// Returns the hash code of this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return _hashCode;
        }

        internal static int GetCrossTypeHashCode(Type type, int hash)
        {
            QueryTalkException exception;
            Type clrType;
            if (Mapping.CheckClrCompliance(type, out clrType, out exception) == Mapping.ClrTypeMatch.ClrMatch)
            {
                return CombineHashCodes(clrType.Name.GetHashCode(), hash);
            }
            else
            {
                return CombineHashCodes(type.Name.GetHashCode(), hash);
            }
        }

        internal static int CombineHashCodes(int h1, int h2)
        {
            return (((h1 << 5) + h1) ^ h2);
        }

        #endregion

        /// <summary>
        /// Returns a string representation of the Value object.
        /// </summary>
        public override string ToString()
        {
            if (!Original.IsUndefined())
            {
                return Original.ToString();
            }
            else
            {
                return Text.Null;
            }
        }

    }
}
