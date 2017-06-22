#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>
    public sealed class RowNumberRankingChainer : RankingChainer, IQuery,
        IRankingPartitionBy,
        IRankingOrderBy
    {
        internal RowNumberRankingChainer() 
        {
            Build = (buildContext, buildArgs) =>
                {
                    return Wall.Text.RowNumberOver;
                };
        }
    }
}
