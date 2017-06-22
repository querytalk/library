#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.ComponentModel;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Represents a table-valued function in a database. 
    /// </summary>
    public abstract class DbTableFunc<TNode> : DbTable<TNode>, IFunction
        where TNode : DbRow
    {

        #region Public/Protected

        /// <summary>
        /// Specifies a list of arguments which are to be passed to the table-valued function. The parameter values have to be passed in the order of parameters as declared in the table-valued function.
        /// </summary>
        /// <param name="arguments">Are the arguments to pass.</param>
        protected DbTableFunc<TNode> Pass(params FunctionArgument[] arguments)
        {
            DbMapping.PrepareFunctionArguments(arguments, this);
            _arguments = arguments;
            return this;
        }

        /// <summary>
        /// Executes the table-valued function returning a strongly typed result set.
        /// </summary>
        /// <param name="client">A client assembly.</param>
        /// <param name="arguments">Are the arguments to pass.</param>
        protected Result<TNode> GoFunc(System.Reflection.Assembly client, params FunctionArgument[] arguments)
        {
            return PublicInvoker.Call<Result<TNode>>(() =>
            {
                _arguments = arguments;
                var context = new SemqContext((DbNode)this);
                var query = ((ISelect)((ISemantic)this).Translate(context, null))
                    .Select();

                Mapper.SetSql(((IEndProc)query).EndProcInternal().Sql);

                var connectable = Reader.GetConnectable(client, Mapper);
                return Reader.LoadTable<TNode>(connectable, null, true);
            });
        }

        #endregion

        private FunctionArgument[] _arguments = new FunctionArgument[] { };

        /// <summary>
        /// Initializes a new instance of the DbTableFunc class.
        /// </summary>
        protected DbTableFunc()
            : base(null)
        { }

        #region IFunction

        FunctionArgument[] IFunction.Arguments
        {
            get
            {
                return _arguments;
            }
        }

        Udf IFunction.GetUdf()
        {
            DbMapping.PrepareFunctionArguments(_arguments, this);
            return new Udf(Map.Name, _arguments);
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
