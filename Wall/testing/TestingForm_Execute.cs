#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using QueryTalk;

namespace QueryTalk.Wall
{
    internal partial class TestingForm : Form
    {
        private Connectable Build()
        {
            // use original executable object
            var executable = _connectable.Executable;

            // set testing params
            SetTestingParameters(executable);

            // create new connectable object (using the original executable)
            var connectable = new Connectable(
                _connectable.Client, executable, _connectable.ConnectionStringAsync,
                _sql.Text);

            connectable.SetTimeout(_connectable.CommandTimeout);

            // wrap final SQL by BEGIN TRANSACTION/ROLLBACK block
            connectable.Sql = Wall.Text.GenerateSql(1000)

                .Append(Wall.Text.Free.BeginTestTransaction)
                .NewLine()
                .NewLine(connectable.Sql)
                .NewLine(Wall.Text.Free.EndQueryTalkCode)
                .NewLine(Wall.Text.Free.EndTestTransaction)
                .ToString();

            return connectable;
        }

        private void Run()
        {
            try
            {
                if (!ValidateTransactions())
                {
                    MessageBox.Show("It is not allowed to modify the transaction code." +
                        "\r\n\r\nPlease avoid modifying or adding BEGIN TRANSACTION, SAVE TRANSACTION, COMMIT TRANSACTION, ROLLBACK TRANSACTION instructions.", Wall.Text.Free.Warning,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }

                var runningParam = _params.Values.Where(p => p.Processing == Processing.Running).FirstOrDefault();
                if (runningParam != null)
                {
                    MessageBox.Show(String.Format("The table-valued parameter {0} is still loading. Please wait until the load completes or cancel the operation.",
                        runningParam.ParamName), Wall.Text.Free.Warning,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    return;
                }

                if (CancelProcessing())
                {
                    return;
                }

                _processing = Processing.Running;
                _elapsed.Visible = false;
                AdjustAppearance();

                var connectable = Build();      

                _watch.Restart();

                var onCompleted = new Action<Result>(result =>
                {
                    _watch.Stop();
                    ShowElapsed();
                    try
                    {
                        if (result.AsyncStatus.IsCompleted())
                        {
                            ShowResult(result, GetOutputValues(connectable, result));
                        }
                        else
                        {
                            ShowException(result.Exception);
                            _async.SetResultException(null);
                        }
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        ShowException(ex);
                    }
                });

                _async = connectable.GoAsync(onCompleted);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                _processing = Processing.Idle;
                _watch.Stop();
                ShowException(ex);
            }
        }

    }
}
