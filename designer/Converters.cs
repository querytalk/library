#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections;
using System.Collections.Generic;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {

        #region ToView

        /// <summary>
        /// Converts the specified data to the data view.
        /// </summary>
        /// <typeparam name="T">The type of the object to convert.</typeparam>
        /// <param name="data">The data to convert.</param>
        public static View ToView<T>(T data)
        {
            return ViewConverter.ToView<T>(data);
        }

        // internal: used in table design (.DesignTable)
        internal static View ToView<T>()
        {
            return ViewConverter.ToView<T>(default(T));
        }

        #endregion

        #region ToJson

        /// <summary>
        /// Converts the specified data to a JSON string.
        /// </summary>
        /// <typeparam name="T">The type of the object to convert.</typeparam>
        /// <param name="data">The data to convert.</param>
        public static string ToJson<T>(T data)
        {
            return JsonConverter.ToJson<T>(data);
        }

        #endregion

        #region Pack

        /// <summary>
        /// Converts the specified object to an object of another class.
        /// </summary>
        /// <typeparam name="T">The type of the target to convert to.</typeparam>
        /// <param name="source">The object to convert.</param>
        public static T Pack<T>(object source)
            where T : new()
        {
            // allow null
            if (source == null)
            {
                return default(T);
            }

            var target = new T();
            ClassConverter.SetClass(target, source);
            return target;
        }

        /// <summary>
        /// Converts the specified object to an object of another class.
        /// </summary>
        /// <typeparam name="T">The type of the target to convert to.</typeparam>
        /// <param name="target">The target object to convert to.</param>
        /// <param name="source">The object to convert.</param>
        public static T Pack<T>(T target, object source)
            where T : new()
        {
            // allow null
            if (source == null)
            {
                return target;
            }

            return ClassConverter.SetClass(target, source);
        }

        /// <summary>
        /// Converts the specified objects to objects of another class.
        /// </summary>
        /// <typeparam name="T">The type of the target to convert to.</typeparam>
        /// <param name="source">The source objects to convert.</param>
        public static IEnumerable<T> PackRows<T>(IEnumerable source)
            where T : new()
        {
            return ClassConverter.PackRows<T>(source);
        }

        #endregion

    }
}