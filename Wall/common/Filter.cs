#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace QueryTalk.Wall
{
    internal static class Filter
    {
        // Escapes single quotes.
        internal static string Escape(string value)
        {
            return Regex.Replace(value, "'", "''");
        }

        // Encloses with brackets.
        internal static string Enclose(string value)
        {
            return String.Format("({0})", value);
        }

        // Delimits with square brackets.
        internal static string Delimit(string value)
        {
            return String.Format("[{0}]", Regex.Replace(value, @"\]", "]]"));
        }

        // Delimits with square brackets except the asterix (*).
        internal static string DelimitNonAsterix(string value)
        {
            if (value.Trim() != Text.Asterisk)
            {
                return String.Format("[{0}]", Regex.Replace(value, @"\]", "]]"));
            }
            else
            {
                return Wall.Text.Asterisk;
            }
        }

        // Delimits with quotes except null.
        internal static string DelimitQuote(string value)
        {
            if (value != null)
            {
                return String.Format("'{0}'", Escape(value));
            }
            else
            {
                return Text.Null;
            }
        }

        // Delimits with quotes except the asterix.
        internal static string DelimitQuoteNonAsterix(string value)
        {
            if (value.Trim() != Text.Asterisk)
            {
                return String.Format("'{0}'", Escape(value));
            }
            else
            {
                return Text.Asterisk;
            }
        }

        // Delimits (non-asterix) a one- or two-part identifier.
        internal static string DelimitColumnMultiPart(string identifier, out QueryTalkException exception)
        {
            exception = null;

            StringBuilder parts = new StringBuilder();
            string[] names = identifier.Split('.');

            // not allowed:
            if (names.Length > 2)
            {
                exception = new QueryTalkException("Filter.DelimitColumnMultiPart",
                    QueryTalkExceptionType.InvalidColumnIdentifier,
                    String.Format("identifier = {0}", identifier));
                return identifier;
            }

            Array.ForEach(names, name =>
            {
                parts.Append(Filter.DelimitNonAsterix(name) + ".");
            });

            return parts.ToString().TrimEnd('.');
        }

        // Delimits a table multi-part identifier.
        internal static string DelimitTableMultiPart(string identifier, out QueryTalkException exception)
        {
            exception = null;

            StringBuilder parts = new StringBuilder();
            string[] names = identifier.Split('.');

            // not allowed:
            if (names.Length > 4)
            {
                exception = new QueryTalkException("Filter.DelimitMultiPartOrParam",
                    QueryTalkExceptionType.InvalidIdentifier,
                    String.Format("identifier = {0}", identifier));
                return identifier;
            }

            Array.ForEach(names, name =>
            {
                parts.Append(Filter.Delimit(name) + ".");
            });

            return parts.ToString().TrimEnd('.');
        }

        // Delimits a multi-part identifier or param.
        internal static string DelimitMultiPartOrParam(
            string identifier,              
            IdentifierType identifierType,  
            out QueryTalkException exception     
            )      
        {
            exception = null;

            // leave variable/param as is
            if (Variable.TryValidateName(identifier, out exception))
            {
                return identifier;
            }

            if (exception != null)
            {
                return identifier;
            }

            StringBuilder parts = new StringBuilder();
            string[] names = identifier.Split('.');

            if (names.Length > 4)
            {
                exception = new QueryTalkException("Filter.DelimitMultiPartOrParam",
                    QueryTalkExceptionType.InvalidIdentifier,
                    String.Format("identifier = {0}", identifier));
                return identifier;
            }

            if (identifierType == IdentifierType.ColumnOrParam && names.Length > 2)
            {
                exception = new QueryTalkException("Filter.DelimitMultiPartOrParam",
                    QueryTalkExceptionType.InvalidColumnIdentifier,
                    String.Format("identifier = {0}", identifier));
                return identifier;
            }

            // valid column identifier:

            Array.ForEach(names, name =>
            {
                parts.Append(Filter.DelimitNonAsterix(name) + ".");
            });

            return parts.ToString().TrimEnd('.');
        }

        // Delimits a multi-part identifier or param (and sets the columnName argument).
        internal static string DelimitMultiPartOrParam(
            string identifier,             
            IdentifierType identifierType,  
            out QueryTalkException exception,   
            out string columnName
            )
        {
            exception = null;
            columnName = null;

            // leave variable/param as is
            if (Variable.TryValidateName(identifier, out exception))
            {
                return identifier;
            }

            if (exception != null)
            {
                return identifier;
            }

            StringBuilder parts = new StringBuilder();
            string[] names = identifier.Split('.');

            if (names.Length > 4)
            {
                exception = new QueryTalkException("Filter.DelimitMultiPartOrParam",
                    QueryTalkExceptionType.InvalidIdentifier,
                    String.Format("identifier = {0}", identifier));
                return identifier;
            }

            if (identifierType == IdentifierType.ColumnOrParam && names.Length > 2)
            {
                exception = new QueryTalkException("Filter.DelimitMultiPartOrParam",
                    QueryTalkExceptionType.InvalidColumnIdentifier,
                    String.Format("identifier = {0}", identifier));
                return identifier;
            }

            // valid column identifier:

            columnName = names[names.Length - 1];

            Array.ForEach(names, name =>
            {
                parts.Append(Filter.DelimitNonAsterix(name) + ".");
            });

            return parts.ToString().TrimEnd('.');
        }

        // Trims single quotes.
        internal static string TrimSingleQuotes(string value)
        {
            var s1 = Regex.Replace(value, @"^((N')|('))", "");  // trim start
            var s2 = Regex.Replace(s1, @"'$", "");              // trim end
            return s2;
        }

    }
}
