#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Reflection;
using QueryTalk.Wall;

namespace QueryTalk
{
    /// <summary>
    /// Encapsulates the public static members of the QueryTalk's designer.
    /// </summary>
    public abstract partial class Designer
    {
        /// <summary>
        /// Begins the transactional SQL batch.
        /// </summary>
        public static NameChainer As()
        {
            return Call<NameChainer>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var root = new d();
                root.Name = Wall.Text.NotAvailable;
                return new NameChainer(root);
            });
        }

        /// <summary>
        /// Begins the transactional SQL batch.
        /// </summary>
        /// <param name="name">The name of the batch.</param>
        public static NameChainer As(string name)
        {
            return Call<NameChainer>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var root = new d();
                root.Name = name;
                return new NameChainer(root);
            });
        }

        /// <summary>
        /// Begins the transactional SQL batch.
        /// </summary>
        /// <param name="name">The name of the batch.</param>
        /// <param name="embeddedTransactionIsolationLevel">Defines the transaction isolation level of the batch.</param>
        public static NameChainer As(string name,
            QueryTalk.Designer.IsolationLevel embeddedTransactionIsolationLevel = IsolationLevel.Default)
        {
            return Call<NameChainer>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (name == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "name", ".As");
                }

                var root = new d();
                root.Name = name;
                return new NameChainer(root, embeddedTransactionIsolationLevel);
            });
        }

        /// <summary>
        /// Begins the non-transactional SQL batch.
        /// </summary>
        public static NameChainer AsNon()
        {
            return Call<NameChainer>(Assembly.GetCallingAssembly(), (ca) =>
            {
                var root = new d();
                root.Name = Wall.Text.NotAvailable;
                root.IsEmbeddedTransaction = false;
                root.IsEmbeddedTryCatch = true;
                return new NameChainer(root);
            });
        }

        /// <summary>
        /// Begins the non-transactional SQL batch.
        /// </summary>
        /// <param name="name">The name of the batch.</param>
        /// <param name="hasTryCatch">A flag to indicate whether the try-catch construct is included.</param>
        public static NameChainer AsNon(string name, bool hasTryCatch = true)
        {
            return Call<NameChainer>(Assembly.GetCallingAssembly(), (ca) =>
            {
                if (name == null)
                {
                    _Throw(QueryTalkExceptionType.ArgumentNull, "name", ".AsNon");
                }

                var root = new d();
                root.Name = name;
                root.IsEmbeddedTransaction = false;
                root.IsEmbeddedTryCatch = hasTryCatch;
                return new NameChainer(root);
            });
        }


    }
}