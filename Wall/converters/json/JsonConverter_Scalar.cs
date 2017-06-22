#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Text;

namespace QueryTalk.Wall
{
    internal partial class JsonConverter
    {
        private static string ConvertScalar(object data, Type clrType)
        {
            var info = Mapping.ClrMapping[clrType];
            var json = new StringBuilder();
            json.Append(info.ToJson(data));
            return json.ToString();
        }
    }
}
