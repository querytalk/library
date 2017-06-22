#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    internal class ViewDebuggerProxy
    {
        private View _view;

        public ViewDebuggerProxy(View view)
        {
            if (view == null)
            {
                throw new ArgumentNullException("view");
            }

            _view = view;
        }

        public string Sql
        {
            get
            {
                return _view.Sql;
            }
        }

    }
}
