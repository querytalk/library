#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using QueryTalk;

namespace QueryTalk.Wall
{
    internal partial class TestingForm : Form
    {
        private void ShowResult(Result result, ResultSet<dynamic> outputValues)
        {
            bool canceled = false;

            try
            {
                if (result == null)
                {
                    return;
                }

                int count = result.TableCount + 1; 

                if (count == 0)
                {
                    return;
                }

                RemoveTabs();

                TabPage tab = null;
                TabPage firstTab = null;
                int totalRowcount = 0;
                for (int i = 0; i < count; ++i)
                {
                    RefreshCounter(count, i);

                    if (_processing != Processing.Running)
                    {
                        canceled = true;
                        break;
                    }

                    string name = String.Format("{0}{1}", Wall.Text.Free.Table, i + 1);
                    int rowcount = 0;

                    // new tab
                    tab = new TabPage();
                    _tabControl.ThreadSafeInvoke(new Action(() => _tabControl.TabPages.Add(tab)));

                    // new grid
                    var grid = new DataGridView();
                    grid.DataError += OnDataError;
                    grid.CellFormatting += OnCellFormating;

                    tab.ThreadSafeInvoke(new Action(() => tab.Controls.Add(grid)));

                    // Do not move this method downwards because it may not work correctly - !
                    SetGridAppearance(grid, false); 

                    var isOutputValues = false;
                    ResultSet<dynamic> table;
                    if (i < count - 1)
                    {
                        table = result.GetTable(i + 1);
                    }
                    else
                    {
                        table = outputValues;
                        isOutputValues = true;
                    }

                    grid.ThreadSafeInvoke(new Action(() => _HandleGrid(count, i, grid, table)));

                    // hide grid and show NoTable content if there is no result set
                    if (((List<dynamic>)grid.DataSource).Count == 0)
                    {
                        tab.ThreadSafeInvoke(new Action(() =>
                        {
                            tab.Controls.Clear();
                            tab.Controls.Add(new TestingFormNoTable());
                        }));
                    }

                    rowcount = table.RowCount;

                    if (!isOutputValues)
                    {
                        totalRowcount += rowcount;
                    }

                    tab.ThreadSafeInvoke(new Action(() =>
                    {
                        tab.Text = String.Format("{0}", name);
                        tab.Tag = rowcount;
                    }));

                    if (i == 0)
                    {
                        firstTab = tab;
                    }
                }

                if (tab != null && _processing == Processing.Running)
                {
                    tab.ThreadSafeInvoke(new Action(() => tab.Text = Wall.Text.Free.ReturningValues));
                }

                _processing = Processing.Idle;

                _tabControl.ThreadSafeInvoke(new Action(() =>_HandleTabControl(totalRowcount)));

                if (canceled)
                {
                    Reader.ThrowOperationCancelledByUserException<dynamic>(_connectable, 0);
                }

                if (firstTab != null)
                {
                    _tabControl.ThreadSafeInvoke(new Action(() =>
                    {
                        _tabControl.SelectedTab = firstTab;
                    }));
                }
            }
            catch (QueryTalkException)
            {
                _processing = Processing.Idle;
                throw;
            }
            catch (System.Exception ex)
            {
                _processing = Processing.Idle;
                throw ClrException(ex);
            }
        }

        private void _HandleTabControl(int totalRowcount)
        {
            _tabControl.Visible = true;
            _elapsed.Text = String.Format("{0}  ({1:N0} rows)", _elapsed.Text, totalRowcount);
            _elapsed.Visible = true;

            _tabControl.SelectedIndexChanged += (o, e) =>
            {
                var selectedTab = _tabControl.SelectedTab;

                if (selectedTab == _tabSQL)
                {
                    ShowRowCount(totalRowcount);
                }
                else
                {
                    if (selectedTab.Tag != null && selectedTab.Tag is int)
                    {
                        ShowRowCount(int.Parse(selectedTab.Tag.ToString()));
                    }
                    else
                    {
                        ShowRowCount(-1);
                    }
                }
            };
        }

        private static void _HandleGrid(int count, int i, DataGridView grid, ResultSet<dynamic> table)
        {
            grid.DataSource = table.ToList();
            var columnsToReplace = new List<Tuple<DataGridViewColumn, DataGridViewTextBoxColumn>>();
            foreach (DataGridViewColumn column in grid.Columns)
            {
                if (column.ValueType == typeof(System.Byte[]) || column.ValueType == typeof(System.Boolean))
                {
                    DataGridViewTextBoxColumn newColumn = new DataGridViewTextBoxColumn();
                    newColumn.Name = column.Name;
                    newColumn.ValueType = typeof(System.String);
                    newColumn.DataPropertyName = column.Name;
                    newColumn.DisplayIndex = column.DisplayIndex;
                    columnsToReplace.Add(Tuple.Create(column, newColumn));
                }
            }

            columnsToReplace.ForEach(column =>
            {
                grid.Columns.Remove(column.Item1);
                grid.Columns.Add(column.Item2);
            });

            // the special appearance of the returning values
            if (i == count - 1)
            {
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
        }

        private void OnDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            var view = (DataGridView)sender;
            view.Rows[e.RowIndex].ErrorText = Wall.Text.Free.DataGridErrorMessage;
            e.ThrowException = _throwGridException;
        }

        private void OnCellFormating(object o, DataGridViewCellFormattingEventArgs e)
        {
            // null
            if (e.Value == null)
            {
                e.Value = Wall.Text.Null;
                e.CellStyle.ForeColor = _defaultNullColor;
                e.FormattingApplied = true;
                return;
            }

            // format DateTime as ISO 8601 (string) 
            if (e.Value is DateTime)
            {
                var value = (DateTime)e.Value;
                object value2;

                // is clean date
                if (value.Date == value)
                {
                    value2 = String.Format("{0:yyyy-MM-dd}", e.Value);
                }
                else
                {
                    value2 = String.Format("{0:yyyy-MM-dd HH:mm:ss.fffffff}", e.Value);
                }
                e.Value = value2;
                e.FormattingApplied = true;
                return;
            }

            // DateTimeOffset
            if (e.Value is DateTimeOffset)
            {
                object value2 = String.Format("{0:o}", e.Value);
                e.Value = value2;
                e.FormattingApplied = true;
                return;
            }

            // byte array
            if (e.Value is System.Byte[])
            {
                var value2 = Mapping.Build(e.Value, Mapping.DefaultBinaryType);
                e.Value = value2;
                e.FormattingApplied = true;
                return;
            }
        }

        private void RefreshCounter(int count, int elapsed)
        {
            if (count > _progressTabThreshold)
            {
                if (elapsed == 0)
                {
                    _elapsed.ThreadSafeInvoke(new Action(() =>
                    {
                        _progress.Value = 0;
                        _progress.Maximum = count;
                        _progress.Visible = true;
                    }));
                }

                if (elapsed + 1 >= count)
                {
                    _elapsed.ThreadSafeInvoke(new Action(() => _progress.Visible = false));
                }

                _elapsed.ThreadSafeInvoke(new Action(() => _progress.PerformStep()));
            }
        }

        private void ShowRowCount(int rowCount)
        {
            this.ThreadSafeInvoke(new Action(() =>
            {
                if (rowCount > 0)
                {
                    _statusRows.Text = String.Format("{0:N0} rows", rowCount);
                }
                else
                {
                    _statusRows.Text = null;  
                }
            }));
        }

        private void RemoveTabs()
        {
            _tabControl.ThreadSafeInvoke(new Action(() =>
            {
                _tabControl.SelectedTab = _tabSQL;
            }));

            foreach (TabPage tab in _tabControl.TabPages)
            {
                if (tab == _tabSQL || Variable.Detect(tab.Text) || Common.IsTempTable(tab.Text))
                {
                    continue;
                }

                _tabControl.ThreadSafeInvoke(new Action(() => _tabControl.TabPages.Remove(tab)));
            }
        }

        private void ShowException(System.Exception exception)
        {
            _processing = Processing.Idle;
            _tabControl.ThreadSafeInvoke(new Action(() =>
            {
                _tabControl.SelectedTab = _tabSQL;
                _tabControl.Visible = true;
            }));
            _error.ThreadSafeInvoke(new Action(() => _error.ForeColor = Color.Red));

            if (exception is QueryTalkException)
            {
                _error.ThreadSafeInvoke(new Action(() => _error.Text = ((QueryTalkException)exception).Report));

                // Reset "Cancel" exception if an exception occured due to the user cancellation of the processing.
                _connectable.ForceAsyncCanceledReset();
            }
            else
            {
                _error.ThreadSafeInvoke(new Action(() => _error
                    .AppendText(Wall.Text.Free.UnexpectedException, Color.Black)
                    .AppendText(Environment.NewLine + Environment.NewLine, Color.Black)
                    .AppendText(exception.Message, Color.Red)
                    .AppendText(Environment.NewLine + Environment.NewLine, Color.Black)
                    .AppendText(Wall.Text.Free.ReportExceptionToQueryTalk, Color.Black)));
            }
            _sqlSplitter.ThreadSafeInvoke(new Action(() => _sqlSplitter.Panel2Collapsed = false));
        }

        private void ShowMessage(string message)
        {
            _error.ThreadSafeInvoke(new Action(() =>
            {
                _error.ForeColor = Color.Black;
                _error.Text = message;
            }));
            _sqlSplitter.ThreadSafeInvoke(new Action(() => _sqlSplitter.Panel2Collapsed = false));
        }

        private void ShowElapsed()
        {
            _elapsed.ThreadSafeInvoke(new Action(() => _elapsed.Text = String.Format(
                "{0:N0} ms", _watch.ElapsedMilliseconds)));
        }

        private void ShowTableValuedGrid(string param, int rowIndex)
        {
            try
            {
                if (_params[param].Processing == Processing.Running)
                {
                    ShowTableParamStop(param, rowIndex, Processing.Idle);
                    return;
                }

                // get argument, it must exist
                ParameterArgument argument = _connectable.Executable.Arguments
                    .Where(arg => arg.ParamName == param)
                    .Select(arg => arg)
                    .FirstOrDefault();

                TabPage tab = FindTab(argument.ParamName);

                if (tab == null)
                {
                    tab = new TabPage();
                    tab.ThreadSafeInvoke(new Action(() => tab.Text = argument.ParamName));
                    _tabControl.ThreadSafeInvoke(new Action(() => _tabControl.TabPages.Add(tab)));
                    
                    var grid = new DataGridView();

                    var isReadOnly = (argument.DT == DT.TableVariable);

                    SetGridAppearance(grid, !isReadOnly);

                    grid.ThreadSafeInvoke(new Action(() =>
                    {
                        // lock grid in case of table variables (modified data does not affect the query)
                        if (isReadOnly)
                        {
                            grid.ReadOnly = true;
                            grid.AllowUserToAddRows = false;
                        }

                        grid.DataError += OnDataError;
                    }));

                    if (_params[param].Processing != 0)
                    {
                        return;
                    }

                    ShowTableParamStop(param, rowIndex, Processing.Running);
                    _params[param].Async = ((View)argument.Original)
                        .ConnectByInternal(_connectable.Client, _connectable.ConnectionString)
                        .GoAsync<DataTable>(ret =>
                        {
                            if (ret.AsyncStatus == AsyncStatus.Completed)
                            {
                                // show data
                                grid.ThreadSafeInvoke(new Action(() => grid.DataSource = ret.ToDataTable()));
                                tab.ThreadSafeInvoke(new Action(() => tab.Controls.Add(grid)));
                                ShowTableParamStop(param, rowIndex, Processing.Completed);

                                // set rowCount
                                ShowRowCount(ret.RowCount);
                                tab.Tag = ret.RowCount;  
                            }
                            else
                            {
                                ShowException(ret.Exception);
                                ShowTableParamStop(param, rowIndex, 0);
                            }
                        });
                }

                _tabControl.ThreadSafeInvoke(new Action(() => _tabControl.SelectedTab = tab));

            }
            catch (System.Exception ex)
            {
                ShowException(ex);
                ShowTableParamStop(param, rowIndex, 0);
            }
        }

        private void ShowTableParamStop(string param, int rowIndex, Processing processing)
        {
            _parameters.ThreadSafeInvoke(new Action(() =>
            {
                if (processing == Processing.Running)
                {
                    _parameters.Rows[rowIndex].Cells[2].Value = Wall.Text.Free.Stop;
                    _parameters.Rows[rowIndex].Cells[2].Style.ForeColor = Color.Red;
                    _parameters.Rows[rowIndex].Cells[2].Style.SelectionForeColor = Color.Red;
                    _params[param].Processing = processing; 
                }
                else if (processing == Processing.Idle)
                {
                    _parameters.Rows[rowIndex].Cells[2].Value = Wall.Text.ThreeDots;
                    _parameters.Rows[rowIndex].Cells[2].Style.ForeColor = Color.Black;
                    _parameters.Rows[rowIndex].Cells[2].Style.SelectionForeColor = Color.Black;
                    _params[param].Processing = processing; 
                    _params[param].Async.Cancel();


                    _tabControl.ThreadSafeInvoke(new Action(() =>
                    {
                        var tab = FindTab(param);
                        if (tab != null)
                        {
                            _tabControl.TabPages.Remove(tab);
                        }
                    }));
                }
                else
                {
                    _parameters.Rows[rowIndex].Cells[2].Value = Wall.Text.ThreeDots;
                    _parameters.Rows[rowIndex].Cells[2].Style.ForeColor = Color.Black;
                    _parameters.Rows[rowIndex].Cells[2].Style.SelectionForeColor = Color.Black;
                    _params[param].Processing = processing;
                }
            }));
        }

    }
}
