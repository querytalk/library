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
    public sealed class UpdateChainer : WriteChainer
    {
        internal override string Method
        {
            get
            {
                return Text.Method.Update;
            }
        }

        internal UpdateChainer(Chainer prev, string alias)
            : base(prev)
        {
            Query.SetUpdate(this);
            alias = base.TryGetTableAlias(alias);
            var aliasDelimited = Filter.Delimit(alias);

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(200);
                var select = GetPrev<SelectChainer>();
                TakeTop(select);
                select.SkipBuild = true;

                sql.NewLine(BuildTop(buildContext, buildArgs))
                    .Append(aliasDelimited).S();

                var columnsObject = Prev.GetPrev<ColumnsChainer>();  
                columnsObject.Build(buildContext, buildArgs);
                columnsObject.SkipBuild = true;

                var values = ProcessValueArrayInliner(buildContext, buildArgs, select.Columns);
                if (values == null)
                {
                    values = select.Columns;  
                }

                var columns = ProcessValueArrayInliner(buildContext, buildArgs, Columns);
                if (columns == null)
                {
                    columns = Columns;  
                }

                sql.NewLine(Text.Set).S();
                var i = 0;
                foreach (var column in columns)
                {
                    if (i > 0)
                    {
                        sql.NewLineIndent(Text.Comma).S();
                    }

                    sql.Append(aliasDelimited)
                        .Append(Text.Dot)
                        .Append(column.Build(buildContext, buildArgs))
                        .Append(Text._Equal_)
                        .Append(values[i].Build(buildContext, buildArgs)).S();

                    ++i;
                }

                OutputChainer.TryAppendOutput(this, sql, buildContext, buildArgs);

                return sql.ToString();
            }; 
        }


        private string BuildTop(BuildContext buildContext, BuildArgs buildArgs)
        {
            return String.Format("{0} {1}",
                Text.Update,
                base.BuildTop(buildContext));
        }

    }
}
