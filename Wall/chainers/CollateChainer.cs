#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Linq;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class CollateChainer : Chainer, IQuery, IColumnName, INonPredecessor,
        IScalar,
        IAs
    {
        // allowed inliners
        private static readonly DT[] _inliners = { DT.InColumn };

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

        internal CollateChainer(Chainer prev, string collation) 
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => collation, collation));

            Build = (buildContext, buildArgs) =>
                {
                    if (prev is IColumnName)
                    {
                        _columnName = ((IColumnName)prev).ColumnName;
                    }

                    return Text.GenerateSql(50)
                        .Append(prev.Build(buildContext, buildArgs)).S()
                        .Append(Text.Collate).S()
                        .Append(collation)
                        .ToString();
                };
        }

        internal CollateChainer(System.String identifier, string collation)
            : base(null)
        {
            CheckNullAndThrow(Arg(() => identifier, identifier));
            CheckNullAndThrow(Arg(() => collation, collation));

            Build = (buildContext, buildArgs) =>
            {
                string sql;
                Variable variable = buildContext.TryGetVariable(identifier, out chainException);
                if (variable.IsInliner())
                {
                    if (!_inliners.Contains(variable.DT))
                    {
                        buildContext.TryTakeException(variable.DT.InvalidInlinerException(GetType().Name,
                            variable.Name, _inliners));
                        return null;
                    }

                    var arg2 = variable.Name;
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
                            else
                            {
                                _columnName = ((Column)inlinerArgument.Value).ColumnName;
                                return ((Column)inlinerArgument.Value).Build(buildContext, buildArgs);
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
                    Column argument = new Column(identifier);  
                    if (argument.ProcessVariable(buildContext, buildArgs, out sql, variable))
                    {
                        return sql;
                    }
                }

                // not a variable:

                _columnName = identifier;

                sql = Text.GenerateSql(100)
                    .Append(Filter.DelimitMultiPartOrParam(identifier, IdentifierType.ColumnOrParam, out chainException)).S()
                    .Append(Text.Collate).S()
                    .Append(collation)
                    .ToString();
                 buildContext.TryTakeException(chainException);
                 return sql;
            };
        }

        internal CollateChainer(DbColumn column, string collation)
            : base(null)
        {
            CheckNullAndThrow(Arg(() => collation, collation));

            Build = (buildContext, buildArgs) =>
            {
                return Text.GenerateSql(50)
                    .Append(column.Build(buildContext, buildArgs)).S()
                    .Append(Text.Collate).S()
                    .Append(collation)
                    .ToString();
            };
        }
    }
}
