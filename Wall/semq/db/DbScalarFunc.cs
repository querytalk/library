#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Linq;
using System.ComponentModel;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Represents a scalar function in a database.
    /// </summary>
    public abstract class DbScalarFunc : DbNode
    { 
        /// <summary>
        /// Initializes a new instance of the DbScalarFunc class.
        /// </summary>
        protected DbScalarFunc()
          : base((DbNode)null)
        { }

        /// <summary>
        /// Executes the scalar function returning the value of a specified type.
        /// </summary>
        /// <typeparam name="T">The type of a value to return.</typeparam>
        /// <param name="client">A client assembly.</param>
        /// <param name="arguments">Are the arguments to pass.</param>
        protected T GoFunc<T>(System.Reflection.Assembly client, params ParameterArgument[] arguments)
        {
            return PublicInvoker.Call<T>(() =>
            {
                var root = Mapper.GetRoot();

                DbMapping.CreateParams(root, this);

                var parameters = String.Join(",", root.AllParams.Select(p => p.Name).ToArray());

                // build
                string sql = Text.GenerateSql(100)
                    .NewLine(Text.Select).S()
                    .Append(Map.Name.Sql)
                        .EncloseLeft()
                        .Append(parameters)
                        .EncloseRight()
                    .Append(Text._As_)
                        .Append(Text.LeftSquareBracket)
                        .Append(Text.SingleColumnName)
                        .Append(Text.RightSquareBracket)
                    .Terminate()
                    .ToString();

                Mapper.SetSql(sql);

                var cpass = new PassChainer(Mapper, arguments);
                var connectable = Reader.GetConnectable(client, cpass);
                return Reader.LoadTable<Row<T>>(connectable, null, true).ToValue<T>();
            });
        }

        /// <summary>
        /// Specifies a list of arguments which are to be passed to the scalar function. The parameter values have to be passed in the order of parameters as declared in the scalar function.
        /// </summary>
        /// <param name="arguments">Are the arguments to pass.</param>
        protected Udf Pass(params FunctionArgument[] arguments)
        {
            DbMapping.PrepareFunctionArguments(arguments, this);
            return new Udf(Map.Name, arguments);
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
