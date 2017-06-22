#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class DeleteChainer : WriteChainer,
        ITop
    {
        internal override string Method 
        { 
            get 
            {
                return Text.Method.Delete;
            } 
        }

        internal DeleteChainer(Chainer prev, string alias)
            : base(prev)
        {
            Query.Clause.Delete = this;
            alias = base.TryGetTableAlias(alias);

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(50)
                    .NewLine(Text.Delete).S()
                    .Append(BuildTop(buildContext))
                    .Append(Filter.Delimit(alias));

                OutputChainer.TryAppendOutput(this, sql, buildContext, buildArgs);
   
                return sql.ToString();
            };
        }
    }
}
