#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Text.RegularExpressions;

namespace QueryTalk.Wall
{
    internal class AliasColumnPair
    {
        private string _value;

        internal string Alias { get; private set; }
        internal string Column { get; private set; }

        internal AliasColumnPair(string value)
        {
            _value = value;
            Split();
        }

        // split the string value by its first dot, if any
        //  - (1) If string is null or empty, return.
        //  - (2) If string has no dot => alias
        //  - (4) If string has a dot and at least one char after the dot => alias.column
        //  - (3) If string terminates by dot => alias
        internal void Split()
        {
            // (1) null/Empty
            if (String.IsNullOrEmpty(_value))
            {
                return;
            }

            Alias = _value;

            // (2) no dot
            if (!Regex.IsMatch(_value, @"\."))
            {
                return;
            }

            var reg = new Regex(@"^[^\.]*(\.)");
            var match = reg.Match(_value);

            // double check
            if (!match.Success)
            {
                return;
            }

            // alias
            var alias = match.Groups[0].Value;

            // (3) check if the dot is a string terminator
            var columnLength = _value.Length - alias.Length;
            if (columnLength == 0)
            {
                return;
            }

            // (4) here: we can make a clean split (alias, column)

            Column = _value.Substring(alias.Length, columnLength);

            // correct alias: remove terminating dot
            Alias = alias.Remove(alias.Length - 1);
        }
    }
}