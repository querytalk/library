#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {
        /// <summary>
        /// <para>Specifies a one-part SQL identifier.</para>
        /// </summary>
        /// <param name="name">Is a one-part identifier.</param>
        /// <param name="splitByDot">If true then the column name with a dot will be treated as a compound name consisting of a table alias and column name.</param>
        public static Identifier Identifier(string name, bool splitByDot = true)
        {
            return new Identifier(name, splitByDot); 
        }

        /// <summary>
        /// <para>Specifies a two-part SQL identifier.</para>
        /// </summary>
        /// <param name="part1">Is the first part of a 2-part identifier, either a schema name or a table alias.</param>
        /// <param name="part2">Is the second part of a 2-part identifier, either a table name or a column name.</param>
        public static Identifier Identifier(string part1, string part2)
        {
            return new Identifier(part1, part2); 
        }

        /// <summary>
        /// <para>Specifies a three-part SQL identifier.</para>
        /// </summary>
        /// <param name="part1">Is the first part of a 3-part identifier.</param>
        /// <param name="part2">Is the second part of a 3-part identifier.</param>
        /// <param name="part3">Is the third part of a 3-part identifier.</param>
        public static Identifier Identifier(string part1, string part2, string part3)
        {
            return new Identifier(part1, part2, part3); 
        }

        /// <summary>
        /// <para>Specifies a fourth-part SQL identifier.</para>
        /// </summary>
        /// <param name="part1">Is the first part of a 4-part identifier.</param>
        /// <param name="part2">Is the second part of a 4-part identifier.</param>
        /// <param name="part3">Is the third part of a 4-part identifier.</param>
        /// <param name="part4">Is the fourth part of a 4-part identifier.</param>
        public static Identifier Identifier(string part1, string part2, string part3, string part4)
        {
            return new Identifier(part1, part2, part3, part4);
        }
    }
}
