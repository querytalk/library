#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data;

namespace QueryTalk.Wall
{
    internal static class Cache
    {
        internal enum LoaderType 
        { 
            Dynamic,  
            Row,       
            Class   
        }

        internal static LoaderType GetLoaderType(Type type)
        {
            if (type == typeof(System.Object))
            {
                return LoaderType.Dynamic;
            }
            else if (Row.IsRow(type))
            {
                return LoaderType.Row;
            }
            else
            {
                return LoaderType.Class;
            }
        }

        internal struct LoaderCacheKey : IEquatable<LoaderCacheKey>
        {
            private int _sqlHash;
            internal int SqlHash
            {
                get { return _sqlHash; }
            }

            private LoaderType _loaderType;
            internal LoaderType LoaderType 
            {
                get { return _loaderType; } 
            }

            private Type _type;
            internal Type Type
            {
                get { return _type; }
            }

            internal static LoaderCacheKey Default
            {
                get
                {
                    return new LoaderCacheKey(0, Cache.LoaderType.Dynamic, null);
                }
            }

            internal LoaderCacheKey(int sqlHash, LoaderType loaderType, Type type)
            {
                _sqlHash = sqlHash;
                _loaderType = loaderType;
                _type = type;
            }

            #region IEquatable

            public override bool Equals(object obj)
            {
                if (obj is LoaderCacheKey)
                {
                    return this.Equals((LoaderCacheKey)obj);
                }

                return false;
            }

            public bool Equals(LoaderCacheKey key)
            {
                return
                    (SqlHash == key.SqlHash) &&
                    (LoaderType == key.LoaderType) &&
                    (Type == key.Type);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + SqlHash;
                    hash = hash * 23 + Type.GetHashCode();
                    hash = hash * 23 + LoaderType.GetHashCode();
                    return hash;
                }
            }

            public static bool operator ==(LoaderCacheKey key1, LoaderCacheKey key2)
            {
                return key1.Equals(key2);
            }

            public static bool operator !=(LoaderCacheKey key1, LoaderCacheKey key2)
            {
                return !(key1.Equals(key2));
            }

            #endregion

        }

        internal static LoaderCacheKey CreateLoaderCacheKey(int hash, Type type)
        {
            return new LoaderCacheKey(hash, GetLoaderType(type), type);
        }

        internal class LoaderCacheValue
        {
            internal object Loader { get; set; }

            internal Loader.ColumnInfo[] Columns { get; set; }

            internal LoaderCacheValue(Loader.ColumnInfo[] columns, object loader)
            {
                Columns = columns;
                Loader = loader;
            }
        }

        internal static bool TryGetLoaderCache<T>(IDataRecord reader, Cache.LoaderCacheKey cacheKey, out Cache.LoaderCacheValue cacheValue)
        {
            if (Cache.Loaders.TryGetValue(cacheKey, out cacheValue))
            {
                try
                {
                    ((Func<IDataRecord, T>)cacheValue.Loader).Invoke(reader);
                    return true;   
                }
                // safe: 
                //   The exception is intercepted here only once in order to generate new loader code.
                //   (Very unlikely to happen.)
                catch
                {
                    Cache.Loaders.TryRemove(cacheKey, out cacheValue);
                    return false;   
                }
            }

            return false;   
        }

        internal static ConcurrentDictionary<LoaderCacheKey, LoaderCacheValue> Loaders { get; set; }

        internal static ConcurrentDictionary<Type, List<IPropertyAccessor>> PropertyAccessors { get; set; }

        internal static ConcurrentDictionary<Type, List<JsonProperty>> JsonCache { get; set; }

        internal static ConcurrentDictionary<Type, List<IPropertyAccessor>> IRowAccessors { get; set; }

        internal static HashSet<CrudProcedure> CrudProcedures { get; set; }

        static Cache()
        {
            Loaders = new ConcurrentDictionary<LoaderCacheKey, LoaderCacheValue>();
            PropertyAccessors = new ConcurrentDictionary<Type, List<IPropertyAccessor>>();
            JsonCache = new ConcurrentDictionary<Type, List<JsonProperty>>();
            IRowAccessors = new ConcurrentDictionary<Type, List<IPropertyAccessor>>();
            CrudProcedures = new HashSet<CrudProcedure>(new CrudProcedureEqualityComparer());
        }
    }
}

