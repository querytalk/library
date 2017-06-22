#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using System.Diagnostics;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer : Chainer, INonPredecessor, IName 
    {

        #region Internal Designer

        // creates new designer (safe) 
        internal static NameChainer GetNewDesigner(string name = null)
        {
            return GetNewDesigner(name, false, true);
        }

        // creates new designer with arguments (safe)
        internal static NameChainer GetNewDesigner(
            string name, 
            bool isEmbeddedTransaction, 
            bool isEmbeddedTryCatch, Designer.IsolationLevel isolationLevel = IsolationLevel.Default)
        {
            var root = new InternalRoot(isEmbeddedTransaction);
            root.IsEmbeddedTryCatch = isEmbeddedTryCatch;
            root.Name = name;
            return new NameChainer(root, isolationLevel);
        }

        // gets the designer of this root (unsafe)
        //   note: This designer is not safe because the root can be used only once.
        internal NameChainer GetDesigner()
        {
            return new NameChainer(this);
        }

        internal Designer.IsolationLevel EmbeddedTransactionIsolationLevel { get; set; }

        private bool _isUsed;
        internal bool IsUsed
        {
            get
            {
                return _isUsed;
            }
        }
        internal void SetAsUsed()
        {
            _isUsed = true;
        }

        #endregion

        #region Properties

        // compilable type 
        //   note: The CompilableType is set at the end of chaining!
        internal Nullable<Compilable.ObjectType> CompilableType { get; set; }

        // first query (for views)
        internal new Query Query
        {
            get
            {
                var node = Next;
                while (node != null)
                {
                    if (node.IsQuery)
                    {
                        return node.Query;
                    }
                    node = node.Next;
                }
                return null;
            }
        }

        private string _rootName;
        /// <summary>
        /// A name of the QueryTalk object.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public string Name
        {
            get 
            { 
                return _rootName; 
            }
            internal set
            {
                _rootName = value ?? Wall.Text.NotAvailable;
            }
        }

        // root node 
        internal DbNode Node { get; set; }
        internal void CheckNodeReuseAndThrow()
        {
            if (Node != null)
            {
                Node.CheckReuseAndThrow();
            }
        }

        private bool _isEmbeddedTransaction = true;   
        /// <summary>
        /// Indicates whether the procedure is wrapped by the transaction block. 
        /// </summary>
        internal protected bool IsEmbeddedTransaction
        {
            get
            {
                return _isEmbeddedTransaction;
            }
            set
            {
                _isEmbeddedTransaction = value;
            }
        }

        private bool _isEmbeddedTryCatch = true;     
        /// <summary>
        /// Indicates whether the procedure is wrapped by the try-catch block. 
        /// </summary>
        internal protected bool IsEmbeddedTryCatch
        {
            get
            {
                return _isEmbeddedTryCatch;
            }
            set
            {
                _isEmbeddedTryCatch = value;
            }
        }

        internal ConnectionKey ConnectionKey { get; set; }

        #region Statements

        private List<Statement> _statements = new List<Statement>();
        internal List<Statement> Statements
        {
            get
            {
                return _statements;
            }
        }

        // auto-incremental statement index
        private int _statementIndex = 0;

        internal int GetStatementIndex()
        {
            ++_statementIndex;   // auto-increment
            return _statementIndex - 1;
        }

        #endregion

        #region Params

        // collection of all params (explicit+implicit)
        private List<Variable> _params = new List<Variable>();
        internal List<Variable> AllParams
        {
            get
            {
                return _params;
            }
        }

        // collection of explicit params only
        internal List<Variable> ExplicitParams
        {
            get
            {
                return _params.Where(p => !p.IsParameterized).ToList();
            }
        }

        // collection of optional params only
        internal List<Variable> OptionalParams
        {
            get
            {
                return _params.Where(p => p.IsOptional).ToList();
            }
        }

        // collection of implicit params only
        internal List<Variable> ImplicitParams
        {
            get
            {
                return _params.Where(p => p.IsParameterized).ToList();
            }
        }

        // is object parameterized
        internal bool HasParams
        {
            get
            {
                return _params.Count > 0;
            }
        }

        // returns true if argumentsCount is valid (considering the optional arguments)
        internal bool ParamCountCheck(int argumentsCount)
        {
            return (argumentsCount <= ExplicitParams.Count)
                && (argumentsCount + OptionalParams.Count >= ExplicitParams.Count);
        }

        // general lookup method for checking the param existence
        internal bool ParamExists(string param)
        {
            return _params.Where(p => p.Name.EqualsCS(param)).Any();
        }

        // The central method for adding the params. 
        internal void TryAddParamOrThrow(Variable param, bool isAuto)
        {
            // check variable format (only if given by user)
            if (!isAuto)
            {
                Variable.CheckNameFormat(param.Name, out chainException);
                TryThrow(Wall.Text.Method.Param);
            }

            if (ParamExists(param.Name))
            {
                Throw(QueryTalkExceptionType.ParamOrVariableAlreadyDeclared,
                    String.Format("param = {0}", param.Name),
                    Wall.Text.Method.Param);
            }

            // check size declaration
            if (param.DT.IsDataType())
            {
                var info = Mapping.SqlMapping[param.DT];

                if ((info.SizeType == Mapping.SizeType.None
                    && param.DataType.Length + param.DataType.Precision + param.DataType.Scale > 0)
                    || (info.SizeType == Mapping.SizeType.Length
                    && param.DataType.Precision + param.DataType.Scale > 0)
                    || (info.SizeType == Mapping.SizeType.Precision
                    && param.DataType.Length > 0))
                {
                    Throw(QueryTalkExceptionType.InvalidDbTypeDeclaration,
                        String.Format("param = {0}{1}   " +
                            "db type = {2}{3}   " +
                            "length = {4}{5}   " +
                            "precision = {6}{7}   " +
                            "scale = {8}",
                            param.Name, Environment.NewLine,
                            info.SqlPlain, Environment.NewLine,
                            param.DataType.Length, Environment.NewLine,
                            param.DataType.Precision, Environment.NewLine,
                            param.DataType.Scale),
                        Wall.Text.Method.Pass);
                }

                // check size components
                if (param.DataType.Scale > param.DataType.Precision || param.DataType.Length < 0 || param.DataType.Precision < 0 || param.DataType.Scale < 0)
                {
                    Throw(QueryTalkExceptionType.InvalidDbTypeDeclaration,
                         String.Format("param = {0}{1}   " +
                            "db type = {2}{3}   " +
                            "length = {4}{5}   " +
                            "precision = {6}{7}   " +
                            "scale = {8}",
                            param.Name, Environment.NewLine,
                            info.SqlPlain, Environment.NewLine,
                            param.DataType.Length, Environment.NewLine,
                            param.DataType.Precision, Environment.NewLine,
                            param.DataType.Scale),
                        Wall.Text.Method.Pass);
                }
            }

            _params.Add(param);

            // add non-inline param to the variable collection
            if (param.DT.IsNotInliner())
            {
                TryAddVariableOrThrow(param, Wall.Text.Method.ParamOrTableParam, isAuto, false);
            }
        }

        #endregion

        #region Variables

        // collection of names of the parameters and variables together
        private List<Variable> _variables = new List<Variable>();
        internal List<Variable> Variables
        {
            get
            {
                return _variables;
            }
        }

        // attention:
        //   If root object is accessed through the BuildContext object, use buildContext.TryGetVariable method instead.
        internal Variable TryGetVariable(
            string variableName, 
            out QueryTalkException exception, 
            Variable.SearchType searchType = Variable.SearchType.Any)
        {
            exception = null;

            if (variableName == null)
            {
                exception = Variable.InvalidVariableException(variableName,
                    QueryTalkExceptionType.ArgumentNull);
                return null;
            }

            Variable param;       
            Variable variable;  

            // try get param
            param = AllParams.Where(p => p.Name.EqualsCS(variableName))
                .FirstOrDefault();

            // try get non-param
            variable = _variables.Where(v => v.Name.EqualsCS(variableName))
                .FirstOrDefault();

            if (searchType == Variable.SearchType.Any)
            {
                return param ?? variable;
            }

            if (searchType == Variable.SearchType.Inliner
                && (param == null || param.DT.IsNotInliner()))
            {
                exception = Variable.InvalidVariableException(variableName,
                    QueryTalkExceptionType.InlinerNotFound);
                return null;
            }

            // param/variable should exists
            if (searchType != Variable.SearchType.Any && (param == null && variable == null))
            {
                exception = Variable.InvalidVariableException(variableName,
                    QueryTalkExceptionType.ParamOrVariableNotDeclared);
                return null;
            }

            return param ?? variable;
        }

        // general lookup method for checking the variable existence
        //   returns:
        //     true : it exists
        //     false: not exists
        //     null : check not needed (it is a snippet)
        internal Nullable<bool> VariableExists(string variable)
        {
            // do not check the snippet neither the view
            if (CompilableType.IsViewOrSnippet())
            {
                return null;
            }

            return _variables.Where(v => v.Name.EqualsCS(variable)).Any();
        }

        // add method that throws exception if variable/param already exists
        internal void TryAddVariableOrThrow(Variable variable, string method, bool isAuto, 
            bool checkParams = true)        // if false then the variable is not checked in the params collection
                                            // (in case when already added param is to be added to the variable collection as well) 
        {
            // check variable format
            if (!isAuto)
            {
                Variable.CheckNameFormat(variable.Name, out chainException);
                TryThrow(method);
            }

            // check variable in param collection
            if (checkParams)
            {
                if (ParamExists(variable.Name))
                {
                    Throw(QueryTalkExceptionType.ParamOrVariableAlreadyDeclared,
                        String.Format("{0} = {1}", Wall.Text.Free.Variable, variable.Name),
                        method);
                }
            }

            // check variable in variable collection
            if (VariableExists(variable.Name) == true)
            {
                Throw(QueryTalkExceptionType.ParamOrVariableAlreadyDeclared,
                    String.Format("{0} = {1}", Wall.Text.Free.Variable, variable.Name),
                    method);
            }

            _variables.Add(variable);
        }

        // return true if an argument exists as a param or variable
        internal Nullable<bool> ParamOrVariableExists(string paramOrVariable)
        {
            // do not check the snippet
            if (CompilableType.IsViewOrSnippet())
            {
                return null;
            }

            // check params
            bool exists = ParamExists(paramOrVariable);
            if (exists)
            {
                return true;
            }
            // check variables
            exists = VariableExists(paramOrVariable) == true;
            if (exists)
            {
                return true;
            }

            // neither param nor variable
            return false;
        }

        // copy variables from snippet's root
        internal void TakeVariables(Snippet snippet)
        {
            foreach (var variable in snippet.GetRoot().Variables)
            {
                TryAddVariableOrThrow(variable, QueryTalk.Wall.Text.Method.Inject, false);
            }
        }

        #endregion

        #region Labels

        // collection of labels
        private List<string> _labels = new List<string>();

        // add method that throws exception if label already exists
        internal void TryAddLabelOrThrow(string label)
        {
            if (_labels.Contains(label.ToUpperCS()))
            {
                Throw(QueryTalkExceptionType.LabelAlreadyDeclared,
                    ArgVal(() => label, label),
                    ".Label");
            }
            _labels.Add(label.ToUpperCS());
        }

        #endregion

        #region Temp tables

        // collection of temp tables
        private List<string> _tempTables = new List<string>();

        internal void TryAddTempTable(string tempTable)
        {
            if (!_tempTables.Contains(tempTable.ToUpperCS()))
            {
                _tempTables.Add(tempTable.ToUpperCS());
            }
        }

        // general lookup method for checking temp table existence
        internal bool TempTableExists(string table, bool remove = false)
        {
            var exists = _tempTables.Where(tt => tt.EqualsCS(table)).Any();
            if (exists && remove)
            {
                _tempTables.Remove(table.ToUpperCS());
            }

            return exists;
        }

        #endregion

        // auto-incremental 1-based variable index
        private long _variableIndex = 0;

        // variable index provider method
        internal long GetVariableIndex()
        {
            return ++_variableIndex;
        }

        // variable unique index based on System.Guid
        //   To avoid collisions when the data views are passed directly to the stored proc or function inside the .Exec method.
        internal static long GetVariableGuid()
        {
            return Math.Abs(BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0));
        }
       
        // auto-incremental 1-based column index
        private int _colIndex = 0;
        internal int ColIndex
        {
            get
            {
                return ++_colIndex; 
            }
        }

        internal int IfCounter { get; set; }
        internal int WhileCounter { get; set; }
        internal int TryCatchCounter { get; set; }
        internal int CursorCounter { get; set; }
        internal int TranCounter { get; set; }

        private int _uniqueIndex = 0;
        internal int GetUniqueIndex()
        {
            return ++_uniqueIndex;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// This constructor is not intended for public use.
        /// </summary>
        protected Designer()
            : base(null)
        { }

        #endregion

        #region ClearForReuse

        // prepare root for reuse (for Testing Environment only)
        internal void ClearForReuse()
        {
            Statements.Clear();
            _isUsed = false;
            _params.Clear();
            _variables.Clear();
            _labels.Clear();
            _tempTables.Clear();
            _statementIndex = 0;
            _uniqueIndex = 0;
            _colIndex = 0;
            _variableIndex = 0;
            TranCounter = 0;
            CursorCounter = 0;
            TryCatchCounter = 0;
            WhileCounter = 0;
            IfCounter = 0;
        }

        #endregion

        #region Helper methods

        private static void Call(Assembly callingAssembly, Action<Assembly> method)
        {
            try
            {
                method(callingAssembly);
            }
            catch (QueryTalkException) { throw; }
            catch (System.Exception ex)
            {
                if (ex.InnerException is QueryTalkException)
                {
                    var ex2 = (QueryTalkException)ex;
                    if (ex2.ClrException != null)
                    {
                        throw ex2.ClrException;
                    }

                    throw ex.InnerException;
                }

                throw;
            }
        }

        private static T Call<T>(Assembly callingAssembly, Func<Assembly, T> method)
        {
            try
            {
                return method(callingAssembly);
            }
            catch (QueryTalkException) { throw; }
            catch (System.Exception ex)
            {
                if (ex.InnerException is QueryTalkException)
                {
                    var ex2 = (QueryTalkException)ex;
                    if (ex2.ClrException != null)
                    {
                        throw ex2.ClrException;
                    }

                    throw ex.InnerException;
                }

                throw;
            }
        }

        private static T[] MergeArrays<T>(T firstColumn, IEnumerable<T> otherColumns)
        {
            T[] merged = new T[otherColumns.Count() + 1];
            merged[0] = firstColumn;
            otherColumns.ToArray<T>().CopyTo(merged, 1);
            return merged;
        }

        private static void _Throw(QueryTalkExceptionType exceptionType, string arguments, string method)
        {
            throw new QueryTalkException("d.Throw", exceptionType, arguments, method);
        }

        #endregion

        #region Hidden base methods

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool Equals(object objA, object objB)
        {
            throw new System.NotImplementedException("Equals method of QueryTalk.Wall.Designer type is not allowed.");
        }

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool ReferenceEquals(object objA, object objB)
        {
            throw new System.NotImplementedException("ReferenceEquals method of QueryTalk.Wall.Designer type is not allowed.");
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return String.Format("Designer ({0})", Name);
        }

        #endregion

    }
}
