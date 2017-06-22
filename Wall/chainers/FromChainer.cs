#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public class FromChainer : TableChainer, IBegin, IQuery, INonSelectView, IViewAllowed,
        IFrom,
        ISemqJoin,
        IFromAs,
        IInnerJoin,
        ILeftOuterJoin,
        IRightOuterJoin,
        IFullJoin,
        ICrossJoin,
        ICrossApply,
        IOuterApply,
        IPivot,
        IWhere,
        IGroupBy,
        IOrderBy,
        ISelect,
        IDelete
    {
        internal override string Keyword  
        { 
            get 
            { 
                return Text.From; 
            } 
        }

        internal override string Method 
        { 
            get 
            { 
                return Text.Method.From; 
            } 
        }

        internal FromChainer(Chainer prev, Table table) 
            : base(prev, table)
        { }

        internal FromChainer(Chainer prev, IOpenView table)
            : base(prev, table)
        { }

        internal string BuildCte(BuildContext buildContext, BuildArgs buildArgs, bool isFirstCte)
        {
            var sql = Text.GenerateSql(500);

            if (isFirstCte)
            {
                sql.NewLine(Text.With).S();
            }
            else
            {
                sql.NewLine(Text.Comma);
            }

            sql.Append(Filter.Delimit(Alias.Name))
                .Append(Text._As_)
                .NewLine(Text.LeftBracket)
                .Append(TableArgument.Build(buildContext, buildArgs))
                .NewLine(Text.RightBracket);

            return sql.ToString();
        }
    }
}
