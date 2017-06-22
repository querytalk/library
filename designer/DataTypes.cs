#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {
        /// <summary>
        /// A 64-bit signed integer.
        /// </summary>
        public static DataType Bigint
        {
            get
            {
                return new DataType(DT.Bigint);
            }
        }

        /// <summary>
        /// System.Array of type System.Byte. A fixed-length stream of binary data ranging between 1 and 8,000 bytes.
        /// </summary>
        /// <param name="length">The length of an array.</param>
        /// <returns></returns>
        public static DataType Binary(int length)
        {
            return new DataType(DT.Binary, length);
        }

        /// <summary>
        /// System.Boolean. An unsigned numeric value that can be 0, 1, or null.
        /// </summary>
        public static DataType Bit
        {
            get
            {
                return new DataType(DT.Bit);
            }
        }

        /// <summary>
        /// System.String. A fixed-length stream of non-Unicode characters ranging between 1 and 8,000 characters.
        /// </summary>
        /// <param name="length">The length of a string.</param>
        /// <returns></returns>
        public static DataType Char(int length)
        {
            return new DataType(DT.Char, length);
        }

        /// <summary>
        /// Date data ranging in value from January 1,1 AD through December 31, 9999 AD.
        /// </summary>
        public static DataType Date
        {
            get
            {
                return new DataType(DT.Date);
            }
        }

        /// <summary>
        /// System.DateTime. Date and time data ranging in value from January 1, 1753 to December 31, 9999 to an accuracy of 3.33 milliseconds.
        /// </summary>
        public static DataType Datetime
        {
            get
            {
                return new DataType(DT.Datetime);
            }
        }

        /// <summary>
        /// System.DateTime. Date value range is from January 1,1 AD through December 31, 9999 AD. Time value range is 00:00:00 through 23:59:59.9999999 with an accuracy of 100 nanoseconds.
        /// </summary>
        public static DataType Datetime2()
        {
            return new DataType(DT.Datetime2);
        }

        /// <summary>
        /// System.DateTime. Date value range is from January 1,1 AD through December 31, 9999 AD. Time value range is 00:00:00 through 23:59:59.9999999 with an accuracy of 100 nanoseconds.
        /// </summary>
        /// <param name="precision">The number of digits to the right of the decimal point, which represents the fractional second precision, can be specified from 0 up to 7 (100 nanoseconds).</param>
        public static DataType Datetime2(int precision)
        {
            return new DataType(DT.Datetime2, precision);
        }

        /// <summary>
        /// Date and time data with time zone awareness. Date value range is from January 1,1 AD through December 31, 9999 AD. Time value range is 00:00:00 through 23:59:59.9999999 with an accuracy of 100 nanoseconds. Time zone value range is -14:00 through +14:00.
        /// </summary>
        public static DataType Datetimeoffset()
        {
            return new DataType(DT.Datetimeoffset);
        }

        /// <summary>
        /// Date and time data with time zone awareness. Date value range is from January 1,1 AD through December 31, 9999 AD. Time value range is 00:00:00 through 23:59:59.9999999 with an accuracy of 100 nanoseconds. Time zone value range is -14:00 through +14:00.
        /// </summary>
        /// <param name="precision">The number of digits to the right of the decimal point, which represents the fractional second precision, can be specified from 0 up to 7 (100 nanoseconds).</param>
        public static DataType Datetimeoffset(int precision)
        {
            return new DataType(DT.Datetimeoffset, precision);
        }

        /// <summary>
        /// System.Decimal. A fixed precision and scale numeric value between -10 38 -1 and 10 38 -1.
        /// </summary>
        /// <param name="precision">The maximum total number of decimal digits that can be stored, both to the left and to the right of the decimal point.</param>
        /// <param name="scale">The maximum number of decimal digits that can be stored to the right of the decimal point.</param>
        public static DataType Decimal(int precision, int scale)
        {
            return new DataType(DT.Decimal, precision, scale);
        }

        /// <summary>
        /// System.Double. A floating point number within the range of -1.79E +308 through 1.79E +308.
        /// </summary>
        public static DataType Float
        {
            get
            {
                return new DataType(DT.Float);
            }
        }

        /// <summary>
        /// System.Array of type System.Byte. A variable-length stream of binary data ranging from 0 to 2 31 -1 (or 2,147,483,647) bytes.
        /// </summary>
        public static DataType Image
        {
            get
            {
                return new DataType(DT.Image);
            }
        }

        /// <summary>
        /// System.Int32. A 32-bit signed integer.
        /// </summary>
        public static DataType Int
        {
            get
            {
                return new DataType(DT.Int);
            }
        }

        /// <summary>
        /// System.Decimal. A currency value ranging from -2 63 (or -9,223,372,036,854,775,808) to 2 63 -1 (or +9,223,372,036,854,775,807) with an accuracy to a ten-thousandth of a currency unit.
        /// </summary>
        public static DataType Money
        {
            get
            {
                return new DataType(DT.Money);
            }
        }

        /// <summary>
        /// System.String. A fixed-length stream of Unicode characters ranging between 1 and 4,000 characters.
        /// </summary>
        /// <param name="length">The length of a string.</param>
        public static DataType NChar(int length)
        {
            return new DataType(DT.NChar, length);
        }

        /// <summary>
        /// System.String. A variable-length stream of Unicode data with a maximum length of 2 30 - 1 (or 1,073,741,823) characters.
        /// </summary>
        public static DataType NText
        {
            get
            {
                return new DataType(DT.NText);
            }
        }

        /// <summary>
        /// System.Decimal. A fixed precision and scale numeric value between -10 38 -1 and 10 38 -1.
        /// </summary>
        /// <param name="precision">The maximum total number of decimal digits that can be stored, both to the left and to the right of the decimal point.</param>
        /// <param name="scale">The maximum number of decimal digits that can be stored to the right of the decimal point.</param>
        public static DataType Numeric(int precision, int scale)
        {
            return new DataType(DT.Numeric, precision, scale);
        }

        /// <summary>
        /// System.String. A variable-length stream of Unicode characters ranging between 1 and 4,000 characters. Implicit conversion fails if the string is greater than 4,000 characters. Explicitly set the object when working with strings longer than 4,000 characters.
        /// </summary>
        /// <param name="length">The lenght of a string.</param>
        public static DataType NVarchar(int length)
        {
            return new DataType(DT.NVarchar, length);
        }

        /// <summary>
        /// System.String. A variable-length stream of Unicode characters. Max indicates that the maximum storage size is 2^31-1 bytes (2 GB).
        /// </summary>
        public static DataType NVarcharMax
        {
            get
            {
                return new DataType(DT.NVarcharMax);
            }
        }

        /// <summary>
        /// System.Single. A floating point number within the range of -3.40E +38 through 3.40E +38.
        /// </summary>
        public static DataType Real
        {
            get
            {
                return new DataType(DT.Real);
            }
        }

        /// <summary>
        /// System.DateTime. Date and time data ranging in value from January 1, 1900 to June 6, 2079 to an accuracy of one minute.
        /// </summary>
        public static DataType Smalldatetime
        {
            get
            {
                return new DataType(DT.Smalldatetime);
            }
        }

        /// <summary>
        /// System.Int16. A 16-bit signed integer.
        /// </summary>
        public static DataType Smallint
        {
            get
            {
                return new DataType(DT.Smallint);
            }
        }

        /// <summary>
        /// System.Decimal. A currency value ranging from -214,748.3648 to +214,748.3647 with an accuracy to a ten-thousandth of a currency unit.
        /// </summary>
        public static DataType Smallmoney
        {
            get
            {
                return new DataType(DT.Smallmoney);
            }
        }

        /// <summary>
        /// System.Object. A special data type that can contain numeric, string, binary, or date data as well as the SQL Server values Empty and Null, which is assumed if no other type is declared.
        /// </summary>
        public static DataType Sqlvariant
        {
            get
            {
                return new DataType(DT.Sqlvariant);
            }
        }

        /// <summary>
        /// The sysname data type is used for the SQL identifiers. sysname is functionally the same as nvarchar(128) except that, by default, sysname is NOT NULL.
        /// </summary>
        public static DataType Sysname
        {
            get
            {
                return new DataType(DT.Sysname);
            }
        }

        /// <summary>
        /// System.String. A variable-length stream of non-Unicode data with a maximum length of 2 31 -1 (or 2,147,483,647) characters.
        /// </summary>
        public static DataType Text
        {
            get
            {
                return new DataType(DT.Text);
            }
        }

        /// <summary>
        /// Time data based on a 24-hour clock. Time value range is 00:00:00 through 23:59:59.9999999 with an accuracy of 100 nanoseconds. Corresponds to a SQL Server time value.
        /// </summary>
        public static DataType Time()
        {
            return new DataType(DT.Time);
        }

        /// <summary>
        /// Time data based on a 24-hour clock. Time value range is 00:00:00 through 23:59:59.9999999 with an accuracy of 100 nanoseconds. Corresponds to a SQL Server time value.
        /// </summary>
        /// <param name="precision">The number of digits to the right of the decimal point, which represents the fractional second precision, can be specified from 0 up to 7 (100 nanoseconds).</param>
        public static DataType Time(int precision)
        {
            return new DataType(DT.Time, precision);
        }

        /// <summary>
        /// System.Array of type System.Byte. Automatically generated binary numbers, which are guaranteed to be unique within a database. Timestamp is used typically as a mechanism for version-stamping table rows. The storage size is 8 bytes.
        /// </summary>
        public static DataType Timestamp
        {
            get
            {
                return new DataType(DT.Timestamp);
            }
        }

        /// <summary>
        /// System.Array of type System.Byte. Automatically generated binary numbers, which are guaranteed to be unique within a database. Rowversion is used typically as a mechanism for version-stamping table rows. The storage size is 8 bytes.
        /// </summary>
        public static DataType Rowversion
        {
            get
            {
                return new DataType(DT.Rowversion);
            }
        }

        /// <summary>
        /// System.Byte. An 8-bit unsigned integer.
        /// </summary>
        public static DataType Tinyint
        {
            get
            {
                return new DataType(DT.Tinyint);
            }
        }

        /// <summary>
        /// System.Guid. A globally unique identifier (or GUID).
        /// </summary>
        public static DataType Uniqueidentifier
        {
            get
            {
                return new DataType(DT.Uniqueidentifier);
            }
        }

        /// <summary>
        /// System.Array of type System.Byte. A variable-length stream of binary data ranging between 1 and 8,000 bytes. Implicit conversion fails if the byte array is greater than 8,000 bytes. Explicitly set the object when working with byte arrays larger than 8,000 bytes.
        /// </summary>
        /// <param name="length">The length of an array.</param>
        public static DataType Varbinary(int length)
        {
            return new DataType(DT.Varbinary, length);
        }

        /// <summary>
        /// System.Array of type System.Byte.  A variable-length stream of binary data. Max indicates that the maximum storage size is 2^31-1 bytes.
        /// </summary>
        public static DataType VarbinaryMax
        {
            get
            {
                return new DataType(DT.VarbinaryMax);
            }
        }

        /// <summary>
        /// System.String. A variable-length stream of non-Unicode characters ranging between 1 and 8,000 characters.
        /// </summary>
        /// <param name="length">The length of a string.</param>
        public static DataType Varchar(int length)
        {
            return new DataType(DT.Varchar, length);
        }

        /// <summary>
        /// System.String. A variable-length stream of non-Unicode characters. Max indicates that the maximum storage size is 2^31-1 bytes (2 GB).
        /// </summary>
        public static DataType VarcharMax
        {
            get
            {
                return new DataType(DT.VarcharMax);
            }
        }

        /// <summary>
        /// An XML value.
        /// </summary>
        public static DataType Xml
        {
            get
            {
                return new DataType(DT.Xml);
            }
        }

        /// <summary>
        /// The concatenator data type based on the nvarchar(MAX) db type used for concatenation of SQL identifiers that are stored as values of the concatenator variable.
        /// </summary>
        public static DataType Concatenator
        {
            get
            {
                return new DataType(DT.Concatenator);
            }
        }

    }
}
