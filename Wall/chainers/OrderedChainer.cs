#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class OrderedChainer : Chainer, INonPredecessor, IQuery, 
        IScalar
    {
        internal override string Method
        {
            get
            {
                return Text.Method.AsAscDesc;
            }
        }

        internal OrderedChainer(Chainer prev, SortOrder sortOrder) 
            : base(prev)
        {
            Build = (buildContext, buildArgs) =>
                {
                    return String.Format("{0} {1}",
                        prev.Build(buildContext, buildArgs),
                        sortOrder.ToUpperCase());
                };
        }

        internal OrderedChainer(DbColumn prev, SortOrder sortOrder)
            : base(null)
        {
            Build = (buildContext, buildArgs) =>
            {
                return String.Format("{0} {1}",
                    prev.Build(buildContext, buildArgs),
                    sortOrder.ToUpperCase());
            };
        }

        internal OrderedChainer(System.String identifier, SortOrder sortOrder)
            : base(null)
        {
            CheckNull(Arg(() => identifier, identifier));

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(100)
                    .Append(Filter.DelimitMultiPartOrParam(identifier, IdentifierType.ColumnOrParam, out chainException)).S()
                    .Append(sortOrder.ToUpperCase())
                    .ToString();
                 buildContext.TryTakeException(chainException);
                return sql;
            };  
        }

    }
}
