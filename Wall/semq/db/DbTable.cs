#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Diagnostics;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Represents a database table or view.
    /// </summary>
    public abstract class DbTable<TRoot> : DbNode, ISemantic, IPredicate, IRelation, IOpenView, ISemqToSql 
        where TRoot : DbRow
    {

        #region Public/Protected

        /// <summary>
        /// Returns a set of all rows in the data source.
        /// </summary>
        public virtual Result<TRoot> Go()
        {
            return Go(System.Reflection.Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// Returns a set of all rows in the data source.
        /// </summary>
        /// <param name="client">A client assembly.</param>
        protected Result<TRoot> Go(System.Reflection.Assembly client)
        {
            return PublicInvoker.Call<Result<TRoot>>(client, (ca) =>
            {
                try
                {
                    var node = (DbNode)this;

                    // for reusability - !
                    var subject = node.RootWithBrokenChain ?? node.Root;

                    var query = ((ISemantic)subject).Translate(new SemqContext(subject), null);
                    query = ((ISelect)query).Select();
                    var proc = ((IEndProc)query).EndProcInternal();
                    var connectable = Reader.GetConnectable(ca, subject, proc);
                    var result = Reader.LoadTable<TRoot>(connectable, null, true);

                    subject.Recover();

                    return result;
                }
                catch (QueryTalkException ex)
                {
                    Loader.TryThrowInvalidSqlOperationException(ex, Name, Text.Method.Go);
                    throw;
                }
            });
        }

        /// <summary>
        /// Returns a set of all rows in the data source asynchronously.
        /// </summary>
        /// <param name="onCompleted">Is a delegate method that is invoked when the asynchronous operation completes.</param>
        /// <returns>The object of the asynchronous operation.</returns>
        public Async<Result<TRoot>> GoAsync(Action<Result<TRoot>> onCompleted = null)
        {
            return PublicInvoker.Call<Async<Result<TRoot>>>(Assembly.GetCallingAssembly(), (ca) =>
            {
                try
                {
                    var node = (DbNode)this;
                    var subject = node.Root;
                    var query = ((ISemantic)subject).Translate(new SemqContext(subject), null);
                    query = ((ISelect)query).Select();
                    var proc = ((IEndProc)query).EndProcInternal();
                    var connectable = Reader.GetConnectable(ca, subject, proc);
                    connectable.OnAsyncCompleted = onCompleted;
                    return Reader.LoadTableAsync<TRoot>(connectable, null);
                }
                catch (QueryTalkException ex)
                {
                    Loader.TryThrowInvalidSqlOperationException(ex, Name, Text.Method.GoAsync);
                    throw;
                }
            });
        }

        /// <summary>
        /// A NOT predicate operator.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DbTable<TRoot> Not
        {
            get
            {
                ((IPredicate)this).HasNot = !((IPredicate)this).HasNot;
                return this;
            }
        }

        /// <summary>
        /// An OR predicate operator.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public DbTable<TRoot> Or
        {
            get
            {
                if (_predicates.Count == 0                          // first predicate cannot begin with .Or
                    || ((IPredicate)this).HasNot                    // .Not operator cannot precede .Or
                    || ((IPredicate)this).PredicateGroup != null    // first predicate inside the predicate group cannot begin with .Or
                    )
                {
                    throw new QueryTalkException("DbTable.Or", QueryTalkExceptionType.InvalidOr,
                        null, Text.Method.Or).SetObjectName(Name);
                }

                ((IPredicate)this).HasOr = true;
                return this;
            }
        }

        /// <summary>
        /// Specifies the foreign key that is to be used in relation. 
        /// The foreign key should be explicitly given when there is more than one relation between two tables.
        /// </summary>
        /// <param name="foreignKey">A column that is part of a foreign key.</param>
        public DbTable<TRoot> By(DbColumn foreignKey)
        {
            if (foreignKey == null)
            {
                throw new QueryTalkException(".By", QueryTalkExceptionType.ArgumentNull, "foreign key", Text.Method.By);
            }

            ((IRelation)this).FK = foreignKey.ColumnID;

            return this;
        }

        #endregion

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Predicate> _predicates = new List<Predicate>();

        /// <summary>
        /// Initializes a new instance of the DbTable class.
        /// </summary>
        /// <param name="prev"></param>
        protected DbTable(DbNode prev)
            : base(prev)
        { }

        #region IRelation

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DB3 IRelation.FK { get; set; }

        #endregion

        #region IPredicate

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        List<Predicate> IPredicate.Predicates
        {
            get
            {
                return _predicates;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Predicate IPredicate.Predicate { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool IPredicate.HasNot { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool IPredicate.HasOr { get; set; }

        void IPredicate.AddPredicate(Predicate predicate)
        {
            _predicates.Add(predicate);
        }

        void IPredicate.AddPredicate(PredicateType predicateType, DbNode sentence)
        {
            _predicates.Add(new Predicate(this, predicateType, true, LogicalOperator.And, (ISemantic)sentence));
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool IPredicate.HasPredicate
        {
            get
            {
                return _predicates.Count > 0;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        PredicateGroup IPredicate.PredicateGroup { get; set; }

        void IPredicate.SetPredicateGroup(PredicateGroupType predicateGroupType)
        {
            PredicateGroup.SetPredicateGroup(this, predicateGroupType);
        }

        void IPredicate.ResetPredicateGroup()
        {
            ((IPredicate)this).PredicateGroup = null;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        int IPredicate.PredicateGroupLevel { get; set; }

        #endregion

        #region ISemantic

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool ISemantic.IsQuery
        {
            get
            {
                return Prev != null
                    || Next != null
                    || ((IPredicate)this).HasPredicate;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DbNode ISemantic.Subject
        {
            get
            {
                return Root;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DbNode ISemantic.RootSubject { get; set; }

        Chainer ISemantic.Translate(SemqContext context, DbNode predecessor)
        {
            return Translate.TranslateNode(context, predecessor, this);
        }

        #endregion

        #region Not browsable

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Type GetType()
        {
            return base.GetType();
        }

        #endregion

    }

}
