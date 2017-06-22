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
    /// Represents a scalar expression.
    /// </summary>  
    public class ScalarArgument : Argument,
        IScalar
    {

        // We use this property in order to gain access to the Subject node
        // during the DbColumn building (to find the co-relating table of the db column).
        internal DbNode RootSubject { get; set; }

        internal ScalarArgument(Chainer arg)
            : base(arg)
        {
            TryTakeAll(arg);

            if (arg is View)
            {
                Build = (buildContext, buildArgs) =>
                {
                    return Filter.Enclose(arg.Build(buildContext, buildArgs));
                };
            }
        }

        // We need this constructor because of the implicit conversion restriction.
        // Implicit conversion cannot be applied to the base class (System.Object), to the interfaces, and cannot be used
        // as a target argument of an extensions method. Another reason is that the Expression extension methods
        // use either System.String or IScalar, an interface that needs this constructor here.
        //   note: Since the argument is passed as the interface, it cannot get implicitly converted. 
        internal ScalarArgument(IScalar arg)
            : base(arg)
        {
            if (arg is Chainer)
            {
                TryTakeAll((Chainer)arg);

                if (arg is View)
                {
                    Build = (buildContext, buildArgs) =>
                    {
                        return Filter.Enclose(((Chainer)arg).Build(buildContext, buildArgs));
                    };
                }
            }
            else if (arg is DbColumn)
            {
                Build = ((DbColumn)arg).Build;
            }
        }

        #region Identifier

        internal ScalarArgument(Identifier arg)
            : this((Chainer)arg)
        {
            CtorBody(arg);
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(Identifier arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region Value

        internal ScalarArgument(Value arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);        
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(Value arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region System.String

        internal ScalarArgument(System.String arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                string sql;
                var variable = TryGetVariable(buildContext);
                if (ProcessVariable(buildContext, buildArgs, out sql, variable))
                {
                    return sql;
                }

                // process string as value
                if (buildContext.IsCurrentStringAsValue)
                {
                    return buildContext.BuildString(arg);
                }

                if (this is ValueScalarArgument)
                {
                    if (variable != null)
                    {
                        return arg;
                    }

                    return BuildClr(arg, buildContext);
                }
                else
                {
                    sql = Filter.DelimitMultiPartOrParam(arg, IdentifierType.ColumnOrParam, out chainException);
                    buildContext.TryTakeException(chainException);
                    return sql;
                }
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(System.String arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region VScalarArgument

        internal ScalarArgument(ValueScalarArgument arg)
            : this(arg as Chainer)
        {
            if (CheckNull(Arg(() => arg, arg)))
            {
                Build = (buildContext, buildArgs) =>
                {
                    // store RootSubject into DbColumn object
                    arg.RootSubject = RootSubject;
                    return arg.Build(buildContext, buildArgs);
                };
            }

            SetArgType(arg);
        }

        #endregion

        #region QueryTalk CLR Types

        #region System.Boolean

        internal ScalarArgument(System.Boolean arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(System.Boolean arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region System.Byte

        internal ScalarArgument(System.Byte arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(System.Byte arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region System.Byte[]

        internal ScalarArgument(System.Byte[] arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(System.Byte[] arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region System.DateTime

        internal ScalarArgument(System.DateTime arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(System.DateTime arg)
        {
            return new ScalarArgument(arg);   // default DateTime mapping
        }

        #endregion

        #region System.DateTimeOffset

        internal ScalarArgument(System.DateTimeOffset arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(System.DateTimeOffset arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region System.Decimal

        internal ScalarArgument(System.Decimal arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(System.Decimal arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region System.Double

        internal ScalarArgument(System.Double arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(System.Double arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region System.Guid

        internal ScalarArgument(Guid arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(System.Guid arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region System.Int16

        internal ScalarArgument(System.Int16 arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(System.Int16 arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region System.Int32

        internal ScalarArgument(System.Int32 arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(System.Int32 arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region System.Int64

        internal ScalarArgument(System.Int64 arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(System.Int64 arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region System.Single

        internal ScalarArgument(System.Single arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(System.Single arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region System.TimeSpan

        internal ScalarArgument(System.TimeSpan arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(System.TimeSpan arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #endregion

        #region Sys

        internal ScalarArgument(SysFn arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(SysFn arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region Case

        internal ScalarArgument(CaseExpressionChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(CaseExpressionChainer arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region Of

        internal ScalarArgument(OfChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(OfChainer arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region Udf

        internal ScalarArgument(Udf arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(Udf arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region View

        internal ScalarArgument(View arg)
            : this(arg as Chainer)
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
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(View arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region Expression

        internal ScalarArgument(Expression arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(Expression arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region Collate

        internal ScalarArgument(CollateChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(CollateChainer arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region Concatenator

        internal ScalarArgument(Concatenator arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildConcatenator(buildContext, buildArgs, arg);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(Concatenator arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        #region DbColumn

        internal ScalarArgument(DbColumn arg)
            : base(arg)
        {
            if (CheckNull(Arg(() => arg, arg)))
            {
                Build = (buildContext, buildArgs) =>
                {
                    // store Subject into DbColumn object
                    arg.RootSubject = RootSubject;

                    // late call of the arg Build method
                    return arg.Build(buildContext, buildArgs);
                };  
            }

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ScalarArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ScalarArgument(DbColumn arg)
        {
            return new ScalarArgument(arg);
        }

        #endregion

        internal static string Concatenate(
            ScalarArgument[] arguments, 
            BuildContext buildContext, 
            BuildArgs buildArgs)
        {
            return String.Join(Text.CommaWithSpace,
                arguments.Select(argument =>
                {
                    if (argument == null)
                    {
                        argument = Designer.Null;
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
