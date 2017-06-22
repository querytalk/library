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
    public sealed class CollectChainer : EndChainer, IBegin, IQuery, IOpenView, IViewAllowed, ISelectable,
        IThenCollect,
        IUnion,
        IIntoVars,
        IIntoTempTable,  
        IInsert,
        IColumns,
        IEndView,
        IEndProc,
        IConnectBy,
        IGo
    {
        internal override string Keyword
        {
            get
            {
                return Text.Select;
            }
        }

        internal override string Method
        {
            get
            {
                return Text.Method.Collect;
            }
        }

        private string _intoMethod = Text.Method.IntoVars;
        private Column[] _columns;

        #region ISelectable

        Column[] ISelectable.Columns
        {
            get
            {
                return _columns;
            }
        }

        int ISelectable.ColumnCount
        {
            get
            {
                if (_columns != null)
                {
                    return _columns.Count();
                }
                else
                {
                    return 0;
                }
            }
        }

        bool ISelectable.IsEmpty
        {
            get
            {
                return false;  
            }
        }

        // collection of variables used by .IntoVars method
        private string[] _variables;
        string[] ISelectable.Variables
        {
            set
            {
                if (value == null || value.Length == 0)
                {
                    return;
                }

                if (value.Count() != _columns.Count())
                {
                    Throw(QueryTalkExceptionType.VariableColumnCountMismatch,
                        String.Format("column count = {0}{1}   variable count = {2}{3}   variables = ({4})",
                                _columns.Count(), Environment.NewLine,
                                value.Count(), Environment.NewLine,
                                String.Join(", ", value.Select(v => v ?? Text.ClrNull))),
                        _intoMethod);
                }

                // check variable names
                Array.ForEach(value, variable =>
                {
                    SetChainer.CheckAndThrow(variable, GetRoot(), _intoMethod, value);
                });

                // check if none of the column is alias column
                var alias = _columns.Where(column => column != null
                    && column.Original is ColumnAsChainer).FirstOrDefault();
                if (alias != null)
                {
                    Throw(QueryTalkExceptionType.AliasedColumnDisallowed, null, _intoMethod);
                }

                _variables = value;
            }
        }

        #endregion

        internal CollectChainer(Chainer prev, Column[] columns)
            : base(prev)
        {
            UseStringAsValue = true;
            Query.Clause.Collect = this;

            if (columns == null)
            {
                _columns = new Column[] { Designer.Null };
            }
            else if (columns.Length == 0)
            {
                Throw(QueryTalkExceptionType.ArgumentNull, null);
            }
            else
            {
                _columns = columns;
            }

            // null column correction (important!)
            for (int i = 0; i < _columns.Length; ++i)
            {
                if (_columns[i] == null)
                {
                    _columns[i] = Designer.Null;
                }
            }

            Query.AddArguments(_columns);

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(200)
                    .NewLine(Keyword).S()
                    .Append(BuildTop(buildContext))
                    .Append(Column.Concatenate(_columns, buildContext, buildArgs, _variables));

                TryThrow(buildContext);

                return sql.ToString();
            };
        }

    }
}
