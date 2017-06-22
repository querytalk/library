#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace QueryTalk.Wall
{
    // Represents a SQL statement.
    internal class Statement
    {

        private int _statementIndex = 0;
        internal int StatementIndex
        {
            get
            {
                return _statementIndex;
            }
        }

        // first object of the statement
        internal Chainer First { get; set; }

        // the root object of the compilable object whose part is this statement
        internal Designer Root
        {
            get
            {
                if (First != null)
                {
                    return First.TryGetRoot();
                }
                else
                {
                    return null;
                }
            }
        }

        internal Func<BuildContext, BuildArgs, string> Build { get; set; }

        private QueryTalkException _exception;
        internal QueryTalkException Exception
        {
            get
            {
                return _exception;
            }
        }

        internal bool SkipBuild { get; set; }

        internal Statement(Chainer firstObject)
        {
            First = firstObject;
            Root.Statements.Add(this);
            _statementIndex = Root.GetStatementIndex();
        }

        private static bool IsDefaultAlias(string alias)
        {
            if (alias == null)
            {
                return false;
            }
            else
            {
                // a default alias is a number
                return Regex.IsMatch(alias.ToUpperCS(), @"^-*\d+$");
            }
        }

        // collection of all valid aliases in the build context
        private List<AliasAs> _aliases = new List<AliasAs>();

        internal void CheckAlias(AliasAs alias)
        {
            // check alias name
            if (IsDefaultAlias(alias.Name))
            {
                _exception = new QueryTalkException("Statement.CheckAlias", QueryTalkExceptionType.InvalidTableAlias,
                    String.Format("table alias = {0}", alias.Name));
                return;
            }

            // alias should be unique in the build context
            if (_aliases.Select(a => a.Name).Contains(alias.Name))
            {
                _exception = new QueryTalkException("Statement.CheckAlias", QueryTalkExceptionType.TableAliasDuplicate,
                    String.Format("table alias = {0}", alias));
                return;
            }

            _aliases.Add(alias);
        }

        internal void RemoveAlias(AliasAs alias)
        {
            _aliases.Remove(alias);
        }

        internal Query ToQuery()
        {
            return this as Query;
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("[{0}.{1}]", First.GetType().Name, _statementIndex);
        }

    }
}
