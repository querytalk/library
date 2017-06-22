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
    /// Represents an argument of a built-in function.
    /// </summary>
    public sealed class FunctionArgument : Argument
    {
        private FunctionArgument(Chainer arg)
            : base(arg)
        {
            TryTakeAll(arg);
        }

        #region Identifier

        internal FunctionArgument(Identifier arg)
            : this((Chainer)arg)
        {
            CtorBody(arg);
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(Identifier arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region Value

        internal FunctionArgument(Value arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(Value arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region System.String

        // Build method for string argument
        private string _FunctionStringArgBuildMethod(BuildContext buildContext, BuildArgs buildArgs, string arg)
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
        }

        // set Build method for string argument
        internal void SetStringBuildMethod(string arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return _FunctionStringArgBuildMethod(buildContext, buildArgs, arg);
            };
        }

        internal FunctionArgument(System.String arg)
            : base(arg)
        {
            SetStringBuildMethod(arg);
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(System.String arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region QueryTalk CLR Types

        #region System.Boolean

        internal FunctionArgument(System.Boolean arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(System.Boolean arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region System.Byte

        internal FunctionArgument(System.Byte arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(System.Byte arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region System.Byte[]

        internal FunctionArgument(System.Byte[] arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(System.Byte[] arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region System.DateTime

        internal FunctionArgument(System.DateTime arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(System.DateTime arg)
        {
            return new FunctionArgument(arg);   // default DateTime mapping
        }

        #endregion

        #region System.DateTimeOffset

        internal FunctionArgument(System.DateTimeOffset arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(System.DateTimeOffset arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region System.Decimal

        internal FunctionArgument(System.Decimal arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(System.Decimal arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region System.Double

        internal FunctionArgument(System.Double arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(System.Double arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region System.Guid

        internal FunctionArgument(Guid arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(System.Guid arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region System.Int16

        internal FunctionArgument(System.Int16 arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(System.Int16 arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region System.Int32

        internal FunctionArgument(System.Int32 arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(System.Int32 arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region System.Int64

        internal FunctionArgument(System.Int64 arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(System.Int64 arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region System.Single

        internal FunctionArgument(System.Single arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(System.Single arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region System.TimeSpan

        internal FunctionArgument(System.TimeSpan arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(System.TimeSpan arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #endregion

        #region SysFn

        internal FunctionArgument(SysFn arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(SysFn arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region Case

        internal FunctionArgument(CaseExpressionChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(CaseExpressionChainer arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region Of

        internal FunctionArgument(OfChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(OfChainer arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region Udf

        internal FunctionArgument(Udf arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(Udf arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region View

        internal FunctionArgument(View arg)
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
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(View arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region Expression

        internal FunctionArgument(Expression arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(Expression arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region Concatenator

        internal FunctionArgument(Concatenator arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildConcatenator(buildContext, buildArgs, arg);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument GroupingArgument convert.</param>
        public static implicit operator FunctionArgument(Concatenator arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        #region DbColumn

        internal FunctionArgument(DbColumn arg)
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
        /// Implicitly converts an argument into the object of FunctionArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator FunctionArgument(DbColumn arg)
        {
            return new FunctionArgument(arg);
        }

        #endregion

        internal static string Concatenate(
            FunctionArgument[] arguments, 
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
