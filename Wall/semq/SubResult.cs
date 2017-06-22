#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    internal class SubResult
    {
        private bool _executed = false;
        internal bool Executed 
        {
            get
            {
                return _executed;
            }
        }

        internal SubResult SetExecuted()
        {
            if (_executed)
            {
                return this;
            }

            _executed = true;

            return this;
        }

        private int _affectedCount = 0;
        internal int AffectedCount 
        { 
            get
            {
                return _affectedCount;
            }
        }

        internal SubResult AddAffectedCount(int affectedCount)
        {
            _affectedCount += affectedCount;
            return this;
        }

        internal SubResult()
        { }

        internal SubResult(bool executed, int affectedCount)
        {
            _executed = executed;
            _affectedCount = affectedCount;
        }

    }
}
