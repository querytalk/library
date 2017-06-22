#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryTalk.Wall
{
    /// <summary>
    /// <para>Represents the abstract link between the two tables irrespective of the relation type.</para>
    /// <para>Two tables have a link (or are linked together) if at least one table establish the relationship with the other table.</para>
    /// </summary>
    public class Link
    {
        // first table in the link
        internal DB3 TableA { get; private set; }

        // second table in the link
        internal DB3 TableB { get; private set; }

        // intermediate table between the two tables
        internal DB3 Intermediate { get; private set; }

        internal bool HasIntermediate
        {
            get
            {
                return !Intermediate.IsDefault;
            }
        }

        // collection of all relations between the two tables
        private HashSet<Relation> _relations;
        internal HashSet<Relation> Relations
        {
            get
            {
                return _relations;
            }
        }

        internal bool HasManyRelations
        {
            get
            {
                return _relations.Count > 1;
            }
        }

        // returns true if the given table is on reference side of the relationship
        internal bool IsRefTable(DB3 table)
        {
            return _relations.Where(a => a.IsRefTable(table)).Any();
        }

        // returns true if given table is on foreign key side of the relationship
        internal bool IsFKTable(DB3 table)
        {
            return _relations.Where(a => a.IsFKTable(table)).Any();
        }

        /// <summary>
        /// Adds a relation to the link object.
        /// </summary>
        /// <param name="relation">A specified relation to be added.</param>
        public void AddRelation(Relation relation)
        {
            _relations.Add(relation);
        }

        internal DB3 DefaultForeignKey
        {
            get
            {
                if (_relations.Count == 0 || _relations.Count > 1)
                {
                    return DB3.Default;
                }
                else
                {
                    return _relations.First().ID;
                }
            }
        }

        internal Relation TryGetRelation(DB3 foreignKey, string method = null)
        {
            if (HasIntermediate)
            {
                throw new QueryTalkException("Link.TryGetRelation", QueryTalkExceptionType.IntermediateTableDisallowed,
                    String.Format("linked tables = {0}:{1}{2}   intermediate table = {3}",
                        DbMapping.GetNodeMap(TableA).Name.Sql,
                        DbMapping.GetNodeMap(TableB).Name.Sql, 
                        Environment.NewLine, DbMapping.GetNodeMap(Intermediate).Name.Sql),
                        method);
            }

            Relation relation = null;

            if (foreignKey.Equals(DB3.Default))
            {
                // single relation
                if (_relations.Count == 1)
                {
                    return _relations.First();
                }
                // many relations - missing FK
                else
                {
                    throw new QueryTalkException("Link.TryGetRelation", QueryTalkExceptionType.MissingForeignKey,
                        String.Format("linked tables = {0}:{1}",
                            DbMapping.GetNodeMap(TableA).Name.Sql,
                            DbMapping.GetNodeMap(TableB).Name.Sql), method);
                }
            }
            // foreign key is defined:
            else
            {
                relation = _relations.Where(a => a.FKColumns.Contains(foreignKey))     
                    .FirstOrDefault();

                if (relation == null)
                {
                    DbMapping.ThrowForeignKeyNotFoundException(foreignKey, TableA);
                }
            }

            return relation;
        }

        #region Constructors

        /// <summary>
        /// Initializes the link object.
        /// </summary>
        public Link(DB3 tableA, DB3 tableB)
        {
            TableA = tableA;
            TableB = tableB;
            _relations = new HashSet<Relation>(new MapEqualityComparer());
        }

        /// <summary>
        /// Initializes the link object with an intermediate table.
        /// </summary>
        public Link(DB3 tableA, DB3 tableB, DB3 intermediate)
            : this(tableA, tableB)
        {
            Intermediate = intermediate;
        }

        #endregion

        /// <summary>
        /// Determines whether the specified object is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Link))
            {
                return false;
            }

            var o = (Link)obj;
            return Equals(o.TableA, o.TableB);
        }

        /// <summary>
        /// Determines whether two specified values are equal.
        /// </summary>
        /// <param name="tableA">The first value to compare.</param>
        /// <param name="tableB">The second value to compare.</param>
        internal bool Equals(DB3 tableA, DB3 tableB)
        {
            return
                (TableA.Equals(tableA) && TableB.Equals(tableB))
                    ||
                (TableA.Equals(tableB) && TableB.Equals(tableA));
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            var h1 = TableA.GetHashCode();
            var h2 = TableB.GetHashCode();

            // sort
            if (h1 < h2)
            {
                unchecked
                {
                    return (527 + h1) * 31 + h2;
                }
            }
            else
            {
                unchecked
                {
                    return (527 + h2) * 31 + h1;
                }
            }
        }

        /// <summary>
        /// Returns the string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return String.Format("{0}:{1}", TableA.ToString(), TableB.ToString());
        }
    }

    /// <summary>
    /// IEqualityComparer implementation class of the Link class. 
    /// </summary>
    public class LinkEqualityComparer : IEqualityComparer<Link>
    {
        /// <summary>
        /// Determines whether two specified values are equal.
        /// </summary>
        /// <param name="x">The first value to compare.</param>
        /// <param name="y">The second value to compare.</param>
        public bool Equals(Link x, Link y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Equals(y);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        public int GetHashCode(Link obj)
        {
            if (obj == null)
            {
                return 0;
            }

            return obj.GetHashCode();
        }
    }


}
