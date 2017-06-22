#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {
        /// <summary>
        /// Declares a variable.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="variable">A variable name.</param>
        /// <param name="dataType">A data type definition of a variable.</param>
        public static DeclareChainer Declare(this IAny prev, string variable, DataType dataType)
        {
            return new DeclareChainer((Chainer)prev, variable, dataType);
        }

        /// <summary>
        /// Declares a variable using the CLR-SQL mapping.
        /// </summary>
        /// <typeparam name="T">A CLR type mapped to a certain SQL type.</typeparam>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="variable">A variable name.</param>
        public static DeclareChainer Declare<T>(this IAny prev, string variable)
        {
            return new DeclareChainer((Chainer)prev, variable, typeof(T)); 
        }

        /// <summary>
        /// Declares a variable inferring its data type from a specified argument.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="variable">A variable name.</param>
        /// <param name="argument">An argument which provides a CLR type mapped to a certain SQL type.</param>
        public static DeclareChainer Declare(this IAny prev, string variable, DeclareArgument argument)
        {
            return new DeclareChainer((Chainer)prev, variable, argument);
        }

        /// <summary>
        /// Declares a variable of a specified user-defined type.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="variable">A variable name.</param>
        /// <param name="userDefinedType">The identifier of the user-defined type.</param>
        public static DeclareChainer Declare(this IAny prev, string variable, Identifier userDefinedType)
        {
            return new DeclareChainer((Chainer)prev, variable, DT.Udt, new TableArgument(userDefinedType));
        }

        /// <summary>
        /// Declares a table variable of a specified user-defined table type.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="variable">A variable name.</param>
        /// <param name="userDefinedType">A user-defined table type.</param>
        public static DeclareChainer DeclareTable(this IAny prev, string variable, TableArgument userDefinedType)
        {
            return new DeclareChainer((Chainer)prev, variable, DT.Udtt, userDefinedType);
        }

    }
}
