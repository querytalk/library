#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Text;

namespace QueryTalk.Wall
{
    // note:
    //   Build is done by IOutput (IWritable) object.
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>

    public sealed class OutputChainer : EndChainer, IQuery
    {
        internal override string Method 
        { 
            get 
            {
                return Text.Method.Output;
            } 
        }

        internal OutputChainer(Chainer prev, Column[] columns, Nullable<OutputSource> outputTable)
            : base(prev)
        {
            // check (null, null) => no output clause
            if (columns == null && outputTable == null)
            {
                return;
            }

            // check columns
            if (!CheckNullOrEmpty(Argc(() => columns, columns)))
            {
                if (outputTable != null)
                {
                    columns = new Column[] { String.Format("{0}.*", outputTable) };
                }
                else
                {
                    // set default columns (either Inserted.* or Deleted.*)
                    if (prev is DeleteChainer)
                    {
                        columns = new Column[] { Text.Free.Deleted };
                    }
                    // prev is InsertChainer/UpdateChainer
                    else
                    {
                        columns = new Column[] { Text.Free.Inserted };
                    }
                }
            }

            ((IOutput)prev).OutputColumns = columns;
        }

        internal static void TryAppendOutput(IOutput target, StringBuilder sql, BuildContext buildContext, BuildArgs buildArgs)
        {
            if (target.OutputColumns == null)
            {
                return;
            }

            // important: 
            //   set buildContext.IsCurrentStringAsValue as false => strings treated as identifiers
            buildContext.IsCurrentStringAsValue = false;
            sql.NewLine(Text.Output).S()
                .Append(Column.Concatenate(target.OutputColumns, buildContext, buildArgs, null, Text.CommaWithSpace)).S();
            buildContext.ResetCurrentStringAsValueFlag();

            if (target.OutputTarget != null)
            {
                sql.Append(Text.Into).S()
                    .Append(target.OutputTarget.Build(buildContext, buildArgs)).S();
            }
        }

    }
}
