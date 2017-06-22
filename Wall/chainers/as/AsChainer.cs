#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    // Represents a central alias object used for the table or column aliasing.
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public class AliasAs : Chainer, INonPredecessor
    {
        internal override string Method
        {
            get
            {
                return Text.Method.FromAs;
            }
        }

        // non-delimited string value of alias object
        private string _value;
        internal string Name
        {
            get
            {
                return _value;
            }
        }

        // indicates a table alias
        internal bool IsTableAlias
        {
            get
            {
                return this is ITableAlias;
            }
        }

        // return true if alias name is null or empty
        internal bool IsUndefined
        {
            get
            {
                return String.IsNullOrEmpty(_value);
            }
        }

        internal bool IsDefault { get; private set; }

        // ctor for non-chaining alias instantiation
        internal AliasAs(string alias)
            : this(null, alias)
        { }

        // ctor for non-chaining alias instantiation (in SEMQ)
        internal AliasAs(int index)
            : this(null, index.ToString(), true)
        { }

        // regular ctor
        //   isInternal : indicates that the alias is assigned internally - as an integer number (e.g. by SEMQ)
        internal AliasAs(Chainer prev, string alias, bool isInternal = false)
            : base(prev)
        {
            if (IsTableAlias && alias == null)
            {
                IsDefault = true;
            }

            if (!isInternal && Common.CheckIdentifier(alias) != IdentifierValidity.RegularIdentifier)
            {
                chainException = new QueryTalkException("Alias.ctor",
                    QueryTalkExceptionType.InvalidAlias, ArgVal(() => alias, alias));
            }

            _value = alias;
        }

        internal AliasAs(Chainer prev, int index)
            : base(prev)
        {
            _value = index.ToString();
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return _value;
        }
    }
}
