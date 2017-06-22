#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using QueryTalk.Wall;

namespace QueryTalk
{
    // For table identifiers only. Constructors are public - needed for bulk table params.
    /// <summary>
    /// Represents a table or view identifier.
    /// </summary>
    public sealed class TableArgument : Argument 
    {
        // allowed inliners 
        private static readonly DT[] _inliners = { DT.InTable };

        internal override string Method
        {
            get
            {
                return Text.Method.Param;
            }
        }

        internal string Sql { get; private set; }

        #region Identifier

        /// <summary>
        /// Initializes a new instance of the TableArgument class.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public TableArgument(Identifier arg)
            : base(arg)
        {
            if (CheckNull(Arg(() => arg, arg)))
            {
                Sql = arg.ToString();
            }

            Build = (buildContext, buildArgs) =>
                {
                    buildContext.TryTakeException(chainException);
                    return Sql;
                };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of TableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator TableArgument(Identifier arg)
        {
            return new TableArgument(arg);
        }

        #endregion

        #region System.String

        /// <summary>
        /// Initializes a new instance of the TableArgument class.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public TableArgument(System.String arg)
            : base(arg)
        {
            SetArgType(arg);

            if (CheckNull(Arg(() => arg, arg)))
            {
                Sql = Filter.DelimitTableMultiPart(arg, out chainException);
            }

            Build = (buildContext, buildArgs) =>
            {
                buildContext.TryTakeException(chainException);
                TryThrow();

                string sql;

                Variable variable = TryGetVariable(buildContext);

                if (variable.IsInliner())
                {
                    string arg2 = variable.Name;

                    // check inliner type
                    if (variable.DT != DT.InTable)
                    {
                        buildContext.TryTakeException(variable.DT.InvalidInlinerException(GetType().Name,
                            arg2, _inliners));
                        return null;
                    }

                    if (buildArgs.Executable != null)
                    {
                        ParameterArgument argument = buildArgs.Executable.GetInlinerArgument(arg2);
                        if (argument.Value != null)
                        {
                            if (argument.Value is System.String)
                            {
                                arg2 = Filter.DelimitMultiPartOrParam((string)argument.Value, IdentifierType.Table, out chainException);
                            }
                            // Identifier
                            else 
                            {
                                arg2 = ((Identifier)argument.Value).Build(buildContext, buildArgs);
                            }
                        }
                        else
                        {
                            buildContext.TryTakeException(new QueryTalkException(this,
                                QueryTalkExceptionType.InlinerArgumentNull,
                                String.Format("{0} = null", arg2)));
                            return null;
                        }
                    }
                    return arg2;
                }

                if (ProcessVariable(buildContext, buildArgs, out sql))
                {
                    Sql = sql;
                }
                else
                {
                    Sql = Filter.DelimitMultiPartOrParam(arg, IdentifierType.Table, out chainException);
                    buildContext.TryTakeException(chainException);
                }

                return Sql;
            };
        }

        /// <summary>
        /// Implicitly converts an argument into the object of TableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator TableArgument(System.String arg)
        {
            return new TableArgument(arg);
        }

        #endregion

        #region DbNode

        /// <summary>
        /// Implicitly converts an argument into the object of TableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public TableArgument(DbNode arg)
            : base(arg)
        {
            if (CheckNull(Arg(() => arg, arg)))
            {
                Sql = arg.Name;
            }

            Build = (buildContext, buildArgs) =>
            {
                buildContext.TryTakeException(chainException);
                return Sql;
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of TableArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator TableArgument(DbNode arg)
        {
            return new TableArgument(arg);
        }

        #endregion

    }
}
