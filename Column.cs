#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents a column in the select statement.
    /// </summary>   
    public sealed class Column : Argument, IScalar,
        IAs
    {
        // allowed inliners
        private static readonly DT[] _inliners = { DT.InColumn };

        internal override string Method
        {
            get
            {
                return Text.Method.Select;
            }
        }

        private bool _isVariable;

        private string _columnName;
        internal string ColumnName
        {
            get
            {
                return _columnName;
            }
        }

        private Column(Chainer arg)
            : base(arg)
        {
            TryTakeAll(arg);
        }

        #region Identifier

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a column identifier.</param>
        public Column(Identifier arg)
            : this((Chainer)arg)
        {
            if (arg != null)
            {
                _columnName = ((IColumnName)arg).ColumnName;
            }

            CtorBody(arg);
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(Identifier arg)
        {
            return new Column(arg);
        }

        #endregion

        #region System.String

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a column identifier.</param>
        public Column(System.String arg)
            : base(arg)
        {
            SetArgType(arg);

            _isVariable = Variable.Detect(arg);

            Build = (buildContext, buildArgs) =>
            {
                if (buildContext == null)
                {
                    return Filter.Delimit(arg);
                };

                string sql;

                Variable variable = TryGetVariable(buildContext);

                if (variable.IsInliner())
                {
                    if (!_inliners.Contains(variable.DT))
                    {
                        buildContext.TryTakeException(variable.DT.InvalidInlinerException(GetType().Name,
                            variable.Name, _inliners));
                        return null;
                    }

                    string arg2 = variable.Name;

                    if (buildArgs.Executable != null)
                    {
                        ParameterArgument inlinerArgument = buildArgs.Executable.GetInlinerArgument(variable.Name);

                        if (inlinerArgument.Value != null)
                        {
                            if (inlinerArgument.Value is System.String)
                            {
                                _columnName = (System.String)inlinerArgument.Value;
                                return Filter.DelimitColumnMultiPart((string)inlinerArgument.Value, out chainException);
                            }
                            else if (inlinerArgument.Value is Column)
                            {
                                _columnName = ((Column)inlinerArgument.Value).ColumnName;
                                return ((Column)inlinerArgument.Value).Build(buildContext, buildArgs);
                            }
                            // Column[]
                            else
                            {
                                var columns = (Column[])inlinerArgument.Value;
                                return Column.Concatenate(columns, buildContext, buildArgs, null, null, false);
                            }
                        }
                        else
                        {
                            buildContext.TryTakeException(new QueryTalkException(this, QueryTalkExceptionType.InlinerArgumentNull,
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
                }

                if (buildContext.IsCurrentStringAsValue)
                {
                    return buildContext.BuildString(arg);
                }

                sql = Filter.DelimitMultiPartOrParam(arg, IdentifierType.ColumnOrParam, out chainException, out _columnName);
                buildContext.TryTakeException(chainException);

                return sql;
            };
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(System.String arg)
        {
            return new Column(arg);
        }

        #endregion

        #region QueryTalk CLR Types

        #region System.Boolean

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a value.</param>
        public Column(System.Boolean arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(System.Boolean arg)
        {
            return new Column(arg);
        }

        #endregion

        #region System.Byte

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a value.</param>
        public Column(System.Byte arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(System.Byte arg)
        {
            return new Column(arg);
        }

        #endregion

        #region System.Byte[]

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a value.</param>
        public Column(System.Byte[] arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(System.Byte[] arg)
        {
            return new Column(arg);
        }

        #endregion

        #region System.DateTime

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a value.</param>
        public Column(System.DateTime arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(System.DateTime arg)
        {
            return new Column(arg);   
        }

        #endregion

        #region System.DateTimeOffset

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a value.</param>
        public Column(System.DateTimeOffset arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(System.DateTimeOffset arg)
        {
            return new Column(arg);
        }

        #endregion

        #region System.Decimal

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a value.</param>
        public Column(System.Decimal arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(System.Decimal arg)
        {
            return new Column(arg);
        }

        #endregion

        #region System.Double

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a value.</param>
        public Column(System.Double arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(System.Double arg)
        {
            return new Column(arg);
        }

        #endregion

        #region System.Guid

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a value.</param>
        public Column(Guid arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(System.Guid arg)
        {
            return new Column(arg);
        }

        #endregion

        #region System.Int16

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a value.</param>
        public Column(System.Int16 arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(System.Int16 arg)
        {
            return new Column(arg);
        }

        #endregion

        #region System.Int32

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a value.</param>
        public Column(System.Int32 arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(System.Int32 arg)
        {
            return new Column(arg);
        }

        #endregion

        #region System.Int64

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a value.</param>
        public Column(System.Int64 arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(System.Int64 arg)
        {
            return new Column(arg);
        }

        #endregion

        #region System.Single

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a value.</param>
        public Column(System.Single arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(System.Single arg)
        {
            return new Column(arg);
        }

        #endregion

        #region System.TimeSpan

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a value.</param>
        public Column(System.TimeSpan arg)
            : base(arg)
        {
            Build = (buildContext, buildArgs) =>
            {
                return BuildClr(arg, buildContext);
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(System.TimeSpan arg)
        {
            return new Column(arg);
        }

        #endregion

        #endregion

        #region Sys

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a built-in SQL function.</param>
        public Column(SysFn arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(SysFn arg)
        {
            return new Column(arg);
        }

        #endregion

        #region Case

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a CASE statement.</param>
        public Column(CaseExpressionChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(CaseExpressionChainer arg)
        {
            return new Column(arg);
        }

        #endregion

        #region Of

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is an aliased mapped column.</param>
        public Column(OfChainer arg)
            : this(arg as Chainer)
        {
            // set column name
            if (arg != null)
            {
                _columnName = arg.Column.ColumnName;
            }

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(OfChainer arg)
        {
            return new Column(arg);
        }

        #endregion

        #region View

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a scalar view.</param>
        public Column(View arg)
            : base(arg)
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
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(View arg)
        {
            return new Column(arg);
        }

        #endregion

        #region Expression

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is an expression.</param>
        public Column(Expression arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(Expression arg)
        {
            return new Column(arg);
        }

        #endregion

        #region Value

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a Value argument.</param>
        public Column(Value arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(Value arg)
        {
            return new Column(arg);
        }

        #endregion

        #region Udf

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is an UDF function.</param>
        public Column(Udf arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(Udf arg)
        {
            return new Column(arg);
        }

        #endregion

        #region Alias

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is an aliased column.</param>
        public Column(ColumnAsChainer arg)
            : this(arg as Chainer)
        {
            CheckNullAndThrow(Arg(() => arg, arg));

            SetArgType(arg);

            if (arg.Exception == null)
            {
                if (arg.Prev is Identifier)
                {
                    // check if more than 2-parts
                    if ((arg.Prev as Identifier).NumberOfParts > 2)
                    {
                        chainException = new QueryTalkException(this, QueryTalkExceptionType.InvalidColumnIdentifier,
                            String.Format("identifier = {0} AS {1}",
                                arg.Prev.ToString(),
                                Filter.Delimit(arg.Name ?? "")));
                    }
                }
            }
            else
            {
                TryTakeException(arg.Exception);
                if (arg.IsUndefined)
                {
                    chainException.Extra = Text.Free.AliasNullExtra;
                }
            }

            if (arg != null)
            {
                _columnName = ((IColumnName)arg).ColumnName;
            }
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(ColumnAsChainer arg)
        {
            return new Column(arg);
        }

        #endregion

        #region Collate

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a COLLATE expression.</param>
        public Column(CollateChainer arg)
            : this(arg as Chainer)
        {
            if (CheckNull(Arg(() => arg, arg)))
            {
                Build = (buildContext, buildArgs) =>
                {
                    var sql = arg.Build(buildContext, buildArgs);

                    _columnName = ((IColumnName)arg).ColumnName;

                    return sql;
                };
            }

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(CollateChainer arg)
        {
            return new Column(arg);
        }

        #endregion

        #region DbColumn

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a mapped column.</param>
        public Column(DbColumn arg)
            : base(arg)
        {
            if (arg != null)
            {
                Build = arg.Build;
                _columnName = arg.ColumnName;
            }
            else
            {
                CheckNull(Arg(() => arg, arg));
            }

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(DbColumn arg)
        {
            return new Column(arg);
        }

        #endregion 

        #region DbColumns

        /// <summary>
        /// Represents a column in the select statement.
        /// </summary>
        /// <param name="arg">Is a mapped column.</param>
        public Column(DbColumns arg)
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
        /// Implicitly converts an argument into a Column.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Column(DbColumns arg)
        {
            return new Column(arg);
        }

        #endregion 

        #region ColumnArgument Converter

        internal static Column ToColumn(NonSelectColumnArgument column)
        {
            if (column == null)
            {
                return null;
            }

            if (column.ArgType == typeof(System.String))
            {
                return new Column((System.String)column.Original);
            }
            else if (column.ArgType == typeof(Identifier))
            {
                return new Column((Identifier)column.Original);
            }
            else if (column.ArgType == typeof(OfChainer))
            {
                return new Column((OfChainer)column.Original);
            }
            else if (column.ArgType == typeof(DbColumn))
            {
                return new Column((DbColumn)column.Original);
            }

            return null;
        }

        #endregion

        internal static string Concatenate(
            Column[] arguments,
            BuildContext buildContext, 
            BuildArgs buildArgs,
            string[] variables = null,
            string formatter = null,
            Nullable<bool> isProvideMissingNames = null)
        {
            var columns = new List<string>();      
            var columnNames = new List<string>();  

            // default formatter
            formatter = formatter ?? String.Format("{0}{1},", Environment.NewLine, Text.TwoSpaces);

            for (int i = 0; i < arguments.Length; ++i)
            {
                if (arguments[i] == null)
                {
                    arguments[i] = Designer.Null;
                }

                Column argument = arguments[i];

                if (argument.Exception != null)
                {
                    buildContext.TryTakeException(argument.Exception);
                    return null;
                }

                string column = argument.Build(buildContext, buildArgs);

                if (argument.Exception != null)
                {
                    buildContext.TryTakeException(argument.Exception);
                    return null;
                }

                if (buildContext.Exception != null)
                {
                    return null;
                }

                if (variables == null)
                {
                    columns.Add(column);
                    columnNames.Add(argument.ColumnName);
                }
                else
                {
                    // variables: 
                    if (column == Text.Asterisk)
                    {
                        buildContext.Exception = new QueryTalkException("Column.Concatenate",
                            QueryTalkExceptionType.UndefinedColumnsDisallowed,
                            String.Format("column = *{0}   variable = {1}",
                                Environment.NewLine, variables[i]));
                    }

                    columns.Add(String.Format("{0} = {1}", variables[i], column));
                }
            }

            // check duplicates and handle missing column names
            if (variables == null)
            {
                if (isProvideMissingNames == null)
                {
                    isProvideMissingNames = 
                        !(buildContext.Current is IWritable
                        && !buildContext.Current.Query.HasFrom);
                }

                // provide missing column names
                if (isProvideMissingNames == true)
                {
                    ProvideMissingNames(buildContext, arguments, columns, columnNames);
                }
            }

            return String.Join(formatter, columns.ToArray());
        }

        internal static void ProvideMissingNames(BuildContext buildContext, 
            Column[] arguments,       
            List<string> columns,      
            List<string> columnNames   
            )
        {
            for (int i = 0; i < columns.Count; ++i)
            {
                if (columnNames[i] != null)
                {
                    continue;
                }

                var argument = arguments[i];
                if (argument.ArgType == typeof(System.String) && argument._isVariable)
                {
                    QueryTalkException exception;

                    var variable = buildContext.TryGetVariable((string)argument.Original, out exception);
                    if (variable != null
                        && (variable.DT == DT.InColumn
                            || variable.IsConcatenator()))
                    {
                        continue;
                    }
                }

                string newColumnName = null;

                while (true)
                {
                    newColumnName = Text.Column + buildContext.Root.ColIndex.ToString();
                    if (columnNames.Contains(newColumnName))
                    {
                        continue;
                    }

                    columnNames[i] = newColumnName;
                    break;
                }

                columns[i] = String.Format("{0} AS [{1}]", columns[i], newColumnName);
            }
        }

    }
}
