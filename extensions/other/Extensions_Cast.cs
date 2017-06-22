#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;
using System.Data;
using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        #region E

        /// <summary>
        /// Converts an expression string into the Expression object.
        /// </summary>
        /// <param name="expressionString">An expression string which is to be converted into the Expression object.</param>
        public static Expression E(this string expressionString)
        {
            return Expression.CreateByExpressionString(expressionString);
        }

        #endregion

        #region V

        #region System.Boolean

        /// <summary>
        /// Converts a value into the Value object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static Value V(this System.Boolean value)
        {
            return new Value(value);
        }

        /// <summary>
        /// Converts the specified value into the literal value.
        /// </summary>
        /// <param name="value">The value to be used as literal.</param>
        public static Value L(this System.Boolean value)
        {
            return new Value(value, Parameterization.None);
        }

        #endregion

        #region System.Byte

        /// <summary>
        /// Converts a value into the Value object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static Value V(this System.Byte value)
        {
            return new Value(value);
        }

        /// <summary>
        /// Converts the specified value into the literal value.
        /// </summary>
        /// <param name="value">The value to be used as literal.</param>
        public static Value L(this System.Byte value)
        {
            return new Value(value, Parameterization.None); 
        }

        #endregion

        #region System.Byte[]

        /// <summary>
        /// Converts a value into the Value object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static Value V(this System.Byte[] value)
        {
            return new Value(value);
        }

        /// <summary>
        /// Converts the specified value into the literal value.
        /// </summary>
        /// <param name="value">The value to be used as literal.</param>
        public static Value L(this System.Byte[] value)
        {
            return new Value(value, Parameterization.None);
        }

        #endregion

        #region System.DateTime

        /// <summary>
        /// Converts a value into the Value object.
        /// </summary>
        /// <param name="value">A System.DateTime value which is converted with default precision of 3-digits.</param>
        public static Value V(this System.DateTime value)
        {
            return new Value(value, Mapping.DefaultDateTimeType, true); 
        }

        /// <summary>
        /// Converts the specified value into the literal value.
        /// </summary>
        /// <param name="value">The value to be used as literal.</param>
        public static Value L(this System.DateTime value)
        {
            return new Value(value, Parameterization.None); 
        }

        #endregion

        #region System.DateTimeOffset

        /// <summary>
        /// Converts a value into the Value object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static Value V(this System.DateTimeOffset value)
        {
            return new Value(value); 
        }

        /// <summary>
        /// Converts the specified value into the literal value.
        /// </summary>
        /// <param name="value">The value to be used as literal.</param>
        public static Value L(this System.DateTimeOffset value)
        {
            return new Value(value, Parameterization.None); 
        }

        #endregion

        #region System.Decimal

        /// <summary>
        /// Converts a value into the Value object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static Value V(this System.Decimal value)
        {
            return new Value(value); 
        }

        /// <summary>
        /// Converts the specified value into the literal value.
        /// </summary>
        /// <param name="value">The value to be used as literal.</param>
        public static Value L(this System.Decimal value)
        {
            return new Value(value, Parameterization.None); 
        }

        #endregion

        #region System.Single

        /// <summary>
        /// Converts a value into the Value object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static Value V(this System.Single value)
        {
            return new Value(value); 
        }

        /// <summary>
        /// Converts the specified value into the literal value.
        /// </summary>
        /// <param name="value">The value to be used as literal.</param>
        public static Value L(this System.Single value)
        {
            return new Value(value, Parameterization.None); 
        }

        #endregion

        #region System.Double

        /// <summary>
        /// Converts a value into the Value object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static Value V(this System.Double value)
        {
            return new Value(value); 
        }

        /// <summary>
        /// Converts the specified value into the literal value.
        /// </summary>
        /// <param name="value">The value to be used as literal.</param>
        public static Value L(this System.Double value)
        {
            return new Value(value, Parameterization.None); 
        }

        #endregion

        #region System.Guid

        /// <summary>
        /// Converts a value into the Value object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static Value V(this System.Guid value)
        {
            return new Value(value); 
        }

        /// <summary>
        /// Converts the specified value into the literal value.
        /// </summary>
        /// <param name="value">The value to be used as literal.</param>
        public static Value L(this System.Guid value)
        {
            return new Value(value, Parameterization.None); 
        }

        #endregion

        #region System.Int16

        /// <summary>
        /// Converts a value into the Value object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static Value V(this System.Int16 value)
        {
            return new Value(value); 
        }

        /// <summary>
        /// Converts the specified value into the literal value.
        /// </summary>
        /// <param name="value">The value to be used as literal.</param>
        public static Value L(this System.Int16 value)
        {
            return new Value(value, Parameterization.None); 
        }

        #endregion

        #region System.Int32

        /// <summary>
        /// Converts a value into the Value object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static Value V(this System.Int32 value)
        {
            return new Value(value); 
        }

        /// <summary>
        /// Converts the specified value into the literal value.
        /// </summary>
        /// <param name="value">The value to be used as literal.</param>
        public static Value L(this System.Int32 value)
        {
            return new Value(value, Parameterization.None); 
        }

        #endregion

        #region System.Int64

        /// <summary>
        /// Converts a value into the Value object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static Value V(this System.Int64 value)
        {
            return new Value(value); 
        }

        /// <summary>
        /// Converts the specified value into the literal value.
        /// </summary>
        /// <param name="value">The value to be used as literal.</param>
        public static Value L(this System.Int64 value)
        {
            return new Value(value, Parameterization.None); 
        }

        #endregion

        #region System.String

        /// <summary>
        /// Converts a value into the Value object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static Value V(this System.String value)
        {
            return new Value(value, Mapping.DefaultStringType, true); 
        }

        /// <summary>
        /// Converts the specified value into the literal value.
        /// </summary>
        /// <param name="value">The value to be used as literal.</param>
        public static Value L(this System.String value)
        {
            return new Value(value, Parameterization.None); 
        }

        #endregion

        #region System.TimeSpan

        /// <summary>
        /// Converts a value into the Value object.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        public static Value V(this System.TimeSpan value)
        {
            return new Value(value); 
        }

        /// <summary>
        /// Converts the specified value into the literal value.
        /// </summary>
        /// <param name="value">The value to be used as literal.</param>
        public static Value L(this System.TimeSpan value)
        {
            return new Value(value, Parameterization.None); 
        }

        #endregion

        #endregion

        #region ToIdentifier

        /// <summary>
        /// Converts a string argument into the SQL column identifier.
        /// </summary>
        /// <param name="column">A column name.</param>
        /// <param name="splitByDot">If true then the column name with a dot will be treated as a compound name consisting of a table alias and column name.</param>
        public static Identifier I(this System.String column, bool splitByDot = true)
        {
            return new Identifier(column, splitByDot);       
        }

        /// <summary>
        /// Returns the identifier of a specified node.
        /// </summary>
        /// <param name="node">Is the node object.</param>
        public static Identifier I(this DbNode node)
        {
            // check null
            if (node == null)
            {
                throw new QueryTalkException("I", QueryTalkExceptionType.ArgumentNull, "node = null", Text.Method.I);
            }
            return DbMapping.GetNodeName(node.NodeID); 
        }

        #endregion

        #region CastAs

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this System.Boolean value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this System.Byte value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this System.Byte[] value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this System.DateTime value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this System.DateTimeOffset value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this System.Decimal value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this System.Double value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this System.Guid value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this System.Int16 value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this System.Int32 value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this System.Int64 value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this System.Single value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this System.TimeSpan value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this string value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this IScalar value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this DbColumn value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        /// <summary>
        /// Converts a value of one data type to another.
        /// </summary>
        /// <param name="value">A Value object.</param>
        /// <param name="dataType">The target data type definition.</param>
        public static SysFn CastAs(this Value value, Wall.DataType dataType)
        {
            return Designer.Cast(value, dataType);
        }

        #endregion

        #region Column

        /// <summary>
        /// Creates the column object from the string identifier of the column.
        /// </summary>
        /// <param name="column">A System.String identifier of a column. Can be two-part containing the table alias.</param>
        public static Column ToColumn(this System.String column)
        {
            if (column == null)
            {
                throw new QueryTalkException("d.ToColumn", QueryTalkExceptionType.ArgumentNull, "column = null");
            }

            return new Column(column);
        }

        #endregion

        #region ToView

        /// <summary>
        /// Converts the specified data to the data view.
        /// </summary>
        /// <typeparam name="T">The type of the data to convert.</typeparam>
        /// <param name="data">The data to convert.</param>
        public static View ToView<T>(this IEnumerable<T> data)
        {
            return ViewConverter.ToView(data);    
        }

        /// <summary>
        /// Converts the specified data to the data view.
        /// </summary>
        /// <param name="data">The data to convert.</param>
        public static View ToView(this DataTable data)
        {
            return ViewConverter.ToView(data);    
        }

        #endregion

    }
}
