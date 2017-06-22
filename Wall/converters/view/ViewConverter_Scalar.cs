#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    internal static partial class ViewConverter
    {
        internal static View ConvertScalar<T>(T data, Type clrType)
        {
            var type = typeof(T);
            var sql = Text.GenerateSql(50)
                .Append(Text.Select).S();

            AppendScalarColumn(sql, data, clrType);
            ViewColumnInfo column = new ViewColumnInfo(Text.SingleColumnName, type, clrType);
            var view = new View(sql.ToString(), clrType, new ViewColumnInfo[] { column }, 1);

            return view;
        }

    }
}
