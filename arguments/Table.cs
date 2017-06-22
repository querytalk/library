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
    /// Represents a table in a query.
    /// </summary>   
    public sealed class Table : Argument,
        IAs
    {
        // allowed inliners      
        private static readonly DT[] _inliners = { DT.InTable };

        // for db tables only
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal AliasAs Alias { get; private set; }

        private Table()
            : base(null)
        { }

        private Table(Chainer arg)
            : base(arg)
        {
            TryTakeAll(arg);
        }

        #region System.String

        internal Table(System.String arg)
            : base(arg)
        {
            SetArgType(arg);

            Build = (buildContext, buildArgs) =>
            {
                string sql;
                Variable variable = TryGetVariable(buildContext);

                if (variable.IsInliner())
                {
                    string arg2 = variable.Name;
                    if (variable.DT != DT.InTable)
                    {
                        buildContext.TryTakeException(variable.DT.InvalidInlinerException(GetType().Name,
                            arg2, _inliners));
                        return null;
                    }

                    if (buildArgs.Executable != null)
                    {
                        // get param argument
                        ParameterArgument argument = buildArgs.Executable.GetInlinerArgument(arg2);
                        if (argument.Value != null)
                        {
                            if (argument.Value is System.String)
                            {
                                arg2 = Filter.DelimitTableMultiPart((string)argument.Value, out chainException);
                            }
                            else if (argument.Value is View)
                            {
                                arg2 = Filter.Enclose(arg2);
                            }
                            else
                            {
                                arg2 = argument.Build(buildContext, buildArgs);
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

                else
                {
                    if (ProcessVariable(buildContext, buildArgs, out sql, variable))
                    {
                        return sql;
                    }

                    // specific (existing) variable checks
                    if (variable != null && variable.VariableType.IsParamOrSqlVariable())
                    {
                        // variable should be a table
                        if (!variable.DT.IsTable())
                        {
                            buildContext.TryTakeException(CreateException(QueryTalkExceptionType.TableVariableMissing, arg));
                            return null;
                        }

                        // check concatenation with non-udt table variable (NOT ALLOWED)
                        if (buildContext.Current.Query.Master.IsConcatenated)
                        {
                            if (variable.NameType == null)
                            {
                                buildContext.TryTakeException(CreateException(QueryTalkExceptionType.TableVariableDisallowed, arg));
                                return null;
                            }
                        }
                    }
                }

                // default
                sql = Filter.DelimitMultiPartOrParam(arg, IdentifierType.Table, out chainException);
                buildContext.TryTakeException(chainException);
                return sql;
            };  
        }

        private static QueryTalkException CreateException(QueryTalkExceptionType exceptionType, string variable)
        {
            return new QueryTalkException("TableArgument.Build", exceptionType,
                String.Format("variable = {0}", variable));
        }

        /// <summary>
        /// Implicitly converts an argument into the object of Table type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Table(System.String arg)
        {
            return new Table(arg);
        }

        #endregion

        #region Identifier

        // alias: for db objects only
        internal Table(Identifier arg, AliasAs alias = null)
            : this(arg as Chainer)
        {
            Alias = alias;
            SetArgType(arg);    
        }

        /// <summary>
        /// Implicitly converts an argument into the object of Table type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Table(Identifier arg)
        {
            return new Table(arg);
        }

        #endregion

        #region Udf

        internal Table(Udf arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of Table type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Table(Udf arg)
        {
            return new Table(arg);
        }

        #endregion

        #region View

        internal Table(View arg)
            : base(arg)
        {
            if (CheckNull(Arg(() => arg, arg)))
            {
                TryTakeException(arg.Exception);
                Build = (buildContext, buildArgs) =>
                    {
                        return (arg.Build(buildContext, buildArgs));
                    };
            }

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of Table type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Table(View arg)
        {
            return new Table(arg);
        }

        #endregion

        #region Concatenator

        internal Table(Concatenator arg)
            : base(arg)
        {
            TryTakeAll(arg);
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of Table type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Table(Concatenator arg)
        {
            return new Table(arg);
        }

        #endregion

    }
}
