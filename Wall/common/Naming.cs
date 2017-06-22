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
    internal static class Naming
    {
        private const string INVALID_REGEX = @"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]";
        private const string UNDERSCORE = "_";
        private const string PREFIX = "A";
        private const string RENAMED = "Renamed";
        private const string RENAMED_REGEX = @"Renamed\d*$";

        // reserved column names used by the library and the mapper application
        private static string[] RESERVED_COLUMN_NAMES = new string[] 
        { 
            "QueryTalk", 
            "NodeID", 
	        "By",
            "Go",
            "GoAsync",
            "GoFunc",
            "Not",
            "Or",
            "Pass",
            "At",
            "GoAt",
            "ToString",
            "Equals",
            "GetType",
            "GetHashCode",
            "GetStatus",
            "Designer",
            "Invokers"
        };

        internal static bool IsValidClrName(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            // check invalid characters
            if (Regex.IsMatch(name, INVALID_REGEX))
            {
                return false;
            }

            // first char is a number
            if (Regex.IsMatch(name, @"^[\d]"))
            {
                return false;
            }

            return true;
        }

        // Get valid CLR name using the following rules:
        //   (1) A CLR name must be CLR compliant (2.4.2 of the C# specification).
        //   (2) Spaces are replaced by a single underscore.
        //   (3) Non-compliant characters are removed.
        //   (4) Two or more consecutive underscore characters are replaced with a single one.
        //   (5) If the name consists only of the underscore, it is replaced by 'A'.
        //   (6) If the first character is a number or underscore, the letter 'A' is appended as a prefix.
        //   (7) The first character of a property should be a capital letter (Admin.IsCapitalizationOn = true).
        //   (8) Ending underscores are removed.
        //
        // note:
        //   This method should never fail, should never throw an exception and should always return a valid CLR name.
        internal static string GetClrName(string name)
        {
            // check null/empty (NOT LIKELY TO HAPPEN)
            if (String.IsNullOrWhiteSpace(name))
            {
                return PREFIX;
            }

            // (1) A CLR name must be CLR compliant (2.4.2 of the C# specification).
            var regex = new Regex(INVALID_REGEX);

            // (2) Spaces are replaced by a single underscore.
            string name2 = Regex.Replace(name, @"\s+", UNDERSCORE);

            // (3) Non-compliant characters are removed. 
            name2 = regex.Replace(name2, "");

            // check empty
            if (String.IsNullOrEmpty(name2))
            {
                return PREFIX;
            }

            // (4) Two or more consecutive underscore characters are replaced with a single one.
            name2 = Regex.Replace(name2, "_{2,}", UNDERSCORE);

            // (5) If the name consists only of the underscore, it is replaced by 'A'.
            if (name2 == UNDERSCORE)
            {
                return PREFIX;
            }

            // (6) If the first character is a number or underscore, the letter 'A' is appended as a prefix. 
            if (Regex.IsMatch(name2, @"^[\d_]"))
            {
                return PREFIX + name2.TrimEnd('_');
            }

            var name3 = name2;

            // (7) The first character is a capital letter 
            if (Admin.IsCapitalizationOn)
            {
                name3 = name2.Substring(0, 1).ToUpperInvariant();
                if (name2.Length > 1)
                {
                    name3 = name3 + name2.Substring(1);
                }
            }

            // (8) Remove ending underscore and return.
            return name3.TrimEnd('_');
        }

        // Check the validity of a column's CLR name.
        internal static bool IsValidClrColumnName(string name, string tableName, string databaseName = null)
        {
            // shouldn't be the same as the database name 
            if (databaseName != null && name == databaseName)
            {
                return false;  
            }
            // shouldn't be the same as the table name 
            else if (name == tableName)
            {
                return false;  
            }
            // renamed name is valid
            if (Regex.IsMatch(name, RENAMED_REGEX))
            {
                return true;
            }
            // QueryTalk prefix is not allowed
            else if (Regex.IsMatch(name, "^QueryTalk_"))
            {
                return false;  
            }
            // shouldn't be reserved
            else if (RESERVED_COLUMN_NAMES.Contains(name))
            {
                return false;  
            }

            return true; 
        }

        // provide a unique CLR name for every column inside the set of columns
        internal static string GetClrColumnName(string name, HashSet<string> names, string tableName, string databaseName = null)
        {
            var name2 = GetClrName(name);

            var renameIndex = 1;
            while (true)
            {
                if (!names.Add(name2) || !IsValidClrColumnName(name2, tableName, databaseName))
                {
                    name2 = Rename(name2, renameIndex++);
                }
                else
                {
                    break;
                }
            }

            return name2;
        }

        // Provides a renamed name in a form <name>_Rename<ix>.
        private static string Rename(string name, int ix)
        {
            var renReg = new Regex(RENAMED_REGEX);
            bool isRenamed = renReg.IsMatch(name);

            if (ix == 1)
            {
                if (isRenamed)
                {
                    return renReg.Replace(name.TrimEnd('_'), RENAMED);
                }
                else
                {
                    return String.Format("{0}{1}", name.TrimEnd('_'), RENAMED);
                }
            }
            else
            {
                if (isRenamed)
                {
                    return String.Format("{0}{1}", renReg.Replace(name.TrimEnd('_'), RENAMED), ix);
                }
                else
                {
                    return String.Format("{0}{1}{2}", name.TrimEnd('_'), RENAMED, ix);
                }
            }
        }

    }
}
