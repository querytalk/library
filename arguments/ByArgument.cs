#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Diagnostics;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Represents an argument of the .By method.
    /// </summary>
    public sealed class ByArgument : Argument
    {

        #region Properties

        // non-delimited alias, always given
        private string _alias;
        internal string Alias
        {
            get
            {
                return _alias;
            }
        }

        // non-delimited column
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _column;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal string Column
        {
            get
            {
                return _column;
            }
        }

        // column of .Of object
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DbColumn _dbColumn;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal DbColumn DbColumn
        {
            get
            {
                return _dbColumn;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal ByType JoinType { get; private set; }

        #endregion
     
        #region System.Int32

        internal ByArgument(System.Int32 arg)
            : base(arg)
        {
            _alias = arg.ToString();
            JoinType = Wall.ByType.Alias;
            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ByArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ByArgument(System.Int32 arg)
        {
            return new ByArgument(arg);
        }

        #endregion

        #region System.String

        internal ByArgument(System.String arg)
            : base(arg)
        {
            if (String.IsNullOrEmpty(arg))
            {
                arg = null;
            }

            if (CheckNull(Arg(() => arg, arg)))
            {
                if (arg.Split(new char[]{ Text.DotChar }).Length > 2)
                {
                    chainException = new QueryTalkException(this,
                        QueryTalkExceptionType.InvalidColumnIdentifier,
                        String.Format("identifier = {0}", arg),
                        Text.Method.Identifier);
                    return;
                }

                var pair = new AliasColumnPair(arg);
                _alias = pair.Alias;
                _column = pair.Column;

                if (_column != null)
                {
                    JoinType = Wall.ByType.NonMappedColumn;
                }
                else
                {
                    JoinType = Wall.ByType.Alias;
                }
            }

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ByArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ByArgument(System.String arg)
        {
            return new ByArgument(arg);
        }

        #endregion

        #region Identifier

        internal ByArgument(Identifier arg)
            : base(arg)
        {
            CtorBody(arg);
            SetArgType(arg);

            if (chainException == null)
            {
                // identifier should consist of alias and column. 
                if (arg.NumberOfParts != 2)
                {
                    chainException = new QueryTalkException(this,
                        QueryTalkExceptionType.InvalidByIdentifier,
                        String.Format("identifier = {0}", arg),
                        Text.Method.Identifier);
                    return;
                }

                _alias = arg.Part1;
                _column = arg.Part2;

                if (_column != null)
                {
                    JoinType = Wall.ByType.NonMappedColumn;
                }
                else
                {
                    JoinType = Wall.ByType.Alias;
                }
            }
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ByArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ByArgument(Identifier arg)
        {
            return new ByArgument(arg);
        }

        #endregion

        #region Of

        internal ByArgument(OfChainer arg)
            : base(arg)
        {
            if (CheckNull(Arg(() => arg, arg)))
            {
                Build = arg.Build;
                _dbColumn = arg.Column;
                _column = arg.Column.ColumnName;
                _alias = arg.Column.OfAlias;
                JoinType = Wall.ByType.MappedColumn;
            }

            SetArgType(arg);
        }

        /// <summary>
        /// Implicitly converts an argument into the object of ByArgument type.
        /// </summary>
        /// <param name="arg">An argument to convert.</param>
        public static implicit operator ByArgument(OfChainer arg)
        {
            return new ByArgument(arg);
        }

        #endregion

    }
}
