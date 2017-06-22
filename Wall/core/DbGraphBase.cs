#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.ComponentModel;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public abstract class DbGraph<TRoot> : IGraph, IConnectable
        where TRoot : DbRow
    {

        #region Properties

        internal IGraph InnerGraph { get; set; }

        private DbNode _innerNode;

        /// <summary>
        /// Returns the inner node of the graph instance.
        /// </summary>
        /// <param name="context">A semantic context.</param>
        protected DbNode GetInnerNode(SemqContext context)
        {
            if (context == null)
            {
                throw new QueryTalkException(this, QueryTalkExceptionType.NullArgumentInnerException,
                    "context = null", "DbGraphBase.GetInnerNode");
            }

            if (_innerNode != null)
            {
                return _innerNode;
            }
            else
            {
                var innerNode = InnerGraph.Subject; 
                context.SetGraphIndex(innerNode);
                return innerNode;
            }
        }

        /// <summary>
        /// Sets the inner node of the graph instance.
        /// </summary>
        /// <param name="context">A semantic context.</param>
        /// <param name="innerNode">The inner node.</param>
        protected void SetInnerNode(SemqContext context, DbNode innerNode)
        {
            if (context == null)
            {
                throw new QueryTalkException(this, QueryTalkExceptionType.NullArgumentInnerException,
                    "context = null", "DbGraphBase.SetInnerNode");
            }

            _innerNode = innerNode;
            context.SetGraphIndex(_innerNode);
        }

        #endregion

        #region IGraph

        private DbNode _predecessorNode;
        DbNode IGraph.Subject
        {
            get
            {
                if (_predecessorNode != null)
                {
                    return _predecessorNode.Root;
                }
                else
                {
                    return ((IGraph)this).Root.Subject;
                }
            }
        }

        Action<SemqContext, IGraph> IGraph.Translate { get; set; }

        #endregion

        #region IConnectable

        ConnectionKey IConnectable.ConnectionKey
        {
            get
            {
                return ((IGraph)this).Subject.Mapper.GetRoot().ConnectionKey;
            }
        }

        void IConnectable.SetConnectionKey(ConnectionKey connectionKey)
        {
            ((IGraph)this).Subject.Mapper.GetRoot().ConnectionKey = connectionKey;
        }

        void IConnectable.ResetConnectionKey()
        {
            ((IGraph)this).Subject.Mapper.GetRoot().ConnectionKey = null;
        }

        #endregion

        #region Chaining

        private IGraph _next;
        IGraph IGraph.Next
        {
            get
            {
                return _next;
            }
            set
            {
                _next = value;
            }
        }

        private IGraph _prev;
        IGraph IGraph.Prev
        {
            get
            {
                return _prev;
            }
        }

        private IGraph _root;
        IGraph IGraph.Root
        {
            get
            {
                return _root;
            }
        }

        #endregion

        #region Constructors

        #region Base ctors

        internal DbGraph(IGraph prev)
        {
            if (prev != null)
            {
                _root = prev.Root;
                prev.Next = this;
                _prev = prev;
            }
            else
            {
                _root = this;
            }
        }

        internal DbGraph(DbNode node)
            : this((IGraph)null)
        {
            CheckNullAndThrow(Arg(() => node, node));
            _predecessorNode = node;
        }

        #endregion

        #region Graph Leaf

        internal DbGraph(DbNode node, DbNode relatedNode)
            : this(node.Root)
        {
            node = node.Root;
            relatedNode.CheckWithAndThrow("relatedNode", Text.Method.With);
            _predecessorNode = node;

            ((IGraph)this).Translate = (context, predecessor) =>
            {
                context.SetGraphIndex(node);
                SetInnerNode(context, relatedNode);
                Translate.TranslateGraphNode(this, context, _predecessorNode, relatedNode, true);
            };
        }

        internal DbGraph(IGraph graph, DbNode relatedNode)
            : this(graph)
        {
            relatedNode.CheckWithAndThrow("relatedNode", Text.Method.With);
            _predecessorNode = ((IGraph)graph).Subject;

            ((IGraph)this).Translate = (context, predecessor) =>
            {
                SetInnerNode(context, relatedNode);
                Translate.TranslateGraphNode(this, context, _predecessorNode, relatedNode, true);
            };
        }

        #endregion

        #region Graph Tree

        internal DbGraph(DbNode node, IGraph relatedGraph)
            : this(node.Root)
        {
            node = node.Root;
            CheckNullAndThrow(Arg(() => relatedGraph, relatedGraph));
            _predecessorNode = node;
            InnerGraph = relatedGraph;

            ((IGraph)this).Translate = (context, predecessor) =>
            {
                context.SetGraphIndex((DbNode)node);
                var innerNode = GetInnerNode(context);
                Translate.TranslateGraphNode(this, context, _predecessorNode, innerNode);
                Translate.TranslateGraph(relatedGraph.Root, context, this);
            };
        }

        internal DbGraph(IGraph graph, IGraph relatedGraph)
            : this(graph)
        {
            CheckNullAndThrow(Arg(() => relatedGraph, relatedGraph));
            _predecessorNode = ((IGraph)graph).Subject;
            InnerGraph = relatedGraph;

            ((IGraph)this).Translate = (context, predecessor) =>
            {
                var innerNode = GetInnerNode(context);
                Translate.TranslateGraphNode(this, context, _predecessorNode, innerNode);
                Translate.TranslateGraph(relatedGraph.Root, context, this);
            };
        }

        #endregion

        #endregion

        #region Supporting methods

        /// <summary>
        /// Gets the member name of the member expression.
        /// </summary>
        /// <typeparam name="T">The returning type of the delegate method.</typeparam>
        /// <param name="expr">A lambda expression.</param>
        /// <param name="argument">The argument value.</param>
        internal protected Tuple<string, object> Arg<T>(Expression<Func<T>> expr, object argument)
        {
            if (expr == null)
            {
                throw new QueryTalkException(this, QueryTalkExceptionType.NullArgumentInnerException,
                    "expr = null", "DbGraphBase.Arg");
            }

            var body = ((MemberExpression)expr.Body);
            return Tuple.Create(Regex.Replace(body.Member.Name, "^_", String.Empty), argument);
        }

        /// <summary>
        /// Performs a null reference check.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        internal protected void CheckNullAndThrow(Tuple<string, object> argument)
        {
            if (argument == null)
            {
                throw new QueryTalkException(this, QueryTalkExceptionType.NullArgumentInnerException,
                    "argument = null", "DbGraphBase.CheckNullAndThrow");
            }

            if (argument.Item2.IsUndefined())
            {
                var exception = new QueryTalkException("DbGraphBase.ctor", QueryTalkExceptionType.ArgumentNull,
                    String.Format("{0} = undefined", argument.Item1), Text.Method.With);
                if (_predecessorNode != null)
                {
                    exception.ObjectName = _predecessorNode.Name;
                }

                throw exception;
            }
        }

        #endregion

        #region Overriden

        /// <summary>
        /// Not intended for public use.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            if (_innerNode != null)
            {
                return String.Format("graph: {0} -> {1}", ((IGraph)this).Subject.Name, _innerNode.Name);
            }
            else
            {
                return String.Format("graph: {0}", ((IGraph)this).Subject.Name);
            }
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
        public new Type GetType()
        {
            return base.GetType();
        }

        #endregion

    }
}


