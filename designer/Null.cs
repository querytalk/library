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
        /// Represents a NULL value in the database.
        /// </summary>
        public static Value Null
        {
            get
            {
                return new Value();
            }
        }

        /// <summary>
        /// Represents a DEFAULT value for the column in the database.
        /// </summary>
        public static Value Default
        {
            get
            {
                var special = new Value();
                special.SetSpecialValue(QueryTalk.Wall.Text.Default);
                return special;
            }
        }

        /// <summary>
        /// Represents DEFAULT VALUES for the columns in the database.
        /// </summary>
        public static Value DefaultValues
        {
            get
            {
                var special = new Value();
                special.SetSpecialValue(QueryTalk.Wall.Text.DefaultValues);
                return special;
            }
        }

    }
}
