#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Data.SqlClient;
using System.Reflection;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class Connectable : Chainer, IDesignerRoot, INonPredecessor, IExecutable, IName,
        IGo
    {

        #region IDesignerRoot

        Designer IDesignerRoot.GetRoot()
        {
            return ((IDesignerRoot)Executable).GetRoot();
        }

        #endregion

        #region Properties

        internal override string Method
        {
            get
            {
                return Text.Method.ConnectBy;
            }
        }

        internal Assembly Client { get; private set; }

        private string _connectionString = null;
        internal string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    return null;
                }

                // return async connection string to avoid DTC escalation when using TransactionScope
                return ConnectionStringAsync;   
            }
        }

        internal string ConnectionStringAsync
        {
            get
            {
                try
                {
                    if (_connectionString == null)
                    {
                        return null;
                    }

                    var connStringBuilder = new SqlConnectionStringBuilder(_connectionString);
                    connStringBuilder.AsynchronousProcessing = true;
                    return connStringBuilder.ToString();
                }
                catch (System.Exception ex)
                {
                    var exception = new QueryTalkException("Connectable.ConnectionStringAsync", QueryTalkExceptionType.InvalidConnectionString,
                        null, null);
                    exception.ClrException = ex;
                    throw exception;
                }
            }
        }

        private int _commandTimeout = 30; 
        internal int CommandTimeout
        {
            get
            {
                return _commandTimeout;
            }
        }

        internal void SetTimeout(int timeout)
        {
            _commandTimeout = timeout;
        }

        private Action<dynamic> _onAsyncCompletedEmpty = (ret) => { };
        private object _onAsyncCompleted = null;

        // handler that is invoked when asynchronous operation has completed
        internal object OnAsyncCompleted
        {
            get
            {
                if (_onAsyncCompleted != null)
                {
                    return _onAsyncCompleted;
                }
                else
                {
                    return _onAsyncCompletedEmpty;
                }
            }
            set
            {
                _onAsyncCompleted = value;
            }
        }

        internal bool HasOnAsyncCompleted
        {
            get
            {
                return _onAsyncCompleted != null;
            }
        }

        internal bool _asyncCanceled = false;
        internal bool AsyncCanceled
        {
            get
            {
                return _asyncCanceled;
            }
            set
            {
                if (_asyncCanceled == false && value == true)
                {
                    _asyncCanceled = true;
                }
            }
        }

        // forces to remove cancellation flag
        internal void ForceAsyncCanceledReset()
        {
            _asyncCanceled = false;
        }

        /// <summary>
        /// The name of the connectable object.
        /// </summary>
        string IName.Name
        {
            get
            {
                return ((IName)Executable).Name;
            }
        }

        private string _sql;
        internal string Sql
        {
            get
            {
                return _sql;
            }
            set
            {
                _sql = value;
            }
        }

        // a clean SQL body originated from the compilable object, inluding inlining, but no wrappers
        internal string Body
        {
            get
            {
                return Executable.Body;
            }
        }

        private string _userToImpersonate = null;
        internal string UserToImpersonate
        {
            get
            {
                return _userToImpersonate;
            }
        }

        // ignore table load
        internal bool IgnoreLoad;
        private string _testBody { get; set; }

        internal ParameterArgument[] OutputArguments { get; set; }

        internal int ReturnValue { get; set; }

        internal bool IsTesting
        {
            get
            {
                return _testBody != null;
            }
        }

        #endregion

        #region Constructors

        private Connectable(Executable executable, Assembly client, string testBody = null)
            : base(null)
        {
            Executable = executable;
            Client = client;
            _testBody = testBody;
            Build();
        }

        internal Connectable(Assembly client, Executable executable, ConnectionData connectionData, string testBody = null)
            : this(executable, client, testBody)
        {
            CheckNullAndThrow(Arg(() => connectionData, connectionData));

            Executable = executable;
            _connectionString = connectionData.ConnectionString;
            _commandTimeout = connectionData.CommandTimeout;
            Impersonate(connectionData.ExecuteAsUser);
        }

        #endregion

        #region Build

        // build the executable SQL code
        private new void Build()
        {
            // try throw the exception of the execution phase.
            TryThrow();
            Executable.ArgumentsCriticalCheck(true);
            TryThrow();

            var buildArgs = new BuildArgs(null);
            buildArgs.TestBody = _testBody; 

            // executable is build by the context of its compilable object
            var execSql = Executable.Build(
                new BuildContext(Executable.Compilable), buildArgs);

            _sql = BuildOutputWrapper(execSql);
        }

        // build the most outer SQL wrapper
        internal string BuildOutputWrapper(string execSql)
        {
            var root = Executable.Compilable.GetRoot();
            var outputArguments = ParameterArgument.GetOutputArguments(Executable.Arguments);
            var sql = Text.GenerateSql(1000).Append(Text.Free.QueryTalkCode);

            // after: drop temp tables
            var sqlAfter = Text.GenerateSql(100);
            sqlAfter.Append(DropTempTables());

            if (root.IsEmbeddedTryCatch)
            {
                sqlAfter
                    .NewLine(Text.EndTry)
                    .NewLine(Text.BeginCatch)
                    .NewLine(Text.Free.RaiserrorS)
                    .NewLine(Text.EndCatch).Terminate();
            }

            sql.Append(Text.Declare).S().Append(Text.Reserved.ReturnValueOuterParam)
                .Append(Text._As_).Append(Text.Free.EnclosedInt).Terminate().S()
                .Append(Text.Set).S().Append(Text.Reserved.ReturnValueOuterParam).Append(Text._Equal_)
                .Append(Text.Zero).Terminate();

            // TRY outer wrapper
            if (root.IsEmbeddedTryCatch)
            {
                sql.NewLine(Text.BeginTry);
            }

            // output arguments
            string outputValues = String.Empty;
            foreach (var argument in outputArguments)
            {
                // param in outer wrapper that holds the outer reference
                string paramOuterName = String.Format("{0}{1}{2}", argument.ParamName, Text.Underscore, Text.Output);

                // before
                sql.NewLine(Text.Declare).S()
                    .Append(paramOuterName).Append(Text._As_)
                    .Append(Executable.GetParamDeclaration(argument, false, true))
                    .Terminate().S()
                    .Append(Text.Set).S().Append(paramOuterName).Append(Text._Equal_);

                if (argument.TestValue != null)
                {
                    Testing.AppendTestValue(sql, argument);
                }
                else
                {
                    sql.Append(Mapping.BuildUnchecked(argument.Value));
                }

                sql.TerminateSingle();

                // after: return output values
                outputValues = Text.GenerateSql(100)
                    .NewLineIndent(Text.Comma)
                    .Append(paramOuterName)
                    .Append(Text._As_)
                    .Append(Filter.Delimit(paramOuterName))
                    .ToString();
            }

            // append last sql code: return value + output values
            sqlAfter
                .NewLine(Text.Select).S()
                .Append(Text.Free.ReturnValue)              
                .Append(Text._As_)
                .Append(Text.Reserved.ReturnValueColumnName)
                .Append(outputValues);               

            TryThrow(Text.Method.Pass);

            sql.NewLine(execSql)
                .Append(sqlAfter.ToString())
                .TerminateSingle();

            return sql.ToString();
        }

        private string DropTempTables()
        {
            var sql = Text.GenerateSql(200);
            Executable.Arguments.ForEach(argument =>
            {
                if (argument.DT == DT.TempTable)
                {
                    sql.NewLine(DropTempTableChainer.BuildDropNoCheck(argument.ParamName));
                }
            });

            return (Executable.Arguments.Count > 0) ? sql.ToString() : null;
        }

        #endregion

        #region Impersonate

        // Wrap SQL code with EXECUTE AS/REVERT wrapper.
        internal void Impersonate(string userToImpersonate)
        {
            if (userToImpersonate == null)
            {
                return;
            }

            _userToImpersonate = userToImpersonate;

            var sql = Text.GenerateSql(1000)
                .Append(Text.ExecuteAsUser).Append(Text._Equal_)
                .Append(Filter.DelimitQuoteNonAsterix(_userToImpersonate))
                .Terminate()
                .NewLine(_sql)
                .NewLine(Text.Revert)
                .Terminate();

            _sql = sql.ToString();
        }

        #endregion

        #region ToProc

        // sets the procedure reference
        internal Procedure ToProc()
        {
            // procedure:
            if (Executable.Compilable is Procedure)
            {
                var proc = (Procedure)Executable.Compilable;

                // always return compiled procedure
                if (proc.IsCompiled)
                {
                    return proc;
                }

                // return compiled instance 
                var compiled = Designer.GetNewDesigner(proc.Name)
                    .Inject(Executable.Body)
                    .EndProc();
                compiled.CopyParamsNonInlining(proc);
                return compiled;
            }
            // view:
            else
            {
                return Designer.GetNewDesigner(((IName)this).Name)
                    .Inject(Executable.Body)
                    .EndProc();
            }
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("Connectable ({0})", Executable.Compilable.Name);
        }

        #endregion

    }
}
