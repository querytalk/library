#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Collections.Generic;

namespace QueryTalk.Wall
{
    // class that contains all clauses of a query
    internal class QueryPart
    {
        internal List<FromChainer> Ctes { get; set; }

        internal List<TableChainer> Tables { get; set; }

        internal SelectChainer Select { get; set; }

        internal CollectChainer Collect { get; set; }

        internal List<Chainer> Wheres { get; set; }

        internal Chainer GroupBy { get; set; }

        internal Chainer Having { get; set; }

        internal Chainer OrderBy { get; set; }

        internal Chainer IntoTempTable { get; set; }

        internal Chainer Delete { get; set; }

        internal Chainer Insert { get; set; }

        internal Chainer Update { get; set; }

        internal Chainer Union { get; set; }

        internal Joiner Joiner { get; set; }

        internal QueryPart()
        {
            Ctes = new List<FromChainer>();
            Tables = new List<TableChainer>();
            Wheres = new List<Chainer>();
        }
    }
}
