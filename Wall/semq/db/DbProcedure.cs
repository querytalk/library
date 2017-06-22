#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Linq;
using System.ComponentModel;
using System.Reflection;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Represents a stored procedure in a database.
    /// </summary>
    public abstract class DbProcedure : DbNode, IExecutable 
    {
        /// <summary>
        /// Initializes a new instance of the DbProcedure class.
        /// </summary>
        protected DbProcedure()
          : base((DbNode)null)
        { }

        internal ParameterArgument[] Arguments { get; private set; }

        /// <summary>
        /// Specifies a list of arguments which are to be passed to the stored procedure. The parameter values have to be passed in the order of parameters as declared in the stored procedure.
        /// </summary>
        /// <param name="arguments">Are the arguments to pass.</param>
        protected PassChainer Pass(params ParameterArgument[] arguments)
        {
            if (arguments == null)
            {
                arguments = new ParameterArgument[] { Designer.Null };
            }

            BuildProc(arguments);
            return new PassChainer(this);
        }

        /// <summary>
        /// Executes the procedure returning a dynamic result with all the result sets from the data source.
        /// </summary>
        /// <param name="client">A client assembly.</param>
        /// <param name="arguments">Are the arguments to pass.</param>
        protected Result Go(Assembly client, params ParameterArgument[] arguments)
        {
            return PublicInvoker.Call<Result>(() =>
            {
                BuildProc(arguments);
                var cpass = new PassChainer(Mapper, arguments);
                var connectable = Reader.GetConnectable(client, this, cpass);
                return Reader.LoadAll(connectable);
            });
        }

        /// <summary>
        /// Executes the procedure returning a strongly typed result set.
        /// </summary>
        /// <param name="client">A client assembly.</param>
        /// <param name="arguments">Are the arguments to pass.</param>
        protected Result<T> Go<T>(Assembly client, params ParameterArgument[] arguments)          
        {
            return PublicInvoker.Call<Result<T>>(() =>
            {
                BuildProc(arguments);
                var cpass = new PassChainer(Mapper, arguments);
                var connectable = Reader.GetConnectable(client, this, cpass);
                return Reader.LoadTable<T>(connectable, null);
            });
        }

        internal void BuildProc(ParameterArgument[] arguments)
        {
            Arguments = arguments;

            var root = Mapper.GetRoot();
            DbMapping.CreateParams(root, this);

            string parameters = null;
            if (arguments != null && arguments.Length > 0)
            {
                var i = 0;
                parameters = String.Join(",",
                    root.AllParams
                        .Select(p =>
                        {
                            var s = p.Name;

                            if (arguments[i] == null)
                            {
                                arguments[i] = Designer.Null;
                            }

                            if (arguments[i].IsArgumentOutput)
                            {
                                s = String.Format("{0} {1}", s, Text.Output);
                            }

                            ++i;
                            return s;
                        }
                    ));
            }

            string sql = Text.GenerateSql(100)
                .NewLine(Text.Exec).S()
                .Append(Text.Reserved.ReturnValueOuterParam).Append(Text._Equal_)
                .Append(Map.Name.Sql).S()
                .Append(parameters).Terminate()
                .NewLine(Text.Set).S()  // SET @_ri = @_ro;
                    .Append(Text.Reserved.ReturnValueInnerParam)
                    .Append(Text._Equal_)
                    .Append(Text.Reserved.ReturnValueOuterParam)
                .Terminate()
                .ToString();

            Mapper.SetSql(sql);
        }

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
