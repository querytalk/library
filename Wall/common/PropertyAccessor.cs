#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QueryTalk.Wall
{
    // based on https://github.com/chaquotay/PropertyAccess
    internal interface IPropertyAccessor
    {
        object GetValue(object target);
        void SetValue(object target, object value);
    }

    // based on https://github.com/chaquotay/PropertyAccess
    internal class PropertyAccessor<TTarget, TProperty> : IPropertyAccessor
    {
        #region Delegates

        private delegate TProperty PropertyGetter(TTarget target);
        private delegate TProperty StaticPropertyGetter();
        private delegate void PropertySetter(TTarget target, TProperty value);
        private delegate void StaticPropertySetter(TProperty value);

        #endregion

        private readonly PropertyGetter _getter;
        private readonly PropertySetter _setter;

        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            _getter = CreateGetter(propertyInfo);
            _setter = CreateSetter(propertyInfo);
        }

        // property getter
        private static PropertyGetter CreateGetter(PropertyInfo property)
        {
            var getMethod = property.GetGetMethod();
            if (property.CanRead && getMethod != null)
            {
                if (getMethod.IsStatic)
                {
                    var staticGetter = (StaticPropertyGetter)Delegate.CreateDelegate(typeof(StaticPropertyGetter), getMethod);
                    return target => staticGetter.Invoke();
                }
                else
                {
                    return (PropertyGetter)Delegate.CreateDelegate(typeof(PropertyGetter), getMethod);
                }
            }
            else
            {
                return target =>
                {
                    throw new QueryTalkException(
                        "PropertyAccessor.CreateGetter",
                        QueryTalkExceptionType.NonAccessableProperty,
                        String.Format("property class = {0}{1}   property = {2}",
                            typeof(TTarget),
                            Environment.NewLine,
                            property.Name));
                };
            }
        }

        // property setter
        private static PropertySetter CreateSetter(PropertyInfo property)
        {
            var setMethod = property.GetSetMethod();
            if (property.CanWrite && setMethod != null)
            {
                if (setMethod.IsStatic)
                {
                    var staticSetter = (StaticPropertySetter)Delegate.CreateDelegate(typeof(StaticPropertySetter), setMethod);
                    return (target, value) => staticSetter.Invoke(value);
                }
                else
                {
                    return (PropertySetter)Delegate.CreateDelegate(typeof(PropertySetter), setMethod);
                }
            }
            else
            {
                return (target, value) =>
                {
                    throw new QueryTalkException(
                        "PropertyAccessor.CreateSetter",
                        QueryTalkExceptionType.NonAccessableProperty,
                        String.Format("property class = {0}{1}   property = {2}",
                            typeof(TTarget),
                            Environment.NewLine,
                            property.Name));
                };
            }
        }

        #region IPropertyAccessor

        internal TProperty GetValue(TTarget target)
        {
            return _getter.Invoke(target);
        }

        public object GetValue(object target)
        {
            return GetValue((TTarget)target);
        }

        internal void SetValue(TTarget target, TProperty value)
        {
            _setter.Invoke(target, value);
        }

        public void SetValue(object target, object value)
        {
            SetValue((TTarget)target, (TProperty)value);
        }

        #endregion

    }

    internal class PropertyAccessor
    {
        internal static IPropertyAccessor Create(Type targetType, PropertyInfo property)
        {
            var type = typeof(PropertyAccessor<,>).MakeGenericType(targetType, property.PropertyType);
            return (IPropertyAccessor)Activator.CreateInstance(type, property);
        }

        #region GetValues

        // get property values of a given object
        internal static object[] GetValues(object obj)
        {
            if (obj == null)
            {
                throw new QueryTalkException("PropertyAccessor.GetValues",
                    QueryTalkExceptionType.NullArgumentInnerException,
                        "argument = null", "PropertyAccessor.GetValues");
            }

            var type = obj.GetType();
            List<IPropertyAccessor> accessors = null;
            bool cached = Cache.PropertyAccessors.TryGetValue(type, out accessors);
            if (!cached)
            {
                accessors = new List<IPropertyAccessor>();
                var properties = type.GetSortedReadableProperties();
                Type clrType; QueryTalkException exception;
                foreach (var property in properties
                    .OrderBy(p => p.Name)) 
                {
                    if (Mapping.CheckClrCompliance(property.PropertyType, out clrType, out exception) == Mapping.ClrTypeMatch.ClrMatch)
                    {
                        accessors.Add(PropertyAccessor.Create(type, property));
                    }
                }

                Cache.PropertyAccessors[type] = accessors;
            }
            else
            { }

            List<object> values = new List<object>();
            foreach (var accessor in accessors)
            {
                values.Add(accessor.GetValue(obj));
            }

            return values.ToArray();
        }

        // get property values of a given object by excluding properties from the specified list
        internal static object[] GetValues(object obj, params string[] excludedColumns)
        {
            if (obj == null)
            {
                throw new QueryTalkException("PropertyAccessor.GetValues", QueryTalkExceptionType.NullArgumentInnerException,
                    "argument = null", "PropertyAccessor.GetValues");
            }

            var type = obj.GetType();
            List<IPropertyAccessor> accessors = null;
            bool cached = Cache.PropertyAccessors.TryGetValue(type, out accessors);
            if (!cached)
            {
                accessors = new List<IPropertyAccessor>();
                var properties = type.GetSortedReadableProperties();
                Type clrType; QueryTalkException exception;
                foreach (var property in properties
                    .Where(p => !excludedColumns.Contains(p.Name))
                    .OrderBy(p => p.Name))  
                {
                    if (Mapping.CheckClrCompliance(property.PropertyType, out clrType, out exception) == Mapping.ClrTypeMatch.ClrMatch)
                    {
                        accessors.Add(PropertyAccessor.Create(type, property));
                    }
                }

                Cache.PropertyAccessors[type] = accessors;
            }
            else
            { }

            List<object> values = new List<object>();
            foreach (var accessor in accessors)
            {
                values.Add(accessor.GetValue(obj));
            }

            return values.ToArray();
        }

        #endregion

        #region SetValues

        // set property values of a given object
        internal static void SetValues(object obj, object[] values)
        {
            if (obj == null)
            {
                throw new QueryTalkException("PropertyAccessor.GetValues", QueryTalkExceptionType.NullArgumentInnerException, 
                    "argument = null", "PropertyAccessor.SetValues");
            }

            var type = obj.GetType();
            List<IPropertyAccessor> accessors = null;
            bool cached = Cache.PropertyAccessors.TryGetValue(type, out accessors);
            if (!cached)
            {
                accessors = new List<IPropertyAccessor>();
                var properties = type.GetSortedReadableProperties();
                Type clrType; QueryTalkException exception;
                foreach (var property in properties
                    .OrderBy(p => p.Name))  
                {
                    if (Mapping.CheckClrCompliance(property.PropertyType, out clrType, out exception) == Mapping.ClrTypeMatch.ClrMatch)
                    {
                        accessors.Add(PropertyAccessor.Create(type, property));
                    }
                }

                Cache.PropertyAccessors[type] = accessors;
            }

            var i = 0;
            foreach (var accessor in accessors)
            {
                accessor.SetValue(obj, values[i++]);
            }
        }

        // set property values of a given object applying to the properties with the indexes from the specified list
        internal static void SetValues(object obj, object[] values, int[] columns)
        {
            if (obj == null)
            {
                throw new QueryTalkException("PropertyAccessor.GetValues",
                    QueryTalkExceptionType.NullArgumentInnerException,
                        "argument = null", "PropertyAccessor.SetValues");
            }

            var type = obj.GetType();
            List<IPropertyAccessor> accessors = null;
            bool cached = Cache.PropertyAccessors.TryGetValue(type, out accessors);
            if (!cached)
            {
                accessors = new List<IPropertyAccessor>();
                var properties = type.GetSortedReadableProperties();
                Type clrType; QueryTalkException exception;
                foreach (var property in properties
                    .OrderBy(p => p.Name))  
                {
                    if (Mapping.CheckClrCompliance(property.PropertyType, out clrType, out exception) == Mapping.ClrTypeMatch.ClrMatch)
                    {
                        accessors.Add(PropertyAccessor.Create(type, property));
                    }
                }

                Cache.PropertyAccessors[type] = accessors;
            }

            foreach (var index in columns)
            {
                accessors[index-1].SetValue(obj, values[index-1]);
            }
        }

        #endregion

    }

}
