#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;

namespace QueryTalk.Wall
{
    /// <summary>
    /// Encapsulates the public static members of the API class. (This API is intended to be used by the mapper application.)
    /// </summary>
    public static class Api
    {

        #region class:DbMapping

        /// <summary>
        /// Adds a database to the mapping data.
        /// </summary>
        /// <param name="map">A database mapping object.</param>
        public static DatabaseMap AddDatabase(DatabaseMap map)
        {
            return DbMapping.AddDatabase(map);
        }

        /// <summary>
        /// Adds a row type to the mapping data.
        /// </summary>
        /// <param name="type">A type of the row.</param>
        /// <param name="nodeID">A node mapping identifier that row belongs to.</param>
        public static void AddRowType(Type type, DB3 nodeID)
        {
            DbMapping.AddRowType(type, nodeID);
        }

        /// <summary>
        /// Adds a node to the mapping data.
        /// </summary>
        /// <param name="map">A node mapping object.</param>
        public static void AddNode(NodeMap map)
        {
            DbMapping.AddNode(map);
        }

        /// <summary>
        /// Adds a link to the mapping data.
        /// </summary>
        /// <param name="link">A link mapping object.</param>
        public static Link AddLink(Link link)
        {
            return DbMapping.AddLink(link);
        }

        /// <summary>
        /// Adds a node invoker to the mapping data.
        /// </summary>
        /// <param name="invoker">A mapping object of an invoker.</param>
        public static void AddNodeInvoker(NodeInvoker invoker)
        {
            DbMapping.AddNodeInvoker(invoker);
        }

        /// <summary>
        /// Adds a graph invoker to the mapping data.
        /// </summary>
        /// <param name="invoker">A mapping object of a graph invoker.</param>
        public static void AddGraphInvoker(GraphInvoker invoker)
        {
            DbMapping.AddGraphInvoker(invoker);
        }

        #endregion

        #region class:Naming

        /// <summary>
        /// Checks the CLR validity of a specified name.
        /// </summary>
        /// <param name="name">The name to check.</param>
        public static bool IsValidClrName(string name)
        {
            return Naming.IsValidClrName(name);
        }

        /// <summary>
        /// Converts the SQL name to the CLR name.
        /// </summary>
        /// <param name="name">The SQL name to convert to the CLR name.</param>
        public static string GetClrName(string name)
        {
            return Naming.GetClrName(name);
        }

        /// <summary>
        /// Checks the CLR validity of a specified column name.
        /// </summary>
        /// <param name="name">The column name to check.</param>
        /// <param name="tableName">The name of the table of the specified column.</param>
        public static bool IsValidClrColumnName(string name, string tableName)
        {
            return Naming.IsValidClrColumnName(name, tableName);
        }

        /// <summary>
        /// Converts the SQL column name to the CLR name.
        /// </summary>
        /// <param name="name">The SQL name to convert to the CLR name.</param>
        /// <param name="names">The names of the columns (of the same table) that has already been processed.</param>
        /// <param name="tableName">The name of the table of the specified column.</param>
        /// <param name="databaseName">The name of the database.</param>
        public static string GetClrColumnName(string name, HashSet<string> names, string tableName, string databaseName)
        {
            return Naming.GetClrColumnName(name, names, tableName, databaseName);
        }

        #endregion

        #region class:Common

        /// <summary>
        /// Conditionally converts a string value into upper case.
        /// </summary>
        /// <param name="value">A value to convert.</param>
        /// <param name="isCaseSensitive">If true, then the conversion is ignored.</param>
        public static string ToUpperCS(string value, bool isCaseSensitive = true)
        {
            return Common.ToUpperCS(value, isCaseSensitive);
        }

        /// <summary>
        /// Determines whether the two string values are equal.
        /// </summary>
        /// <param name="value1">The first value to compare.</param>
        /// <param name="value2">The second value to compare.</param>
        /// <param name="isCaseSensitive">Defines the type of the comparison.</param>
        public static bool EqualsCS(string value1, string value2, bool isCaseSensitive = true)
        {
            return Common.EqualsCS(value1, value2, isCaseSensitive);
        }

        #endregion

        #region Call

        /// <summary>
        /// Invokes the specified method.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        public static void Call(Action method)
        {
            PublicInvoker.Call(method);
        }

        #endregion

        #region Comparators (Boolean, String, Byte[], Guid)

        #region Boolean

        /// <summary>
        /// Determines whether the two values are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        public static bool IsEqual(Boolean v1, Boolean v2)
        {
            return v1 == v2;
        }

        /// <summary>
        /// Determines whether the two values are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        public static bool IsEqual(Nullable<Boolean> v1, Boolean v2)
        {
            return v1 == v2;
        }

        /// <summary>
        /// Determines whether the two values are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        public static bool IsEqual(Boolean v1, Nullable<Boolean> v2)
        {
            return v1 == v2;
        }

        /// <summary>
        /// Determines whether the two values are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        public static bool IsEqual(Nullable<Boolean> v1, Nullable<Boolean> v2)
        {
            return v1 == v2;
        }

        /// <summary>
        /// Determines whether the first value is greater than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is greater or equal to.</param>
        public static bool IsGreater(Boolean v1, Boolean v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return IsEqual(v1, v2) || IsGreater(v1, v2);
            }

            return v1 && !v2;
        }

        /// <summary>
        /// Determines whether the first value is greater than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is greater or equal to.</param>
        public static bool IsGreater(Nullable<Boolean> v1, Boolean v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return IsEqual(v1, v2) || IsGreater(v1, v2);
            }

            return (v1 != null) && (bool)v1 && !v2;
        }

        /// <summary>
        /// Determines whether the first value is greater than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is greater or equal to.</param>
        public static bool IsGreater(Boolean v1, Nullable<Boolean> v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return IsEqual(v1, v2) || IsGreater(v1, v2);
            }

            return (v2 == null) || v1 && !(bool)v2;
        }

        /// <summary>
        /// Determines whether the first value is greater than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is greater or equal to.</param>
        public static bool IsGreater(Nullable<Boolean> v1, Nullable<Boolean> v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return IsEqual(v1, v2) || IsGreater(v1, v2);
            }

            // not-null > v2
            if (v1 != null)
            {
                return IsGreater((bool)v1, v2);
            }
            // null > v2
            else if (v2 != null)
            {
                return false;
            }
            // null > null
            else return false;
        }

        /// <summary>
        /// Determines whether the first value is smaller than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is less or equal to.</param>
        public static bool IsLess(Boolean v1, Boolean v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return IsEqual(v1, v2) || IsLess(v1, v2);
            }

            return !v1 && v2;
        }

        /// <summary>
        /// Determines whether the first value is smaller than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is less or equal to.</param>
        public static bool IsLess(Nullable<Boolean> v1, Boolean v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return IsEqual(v1, v2) || IsLess(v1, v2);
            }

            return (v1 == null) || !(bool)v1 && v2;
        }

        /// <summary>
        /// Determines whether the first value is smaller than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is less or equal to.</param>
        public static bool IsLess(Boolean v1, Nullable<Boolean> v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return IsEqual(v1, v2) || IsLess(v1, v2);
            }

            return (v2 != null) && !v1 && (bool)v2;
        }

        /// <summary>
        /// Determines whether the first value is smaller than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is less or equal to.</param>
        public static bool IsLess(Nullable<Boolean> v1, Nullable<Boolean> v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return IsEqual(v1, v2) || IsLess(v1, v2);
            }

            // v1 < not-null
            if (v2 != null)
            {
                return IsGreater(v1, (bool)v2);
            }
            // not-null < null
            else if (v1 != null)
            {
                return false;
            }
            // null < null
            else return false;
        }

        #endregion

        #region String

        /// <summary>
        /// Determines whether the two string values are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        public static bool IsEqual(String v1, String v2)
        {
            return String.Compare(v1, v2, StringComparison.OrdinalIgnoreCase) == 0;
        }

        /// <summary>
        /// Determines whether the first string value is greater than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is less or equal to.</param>
        public static bool IsGreater(String v1, String v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return String.Compare(v1, v2, StringComparison.OrdinalIgnoreCase) >= 0;
            }
            else
            {
                return String.Compare(v1, v2, StringComparison.OrdinalIgnoreCase) > 0;
            }
        }

        /// <summary>
        /// Determines whether the first string value is smaller than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is less or equal to.</param>
        public static bool IsLess(String v1, String v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return String.Compare(v1, v2, StringComparison.OrdinalIgnoreCase) <= 0;
            }
            else
            {
                return String.Compare(v1, v2, StringComparison.OrdinalIgnoreCase) < 0;
            }
        }

        #endregion

        #region Guid

        /// <summary>
        /// Determines whether the two Guid values are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        public static bool IsEqual(Guid v1, Guid v2)
        {
            return Comparers.GuidEqual(v1, v2);
        }

        /// <summary>
        /// Determines whether the two Guid values are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        public static bool IsEqual(Nullable<Guid> v1, Guid v2)
        {
            if (v1 == null)
            {
                return false;
            }

            return Comparers.GuidEqual((Guid)v1, v2);
        }

        /// <summary>
        /// Determines whether the two Guid values are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        public static bool IsEqual(Guid v1, Nullable<Guid> v2)
        {
            if (v2 == null)
            {
                return false;
            }

            return Comparers.GuidEqual(v1, (Guid)v2);
        }

        /// <summary>
        /// Determines whether the two Guid values are equal.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        public static bool IsEqual(Nullable<Guid> v1, Nullable<Guid> v2)
        {
            if (v1 == null && v2 == null)
            {
                return true;
            }
            
            if (v1 == null)
            {
                return false;
            }
            
            if (v2 == null)
            {
                return false;
            }
            
            return Comparers.GuidEqual((Guid)v1, (Guid)v2);
        }

        /// <summary>
        /// Determines whether the first Guid value is greater than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is less or equal to.</param>
        public static bool IsGreater(Guid v1, Guid v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return IsEqual(v1, v2) || IsGreater(v1, v2);
            }

            return Comparers.GuidGreaterThan(v1, v2);
        }

        /// <summary>
        /// Determines whether the first Guid value is greater than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is less or equal to.</param>
        public static bool IsGreater(Nullable<Guid> v1, Guid v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return IsEqual(v1, v2) || IsGreater(v1, v2);
            }

            if (v1 == null)
            {
                return false;
            }

            return Comparers.GuidGreaterThan((Guid)v1, v2);
        }

        /// <summary>
        /// Determines whether the first Guid value is greater than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is less or equal to.</param>
        public static bool IsGreater(Guid v1, Nullable<Guid> v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return IsEqual(v1, v2) || IsGreater(v1, v2);
            }

            if (v2 == null)
            {
                return true;
            }

            return Comparers.GuidGreaterThan(v1, (Guid)v2);
        }

        /// <summary>
        /// Determines whether the first Guid value is greater than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is less or equal to.</param>
        public static bool IsGreater(Nullable<Guid> v1, Nullable<Guid> v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return IsEqual(v1, v2) || IsGreater(v1, v2);
            }

            if (v1 == null && v2 == null)
            {
                return false;
            }
            
            if (v1 == null)
            {
                return false;
            }
            
            if (v2 == null)
            {
                return true;
            }
            
            return Comparers.GuidGreaterThan((Guid)v1, (Guid)v2);
        }

        /// <summary>
        /// Determines whether the first Guid value is smaller than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is less or equal to.</param>
        public static bool IsLess(Guid v1, Guid v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return IsEqual(v1, v2) || IsLess(v1, v2);
            }

            return Comparers.GuidLessThan(v1, v2);
        }

        /// <summary>
        /// Determines whether the first Guid value is smaller than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is less or equal to.</param>
        public static bool IsLess(Nullable<Guid> v1, Guid v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return IsEqual(v1, v2) || IsLess(v1, v2);
            }

            if (v1 == null)
            {
                return true;
            }

            return Comparers.GuidLessThan((Guid)v1, v2);
        }

        /// <summary>
        /// Determines whether the first Guid value is smaller than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is less or equal to.</param>
        public static bool IsLess(Guid v1, Nullable<Guid> v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return IsEqual(v1, v2) || IsLess(v1, v2);
            }

            if (v2 == null)
            {
                return false;
            }

            return Comparers.GuidLessThan(v1, (Guid)v2);
        }

        /// <summary>
        /// Determines whether the first Guid value is smaller than the second one.
        /// </summary>
        /// <param name="v1">The first value to compare.</param>
        /// <param name="v2">The second value to compare.</param>
        /// <param name="orEqual">If true, then the operator is less or equal to.</param>
        public static bool IsLess(Nullable<Guid> v1, Nullable<Guid> v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return IsEqual(v1, v2) || IsLess(v1, v2);
            }

            if (v1 == null && v2 == null)
            {
                return false;
            }
            
            if (v1 == null)
            {
                return true;
            }
            
            if (v2 == null)
            {
                return false;
            }
            
            return Comparers.GuidLessThan((Guid)v1, (Guid)v2);
        }

        #endregion

        #region Byte[]

        /// <summary>
        /// Determines whether the two byte arrays are equal.
        /// </summary>
        /// <param name="v1">The first byte array to compare.</param>
        /// <param name="v2">The second byte array to compare.</param>
        public static bool IsEqual(Byte[] v1, Byte[] v2)
        {
            return Comparers.ByteArrayEqual(v1, v2);
        }

        /// <summary>
        /// Determines whether the first byte array is smaller than the second one.
        /// </summary>
        /// <param name="v1">The first byte array to compare.</param>
        /// <param name="v2">The second byte array to compare.</param>
        /// <param name="orEqual">If true, then the operator is less or equal to.</param>
        public static bool IsGreater(Byte[] v1, Byte[] v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return Comparers.ByteArrayComparer(v1, v2) >= 0;
            }

            return Comparers.ByteArrayGreaterThan(v1, v2);
        }

        /// <summary>
        /// Determines whether the first byte array is smaller than the second one.
        /// </summary>
        /// <param name="v1">The first byte array to compare.</param>
        /// <param name="v2">The second byte array to compare.</param>
        /// <param name="orEqual">If true, then the operator is less or equal to.</param>
        public static bool IsLess(Byte[] v1, Byte[] v2, bool orEqual = false)
        {
            if (orEqual)
            {
                return Comparers.ByteArrayComparer(v1, v2) <= 0;
            }

            return Comparers.ByteArrayLessThan(v1, v2);
        }

        #endregion

        #endregion

        #region Check .GoAt method

        /// <summary>
        /// Checks the validity of the .GoAt method.
        /// </summary>
        /// <typeparam name="TRoot">The type of the root node.</typeparam>
        /// <typeparam name="TNode">The type of the node.</typeparam>
        public static void CheckGoAt<TRoot, TNode>() 
            where TRoot : DbRow
            where TNode : DbRow
        {
            if (typeof(TNode) != typeof(TRoot))
            {
                throw new QueryTalkException("Api.CheckGoAt", QueryTalkExceptionType.InvalidGoAt,
                    String.Format("root = {0}{1}   node = {2}", typeof(TRoot), Environment.NewLine, typeof(TNode)), Text.Method.GoAt);
            }
        }

        #endregion

    }
}
