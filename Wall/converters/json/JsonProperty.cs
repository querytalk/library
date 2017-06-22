#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    internal class JsonProperty
    {
        internal string PropertyName { get; set; }

        internal Type ClrType { get; set; }

        internal IPropertyAccessor Accessor { get; set; }

        internal JsonProperty(string propertyName, Type clrType, IPropertyAccessor accessor)
        {
            PropertyName = propertyName;
            ClrType = clrType;
            Accessor = accessor;
        }
    }

}
