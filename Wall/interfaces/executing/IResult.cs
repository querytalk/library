#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    internal interface IResult
    {
        bool Executed { get; }

        int ReturnValue { get; }

        int TableCount { get; }

        int RowCount { get; }

        int AffectedCount { get; }

        string Sql { get; }
    }
}
