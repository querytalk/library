#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;

namespace QueryTalk.Wall
{
    // base class for InsertChainer, UpdateChainer, DeleteChainer classes
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public abstract class WriteChainer : EndChainer, IQuery, IWritable, IOutput,
        ITop
    {
        internal Column[] Columns { get; set; }

        #region IOutput

        Column[] IOutput.OutputColumns { get; set; }

        TableArgument IOutput.OutputTarget { get; set; }

        #endregion

        internal WriteChainer(Chainer prev)
            : base(prev)
        { }

        internal Column[] ProcessValueArrayInliner(BuildContext buildContext, BuildArgs buildArgs, Column[] values)
        {
            if (values.Length == 1 && values[0].Original is System.String)
            {
                var name = (System.String)values[0].Original;
                Variable variable = buildContext.ParamRoot.TryGetVariable(name, out chainException);

                if (variable.IsInliner())
                {
                    // inliner type check
                    if (variable.DT != DT.InColumn)
                    {
                        buildContext.TryTakeException(variable.DT.InvalidInlinerException(GetType().Name,
                            variable.Name, new[] { DT.InColumn }));
                        return null;
                    }

                    string arg2 = variable.Name;

                    if (buildArgs.Executable != null)
                    {
                        ParameterArgument inlinerArgument = buildArgs.Executable.GetInlinerArgument(variable.Name);

                        if (inlinerArgument.Value != null)
                        {
                            if (inlinerArgument.Value is System.String[])
                            {
                                var args = new List<Column>();
                                foreach (var column in (System.String[])inlinerArgument.Value)
                                {
                                    args.Add(column);
                                }
                                return args.ToArray();
                            }
                            else if (inlinerArgument.Value is Column[])
                            {
                                return (Column[])inlinerArgument.Value;
                            }
                        }
                        else
                        {
                            buildContext.TryTakeException(new QueryTalkException(this,
                                QueryTalkExceptionType.InlinerArgumentNull,
                                String.Format("{0} = null", arg2)));
                            return null;
                        }
                    }
                }
            }

            return null;
        }

        internal string TryGetTableAlias(string alias)
        {
            TableChainer ctable = GetPrevTable(Statement.StatementIndex);

            if (alias == null)
            {
                if (ctable != null && ctable is FromChainer)
                {
                    return ctable.Alias.Name;  
                }
            }
            else
            {
                while (ctable != null)
                {
                    if (ctable != null && ctable.Alias.Name == alias)
                    {
                        return alias;  
                    }

                    ctable = ctable.GetPrevTable(Statement.StatementIndex);
                }
            }

            Throw(QueryTalkExceptionType.TargetTableNotFound, String.Format("target alias = {0}", alias ?? Text.ClrNull));

            return null;
        }
    }
}
