#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    internal class DbColumnDebuggerProxy
    {
        private DbColumn _column;

        public DbColumnDebuggerProxy(DbColumn column)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }

            _column = column;
        }

        public DbNode Node
        {
            get
            {
                return _column.Parent;
            }
        }

    }
}
