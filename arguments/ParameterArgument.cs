#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents a parameter of a parameterized query or procedure.
    /// </summary>
    public sealed class ParameterArgument : Argument
    {

        #region Properties

        internal string ParamName { get; private set; }

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

        internal DataType DataType { get; set; }

        internal string NameTypeSql { get; private set; }

        internal bool IsBulk
        {
            get
            {
                return DT == Wall.DT.BulkTable;
            }
        }

        internal object Value { get; private set; }

        // param was declared as output
        internal bool IsParamOutput { get; set; }

        // argument is given as output
        internal bool IsArgumentOutput { get; set; }

        // if a variable name is passed it is stored into this field
        internal bool IsPassedVariable { get; private set; }

        // value converted into SQL code 
        internal string Sql { get; private set; }

        // If true then the argument should hold the returning output value.
        internal bool IsOutput
        {
            get
            {
                return IsParamOutput && IsArgumentOutput;
            }
        }

        // for Testing Environment only (scalar or table-valued argument) 
        //   note: The testing parameters cannot affect the execution parameters.
        internal object TestValue { get; set; }

        // true if a value has been parameterized 
        internal bool IsParameterized { get; set; }

        // sets output value (in case that the argument was passed as Value object)
        //   anticipated: argument is supposed to be Value type
        internal void SetOutput(object output)
        {
            ((Value)Original).SetValue(output);
        }

        // returns OUTPUT arguments
        internal static ParameterArgument[] GetOutputArguments(IEnumerable<ParameterArgument> arguments)
        {
            return arguments
                .Where(argument => argument.DT.IsNotInliner()
                    && !argument.IsPassedVariable && argument.IsOutput)
                .ToArray();
        }

        #endregion

        #region Constructors

        // Main constructor that all other CLR constructors call. When an argument of a certain value CLR type is passed to this constructor, 
        // the boxing occurs. We choose this approach due to its simplicity.
        private ParameterArgument(object arg)
            : base(arg)
        {
            Value = arg;

            Build = (buildContext, buildArgs) =>
            {
                if (Sql == null)
                {
                    if (arg is System.String && DT.IsInliner())
                    {
                        return Filter.DelimitColumnMultiPart((string)arg, out chainException);
                    }

                    return Mapping.BuildUnchecked(arg, out chainException);
                }
                else
                {
                    return Sql;
                }
            };
        }

        private ParameterArgument(Chainer arg)
            : base(arg)
        {
            Value = arg;

            TryTakeAll(arg);
        }

        #region System.String

        // store original value (to avoid later unneccassary conversions)
        private string _string;

        internal ParameterArgument(System.String arg)
            : this(arg as object)
        {
            ArgType = typeof(System.String);
            Value arg2 = null;

            if (Common.CheckIdentifier(arg) == IdentifierValidity.Variable)
            {
                IsPassedVariable = true;
            }
            else
            {
                arg2 = new Value(arg);
            }

            Build = (buildContext, buildArgs) =>
            {
                if (IsPassedVariable)
                {
                    return arg;
                }
                else
                {
                    return arg2.Build(buildContext, buildArgs);
                }
            };

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(System.String arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region System.Boolean

        internal ParameterArgument(System.Boolean arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(System.Boolean arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region System.Byte

        internal ParameterArgument(System.Byte arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(System.Byte arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region System.Byte[]

        internal ParameterArgument(System.Byte[] arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(System.Byte[] arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region System.DateTime

        internal ParameterArgument(System.DateTime arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(System.DateTime arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region System.DateTimeOffset

        internal ParameterArgument(System.DateTimeOffset arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(System.DateTimeOffset arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region System.Decimal

        internal ParameterArgument(System.Decimal arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(System.Decimal arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region System.Double

        internal ParameterArgument(System.Double arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(System.Double arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region System.Guid

        internal ParameterArgument(Guid arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(System.Guid arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region System.Int16

        internal ParameterArgument(System.Int16 arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(System.Int16 arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region System.Int32

        internal ParameterArgument(System.Int32 arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(System.Int32 arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region System.Int64

        internal ParameterArgument(System.Int64 arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(System.Int64 arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region System.Single

        internal ParameterArgument(System.Single arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(System.Single arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region System.TimeSpan

        internal ParameterArgument(System.TimeSpan arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(System.TimeSpan arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region Value

        internal ParameterArgument(Value arg)
            : this(arg as Chainer)
        {
            if (arg.IsNullReference())
            {
                arg = Designer.Null;
            }

            SetArgType(arg);
            ArgType = arg.ClrType; 

            Value = arg.Original;
            _string = arg.ToString();

            if (arg.IsOutput)
            {
                IsArgumentOutput = true;  
            }

            if (arg.DbT != null)
            {
                DataType = arg.DbT;
            }
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(Value arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region Identifier
        // used for inlining: to pass inline table

        internal ParameterArgument(Identifier arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(Identifier arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region Column
        // used for inlining: to pass inline column

        internal ParameterArgument(Column arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(Column arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region Column (Collection)
        // used for inlining: to pass inline columns

        internal ParameterArgument(Column[] arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(Column[] arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region String (Collection)
        // used for inlining: to pass inline columns (as identifiers ONLY, not values)

        internal ParameterArgument(System.String[] arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(System.String[] arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region Expression
        // used for inlining: to pass inline expression

        internal ParameterArgument(Expression arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(Expression arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region Procedure
        // used for inlining: to pass a procedure

        internal ParameterArgument(Procedure arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(Procedure arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region Snippet
        // used for inlining: to pass a snippet

        internal ParameterArgument(Snippet arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(Snippet arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region View
        // used for table-valued arguments

        internal ParameterArgument(View arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
            _dt = DT.View;    
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(View arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region DataTable
        // used as data source for bulk insert

        internal ParameterArgument(DataTable arg)
            : this(arg as object)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(DataTable arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region Variable (@Var)
        // used only for variables passed in the SQL Server
        // (Note: isvariable arg is not used, for method overloading only)

        internal ParameterArgument(string variable, bool isvariable)
            : base(null)
        {
            // We anticipate that paramName & variable check has already been done and that both args are valid.
            Value = variable;
            IsPassedVariable = true;
        }

        #endregion

        #region Variable (@Var OUTPUT)
        // used in .Exec/.Pass method when passing an OUTPUT variable

        internal ParameterArgument(OutputVar arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
            IsPassedVariable = true;
            IsArgumentOutput = true;
            Value = arg.Build(null, null);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(OutputVar arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region Pass
        // used for inlining: to pass a procedure

        internal ParameterArgument(PassChainer arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(PassChainer arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #region ExecArg
        // used for inlining: to pass a procedure

        internal ParameterArgument(ExecArgument arg)
            : this(arg as Chainer)
        {
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ParameterArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ParameterArgument(ExecArgument arg)
        {
            return new ParameterArgument(arg);
        }

        #endregion

        #endregion

        #region Check

        // Check argument type and prepare exception object if the check fails.
        internal bool CheckType(Variable param, object value, out QueryTalkException exception)
        {
            exception = null;
            
            #region Table-value param

            if (DT.IsTable())
            {
                // check null: #temp table & bulk table are allowed to be passed a null value (in the inner call)
                // Note: table variable param cannot be used in inner calls => raise an exception
                if (QueryTalk.Value.IsNull(value))
                {
                    throw new QueryTalkException("Param.ArgumentCheck",
                        QueryTalkExceptionType.ParamArgumentNull,
                        String.Format("param = {0}", ParamName),
                        Text.Method.Pass);
                }

                // check type match
                if (!Mapping.TableParamMapping[DT].Contains(ArgType))
                {
                    exception = new QueryTalkException("Param.ArgumentCheck",
                        QueryTalkExceptionType.ParamArgumentTypeMismatch,
                        String.Format(
                            "param = {0}{1}   argument type = {2}{3}   required type(s) = {4}",
                            ParamName, Environment.NewLine,
                            ArgType, Environment.NewLine,
                            String.Join(", ", Mapping.TableParamMapping[DT]
                                .Select(a => a.FullName).ToArray())),
                            Text.Method.Pass);
                    return false;
                }

                // check View: should store data values only. 
                if (ArgType == typeof(View))
                {
                    // if a view does not origin from a CLR collection
                    if (!((View)value).IsValidDataView)
                    {
                        exception = new QueryTalkException(
                            "Param.ArgumentCheck",
                            QueryTalkExceptionType.InvalidDataView,
                            String.Format("param = {0}", ParamName));
                        return false;
                    }
                    return true;
                }
            }

            #endregion

            // it's ok if value is null
            if (QueryTalk.Value.IsNull(value))
            {
                return true;
            }

            // type check
            if (!Mapping.CheckArgument(param, ArgType, out exception))
            {
                if (DT.IsInliner())
                {
                    exception.Arguments = String.Format(
                        "param = {0}{1}   param type = {2}{3}   argument = {4}{5}   argument type = {6}{7}   required type(s) = {8}",
                            param.Name, Environment.NewLine,
                            Text.Free.InlinerType + "." + param.DT.ToInliner(), Environment.NewLine,
                            value.ToReport(), Environment.NewLine,
                            ArgType, Environment.NewLine,
                            String.Join(", ", Mapping.InlineMapping[param.DT]
                                .Select(a => a.FullName).ToArray()));
                }
                else
                {
                    exception.Arguments = String.Format(
                        "param = {0}{1}   param type = {2}{3}   argument = {4}{5}   argument type = {6}{7}   required type = {8}",
                            param.Name, Environment.NewLine,
                            param.DT.ToString().ToLower(), Environment.NewLine,
                            value.ToReport(), Environment.NewLine,
                            ArgType, Environment.NewLine,
                            Mapping.SqlMapping[param.DT].ClrType
                            );
                }
                return false;
            }
            return true;
        }

        #endregion

        #region Build

        // Builds argument with critical check if the argument is bound to a certain param.
        internal void BuildArgument(Designer root, string paramName)
        {
            // check if a param with the given param name exists
            Variable param = root.TryGetVariable(paramName, out chainException, Variable.SearchType.Param);

            if (chainException != null)
            {
                chainException.Method = Text.Method.Pass;
                return;
            }

            // check: argument is null/NULL 
            if (QueryTalk.Value.IsNull(Value))
            {
                if (IsPassedVariable || !param.ParamNullCheck(out chainException))
                {
                    chainException.Method = Text.Method.Pass;
                    chainException.Arguments = String.Format("param = {0}", paramName);
                    return;
                }
            }

            // store param data
            ParamName = paramName;
            _dt = param.DT;
            DataType = param.DataType;
            IsParamOutput = param.IsOutput;
            IsParameterized = param.IsParameterized;

            // clear parameterized value on param
            if (IsParameterized)
            {
                param.ClearParameterizedValue();
            }

            // udt: store compiled SQL
            if (param.DT.IsNameType())
            {
                NameTypeSql = param.NameType.Sql;
            }

            // table variable/temp table/bulk table: cannot be passed as variable
            if (DT.IsVTB())
            {
                // bulk table
                if (DT == Wall.DT.BulkTable)
                {
                    if (Value.GetType() != typeof(DataTable))
                    {
                        chainException = new QueryTalkException("Param.BuildArgument", QueryTalkExceptionType.InvalidTableArgument,
                            String.Format(
                                "bulk table = {0}{1}   argument type = {2}{3}   required type = {4}",
                                ParamName, Environment.NewLine, ArgType, Environment.NewLine, typeof(DataTable)),
                                Text.Method.Pass);
                        return;
                    }
                    else
                    {
                        return;
                    }
                }

                // table variable/temp table
                if (Value.GetType() != typeof(View))
                {
                    chainException = new QueryTalkException("Param.BuildArgument", QueryTalkExceptionType.InvalidTableArgument,
                        String.Format(
                            "param = {0}{1}   argument type = {2}{3}   required type = {4}",
                            ParamName, Environment.NewLine, ArgType, Environment.NewLine, typeof(View)),
                            Text.Method.Pass);
                    return;
                }
            }

            // finish if argument has been passed as variable
            if (IsPassedVariable)
            {
                Sql = Original.ToString();  
                return;
            }
            
            // argument type check
            if (!CheckType(param, Value, out chainException))
            {
                chainException.Method = Text.Method.Pass;
                return;
            }

            // Here the check has passed. Argument is properly bound to its param.
            // Now build the SQL output.

            // value
            if (DT.IsDataType() || DT.IsNameType())
            {
                if (ArgType == typeof(View))
                {
                    Sql = ((View)Original).Sql;
                }
                else if (ArgType == typeof(DataTable))
                {
                    Sql = null;   
                }
                else if (ArgType == typeof(OutputVar))
                {
                    Sql = ((OutputVar)Original).Value;
                }
                // common type
                else
                {
                    Sql = Mapping.BuildUnchecked(Value, out chainException);
                }
            }
            // inline
            else
            {
                // table
                if (DT == DT.InTable)
                {
                    if (IsPassedVariable)
                    {
                        Sql = (string)Value;    
                    }
                    // Identifier (build by Build method)
                    else { }
                }
                // sql
                else if (DT == DT.InSql)
                {
                    if (IsPassedVariable)
                    {
                        Sql = (string)Value;  
                    }
                    else
                    {
                        Sql = _string;    
                    }
                }
                // other inline types must be built by the Build method
                else
                { }
            }
        }

        #endregion

    }
}
