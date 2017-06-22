#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class SelectChainer : EndChainer, IQuery, IOpenView, IViewAllowed, ISelectable,
        IAny,
        IFrom,
        ICollect,
        ITop, 
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
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal override string Method
        {
            get
            {
                return (IsCollect) ? Text.Method.Collect : Text.Method.Select;
            }
        }

        private string _intoMethod = Text.Method.IntoVars;

        internal bool IsCollect { get; private set; }

        private Column[] _columns;
        internal Column[] Columns
        {
            get
            {
                return _columns;
            }
        }

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

        private bool _isEmpty;
        bool ISelectable.IsEmpty
        {
            get
            {
                return _isEmpty;
            }
        }

        private string[] _variables;
        string[] ISelectable.Variables
        {
            set
            {
                if (value == null || value.Length == 0)
                {
                    return;
                }

                if (_isEmpty)
                {
                    Throw(QueryTalkExceptionType.UndefinedColumnsDisallowed, null, _intoMethod);
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

                Array.ForEach(value, variable =>
                {
                    SetChainer.CheckAndThrow(variable, GetRoot(), _intoMethod, value);
                });

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

        internal SelectChainer(Chainer prev, Column[] columns, bool isDistinct)
            : base(prev)
        {
            _Body(columns, isDistinct);
        }

        internal SelectChainer(ISemantic prev, Column[] columns, bool isDistinct)
            : base(prev.Translate(new SemqContext(((DbNode)prev).Root), null))
        {
            _Body(columns, isDistinct);
        }

        private void _Body(Column[] columns, bool isDistinct)
        {
            IsCollect = false;
            UseStringAsValue = false;
            Query.Clause.Select = this;

            if (columns == null)
            {
                _columns = new Column[] { Designer.Null };
            }
            else if (columns.Length == 0)
            {
                _columns = new Column[] { Wall.Text.Asterisk };
                _isEmpty = true;
            }
            else
            {
                _columns = columns;
            }

            // null column correction
            for (int i = 0; i < _columns.Length; ++i)
            {
                if (_columns[i] == null)
                {
                    _columns[i] = Designer.Null;
                }
            }

            if (isDistinct)
            {
                chainKeyword = Text.SelectDistinct;
                chainMethod = Text.Method.SelectDistinct;
                IsDistinct = isDistinct;
            }
            else
            {
                chainKeyword = Text.Select;
                chainMethod = Text.Method.Select;
            }

            Query.AddArguments(_columns);

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(200)
                    .NewLine(Keyword).S()
                    .Append(BuildTop(buildContext));

                TryAddAsteriskColumns();

                sql.Append(Column.Concatenate(_columns, buildContext, buildArgs, _variables));

                TryThrow(buildContext);

                return sql.ToString();
            };

        }

        private void TryAddAsteriskColumns()
        {
            List<Column> columns = new List<Column>();

            foreach (var column in _columns)
            {
                string alias;
                if (!DetectAsterisk(column, out alias))
                {
                    columns.Add(column);
                    continue;
                }

                // we found the asterisk:

                var ctable = GetTarget(ref alias);

                if (column.Original is DbColumns)
                {
                    var dbColumns = (DbColumns)column.Original;

                    string dbColumnsAlias = dbColumns.Alias;

                    columns.AddRange(
                        DbMapping.GetNodeMap(dbColumns.Parent.NodeID)
                            .GetColumnsByDatabaseOrder(dbColumnsAlias, dbColumns.Parent));

                    continue;
                }              

                // if ctable is found check if it contains node
                if (ctable != null && ctable.Node != null 
                    && !ctable.Node.IsSynonym      // if node has a query synonym
                                                   // than use the asterix (*) 
                                                   // since the columns are not mapped to the node
                    )
                {
                    var allColumns = DbMapping.GetNodeMap(ctable.Node.NodeID).GetColumnsByDatabaseOrder(alias);
                    columns.AddRange(allColumns);
                    continue;
                }       

                columns.Add(column);
            }

            _columns = columns.ToArray();
        }

        // detects if the given column contain asterix sign 
        private static bool DetectAsterisk(Column column, out string alias)
        {
            alias = null;

            if (column.Original is System.String)
            {
                var pair = new AliasColumnPair((string)column.Original);
                if (pair.Column != null && pair.Column.Trim() == Text.Asterisk)
                {
                    alias = pair.Alias;
                    return true;
                }
                else if (pair.Column == null && pair.Alias.Trim() == Text.Asterisk)
                {
                    return true;
                }

                return false;
            }
            else if (column.Original is Identifier)
            {
                var identifier = (Identifier)column.Original;
                if (identifier.NumberOfParts == 1 && identifier.Part1 == Text.Asterisk)
                {
                    return true;
                }
                else if (identifier.NumberOfParts == 2 && identifier.Part2 == Text.Asterisk)
                {
                    alias = identifier.Part1;
                    return true;
                }
            }
            else if (column.Original is DbColumns)
            {
                if (column.ArgType == typeof(OfChainer))
                {
                    alias = ((OfChainer)column.Original).Column.OfAlias ?? null;
                }
                else
                {
                    alias = ((DbColumns)column.Original).Alias ?? null;
                }

                return true;
            }

            return false;
        }

        // Get target table by alias.
        //   anticipated:
        //     If alias is not given, then the target is the first table - which must be FromChainer.
        private TableChainer GetTarget(ref string alias)
        {
            TableChainer ctable = GetPrevTable(Statement.StatementIndex);

            if (alias == null)
            {
                if (ctable is FromChainer)
                {
                    alias = ctable.Alias.Name;
                    return ctable;
                }

                return null;
            }

            while (ctable != null)
            {
                // case-sensitive comparison
                if (ctable.Alias.Name.EqualsCS(alias))
                {
                    return ctable;
                }

                ctable = ctable.GetPrevTable(Statement.StatementIndex);
            }

            return null;  // not found
        }

    }
}
