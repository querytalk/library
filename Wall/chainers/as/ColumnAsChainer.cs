#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public class ColumnAsChainer : AliasAs, IQuery, IColumnName
    {
        // allowed inliners
        private static readonly DT[] _inliners = { DT.InColumn };

        internal override string Method
        {
            get
            {
                return Text.Method.As;
            }
        }

        private object _value;
        private Func<BuildContext, BuildArgs, string> _prevBuild = null;

        #region IColumnName

        private string _columnName;
        string IColumnName.ColumnName
        {
            get
            {
                return _columnName;
            }
        }

        #endregion

        #region ChainObject

        internal ColumnAsChainer(Chainer prev, string alias) 
            : base(prev, alias)
        {
            if (!CheckNull(Arg(() => prev, prev)))
            {
                chainException.Extra = Text.Free.ChainObjectNullExtra;
                return;
            }

            if (chainException != null)
            {
                return;
            }

            _columnName = alias;
            _prevBuild = (buildContext, buildArgs) =>
                {
                    return prev.Build(buildContext, buildArgs);
                };

            Build = (buildContext, buildArgs) =>
            {
                // do not perform critical alias check on scalars, only on table arguments
                if (!(prev is IScalar))
                {
                    Statement.CheckAlias(this);
                }

                var sql = Text.GenerateSql(20)
                    .Append(prev.Build(buildContext, buildArgs))
                    .Append(Text._As_)
                    .Append(Filter.Delimit(Name))
                    .ToString();

                TryThrow(buildContext);

                return sql;
            };
        }

        #endregion

        #region System.String

        internal ColumnAsChainer(string column, string alias)
            : base(alias)
        {
            _columnName = alias;

            if (!CheckNull(Arg(() => column, column)))
            {
                return;
            }

            if (chainException != null)
            {
                return;
            }

            Build = (buildContext, buildArgs) =>
            {
                QueryTalkException exception;
                Variable variable = buildContext.TryGetVariable(column, out exception);
                string inlinerSql = null;
                string columnSql = null;

                if (variable.IsInliner())
                {
                    inlinerSql = variable.Name;
                    if (variable.DT != DT.InColumn)
                    {
                        buildContext.TryTakeException(variable.DT.InvalidInlinerException(GetType().Name, inlinerSql, _inliners));
                        return null;
                    }

                    if (buildArgs.Executable != null)
                    {
                        ParameterArgument argument = buildArgs.Executable.GetInlinerArgument(inlinerSql);
                        if (argument.Value != null)
                        {
                            if (argument.Value is System.String)
                            {
                                inlinerSql = Filter.DelimitColumnMultiPart((string)argument.Value, out chainException);
                            }
                            else
                            {
                                inlinerSql = ((Column)argument.Value).Build(buildContext, buildArgs);
                            }
                        }
                        else
                        {
                            buildContext.TryTakeException(new QueryTalkException(this,
                                QueryTalkExceptionType.InlinerArgumentNull,
                                String.Format("{0} = null", inlinerSql)));
                            return null;
                        }
                    }
                }
                else
                {
                    columnSql = Variable.ProcessVariable(column, buildContext, buildArgs, out chainException);
                    TryThrow();

                    if (columnSql == null)
                    {
                        if (buildContext.IsCurrentStringAsValue)
                        {
                            columnSql = buildContext.BuildString(column);
                        }
                        else
                        {
                            columnSql = Filter.DelimitMultiPartOrParam(column, IdentifierType.ColumnOrParam, out chainException);
                        }
                    }
                }

                var sql = Text.GenerateSql(100)
                    .Append(inlinerSql ?? columnSql).S()
                    .Append(Text.As).S()
                    .Append(Filter.Delimit(Name))
                    .ToString();

                    buildContext.TryTakeException(chainException);

                return sql;
            };
        }

        #endregion

        #region QueryTalk CLR types

        internal ColumnAsChainer(System.Boolean value, string alias)
            : base(alias)
        {
            _value = value;
            _columnName = alias;
            if (chainException != null)
            {
                return;
            }

            _prevBuild = (buildContext, buildArgs) =>
            {
                return value.Parameterize(buildContext) ?? Mapping.BuildCast(value);
            };

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(20)
                    .Append(_prevBuild(buildContext, buildArgs))
                    .Append(Text._As_)
                    .Append(Filter.Delimit(Name))
                    .ToString();
            };
        }

        internal ColumnAsChainer(System.Byte value, string alias)
            : base(alias)
        {
            _value = value;
            _columnName = alias;
            if (chainException != null)
            {
                return;
            }

            _prevBuild = (buildContext, buildArgs) =>
            {
                return value.Parameterize(buildContext) ?? Mapping.BuildCast(value);
            };

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(20)
                    .Append(_prevBuild(buildContext, buildArgs))
                    .Append(Text._As_)
                    .Append(Filter.Delimit(Name))
                    .ToString();
            };
        }

        internal ColumnAsChainer(System.Byte[] value, string alias)
            : base(alias)
        {
            _value = value;
            _columnName = alias;
            if (chainException != null)
            {
                return;
            }

            _prevBuild = (buildContext, buildArgs) =>
            {
                return value.Parameterize(buildContext) ?? Mapping.BuildCast(value);
            };

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(20)
                    .Append(_prevBuild(buildContext, buildArgs))
                    .Append(Text._As_)
                    .Append(Filter.Delimit(Name))
                    .ToString();
            };
        }

        internal ColumnAsChainer(System.DateTime value, string alias)
            : base(alias)
        {
            _value = value;
            _columnName = alias;
            if (chainException != null)
            {
                return;
            }

            _prevBuild = (buildContext, buildArgs) =>
            {
                return value.Parameterize(buildContext) ?? Mapping.BuildCast(value);
            };

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(20)
                    .Append(_prevBuild(buildContext, buildArgs))
                    .Append(Text._As_)
                    .Append(Filter.Delimit(Name))
                    .ToString();
            };
        }

        internal ColumnAsChainer(System.DateTimeOffset value, string alias)
            : base(alias)
        {
            _value = value;
            _columnName = alias;
            if (chainException != null)
            {
                return;
            }

            _prevBuild = (buildContext, buildArgs) =>
            {
                return value.Parameterize(buildContext) ?? Mapping.BuildCast(value);
            };

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(20)
                    .Append(_prevBuild(buildContext, buildArgs))
                    .Append(Text._As_)
                    .Append(Filter.Delimit(Name))
                    .ToString();
            };
        }

        internal ColumnAsChainer(System.Decimal value, string alias)
            : base(alias)
        {
            _value = value;
            _columnName = alias;
            if (chainException != null)
            {
                return;
            }

            _prevBuild = (buildContext, buildArgs) =>
            {
                return value.Parameterize(buildContext) ?? Mapping.BuildCast(value);
            };

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(20)
                    .Append(_prevBuild(buildContext, buildArgs))
                    .Append(Text._As_)
                    .Append(Filter.Delimit(Name))
                    .ToString();
            };
        }

        internal ColumnAsChainer(System.Double value, string alias)
            : base(alias)
        {
            _value = value;
            _columnName = alias;
            if (chainException != null)
            {
                return;
            }

            _prevBuild = (buildContext, buildArgs) =>
            {
                return value.Parameterize(buildContext) ?? Mapping.BuildCast(value);
            };

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(20)
                    .Append(_prevBuild(buildContext, buildArgs))
                    .Append(Text._As_)
                    .Append(Filter.Delimit(Name))
                    .ToString();
            };
        }

        internal ColumnAsChainer(System.Guid value, string alias)
            : base(alias)
        {
            _value = value;
            _columnName = alias;
            if (chainException != null)
            {
                return;
            }

            _prevBuild = (buildContext, buildArgs) =>
            {
                return value.Parameterize(buildContext) ?? Mapping.BuildCast(value);
            };

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(20)
                    .Append(_prevBuild(buildContext, buildArgs))
                    .Append(Text._As_)
                    .Append(Filter.Delimit(Name))
                    .ToString();
            };
        }

        internal ColumnAsChainer(System.Int16 value, string alias)
            : base(alias)
        {
            _value = value;
            _columnName = alias;
            if (chainException != null)
            {
                return;
            }

            _prevBuild = (buildContext, buildArgs) =>
            {
                return value.Parameterize(buildContext) ?? Mapping.BuildCast(value);
            };

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(20)
                    .Append(_prevBuild(buildContext, buildArgs))
                    .Append(Text._As_)
                    .Append(Filter.Delimit(Name))
                    .ToString();
            };
        }

        internal ColumnAsChainer(System.Int32 value, string alias)
            : base(alias)
        {
            _value = value;
            _columnName = alias;
            if (chainException != null)
            {
                return;
            }

            _prevBuild = (buildContext, buildArgs) =>
            {
                return value.Parameterize(buildContext) ?? Mapping.BuildCast(value);
            };

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(20)
                    .Append(_prevBuild(buildContext, buildArgs))
                    .Append(Text._As_)
                    .Append(Filter.Delimit(Name))
                    .ToString();
            };
        }

        internal ColumnAsChainer(System.Int64 value, string alias)
            : base(alias)
        {
            _value = value;
            _columnName = alias;
            if (chainException != null)
            {
                return;
            }

            _prevBuild = (buildContext, buildArgs) =>
            {
                return value.Parameterize(buildContext) ?? Mapping.BuildCast(value);
            };

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(20)
                    .Append(_prevBuild(buildContext, buildArgs))
                    .Append(Text._As_)
                    .Append(Filter.Delimit(Name))
                    .ToString();
            };
        }

        internal ColumnAsChainer(System.Single value, string alias)
            : base(alias)
        {
            _value = value;
            _columnName = alias;
            if (chainException != null)
            {
                return;
            }

            _prevBuild = (buildContext, buildArgs) =>
            {
                return value.Parameterize(buildContext) ?? Mapping.BuildCast(value);
            };

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(20)
                    .Append(_prevBuild(buildContext, buildArgs))
                    .Append(Text._As_)
                    .Append(Filter.Delimit(Name))
                    .ToString();
            };
        }

        internal ColumnAsChainer(System.TimeSpan value, string alias)
            : base(alias)
        {
            _value = value;
            _columnName = alias;
            if (chainException != null)
            {
                return;
            }

            _prevBuild = (buildContext, buildArgs) =>
            {
                return value.Parameterize(buildContext) ?? Mapping.BuildCast(value);
            };

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(20)
                    .Append(_prevBuild(buildContext, buildArgs))
                    .Append(Text._As_)
                    .Append(Filter.Delimit(Name))
                    .ToString();
            };
        }

        #endregion

        #region DbColumn

        internal ColumnAsChainer(DbColumn column, string alias)
            : base(null, alias)
        {
            if (!CheckNull(Arg(() => column, column)))
            {
                return;
            }

            if (chainException != null)
            {
                return;
            }

            _columnName = alias;

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(20)
                    .Append(column.Build(buildContext, buildArgs))
                    .Append(Text._As_)
                    .Append(Filter.Delimit(Name))
                    .ToString();

                TryThrow(buildContext);

                return sql;
            };
        }

        #endregion

    }
}
