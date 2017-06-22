#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Data;

namespace QueryTalk.Wall
{
    internal static partial class Mapping
    {

        private static void SetTableParamMapping()
        {
            TableParamMapping = new Dictionary<DT, Type[]>();
            TableParamMapping[DT.None] = new Type[] { typeof(View) };
            TableParamMapping[DT.View] = new Type[] { typeof(View) };
            TableParamMapping[DT.Udtt] = new Type[] { typeof(View) };
            TableParamMapping[DT.TableVariable] = new Type[] { typeof(View) };
            TableParamMapping[DT.TempTable] = new Type[] { typeof(View), typeof(DataTable) };
            TableParamMapping[DT.BulkTable] = new Type[] { typeof(DataTable) };
        }

    }
}
