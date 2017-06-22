#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Reflection;
using QueryTalk.Wall;

namespace QueryTalk
{
    public static partial class Extensions
    {
        /// <summary>
        /// Returns the number of the rows returned by a semantic query.
        /// </summary>
        /// <typeparam name="T">The type of the subject node.</typeparam>
        /// <param name="prev">Is a subject node.</param>
        public static int CountGo<T>(this DbTable<T> prev)
            where T : DbRow
        {
            prev.CheckNullAndThrow(Text.Free.Sentence, Text.Method.SelectCount);
            var ca = Assembly.GetCallingAssembly();
            var select = new SelectChainer((ISemantic)prev, new Column[] { Designer.Count().As(Text.Count) }, false);
            var connectable = Reader.GetConnectable(ca, select);
            return Reader.LoadTable<Row<int>>(connectable, null).ToValue<int>();
        }
    }
}
