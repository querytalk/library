#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace QueryTalk.Wall
{
    internal static class Loader
    {
        private const string _isDBNullText = "IsDBNull";
        private const string _columnNameText = "ColumnName";
        private const string _columnOrdinalText = "ColumnOrdinal";
        private const string _dataTypeText = "DataType";
        private const string _dataTypeNameText = "DataTypeName";
        private const string _allowDBNullText = "AllowDBNull";
        private const string _dynamicAssemblyName = "QueryTalk.Dynamic";
        private const string _dynamicModuleName = "QtDynamicModule";
        private const string _getValueMethodName = "get_value";
        private const string _setValueMethodName = "set_value";

        internal class ColumnInfo : ArrayEqualityComparer<ColumnInfo>
        {
            internal string ColumnName { get; set; }               
            internal MethodInfo ReaderGetMethod { get; set; }     
            internal bool IsUnbox { get; set; }
            internal Type CtorParameterType { get; set; }
            internal int ReaderOrdinal { get; set; }
            internal PropertyInfo Property { get; set; }
            internal bool IsNullable { get; set; }
            internal Type ValueType { get; set; }               
            internal Type NullableType { get; set; }               

            public override bool Equals(object obj)
            {
                if (!(obj is ColumnInfo))
                {
                    return false;
                }

                ColumnInfo info = (ColumnInfo)obj;

                if (info.ColumnName == ColumnName
                    && info.ReaderOrdinal == ReaderOrdinal
                    && info.ValueType.FullName == ValueType.FullName
                    && info.IsNullable == IsNullable
                    && info.IsUnbox == IsUnbox)
                {
                    return true;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return String.Format("{0};{1};{2};{3};{4}",
                    ColumnName, ReaderOrdinal, ValueType.FullName, IsNullable, IsUnbox)
                        .GetHashCode();
            }
        }

        private static MethodInfo _isDBNullMethod = typeof(IDataRecord).GetMethod(_isDBNullText, new Type[] { typeof(int) });

        #region Provide loader

        private static int GetLoaderHash(IDataRecord reader)
        {
            var count = reader.FieldCount;
            int hash = 17;

            unchecked
            {
                for (int i = 0; i < count; ++i)
                {
                    hash = hash * 23 +
                        String.Format("{0};{1}",
                            reader.GetName(i),
                            reader.GetDataTypeName(i))
                        .GetHashCode();
                }
            }

            return hash;
        }

        internal static Func<IDataRecord, T> ProvideLoader<T>(IDataRecord reader, out QueryTalkException exception)
        {
            exception = null;
            object loader;
            Type type = typeof(T);
            Loader.ColumnInfo[] columns;
            Cache.LoaderCacheKey cacheKey = Cache.LoaderCacheKey.Default;
            Cache.LoaderCacheValue cacheValue;   
            var loaderType = Cache.GetLoaderType(type);    
            int hash = 0;

            // check cache
            if (Admin.IsLoaderCachingOn)
            {
                hash = GetLoaderHash(reader);              
                cacheKey = new Cache.LoaderCacheKey(hash, loaderType, type);
                if (Cache.TryGetLoaderCache<T>(reader, cacheKey, out cacheValue))
                {
                    return (Func<IDataRecord, T>)cacheValue.Loader;
                }
            }

            // dynamic loading
            if (loaderType == Cache.LoaderType.Dynamic)
            {
                columns = ReflectReader(reader, out exception);
                if (exception != null)
                {
                    exception.Arguments += String.Format("{0}   table = {1}", Environment.NewLine, type);
                    return null;
                }

                TryThrowInvalidDataClassException(null, columns);

                Type dynamicType = Loader.EmitTable(columns, Guid.NewGuid());
                loader = Loader.EmitRowLoaderDynamic(columns, dynamicType);
            }
            // Row 
            else if (loaderType == Cache.LoaderType.Row)
            {
                columns = ReflectRow(type, reader);
                TryThrowInvalidDataClassException(type, columns);
                loader = Loader.EmitRowLoader<T>(columns);
            }
            // regular data class
            else
            {
                columns = ReflectClass(type, reader);
                TryThrowInvalidDataClassException(type, columns);
                loader = Loader.EmitRowLoader<T>(columns);
            }
                
            // cache loader
            if (Admin.IsLoaderCachingOn)
            {
                cacheValue = new Cache.LoaderCacheValue(columns, loader);
                Cache.Loaders.TryAdd(cacheKey, cacheValue);
            }

            return (Func<IDataRecord, T>)loader;
        }

        #endregion

        #region Reflectors

        private static void CheckParmeterlessConstructorAndThrow(Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) == null)
            {
                throw new QueryTalkException("Loader", QueryTalkExceptionType.InvalidDataClass,
                    String.Format("data class = {0}", type), Text.Method.Go);
            }
        }

        private static ColumnInfo[] ReflectClass(Type type, IDataRecord reader)
        {
            string propertyName = Text.NotAvailable;

            CheckParmeterlessConstructorAndThrow(type);

            PropertyInfo[] properties = type.GetWritableProperties();
            if (properties.Length == 0)
            {
                throw new QueryTalkException("Reader.ReflectClass<T>", QueryTalkExceptionType.InvalidDataClass,
                    String.Format("data class = {0}", type));
            }

            var schema = ((SqlDataReader)reader).GetSchemaTable();
            var clrNames = schema.AsEnumerable()
                .Select(a => Naming.GetClrName(a.Field<string>(_columnNameText)))
                .ToList();
            IEnumerable<string> duplicates;
            Common.FindDuplicates(clrNames, out duplicates);

            QueryTalkException exception;
            var columns = new List<Loader.ColumnInfo>();
            int i = 0;
            foreach (PropertyInfo property in properties)
            {
                propertyName = property.Name;
                Loader.ColumnInfo column = new Loader.ColumnInfo();

                Type clrType;
                var clrTypeMatch = Mapping.CheckClrCompliance(property.PropertyType, out clrType, out exception);
                if (clrTypeMatch != Mapping.ClrTypeMatch.ClrMatch)
                {
                    continue;   
                }

                ClrMappingInfo mapping;
                if (property.PropertyType.IsNullable())
                {
                    column.IsNullable = true;
                    column.ValueType = Nullable.GetUnderlyingType(property.PropertyType);
                    mapping = Mapping.ClrMapping[column.ValueType];
                }
                else
                {
                    column.IsNullable = false;
                    column.ValueType = property.PropertyType;
                    mapping = Mapping.ClrMapping[property.PropertyType];
                }

                var match = schema.AsEnumerable()
                    .Where(a => Naming.GetClrName(a.Field<string>(_columnNameText)).EqualsCS(property.Name))  
                    .FirstOrDefault();

                // allow column mismatch - !
                if (match == null)
                {
                    continue;
                }

                if (duplicates.Contains(match.Field<string>(_columnNameText)))
                {
                    throw new QueryTalkException("Loader.ReflectReader", QueryTalkExceptionType.ColumnNameDuplicate,
                        String.Format("duplicate CLR name(s) = {0}", match.Field<string>(_columnNameText)));
                }

                int ordinal = match.Field<int>(_columnOrdinalText);
                column.ReaderGetMethod = typeof(SqlDataReader).GetMethod(mapping.SqlDataReaderGetMethodName,
                    new Type[] { typeof(int) });
                column.ReaderOrdinal = ordinal;
                column.Property = property;
                column.IsUnbox = mapping.IsUnbox;
                column.CtorParameterType = mapping.CtorParameterType;
                column.ColumnName = property.Name;
                columns.Add(column);
                ++i;
            }

            return columns.ToArray();
        }

        private static ColumnInfo[] ReflectRow(Type type, IDataRecord reader)
        {
            var properties = type.GetProperties();
            Type[] columnTypes = type.GetGenericArguments();
            Loader.ColumnInfo[] columns;
            string propertyName = Text.NotAvailable;
            int columnCount = columnTypes.Length;

 
            if (reader.FieldCount < columnTypes.Length)
            {
                throw new QueryTalkException("Loader.ReflectRow", QueryTalkExceptionType.NoMoreColumns,
                    String.Format("QtRow = {0}{1}   Number of columns in data source = {2}",
                        type, Environment.NewLine, reader.FieldCount));
            }

            try
            {
                QueryTalkException exception;
                columns = new Loader.ColumnInfo[columnCount];
                for (int i = 0; i < columnCount; ++i)
                {
                    Type columnType = columnTypes[i];

                    Loader.ColumnInfo column = new Loader.ColumnInfo();
                    PropertyInfo property = properties[i]; 
                    propertyName = property.Name;
                    Type clrType;

                    if (Mapping.CheckClrCompliance(columnType, out clrType, out exception) != Mapping.ClrTypeMatch.ClrMatch)
                    {
                        exception.Arguments = String.Format("class = {0}{1}   property = {2}{3}   {4}",
                            type, Environment.NewLine, property.Name, Environment.NewLine, exception.Arguments);
                        throw exception;
                    }

                    ClrMappingInfo mapping;
                    if (columnType.IsNullable())
                    {
                        column.IsNullable = true;
                        column.ValueType = Nullable.GetUnderlyingType(columnType); 
                        mapping = Mapping.ClrMapping[column.ValueType];
                    }
                    else
                    {
                        column.IsNullable = false;
                        column.ValueType = columnType;
                        mapping = Mapping.ClrMapping[columnType];
                    }

                    column.ReaderGetMethod = typeof(SqlDataReader).GetMethod(mapping.SqlDataReaderGetMethodName,
                        new Type[] { typeof(int) });
                    column.ReaderOrdinal = i;
                    column.Property = property;
                    column.IsUnbox = mapping.IsUnbox;
                    column.CtorParameterType = mapping.CtorParameterType;
                    column.ColumnName = property.Name;
                    columns[i] = column;
                }

                return columns;
            }
            catch (System.IndexOutOfRangeException)  // property name does not match any IDataReader field
            {
                throw new QueryTalkException("Reader.ReflectRow", QueryTalkExceptionType.MismatchedTargetColumn,
                    String.Format("class = {0}{1}   property = {2}", type, Environment.NewLine, propertyName));
            }
        }

        private static ColumnInfo[] ReflectReader(IDataRecord reader, out QueryTalkException exception)
        {
            exception = null;
            var columns = new List<Loader.ColumnInfo>();
            DataTable readerSchema = ((SqlDataReader)reader).GetSchemaTable();
            
            int i = 0;
            int index = 1;
            List<string> columnNames = new List<string>();
            foreach (var row in readerSchema.AsEnumerable())
            {
                Loader.ColumnInfo column = new ColumnInfo();
                column.ColumnName = Naming.GetClrName((string)row[_columnNameText]);

                if (String.IsNullOrEmpty(column.ColumnName))
                {
                    column.ColumnName = GetColumnName(readerSchema, ref index);
                }

                column.ReaderOrdinal = (int)row[_columnOrdinalText];
                column.ValueType = (Type)row[_dataTypeText];

                Type clrType;
                if (Mapping.CheckClrCompliance(column.ValueType, out clrType, out exception) != Mapping.ClrTypeMatch.ClrMatch)
                {
                    exception = null;
                    continue;
                }

                if ((bool)row[_allowDBNullText] && !column.ValueType.IsClass)
                {
                    column.IsNullable = true;
                    column.NullableType = Mapping.ClrMapping[column.ValueType].NullableType;
                }

                ClrMappingInfo mapping = Mapping.ClrMapping[column.ValueType];
                column.ReaderGetMethod = typeof(SqlDataReader).GetMethod(mapping.SqlDataReaderGetMethodName,
                    new Type[] { typeof(int) });
                column.IsUnbox = mapping.IsUnbox;
                column.CtorParameterType = mapping.CtorParameterType;

                // provide unique column name
                int j = 1;
                var renamed = column.ColumnName;
                while (columnNames.Contains(renamed))
                {
                    renamed = String.Format("{0}{1}{2}", 
                        column.ColumnName,
                        Text.Free.Renamed,
                        j > 1 ? j.ToString() : String.Empty);
                    ++j;
                }
                if (j > 1)
                {
                    column.ColumnName = renamed;
                }

                columns.Add(column);
                columnNames.Add(column.ColumnName);
                ++i;
            }

            return columns.ToArray();
        }

        private static string GetColumnName(DataTable schema, ref int index)
        {
            string columnName;
            do
            {
                columnName = String.Format("{0}{1}", Text.Column, index++);
            }
            while (schema.AsEnumerable().Where(row => (string)row[_columnNameText] == columnName).Any());

            return columnName;
        }

        #endregion

        #region Emit loaders

        private static Func<IDataRecord, T> EmitRowLoader<T>(ColumnInfo[] columns)
        {
            DynamicMethod method = new DynamicMethod("RowReader", typeof(T),
                new Type[] { typeof(IDataRecord) }, typeof(T), true);

            ILGenerator il = method.GetILGenerator();               // create IL generator
            LocalBuilder row = il.DeclareLocal(typeof(T));          // T row

            il.Emit(OpCodes.Newobj,
                typeof(T).GetConstructor(Type.EmptyTypes));         // emit row constructor
            il.Emit(OpCodes.Stloc, row);                            // store newly created row object

            int count = columns.Length;
            for (int i = 0; i < count; ++i)
            {
                ColumnInfo column = columns[i];                     // get column info

                Label endIfLabel = il.DefineLabel();                // define end label
                il.Emit(OpCodes.Ldarg_0);                           // load reader
                il.Emit(OpCodes.Ldc_I4, column.ReaderOrdinal);      // load ordinal as argument of reader's IsDBNull method
                il.Emit(OpCodes.Callvirt, _isDBNullMethod);         // call reader.IsDBNull(ordinal)
                il.Emit(OpCodes.Brtrue, endIfLabel);                // if method returns true goto label
                il.Emit(OpCodes.Ldloc, row);                        // load T object

                il.Emit(OpCodes.Ldarg_0);                           // load reader again
                il.Emit(OpCodes.Ldc_I4, column.ReaderOrdinal);      // load ordinal as argument of reader's get method
                il.Emit(OpCodes.Callvirt, column.ReaderGetMethod);  // call getXXXMethod as obtain value for the property

                if (column.IsUnbox)
                {
                    il.Emit(OpCodes.Unbox_Any, column.ValueType);
                }

                // handle nullable
                if (column.IsNullable)
                {
                    LocalBuilder nullable = il.DeclareLocal(column.Property.PropertyType);
                    LocalBuilder locValue = il.DeclareLocal(column.ValueType);
                    il.Emit(OpCodes.Stloc, locValue);               // store reader's value (from getXXMethod) locally
                    ConstructorInfo ctorOfNullable =
                        column.Property.PropertyType.GetConstructor(new[] { column.ValueType });
                    il.Emit(OpCodes.Ldloc, locValue);               // load value
                    il.Emit(OpCodes.Newobj, ctorOfNullable);        // create nullable object containing the value
                    il.Emit(OpCodes.Stloc, nullable);               // store nullable object
                    il.Emit(OpCodes.Ldloc, nullable);               // load nullable object
                }
                // if a property type's instance need to be created by a single parameter (of type CtorParameterType)
                // e.g. System.Data.Linq.Binary(byte[]) 
                else if (column.CtorParameterType != null)
                {
                    LocalBuilder creatable = il.DeclareLocal(column.Property.PropertyType);
                    LocalBuilder locValue = il.DeclareLocal(column.Property.PropertyType);
                    il.Emit(OpCodes.Stloc, locValue);               // store reader's value (from getXXMethod) locally
                    ConstructorInfo ctorOfProperty =
                        column.Property.PropertyType.GetConstructor(new[] { column.CtorParameterType });
                    il.Emit(OpCodes.Ldloc, locValue);               // load value
                    il.Emit(OpCodes.Newobj, ctorOfProperty);        // create property value object containing the value
                    il.Emit(OpCodes.Stloc, creatable);              // store new property value object
                    il.Emit(OpCodes.Ldloc, creatable);              // load new property value object
                }

                // on the top of the stack here is either value or nullable object

                il.Emit(OpCodes.Callvirt,
                    column.Property.GetSetMethod());                // call property's accessor set method using the stack value/nullable object
                il.MarkLabel(endIfLabel);                           // mark label
            }

            il.Emit(OpCodes.Ldloc, row);                            // load row object onto the stack - as method returning value
            il.Emit(OpCodes.Ret);                                   // return

            return (Func<IDataRecord, T>)method.CreateDelegate(
                typeof(Func<IDataRecord, T>));                      // return method delegate
        }

        private static Func<IDataRecord, dynamic> EmitRowLoaderDynamic(ColumnInfo[] columns, Type dynamicType)
        {
            DynamicMethod method = new DynamicMethod("RowLoader", dynamicType,
                new Type[] { typeof(IDataRecord) }, dynamicType, true);

            ILGenerator il = method.GetILGenerator();                // create IL generator
            LocalBuilder row = il.DeclareLocal(dynamicType);         // row as ExpandoObject

            il.Emit(OpCodes.Newobj,
                dynamicType.GetConstructor(Type.EmptyTypes));        // emit row constructor
            il.Emit(OpCodes.Stloc, row);                             // store newly created row object

            var properties = dynamicType.GetProperties();

            int count = columns.Length;
            for (int i = 0; i < count; ++i)
            {
                ColumnInfo column = columns[i];                      // get column info
                PropertyInfo property = properties[i];               // get property info (not included in columns)
                Label endIfLabel = il.DefineLabel();                 // define end label
                il.Emit(OpCodes.Ldarg_0);                            // load data reader as argument at index 0 of the isDBNullMethod(int)
                il.Emit(OpCodes.Ldc_I4, i);                          // load ordinal for first argument -> 1|0
                il.Emit(OpCodes.Callvirt, _isDBNullMethod);          // call isDBNull
                il.Emit(OpCodes.Brtrue, endIfLabel);                 // if returns true goto label
                il.Emit(OpCodes.Ldloc, row);                         // load T object
                il.Emit(OpCodes.Ldarg_0);                            // load data reader as argument at index 0 of getXXXMethod(int)
                il.Emit(OpCodes.Ldc_I4, column.ReaderOrdinal);       // load ordinal for ReaderGetMethod method
                il.Emit(OpCodes.Callvirt, column.ReaderGetMethod);   // call getXXXMethod -> value for the property

                if (column.IsUnbox)
                {
                    il.Emit(OpCodes.Unbox_Any, column.ValueType);
                }

                if (column.IsNullable)
                {
                    LocalBuilder nullable = il.DeclareLocal(column.NullableType);
                    LocalBuilder locValue = il.DeclareLocal(column.ValueType);
                    il.Emit(OpCodes.Stloc, locValue);                // store reader's value (from getXXMethod) locally
                    ConstructorInfo ctorOfNullable =
                        column.NullableType.GetConstructor(new[] { column.ValueType });
                    il.Emit(OpCodes.Ldloc, locValue);                // load value ->
                    il.Emit(OpCodes.Newobj, ctorOfNullable);         // -> and create nullable object containing that value
                    il.Emit(OpCodes.Stloc, nullable);                // store nullable object
                    il.Emit(OpCodes.Ldloc, nullable);                // load nullable object
                }

                il.Emit(OpCodes.Callvirt,
                    property.GetSetMethod());                        // call property's accessor set method 
                il.MarkLabel(endIfLabel);                            // mark label
            }

            il.Emit(OpCodes.Ldloc, row);                             // load row object onto the stack - as method returning value
            il.Emit(OpCodes.Ret);                                    // return

            return (Func<IDataRecord, dynamic>)method.CreateDelegate(
                typeof(Func<IDataRecord, dynamic>));                 // return method delegate
        }

        #endregion

        #region Emit table

        private static Type EmitTable(ColumnInfo[] columns, Guid tableTypeGuid)
        {
            AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = _dynamicAssemblyName;
            var assemblyBuilder = 
                Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder module =
                assemblyBuilder.DefineDynamicModule(_dynamicModuleName);

            string typeName = String.Format("{0}.d{1}", 
                assemblyName.Name,
                Regex.Replace(tableTypeGuid.ToString(), "-", ""));
            TypeBuilder typeBuilder =
                module.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class);

            MethodAttributes getSetAttr =
                MethodAttributes.Public | MethodAttributes.HideBySig;

            foreach (var column in columns)
            {
                Type columnType = column.IsNullable ? column.NullableType : column.ValueType;

                FieldBuilder fieldBuilder = typeBuilder.DefineField(
                    "_" + column.ColumnName,
                    columnType,
                    FieldAttributes.Private);

                // add DebuggerBrowsableAttribute to every field (do not show fields in the debugger)
                Type debuggerAttribute = typeof(System.Diagnostics.DebuggerBrowsableAttribute);
                ConstructorInfo debuggerCtorInfo = debuggerAttribute.GetConstructor(new Type[1] { typeof(System.Diagnostics.DebuggerBrowsableState) });
                CustomAttributeBuilder attributeBuilder = 
                    new CustomAttributeBuilder(debuggerCtorInfo, new object[1] { System.Diagnostics.DebuggerBrowsableState.Never });
                fieldBuilder.SetCustomAttribute(attributeBuilder);

                PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(
                    column.ColumnName,
                    System.Reflection.PropertyAttributes.None,
                    columnType,
                    null);

                MethodBuilder getValueMethodBuilder =
                    typeBuilder.DefineMethod(_getValueMethodName,
                        getSetAttr,
                        columnType,
                        Type.EmptyTypes);

                ILGenerator currGetIL = getValueMethodBuilder.GetILGenerator();
                currGetIL.Emit(OpCodes.Ldarg_0);
                currGetIL.Emit(OpCodes.Ldfld, fieldBuilder);
                currGetIL.Emit(OpCodes.Ret);

                MethodBuilder setValueMethodBuilder =
                    typeBuilder.DefineMethod(_setValueMethodName,
                        getSetAttr,
                        null,
                        new Type[] { columnType });

                ILGenerator currSetIL = setValueMethodBuilder.GetILGenerator();
                currSetIL.Emit(OpCodes.Ldarg_0);
                currSetIL.Emit(OpCodes.Ldarg_1);
                currSetIL.Emit(OpCodes.Stfld, fieldBuilder);
                currSetIL.Emit(OpCodes.Ret);

                propertyBuilder.SetGetMethod(getValueMethodBuilder);
                propertyBuilder.SetSetMethod(setValueMethodBuilder);
            }

            return typeBuilder.CreateType();
        }

        #endregion

        #region Analyser

        // The only purpose of this method is to give the client more accurate information about the target property
        // which caused the InvalidCastException exception. This method may even fail with inner exception and/or return null, 
        // and in that case the less accurate InvalidCastException info will be shown to the client.
        internal static Row<string, string, string> AnalyseInvalidCastException(Connectable connectable, Type type, int tableIndex)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(connectable.ConnectionString))
                {
                    cn.Open();

                    Importer.ExecuteBulkInsert(connectable, cn);
                    using (SqlCommand cmd = new SqlCommand(connectable.Sql, cn))
                    {
                        cmd.CommandTimeout = connectable.CommandTimeout;
                        int i = 0;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (i++ != tableIndex)
                            {
                                if (!reader.NextResult())
                                {
                                    return null;
                                }
                            }

                            DataTable schema = reader.GetSchemaTable();
                            int hash = GetLoaderHash(reader);
                            var cacheKey = Cache.CreateLoaderCacheKey(hash, type);
                            Cache.LoaderCacheValue cacheValue;

                            if (!Cache.Loaders.TryGetValue(cacheKey, out cacheValue))
                            {
                                return null;    // do not continue with analysis if cache does not exist (not likely)
                            }

                            int j = 0;
                            foreach (var column in cacheValue.Columns)
                            {
                                DataRow row;
                                if (Row.IsRow(type))
                                {
                                    row = schema.Rows[j++];
                                }
                                else
                                {
                                    row = schema.AsEnumerable()
                                        .Where(a => a.Field<string>(_columnNameText).EqualsCS(column.ColumnName))  
                                        .Select(a => a)
                                        .FirstOrDefault();
                                }

                                if (row != null)
                                {
                                    string sqlTypeName = row.Field<string>(_dataTypeNameText);
                                    var clrInfo = Mapping.ClrMapping[column.ValueType];
                                    bool ismatch = false;
                                    foreach (var dbtype in clrInfo.DTypes)
                                    {
                                        if (Mapping.SqlMapping[dbtype].SqlPlain.EqualsCS(sqlTypeName, false))
                                        {
                                            ismatch = true;
                                            break;
                                        }
                                    }

                                    if (!ismatch)
                                    {
                                        return Row.Create(column.ColumnName, column.ValueType.ToString(), sqlTypeName);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // safe: the "real" exception has already been caught
            catch { }

            return null;
        }

        #endregion

        #region SEMQ

        internal static void SetTableAsLoaded<T>(IEnumerable<T> result)
        {
            if (result.Count() == 0)
            {
                return;
            }

            var example = result.First();  

            if (example is DbRow)
            {
                foreach (var row in result)
                {
                    ((DbRow)(object)row).SetLoadedByLoader();
                }
            }
        }

        internal static void SetRowAsLoaded<T>(T row)
        {
            if (row == null)
            {
                return;
            }

            if (row is DbRow)
            {
                ((DbRow)(object)row).SetLoadedByLoader();
            }
        }

        #endregion

        #region Exceptions

        internal static void TryThrowException(QueryTalkException exception, Connectable connectable)
        {
            if (exception != null)
            {
                exception.ObjectName = ((IName)connectable.Executable).Name;
                exception.Method = Text.Method.Go;
                throw exception;
            }
        }

        internal static void TryThrowInvalidDataClassException(Type type, ColumnInfo[] columns)
        {
            if (columns.Count() == 0)
            {
                throw new QueryTalkException("Loader", QueryTalkExceptionType.InvalidDataClass, null, 
                   Text.Method.GoOrGoAsync, String.Format("data class = {0}", type == null ? "dynamic" : type.ToString()));
            }
        }

        // Throw InvalidSqlOperationException if SQL error number is 305 or 402.
        // SQL server does not allow that the following data types:
        //    - xml
        //    - text
        //    - ntext
        //    - image
        // are used in the predicates (e.g. equal operator).
        internal static void TryThrowInvalidSqlOperationException(QueryTalkException ex, string objectName, string method = null)
        {
            var errorNumber = ex.SqlErrorNumber;
            if (errorNumber == 305 || errorNumber == 402)
            {
                throw new QueryTalkException("Loader", QueryTalkExceptionType.InvalidSqlOperationException,
                    objectName, method ?? ex.Method, null);
            }
        }

        #endregion

    }
}
