#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents a column in non-select clauses.
    /// </summary>
    public sealed class NonSelectColumnArgument : Argument,
        IScalar
    {
        internal NonSelectColumnArgument(Chainer arg)
            : base(arg)
        {
            TryTakeAll(arg);
        }

        #region Identifier

        internal NonSelectColumnArgument(Identifier arg)
            : this((Chainer)arg)
        {
            CtorBody(arg);
            SetArgType(arg);
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NonSelectColumnArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NonSelectColumnArgument(Identifier arg)
        {
            return new NonSelectColumnArgument(arg);
        }

        #endregion

        #region System.String

        internal NonSelectColumnArgument(System.String arg)
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
        /// Implicitly converts an argument into the object of NonSelectColumnArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NonSelectColumnArgument(System.String arg)
        {
            return new NonSelectColumnArgument(arg);
        }

        #endregion

        #region Of

        internal NonSelectColumnArgument(OfChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of NonSelectColumnArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NonSelectColumnArgument(OfChainer arg)
        {
            return new NonSelectColumnArgument(arg);
        }

        #endregion

        #region DbColumn

        internal NonSelectColumnArgument(DbColumn arg)
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
        /// Implicitly converts an argument into a ColumnArgument.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator NonSelectColumnArgument(DbColumn arg)
        {
            return new NonSelectColumnArgument(arg);
        }

        #endregion

    }
}
