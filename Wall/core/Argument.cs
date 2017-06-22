#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Data;
using System.Diagnostics;

namespace QueryTalk.Wall
{
    // Base argument class.
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    [DebuggerTypeProxy(typeof(Wall.ArgumentDebuggerProxy))]
    
    public abstract class Argument : Chainer, INonPredecessor
    {
        /// <summary>
        /// Is a type of an argument.
        /// </summary>
        protected internal Type ArgType { get; set; }

        internal Argument(object arg)
            : base(null)
        {
            if (arg == null 
                && !(this is Value)         // skip Designer.Null to avoid infinite loop
                && !(this is Table)         // no correction of table arguments
                && !(this is ExecArgument)  // no correction of exec argument
                )
            {
                var nullArgument = Designer.Null;   
                Build = nullArgument.Build;        
                Original = nullArgument;    // assing RootObject.Null reference (keep this line AFTER the build method assignment!)
                return;
            }

            if (arg != null)
            {
                ArgType = arg.GetType();
            }

            Original = arg;

            if (ArgType == typeof(Value))
            {
                SetDebugValue(((Value)arg).ToString());
            }
        }

        #region Name

        internal void CtorBody(Identifier arg)
        {
            TryTakeAll(arg);
            if (CheckNull(Arg(() => arg, arg)))
            {
                if (arg.NumberOfParts > 2)
                {
                    chainException = new QueryTalkException(this,
                        QueryTalkExceptionType.InvalidColumnIdentifier,
                        String.Format("identifier = {0}", arg.ToString()),
                        Text.Method.Identifier);
                }
            }
            else
            {
                chainException.Extra = Text.Free.NameNullExtra;
            }
        }

        #endregion

        #region BuildClr (excluding System.String)
        // All CLR arguments (excluding System.String) are finally handled here.

        internal static string BuildClr(System.Boolean arg, BuildContext buildContext)
        {
            return arg.Parameterize(buildContext) ?? Mapping.BuildCast(arg);
        }

        internal static string BuildClr(System.Byte arg, BuildContext buildContext)
        {
            return arg.Parameterize(buildContext) ?? Mapping.BuildCast(arg);
        }

        internal static string BuildClr(System.Byte[] arg, BuildContext buildContext)
        {
            return arg.Parameterize(buildContext) ?? Mapping.BuildCast(arg);
        }

        internal static string BuildClr(System.DateTime arg, BuildContext buildContext)
        {
            var sql = arg.Parameterize(buildContext);   
            if (sql == null)
            {
                sql = Mapping.BuildCast(arg);
            }
            return sql;
        }

        internal static string BuildClr(System.DateTimeOffset arg, BuildContext buildContext)
        {
            return arg.Parameterize(buildContext) ?? Mapping.BuildCast(arg);
        }

        internal static string BuildClr(System.Decimal arg, BuildContext buildContext)
        {
            return arg.Parameterize(buildContext) ?? Mapping.BuildCast(arg);
        }

        internal static string BuildClr(System.Double arg, BuildContext buildContext)
        {
            return arg.Parameterize(buildContext) ?? Mapping.BuildCast(arg);
        }

        internal static string BuildClr(System.Guid arg, BuildContext buildContext)
        {
            return arg.Parameterize(buildContext) ?? Mapping.BuildCast(arg);
        }

        internal static string BuildClr(System.Int16 arg, BuildContext buildContext)
        {
            return arg.Parameterize(buildContext) ?? Mapping.BuildCast(arg);
        }

        internal static string BuildClr(System.Int32 arg, BuildContext buildContext)
        {
            return arg.Parameterize(buildContext) ?? Mapping.BuildCast(arg);
        }

        internal static string BuildClr(System.Int64 arg, BuildContext buildContext)
        {
            return arg.Parameterize(buildContext) ?? Mapping.BuildCast(arg);
        }

        internal static string BuildClr(System.Single arg, BuildContext buildContext)
        {
            return arg.Parameterize(buildContext) ?? Mapping.BuildCast(arg);
        }

        internal static string BuildClr(System.TimeSpan arg, BuildContext buildContext)
        {
            return arg.Parameterize(buildContext) ?? Mapping.BuildCast(arg);
        }

        internal static string BuildClr(string arg, BuildContext buildContext)
        {
            return arg.Parameterize(buildContext, Mapping.DefaultStringType) ?? Mapping.Build(arg, Mapping.DefaultStringType);
        }

        #endregion

        #region Process Variable

        internal Variable TryGetVariable(BuildContext buildContext)
        {
            QueryTalkException exception;

            if (buildContext == null)
            {
                return null;
            }

            return TryGetVariable(buildContext.ParamRoot, out exception);
        }

        internal Variable TryGetVariable(Designer root, out QueryTalkException exception)
        {
            exception = null;

            if (root == null)
            {
                return null;
            }

            string variableName = null;
            if (Original != null)
            {
                if (Original is System.String)
                {
                    variableName = (string)Original;
                }
                else if (Original is ColumnAsChainer)
                {
                    variableName = ((IColumnName)Original).ColumnName;
                }
            }

            if (variableName == null)
            {
                return null;
            }

            return root.TryGetVariable(variableName, out exception);
        }

        // main method for non-inliner variable processing
        internal bool ProcessVariable(
            BuildContext buildContext,
            BuildArgs buildArgs,
            out string sql)
        {
            var variable = TryGetVariable(buildContext);
            return ProcessVariable(buildContext, buildArgs, out sql, variable);
        }

        internal bool ProcessVariable(
            BuildContext buildContext,
            BuildArgs buildArgs,
            out string sql,
            Variable variable)
        {
            sql = null;

            if (variable == null)
            {
                if (Variable.Detect(Original))
                {
                    if (buildContext.Root.CompilableType.IsProcOrSnippet()
                        || (buildContext.Root.CompilableType == Compilable.ObjectType.View
                            && buildContext.Query.Master.Root.CompilableType.IsProc()))
                    {
                        buildContext.Exception = new QueryTalkException("Argument.ProcessVariable",
                            QueryTalkExceptionType.ParamOrVariableNotDeclared,
                            String.Format("variable = {0}", Original));
                        return true;
                    }

                    // non-declared variables in views are valid:
                    if (buildContext.Root.CompilableType == Compilable.ObjectType.View)
                    {
                        sql = (string)Original;
                        return true;
                    }
                }
                return false;  
            }

            // here variable has been found:

            buildContext.TryTakeException(variable.DisallowedInliningException());
            if (buildContext.Exception != null)
            {
                return true;
            }

            if (variable.IsConcatenator())
            {
                sql = BuildConcatenator(buildContext, buildArgs, variable);
                return true;
            }

            buildContext.TryAddParamToConcatRoot(variable);

            return false;   
        }

        #endregion

        #region Concatenation

        internal static string BuildConcatenator(BuildContext buildContext, BuildArgs buildArgs, Variable variable)
        {
            if (buildContext.Current.IsQuery)
            {
                // not allowed in writable methods where direct values are used
                if (buildContext.Current is IWritable)
                {
                    buildContext.Current.Throw(QueryTalkExceptionType.ConcatenatorDisallowed,
                        String.Format("concatenator = {0}", variable.Name), Text.Method.Collect);
                    return null;
                }

                var concatenator = new Concatenator(variable);
                return concatenator.Build(buildContext, buildArgs);
            }
            else
            {
                buildContext.Current.Throw(QueryTalkExceptionType.ConcatenatorDisallowed,
                    String.Format("concatenator = {0}", variable.Name));
                return null;
            }
        }

        #endregion

        #region SetArgType

        private enum DebugSetter { Arg, ToString, None }

        // sets the debugging value using .ToString method (null safe)
        private void SetDebugValue(object arg,
            DebugSetter debugSetter = DebugSetter.ToString
            )
        {
            if (arg != null)
            {
                if (debugSetter == DebugSetter.ToString)
                {
                    DebugValue = arg.ToString();
                }
                else if (debugSetter == DebugSetter.Arg)
                {
                    DebugValue = arg;
                }
            }
            else
            {
                DebugValue = null;
            }
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(Identifier arg)
        {
            ArgType = typeof(Identifier);
            SetDebugValue(arg);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(Value arg)
        {
            ArgType = typeof(Value);
            SetDebugValue(arg);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(SysFn arg)
        {
            ArgType = typeof(SysFn);
            SetDebugValue(arg);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(CaseExpressionChainer arg)
        {
            ArgType = typeof(CaseExpressionChainer);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(OfChainer arg)
        {
            ArgType = typeof(OfChainer);
            SetDebugValue(arg);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(Udf arg)
        {
            ArgType = typeof(Udf);
            SetDebugValue(arg, DebugSetter.None);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(Expression arg)
        {
            ArgType = typeof(Expression);
            SetDebugValue(arg, DebugSetter.None);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(DbColumn arg)
        {
            ArgType = typeof(DbColumn);
            SetDebugValue(arg);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(DbColumns arg)
        {
            ArgType = typeof(DbColumns);
            SetDebugValue(arg);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(OrderedChainer arg)
        {
            ArgType = typeof(OrderedChainer);
            SetDebugValue(arg, DebugSetter.None);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(Column arg)
        {
            ArgType = typeof(Column);
            SetDebugValue(arg, DebugSetter.None);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(Column[] arg)
        {
            ArgType = typeof(Column[]);
            SetDebugValue(arg, DebugSetter.None);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(System.String[] arg)
        {
            ArgType = typeof(System.String[]);
            SetDebugValue(arg, DebugSetter.None);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(View arg)
        {
            ArgType = typeof(View);
            SetDebugValue(arg, DebugSetter.Arg);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(Procedure arg)
        {
            ArgType = typeof(Procedure);
            SetDebugValue(arg, DebugSetter.Arg);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(Snippet arg)
        {
            ArgType = typeof(Snippet);
            SetDebugValue(arg, DebugSetter.None);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(DataTable arg)
        {
            ArgType = typeof(DataTable);
            SetDebugValue(arg, DebugSetter.Arg);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(OutputVar arg)
        {
            ArgType = typeof(OutputVar);
            SetDebugValue(arg, DebugSetter.None);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(ScalarArgument arg)
        {
            ArgType = typeof(ScalarArgument);
            SetDebugValue(arg, DebugSetter.Arg);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(ValueScalarArgument arg)
        {
            ArgType = typeof(ValueScalarArgument);
            SetDebugValue(arg, DebugSetter.Arg);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(DbNode arg)
        {
            ArgType = typeof(DbNode);
            SetDebugValue(arg, DebugSetter.Arg);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(CollateChainer arg)
        {
            ArgType = typeof(CollateChainer);
            SetDebugValue(arg, DebugSetter.None);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(ColumnAsChainer arg)
        {
            ArgType = typeof(ColumnAsChainer);
            SetDebugValue(arg, DebugSetter.None);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(Concatenator arg)
        {
            ArgType = typeof(Concatenator);
            SetDebugValue(arg, DebugSetter.None);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(PassChainer arg)
        {
            ArgType = typeof(PassChainer);
            SetDebugValue(arg, DebugSetter.Arg);
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(ExecArgument arg)
        {
            ArgType = typeof(ExecArgument);
            SetDebugValue(arg, DebugSetter.Arg);
        }

        #region CLR

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(System.String arg)
        {
            ArgType = typeof(System.String);
            DebugValue = arg;
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(System.Boolean arg)
        {
            ArgType = typeof(System.Boolean);
            DebugValue = arg;
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(System.Byte arg)
        {
            ArgType = typeof(System.Byte);
            DebugValue = arg;
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(System.Byte[] arg)
        {
            ArgType = typeof(System.Byte[]);
            DebugValue = arg;
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(System.DateTime arg)
        {
            ArgType = typeof(System.DateTime);
            DebugValue = arg;
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(System.DateTimeOffset arg)
        {
            ArgType = typeof(System.DateTimeOffset);
            DebugValue = arg;
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(System.Decimal arg)
        {
            ArgType = typeof(System.Decimal);
            DebugValue = arg;
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(System.Double arg)
        {
            ArgType = typeof(System.Double);
            DebugValue = arg;
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(System.Guid arg)
        {
            ArgType = typeof(System.Guid);
            DebugValue = arg;
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(System.Int16 arg)
        {
            ArgType = typeof(System.Int16);
            DebugValue = arg;
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(System.Int32 arg)
        {
            ArgType = typeof(System.Int32);
            DebugValue = arg;
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(System.Int64 arg)
        {
            ArgType = typeof(System.Int64);
            DebugValue = arg;
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(System.Single arg)
        {
            ArgType = typeof(System.Single);
            DebugValue = arg;
        }

        /// <summary>
        /// Sets the argument's type.
        /// </summary>
        /// <param name="arg">Is an argument.</param>
        protected internal void SetArgType(System.TimeSpan arg)
        {
            ArgType = typeof(System.TimeSpan);
            DebugValue = arg;
        }

        #endregion

        #endregion

        #region ToString

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return DebuggerInfo.ToDebugString(GetType(), ArgType.ToString());
        }

        #endregion

    }
}
