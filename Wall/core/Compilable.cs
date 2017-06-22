#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Text;

namespace QueryTalk.Wall
{
    // Compilable object containing SQL code that can be reused without recompilation. 
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public class Compilable : Chainer, IDesignerRoot
    {

        #region Properties

        internal enum ObjectType 
        { 
            View,           
            Procedure,        
            StoredProc,       
            Snippet,      
            Proxy,          
            SqlBatch,     
            Mapper       
        }

        private ObjectType _compilableType = ObjectType.Procedure;  // default compilable type
        internal ObjectType CompilableType
        {
            get
            {
                return _compilableType;
            }
        }

        /// <summary>
        /// A name of the compilable object.
        /// </summary>
        public string Name
        {
            get
            {
                return GetRoot().Name;
            }
        }

        internal bool HasName
        {
            get
            {
                return Name != null && Name != Text.NotAvailable;
            }
        }

        private string _sql = null;
        /// <summary>
        /// A compiled SQL code.
        /// </summary>
        public string Sql
        {
            get
            {
                return _sql;
            }
            protected set
            {
                _sql = value;
            }
        }

        internal bool compiled = true;   
        /// <summary>
        /// An indicator telling if an object is compiled in the construction (desing) time.
        /// </summary>
        public bool IsCompiled
        {
            get
            {
                return compiled;
            }
        }

        internal Chainer LastNode
        {
            get
            {
                return Prev;
            }
        }

        #endregion

        #region IDesignerRoot

        Designer IDesignerRoot.GetRoot()
        {
            return base.GetRoot();
        }

        #endregion

        #region Constructor

        internal Compilable(Chainer prev, ObjectType compilableType, bool isInternal = false) 
            : base(prev)
        {
            _compilableType = compilableType;
            var root = GetRoot();
            root.CompilableType = compilableType;

            if (root.ConnectionKey != null && !isInternal)
            {
                root.ConnectionKey = null;
            }

            if (compilableType == ObjectType.View)
            {
                root.IsEmbeddedTransaction = false;
            }
        }

        #endregion

        #region BuildChain

        // main chain build method
        internal string BuildChain(BuildContext buildContext, BuildArgs buildArgs)
        {
            var sql = Wall.Text.GenerateSql(250);

            foreach (var statement in GetRoot().Statements)
            {
                if (statement.SkipBuild)
                {
                    continue;
                }

                var firstObject = statement.First;

                // add .Select after .OrderBy (if missing)
                var orderBy = firstObject.GetNext<OrderByChainer>();
                if (orderBy != null && !(orderBy.Next is SelectChainer))
                {
                    var next = orderBy.Next;
                    var cselect = new SelectChainer(orderBy, new Column[] { }, false);  
                    cselect.SetNext(next);
                }

                if (statement.Build != null)
                {
                    buildContext.Current = firstObject;
                    var append = statement.Build(buildContext, buildArgs);
                    sql.Append(append).S();
                }
                else
                {
                    BuildSimpleStatement(buildContext, buildArgs, sql, firstObject);
                }
            }

            Terminate(sql);

            if (sql.Length == 1 && sql[0] == Text.TerminatorChar)
            {
                return String.Empty;
            }

            return sql.ToString();
        }

        // builds simple statement
        private static void BuildSimpleStatement(
            BuildContext buildContext,
            BuildArgs buildArgs,
            StringBuilder sql, 
            Chainer firstObject)
        {
            var node = firstObject;

            bool terminate = false;
            while (!terminate)
            {
                node.BuildNode(null, buildContext, buildArgs, sql);

                if (node.EndOfStatement)
                {
                    terminate = true;
                }

                node = node.Next;
            }
        }

        private void Terminate(StringBuilder sql)
        {
            sql = sql.TrimEnd();

            if (CompilableType != ObjectType.View 
                && !(Prev is CommentChainer      
                    || Prev is LabelChainer))   
            {
                sql.TerminateSingle();
            }
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            if (HasName)
            {
                return String.Format("{0} ({1})", GetType(), Name);
            }
            else
            {
                return String.Format("{0}", GetType());
            }
        }

        #endregion

    }
}
