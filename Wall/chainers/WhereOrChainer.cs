#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class WhereOrChainer : ConditionChainer, IQuery, INonSelectView, IViewAllowed, 
        IWhereOr,
        IGroupBy,
        IOrderBy,
        ISelect,
        IDelete
    {
        internal override string Keyword
        {
            get
            {
                return Text.GenerateSql(10).Indent().Append(Text.Or).ToString();
            }
        }

        internal override string Method
        {
            get
            {
                return Text.Method.Or;
            }
        }

        internal WhereOrChainer(Chainer prev, Expression expression, PredicateGroup predicateGroup = null) 
            : base(prev, expression, predicateGroup)
        {
            Query.Clause.Wheres.Add(this); 
        }

        internal WhereOrChainer(Chainer prev, ScalarArgument argument1, ScalarArgument argument2, bool equality,
            PredicateGroup predicateGroup = null) 
            : base(prev, argument1, argument2, equality, predicateGroup)
        {
            Query.Clause.Wheres.Add(this); 
            chainMethod = equality ? Text.Method.Or : Text.Method.OrNot;
        }

        // .OrExists
        internal WhereOrChainer(Chainer prev, INonSelectView nonSelectView, bool exists, PredicateGroup predicateGroup = null)
            : base(prev, nonSelectView, exists, predicateGroup)
        {
            Query.Clause.Wheres.Add(this); 
            chainMethod = exists ? Text.Method.OrExists : Text.Method.OrNotExists;
        }
    }
}
