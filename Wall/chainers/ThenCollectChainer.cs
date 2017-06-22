#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Linq;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class ThenCollectChainer : Chainer, IQuery, ISelectable,
        IThenCollect,
        IColumns, 
        IInsert
    {
        internal override string Method
        {
            get
            {
                return Text.Method.ThenCollect;
            }
        }

        private Column[] _values;

        #region ISelectable

        Column[] ISelectable.Columns
        {
            get
            {
                return _values;
            }
        }

        int ISelectable.ColumnCount
        {
            get
            {
                if (_values != null)
                {
                    return _values.Count();
                }
                else
                {
                    return 0;
                }
            }
        }

        bool ISelectable.IsEmpty
        {
            get
            {
                return false;   // not allowed to be empty
            }
        }

        // not used
        string[] ISelectable.Variables
        {
            set { return; }
        }

        #endregion 

        internal ThenCollectChainer(Chainer prev, Column[] values)
            : base(prev)
        {
            CheckNullOrEmptyAndThrow(Argc(() => values, values));
            _values = values;

            // no build method: handled by InsertChainer object
        }

    }
}
