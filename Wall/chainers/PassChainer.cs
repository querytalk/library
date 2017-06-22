#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public class PassChainer : Chainer, IExecutable, IProcedure,
        IConnectBy,
        IGo
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Pass;
            }
        }

        internal static PassChainer Create(Designer root, ExecArgument procOrBatch)
        {
            var sqlObject = new SqlBatch(root, procOrBatch); 

            if (sqlObject.CompilableType == Compilable.ObjectType.StoredProc)
            {
                return new PassChainer((IStoredProc)sqlObject, procOrBatch.Arguments);
            }
            // SQL batch
            else
            {
                return new PassChainer((IStoredProc)sqlObject, procOrBatch.Arguments, sqlObject.BatchParameters);
            }
        }

        private PassChainer(Chainer prev)
            : base(prev)
        {
            var procOrView = prev;
            CheckNullAndThrow(Arg(() => procOrView, procOrView));
        }

        internal PassChainer(Chainer prev, ParameterArgument[] arguments)
            : this(prev)
        {
            Compilable compilable = (Compilable)prev;
            var root = GetRoot();

            if (arguments == null)
            {
                arguments = new ParameterArgument[] { Designer.Null };
            }

            // check arguments-params count match
            int argumentsCount = arguments.Count();
            int explicitParamsCount = root.ExplicitParams.Count;
            if (!root.ParamCountCheck(argumentsCount))
            {
                ThrowArgumentCountMismatch(explicitParamsCount, argumentsCount);
            }

            // check each argument
            int i = 0;
            Array.ForEach(arguments, argument =>
                {
                    if (argument != null && argument.Exception != null)
                    {
                        if (argument.Exception.Arguments == null && i < root.ExplicitParams.Count)
                        {
                            argument.Exception.Arguments = String.Format("param = {0}", root.ExplicitParams[i].Name);
                        }

                        TryThrow(argument.Exception);
                    }
                    ++i;
                });

            Executable = new Executable(compilable, arguments);
        }

        // ctor for non-mapped stored procedure
        internal PassChainer(IStoredProc prev, ParameterArgument[] arguments)
            : this((Chainer)prev)
        {
            var root = GetRoot();

            if (arguments == null)
            {
                arguments = new ParameterArgument[] { null };
            }

            Array.ForEach(arguments, argument =>
            {
                if (argument == null)
                {
                    argument = Designer.Null;
                }

                TryThrow(argument.Exception);

                // infer data type from value
                var param = Variable.InferParam(root, argument, out chainException);
                TryThrow();
                root.TryAddParamOrThrow(param, true);
            });

            Executable = new Executable((Compilable)prev, arguments);
        }

        // ctor for SQL batch
        internal PassChainer(IStoredProc prev, ParameterArgument[] arguments, List<string> batchParameters)
            : this((Chainer)prev)
        {
            var root = GetRoot();

            // check arguments-params count
            int argumentsCount = arguments == null ? 0 : arguments.Count();
            int parametersCount = batchParameters.Count;
            if (argumentsCount != parametersCount)
            {
                ThrowArgumentCountMismatch(parametersCount, argumentsCount);
            }

            if (arguments == null)
            {
                arguments = new ParameterArgument[] { null };
            }

            int i = 0;
            Array.ForEach(arguments, argument =>
            {
                if (argument == null)
                {
                    argument = Designer.Null;
                }

                TryThrow(argument.Exception);

                // infer data type from value
                var param = Variable.InferParam(root, argument, out chainException, batchParameters[i++]);
                TryThrow();
                root.TryAddParamOrThrow(param, true);
            });

            Executable = new Executable((Compilable)prev, arguments);
        }

        // ctor for mapped stored procedure
        internal PassChainer(DbProcedure dbProcedure)
            : this(dbProcedure.Mapper)
        {
            Executable = new Executable((Compilable)dbProcedure.Mapper, dbProcedure.Arguments);
        }

        private void ThrowArgumentCountMismatch(int parametersCount, int argumentsCount)
        {
            Throw(QueryTalkExceptionType.ArgumentCountMismatch,
                String.Format("parameters count = {0}{1}   arguments count = {2}",
                    parametersCount, Environment.NewLine, argumentsCount),
                Text.Method.Pass);
        }

    }

}
