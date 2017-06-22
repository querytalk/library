#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Diagnostics;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents a non-parameterized SQL query. (This class has no public constructor.)
    /// </summary>
    [DebuggerTypeProxy(typeof(Wall.ViewDebuggerProxy))]
    
    public sealed class View : Compilable, IExecutable,
        IScalar,
        IViewAs,
        IGo,
        IConnectBy
    {

        #region Properties

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal override string Method
        {
            get
            {
                return Text.Method.EndView;
            }
        }

        internal new Query Query
        {
            get
            {
                return GetRoot().Query;
            }
        }

        internal Query OuterQuery { get; private set; }

        internal Type DataType { get; private set; }

        internal bool IsValidDataView
        {
            get
            {
                return Columns != null && Columns.Length > 0;
            }
        }

        internal ViewColumnInfo[] Columns { get; private set; }

        internal int RowCount { get; private set; }

        internal TableArgument UserDefinedType { get; private set; }

        #endregion

        #region Public

        /// <summary>
        /// Sets a user-defined type of a table which will receive data passed by this view.
        /// </summary>
        /// <param name="userDefinedType">A user-defined type name.</param>
        public View ForUdt(TableArgument userDefinedType)
        {
            UserDefinedType = userDefinedType;
            return this;
        }

        #endregion

        #region Constructor

        internal View(Chainer prev)
            : base(prev, ObjectType.View)
        {
            Sql = BuildChain(new BuildContext(this), new BuildArgs(null));

            Build = (buildContext, buildArgs) =>
            {
                return Sql;
            };

            CheckAndThrow();
        }

        // internal ctor for open views
        //   outerQuery: 
        //     one-level higher outer query whose table is this open view
        internal View(IOpenView prev, Query outerQuery)
            : base((Chainer)prev, ObjectType.View)
        {
            CheckAndThrow();

            OuterQuery = outerQuery;

            compiled = false;

            Build = (buildContext, buildArgs) =>
            {
                Query.Master = outerQuery.Master;
                Query.Level = outerQuery.Level + 1;

                // build with own build context, but take ParamRoot and exec context (buildArgs) from the caller
                return BuildChain(new BuildContext(this, buildContext.ParamRoot), buildArgs);
            };
        }

        internal View(IOpenView prev)
            : base((Chainer)prev, ObjectType.View)
        {
            CheckAndThrow();

            compiled = false;

            Build = (buildContext, buildArgs) =>
            {
                // build with own build context, but take ParamRoot and exec context (buildArgs) from the caller
                return BuildChain(new BuildContext(this, buildContext.ParamRoot), buildArgs);
            };
        }

        internal View(string sql, Type dataType, ViewColumnInfo[] columns, int rowsCount)
            : base(new InternalRoot(), ObjectType.View)
        {
            Sql = sql;
            DataType = dataType;
            Columns = columns;
            RowCount = rowsCount;

            Build = (buildContext, buildArgs) =>
                {
                    return Sql;
                };
        }

        #endregion

        #region CheckAndThrow

        private void CheckAndThrow()
        {
            if (GetRoot().HasParams)
            {
                Throw(QueryTalkExceptionType.InvalidView, null, Text.Method.Param);
            }
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            var viewType = DataType != null ? " | contains data" : null;

            if (HasName)
            {
                return String.Format("{0}({1}){2}", GetType(), Name, viewType);
            }
            else
            {
                return String.Format("{0}{1}", GetType(), viewType);
            }
        }

        #endregion

    }
}
