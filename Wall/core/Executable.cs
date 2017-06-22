#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;

namespace QueryTalk.Wall
{
    // Executable object is a wrapper around compilable object. It represents an isolated execution context of the compilable object.
    // Every executable object is executed in its own executable context using sp_executesql procedure.
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class Executable : Chainer, IDesignerRoot, INonPredecessor, IExecutable, IName,
        IPass,
        IConnectBy
    {

        #region IDesignerRoot

        Designer IDesignerRoot.GetRoot()
        {
            if (_compilable != null)
            {
                return ((IDesignerRoot)_compilable).GetRoot();
            }
            else
            {
                return GetRoot();
            }
        }

        #endregion

        #region Fields & Properties

        private Designer _rootCompilable;      
        private StringBuilder _paramDeclaration;  
        private StringBuilder _paramAssignment;      

        private Compilable _compilable;
        internal Compilable Compilable
        {
            get
            {
                return _compilable;
            }
        }

        /// <summary>
        /// The name of the compilable object.
        /// </summary>
        string IName.Name
        {
            get
            {
                if (_compilable != null)
                {
                    return _compilable.Name;
                }
                else
                {
                    return Text.Unknown;
                }
            }
        }

        // collection of passed arguments
        private List<ParameterArgument> _arguments = new List<ParameterArgument>();

        internal List<ParameterArgument> Arguments
        {
            get
            {
                return _arguments;
            }
        }

        // true if executable is called by .Exec method in outer procedure
        private bool _inner = false;
        internal bool Inner
        {
            get
            {
                return _inner;
            }
            set
            {
                _inner = value;
            }
        }

        // a calling (parent) executable in nested hierarchy of QueryTalk procedures 
        private Executable _inlineCaller;
        internal Executable InlineCaller
        {
            get
            {
                return _inlineCaller;
            }
        }

        // a clean SQL body originated from the compilable object, inluding inlining, but no wrappers 
        private string _body;
        internal string Body
        {
            get
            {
                return _body;
            }
        }

        private string _sql;
        /// <summary>
        /// An executable SQL code with parameters. If null then compilable object contains inlining which is resolved after the .ConnectBy method is called.
        /// </summary>
        public string Sql
        {
            get
            {
                return _sql;
            }
        }

        #endregion

        #region Constructors

        internal Executable(Compilable compilable)
            : base(null)
        {
            _compilable = compilable;
            _rootCompilable = compilable.GetRoot();     

            Build = (buildContext, buildArgs) =>
            {
                var sql = new StringBuilder();

                // in case of inlining
                if (buildArgs.Executable != null)
                {
                    _inlineCaller = buildArgs.Executable;
                }

                if (buildArgs.TestBody != null)
                {
                    _body = buildArgs.TestBody;  
                }
                else
                {
                    _body = compilable.Build(buildContext, new BuildArgs(this));
                }

                string bodyWrapped = _body;

                // for stored procedures
                if (_compilable.CompilableType == Compilable.ObjectType.StoredProc)
                {
                    bodyWrapped = BuildStoredProc();
                }

                BuildParamAppendix(buildArgs);
                string bodyWrappedAndEscaped = Filter.Escape(bodyWrapped);    
                BuildTableValuedParams(sql);
                BuildBody(sql, bodyWrappedAndEscaped);

                _sql = sql.ToString();
                return _sql;
            };
        }

        internal Executable(Compilable compilable, ParameterArgument[] arguments)
            : this(compilable)
        {
            AddArguments(arguments);
            TryThrow(Text.Method.Pass);
        }

        #endregion

        #region Check/GetArgument

        internal bool CheckArgumentByName(string paramName)
        {
            return _arguments.Where(p => p.ParamName.EqualsCS(paramName))
                .Any();
        }

        internal ParameterArgument GetArgumentByName(string paramName)
        {
            return _arguments.Where(p => p.ParamName.EqualsCS(paramName))
                .FirstOrDefault();
        }

        #endregion

        #region AddArgument(s)

        private void AddArgument(string paramName, ParameterArgument argument)
        {
            if (chainException != null)
            {
                return;
            }

            if (argument == null)
            {
                argument = new ParameterArgument(Designer.Null);
            }

            // check if argument has already been passed
            if (CheckArgumentByName(paramName))
            {
                Throw( QueryTalkExceptionType.ParamArgumentAlreadyPassed,
                    String.Format("param = {0}{1}   argument = {2}",
                        paramName, Environment.NewLine, argument.Value.ToReport()),
                    Text.Method.Pass);
            }

            argument.BuildArgument(_rootCompilable, paramName);
            TryThrow(argument.Exception);

            if (argument.IsArgumentOutput) 
            {
                if (!_rootCompilable.ExplicitParams
                    .Where(p => p.Name.EqualsCS(paramName) && p.IsOutput)
                    .Any())
                {
                    Throw(QueryTalkExceptionType.NonOutputParam,
                        String.Format("param = {0}{1}   argument = {2}",
                            paramName, Environment.NewLine, argument.Value.ToReport()),
                        Text.Method.Pass);
                }
            }

            _arguments.Add(argument);
        }

        private void AddArguments(ParameterArgument[] arguments)
        {
            if (arguments == null)
            {
                return;
            }

            if (!_rootCompilable.ParamCountCheck(arguments.Length))
            {
                Throw(QueryTalkExceptionType.ArgumentCountMismatch,
                    String.Format("parameters count = {0}{1}   arguments count = {2}",
                        _rootCompilable.ExplicitParams.Count, Environment.NewLine, arguments.Length),
                    Text.Method.PassOrExec);
            }

            for (int i = 0; i < arguments.Length; ++i)
            {
                AddArgument(_rootCompilable.ExplicitParams[i].Name, arguments[i]);
            }
        }

        #endregion

        #region GetInlineArgument

        internal ParameterArgument GetInlinerArgument(string inlinerName)
        {
            return GetInlinerArgument(inlinerName, this);
        }

        private ParameterArgument GetInlinerArgument(string inlinerName, Executable executable)
        {
            ParameterArgument argument = executable.GetArgumentByName(inlinerName);

            if (argument.IsPassedVariable)
            {
                if (executable.InlineCaller != null)
                {
                    return GetInlinerArgument(argument.Value.ToString(), executable.InlineCaller);
                }

                return argument;
            }
            else
            {
                return argument;
            }
        }

        #endregion

        #region Critical check

        internal void ArgumentsCriticalCheck(bool outerCall)
        {
            if (chainException != null)
            {
                return;
            }

            if (Compilable is Procedure)
            {
                Designer root = (Designer)((Executable)Root).Compilable.Root;

                root.AllParams.ForEach(param =>
                {
                    if (!param.IsOptional)
                    {
                        if (!((Executable)Root).CheckArgumentByName(param.Name) && !param.IsParameterized)
                        {
                            Throw(QueryTalkExceptionType.ParamArgumentNotPassed, String.Format("param = {0}", param.Name),
                                Text.Method.Pass);
                        }
                    }
                });

                if (outerCall)
                {
                    return;
                }
                else
                {
                    CheckInnerCall();
                }
            }
            else if (!outerCall)
            {
                CheckInnerCall();
            }
        }

        internal static void CriticalCheckArgumentSize(ParameterArgument argument)
        {
            if (!Mapping.SqlMapping[argument.DT].CheckSize(
                argument.Value, argument.DataType.Length, argument.DataType.Precision, argument.DataType.Scale))
            {
                throw new QueryTalkException("CriticalCheckArgumentSize", 
                    QueryTalkExceptionType.InvalidArgumentSize,
                    String.Format("param = {0}{1}   " +
                        "parameter length = {2}{3}   " +
                        "parameter precision = {4}{5}   " +
                        "parameter scale = {6}{7}   " +
                        "argument = {8}{9}   " +
                        "argument db type = {10}",
                        argument.ParamName, Environment.NewLine,
                        argument.DataType.Length, Environment.NewLine,
                        argument.DataType.Precision, Environment.NewLine,
                        argument.DataType.Scale, Environment.NewLine,
                        argument.Value.ToReport(), Environment.NewLine,
                        Mapping.SqlMapping[argument.DT].SqlByValue(argument.Value)),
                    Text.Method.Pass);
            }
        }

        private void CheckInnerCall()
        {
            // check if client passed variable by ref (not allowed)
            var passedByRef = Arguments.Where(argument => argument.IsOutput && !argument.IsPassedVariable).FirstOrDefault();
            if (passedByRef != null)
            {
                var paramName = passedByRef.ParamName;
                Throw(QueryTalkExceptionType.OutputVariableDisallowed,
                    ArgVal(() => paramName, paramName),
                    Text.Method.Pass);
            }

            // check bulk table: nested not allowed
            var bulkTable = Arguments.Where(argument => argument.IsBulk).FirstOrDefault();
            if (bulkTable != null)
            {
                Throw(QueryTalkExceptionType.NestedBulkInsertDisallowed,
                    String.Format("bulk table = {0}", bulkTable.ParamName), Text.Method.Pass);
            }
        }

        #endregion

        #region Build

        internal static string GetParamDeclaration(ParameterArgument argument, bool check, bool outer)
        {
            var dt = argument.DT;
            var dbt = argument.DataType;

            if (dt.IsNameType())
            {
                var sql = Text.GenerateSql(500)
                    .Append(argument.NameTypeSql);

                if (!outer && dt == DT.Udtt)
                {
                    sql.S().Append(Text.Readonly);
                }

                return sql.ToString();
            }

            if (dbt != null)
            {
                if (dbt.Precision != 0)
                {
                    if (check)
                    {
                        CriticalCheckArgumentSize(argument);
                    }

                    return String.Format("{0}({1},{2})", Mapping.SqlMapping[dt].Sql, dbt.Precision, dbt.Scale);
                }
                else if (dbt.Length != 0)
                {
                    if (check)
                    {
                        CriticalCheckArgumentSize(argument);
                    }

                    return String.Format("{0}({1})", Mapping.SqlMapping[dt].Sql, dbt.Length);
                }
                // if precision/size is not declared, infer it from the value
                else
                {
                    return Mapping.SqlMapping[dt].SqlByValue(argument.Value);
                }
            }
            else
            {
                return Mapping.SqlMapping[dt].SqlByValue(argument.Value);
            }
        }

        private string BuildStoredProc()
        {
            return Text.GenerateSql(1000)
                .NewLine(Text.Exec).S()
                .Append(Text.Reserved.ReturnValueOuterParam).Append(Text._Equal_)
                .Append(_body)
                .Append(BuildStoredProcArguments())
                .NewLine(Text.Set).S() 
                    .Append(Text.Reserved.ReturnValueInnerParam)
                    .Append(Text._Equal_)
                    .Append(Text.Reserved.ReturnValueOuterParam)
                .TerminateSingle()
                .ToString();
        }

        private void BuildTableValuedParams(StringBuilder sql)
        {
            foreach (var argument in _arguments
                .Where(argument =>
                    argument.DT.IsTable()           
                    && argument.DT.IsNotInliner()   
                    && !argument.IsPassedVariable     
                    && !argument.IsBulk              
                    && !argument.DT.IsTableVariable()   
                ).ToList())
            {
                if (argument.DT == DT.TempTable)
                {
                    sql.NewLine(Text.Select).S().Append(Text.Asterisk).S()
                        .Append(Text.Into).S().Append(argument.ParamName).S()
                        .NewLine(Text.From).S().Append(Text.LeftBracket).S()
                        .NewLine(argument.TestValue == null ?
                            argument.Sql                        // regular value
                                :
                            ((View)argument.TestValue).Sql)     // test value
                        .Append(Text.RightBracket).Append(Text._As_).Append(Text.DelimitedTargetAlias2)
                        .Terminate();
                }

                // UDT @table 
                else
                {
                    sql.NewLine(Text.Declare).S()
                        .Append(argument.ParamName).S()
                        .Append(Text.As).S()
                        .Append(argument.NameTypeSql)
                        .Terminate();

                    sql.NewLine(Text.Insert).S()
                        .Append(argument.ParamName).S()
                        .NewLine(argument.TestValue == null ?
                            argument.Sql                        // regular value
                                :
                            ((View)argument.TestValue).Sql)     // test value
                        .Terminate();
                }
            }
        }

        internal void BuildParamAppendix(BuildArgs buildArgs)
        {
            _paramDeclaration = new StringBuilder();
            _paramAssignment = new StringBuilder();

            if (Admin.IsValueParameterizationOn && !buildArgs.IsTesting)
            {
                _rootCompilable.ImplicitParams.ForEach(param =>
                {
                    _arguments.Add(param.ParameterizedValue);
                });
            }

            // return value
            _paramDeclaration.Append(Text.Reserved.ReturnValueInnerParam).S()
                .Append(Text.Free.EnclosedInt).S().Append(Text.Output);
            _paramAssignment.Append(Text.Reserved.ReturnValueInnerParam)
                .Append(Text._Equal_)
                .Append(Text.Reserved.ReturnValueOuterParam).S()
                .Append(Text.Output);

            // auto add arguments of optional params (if not passed)
            _rootCompilable.ExplicitParams.Where(param => param.IsOptional).ToList()
                .ForEach(param =>
                {
                    if (!CheckArgumentByName(param.Name))
                    {
                        _arguments.Add(param.Default);
                    }
                });

            // exclude inline params, table variable, temp tables and bulk tables
            bool paramSeparator = false;
            _arguments.Where(argument => argument.DT.IsNotInliner()
                && !argument.DT.IsVTB())  
                .ToList()
                .ForEach(argument =>
                {
                    // param declaration
                    _paramDeclaration.NewLine(Text.Comma);
                    _paramDeclaration
                        .Append(argument.ParamName)
                        .Append(Text._As_)
                        .Append(GetParamDeclaration(argument, true, false));

                    string declarationOutput = String.Empty;
                    if (argument.IsParamOutput)
                    {
                        declarationOutput = Text.OneSpace + Text.Output;
                    }
                    _paramDeclaration.Append(declarationOutput);

                    // marks the beginning of client params
                    if (!paramSeparator && argument.ParamName != Text.Reserved.ReturnValueInnerParam)
                    {
                        _paramAssignment.NewLine()
                            .NewLine(Text.Free.Params);
                        paramSeparator = true;
                    }

                    _paramAssignment.NewLine(Text.Comma);

                    // argument is a variable:
                    //   note: 
                    //     variables can only be passed in the inner call, otherwise they are treated as string values
                    if (argument.IsPassedVariable && _inner)
                    {
                        _paramAssignment.Append(argument.ParamName)
                            .Append(Text.Equal)
                            .Append(argument.Value);
                    }
                    // argument is a value:
                    else
                    {
                        _paramAssignment.Append(argument.ParamName)
                            .Append(Text.Equal);

                        if (argument.Original is View)
                        {
                            // note: inner table param has the same name as outer table param 
                            _paramAssignment.Append(argument.ParamName);
                        }
                        else if (argument.IsOutput)
                        {
                            _paramAssignment.Append(argument.ParamName)
                                .Append(Text.Underscore)
                                .Append(Text.Output);
                        }
                        else
                        {
                            // the test value has the advantage over the regular value
                            if (argument.TestValue != null)
                            {
                                Testing.AppendTestValue(_paramAssignment, argument);
                            }
                            else
                            {
                                if (argument.DataType != null)
                                {
                                    _paramAssignment.Append(Mapping.Build(argument.Value, argument.DataType));
                                }
                                else
                                {
                                    _paramAssignment.Append(Mapping.BuildUnchecked(argument.Value)); 
                                }
                            }
                        }
                    }

                    string assignmentOutput = String.Empty;
                    if (argument.IsOutput)
                    {
                        assignmentOutput = Text.OneSpace + Text.Output;
                    }

                    _paramAssignment.Append(assignmentOutput);
                });

            if (chainException != null)
            {
                TryThrow(Text.Method.Pass);
            }
        }

        private string BuildStoredProcArguments()
        {
            StringBuilder argumentBuilder = new StringBuilder();

            int i = 0;
            var root = _compilable.GetRoot();
            root.AllParams
                .ForEach(param =>
                {
                    if (i++ == 0)
                    {
                        argumentBuilder.Append(Text.OneSpace); 
                    }
                    else
                    {
                        argumentBuilder.Append(Text.Comma);  
                    }

                    if (Admin.IsValueParameterizationOn)
                    {
                        argumentBuilder.Append(param.Name);
                    }
                    else  // (unreachable code)
                    {
#pragma warning disable CS0162 // Unreachable code detected
                        if (!_inner)
#pragma warning restore CS0162 // Unreachable code detected
                        {
                            argumentBuilder.Append(param.Name);
                        }
                        else
                        {
                            object directArgument = Arguments
                                .Where(a => a.ParamName == param.Name)
                                .Select(a => a.Value)
                                .FirstOrDefault();

                            Type directArgumentType = directArgument == null ? null : directArgument.GetType();
                            if (Variable.Detect(directArgument))
                            {
                                argumentBuilder.Append((string)directArgument);
                            }
                            else
                            {
                                if (directArgument != null
                                    && (directArgumentType == typeof(View) || (directArgumentType == typeof(DataTable))))
                                {
                                    argumentBuilder.Append(param.Name);
                                }
                                else
                                {
                                    argumentBuilder.Append(Mapping.BuildUnchecked(directArgument));
                                }
                            }
                        }
                    }

                    if (param.IsOutput)
                    {
                        argumentBuilder.S().Append(Text.Output);
                    }
                });

            argumentBuilder.Terminate();

            if (chainException != null)
            {
                TryThrow(Text.Method.Pass);
            }

            return argumentBuilder.ToString();
        }

        private void BuildBody(StringBuilder sql, string bodyWrappedAndEscaped)
        {
            sql.NewLine(Text.Free.ExecSpExecutesql_N)
                .NewLine(Text.Declare).S().Append(Text.Reserved.ReturnValueOuterParam).Append(Text._As_)
                    .Append(Text.Free.EnclosedInt).Terminate()
                .NewLine(Text.Declare).S().Append(Text.Reserved.ConcatVar).Append(Text._As_)
                    .Append(Text.Free.EnclosedNVarcharMax).Terminate();

            if (!Compilable.GetRoot().IsEmbeddedTransaction)
            {
                sql.NewLine();
            }
                
            sql
                .Append(bodyWrappedAndEscaped)
                .Append(TerminateBody(bodyWrappedAndEscaped));

            if (!Compilable.GetRoot().IsEmbeddedTransaction)
            {
                sql.NewLine();
            }

            sql
                .NewLine(Text.SingleQuote)
                .Append(Text.Free.CommaNSingleQuote)
                .Append(_paramDeclaration)
                .Append(Text.SingleQuote)
                .NewLine(Text.Comma)
                .Append(_paramAssignment)
                .TerminateSingle()
                .NewLine();
        }

        // last statement should be always terminated
        private string TerminateBody(string body)
        {
            if (body != null 
                && body.Length > 0                     
                && !(Compilable.LastNode is CommentChainer)   
                )
            {
                if (body[body.Length - 1] != Text.TerminatorChar)
                {
                    return Text.Terminator;
                }
            }

            return String.Empty;
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return String.Format("Executable ({0})", Compilable.Name);
        }

        #endregion

    }
}