#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Linq;

namespace QueryTalk.Wall
{

    /// <summary>
    /// QueryTalk's designer types. (Not intended for public use.)
    /// </summary>
    public enum DT : int
    {
        /// <summary>
        /// The type is not specified.
        /// </summary>
        None = 0,                 

        #region SQL Server Scalar (Column) Db Types

        /// <summary>
        /// The bigint data type.
        /// </summary>
        Bigint = 1,

        /// <summary>
        /// The binary data type.
        /// </summary>
        Binary = 2,

        /// <summary>
        /// The bit data type.
        /// </summary>
        Bit = 3,

        /// <summary>
        /// The char data type.
        /// </summary>
        Char = 4,

        /// <summary>
        /// The date data type.
        /// </summary>
        Date = 5,

        /// <summary>
        /// The datetime data type.
        /// </summary>              
        Datetime = 6,

        /// <summary>
        /// The datetime2 data type.
        /// </summary>
        Datetime2 = 7,

        /// <summary>
        /// The datetimeoffset data type.
        /// </summary>      
        Datetimeoffset = 8,

        /// <summary>
        /// The decimal data type.
        /// </summary>
        Decimal = 9,

        /// <summary>
        /// The float data type.
        /// </summary>
        Float = 10,

        /// <summary>
        /// The int data type.
        /// </summary>
        Int = 11,

        /// <summary>
        /// The money data type.
        /// </summary>
        Money = 12,

        /// <summary>
        /// The nchar data type.
        /// </summary>
        NChar = 13,

        /// <summary>
        /// The numeric data type.
        /// </summary>
        Numeric = 14,

        /// <summary>
        /// The nvarchar data type.
        /// </summary>
        NVarchar = 15,

        /// <summary>
        /// The nvarchar(max) data type.
        /// </summary>
        NVarcharMax = 16,

        /// <summary>
        /// The real data type.
        /// </summary>
        Real = 17,

        /// <summary>
        /// The smalldatetime data type.
        /// </summary>
        Smalldatetime = 18,

        /// <summary>
        /// The smallint data type.
        /// </summary>
        Smallint = 19,

        /// <summary>
        /// The smallmoney data type.
        /// </summary>
        Smallmoney = 20,

        /// <summary>
        /// The sql_variant data type.
        /// </summary>
        Sqlvariant = 21,

        /// <summary>
        /// The sysname data type.
        /// </summary>
        Sysname = 22,

        /// <summary>
        /// The time data type.
        /// </summary>
        Time = 23,

        /// <summary>
        /// The timestamp data type.
        /// </summary>        
        Timestamp = 24,

        /// <summary>
        /// The tinyint data type.
        /// </summary>
        Tinyint = 25,

        /// <summary>
        /// The uniqueidentifier data type.
        /// </summary>
        Uniqueidentifier = 26,

        /// <summary>
        /// The varbinary data type.
        /// </summary>
        Varbinary = 27,

        /// <summary>
        /// The varbinary(max) data type.
        /// </summary>
        VarbinaryMax = 28,

        /// <summary>
        /// The varchar data type.
        /// </summary>
        Varchar = 29,

        /// <summary>
        /// The varchar(max) data type.
        /// </summary>
        VarcharMax = 30,

        /// <summary>
        /// The xml data type.
        /// </summary>
        Xml = 31,

        // DEPRECATED BY MICROSOFT & NOT SUPPORTED BY SEMQ
        /// <summary>
        /// The text data type.
        /// </summary>
        Text = 33,

        // DEPRECATED BY MICROSOFT & NOT SUPPORTED BY SEMQ
        /// <summary>
        /// The ntext data type.
        /// </summary>
        NText = 34,

        // DEPRECATED BY MICROSOFT & NOT SUPPORTED BY SEMQ 
        /// <summary>
        /// The image data type.
        /// </summary>          
        Image = 35,

        /// <summary>
        /// The rowversion data type.
        /// </summary>               
        Rowversion = 36,

        /// <summary>
        /// The user-defined data type.
        /// </summary>        
        Udt = 98,

        /// <summary>
        /// The concatenator data type.
        /// </summary>                
        Concatenator = 99,

        #endregion

        #region SQL Server Table-Valued Types

        /// <summary>
        /// The table variable data type.
        /// </summary>
        TableVariable = 100,

        /// <summary>
        /// The user-defined table data type.
        /// </summary>      
        Udtt = 101,

        /// <summary>
        /// The temp table data type.
        /// </summary>    
        TempTable = 102,

        /// <summary>
        /// The bulk table data type.
        /// </summary>        
        BulkTable = 103,

        /// <summary>
        /// The View data type.
        /// </summary>    
        View = 104,

        #endregion

        #region Inlining

        /// <summary>
        /// The table inliner data type.
        /// </summary>
        InTable = 200,

        /// <summary>
        /// The column inliner data type.
        /// </summary>
        InColumn = 201,

        /// <summary>
        /// The expression inliner data type.
        /// </summary>           
        InExpression = 202,

        /// <summary>
        /// The sql inliner data type.
        /// </summary>       
        InSql = 203,

        /// <summary>
        /// The procedure inliner data type.
        /// </summary>           
        InProcedure = 204,

        /// <summary>
        /// The snippet inliner data type.
        /// </summary>       
        InSnippet = 205,

        /// <summary>
        /// The stored procedure inliner data type.
        /// </summary>        
        InStoredProcedure = 206,   

        #endregion

    }

    // designer types extensions
    internal static class DTExt
    {
        internal static DataType ToDataType(this DT dt)
        {
            return new DataType(dt);
        }

        internal static DT ToDT(this Designer.Inliner inlinerType)
        {
            return (DT)Enum.Parse(typeof(DT), 
                "In" + inlinerType.ToString(), true);
        }

        internal static Designer.Inliner ToInliner(this DT dt)
        {
            return (Designer.Inliner)Enum.Parse(typeof(Designer.Inliner),
                dt.ToString().Substring(2, dt.ToString().Length - 2), true);
        }

        internal static bool IsDefined(this DT dt)
        {
            return dt != DT.None;
        }

        internal static bool IsUserDefinedType(this DT dt)
        {
            return dt == DT.Udt || dt == DT.Udtt;
        }

        internal static bool IsInteger(this DT dt)
        {
            return dt == DT.Bit
                || dt == DT.Tinyint
                || dt == DT.Smallint
                || dt == DT.Int
                || dt == DT.Bigint;
        }

        internal static bool IsWeakDateTime(this DT dt)
        {
            return dt == DT.Smalldatetime || dt == DT.Datetime;
        }

        internal static bool IsDateTime(this DT dt)
        {
            return dt == DT.Smalldatetime || dt == DT.Datetime || dt == DT.Datetime2;
        }

        internal static bool IsUnicode(this DT dt)
        {
            return dt == DT.NChar || dt == DT.NText || dt == DT.NVarchar || dt == DT.NVarcharMax || dt == DT.Sysname;
        }

        internal static bool IsDecimalNumber(this DT dt)
        {
            return dt == DT.Decimal || dt == DT.Numeric || dt == DT.Float || dt == DT.Real || dt == DT.Money || dt == DT.Smallmoney;
        }

        internal static bool IsDataType(this DT dt)
        {
            return dt.IsDefined()
                && ((int)dt <= 36
                    || dt == DT.Concatenator); 
        }

        internal static bool IsInliner(this DT dt)
        {
            return (int)dt >= 200 && (int)dt < 300;
        }

        internal static bool IsNotInliner(this DT dt)
        {
            return !IsInliner(dt);
        }

        internal static bool IsConcatenator(this DT dt)
        {
            return dt == DT.Concatenator;
        }

        internal static bool IsTable(this DT dt)
        {
            return (int)dt >= 100 && (int)dt < 200;
        }

        internal static bool IsVTB(this DT dt)
        {
            return dt == DT.TableVariable 
                || dt == DT.TempTable 
                || dt == DT.BulkTable;
        }

        internal static bool IsTableVariable(this DT dt)
        {
            return dt == DT.TableVariable;
        }

        internal static bool IsScalar(this DT dt)
        {
            return !dt.IsTable();
        }

        internal static bool IsRowversion(this DT dt)
        {
            return dt == DT.Timestamp
                || dt == DT.Rowversion;
        }

        internal static bool HasAtSign(this DT dt)
        {
            return
                (int)dt != 98       // udt
                && (int)dt != 101   // udtt
                && (int)dt != 102   // temp table
                && (int)dt != 103;  // bulk table
        }

        internal static bool IsNameType(this DT dt)
        {
            return !dt.HasAtSign();
        }

        internal static bool IsInlinerOrConcatenator(this DT dt)
        {
            return dt.IsInliner() || dt.IsConcatenator();
        }

        internal static bool IsParamOrSqlVariable(this IdentifierType it)
        {
            return (it == IdentifierType.Param) || (it == IdentifierType.SqlVariable);
        }

        #region Exceptions

        internal static QueryTalkException InvalidInlinerException(this DT declaredType, string creator, string name, DT[] requiredTypes)
        {
            return new QueryTalkException(creator, QueryTalkExceptionType.InvalidInliner,
                String.Format("param = {0}{1}   declared inliner type = {2}{3}   allowed inliner type(s) = {4}",
                    name, Environment.NewLine, declaredType.ToInliner(), Environment.NewLine,
                    String.Join(", ", requiredTypes.Select(a => 
                        a.ToInliner().ToString()).ToArray())));
        }

        #endregion

    }

}
