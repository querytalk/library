#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class InjectChainer : EndChainer, IBegin
    {
        // allowed inliners
        private readonly DT[] _inliners = { DT.InSql, DT.InSnippet };
        private readonly DT[] _sqlInliners = { DT.InSql };
        private readonly DT[] _snippetInliners = { DT.InSnippet };

        internal override string Method
        {
            get
            {
                return Text.Method.Inject;
            }
        }

        internal InjectChainer(Chainer prev, string sqlOrInliner)
            : base(prev)
        {
            var root = GetRoot();
            var inliner = root.TryGetVariable(sqlOrInliner, out chainException, Variable.SearchType.Inliner);
            chainException = null;  // reset exception

            if (inliner == null)
            {
                Build = (buildContext, buildArgs) =>
                {
                    return Text.GenerateSql(200)
                        .NewLine(sqlOrInliner)
                        .TerminateSingle()
                        .ToString();
                };
            }
            // inliner
            else
            {
                Build = (buildContext, buildArgs) =>
                {
                    ParameterArgument inlinerArgument = buildArgs.Executable.GetInlinerArgument(inliner.Name);
                    if (inlinerArgument == null)
                    {
                        return null;
                    }

                    if (inliner.DT == DT.InSql)
                    {
                        if (inlinerArgument.DT != DT.InSql)
                        {
                            TryThrow(inliner.DT.InvalidInlinerException(GetType().Name, inliner.Name, _sqlInliners));
                        }

                        string sql2 = inliner.Name;
                        if (buildArgs.Executable != null)
                        {
                            sql2 = (string)inlinerArgument.Value;
                        }

                        return Text.GenerateSql(500)
                            .NewLine(sql2)
                            .TerminateSingle()
                            .ToString();

                    }
                    else if (inliner.DT == DT.InSnippet)
                    {
                        if (inlinerArgument.DT != DT.InSnippet)
                        {
                            buildContext.TryTakeException(inliner.DT.InvalidInlinerException(GetType().Name,
                                inliner.Name, _snippetInliners));
                            return null;
                        }

                        string sql = inliner.Name;

                        if (buildArgs.Executable != null)
                        {
                            var snippet = (Snippet)inlinerArgument.Value;

                            if (snippet != null)
                            {
                                TryTake(snippet);
                                buildContext.ParamRoot = GetRoot();
                                sql = snippet.Build(buildContext, buildArgs);
                            }
                            else
                            {
                                sql = null;     // it is allowed that snippet is not passed
                            }
                        }

                        return sql;
                    }
                    else
                    {
                        chainException = inliner.DT.InvalidInlinerException(GetType().Name,
                            inliner.Name, _inliners);
                        chainException.ObjectName = root.Name;
                        chainException.Method = Text.Method.Exec;
                        throw chainException;
                    }
                };
            }
        }

        internal InjectChainer(Chainer prev, Snippet snippet)
            : base(prev)
        {
            CheckNullAndThrow(Arg(() => snippet, snippet));
            GetRoot().TakeVariables(snippet);

            Build = (buildContext, buildArgs) =>
            {
                return snippet.Build(buildContext, buildArgs);
            };
        }

    }
}
