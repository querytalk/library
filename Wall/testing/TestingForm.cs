#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace QueryTalk.Wall
{
    internal partial class TestingForm : Form
    {

        #region Fields

        private readonly bool _throwGridException = false;

        private readonly Color _defaultColor = Color.FromArgb(250, 250, 250);
        private readonly Color _defaultForeColor = Color.Black;
        private readonly Color _defaultEditableForeColor = Color.FromArgb(0, 135, 0); 
        private readonly Color _defaultNullColor = Color.FromArgb(100, 100, 100);
        private readonly Color _defaultSelectedColor = Color.FromArgb(80, 182, 237); 
        private readonly Color _defaultSelectedReadonlyColor = Color.LightGray;
        private readonly Color _cellBorderColor = Color.FromArgb(169, 169, 169);

        private enum Processing : int { Idle, Running, Completed }

        // how many tabs are needed to show the progress bar
        private readonly int _progressTabThreshold = 30;

        private readonly Connectable _connectable;      
        private string _body;                           
        private Async _async;                          
        private Dictionary<string, TVP> _params;       
        private Stopwatch _watch = new Stopwatch();     
        private ContextMenuStrip _sqlContextMenu;     

        private Processing __processing = Processing.Idle;
        private Processing _processing
        {
            get
            {
                return __processing;
            }
            set
            {
                __processing = value;
                AdjustRunButton();
            }
        }

        private TransactionCodeInfo _tranCodeInfo;

        private ToolTip _offToolTip;
        private ToolTip _interruptToolTip;

        #endregion

        #region Nested classes

        // output value class - leave it public because it is used as a loader data class
        public class OutputValue
        {
            public string Output { get; private set; }
            public object Value { get; private set; }
            internal OutputValue(string output, object value)
            {
                Output = output;
                Value = value;
            }
        }

        // nested class containing table-valued param info for async loading
        private class TVP
        {
            internal string ParamName { get; set; }
            internal bool IsTable { get; set; }
            internal bool IsBulk { get; set; }
            internal Processing Processing { get; set; }
            internal Async Async { get; set; }

            internal TVP(string paramName, bool isTable, bool isBulk)
            {
                ParamName = paramName;
                IsTable = isTable;
                IsBulk = isBulk;
            }
        }

        #endregion

        #region Constructor

        internal TestingForm(Connectable connectable, TestingOption option, string title)
        {
            try
            {
                _connectable = connectable;                     // store connectable object which may be used after the testing, for the execution
                _body = connectable.Executable.Body;            // store body
                InitializeComponent();

                _offToolTip = new ToolTip();
                _offToolTip.SetToolTip(_off, "Close and set Testing Environment OFF.");
                _interruptToolTip = new ToolTip();
                _interruptToolTip.SetToolTip(_interrupt, "Interrupt the execution of the Testing Environment");

                _params = new Dictionary<string, TVP>();

                this.ShowIcon = false;
                Text = String.Format("{0} | {1}", Wall.Text.Free.QtTesting, ParseConnString());
                _statusRows.Text = null;

                var name = ((IName)_connectable).Name;
                if (title != null)
                {
                    _name.Text = title;
                    _statusName.Text = title;
                }
                else if (name != null)
                {
                    _name.Text = name;
                    _statusName.Text = (name == Wall.Text.NotAvailable) ? null : name;
                }
                else
                {
                    _name.Text = Wall.Text.NotAvailable;
                    _statusName.Text = null;
                }

                _sql.Text = _connectable.Body;

                _tranCodeInfo = new TransactionCodeInfo(_connectable.Body);

                // if compilable object is a stored procedure, show extra message
                if (_connectable.Executable.Compilable.CompilableType == Compilable.ObjectType.StoredProc)
                {
                    ShowMessage(Wall.Text.Free.StoredProcTestMessage);
                }

                _sqlContextMenu = new ContextMenuStrip();
                ToolStripMenuItem menuItem = new ToolStripMenuItem(Wall.Text.Free.RestoreSQL,
                    Properties.Resources.restore,
                    (o, e) =>
                    {
                        _sql.ThreadSafeInvoke(new Action(() => _sql.Text = _body));
                    });
                _sqlContextMenu.Items.Add(menuItem);
                _sql.ContextMenuStrip = _sqlContextMenu;

                AddEventHandlers();
                ShowParameterGrid();

                // clear loader cache
                //   note:
                //     We clear cache in order to remove all mapped loaders that contain graph objects (associations)
                //     that cannot be shown in the grid. Otherwise the grid does not show the correct data.
                Cache.Loaders.Clear();

                this.Load += (o, e) =>
                {
                    try
                    {
                        if (option == TestingOption.OpenAndExecute)
                        {
                            Run();
                        }

                    }
                    catch (Exception ex)
                    {
                        ShowException(ex);
                    }
                };

            }
            catch (System.Exception ex)
            {
                throw ClrException(ex);
            }
        }

        #endregion

        #region Cancel Processing

        private bool CancelProcessing()
        {
            if (_processing == Processing.Running)
            {
                if (_async != null)
                {
                    _processing = Processing.Idle;
                    _async.Cancel();
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Appearance

        private void SetGridAppearance(DataGridView grid, bool isEditable)
        {
            grid.ThreadSafeInvoke(new Action(() =>
            {
                grid.ColumnHeadersDefaultCellStyle.BackColor = _defaultSelectedColor; //  Color.Gray; // Color.DarkGray;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                grid.ColumnHeadersDefaultCellStyle.Font = new Font(Wall.Text.Free.DefaultFontFamily, 11.0f, FontStyle.Regular);
                grid.ColumnHeadersHeight = 30;

                grid.DefaultCellStyle = new DataGridViewCellStyle()
                {
                    BackColor = _defaultColor,
                    ForeColor = isEditable ? _defaultEditableForeColor : _defaultForeColor,
                    SelectionBackColor = _defaultSelectedReadonlyColor,
                    SelectionForeColor = _defaultForeColor,
                    Padding = new Padding(2),
                };

                grid.BackgroundColor = _defaultColor;
                grid.Font = new System.Drawing.Font(Wall.Text.Free.DefaultFontFamily, 11.0f);
                grid.BorderStyle = BorderStyle.None;
                grid.RowHeadersVisible = true;
                grid.RowHeadersWidth = 25;
                grid.RowTemplate.Height = 25;   
                grid.Dock = DockStyle.Fill;

                // very, very slow - !
                // grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }));
        }

        private void AdjustAppearance()
        {
            AdjustTabSQL();
        }

        private void AdjustTabSQL()
        {
            _error.ThreadSafeInvoke(new Action(() => _error.Text = ""));               
            _progress.ThreadSafeInvoke(new Action(() => _progress.Visible = false));    
            _sqlSplitter.ThreadSafeInvoke(new Action(() => _sqlSplitter.Panel2Collapsed = true));
        }

        private void AdjustRunButton()
        {
            if (_processing == Processing.Running)
            {
                _run.ThreadSafeInvoke(new Action(() =>
                {
                    _run.Image = null;
                    _run.Text = "Stop";
                    _loaderPic.Visible = true;
                }));
            }
            else
            {
                _run.ThreadSafeInvoke(new Action(() =>
                {
                    _run.Image = null;
                    _run.Text = "Go";
                    _loaderPic.Visible = false;
                }));
            }
        }

        #endregion

        #region Output values

        private static ResultSet<dynamic> GetOutputValues(Connectable connectable, Result result)
        {
            List<dynamic> values = new List<dynamic>();
            values.Add(new OutputValue(Wall.Text.Reserved.ReturnValue, result.ReturnValue));

            if (connectable.OutputArguments != null)
            {
                foreach (var argument in connectable.OutputArguments)
                {
                    values.Add(new OutputValue(argument.ParamName,
                        ((Value)argument.Original).Original));
                }
            }

            return new ResultSet<dynamic>(values);
        }

        #endregion

        #region Supporting methods

        private string ParseConnString()
        {
            var builder = new SqlConnectionStringBuilder(_connectable.ConnectionString);
            if (builder.IntegratedSecurity)
            {
                return String.Format("{0}.{1}", builder.DataSource, builder.InitialCatalog);
            }
            else
            {
                return String.Format("{0}.{1} ({2})", builder.DataSource, builder.InitialCatalog, builder.UserID);
            }
        }

        private static QueryTalkException ClrException(System.Exception ex)
        {
            var exception = new QueryTalkException("TestingForm", QueryTalkExceptionType.ClrException, 
                null, Wall.Text.Method.Test);
            exception.ClrException = ex;

            return exception;
        }

        private TabPage FindTab(string title)
        {
            foreach (TabPage tab in _tabControl.TabPages)
            {
                if (tab.Text.EqualsCS(title, false))
                {
                    return tab;
                }
            }
            return null;
        }

        // detect transaction elements
        //   attention:
        //     User is not allowed to change the original transactions.
        private bool ValidateTransactions()
        {
            return _tranCodeInfo.HasEqualCount(new TransactionCodeInfo(_sql.Text));
        }

        #endregion

    }
}
