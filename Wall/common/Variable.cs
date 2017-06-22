#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Text.RegularExpressions;

namespace QueryTalk.Wall
{
    // Represents a SQL parameter or variable.
    internal sealed class Variable
    {
        // search types that are used by .TryGetVariable method
        internal enum SearchType 
        { 
            Any,            // any type of variable or null
            Param,          // parameter
            Variable,       // variable
            Inliner,        // inliner
            Concat,         // concatenator
            InlinerConcat   // inliner + concat   
        }

        // data type with low precedence (used when inferring patam from null value)
        internal static readonly DataType DbInferenceBaseType = DT.VarcharMax.ToDataType();

        #region Properties

        private DT _dt = DT.None;  
        internal DT DT 
        {
            get
            {
                if (DataType != null)
                {
                    return DataType.DT;
                }
                else
                {
                    return _dt;
                }
            } 
        }

        internal DataType DataType { get; private set; }

        // { Param, SqlVariable }
        internal IdentifierType VariableType { get; private set; }

        internal string Name { get; private set; }

        // { udt, udtt, table variable, temp table, bulk table }
        private TableArgument _nameType;
        internal TableArgument NameType 
        {
            get
            {
                if (_nameType == null 
                    && DataType != null
                    && DataType.DT.IsNameType())
                {
                    _nameType = DataType.Udt;
                }
                return _nameType;
            }
        }

        // default value of the param used if the argument is not passed
        internal ParameterArgument Default { get; set; }

        internal bool IsOptional { get; set; }

        private bool _isnullable = true;
        internal bool IsNullable
        {
            get
            {
                return _isnullable;
            }
            set
            {
                _isnullable = value;
            }
        }

        internal bool IsOutput { get; set; }

        // ordinal of the param in the param sequence
        internal long Ordinal { get; private set; }

        private QueryTalkException _exception;
        internal QueryTalkException Exception 
        {
            get
            {
                return _exception;
            }
        }

        #region Parameterized

        private ParameterArgument _parameterizedValue;
        internal ParameterArgument ParameterizedValue 
        {
            get
            {
                return _parameterizedValue;
            }
        }

        internal void SetParameterizedValue(ParameterArgument value)
        {
            _parameterizedValue = value;
        }

        internal void ClearParameterizedValue()
        {
            _parameterizedValue = null;
        }

        internal bool IsParameterized
        {
            get
            {
                return ParameterizedValue != null;
            }
        }

        #endregion

        #endregion

        #region Constructors
        
        private Variable(long ordinal, string name, IdentifierType variableType)
        {
            VariableType = variableType;
            Ordinal = ordinal;
            Name = name;
        }

        internal Variable(long ordinal, string name, DT dt, IdentifierType variableType)
            : this(ordinal, name, variableType)
        {
            _dt = dt;
        }

        internal Variable(long ordinal, string name, DataType dataType, IdentifierType variableType)
            : this(ordinal, name, variableType)
        {
            DataType = dataType;
        }

        internal Variable(long ordinal, string name, Type type, IdentifierType variableType)
            : this(ordinal, name, variableType)
        {
            Type clrType;
            var typeMatch = Mapping.CheckClrCompliance(type, out clrType, out _exception);
            if (typeMatch != Mapping.ClrTypeMatch.ClrMatch)
            {
                return;
            }

            DataType = Mapping.ClrMapping[clrType].DefaultDataType;
        }

        internal Variable(long ordinal, string name, DT dt, TableArgument nameType, IdentifierType variableType) 
            : this(ordinal, name, variableType)   
        {
            if (nameType.IsUndefined())
            {
                _exception = new QueryTalkException(this, QueryTalkExceptionType.ArgumentNull,
                    "userDefinedType = undefined", Text.Method.Param);
                return;
            }

            if (dt == DT.View && nameType != null)
            {
                dt = Wall.DT.Udtt;
            }

            _dt = dt;
            Ordinal = ordinal;
            Name = name;
            _nameType = nameType;
            _exception = nameType.Exception;
        }

        internal Variable(DataType dataType, string name = null)
        {
            DataType = dataType;
            Name = name;
            VariableType = IdentifierType.Param;
        }

        #endregion

        #region Methods

        internal void SetNameByOrdinal(long ordinal)
        {
            Name = String.Format("@{0}", ordinal);
        }

        // infer param from param argument 
        internal static Variable InferParam(Designer root, ParameterArgument argument, out QueryTalkException exception, string name = null)
        {
            exception = null;
            Variable param;
            long variableIndex = 0; 

            if (argument.DT.IsDataType())
            {
                param = new Variable(argument.DataType);
            }
            else
            {
                if (argument.DT.IsTable())
                {
                    var view = (View)argument.Original;
                    if (view.UserDefinedType == null)
                    {
                        exception = new QueryTalkException("Variable.InferParam", QueryTalkExceptionType.MissingDataViewUdt, null);
                        return null;
                    }

                    var udt = view.UserDefinedType;
                    variableIndex = Designer.GetVariableGuid();  // important!
                    param = new Variable(variableIndex, name, argument.DT, udt, IdentifierType.Param);
                }
                else
                {
                    var dataType = DbInferenceBaseType;   

                    if (!argument.IsUndefined() && argument.ArgType != null)
                    {
                        // special handling of a variable passed by .Output 
                        if (argument.IsArgumentOutput)
                        {
                            dataType = DT.VarcharMax.ToDataType();
                        }
                        // other data type
                        else
                        {
                            dataType = Mapping.ClrMapping[argument.ArgType].DefaultDataType;
                        }
                    }
                    param = new Variable(dataType, name);
                    argument.DataType = dataType;
                }
            }

            if (name == null)
            {
                if (variableIndex == 0)
                {
                    variableIndex = root.GetVariableIndex();    // cannot be mistaken with guid method
                }
                param.SetNameByOrdinal(variableIndex);
            }

            param.IsOutput = argument.IsArgumentOutput;
            return param;
        }

        // checks if param allows null argument
        internal bool ParamNullCheck(out QueryTalkException exception)
        {
            exception = null;

            if (DT.IsTable() || !IsNullable)
            {
                exception = new QueryTalkException("Variable.NullCheck", QueryTalkExceptionType.ParamArgumentNull, null);
                return false;
            }

            return true;
        }

        // Builds param declaration for data type and name type.
        internal string BuildDeclaration()
        {
            if (DT.IsDataType())
            {
                return DataType.Build();
            }
            else if (DT.IsNameType())
            {
                var sql = NameType.Sql;
                    
                // user-defined table type (add READONLY)
                if (DT == Wall.DT.Udtt)
                {
                    sql += String.Format(" {0}", Text.Readonly);
                }

                return sql;
            }

            return null;
        }

        internal static string ProcessVariable(
            string argument,      
            BuildContext buildContext,
            BuildArgs buildArgs,
            out QueryTalkException exception)
        {
            exception = null;

            var variable = buildContext.TryGetVariable(argument, out exception);
            if (exception != null)
            {
                return null;
            }

            if (variable == null)
            {
                if (Variable.Detect(argument))
                {
                    if (buildContext.Root.CompilableType.IsProc()
                        || (buildContext.Root.CompilableType == Compilable.ObjectType.View
                            && buildContext.Query.Master.Root.CompilableType.IsProc()))
                    {
                        buildContext.Exception = new QueryTalkException("Variable.ProcessVariable",
                            QueryTalkExceptionType.ParamOrVariableNotDeclared,
                            String.Format("variable = {0}", argument));
                        return null;
                    }
                }

                return null;   
            }

            buildContext.TryTakeException(variable.DisallowedInliningException());
            if (buildContext.Exception != null)
            {
                return null;
            }

            if (variable.IsConcatenator())
            {
                return Argument.BuildConcatenator(buildContext, buildArgs, variable);
            }

            buildContext.TryAddParamToConcatRoot(variable);

            return null;
        }

        #endregion

        #region Exceptions

        internal QueryTalkException DisallowedInliningException()
        {
            if (DT.IsInliner())
            {
                return new QueryTalkException("Variable.DisallowedInliningException",
                    QueryTalkExceptionType.InvalidInliner,
                        String.Format("inliner = {0}", Name));
            }

            return null;
        }

        internal static QueryTalkException InvalidVariableException(string variable, QueryTalkExceptionType exceptionType)
        {
            return new QueryTalkException("Variable.InvalidVariableTypeException",
                exceptionType, String.Format("variable = {0}", variable));
        }

        #endregion

        #region Check methods

        // returns true if the first character of an identifier is the at sign (@)
        internal static bool Detect(string identifier)
        {
            if (identifier == null || identifier.Length == 0 || identifier.Length > 128)
            {
                return false;
            }

            return identifier.Substring(0, 1) == "@";
        }

        internal static bool Detect(object value)
        {
            if (value == null || !(value is System.String))
            {
                return false;
            }

            return Detect((string)value);
        }

        // checks if the string has a valid variable name
        internal static bool TryValidateName(string name, out QueryTalkException exception)
        {
            exception = null;

            if (!Detect(name))
            {
                return false;
            }

            if (Common.CheckIdentifier(name) != IdentifierValidity.Variable)
            {
                exception = new QueryTalkException("Variable.TryValidateName",
                    QueryTalkExceptionType.InvalidVariableName,
                    String.Format("param/variable = {0}", name));
                exception.Extra = "If it was not intended to use a variable but rather a string, then append the .C method to the string.";
                return false;
            }

            return Common.CheckReservedName("Variable.TryValidateName", name, out exception);
        }

        internal static bool CheckNameFormat(string variable, out QueryTalkException exception)
        {
            exception = null;
            if (Regex.IsMatch(variable, @"^@\d+$"))
            {
                exception = new QueryTalkException("Variable.TryValidateName",
                    QueryTalkExceptionType.InvalidVariableName,
                    String.Format("param/variable = {0}", variable));
                return false;
            }

            return true;
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("{0} ({1})", Name, DT);
        }

        #endregion

    }
}
