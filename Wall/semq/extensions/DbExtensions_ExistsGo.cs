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
        /// Returns true if a semantic query contains any rows.
        /// </summary>
        /// <typeparam name="T">The type of the subject node.</typeparam>
        /// <param name="prev">A semantic query.</param>
        /// <returns></returns>
        public static bool ExistsGo<T>(this DbTable<T> prev)
            where T : DbRow
        {
            prev.CheckNullAndThrow(Text.Free.Sentence, Text.Method.ExistsGo);
            var ca = Assembly.GetCallingAssembly();
            var select = new SelectChainer((ISemantic)prev, new Column[] { Designer.Null }, false);
            var connectable = Reader.GetConnectable(ca, select.TopOne());
            var result = Reader.LoadTable<dynamic>(connectable, null);
            return result.RowCount > 0;
        }
    }
}
