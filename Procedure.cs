#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Linq;
using System.Text;
using System.Diagnostics;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents a parameterized SQL batch that behaves like a stored procedure. (This class has no public constructor.)
    /// </summary>
    [DebuggerTypeProxy(typeof(Wall.ProcedureDebuggerProxy))]  
    public class Procedure : Compilable, IExecutable, IProcedure,
        IPass,
        IGo,
        IConnectBy
    {
        internal override string Method
        {
            get
            {
                return Text.Method.EndProc;
            }
        }

        internal GraphPair[] GraphPairs { get; set; }

        internal bool HasTVP { get; private set; }

        internal string CompiledSql { get; private set; }

        internal Procedure(Chainer prev, bool isInternal = false)
            : base(prev, ObjectType.Procedure, isInternal)
        {
            var root = GetRoot();

            Build = (buildContext, buildArgs) =>
            {
                if (buildArgs.Executable != null && buildArgs.Executable.Exception != null)
                {
                    throw buildArgs.Executable.Exception;
                }

                if (Sql != null)
                {
                    return Sql;
                }

                StringBuilder sql = new StringBuilder();

                // append transaction begin block
                if (root.IsEmbeddedTransaction)
                {
                    BeginEmbeddedTransactionWrapper(root, sql, Text.Reserved.QtTransactionCount, Text.Reserved.QtTransactionSave);
                }

                if (HasTVP)
                {
                    InjectTableVariables(sql, buildArgs, false);
                }

                // build
                sql.Append(BuildChain(buildContext, buildArgs));

                sql.TrimEnd();

                if (!(prev is EndChainer))
                {
                    sql.TerminateSingle();
                }

                // append transaction end block
                if (root.IsEmbeddedTransaction)
                {
                    EndEmbeddedTransactionWrapper(sql, Text.Reserved.QtTransactionCount, Text.Reserved.QtTransactionSave);
                }

                // cache compiled code only, non-compiled should not be cached
                if (buildArgs.Executable == null)
                {
                    Sql = sql.ToString();
                }

                CompiledSql = Sql ?? sql.ToString();

                return CompiledSql;
            };

            CheckAndThrow();

            // detect TVP
            if (root.AllParams.Where(param => param.DT == DT.TableVariable).Any())
            {
                HasTVP = true;
            }

            // compiled property is set to false if a procedure has TVP or inliners
            if (HasTVP || root.AllParams.Where(param => param.DT.IsInliner()).Any())
            {
                compiled = false;
            }

            if (compiled)
            {
                Build(new BuildContext(this), new BuildArgs(null));
            }
        }

        internal void CopyParamsNonInlining(Procedure inlinedProc)
        {
            GetRoot().AllParams.AddRange(inlinedProc.GetRoot().AllParams.Where(param => !param.DT.IsInliner()));
        }

        private void InjectTableVariables(StringBuilder sql, BuildArgs buildArgs, bool skipTimestampColumn)
        {
            if (buildArgs.Executable == null)
            {
                Throw(QueryTalkExceptionType.NullExecutableInnerException, null, null);
            }

            foreach (var argument in (buildArgs.Executable.Arguments).Where(a => a.DT.IsTableVariable()))
            {
                var view = (View)argument.Value;

                sql.NewLine(Text.Declare).S()
                    .Append(argument.ParamName)
                    .Append(Text._As_)
                    .Append(Text.Table).S()
                    .NewLine(Text.LeftBracket);

                BeginTableChainer.BuildFromView(sql, view, skipTimestampColumn);

                sql.NewLine(Text.RightBracket)
                    .Terminate();

                if (view.RowCount > 0)
                {
                    BeginTableChainer.Fill(sql, argument.ParamName, view);
                }
            }
        }

        private void CheckAndThrow()
        {
            var root = GetRoot();
            var firstOptional = root.AllParams.Where(param => param.IsOptional).FirstOrDefault();
            if (firstOptional != null)
            {
                if (root.AllParams.Where(param => !param.IsOptional
                    && param.Ordinal > firstOptional.Ordinal).Any())
                {
                    Throw(QueryTalkExceptionType.OptionalParamInvalidPosition,
                        String.Format("optional param = {0}", firstOptional.Name),
                        Text.Method.ParamOptional);
                }
            }

            if (root.IfCounter != 0)
            {
                Throw(QueryTalkExceptionType.InvalidIfBlock, null, Text.Method.If);
            }

            if (root.WhileCounter != 0)
            {
                Throw(QueryTalkExceptionType.InvalidWhileBlock, null, Text.Method.While);
            }

            if (root.TryCatchCounter != 0)
            {
                Throw(QueryTalkExceptionType.InvalidTryCatchBlock, null, Text.Method.TryCatch);
            }

            if (root.CursorCounter != 0)
            {
                Throw(QueryTalkExceptionType.InvalidCursorBlock, null, Text.Method.EndCursor);
            }
        }

        private static void BeginEmbeddedTransactionWrapper(Designer root, StringBuilder sql,
            string trancountVar, string transaveVar)
        {
            var isolationLevel = root.EmbeddedTransactionIsolationLevel;
            if (isolationLevel != Designer.IsolationLevel.Default)
            {
                sql.NewLine(Text.SetTransactionIsolationLevel).S().Append(isolationLevel.ToSql()).Terminate();
            }

            sql
                .NewLine(String.Format("DECLARE {0} AS [int];", trancountVar))
                .Append(String.Format("SELECT {0} = @@TRANCOUNT;", trancountVar))
                .Append(String.Format("IF {0} > 0 SAVE TRANSACTION {1};", trancountVar, transaveVar))
                .Append("ELSE BEGIN TRANSACTION;")
                .Append("BEGIN TRY;")
                .NewLine();
        }

        private static void EndEmbeddedTransactionWrapper(StringBuilder sql, 
            string trancountVar, string transaveVar)
        {
            sql
                .NewLine()

                // success:
                //   Commit transaction only if the procedure started the transaction (not outer tran)
                //   and if the transaction is committable (active and not uncommittable).
                .NewLine(String.Format("IF {0} = 0 AND XACT_STATE() = 1 COMMIT TRANSACTION;", trancountVar))
                .Append("END TRY ")
                .Append("BEGIN CATCH;")
                .Append(String.Format("IF {0} = 0 ", trancountVar))
                .Append("BEGIN;")

                // failure:
                //   Rollback transaction only if the procedure started the transaction (not outer tran)
                //   and the transaction is active (either committable or uncommittable).
                .Append(Text.Free.RollbackTransactionCond)
                .Append("END;")
                .Append("ELSE BEGIN;")

                // failure:
                //   Rollback only own (partial) changes, if the procedure did not start the transaction (outer tran)
                //   and the transaction is committable (active and not uncommittable). 
                //   The transaction must be rolled back by the caller who started the (outer) transaction.
                // note:
                //   If the transaction is UNCOMMITTABLE then it only can rollback  as a whole, not partially to the savepoint.
                // attention: 
                //   Partial rollback does not decrease the @@TRANCOUNT value!
                .Append(String.Format("IF XACT_STATE() = 1 ROLLBACK TRANSACTION {0};", transaveVar))
                .Append("END;")
                .Append(Text.Free.Raiserror1)
                .Append(Text.Free.Raiserror2)
                .Append(Text.Free.Raiserror3)
                .Append("END CATCH;");
        }

        #region ToString

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            var paramCount = GetRoot().ExplicitParams.Count;
            var compiled = IsCompiled ? "compiled" : "not compiled";

            if (HasName)
            {
                return String.Format("{0} ({1}) | {2} | {3} param(s)", GetType(), Name, compiled, paramCount);
            }
            else
            {
                return String.Format("{0} | {1} | {2} param(s)", GetType(), compiled, paramCount);
            }

        }

        #endregion

    }

}
