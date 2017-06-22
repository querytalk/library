#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;

namespace QueryTalk.Wall
{
    internal class OrderedLink
    {
        // first table in the link
        internal DB3 TableA { get; private set; }

        // second table in the link
        internal DB3 TableB { get; private set; }

        internal Link Link { get; private set; }

        internal OrderedLink(DB3 tableA, DB3 tableB, Link link)
        {
            TableA = tableA;
            TableB = tableB;
            Link = link;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is OrderedLink))
            {
                return false;
            }

            var o = (OrderedLink)obj;
            return Equals(o.TableA, o.TableB);
        }

        internal bool Equals(DB3 tableA, DB3 tableB)
        {
            return TableA.Equals(tableA) && TableB.Equals(tableB);
        }

        public override int GetHashCode()
        {
            var h1 = TableA.GetHashCode();
            var h2 = TableB.GetHashCode();

            unchecked
            {
                return (527 + h1) * 31 + h2;
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

    // IEqualityComparer implementation class of the OrderedLink class (ordered version). 
    internal class OrderedLinkEqualityComparer : IEqualityComparer<OrderedLink>
    {
        /// <summary>
        /// Implementation of Equals method.
        /// </summary>
        public bool Equals(OrderedLink o1, OrderedLink o2)
        {
            if (o1 == null || o2 == null)
            {
                return false;
            }

            return o1.Equals(o2);
        }

        /// <summary>
        /// Implementation of GetHashCode method.
        /// </summary>
        public int GetHashCode(OrderedLink o)
        {
            if (o == null)
            {
                return 0;
            }

            return o.GetHashCode();
        }
    }

}
