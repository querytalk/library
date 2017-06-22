#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    internal static partial class ViewConverter
    {
        internal static View ConvertValue(Value data)
        {
            var sql = Text.GenerateSql(50)
                .Append(Text.Select).S();

            ViewColumnInfo column;
            Type dataType = typeof(string);     // default data type (used when data = null)

            if (data.IsUndefined())
            {
                AppendNullColumn(sql);
                column = new ViewColumnInfo(Text.SingleColumnName, dataType, dataType);
            }
            else
            {
                dataType = data.ClrType;
                AppendScalarColumn(sql, data.Original, dataType);
                column = new ViewColumnInfo(Text.SingleColumnName, data.ClrType, data.ClrType);
            }

            return new View(sql.ToString(), dataType, new ViewColumnInfo[] { column }, 1);
        }

    }
}
