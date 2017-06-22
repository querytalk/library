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
        /// Opens the testing environment window. When closed the execution proceeds. The testing does not affect the data state of the execution.
        /// </summary>
        /// <typeparam name="T">The type of a subject node.</typeparam>
        /// <param name="node">A semantic sentence.</param>
        /// <param name="option">An option that specifies whether the testing environment window should open, should open and execute, or should be skipped.</param>
        public static DbTable<T> Test<T>(this DbTable<T> node, TestingOption option = TestingOption.OpenAndExecute)
            where T : DbRow
        {
            node.CheckNullAndThrow(Text.Free.Sentence, Text.Method.Test);
            return _Test(Assembly.GetCallingAssembly(), node, null, option);
        }

        /// <summary>
        /// Opens the testing environment window. When closed the execution proceeds. The testing does not affect the data state of the execution.
        /// </summary>
        /// <typeparam name="T">The type of a subject node.</typeparam>
        /// <param name="node">A semantic sentence.</param>
        /// <param name="title">A title that will appear in the window.</param>
        /// <param name="option">An option that specifies whether the testing environment window should open, should open and execute, or should be skipped.</param>
        public static DbTable<T> Test<T>(this DbTable<T> node, string title, TestingOption option = TestingOption.OpenAndExecute)
            where T : DbRow
        {
            node.CheckNullAndThrow(Text.Free.Sentence, Text.Method.Test);
            return _Test(Assembly.GetCallingAssembly(), node, title, option);
        }

        private static DbTable<T> _Test<T>(Assembly ca, DbTable<T> node, string title, TestingOption option)
            where T : DbRow
        {
            if (Admin.IsTestingEnvironmentOff(ca) || option == TestingOption.Skip)
            {
                return node;
            }

            // handle reusability - !
            var node2 = node.RootWithBrokenChain ?? node.Root;

            Chainer query;

            if (node2.IsPureSubject())
            {
                query = new SelectChainer((ISemantic)node2, new Column[] { }, false).Top(Admin.TestSelectTopRows);
            }
            else
            {
                query = new SelectChainer((ISemantic)node2, new Column[] { }, false);
            }

            var connectable = Reader.GetConnectable(ca, query);
            using (var form = new TestingForm(connectable, option, title))
            {
                form.ShowDialog();
            }

            node2.Recover();

            return node;
        }

    }
}
