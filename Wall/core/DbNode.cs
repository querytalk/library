#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Represents a SQL object in the database.
    /// </summary>
    public abstract class DbNode : IConnectable
    {
        /// <summary>
        /// A TOP value.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected Nullable<long> QueryTalk_Top { get; set; }

        /// <summary>
        /// A DB3 identifier of this node.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        protected internal DB3 NodeID { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal DbNode SynonymOrThis
        {
            get
            {
                return this.Synonym ?? this;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal DB3 DatabaseID
        {
            get
            {
                return NodeID.DatabaseID;
            }
        }

        // used in late loading
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal DbRow Row { get; set; }

        #region IConnectable

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ConnectionKey IConnectable.ConnectionKey
        {
            get
            {
                return Mapper.GetRoot().ConnectionKey ?? ConnectionKey.CreateMapKey(DatabaseID);
            }
        }

        void IConnectable.SetConnectionKey(ConnectionKey connectionKey)
        {
            Mapper.GetRoot().ConnectionKey = connectionKey;
        }

        void IConnectable.ResetConnectionKey()
        {
            Mapper.GetRoot().ConnectionKey = null;
        }

        #endregion

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private NodeMap _map;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal NodeMap Map
        {
            get
            {
                if (_map == null)
                {
                    _map = DbMapping.GetNodeMap(NodeID);
                }
                return _map;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal string Name
        {
            get
            {
                return Map.Name.Sql;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Mapper _mapper;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Mapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    var root = new InternalRoot(this);
                    _mapper = new Mapper(root);
                }

                return _mapper;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Nullable<long> Top
        {
            get
            {
                return QueryTalk_Top;
            }
        }

        // get designer (unsafe)
        internal NameChainer GetDesigner()
        {
            return new NameChainer(Mapper.Root);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Expression Expression { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal bool HasExpression
        {
            get
            {
                return Expression != null;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal bool IsLast
        {
            get
            {
                return _next == null;
            }
        }

        // table alias set during the translation 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _index = 0;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal int Index 
        {
            get
            {
                return _index;
            }
        }

        internal void ChangeIndex(int index)
        {
            _index = index;
        }

        // index of a related node in graph sequence
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal int RX { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _isFinalizeable = true;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal bool IsFinalizeable
        {
            get
            {
                return _isFinalizeable;
            }
            set
            {
                _isFinalizeable = value;
            }
        }

        #region Reusability

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal bool IsUsed = false;

        internal void SetAsUsed(bool used = true)
        {
            IsUsed = used;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal bool IsAdded { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _canBeReused;

        /// <summary>
        /// Sets this node as reusable.
        /// </summary>
        /// <typeparam name="T">The type of the derived instance of this node.</typeparam>
        internal protected T CanBeReused<T>() where T : DbNode
        {
            _canBeReused = true;
            return (T)this;
        }

        internal void CanBeReused()
        {
            _canBeReused = true;
        }

        internal void CheckReuseAndThrow()
        {
            if (!_canBeReused && IsUsed)
            {
                throw new QueryTalkException("DbNode.CheckReuseAndThrow", QueryTalkExceptionType.NodeReuseDisallowed, 
                    String.Format("node = {0}", Name + " (may not be correct)")); 
            }
        }

        #endregion

        #region Synonym

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal DbNode Synonym { get; set; }

        internal DbNode SetSynonymOrThis()
        {
            SynonymOrThis.ChangeIndex(Index);
            return SynonymOrThis;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal Chainer SynonymQuery { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal bool IsSynonym
        {
            get
            {
                return SynonymQuery != null;
            }
        }

        #endregion

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DbNode _next;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal DbNode Next
        {
            get
            {
                return _next;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DbNode _prev;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal DbNode Prev
        {
            get
            {
                return _prev;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DbNode _root;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal DbNode Root
        {
            get
            {
                if (_prev == null)
                {
                    return this;
                }
                else
                {
                    return _root;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal DbNode QueryRoot { get; set; }

        #region Break and Recover
        // To properly find the root of the broken chain.

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal DbNode RootWithBrokenChain { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DbNode _nextBroken;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DbNode _prevBroken;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DbNode _rootBroken;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<DbNode> _nodes;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal List<DbNode> Nodes
        {
            get
            {
                if (_nodes == null)
                {
                    _nodes = new List<DbNode>();
                }
                return _nodes;
            }
        }

        // break the chain between the node and the next node
        //   attention:
        //     This method provokes serious problems in case of reusability because the subject of the predicate gets replaced with its successor. 
        //     For this reason in order to support reusability the root.Replace method must be used after every execution.
        internal DbNode BreakChain()
        {
            DbNode next = null;
            if (_next != null)
            {
                next = _next;
                next.RootWithBrokenChain = this.RootWithBrokenChain ?? this._root;   
                next._prevBroken = next._prev;
                next._prev = null;
                next._rootBroken = next._root;
                next._root = next;   
            }

            _nextBroken = _next;
            _next = null;
            return next;
        }

        // Recovers the structure of the SEMQ query as it was before the chain breaking.
        // Esential method that makes reusability work properly. 
        internal void Recover()
        {
            // recover nodes
            foreach (var node in _nodes)
            {
                node._RecoverChain();
                node.SetAsUsed(false);
                node.ChangeIndex(0);
                node.IsAdded = false;
            }

            // recover predicates
            foreach (var node in _nodes)
            {
                var defaultPredicate = ((IPredicate)node).Predicates.Where(p => p.IsDefault).FirstOrDefault();
                if (defaultPredicate != null)
                {
                    ((IPredicate)node).Predicates.Remove(defaultPredicate);
                }
            }

            _nodes.Clear();
        }

        private void _RecoverChain()
        {
            // recover chain
            if (_prevBroken != null)
            {
                _prev = _prevBroken;
                _prevBroken = null;
            }
            if (_nextBroken != null)
            {
                _next = _nextBroken;
                _nextBroken = null;
            }
            if (_rootBroken != null)
            {
                _root = _rootBroken;
                _rootBroken = null;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DbNode class.
        /// </summary>
        protected DbNode()
        {
            _root = this;
        }

        /// <summary>
        /// Initializes a new instance of the DbNode class.
        /// </summary>
        /// <param name="prev">The previous node.</param>
        protected DbNode(DbNode prev)
        {
            if (prev != null)
            {
                _root = prev.Root;
                prev._next = this;
                _prev = prev;
            }
            else
            {
                _root = this;
            }
        }

        #endregion

        internal DbNode GetLast()
        {
            var node = _next;
            var last = this;
            while (node != null)
            {
                last = node;
                node = node.Next;
            }
            return last;
        }

        /// <summary>
        /// Determines whether this node is equal to the specified node.
        /// </summary>
        /// <param name="nodeToCompare">Is a specified node to be compared with.</param>
        public bool Equals(DbNode nodeToCompare)
        {
            if (nodeToCompare == null)
            {
                return false;
            }

            return NodeID.Equals(nodeToCompare.NodeID);
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return Name;
        }

    }
}
