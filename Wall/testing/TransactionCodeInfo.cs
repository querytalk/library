#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Text.RegularExpressions;

namespace QueryTalk.Wall
{
    internal class TransactionCodeInfo
    {
        internal int BeginCount { get; set; }
        internal int SaveCount { get; set; }
        internal int CommitCount { get; set; }
        internal int RollbackCount { get; set; }

        internal TransactionCodeInfo(string code)
        {
            if (String.IsNullOrWhiteSpace(code))
            {
                return;
            }

            // remove all block comments
            code = Regex.Replace(code, @"(\/\*)([\s\S]*)(\*\/)", "");

            // remove all inline comments
            code = Regex.Replace(code, "--.*", "");

            var mmBegin = Regex.Matches(code, @"\bBEGIN\s+TRAN(SACTION)?\b", RegexOptions.IgnoreCase);
            var mmSave = Regex.Matches(code, @"\bSAVE\s+TRAN(SACTION)?\b", RegexOptions.IgnoreCase);
            var mmCommit = Regex.Matches(code, @"\bCOMMIT(\s+TRAN(SACTION)?)?\b", RegexOptions.IgnoreCase);
            var mmRollback = Regex.Matches(code, @"\bROLLBACK(\s+TRAN(SACTION)?)?\b", RegexOptions.IgnoreCase);

            BeginCount = mmBegin.Count;
            SaveCount = mmSave.Count;
            CommitCount = mmCommit.Count;
            RollbackCount = mmRollback.Count;
        }

        internal bool HasEqualCount(TransactionCodeInfo code)
        {
            return
                code != null &&
                BeginCount == code.BeginCount &&
                SaveCount == code.SaveCount &&
                CommitCount == code.CommitCount &&
                RollbackCount == code.RollbackCount;
        }
    }
}
