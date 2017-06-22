#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;
using QueryTalk.Wall;
using System.Diagnostics;

namespace QueryTalk
{
    /// <summary>
    /// Represents a strongly typed single result set. (This class has no public constructor.)
    /// </summary>
    /// <typeparam name="T">
    /// The type of a QueryTalk compliant data class.
    /// </typeparam>
    [DebuggerTypeProxy(typeof(Wall.SetDebuggerProxy<>))]
    public class ResultSet<T> : HashSet<T>   
    {
        /// <summary>
        /// The number of rows in the result set.
        /// </summary>
        public int RowCount
        {
            get
            {
                if (DataTable != null)
                {
                    return DataTable.Rows.Count;
                }
                else
                {
                    return Count;
                }
            }
        }

        /// <summary>
        /// An alternative storage containing rows that have been stored into the DataTable class.
        /// </summary>
        public DataTable DataTable { get; private set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Type _tableType = null;
        internal Type TableType
        {
            get
            {
                if (_tableType != null)
                {
                    return _tableType;
                }

                _tableType = typeof(T);

                if (_tableType == typeof(DataTable))
                {
                    return _tableType;
                }

                // important:
                //   Using a dynamic type inside the generic method affects the reflection. Since the dynamic type 
                //   is resolved in run-time while the generic parameter T obtains its value in compile-time, 
                //   passing a dynamic type by generic parameter means that it has to be converted into System.Object.
                //   This is the reason why we infer the (dynamic) type from the object and not from the parameter type.
                if (_tableType == typeof(System.Object) && Count > 0)
                {
                    _isDynamic = true;

                    // infer type from the item object
                    _tableType = this.First().GetType();

                    typeof(ResultSet<>).MakeGenericType(_tableType);
                }

                return _tableType;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _isDynamic = false;
        /// <summary>
        /// Returns a value that indicates whether the type is a dynamically created type.
        /// </summary>
        public bool IsDynamic
        {
            get
            {
                return _isDynamic;
            }
        }

        #region Constructors

        internal ResultSet()
            : base(new HashSet<T>())
        { }

        internal ResultSet(IEnumerable<T> list)
            : base(list ?? new HashSet<T>())
        { }

        internal ResultSet(DataTable dataTable)
            : base()
        {
            DataTable = dataTable;
        }

        // for single DbRow object-row only
        internal ResultSet(DbRow row)
            : base(new HashSet<T>(new T[] {(T)(object)row}))
        { }

        #endregion

        #region Converters

        // returns first cell of the first row
        /// <summary>
        /// Returns the value of the first cell of the resultset table.
        /// </summary>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        public TValue ToValue<TValue>()
        {
            PropertyInfo property = null;

            try
            {
                if (TableType == typeof(DataTable))
                {
                    if (DataTable.AsEnumerable().Count() > 0)
                    {
                        return (TValue)DataTable.Rows[0][0];
                    }
                    else
                    {
                        throw new QueryTalkException("ResultSet.ToValue<TValue>", QueryTalkExceptionType.EmptyResultset,
                            null, Text.Method.ToValue, "data class = DataTable");
                    }
                }
                else
                {
                    if (Count > 0)
                    {
                        var properties = TableType.GetProperties();
                        if (properties.Length == 0)
                        {
                            throw new QueryTalkException("ResultSet.ToValue<TValue>", QueryTalkExceptionType.EmptyResultset,
                                null, Text.Method.ToValue, String.Format("data class = {0}", TableType));
                        }

                        property = properties[0];
                        if (property.GetGetMethod() == null)
                        {
                            throw new QueryTalkException("ResultSet.ToValue<TValue>", QueryTalkExceptionType.NonAccessableProperty,
                                null, Text.Method.ToValue, String.Format("data class = {0}{1}   property = {2}",
                                    TableType, Environment.NewLine, property.Name));
                        }

                        var value = property.GetValue(this.First(), null);
                        return (value != null) ? (TValue)value : default(TValue);
                    }
                    else
                    {
                        throw new QueryTalkException( "ResultSet.ToValue<TValue>",QueryTalkExceptionType.EmptyResultset,
                            null, Text.Method.ToValue, String.Format("data class = {0}", TableType));
                    }
                }
            }
            catch (QueryTalkException)
            {
                throw;
            }
            catch (System.InvalidCastException)
            {
                throw new QueryTalkException("ResultSet.ToValue<TValue>", QueryTalkExceptionType.InvalidCast,
                    null, Text.Method.ToValue, String.Format("returning type = {0}{1}   value type = {2}",
                        typeof(TValue), Environment.NewLine, property.PropertyType));
            }
            catch (System.Exception ex)
            {
                var exception = new QueryTalkException("ResultSet.ToValue<TValue>", QueryTalkExceptionType.ClrException,
                    null, Text.Method.ToValue, String.Format("returning type = {0}", typeof(TValue)));
                exception.ClrException = ex;
                throw exception;
            }
        }

        internal DataTable ToDataTable()
        {
            if (DataTable != null)
            {
                return DataTable;
            }
            else
            {
                return ToDataTable<T>(this);
            }
        }

        internal static DataTable ToDataTable<U>(IEnumerable<U> collection)
        {
            string collectionTypeName;   
            Type tableType = typeof(U);  
            
            if (collection == null)
            {
                throw new QueryTalkException("ToDataTable<T>", QueryTalkExceptionType.CollectionNullOrEmpty, 
                    String.Format("data class = {0}", tableType), Text.Method.ToDataTable);
            }
     
            collectionTypeName = collection.GetType().Name;
            if (collection is Wall.IResult)
            {
                collectionTypeName = Text.Class.ResultSet;
            }

            if (collectionTypeName == Text.Class.ResultSet)
            {
                var resultSet = (ResultSet<U>)collection;

                if (resultSet.DataTable != null)
                {
                    return resultSet.DataTable;
                }

                // check dynamic - not convertable
                if (tableType == typeof(System.Object))
                {
                    if (collection.Count() == 0)
                    {
                        throw new QueryTalkException("ResultSet.getTableType", QueryTalkExceptionType.EmptyResultset,
                            "table type = dynamic", Text.Method.ToDataTable);
                    }

                    // infer type from the item
                    using (var enumerator = collection.GetEnumerator())
                    {
                        enumerator.MoveNext();
                        tableType = enumerator.Current.GetType();
                    }
                }
            }

            DataTable dataTable = new DataTable();
            dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;

            // always build new getters (not using cache)
            var getters = new List<IPropertyAccessor>();

            var props = tableType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(prop => prop.GetGetMethod() != null)
                    .ToArray();

            PropertyInfo[] properties;

            if (tableType.IsDbRow())
            {
                var node = DbMapping.GetNodeMap(tableType);
                var propsByDatabaseOrder = new List<PropertyInfo>();
                foreach (var column in node.SortedColumnsByDatabaseOrder)
                {
                    var prop = props.Where(a => a.Name.EqualsCS(column.ClrName)).Select(a => a).FirstOrDefault();
                    if (prop != null)
                    {
                        propsByDatabaseOrder.Add(prop);
                    }
                }

                properties = propsByDatabaseOrder.ToArray();
            }
            else
            {
                properties = props;
            }

            if (properties.Length == 0)
            {
                throw new QueryTalkException("ResultSet.ToDataTable", QueryTalkExceptionType.InvalidDataClass,
                    String.Format("data class = {0}", tableType), Text.Method.ToDataTable);
            }

            int numberOfPropertiesUsed = 0;
            foreach (PropertyInfo property in properties)
            {
                Type clrType;
                QueryTalkException exception;

                var clrTypeMatch = Mapping.CheckClrCompliance(property.PropertyType, out clrType, out exception);
                if (clrTypeMatch == Mapping.ClrTypeMatch.NodeMatch)
                {
                    continue;
                }

                if (clrTypeMatch != Mapping.ClrTypeMatch.ClrMatch)
                {
                    ThrowNotClrCompliantException(exception, property.PropertyType);
                }

                dataTable.Columns.Add(property.Name, clrType);

                getters.Add(PropertyAccessor.Create(tableType, property));

                ++numberOfPropertiesUsed;
            }

            foreach (U item in collection)
            {
                DataRow row = dataTable.NewRow();
                for (int i = 0; i < numberOfPropertiesUsed; i++)
                {
                    row[i] = getters[i].GetValue(item) ?? DBNull.Value;
                }

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private static void ThrowNotClrCompliantException(QueryTalkException exception, Type classType)
        {
            exception.Extra = String.Format("A property with a non-compliant type belongs to a class {0}. This class cannot be converted to the DataTable object.", classType);
            throw exception;
        }

        #endregion

        #region ToString

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            if (typeof(T) == typeof(System.Object))
            {
                return String.Format("QueryTalk.Set | {0} row(s)", RowCount);
            }
            else
            {
                return String.Format("QueryTalk.Set | {0} row(s) of {1}", RowCount, typeof(T));
            }
        } 

        #endregion

    }

}
