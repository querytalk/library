#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {

        #region Common

        internal static void BuildNode(this Chainer node, QueryPart queryPart,
            BuildContext buildContext, BuildArgs buildArgs, StringBuilder sql)
        {
            if (node == null)
            {
                return;
            }

            var root = node.GetRoot();

            // snippet check
            if (root.CompilableType == Compilable.ObjectType.Snippet && node is ISnippetDisallowed)
            {
                node.Throw(QueryTalkExceptionType.InvalidSnippet, null, node.Method);
            }

            string append = null;

            if (node.Build != null && !node.SkipBuild)
            {
                // important: assign current object
                buildContext.Current = node;

                // important: store query part into the current object
                buildContext.Current.QueryPart = queryPart;

                // build
                append = node.Build(buildContext, buildArgs);

                TryThrow(node.Exception);
                TryThrow(buildContext.Exception);
            }

            if (append != null)
            {
                if (append == Text.Terminator)
                {
                    sql.TrimEnd();
                }

                sql.Append(append);

                // assure that there is always a spacing between the sql parts/clauses
                sql.S();
            }

            // check view
            if (root.CompilableType == Compilable.ObjectType.View 
                && (root.Statements.Count > 1           // view can have only a single statement
                    || !(node is IViewAllowed)))        // only clauses related to select statement can be contained in the view
            {
                root.CheckNodeReuseAndThrow();       
                node.Throw(QueryTalkExceptionType.InvalidView, null, node.Method);
            }
        }

        // returns Root of the chain object in null safe manner
        internal static Designer TryGetRoot(this Chainer cobj)
        {
            if (cobj != null)
            {
                if (cobj.Root is Designer)
                {
                    return cobj.GetRoot();
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        // conditionally assigns a "from" exception to the build context
        internal static bool TryTakeException(this BuildContext buildContext, QueryTalkException exception, string method = null)
        {
            // exception
            if (buildContext != null             
                && exception != null)          
            {
                buildContext.Exception = exception;
                buildContext.Exception.Method = method ?? buildContext.Exception.Method;
                return true;
            }
            return false;
        }

        // conditionally assigns an argument to the build context
        internal static void TryTake(this BuildContext buildContext, Argument argument)
        {
            if (buildContext != null               
                && argument != null)               
            {
                // no action
            }
        }

        // conditionally throws an exception 
        internal static void TryThrow(this QueryTalkException exception, string method = null)
        {
            if (exception != null)
            {
                exception.Method = method ?? exception.Method;
                throw exception;
            }
        }

        // try replace first row of ResultSet<T> table with the given row (of type T)
        internal static ResultSet<T> TryCollectRow<T>(this DbRow row)
            where T : DbRow
        {
            if (row == null)
            {
                return null;
            }

            return new ResultSet<T>(row);
        }

        // return value as string
        internal static string ToReport(this object value)
        {
            if (value == null)
            {
                return Text.ClrNull;
            }
            else
            {
                if (value.GetType() == typeof(Value))
                {
                    return ((Value)value).ToString();
                }
                else
                {
                    var toString = value.ToString();
                    if (String.IsNullOrEmpty(toString) || toString == value.GetType().ToString())
                    {
                        return Text.NotAvailable;
                    }
                    else
                    {
                        if (toString.Length > 200)
                        {
                            return String.Format("{0}...", toString.Substring(0, 200));
                        }
                        else
                        {
                            return toString;
                        }
                    }
                }
            }
        }

        // gets an attribute of an enum field 
        internal static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            Type type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }

        // return passed type or underlying value type of nullable type
        internal static Type GetValueType(this Type valueOrNullableType)
        {
            if (valueOrNullableType.IsNullable())
            {
                return Nullable.GetUnderlyingType(valueOrNullableType);
            }

            return valueOrNullableType;
        }

        // return all public properties with defined public get/set method
        internal static PropertyInfo[] GetWritableProperties(this Type type)
        {
            if (type == null)
            {
                return null;
            }

            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(prop => prop.GetGetMethod() != null && prop.GetSetMethod() != null)
                .OrderBy(prop => prop.Name)  // use DbRow sorting rule
                .ToArray();
        }

        // return all public properties with defined public get method (sorted by its position in the type)
        internal static PropertyInfo[] GetReadableProperties(this Type type)
        {
            if (type == null)
            {
                return null;
            }

            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(prop => prop.GetGetMethod() != null)
                .ToArray();
        }

        // return all public properties with defined public get method (sorted by name)
        internal static PropertyInfo[] GetSortedReadableProperties(this Type type)
        {
            if (type == null)
            {
                return null;
            }

            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(prop => prop.GetGetMethod() != null)
                .OrderBy(prop => prop.Name)  // to be DbRow sorting compliant
                .ToArray();
        }

        // for DbRow type only
        //   note: 
        //     Sorting is crutial here in order to achieve independence from the reflection ordering.
        internal static PropertyInfo[] GetSortedWritableProperties(this Type type, bool withRowID = false)
        {
            if (type == null)
            {
                return null;
            }

            var clrTypes = Mapping.ClrMapping.Select(a => a.Key).ToArray();

            var properties = new List<PropertyInfo>();
            properties.AddRange(type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(prop => prop.GetGetMethod() != null && prop.GetSetMethod() != null
                    && clrTypes.Contains(prop.PropertyType.GetValueType()))    
                .OrderBy(prop => prop.Name));    // important!

            if (withRowID)
            {
                properties.Add((typeof(IRow).GetProperty(Text.Reserved.QtRowID)));
            }
            
            return properties.ToArray();
        }

        // extension method that ensures a thread safe form control handling
        internal static void ThreadSafeInvoke(this System.Windows.Forms.Control ctr, Action action)
        {
            if (ctr.InvokeRequired)
            {
                ctr.Invoke(action);
            }
            else
            {
                action();
            }
        }

        // returns true if collection contains at least one item
        internal static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null
                || collection.Count() == 0;
        }

        // returns true if a given compilable type is a view or snippet
        internal static bool IsViewOrSnippet(this Compilable.ObjectType? compilableType)
        {
            return compilableType == Compilable.ObjectType.View
                || compilableType == Compilable.ObjectType.Snippet;
        }

        // returns true if a given compilable type is a view or mapper
        internal static bool IsViewOrMapper(this Compilable.ObjectType? compilableType)
        {
            return compilableType == Compilable.ObjectType.View
                || compilableType == Compilable.ObjectType.Mapper;
        }

        // returns true if a given compilable type is a procedure or none
        internal static bool IsProc(this Compilable.ObjectType? compilableType)
        {
            return compilableType == null
                || compilableType == Compilable.ObjectType.Procedure;
        }

        // returns true if a given compilable type is a procedure, snippet, or none
        internal static bool IsProcOrSnippet(this Compilable.ObjectType? compilableType)
        {
            return compilableType == null
                || compilableType == Compilable.ObjectType.Procedure
                || compilableType == Compilable.ObjectType.Snippet;
        }

        // returns true if a given compilable type is a procedure, snippet or mapper
        internal static bool IsProcOrSnipOrMapper(this Compilable.ObjectType? compilableType)
        {
            return compilableType == Compilable.ObjectType.Procedure
                || compilableType == Compilable.ObjectType.Snippet
                || compilableType == Compilable.ObjectType.Mapper;
        }

        #endregion

        #region Variable

        // is variable a db type (null safe)
        internal static bool IsDbType(this Variable variable)
        {
            if (variable == null)
            {
                return false;
            }

            return variable.DT.IsDataType();
        }

        // is variable an inliner (null safe)
        internal static bool IsInliner(this Variable variable)
        {
            if (variable == null)
            {
                return false;
            }

            return variable.DT.IsInliner();
        }

        // is variable a concatenator (null safe)
        internal static bool IsConcatenator(this Variable variable)
        {
            if (variable == null)
            {
                return false;
            }

            return variable.DT.IsConcatenator();
        }

        #endregion

        #region IsolationLevel

        // converts QueryTalk.Designer.IsolationLevel into System.Transactions.IsolationLevel
        internal static System.Transactions.IsolationLevel ToSystemEnum(this QueryTalk.Designer.IsolationLevel isolationLevel)
        {
            switch (isolationLevel)
            {
                case Designer.IsolationLevel.ReadCommitted: return System.Transactions.IsolationLevel.ReadCommitted;
                case Designer.IsolationLevel.ReadUncommitted: return System.Transactions.IsolationLevel.ReadUncommitted;
                case Designer.IsolationLevel.RepeatableRead: return System.Transactions.IsolationLevel.RepeatableRead;
                case Designer.IsolationLevel.Serializable: return System.Transactions.IsolationLevel.Serializable;
                case Designer.IsolationLevel.Snapshot: return System.Transactions.IsolationLevel.Snapshot;
                default: return System.Transactions.IsolationLevel.Unspecified;  // will not happen
            }
        }

        // converts QueryTalk.Designer.IsolationLevel into SQL code presentation
        internal static string ToSql(this QueryTalk.Designer.IsolationLevel isolationLevel)
        {
            switch (isolationLevel)
            {
                case Designer.IsolationLevel.ReadCommitted: return Text.ReadCommitted; 
                case Designer.IsolationLevel.ReadUncommitted: return Text.ReadUncommitted; 
                case Designer.IsolationLevel.RepeatableRead: return Text.RepeatableRead; 
                case Designer.IsolationLevel.Snapshot: return Text.Snapshot; 
                case Designer.IsolationLevel.Serializable: return Text.Serializable; 
                default: return Text.ReadCommitted; 
            }
        }

        #endregion

        #region SEMQ

        // builds predicate group
        internal static string Build(this PredicateGroup predicateGroup, PredicateGroupType callerGroupType, string callerKeyword)
        {
            if (predicateGroup == null)
            {
                return callerKeyword;
            }

            var sql = Text.GenerateSql(30);

            // .End()
            if (callerGroupType == PredicateGroupType.End 
                && (predicateGroup.PredicateGroupType == PredicateGroupType.End
                    || predicateGroup.PredicateGroupType == PredicateGroupType.BeginEnd))
            {
                sql.NewLine(Text.RightBracket);
            }
            // .Begin()
            else if (callerGroupType == PredicateGroupType.Begin
                && (predicateGroup.PredicateGroupType == PredicateGroupType.Begin
                    || predicateGroup.PredicateGroupType == PredicateGroupType.BeginEnd))
            {
                switch (predicateGroup.LogicalOperator)
                {
                    case Wall.LogicalOperator.None:
                        sql.Append(Text.Where);
                        break;

                    case Wall.LogicalOperator.And:
                        sql.Append(Text.And);
                        break;

                    case Wall.LogicalOperator.Or:
                        sql.Append(Text.Or);
                        break;
                }

                if (!predicateGroup.Sign)
                {
                    sql.S().Append(Text.Not);
                }

                sql.S().Append(Text.LeftBracket);
            }
            // predicate group is BeginEnd (single predicate inside the group) => return caller's default keyword (no brackets)
            else
            {
                sql.Append(callerKeyword);
            }

            return sql.ToString();
        }

        #endregion

    }
}
