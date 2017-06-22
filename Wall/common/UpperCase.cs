#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    internal static class UpperCase
    {
        internal static string ToUpperCase(this SortOrder sortOrder)
        {
            return sortOrder.ToString().ToUpper();
        }

        internal static string ToUpperCase(this Designer.PartOfDate partOfDate)
        {
            return partOfDate.ToString().ToUpper();
        }
    }
}
