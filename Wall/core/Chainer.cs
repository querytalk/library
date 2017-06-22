#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Diagnostics;

namespace QueryTalk.Wall
{
    // Base object of all objects of method chaining.
    /// <summary>
    /// Not intended for public use.
    /// </summary>
    public abstract class Chainer : IConnectable
    {

        #region IConnectable

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ConnectionKey IConnectable.ConnectionKey
        {
            get
            {
                return GetRoot().ConnectionKey;
            }
        }

        void IConnectable.SetConnectionKey(ConnectionKey connectionKey)
        {
            GetRoot().ConnectionKey = connectionKey;
        }

        void IConnectable.ResetConnectionKey()
        {
            GetRoot().ConnectionKey = null;
        }

        #endregion

        #region Properties

        // next object of the object chain
        private Chainer _next;
        internal Chainer Next
        {
            get
            {
                return _next;
            }
        }

        internal void SetNext(Chainer next)
        {
            _next = next;
        }

        // previous object of the object chain
        private Chainer _prev;
        internal Chainer Prev
        {
            get
            {
                return _prev;
            }
        }

        // root object of the object chain
        private Chainer _root;
        internal Chainer Root
        {
            get
            {
                return _root;
            }
        }

        // returns root object
        internal Designer GetRoot()
        {
            if (Root is Designer)
            {
                return (Designer)Root;
            }
            else if (Root is IDesignerRoot)
            {
                return ((IDesignerRoot)Root).GetRoot();
            }
            else
            {
                throw new QueryTalkException("Chainer.GetRoot", QueryTalkExceptionType.InvalidRootInnerException,
                    String.Format("root type = {0}", Root.GetType()));
            }
        }

        private Statement _statement;
        internal Statement Statement
        {
            get
            {
                return _statement;
            }
        }

        // query statement
        //   attention:
        //      Nullcheck is not performed. Use this property ONLY for the objects that are part of the query.
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Query Query
        {
            get
            {
                return _statement.ToQuery();
            }
        }

        // returns true if this object is part of a query
        internal virtual bool IsQuery
        {
            get
            {
                return (Statement != null) && (Statement is Query);
            }
        }

        internal QueryPart QueryPart { get; set; }

        // true if this object is the last object of the statement
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal bool EndOfStatement
        {
            get
            {
                return
                    _statement != null
                        &&
                    (_next == null
                        ||
                    _next.Statement == null
                        ||
                    _next.Statement.First == Next);
            }
        }

        // build method that performs SQL code generation
        private Func<BuildContext, BuildArgs, string> _build = null;

        internal Func<BuildContext, BuildArgs, string> Build
        {
            get
            {
                return (buildContext, buildArgs) =>
                {
                    if (_build != null)
                    {
                        return _build(buildContext, buildArgs);
                    }
                    else
                    {                     
                        return null; 
                    }
                };
            }
            set
            {
                if (_build == null || !(Original is Value))
                {
                    _build = value;
                }
            }
        }

        // if true then the chain object does not build
        internal bool SkipBuild { get; set; }

        // original value of an argument implicitly converted into other object
        internal object Original { get; set; }

        // used for mapped tables which are implicitly converted into TableArgument arguments
        internal Table TableArgument { get; set; }

        internal bool IsDistinct { get; set; }

        #region Top

        private long? _top = null;
        internal long? Top
        {
            get
            {
                return _top;
            }
        }

        private double? _topPercent = null;
        internal double? TopPercent
        {
            get
            {
                return _topPercent;
            }
        }

        private Variable _topVariable;

        private bool _isWithTies = false;
        internal bool IsWithTies
        {
            get
            {
                return _isWithTies;
            }
        }

        internal void SetTop(long top, bool isWithTies)
        {
            _top = top;
            _isWithTies = isWithTies;
        }

        internal void SetTop(double top, bool isWithTies)
        {
            _topPercent = top;
            _isWithTies = isWithTies;
        }

        internal void SetTop(Variable variable, bool isWithTies)
        {
            _topVariable = variable;
            _isWithTies = isWithTies;
        }

        // copy top data
        internal void TakeTop(Chainer from)
        {
            _top = from._top ?? _top;
            _topPercent = from._topPercent ?? _topPercent;
            _topVariable = from._topVariable ?? _topVariable;
            _isWithTies = from._isWithTies ? true : _isWithTies;

            if (from._top != null || from._topPercent != null)
            {
                from._top = null;
                from._topPercent = null;
                from._topVariable = null;
                from._isWithTies = false;
            }
        }

        internal string BuildTop(BuildContext buildContext)
        {
            if (_top == null && _topPercent == null && _topVariable == null)
            {
                return null;
            }

            buildContext.TryAddParamToConcatRoot(_topVariable);

            StringBuilder sql = new StringBuilder(20)
                .Append(Text.Top)
                .Append(Text.LeftBracket);

            if (_topVariable != null)
            {
                sql.Append(_topVariable.Name)
                    .Append(Text.RightBracket);

                if (!_topVariable.DT.IsInteger())
                {
                    sql.S().Append(Text.Percent);
                }
            }
            else
            {
                if (_top != null)
                {
                    sql.Append(_top)
                        .Append(Text.RightBracket);
                }
                else
                {
                    sql.Append(Mapping.BuildUnchecked(_topPercent))
                        .Append(Text.RightBracket).S()
                        .Append(Text.Percent);
                }
            }

            if (_isWithTies)
            {
                sql.S().Append(Text.WithTies);
            }

            return sql.S().ToString();
        }

        #endregion

        internal Executable Executable { get; set; }

        // SQL keyword related to this object
        internal string chainKeyword = null;
        internal virtual string Keyword
        {
            get
            {
                return chainKeyword;
            }
        }

        // name of a chained method related to this object
        internal string chainMethod = null;
        internal virtual string Method
        {
            get
            {
                return chainMethod;
            }
        }

        // indicates whether a snippet node contains an injectable variable
        internal bool IsConcat { get; set; }

        // if true then strings are used as values (and not as identifiers)
        internal bool UseStringAsValue { get; set; }

        // used in SEMQ to support consecutive expression grouping
        internal ExpressionGroupType ExpressionGroupType { get; set; }

        // debugging value to be shown
        private object _debugValue = Text.Free.DebuggingValueNotAvailable;

        /// <summary>
        /// Gets the debugging value of this instance.
        /// </summary>
        internal protected object DebugValue
        {
            get
            {
                return _debugValue;
            }
            set
            {
                _debugValue = value;
            }
        }

        #endregion

        #region Statement Manager

        // handles statement creation and assignment
        internal void StatementManager(Chainer prev, bool force)
        {
            Action create = () =>
            {
                // query statement
                if (this is IQuery)
                {
                    _statement = new Query(this);
                }
                // common statement
                else
                {
                    _statement = new Statement(this);
                }
            };

            // forced creation
            if (force && _statement == null)
            {
                create();
                return;
            }

            // check the condition to create a new statement
            if ((
                this is IBegin              // object is the beginning of the statement
                    &&
                !Query.IsPrevCte(this))     // previous object must not be CTE
                    &&
                !(prev is UnionChainer)     // previous object must not be UNION
                )
            {
                create();
            }
            // check the condition to include this object as part of the statement
            else
            {
                if ((
                    this is IQuery          // object is part of the query
                        ||
                    this is IStatement)     // or is part of simple statement
                        &&
                    prev.Statement != null  // the statement, to include this object into, should exists
                    )
                {
                    _statement = prev.Statement;
                }
            }
        }

        #endregion

        #region Constructor

        internal Chainer(Chainer prev)
        {
            // non-root:
            if (prev != null)
            {
                _root = prev.Root;
                prev._next = this;
                _prev = prev;
                StatementManager(prev, false);
            }
            // root:
            else
            {
                if (this is INonPredecessor)
                {
                    _root = this;
                }
                else
                {
                    Throw(QueryTalkExceptionType.PredecessorNull, null);
                }
            }
        }

        #endregion

        #region Exception handling

        internal QueryTalkException chainException;  
        internal QueryTalkException Exception
        {
            get
            {
                return chainException;
            }
        }

        internal void TryThrow(string method = null)
        {
            TryThrow(chainException, method);
        }

        internal void TryThrow(QueryTalkException exception, string method = null)
        {
            if (exception != null)
            {
                chainException = exception;
                if (chainException.Method == null)
                {
                    chainException.Method = method ?? Method;
                }
                chainException.ObjectName = chainException.ObjectName ?? ((Root is IName) ? ((IName)Root).Name : null);

                throw chainException;
            }
        }

        internal void TryThrow(BuildContext buildContext, string method = null)
        {
            if (buildContext == null)
            {
                return;
            }

            TryThrow(buildContext.Exception, method);
        }

        internal void Throw(QueryTalkExceptionType exceptionType, string arguments, string method = null)
        {
            TryThrow(new QueryTalkException(this, exceptionType, arguments), method ?? Method);
        }

        internal void TryTakeException(QueryTalkException from)
        {
            if (chainException == null && from != null)           
            {
                chainException = from;
            }
        }

        internal bool CheckNull(Tuple<string, object> arg, string method = null)
        {
            if (arg.Item2.IsUndefined(true))
            {
                chainException = new QueryTalkException(this, QueryTalkExceptionType.ArgumentNull, String.Format("{0}", arg.Item1));
                chainException.Method = method ?? Method;
                return false;
            }

            return true;
        }

        internal void CheckNullAndThrow(Tuple<string, object> arg, string method = null)
        {
            if (arg.Item2.IsUndefined(true)   
                ||
                arg.Item2 is System.String && String.IsNullOrEmpty((string)arg.Item2) 
                )
            {
                Throw(QueryTalkExceptionType.ArgumentNull, String.Format("{0}", arg.Item1), method ?? Method);
            }
        }

        internal bool CheckNullOrEmpty(Tuple<string, ICollection> arg, string method = null)
        {
            if (arg.Item2.IsUndefined(true) || arg.Item2.Count == 0)
            {
                chainException = new QueryTalkException(this, QueryTalkExceptionType.CollectionNullOrEmpty, String.Format("{0}", arg.Item1));
                chainException.Method = method ?? Method;
                return false;
            }

            return true;
        }

        internal void CheckNullOrEmptyAndThrow(
            Tuple<string, ICollection> arg, string method = null)
        {
            if (arg.Item2.IsUndefined(true) || arg.Item2.Count == 0)
            {
                Throw(QueryTalkExceptionType.CollectionNullOrEmpty, String.Format("{0}", arg.Item1), method ?? Method);
            }
        }

        internal void CreateException(QueryTalkExceptionType exceptionType, string arguments, string method = null)
        {
            chainException = new QueryTalkException(this, exceptionType, arguments);
            chainException.Method = method ?? Method;
        }

        internal bool CheckNullOrEmptyAlias(string alias, string method = null)
        {
            if (String.IsNullOrEmpty(alias))
            {
                chainException = new QueryTalkException(this, QueryTalkExceptionType.AliasNullOrEmpty, "alias = null/empty",
                    method ?? Method);
                return false;
            }

            return true;
        }

        internal void CheckNullOrEmptyAliasAndThrow(string alias, string method = null)
        {
            if (String.IsNullOrEmpty(alias))
            {
                Throw(QueryTalkExceptionType.AliasNullOrEmpty, "alias = null/empty", method ?? Method);
            }
        }

        #endregion

        #region TryTake (only partially used)

        internal void TryTake(Chainer from, params TakeProperty[] properties)
        {
            if (from == null)
            {
                return;
            }

            if (properties == null || properties.Length == 0)
            {
                TryTakeException(from.Exception);
            }

            foreach (var property in properties)
            {
                if (property == TakeProperty.Exception)
                {
                    TryTakeException(from.Exception);
                }

                if (property == TakeProperty.Build)
                {
                    Build = from.Build;
                }
            }
        }

        internal void TryTakeAll(Chainer from)
        {
            TryTake(from, TakeProperty.Exception, TakeProperty.Build);
        }

        #endregion

        #region MapRoot

        // sets internal node to the root (used in CRUD to relate root object with the mapping)
        internal void SetRootMap(DB3 nodeID)
        {
            GetRoot().Node = new InternalNode(nodeID);
        }

        #endregion

        #region Concatenator

        internal string BuildConcatenator(BuildContext buildContext, BuildArgs buildArgs, Concatenator arg)
        {
            IsConcat = true;

            if (this is Argument)
            {
                buildContext.Current.IsConcat = true;
            }

            return arg.Build(buildContext, buildArgs);
        }

        #endregion

        #region Helper methods

        // Seeks for the object T moving from this instance backwards.
        internal T GetPrev<T>() 
        {
            var node = this;
            while (node != null)
            {
                if (node is T)
                {
                    return (T)(object)node;
                }
                node = node.Prev;
            }

            return default(T);
        }

        // Seeks for the object T moving from this instance forward.
        internal T GetNext<T>() 
        {
            var node = this;
            while (node != null)
            {
                if (node is T)
                {
                    return (T)(object)node;
                }
                node = node.Next;
            }

            return default(T);
        }

        // Seeks for the previous TableChainer in the same query statement moving from the previous object backwards.
        internal TableChainer GetPrevTable(int statementIndex)
        {
            if (_prev == null || Statement == null)
            {
                return null;
            }

            var ctable = _prev.GetPrev<TableChainer>();

            // condition:
            if (ctable != null && ctable.Statement.StatementIndex == statementIndex)
            {
                return ctable;
            }

            return null;
        }

        #endregion

        #region Arg<T>
        
        internal static Tuple<string, object> Arg<T>(Expression<Func<T>> expr, object arg)
        {
            var body = ((MemberExpression)expr.Body);
            return Tuple.Create(Regex.Replace(body.Member.Name, "^_", String.Empty), arg);
        }

        internal static Tuple<string, ICollection> Argc<T>(Expression<Func<T>> expr, ICollection collection)
        {
            var body = ((MemberExpression)expr.Body);
            return Tuple.Create(Regex.Replace(body.Member.Name, "^_", String.Empty), collection);
        }

        internal static string ArgVal<T>(Expression<Func<T>> expr, object value)
        {
            var body = ((MemberExpression)expr.Body);
            return String.Format("{0} = {1}", Regex.Replace(body.Member.Name, "^_", String.Empty), value);
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

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool Equals(object objA, object objB)
        {
            return Equals(objA, objB);
        }

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool ReferenceEquals(object objA, object objB)
        {
            return ReferenceEquals(objA, objB);
        }

        #endregion

    }
}
