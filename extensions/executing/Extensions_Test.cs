#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Reflection;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates all public extension methods of the QueryTalk library.
    /// </summary>
    public static partial class Extensions
    {

        /// <summary>
        /// Opens the testing environment window. When closed the execution proceeds. The testing does not affect the data state of the execution.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="option">An option that specifies whether the testing environment window should open, should open and execute, or should be skipped.</param>
        public static Connectable Test(this IGo prev, TestingOption option = TestingOption.OpenAndExecute)
        {
            return PublicInvoker.Call<Connectable>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                if (!Admin.IsTestingEnvironmentOff(ca) && option != TestingOption.Skip)
                {
                    using (var form = new TestingForm(connectable, option, null))
                    {
                        form.ShowDialog();
                    }
                }

                return connectable;
            });
        }

        /// <summary>
        /// Opens the testing environment window. When closed the execution proceeds. The testing does not affect the data state of the execution.
        /// </summary>
        /// <param name="prev">A predecessor object.</param>
        /// <param name="title">A title that will appear in the window.</param>
        /// <param name="option">An option that specifies whether the testing environment window should open, should open and execute, or should be skipped.</param>
        public static Connectable Test(this IGo prev, string title, TestingOption option = TestingOption.OpenAndExecute)
        {
            return PublicInvoker.Call<Connectable>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)prev);
                if (!Admin.IsTestingEnvironmentOff(ca) && option != TestingOption.Skip)
                {
                    using (var form = new TestingForm(connectable, option, title))
                    {
                        form.ShowDialog();
                    }
                }

                return connectable;
            });
        }

        /// <summary>
        /// Opens the testing environment window. When closed the execution proceeds. The testing does not affect the data state of the execution.
        /// </summary>
        /// <param name="view">A view object.</param>
        /// <param name="option">An option that specifies whether the testing environment window should open, should open and execute, or should be skipped.</param>
        public static View Test(this View view, TestingOption option = TestingOption.OpenAndExecute)
        {
            return PublicInvoker.Call<View>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)view);
                if (!Admin.IsTestingEnvironmentOff(ca) && option != TestingOption.Skip)
                {
                    using (var form = new TestingForm(connectable, option, null))
                    {
                        form.ShowDialog();
                    }
                }

                return view;
            });
        }

        /// <summary>
        /// Opens the testing environment window. When closed the execution proceeds. The testing does not affect the data state of the execution.
        /// </summary>
        /// <param name="view">A view object.</param>
        /// <param name="title">A title that will appear in the window.</param>
        /// <param name="option">An option that specifies whether the testing environment window should open, should open and execute, or should be skipped.</param>
        public static View Test(this View view, string title, TestingOption option = TestingOption.OpenAndExecute)
        {
            return PublicInvoker.Call<View>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var connectable = Reader.GetConnectable(ca, (Chainer)view);
                if (!Admin.IsTestingEnvironmentOff(ca) && option != TestingOption.Skip)
                {
                    using (var form = new TestingForm(connectable, option, title))
                    {
                        form.ShowDialog();
                    }
                }

                return view;
            });
        }
    }
}
