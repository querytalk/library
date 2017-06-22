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
        /// Declares the parameter using the specified value.
        /// </summary>
        /// <param name="value">Is the value whose type is used to infer the parameter type.</param>
        public static Value Param(System.Boolean value)
        {
            return new Value(value, Parameterization.Param);
        }

        /// <summary>
        /// Declares the parameter using the specified value.
        /// </summary>
        /// <param name="value">Is the value whose type is used to infer the parameter type.</param>
        public static Value Param(System.Byte value)
        {
            return new Value(value, Parameterization.Param);
        }

        /// <summary>
        /// Declares the parameter using the specified value.
        /// </summary>
        /// <param name="value">Is the value whose type is used to infer the parameter type.</param>
        public static Value Param(System.Byte[] value)
        {
            return new Value(value, Parameterization.Param);
        }

        /// <summary>
        /// Declares the parameter using the specified value.
        /// </summary>
        /// <param name="value">Is the value whose type is used to infer the parameter type.</param>
        public static Value Param(System.DateTime value)
        {
            return new Value(value, Parameterization.Param);
        }

        /// <summary>
        /// Declares the parameter using the specified value.
        /// </summary>
        /// <param name="value">Is the value whose type is used to infer the parameter type.</param>
        public static Value Param(System.DateTimeOffset value)
        {
            return new Value(value, Parameterization.Param);
        }

        /// <summary>
        /// Declares the parameter using the specified value.
        /// </summary>
        /// <param name="value">Is the value whose type is used to infer the parameter type.</param>
        public static Value Param(System.Decimal value)
        {
            return new Value(value, Parameterization.Param);
        }
        
        /// <summary>
        /// Declares the parameter using the specified value.
        /// </summary>
        /// <param name="value">Is the value whose type is used to infer the parameter type.</param>
        public static Value Param(System.Double value)
        {
            return new Value(value, Parameterization.Param);
        }

        /// <summary>
        /// Declares the parameter using the specified value.
        /// </summary>
        /// <param name="value">Is the value whose type is used to infer the parameter type.</param>
        public static Value Param(System.Guid value)
        {
            return new Value(value, Parameterization.Param);
        }

        /// <summary>
        /// Declares the parameter using the specified value.
        /// </summary>
        /// <param name="value">Is the value whose type is used to infer the parameter type.</param>
        public static Value Param(System.Int16 value)
        {
            return new Value(value, Parameterization.Param);
        }

        /// <summary>
        /// Declares the parameter using the specified value.
        /// </summary>
        /// <param name="value">Is the value whose type is used to infer the parameter type.</param>
        public static Value Param(System.Int32 value)
        {
            return new Value(value, Parameterization.Param);
        }

        /// <summary>
        /// Declares the parameter using the specified value.
        /// </summary>
        /// <param name="value">Is the value whose type is used to infer the parameter type.</param>
        public static Value Param(System.Int64 value)
        {
            return new Value(value, Parameterization.Param);
        }

        /// <summary>
        /// Declares the parameter using the specified value.
        /// </summary>
        /// <param name="value">Is the value whose type is used to infer the parameter type.</param>
        public static Value Param(System.Single value)
        {
            return new Value(value, Parameterization.Param);
        }

        /// <summary>
        /// Declares the parameter using the specified value.
        /// </summary>
        /// <param name="value">Is the value whose type is used to infer the parameter type.</param>
        public static Value Param(System.String value)
        {
            return new Value(value, Parameterization.Param);
        }

        /// <summary>
        /// Declares the parameter using the specified value.
        /// </summary>
        /// <param name="value">Is the value whose type is used to infer the parameter type.</param>
        public static Value Param(System.TimeSpan value)
        {
            return new Value(value, Parameterization.Param);
        }

        /// <summary>
        /// Declares the parameter using the specified value.
        /// </summary>
        /// <param name="value">Is the value whose type is used to infer the parameter type.</param>
        public static Value Param(System.Object value)
        {
            return new Value(value, Parameterization.Param);
        }

    }
}