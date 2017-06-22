#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    internal partial class JsonConverter
    {
        private static string ConvertValue(Value data)
        {
            if (data.IsUndefined())
            {
                return Text.ClrNull;
            }

            return ConvertScalar(data.Original, data.ClrType);
        }
    }
}
