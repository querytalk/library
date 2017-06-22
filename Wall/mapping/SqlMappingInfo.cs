#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    internal class SqlMappingInfo
    {
        // CLR type
        internal Type ClrType { get; set; }

        // downgrade types only: e.g. System.Int32 can be passed to [bigint] data type
        internal Type[] ClrSubTypes { get; set; }

        // SQL representation of a data type
        internal string Sql { get; set; }

        // plain name of a data type (not enclosed, without size or precision/scale components) 
        internal string SqlPlain { get; set; }

        // db type may have size definition (length, precision - e.g. varchar, varbinary, decimal) or not (e.g. bit, int)
        internal Mapping.SizeType SizeType { get; set; }

        // max size of length definition (if 0 then max size is not defined)
        internal int MaxSize { get; set; }

        // size of MAX value as number of characters in text representation
        internal int MaxSizeInChars { get; set; }

        // SQL representation of type inferred by value
        internal Func<object, string> SqlByValue { get; set; }

        // data type group 
        internal Mapping.DataTypeGroup DataTypeGroup { get; set; }

        // check the size (length, precision, scale)
        // <value, length, precisio, scale, is_not_greater_than_declared>
        internal Func<object, int, int, int, bool> CheckSize { get; set; }     

        // builds CLR value returning its SQL representation 
        internal Func<object, DataType, string> Build { get; set; } 

        internal SqlMappingInfo()
        { }
    }
}
