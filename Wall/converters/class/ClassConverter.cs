#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace QueryTalk.Wall
{
    // Converts .NET data from one class to another.
    internal class ClassConverter
    {
        private static void TryCheckClassType(Type type)
        {
            if (type.IsClass)
            {
                return;
            }

            throw new QueryTalkException("ClassConverter.TryCheckClassType", QueryTalkExceptionType.InvalidPack,
                String.Format("type = {0}", type), Text.Method.Pack);
        }

        internal static T SetClass<T>(T target, object source)
            
        {
            if (target == null)
            {
                throw new QueryTalkException("ClassConverter.ToClass", QueryTalkExceptionType.ArgumentNull,
                    "target= null", Text.Method.Pack);
            }

            if (source == null)
            {
                throw new QueryTalkException("ClassConverter.ToClass", QueryTalkExceptionType.ArgumentNull,
                    "source = null", Text.Method.Pack);
            }

            if (target != null && object.ReferenceEquals(target, source))
            {
                return target;
            }

            Type targetType = target.GetType();
            Type sourceType = source.GetType();

            TryCheckClassType(targetType);
            TryCheckClassType(sourceType);

            var sourceProperties = sourceType.GetReadableProperties();
            var targetProperties = targetType.GetWritableProperties();

            if (targetProperties.Length == 0)
            {
                throw new QueryTalkException("ClassConverter.TryCheckClassType", QueryTalkExceptionType.InvalidPack,
                    String.Format("type = {0}", targetType), Text.Method.Pack);
            }
            if (sourceProperties.Length == 0)
            {
                throw new QueryTalkException("ClassConverter.TryCheckClassType", QueryTalkExceptionType.InvalidPack,
                    String.Format("type = {0}", sourceType), Text.Method.Pack);
            }

            bool match = false;
            Type clrType;
            QueryTalkException exception;

            foreach (var targetProperty in targetProperties)
            {
                var clrTypeMatch = Mapping.CheckClrCompliance(targetProperty.PropertyType, out clrType, out exception);
                if (clrTypeMatch != Mapping.ClrTypeMatch.ClrMatch)
                {
                    continue;
                }

                var sourceProperty = sourceProperties.Where(p => p.Name == targetProperty.Name).FirstOrDefault();
                if (sourceProperty != null)
                {
                    if (Common.GetClrType(sourceProperty.PropertyType) != Common.GetClrType(targetProperty.PropertyType))
                    {
                        throw new QueryTalkException("ClassConverter.ToClass", QueryTalkExceptionType.PackPropertyMismatch,
                            String.Format("target property name = {0}{1}   target property type = {2}{1}   source property type = {3}",
                                targetProperty.Name, Environment.NewLine, targetProperty.PropertyType, sourceProperty.PropertyType), 
                                    Text.Method.Pack);
                    }

                    match = true; 
                    targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
                }
            }

            if (!match)
            {
                throw new QueryTalkException("ClassConverter.TryCheckClassType", QueryTalkExceptionType.InvalidPack,
                    String.Format("target type = {0}{1}   source type = {2}",
                        targetType, Environment.NewLine, sourceType), 
                            Text.Method.Pack);
            }

            return target;
        }

        internal static IEnumerable<T> PackRows<T>(IEnumerable source)
            where T : new()
        {
            if (source == null)
            {
                return null;
            }

            Type targetType = typeof(T);
            Type sourceType = null;

            foreach (var row in source)
            {
                if (row == null)
                {
                    continue;
                }

                sourceType = row.GetType();
                break;
            }

            if (sourceType == null)
            {
                return null;   
            }

            TryCheckClassType(targetType);
            TryCheckClassType(sourceType);

            var sourceProperties = sourceType.GetReadableProperties();
            var targetProperties = targetType.GetWritableProperties();

            if (targetProperties.Length == 0)
            {
                throw new QueryTalkException("ClassConverter.TryCheckClassType", QueryTalkExceptionType.InvalidPack,
                    String.Format("type = {0}", targetType), Text.Method.Pack);
            }
            if (sourceProperties.Length == 0)
            {
                throw new QueryTalkException("ClassConverter.TryCheckClassType", QueryTalkExceptionType.InvalidPack,
                    String.Format("type = {0}", sourceType), Text.Method.Pack);
            }

            var sourceGetters = new List<IPropertyAccessor>();
            var targetGetters = new List<IPropertyAccessor>();

            bool match = false;
            Type clrType;
            QueryTalkException exception;
            var numberOfPropertiesUsed = 0;

            foreach (var targetProperty in targetProperties)
            {
                var clrTypeMatch = Mapping.CheckClrCompliance(targetProperty.PropertyType, out clrType, out exception);
                if (clrTypeMatch != Mapping.ClrTypeMatch.ClrMatch)
                {
                    continue;
                }

                var sourceProperty = sourceProperties.Where(p => p.Name == targetProperty.Name).FirstOrDefault();
                if (sourceProperty != null)
                {
                    if (Common.GetClrType(sourceProperty.PropertyType) != Common.GetClrType(targetProperty.PropertyType))
                    {
                        throw new QueryTalkException("ClassConverter.ToClass", QueryTalkExceptionType.PackPropertyMismatch,
                            String.Format("target property name = {0}{1}   target property type = {2}{1}   source property type = {3}",
                                targetProperty.Name, Environment.NewLine, targetProperty.PropertyType, sourceProperty.PropertyType),
                                    Text.Method.Pack);
                    }

                    match = true;  

                    sourceGetters.Add(PropertyAccessor.Create(sourceType, sourceProperty));
                    targetGetters.Add(PropertyAccessor.Create(targetType, targetProperty));
                    ++numberOfPropertiesUsed;
                }
            }

            if (!match)
            {
                throw new QueryTalkException("ClassConverter.TryCheckClassType", QueryTalkExceptionType.InvalidPack,
                    String.Format("target type = {0}{1}   source type = {2}",
                        targetType, Environment.NewLine, sourceType),
                            Text.Method.Pack);
            }

            var rows = new HashSet<T>();
            foreach (var sourceRow in source)
            {
                if (sourceRow == null)
                {
                    continue;
                }

                var targetRow = new T();

                for (int i = 0; i < numberOfPropertiesUsed; i++)
                {
                    var value = sourceGetters[i].GetValue(sourceRow);
                    targetGetters[i].SetValue(targetRow, value);
                }

                rows.Add(targetRow);
            }

            return rows;
        }

    }
}
