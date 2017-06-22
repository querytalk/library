#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Text.RegularExpressions;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents a stored procedure or a SQL batch.
    /// </summary>
    public sealed class ExecArgument : Argument
    {
        private ExecArgument()
            : base(null)
        { }

        private ExecArgument(Chainer arg)
            : base(arg)
        {
            TryTakeAll(arg);
        }

        private Wall.Compilable.ObjectType _compilableType = Compilable.ObjectType.StoredProc;
        internal Wall.Compilable.ObjectType CompilableType
        {
            get
            {
                return _compilableType;
            }
        }

        internal ParameterArgument[] Arguments { get; private set; }

        // for inliner detection
        internal bool IsString
        {
            get
            {
                return Original is System.String;
            }
        }

        internal bool IsNull
        {
            get
            {
                return Original == null;
            }
        }

        #region System.String

        internal ExecArgument(System.String arg, ParameterArgument[] arguments)
            : base(arg)
        {
            SetArgType(arg);
            if (!CheckNull(Arg(() => arg, arg)))
            {
                return;
            }

            Arguments = arguments;
            _compilableType = ResolveCompilableType(arg);

            Build = (buildContext, buildArgs) =>
            {
                string sql;

                // stored procedure
                if (_compilableType == Compilable.ObjectType.StoredProc)
                {
                    if (ProcessVariable(buildContext, buildArgs, out sql))
                    {
                        return sql;
                    }

                    sql = Filter.DelimitMultiPartOrParam(arg, IdentifierType.Table, out chainException);
                }
                // SQL batch
                else
                {
                    sql = Text.GenerateSql(500)
                        .NewLine(arg)
                        .TerminateSingle()
                        .ToString();
                }

                buildContext.TryTakeException(chainException);
                return sql;
            };
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ExecArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ExecArgument(System.String arg)
        {
            return new ExecArgument(arg, new ParameterArgument[]{});
        }

        #endregion

        #region Identifier

        internal ExecArgument(Identifier arg, ParameterArgument[] arguments)
            : this(arg as Chainer)
        {
            _compilableType = Compilable.ObjectType.StoredProc;

            SetArgType(arg);

            if (!CheckNull(Arg(() => arg, arg)))
            {
                return;
            }

            Arguments = arguments;
            SetArgType(arg);

            Build = (buildContext, buildArgs) =>
            {
                string sql;
                sql = arg.Build(buildContext, buildArgs);
                buildContext.TryTakeException(chainException);
                return sql;
            };  
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ExecArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ExecArgument(Identifier arg)
        {
            return new ExecArgument(arg);
        }

        #endregion

        // resolve object type
        private static Wall.Compilable.ObjectType ResolveCompilableType(string procOrBatch)
        {
            // parsing rule: if a string contains a single space then it is a SQL batch
            if (Regex.IsMatch(procOrBatch, @"\s"))
            {
                return Wall.Compilable.ObjectType.SqlBatch;
            }
            else
            {
                return Wall.Compilable.ObjectType.StoredProc;
            }
        }

    }
}
