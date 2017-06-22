#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryTalk.Wall
{
    // Represents a SQL query.
    internal sealed class Query : Statement
    {

        #region Properties

        internal string Name
        {
            get
            {
                return Root.Name;
            }
        }

        internal bool HasFrom 
        {
            get
            {
                return _defaultPart.Tables.Count > 0;
            }
        }

        #region Query Clauses

        // query parts 
        //   Two query parts are separated by .Union method.
        private List<QueryPart> _parts;

        private QueryPart _defaultPart
        {
            get
            {
                return _parts[0];
            }
        }

        // last query part
        internal QueryPart Clause
        {
            get
            {
                return _parts[_parts.Count - 1]; 
            }
        }

        internal void SetUnion(Chainer clause)
        {
            Clause.Union = clause;
            _parts.Add(new QueryPart());
        }

        internal void SetInsert(Chainer clause)
        {
            _defaultPart.Insert = clause;
        }

        internal void SetUpdate(Chainer clause)
        {
            _defaultPart.Update = clause;
        }

        // Adds CTE or table to the corresponding collection.
        internal void AddTableObject(TableChainer tableObject)
        {
            FromChainer cte;
            if (IsPrevCte(tableObject, out cte))
            {
                QueryTalkException exception = null;
                if (!(cte.TableArgument.Original is View))
                {
                    exception = new QueryTalkException("Query.AddTableObject",
                        QueryTalkExceptionType.InvalidCte, String.Format("CTE = {0}",
                        cte.TableArgument.Original), Text.Method.From);
                }

                if (_parts.Count > 1)
                {
                    exception = new QueryTalkException("Query.AddTableObject",
                        QueryTalkExceptionType.InvalidCte, String.Format("CTE = {0}",
                        cte.TableArgument.Original), Text.Method.From);
                }

                if (exception != null)
                {
                    exception.ObjectName = Root.Name;
                    throw exception;
                }

                Clause.Ctes.Add(cte);
                Clause.Tables.Remove(cte);        
                Clause.Tables.Add(tableObject);
            }
            else
            {
                Clause.Tables.Add(tableObject);
            }

            if (tableObject.Node != null && tableObject.Node.Index > 0)
            {
                _tableAlias = tableObject.Node.Index;
            }
        }

        #endregion

        #region Master
        // Note that the proper query hierarchy can be achieved only in the building phase.

        // most outer (parent) query of this query
        private Query _master;
        internal Query Master 
        {
            get
            {
                if (_master == null)
                {
                    return this;
                }
                else
                {
                    return _master;
                }
            }
            set
            {
                _master = value;
            }
        }

        // zero-based level of the query - Master has level 0
        internal int Level { get; set; }

        internal bool IsMaster
        {
            get
            {
                return Level == 0;
            }
        }

        #endregion

        // concatenation root of the query
        //   If set then query is built on the concatenation root.
        //   A query has a concatenation root, if it contains at least one concatenator.
        internal Designer ConcatRoot { get; set; }

        internal bool IsConcatenated 
        {
            get
            {
                return ConcatRoot != null;
            }
        }

        private int _tableAlias = 0;
        internal int GetIncrementalTableAlias()
        {
            ++_tableAlias; 
            return _tableAlias;
        }

        internal List<Argument> Arguments { get; set; }
        internal void AddArgument(Argument argument)
        {
            Arguments.Add(argument);
        }
        internal void AddArguments(Argument[] arguments)
        {
            if (arguments != null)
            {
                Arguments.AddRange(arguments);
            }
        }

        #endregion

        #region Constructor

        internal Query(Chainer firstObject)
            : base(firstObject)
        {
            _parts = new List<QueryPart>();
            _parts.Add(new QueryPart());
            Arguments = new List<Argument>();

            Build = (buildContext, buildArgs) =>
                {
                    return BuildQuery(buildContext, buildArgs);
                };
        }

        #endregion

        #region Cte

        internal static bool IsPrevCte(Chainer from, out FromChainer cte)
        {
            cte = null;
            if (from is FromChainer)
            {
                if (from.Prev is FromChainer)
                {
                    cte = (FromChainer)from.Prev;
                    return true;
                }
                if (from.Prev is FromAsChainer)
                {
                    cte = (FromChainer)from.Prev.Prev;
                    return true;
                }
            }

            return false;
        }

        internal static bool IsPrevCte(Chainer from)
        {
            return
                from is FromChainer
                    &&
                (from.Prev is FromChainer || from.Prev is FromAsChainer);
        }

        #endregion

        #region BuildQuery

        // builds the query statement
        private string BuildQuery(BuildContext buildContext, BuildArgs buildArgs)
        {
            // perform concatenation check
            //   If a concatenator is found then the query is assigned the concatenation root and consecutively, 
            //   all the variables are properly concatenated and the query is executed in its own exec context.
            CheckConcat(this, buildContext);

            var sql = Text.GenerateSql(250);

            if (Root.CompilableType == Compilable.ObjectType.View
                && _defaultPart.Ctes.Count > 0)
            {
                Root.Throw(QueryTalkExceptionType.InvalidView, null, Text.Method.From);
            }

            // build clauses that belong to the default query part
            BuildCtes(buildContext, buildArgs, sql);
            BuildInsert(buildContext, buildArgs, sql);
            BuildUpdate(buildContext, buildArgs, sql);

            _parts.ForEach(part =>
                {
                    part.Joiner = ProcessJoiner(part, buildContext, buildArgs);
                    BuildCollect(part, buildContext, buildArgs, sql);
                    BuildSelect(part, buildContext, buildArgs, sql);
                    BuildDelete(part, buildContext, buildArgs, sql);
                    part.IntoTempTable.BuildNode(part, buildContext, buildArgs, sql);
                    BuildFrom(part, buildContext, buildArgs, sql);
                    BuildWhere(part, buildContext, buildArgs, sql);
                    part.GroupBy.BuildNode(part, buildContext, buildArgs, sql);
                    part.Having.BuildNode(part, buildContext, buildArgs, sql);
                    part.OrderBy.BuildNode(part, buildContext, buildArgs, sql);
                    part.Union.BuildNode(part, buildContext, buildArgs, sql);
                });

            if (!buildContext.Root.CompilableType.IsViewOrMapper())
            {
                sql.TerminateSingle();
            }

            // concatenation:
            if (IsConcatenated && IsMaster)
            {
                string concatBody = sql.ToString();
                return Text.GenerateSql(500)
                    .NewLine(Text.Set).S()
                    .Append(Text.Reserved.ConcatVar).Append(Text._Equal_)
                    .Append(Text.N).Append(Text.SingleQuote)
                    .Append(concatBody)
                    .Append(Text.SingleQuote).Terminate()
                    .NewLine(Text.Free.ExecSpExecutesql).S()
                    .Append(Text.Reserved.ConcatVar)
                    .Append(BuildConcatParams())
                    .TerminateSingle()
                    .ToString();
            }
            else
            {
                return sql.ToString();
            }
        }

        private void BuildCtes(BuildContext buildContext, BuildArgs buildArgs, StringBuilder sql)
        {
            for (int i = 0; i < _defaultPart.Ctes.Count; ++i)
            {
                sql.Append(_defaultPart.Ctes[i].BuildCte(buildContext, buildArgs, i == 0));
            }
        }

        private void BuildInsert(BuildContext buildContext, BuildArgs buildArgs, StringBuilder sql)
        {
            _defaultPart.Insert.BuildNode(_defaultPart, buildContext, buildArgs, sql);
        }

        private void BuildUpdate(BuildContext buildContext, BuildArgs buildArgs, StringBuilder sql)
        {
            _defaultPart.Update.BuildNode(_defaultPart, buildContext, buildArgs, sql);
        }

        private static void BuildDelete(QueryPart part, BuildContext buildContext, BuildArgs buildArgs, StringBuilder sql)
        {
            part.Delete.BuildNode(part, buildContext, buildArgs, sql);
        }

        private static void BuildCollect(QueryPart part, BuildContext buildContext, BuildArgs buildArgs, StringBuilder sql)
        {
            part.Collect.BuildNode(part, buildContext, buildArgs, sql);
        }

        private static void BuildSelect(QueryPart part, BuildContext buildContext, BuildArgs buildArgs, StringBuilder sql)
        {
            part.Select.BuildNode(part, buildContext, buildArgs, sql);
        }

        private static void BuildFrom(QueryPart part, BuildContext buildContext, BuildArgs buildArgs, StringBuilder sql)
        {
            // tables
            part.Tables.ForEach(table =>
            {
                table.BuildNode(part, buildContext, buildArgs, sql);
                var next = table.Next;

                if (next is AliasAs)
                {
                    next = next.Next;
                }

                while (next is IJoinCond)
                {
                    next.BuildNode(part, buildContext, buildArgs, sql);
                    next = next.Next;
                };
            });
        }

        private static Joiner ProcessJoiner(QueryPart queryPart, BuildContext buildContext, BuildArgs buildArgs)
        {
            var joiner = new Joiner(queryPart, buildContext, buildArgs);
            joiner.ProcessJoin();
            return joiner;
        }

        private static void BuildWhere(QueryPart part, BuildContext buildContext, BuildArgs buildArgs, StringBuilder sql)
        {
            // wheres (WHERE ... AND ... OR ...)
            part.Wheres.ForEach(where =>
            {
                where.BuildNode(part, buildContext, buildArgs, sql);
            });
        }

        #endregion

        #region Query Concatenation

        internal bool CheckConcat(Query query, BuildContext buildContext)
        {
            foreach (var arg in Arguments)
            {
                if (arg == null)
                {
                    continue;
                }

                if (arg.TryGetVariable(buildContext).IsConcatenator())
                {
                    query.ConcatRoot = new InternalRoot();
                    return true;
                }
            }

            return false;
        }

        private string BuildConcatParams()
        {
            StringBuilder declaration = Text.GenerateSql(100);
            StringBuilder assignment = Text.GenerateSql(100);

            bool first = true;
            ConcatRoot.AllParams
                .Where(param => !param.DT.IsVTB()).ToList()  // exclude table variables, temp tables and bulk tables
                .ForEach(param =>
            {
                assignment.NewLine(Text.Comma); 

                if (!first)
                {
                    declaration.NewLine(Text.Comma);
                }
                
                string declare;
                string assign;

                // parameterized value:
                if (param.ParameterizedValue != null)
                {
                    declare = Executable.GetParamDeclaration(param.ParameterizedValue, true, false);
                    var value = param.ParameterizedValue;
                    if (value.DataType != null)
                    {
                        assign = Mapping.Build(value.Value, value.DataType);
                    }
                    else
                    {
                        assign = Mapping.BuildUnchecked(value.Value);
                    }
                }
                // SQL variable:
                else
                {
                    declare = param.BuildDeclaration();
                    assign = param.Name;   // assign the same outer variable
                }

                declaration
                    .Append(param.Name)
                    .Append(Text._As_)
                    .Append(declare);

                assignment.Append(param.Name)
                    .Append(Text.Equal)
                    .Append(assign);

                first = false;
            });

            return Text.GenerateSql(200)
                .NewLine(Text.Free.CommaNSingleQuote)
                .Append(declaration)
                .Append(Text.SingleQuote)
                .Append(assignment)
                .ToString();
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            if (IsMaster)
            {
                return String.Format("MasterQuery.{0} ({1})", StatementIndex, Name);
            }
            else
            {
                return String.Format("ChildQuery.{0} ({1}/{2})", StatementIndex, Name, Master.Name);
            }
        }

        #endregion

    }
}
