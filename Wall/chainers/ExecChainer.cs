#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class ExecChainer : EndChainer, IBegin 
    {
        // the object that is to be executed
        private Chainer _innerObject;

        // allowed inliners
        private static readonly DT[] _inliners = 
        { 
            DT.InProcedure, 
            DT.InStoredProcedure, 
            DT.InSql 
        };
        private readonly DT[] _procInliners = { DT.InProcedure };
        private readonly DT[] _sprocInliners = { DT.InStoredProcedure };
        private readonly DT[] _sqlInliners = { DT.InSql };

        internal string ReturnVariable { get; set; }

        internal override string Method
        {
            get
            {
                return Text.Method.Exec;
            }
        }

        #region Methods

        private string BuildExecProc(BuildContext buildContext, BuildArgs buildArgs)
        {
            // assign the current node of the build context
            buildContext.Current = _innerObject;

            if (ReturnVariable != null)
            {
                return Text.GenerateSql(1000)
                    .Append(Executable.Build(buildContext, buildArgs))
                    .NewLine(Text.Set).S()
                    .Append(ReturnVariable).Append(Text._Equal_).Append(Text.Reserved.ReturnValueOuterParam)
                    .Terminate()
                    .ToString();
            }
            else
            {
                return Executable.Build(buildContext, buildArgs);
            }
        }

        private void BodyMethod(IExecutable iexecutable, string returnVariable)
        {
            ReturnVariable = returnVariable;

            _innerObject = iexecutable as Chainer;
            if (iexecutable is Compilable)
            {
                Executable = new Executable((Compilable)iexecutable);
            }
            else if (iexecutable is DbProcedure)
            {
                var dbProc = (DbProcedure)iexecutable;
                dbProc.BuildProc(null);
                var cpass = new PassChainer(dbProc);
                Executable = cpass.Executable;
                _innerObject = cpass;
            }
            // PassChainer
            else
            {
                Executable = ((Chainer)iexecutable).Executable;
            }

            Executable.Inner = true;    // important!
            Executable.ArgumentsCriticalCheck(false);
            TryThrow(Executable.Exception);
        }

        internal static ExecChainer Create(
            Chainer prev, 
            ExecArgument executable, 
            string returnValueToVariable,
            ParameterArgument[] arguments)
        {
            if (executable.Exception != null)
            {
                executable.Exception.ObjectName = prev.GetRoot().Name;
                executable.Exception.Arguments = "executable = null";
                executable.Exception.Method = Text.Method.Exec;
                throw executable.Exception;
            }

            QueryTalkException exception;
            var root = prev.GetRoot();
            Variable inliner = null;
            if (executable.Original is System.String)
            {
                inliner = root.TryGetVariable((string)executable.Original,
                    out exception, Variable.SearchType.Inliner);
            }
            exception = null;  

            // stored procedure, SQL batch, mapped stored procedure
            if (inliner == null)
            {
                var cpass = PassChainer.Create(new InternalRoot(), executable);
                return new ExecChainer((Chainer)prev, cpass, returnValueToVariable);
            }
            // inliner: stored procedure, procedure, SQL
            else
            {
                if (inliner.DT == DT.InProcedure)
                {
                    return new ExecChainer(prev, inliner, returnValueToVariable, arguments);
                }
                else if (inliner.DT == DT.InStoredProcedure)
                {
                    return new ExecChainer(prev, inliner, returnValueToVariable, arguments);
                }
                else if (inliner.DT == DT.InSql)
                {
                    return new ExecChainer(prev, inliner, returnValueToVariable, arguments);
                }
                else
                {
                    exception = inliner.DT.InvalidInlinerException(typeof(ExecChainer).ToString(),
                        inliner.Name, _inliners);
                    exception.ObjectName = root.Name;
                    exception.Method = Text.Method.Exec;
                    throw exception;
                }
            }
        }

        #endregion

        #region Constructors

        internal ExecChainer(Chainer prev, IExecutable iexecutable, string variable = null)
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => iexecutable, iexecutable));

            BodyMethod(iexecutable, variable);

            Build = (buildContext, buildArgs) =>
            {
                return BuildExecProc(buildContext, buildArgs);
            };
        }

        // inliner: procedure, snippet, stored procedure
        private ExecChainer(
            Chainer prev, 
            Variable inliner, 
            string returnValueToVariable, 
            params ParameterArgument[] arguments) : base(prev)
        {
            Build = (buildContext, buildArgs) =>
            {
                ParameterArgument inlinerArgument = buildArgs.Executable.GetInlinerArgument(inliner.Name);
                if (inlinerArgument.Value == null)
                {
                    buildContext.TryTakeException(new QueryTalkException(this,
                        QueryTalkExceptionType.InlinerArgumentNull,
                        String.Format("{0} = null", inliner.Name)));
                    return null;
                }

                if (inliner.DT == DT.InProcedure)
                {
                    if (inlinerArgument.DT != DT.InProcedure)
                    {
                        buildContext.TryTakeException(inliner.DT.InvalidInlinerException(GetType().Name,
                            inliner.Name, _procInliners));
                        return null;
                    }

                    PassChainer cpass;
                    if (inlinerArgument.ArgType == typeof(PassChainer))
                    {
                        cpass = (PassChainer)inlinerArgument.Original;
                    }
                    // inliner variable
                    else
                    {
                        var proc = (Procedure)inlinerArgument.Original;
                        cpass = new PassChainer(proc, arguments);
                    }

                    BodyMethod(cpass, returnValueToVariable);

                    return BuildExecProc(buildContext, buildArgs);
                }
                else if (inliner.DT == DT.InStoredProcedure)
                {
                    if (inlinerArgument.DT != DT.InStoredProcedure)
                    {
                        buildContext.TryTakeException(inliner.DT.InvalidInlinerException(GetType().Name,
                            inliner.Name, _sprocInliners));
                        return null;
                    }

                    PassChainer cpass;
                    if (inlinerArgument.ArgType == typeof(PassChainer))
                    {
                        cpass = (PassChainer)inlinerArgument.Original;
                    }
                    else if (inlinerArgument.ArgType == typeof(ExecArgument))
                    {
                        cpass = PassChainer.Create(new InternalRoot(), (ExecArgument)inlinerArgument.Original);
                    }
                    // inliner variable
                    else
                    {
                        var execArgument = ((string)inlinerArgument.Original).Pass(arguments);
                        cpass = PassChainer.Create(new InternalRoot(), execArgument);
                    }

                    BodyMethod(cpass, returnValueToVariable);

                    return BuildExecProc(buildContext, buildArgs);
                }
                else
                {
                    if (inlinerArgument.DT != DT.InSql)
                    {
                        buildContext.TryTakeException(inliner.DT.InvalidInlinerException(GetType().Name,
                            inliner.Name, _sqlInliners));
                        return null;
                    }

                    PassChainer cpass;
                    if (inlinerArgument.ArgType == typeof(ExecArgument))
                    {
                        cpass = PassChainer.Create(new InternalRoot(), (ExecArgument)inlinerArgument.Original);
                    }
                    // inliner variable
                    else
                    {
                        var execArgument = ((string)inlinerArgument.Original).Pass(arguments);
                        cpass = PassChainer.Create(new InternalRoot(), execArgument);
                    }

                    BodyMethod(cpass, null);

                    return BuildExecProc(buildContext, buildArgs);
                }

            };
        }

        #endregion

    }
}
