#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System.Drawing;
using System.Windows.Forms;

namespace QueryTalk.Wall
{
    internal partial class TestingForm : Form
    {
        private void AddEventHandlers()
        {
            _run.Click += (o, e) =>
            {
                Run();
            };

            _run.MouseEnter += (o, e) =>
            {
                if (_run.Text == "Go")
                {
                    _run.BackColor = Color.FromArgb(61, 135, 211);
                    _run.ForeColor = Color.White;
                }
                else
                {
                    _run.BackColor = Color.Orange;
                    _run.ForeColor = Color.White;
                }
            };

            _run.MouseLeave += (o, e) =>
            {
                _run.BackColor = Color.LightGray;
                _run.ForeColor = Color.Black;
            };

            _interrupt.Click += (o, e) =>
            {
                CancelProcessing();
                Close();
                Reader.ThrowOperationCancelledByUserException<dynamic>(_connectable, 0);
            };

            _off.Click += (o, e) =>
            {
                CancelProcessing();
                Admin.ShutTestingEnvironmentDown(_connectable.Client, false);
                Close();
            };

            _parameters.DataError += OnDataError;
            _parameters.RowPrePaint += (o, e) =>
            {
                _parameters.AutoResizeRow(e.RowIndex);
            };
            _parameters.CellMouseClick += (o, e) =>
            {
                try
                {
                    if (e.RowIndex >= 0)
                    {
                        var paramName = _parameters.Rows[e.RowIndex].Cells[0].Value.ToString();
                        if (_params[paramName].IsTable && !_params[paramName].IsBulk)
                        {
                            ShowTableValuedGrid(paramName, e.RowIndex);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    ShowException(ex);
                }
            };

            _clear.MouseClick += (o, e) =>
            {
                try
                {
                    ClearParameters();
                }
                catch (System.Exception ex)
                {
                    ShowException(ex);
                }
            };
        }

    }
}
