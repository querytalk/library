#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    internal class ClrMappingInfo
    {
        // CLR type supported by QueryTalk
        internal Type ClrType { get; set; }

        // every QueryTalk CLR type can be related to many SQL data types                
        internal DT[] DTypes { get; set; }

        // default SQL data type definition         
        internal DataType DefaultDataType { get; set; }

        // SqlDataReader GetXXX method 
        internal string SqlDataReaderGetMethodName { get; set; }

        // if a data type is a value type and GetValue method is used, then unboxing has to be done in IL dynamic method
        internal bool IsUnbox { get; set; }

        // nullable "version" of value type OR reference type, if not value type         
        internal Type NullableType { get; set; }

        // for single ctor parameter: if given then a column property should be IL created by passing the parameter of this type to the instance constructor
        internal Type CtorParameterType { get; set; }

        // generates JSON output
        internal Func<object, string> ToJson { get; set; }  

        internal ClrMappingInfo()
        { }
    }
}
