#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;

namespace QueryTalk.Wall
{
    internal static partial class Mapping
    {

        private static void SetInlineMapping()
        {
            InlineMapping = new Dictionary<DT, Type[]>();
            InlineMapping[DT.InTable] = new[] { typeof(System.String), typeof(Identifier) };
            InlineMapping[DT.InColumn] = new[] { typeof(System.String), typeof(Column), typeof(Column[]) };
            InlineMapping[DT.InSql] = new[] { typeof(System.String), typeof(ExecArgument) };
            InlineMapping[DT.InExpression] = new[] { typeof(Expression) };
            InlineMapping[DT.InProcedure] = new[] { typeof(Procedure), typeof(PassChainer) };
            InlineMapping[DT.InSnippet] = new[] { typeof(Snippet) };
            InlineMapping[DT.InStoredProcedure] = new[] { typeof(System.String), typeof(Identifier), typeof(PassChainer), typeof(ExecArgument) };
        }

    }
}
