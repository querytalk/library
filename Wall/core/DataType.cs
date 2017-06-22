#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Represents a data type of a database column, parameter, or variable.
    /// </summary>
    public sealed class DataType
    {
        internal DT DT { get; private set; }

        internal int Length { get; private set; }

        internal int Precision { get; private set; }

        internal int Scale { get; private set; }

        internal TableArgument Udt { get; private set; }

        private QueryTalkException _exception;
        internal QueryTalkException Exception
        {
            get
            {
                return _exception;
            }
        }

        #region Constructors

        private DataType(DT dt)
        {
            DT = dt;
        }

        /// <summary>
        /// Initializes a new instance of the DataType class.
        /// </summary>
        /// <param name="dt">Is the QueryTalk's designer type.</param>
        /// <param name="check">A flag indicating whether the data type definition check is done.</param>
        public DataType(DT dt, bool check = true)
            : this(dt)
        {
            Length = 0;
            Precision = 0;
            Scale = 0;

            if (check && !CheckParamDef(null, out _exception))
            {
                throw _exception;
            }
        }

        /// <summary>
        /// Initializes a new instance of the DataType class.
        /// </summary>
        /// <param name="dt">Is the QueryTalk's designer type.</param>
        /// <param name="length">The length of a data type.</param>
        /// <param name="check">A flag indicating whether the data type definition check is done.</param>
        public DataType(DT dt, int length, bool check = true)
            : this(dt)
        {
            Length = length;
            Precision = 0;
            Scale = 0;

            if (check && !CheckParamDef(null, out _exception))
            {
                throw _exception;
            }
        }

        /// <summary>
        /// Initializes a new instance of the DataType class.
        /// </summary>
        /// <param name="dt">Is the QueryTalk's designer type.</param>
        /// <param name="precision">The precision of a data type.</param>
        /// <param name="scale">The scale of a data type.</param>
        /// <param name="check">A flag indicating whether the data type definition check is done.</param>
        public DataType(DT dt, int precision, int scale, bool check = true)
            : this(dt)
        {
            Length = 0;
            Precision = precision;
            Scale = scale;

            if (check && !CheckParamDef(null, out _exception))
            {
                throw _exception;
            }
        }

        /// <summary>
        /// Initializes a new instance of the DataType class.
        /// </summary>
        /// <param name="dt">Is the QueryTalk's designer type.</param>
        /// <param name="userDefinedType">A user-defined type.</param>
        /// <param name="check">A flag indicating whether the data type definition check is done.</param>
        public DataType(DT dt, TableArgument userDefinedType, bool check = true)
            : this(dt)
        {
            Udt = userDefinedType;

            if (check && !CheckParamDef(Text.Method.SysUdt, out _exception))
            {
                throw _exception;
            }
        }

        #endregion

        private bool CheckParamDef(string method, out QueryTalkException exception)
        {
            exception = null;

            if (DT.IsUserDefinedType())
            {
                if (Udt == null)
                {
                    exception = new QueryTalkException("DataType.CheckParamDef", QueryTalkExceptionType.ArgumentNull,
                        "data type = user-defined type", method);
                    exception.Extra = "User-defined type is missing the identifier. Provide the identifier for the user-defined type.";
                    return false;
                }
                else
                {
                    Udt.Exception.TryThrow();
                    return true;
                }
            }

            if (!DT.IsDataType())
            {
                return true;
            }

            var info = Mapping.SqlMapping[DT];

            // check max size
            if (info.MaxSize > 0 && (Length > info.MaxSize || Precision > info.MaxSize))
            {
                exception = new QueryTalkException("DataTypeDef.CheckParamDef", QueryTalkExceptionType.DbTypeOversize, null, method,
                    String.Format("db type = {0}{1}   length/precision = {2}{3}   allowed maximum = {4}",
                        info.SqlPlain, Environment.NewLine, Length > info.MaxSize ? Length : Precision, Environment.NewLine, info.MaxSize));
                return false;
            }

            // check size components
            if (Scale > Precision || Length < 0 || Precision < 0 || Scale < 0)
            {
                exception = new QueryTalkException("DataTypeDef.CheckParamDef", QueryTalkExceptionType.InvalidDbTypeDeclaration, null, method,
                    String.Format("db type = {0}{1}   length = {2}{3}   precision = {4}{5}   scale = {6}",
                        info.SqlPlain, Environment.NewLine, Length, Environment.NewLine, Precision, Environment.NewLine, Scale));
                return false;
            }

            // check combination of length, precision, scale
            if ((info.SizeType == Mapping.SizeType.None
                && Length + Precision + Scale > 0)
                || (info.SizeType == Mapping.SizeType.Length
                && Precision + Scale > 0)
                || (info.SizeType == Mapping.SizeType.Precision
                && Length > 0))
            {
                exception = new QueryTalkException("DataTypeDef.CheckParamDef", QueryTalkExceptionType.InvalidDbTypeDeclaration, null, method,
                    String.Format("db type = {0}{1}   length = {2}{3}   precision = {4}{5}   scale = {6}",
                        info.SqlPlain, Environment.NewLine, Length, Environment.NewLine, Precision, Environment.NewLine, Scale));
                return false;
            }

            return true;
        }

        internal string Build()
        {
            if (Udt != null)
            {
                return Udt.Sql;    
            }
          
            if (Precision != 0)
            {
                return String.Format("{0}({1},{2})", Mapping.SqlMapping[DT].Sql, Precision, Scale);
            }
            else if (Length != 0)
            {
                return String.Format("{0}({1})", Mapping.SqlMapping[DT].Sql, Length);
            }
            else
            {
                return Mapping.SqlMapping[DT].Sql;
            }
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return Build();
        }
    }
}
