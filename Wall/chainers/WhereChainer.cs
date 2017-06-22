#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class WhereChainer : ConditionChainer, IQuery, INonSelectView, IViewAllowed, 
        IWhereAnd,
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
                return Text.Where;
            }
        }

        internal override string Method
        {
            get
            {
                return Text.Method.Where;
            }
        }

        internal WhereChainer(Chainer prev, Expression expression, PredicateGroup predicateGroup = null) 
            : base(prev, expression, predicateGroup)
        {
            Query.Clause.Wheres.Add(this); 
        }

        internal WhereChainer(Chainer prev, ScalarArgument argument1, ValueScalarArgument argument2, bool equality,
            PredicateGroup predicateGroup = null) 
            : base(prev, argument1, argument2, equality, predicateGroup)
        {
            Query.Clause.Wheres.Add(this); 
            chainMethod = equality ? Text.Method.Where : Text.Method.WhereNot;
        }

        // .WhereExists
        internal WhereChainer(Chainer prev, INonSelectView nonSelectView, bool exists, PredicateGroup predicateGroup = null)
            : base(prev, nonSelectView, exists, predicateGroup)
        {
            Query.Clause.Wheres.Add(this); 
            chainMethod = exists ? Text.Method.WhereExists : Text.Method.WhereNotExists;
        }
    }
}
