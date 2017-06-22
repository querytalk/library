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
        /// Converts the identifier of a user-defined function in the SQL Server into the Udf object.
        /// </summary>
        /// <param name="udf">Is a string identifier of a user-defined function in the SQL Server.</param>
        /// <param name="parameters">Are parameters of a user-defined function.</param>
        public static Udf PassUdf(this string udf, params FunctionArgument[] parameters)
        {
            return new Udf(udf, parameters);
        }

        /// <summary>
        /// Converts the identifier of a user-defined function in the SQL Server into the Udf object.
        /// </summary>
        /// <param name="udf">Is an identifier of a user-defined function.</param>
        /// <param name="parameters">Are parameters of a user-defined function.</param>
        public static Udf PassUdf(this Identifier udf, params FunctionArgument[] parameters)
        {
            return new Udf(udf, parameters);
        }

    }
}
