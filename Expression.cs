#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents a SQL expression.(This class has no public constructor.)
    /// </summary>  
    public sealed class Expression : Chainer, INonPredecessor,
        ISemantic,
        IScalar,
        IAs
    {
        // allowed inliners
        private static readonly DT[] _inliners = { DT.InExpression };

        // forbidden characters
        private static string exprForbiddenRegex = "(;)|(--)";

        internal override string Method
        {
            get
            {
                return Text.Method.Expression;
            }
        }

        // expression arguments storage (for concatenation detection)
        internal ScalarArgument[] Arguments { get; private set; }

        // reference of the first mapped argument of the first expression
        private DbColumn _dbColumn;

        private Expression(Chainer prev, bool overload)
            : base(null)
        { }

        #region Unary

        internal Expression(Operator op, ScalarArgument arg)
            : this(null, true)
        {
            arg = arg ?? Designer.Null;

            Arguments = new[] { arg };

            TryStoreDbColumn(arg);

            TryTake(arg);

            Build = (buildContext, buildArgs) =>
            {
                if (buildContext.TryTakeException(chainException))
                {
                    return null;
                }

                if (!CheckConcatenator(buildContext, arg))
                {
                    return null;
                }

                arg.RootSubject = ((ISemantic)this).RootSubject;

                var sql = BuildUnaryExpression(
                    op,
                    arg.Build(buildContext, buildArgs));

                buildContext.TryTakeException(chainException);

                return sql;
            };
        }

        private static string BuildUnaryExpression(Operator op, string arg)
        {
            switch (op)
            {
                case Operator.Not: return String.Format("NOT ({0})", arg);
                case Operator.IsNull: return String.Format("{0} IS NULL", arg);
                case Operator.IsNotNull: return String.Format("{0} IS NOT NULL", arg);
                case Operator.Exists: return String.Format("EXISTS {0}", arg);
                case Operator.NotExists: return String.Format("NOT EXISTS {0}", arg);
                case Operator.Any: return String.Format("ANY {0}", arg);
                case Operator.Some: return String.Format("SOME {0}", arg);
                case Operator.All: return String.Format("ALL {0}", arg);
                case Operator.BitwiseNot: return String.Format("~{0}", arg);
                default:
                    throw new QueryTalkException("Expression.BuildUnaryExpression", QueryTalkExceptionType.InvalidExpressionOperatorInnerException,
                        String.Format("operator = {0}{1}   argument = {2}", op.ToString(), Environment.NewLine, arg));
            }
        }

        #endregion

        #region Binary

        internal Expression(Operator op, ScalarArgument arg1, ScalarArgument arg2)
            : this(null, true)
        {
            arg1 = arg1 ?? Designer.Null;
            arg2 = arg2 ?? Designer.Null;

            if (Value.IsNull(arg2.Original))
            {
                if (op == Operator.Equal)
                {
                    op = Operator.IsNull;
                }
                else if (op == Operator.NotEqual)
                {
                    op = Operator.IsNotNull;
                }
            }

            Arguments = new[] { arg1, arg2 };

            TryStoreDbColumn(arg1);

            TryTake(arg1);
            TryTake(arg2);

            Build = (buildContext, buildArgs) =>
            {
                if (buildContext.TryTakeException(chainException))
                {
                    return null;
                }

                if (!CheckConcatenator(buildContext, arg1) || !CheckConcatenator(buildContext, arg2))
                {
                    return null;
                }

                arg1.RootSubject = ((ISemantic)this).RootSubject;
                arg2.RootSubject = ((ISemantic)this).RootSubject;

                var sql = BuildBinaryExpression(
                    op,
                    arg1.Build(buildContext, buildArgs),
                    arg2.Build(buildContext, buildArgs));

                buildContext.TryTakeException(chainException);

                return sql;
            };
        }

        private static string BuildBinaryExpression(Operator op, string arg1, string arg2)
        {
            switch (op)
            {
                case Operator.And: return String.Format("({0} AND {1})", arg1, arg2);
                case Operator.AndNot: return String.Format("({0} AND NOT {1})", arg1, arg2);
                case Operator.Or: return String.Format("({0} OR {1})", arg1, arg2);
                case Operator.OrNot: return String.Format("({0} OR NOT {1})", arg1, arg2);
                case Operator.Equal: return String.Format("{0} = {1}", arg1, arg2);
                case Operator.NotEqual: return String.Format("{0} <> {1}", arg1, arg2);
                case Operator.Like: return String.Format("{0} LIKE {1}", arg1, arg2);
                case Operator.NotLike: return String.Format("{0} NOT LIKE {1}", arg1, arg2);
                case Operator.GreaterThan: return String.Format("{0} > {1}", arg1, arg2);
                case Operator.GreaterOrEqualThan: return String.Format("{0} >= {1}", arg1, arg2);
                case Operator.NotGreaterThan: return String.Format("{0} !> {1}", arg1, arg2);
                case Operator.LessThan: return String.Format("{0} < {1}", arg1, arg2);
                case Operator.LessOrEqualThan: return String.Format("{0} <= {1}", arg1, arg2);
                case Operator.NotLessThan: return String.Format("{0} !< {1}", arg1, arg2);
                case Operator.Plus: return String.Format("({0}) + ({1})", arg1, arg2);
                case Operator.Minus: return String.Format("({0}) - ({1})", arg1, arg2);
                case Operator.MultiplyBy: return String.Format("({0}) * ({1})", arg1, arg2);
                case Operator.DivideBy: return String.Format("({0}) / ({1})", arg1, arg2);
                case Operator.Modulo: return String.Format("({0}) % ({1})", arg1, arg2);
                case Operator.AndBitwise: return String.Format("({0}) & ({1})", arg1, arg2);
                case Operator.OrBitwise: return String.Format("({0}) | ({1})", arg1, arg2);
                case Operator.ExclusiveOrBitwise: return String.Format("({0}) ^ ({1})", arg1, arg2);
                case Operator.In: return String.Format("{0} IN ({1})", arg1, arg2);
                case Operator.NotIn: return String.Format("{0} NOT IN ({1})", arg1, arg2);
                case Operator.IsNull: return String.Format("{0} IS NULL", arg1);
                case Operator.IsNotNull: return String.Format("{0} IS NOT NULL", arg1);

                default:
                    throw new QueryTalkException("Expression.BuildBinaryExpression", QueryTalkExceptionType.InvalidExpressionOperatorInnerException,
                        String.Format("operator = {0}{1}   argument1 = {2}{3}   argument2 = {4}", 
                            op.ToString(), Environment.NewLine, arg1, Environment.NewLine, arg2));
            }
        }

        #endregion

        #region BinaryMultiPart (IN)

        internal Expression(Operator op, ScalarArgument arg1, ValueScalarArgument[] arguments)
            : this(null, true)
        {
            arg1 = arg1 ?? Designer.Null;

            CheckNullOrEmpty(Argc(() => arguments, arguments));

            Arguments = new[] { arg1 };

            TryStoreDbColumn(arg1);

            TryTake(arg1);

            Build = (buildContext, buildArgs) =>
            {
                if (buildContext.TryTakeException(chainException))
                {
                    return null;
                }

                if (!CheckConcatenator(buildContext, arg1))
                {
                    return null;
                }

                arg1.RootSubject = ((ISemantic)this).RootSubject;
                Array.ForEach(arguments, a => a.RootSubject = ((ISemantic)this).RootSubject);

                var sql = BuildBinaryExpression(
                    op,
                    (arg1 as Chainer).Build(buildContext, buildArgs),
                    ValueScalarArgument.Concatenate(arguments, buildContext, buildArgs));

                buildContext.TryTakeException(chainException);

                return sql;
            };
        }

        internal Expression(Operator op, ScalarArgument arg1, string collectionSql, QueryTalkException exception)
            : this(null, true)
        {
            if (exception != null)
            {
                chainException = exception;
                chainException.Extra = Text.Free.ExpressionNullExtra;
                return;
            }

            arg1 = arg1 ?? Designer.Null;

            Arguments = new[] { arg1 };

            var list = collectionSql;
            CheckNull(Arg(() => list, list));

            TryTake(arg1);
            TryTakeException(exception);

            Build = (buildContext, buildArgs) =>
            {
                if (buildContext.TryTakeException(chainException))
                {
                    return null;
                }

                if (!CheckConcatenator(buildContext, arg1))
                {
                    return null;
                }

                arg1.RootSubject = ((ISemantic)this).RootSubject;

                var sql = BuildBinaryExpression(
                    op,
                    (arg1 as Chainer).Build(buildContext, buildArgs),
                    collectionSql);

                buildContext.TryTakeException(chainException);

                return sql;
            };
        }

        #endregion

        #region ThreePart

        internal Expression(Operator op, ScalarArgument arg1, ScalarArgument arg2, ScalarArgument arg3)
            : this(null, true)
        {
            arg1 = arg1 ?? Designer.Null;
            arg2 = arg2 ?? Designer.Null;
            arg3 = arg3 ?? Designer.Null;

            Arguments = new[] { arg1, arg2, arg3 };

            TryStoreDbColumn(arg1);

            TryTake(arg1);
            TryTake(arg2);
            TryTake(arg3);

            Build = (buildContext, buildArgs) =>
            {
                if (buildContext.TryTakeException(chainException))
                {
                    return null;
                }

                if (!CheckConcatenator(buildContext, arg1) || !CheckConcatenator(buildContext, arg2) || !CheckConcatenator(buildContext, arg3))
                {
                    return null;
                }

                arg1.RootSubject = ((ISemantic)this).RootSubject;
                arg2.RootSubject = ((ISemantic)this).RootSubject;
                arg3.RootSubject = ((ISemantic)this).RootSubject;

                var sql = BuildThreePartExpression(buildContext,
                    op,
                    arg1.Build(buildContext, buildArgs),
                    arg2.Build(buildContext, buildArgs),
                    arg3.Build(buildContext, buildArgs));

                buildContext.TryTakeException(chainException);

                return sql;
            };
        }

        private string BuildThreePartExpression(BuildContext buildContext, Operator op, string arg1, string arg2, string arg3)
        {
            if (!CheckConcatenator(buildContext, arg1) || 
                !CheckConcatenator(buildContext, arg2) ||
                !CheckConcatenator(buildContext, arg2))
            {
                return null;
            }

            switch (op)
            {
                case Operator.Like: return String.Format("{0} LIKE {1} ESCAPE {2}", arg1, arg2, arg3);
                case Operator.NotLike: return String.Format("{0} NOT LIKE {1} ESCAPE {2}", arg1, arg2, arg3);
                case Operator.Between: return String.Format("{0} BETWEEN {1} AND {2}", arg1, arg2, arg3);
                case Operator.NotBetween: return String.Format("{0} NOT BETWEEN {1} AND {2}", arg1, arg2, arg3);
                default:
                    throw new QueryTalkException("Expression.BuildThreePartExpression", QueryTalkExceptionType.InvalidExpressionOperatorInnerException,
                        String.Format("operator = {0}{1}   argument1 = {2}{3}   argument2 = {4}{5}   argument3 = {6}",
                            op.ToString(), Environment.NewLine, arg1, Environment.NewLine, arg2, Environment.NewLine, arg3));
            }
        }

        #endregion

        #region Collections

        internal static string BuildSequence<T>(IEnumerable<T> scalarCollection, out QueryTalkException exception)
        {
            exception = null;

            if (scalarCollection == null || scalarCollection.Count() == 0)
            {
                exception = new QueryTalkException("Expression.BuildSequence", QueryTalkExceptionType.CollectionNullOrEmpty,
                    String.Format("element type = {0}", typeof(T)), Text.Method.In);
                return null;
            }

            Type type;
            QueryTalkException exclr;
            if (Mapping.CheckClrCompliance(typeof(T), out type, out exclr) != Mapping.ClrTypeMatch.ClrMatch)
            {
                throw exclr;
            }

            StringBuilder sql = Text.GenerateSql(200);
            int i = 0;
            foreach (T item in scalarCollection)
            {
                if (i++ > 0)
                {
                    sql.Append(Text.Comma);
                }

                sql.Append(Mapping.BuildUnchecked(item));
            }

            return sql.ToString();
        }


        #endregion

        #region System.String (inliner)

        /// <summary>
        /// Implicitly converts an argument into an expression.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator Expression(System.String arg)
        {
            return new Expression(arg);
        }

        internal Expression(string arg)
            : base(null)
        {
            Build = (buildContext, buildArgs) =>
            {
                QueryTalkException exception;
                var variable = buildContext.TryGetVariable(arg, out exception);

                if (variable.IsInliner())
                {
                    if (variable.DT != DT.InExpression)
                    {
                        buildContext.TryTakeException(variable.DT.InvalidInlinerException(GetType().Name,
                            variable.Name, _inliners));
                        return null;
                    }

                    string arg2 = variable.Name;
                    if (buildArgs.Executable != null)
                    {
                        ParameterArgument argument = buildArgs.Executable.GetInlinerArgument(arg2);
                        if (argument.Value != null)
                        {
                            arg2 = argument.Build(buildContext, buildArgs);
                        }
                        else
                        {
                            buildContext.TryTakeException(new QueryTalkException(this,
                                QueryTalkExceptionType.InlinerArgumentNull,
                                String.Format("{0} = null", arg2)));
                            return null;
                        }
                    }

                    return arg2.ToString();
                }
                else if (variable.IsConcatenator())
                {
                    buildContext.TryTakeException(InvalidConcatenationException(variable.Name));
                    return null;
                }
                // non-existing variable or expression string
                else
                {
                    // note: non-existing variable will pass the test => the server will throw an error
                    CheckExpressionString(arg);

                    return arg;
                }
            };  
        }

        #endregion

        #region ExpressionString

        // creates expression by expression string
        internal static Expression CreateByExpressionString(string expressionString)
        {
            var enclosed = Filter.Enclose(expressionString);   
            var expression = new Expression(enclosed);
            expression.CheckExpressionString(expressionString);
            return expression;
        }

        private bool CheckExpressionString(string expressionString)
        {
            if (String.IsNullOrWhiteSpace(expressionString))
            {
                throw new QueryTalkException(this, QueryTalkExceptionType.InvalidExpressionString, null);
            }

            Regex regForbidden = new Regex(exprForbiddenRegex);
            if (regForbidden.IsMatch(expressionString))
            {
                throw new QueryTalkException(this, QueryTalkExceptionType.InvalidExpressionString,
                    ArgVal(() => expressionString, expressionString));
            }

            return true;
        }

        #endregion

        #region Equality Simplifier

        // simplifies equality expression 
        //    A.Equal(B) => (A, B) 
        //    A.NotEqual(B) => (A, B, false)
        internal static Expression EqualitySimplifier(ScalarArgument argument1, ScalarArgument argument2, bool equality = true)
        {
            argument1 = argument1 == null ? Designer.Null : argument1;

            Expression expression = null;
            if (argument2 == null ||
                (argument2.Original is Value && (Value)argument2.Original == Designer.Null))
            {
                if (equality)
                {
                    expression = argument1.IsNull();
                }
                else
                {
                    expression = argument1.IsNotNull();
                }
            }
            else
            {
                if (equality)
                {
                    expression = argument1.EqualTo(argument2);
                }
                else
                {
                    expression = argument1.NotEqualTo(argument2);
                }
            }

            expression.Arguments = new[] { argument1, argument2 };

            return expression;
        }

        #endregion

        #region Mapper

        #region ISemantic

        /// <summary>
        /// Implementation of ISemantic interface.
        /// </summary>
        DbNode ISemantic.Subject
        {
            get
            {
                if (_dbColumn != null && (ISemantic)_dbColumn.Parent != null)
                {
                    return ((ISemantic)_dbColumn.Parent).Subject;
                }
                else
                {
                    return null;
                }
            }
        }

        DbNode ISemantic.RootSubject { get; set; }

        /// <summary>
        /// Implementation of ISemantic interface.
        /// </summary>
        bool ISemantic.IsQuery
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Implementation of the Translate method.
        /// </summary>
        Chainer ISemantic.Translate(SemqContext context, DbNode predecessor)
        {
            if (_dbColumn != null && (ISemantic)_dbColumn.Parent != null)
            {
                return ((ISemantic)_dbColumn.Parent).Translate(context, predecessor);
            }
            else
            {
                return null;
            }
        }

        #endregion

        private void TryStoreDbColumn(ScalarArgument arg)
        {
            if (arg.Original is DbColumn)
            {
                _dbColumn = _dbColumn ?? (DbColumn)arg.Original;
            }
            else if (arg.Original is Expression)
            {
                _dbColumn = _dbColumn ?? ((Expression)arg.Original)._dbColumn;
            }
            else if (arg.Original is ScalarArgument)
            {
                TryStoreDbColumn((ScalarArgument)arg.Original);
            }
        }

        #endregion

        #region InvalidConcatenationException

        private bool CheckConcatenator(BuildContext buildContext, ScalarArgument arg)
        {
            string arg2 = null;

            if (arg.ArgType == typeof(System.String))
            {
                if (arg.Original != null && arg.Original.GetType() == typeof(System.String))
                {
                    arg2 = (string)arg.Original;
                }
            }
            else if (arg.Original is ValueScalarArgument && ((ValueScalarArgument)arg.Original).ArgType == typeof(System.String))
            {
                arg2 = (string)((ValueScalarArgument)arg.Original).Original;
            }

            if (arg2 == null)
            {
                return true;   
            }

            QueryTalkException exception;
            var variable = buildContext.TryGetVariable(arg2, out exception);
            if (variable.IsConcatenator())
            {
                buildContext.TryTakeException(InvalidConcatenationException(variable.Name));
                return false;  
            }

            return true;
        }

        private QueryTalkException InvalidConcatenationException(string variableName)
        {
            return new QueryTalkException(this, QueryTalkExceptionType.ConcatenationDisallowed,
                String.Format("concatenator = {0}", variableName), Text.Method.Expression);
        }

        #endregion

    }
}