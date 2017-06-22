#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    // represents a column of the data view
    internal class ViewColumnInfo
    {
        internal string ColumnName { get; set; }     
        internal Type ColumnType { get; set; }               
        internal Type ClrType { get; set; }           
        internal DataType DataType { get; set; }        
        private Nullable<bool> _nullable = true;    
        internal bool IsNullable
        {
            get
            {
                return _nullable        
                    ?? (ColumnType.IsClass || ColumnType.IsNullable());
            }
        }

        // with CLR type
        internal ViewColumnInfo(string propertyName, Type propertyType, Type clrType, Nullable<bool> nullable = null)
        {
            ColumnName = propertyName;
            ColumnType = propertyType;
            ClrType = clrType;
            var info = Mapping.ClrMapping[ClrType];
            DataType = info.DefaultDataType;
            _nullable = nullable;
        }

        // with SQL type
        internal ViewColumnInfo(string propertyName, DataType dbType, Nullable<bool> nullable = null)
        {
            ColumnName = propertyName;
            DataType = dbType;
            _nullable = nullable;
        }
    }

}
