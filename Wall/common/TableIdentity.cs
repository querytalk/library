#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    internal class TableIdentity
    {
        private long _seed = 1;
        internal long Seed 
        { 
            get
            {
                return _seed;
            }
        }

        private long _increment = 1;
        internal long Increment 
        { 
            get
            {
                return _increment;
            }
        }

        internal TableIdentity()
        { }

        internal TableIdentity(long seed, long increment)
        {
            _seed = seed;
            _increment = increment;
        }

    }
}
