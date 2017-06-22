#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    partial class TestingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestingForm));
            this._mainSplitter = new System.Windows.Forms.SplitContainer();
            this.panelParametersGrid = new System.Windows.Forms.Panel();
            this._parameters = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._panelTop = new System.Windows.Forms.Panel();
            this._rowCount = new System.Windows.Forms.Label();
            this._clear = new System.Windows.Forms.LinkLabel();
            this._run = new System.Windows.Forms.Button();
            this._off = new System.Windows.Forms.LinkLabel();
            this._progress = new System.Windows.Forms.ProgressBar();
            this._elapsed = new System.Windows.Forms.Label();
            this._interrupt = new System.Windows.Forms.LinkLabel();
            this._loaderPic = new System.Windows.Forms.PictureBox();
            this._name = new System.Windows.Forms.Label();
            this._panelSQL = new System.Windows.Forms.Panel();
            this._sql = new System.Windows.Forms.RichTextBox();
            this._tabControl = new System.Windows.Forms.TabControl();
            this._tabSQL = new System.Windows.Forms.TabPage();
            this._sqlSplitter = new System.Windows.Forms.SplitContainer();
            this._sqlContentPanel = new System.Windows.Forms.Panel();
            this._error = new System.Windows.Forms.RichTextBox();
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._statusRows = new System.Windows.Forms.ToolStripStatusLabel();
            this._statusName = new System.Windows.Forms.ToolStripStatusLabel();
            this._mainPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this._mainSplitter)).BeginInit();
            this._mainSplitter.Panel1.SuspendLayout();
            this._mainSplitter.Panel2.SuspendLayout();
            this._mainSplitter.SuspendLayout();
            this.panelParametersGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._parameters)).BeginInit();
            this._panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._loaderPic)).BeginInit();
            this._panelSQL.SuspendLayout();
            this._tabControl.SuspendLayout();
            this._tabSQL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._sqlSplitter)).BeginInit();
            this._sqlSplitter.Panel1.SuspendLayout();
            this._sqlSplitter.Panel2.SuspendLayout();
            this._sqlSplitter.SuspendLayout();
            this._sqlContentPanel.SuspendLayout();
            this._statusStrip.SuspendLayout();
            this._mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _mainSplitter
            // 
            this._mainSplitter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this._mainSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainSplitter.Location = new System.Drawing.Point(0, 0);
            this._mainSplitter.Margin = new System.Windows.Forms.Padding(4);
            this._mainSplitter.Name = "_mainSplitter";
            // 
            // _mainSplitter.Panel1
            // 
            this._mainSplitter.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._mainSplitter.Panel1.Controls.Add(this.panelParametersGrid);
            this._mainSplitter.Panel1.Controls.Add(this._panelTop);
            // 
            // _mainSplitter.Panel2
            // 
            this._mainSplitter.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._mainSplitter.Panel2.Controls.Add(this._panelSQL);
            this._mainSplitter.Size = new System.Drawing.Size(1467, 748);
            this._mainSplitter.SplitterDistance = 519;
            this._mainSplitter.SplitterWidth = 10;
            this._mainSplitter.TabIndex = 1;
            // 
            // panelParametersGrid
            // 
            this.panelParametersGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.panelParametersGrid.Controls.Add(this._parameters);
            this.panelParametersGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelParametersGrid.Location = new System.Drawing.Point(0, 369);
            this.panelParametersGrid.Margin = new System.Windows.Forms.Padding(4);
            this.panelParametersGrid.Name = "panelParametersGrid";
            this.panelParametersGrid.Padding = new System.Windows.Forms.Padding(0, 0, 25, 40);
            this.panelParametersGrid.Size = new System.Drawing.Size(519, 379);
            this.panelParametersGrid.TabIndex = 3;
            // 
            // _parameters
            // 
            this._parameters.AllowUserToAddRows = false;
            this._parameters.AllowUserToDeleteRows = false;
            this._parameters.AllowUserToResizeRows = false;
            this._parameters.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this._parameters.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 11F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._parameters.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._parameters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._parameters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Calibri", 11F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._parameters.DefaultCellStyle = dataGridViewCellStyle5;
            this._parameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this._parameters.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._parameters.Location = new System.Drawing.Point(0, 0);
            this._parameters.Margin = new System.Windows.Forms.Padding(4, 4, 4, 40);
            this._parameters.Name = "_parameters";
            this._parameters.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Calibri", 11F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._parameters.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Calibri", 11F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            this._parameters.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this._parameters.Size = new System.Drawing.Size(494, 339);
            this._parameters.TabIndex = 1;
            // 
            // Column1
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.HeaderText = "ParamName";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 111;
            // 
            // Column2
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column2.HeaderText = "DataType";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 88;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(182)))), ((int)(((byte)(237)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column3.HeaderText = "Value";
            this.Column3.MinimumWidth = 50;
            this.Column3.Name = "Column3";
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _panelTop
            // 
            this._panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._panelTop.Controls.Add(this._rowCount);
            this._panelTop.Controls.Add(this._clear);
            this._panelTop.Controls.Add(this._run);
            this._panelTop.Controls.Add(this._off);
            this._panelTop.Controls.Add(this._progress);
            this._panelTop.Controls.Add(this._elapsed);
            this._panelTop.Controls.Add(this._interrupt);
            this._panelTop.Controls.Add(this._loaderPic);
            this._panelTop.Controls.Add(this._name);
            this._panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelTop.Location = new System.Drawing.Point(0, 0);
            this._panelTop.Margin = new System.Windows.Forms.Padding(4);
            this._panelTop.Name = "_panelTop";
            this._panelTop.Size = new System.Drawing.Size(519, 369);
            this._panelTop.TabIndex = 0;
            // 
            // _rowCount
            // 
            this._rowCount.AutoSize = true;
            this._rowCount.BackColor = System.Drawing.Color.Transparent;
            this._rowCount.Font = new System.Drawing.Font("Segoe UI", 10F);
            this._rowCount.ForeColor = System.Drawing.Color.Black;
            this._rowCount.Location = new System.Drawing.Point(54, 106);
            this._rowCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._rowCount.Name = "_rowCount";
            this._rowCount.Size = new System.Drawing.Size(67, 23);
            this._rowCount.TabIndex = 10;
            this._rowCount.Text = "Rows: 0";
            this._rowCount.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this._rowCount.Visible = false;
            // 
            // _clear
            // 
            this._clear.ActiveLinkColor = System.Drawing.Color.WhiteSmoke;
            this._clear.AutoSize = true;
            this._clear.Font = new System.Drawing.Font("Segoe UI", 10F);
            this._clear.ForeColor = System.Drawing.Color.DimGray;
            this._clear.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._clear.LinkColor = System.Drawing.Color.DimGray;
            this._clear.Location = new System.Drawing.Point(423, 342);
            this._clear.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._clear.Name = "_clear";
            this._clear.Size = new System.Drawing.Size(46, 23);
            this._clear.TabIndex = 19;
            this._clear.TabStop = true;
            this._clear.Text = "clear";
            // 
            // _run
            // 
            this._run.BackColor = System.Drawing.Color.LightGray;
            this._run.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this._run.FlatAppearance.BorderSize = 0;
            this._run.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._run.Font = new System.Drawing.Font("Segoe UI Semibold", 23F, System.Drawing.FontStyle.Bold);
            this._run.ForeColor = System.Drawing.Color.Black;
            this._run.Location = new System.Drawing.Point(164, 194);
            this._run.Name = "_run";
            this._run.Size = new System.Drawing.Size(187, 76);
            this._run.TabIndex = 18;
            this._run.Text = "Go";
            this._run.UseVisualStyleBackColor = false;
            // 
            // _off
            // 
            this._off.ActiveLinkColor = System.Drawing.Color.WhiteSmoke;
            this._off.AutoSize = true;
            this._off.Font = new System.Drawing.Font("Segoe UI", 11F);
            this._off.ForeColor = System.Drawing.Color.DimGray;
            this._off.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._off.LinkColor = System.Drawing.Color.DimGray;
            this._off.Location = new System.Drawing.Point(366, 10);
            this._off.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._off.Name = "_off";
            this._off.Size = new System.Drawing.Size(132, 25);
            this._off.TabIndex = 8;
            this._off.TabStop = true;
            this._off.Text = "Close and OFF";
            // 
            // _progress
            // 
            this._progress.Location = new System.Drawing.Point(362, 258);
            this._progress.Margin = new System.Windows.Forms.Padding(4);
            this._progress.Name = "_progress";
            this._progress.Size = new System.Drawing.Size(77, 12);
            this._progress.Step = 1;
            this._progress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this._progress.TabIndex = 7;
            this._progress.Visible = false;
            // 
            // _elapsed
            // 
            this._elapsed.AutoSize = true;
            this._elapsed.BackColor = System.Drawing.Color.Transparent;
            this._elapsed.Font = new System.Drawing.Font("Segoe UI", 10F);
            this._elapsed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(135)))), ((int)(((byte)(0)))));
            this._elapsed.Location = new System.Drawing.Point(173, 277);
            this._elapsed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._elapsed.Name = "_elapsed";
            this._elapsed.Size = new System.Drawing.Size(19, 23);
            this._elapsed.TabIndex = 5;
            this._elapsed.Text = "0";
            this._elapsed.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this._elapsed.Visible = false;
            // 
            // _interrupt
            // 
            this._interrupt.AutoSize = true;
            this._interrupt.Font = new System.Drawing.Font("Segoe UI", 11F);
            this._interrupt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this._interrupt.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._interrupt.LinkColor = System.Drawing.Color.Red;
            this._interrupt.Location = new System.Drawing.Point(259, 164);
            this._interrupt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._interrupt.Name = "_interrupt";
            this._interrupt.Size = new System.Drawing.Size(86, 25);
            this._interrupt.TabIndex = 4;
            this._interrupt.TabStop = true;
            this._interrupt.Text = "interrupt";
            // 
            // _loaderPic
            // 
            this._loaderPic.Image = global::QueryTalk.Properties.Resources.loader;
            this._loaderPic.Location = new System.Drawing.Point(362, 205);
            this._loaderPic.Margin = new System.Windows.Forms.Padding(4);
            this._loaderPic.Name = "_loaderPic";
            this._loaderPic.Size = new System.Drawing.Size(48, 49);
            this._loaderPic.TabIndex = 3;
            this._loaderPic.TabStop = false;
            this._loaderPic.Visible = false;
            // 
            // _name
            // 
            this._name.AutoSize = true;
            this._name.Font = new System.Drawing.Font("Segoe UI", 25F);
            this._name.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(61)))), ((int)(((byte)(135)))), ((int)(((byte)(211)))));
            this._name.Location = new System.Drawing.Point(30, 53);
            this._name.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._name.Name = "_name";
            this._name.Size = new System.Drawing.Size(99, 57);
            this._name.TabIndex = 2;
            this._name.Text = "N/A";
            // 
            // _panelSQL
            // 
            this._panelSQL.Controls.Add(this._sql);
            this._panelSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelSQL.Location = new System.Drawing.Point(0, 0);
            this._panelSQL.Name = "_panelSQL";
            this._panelSQL.Padding = new System.Windows.Forms.Padding(40, 20, 10, 20);
            this._panelSQL.Size = new System.Drawing.Size(938, 748);
            this._panelSQL.TabIndex = 1;
            // 
            // _sql
            // 
            this._sql.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._sql.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._sql.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sql.Font = new System.Drawing.Font("Calibri", 12F);
            this._sql.ForeColor = System.Drawing.Color.Black;
            this._sql.Location = new System.Drawing.Point(40, 20);
            this._sql.Margin = new System.Windows.Forms.Padding(4);
            this._sql.Name = "_sql";
            this._sql.Size = new System.Drawing.Size(888, 708);
            this._sql.TabIndex = 0;
            this._sql.Text = "";
            // 
            // _tabControl
            // 
            this._tabControl.Controls.Add(this._tabSQL);
            this._tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabControl.Font = new System.Drawing.Font("Calibri", 11F);
            this._tabControl.ItemSize = new System.Drawing.Size(50, 23);
            this._tabControl.Location = new System.Drawing.Point(0, 0);
            this._tabControl.Margin = new System.Windows.Forms.Padding(0);
            this._tabControl.Name = "_tabControl";
            this._tabControl.SelectedIndex = 0;
            this._tabControl.ShowToolTips = true;
            this._tabControl.Size = new System.Drawing.Size(1529, 803);
            this._tabControl.TabIndex = 0;
            // 
            // _tabSQL
            // 
            this._tabSQL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._tabSQL.Controls.Add(this._sqlSplitter);
            this._tabSQL.Font = new System.Drawing.Font("Calibri", 11F);
            this._tabSQL.Location = new System.Drawing.Point(4, 27);
            this._tabSQL.Margin = new System.Windows.Forms.Padding(0);
            this._tabSQL.Name = "_tabSQL";
            this._tabSQL.Size = new System.Drawing.Size(1521, 772);
            this._tabSQL.TabIndex = 1;
            this._tabSQL.Text = "SQL";
            // 
            // _sqlSplitter
            // 
            this._sqlSplitter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._sqlSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sqlSplitter.Location = new System.Drawing.Point(0, 0);
            this._sqlSplitter.Margin = new System.Windows.Forms.Padding(0);
            this._sqlSplitter.Name = "_sqlSplitter";
            this._sqlSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _sqlSplitter.Panel1
            // 
            this._sqlSplitter.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this._sqlSplitter.Panel1.Controls.Add(this._sqlContentPanel);
            this._sqlSplitter.Panel1.Padding = new System.Windows.Forms.Padding(27, 12, 27, 12);
            // 
            // _sqlSplitter.Panel2
            // 
            this._sqlSplitter.Panel2.BackColor = System.Drawing.Color.Gainsboro;
            this._sqlSplitter.Panel2.Controls.Add(this._error);
            this._sqlSplitter.Panel2.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this._sqlSplitter.Panel2Collapsed = true;
            this._sqlSplitter.Size = new System.Drawing.Size(1521, 772);
            this._sqlSplitter.SplitterDistance = 279;
            this._sqlSplitter.SplitterWidth = 5;
            this._sqlSplitter.TabIndex = 0;
            // 
            // _sqlContentPanel
            // 
            this._sqlContentPanel.Controls.Add(this._mainSplitter);
            this._sqlContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sqlContentPanel.Location = new System.Drawing.Point(27, 12);
            this._sqlContentPanel.Margin = new System.Windows.Forms.Padding(0);
            this._sqlContentPanel.Name = "_sqlContentPanel";
            this._sqlContentPanel.Size = new System.Drawing.Size(1467, 748);
            this._sqlContentPanel.TabIndex = 2;
            // 
            // _error
            // 
            this._error.BackColor = System.Drawing.Color.Gainsboro;
            this._error.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._error.Dock = System.Windows.Forms.DockStyle.Fill;
            this._error.Font = new System.Drawing.Font("Calibri", 10F);
            this._error.ForeColor = System.Drawing.Color.Red;
            this._error.Location = new System.Drawing.Point(13, 12);
            this._error.Margin = new System.Windows.Forms.Padding(4);
            this._error.Name = "_error";
            this._error.Size = new System.Drawing.Size(124, 22);
            this._error.TabIndex = 0;
            this._error.Text = "";
            // 
            // _statusStrip
            // 
            this._statusStrip.BackColor = System.Drawing.Color.Gainsboro;
            this._statusStrip.Font = new System.Drawing.Font("Segoe UI", 17F);
            this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._statusRows,
            this._statusName});
            this._statusStrip.Location = new System.Drawing.Point(0, 803);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(1529, 33);
            this._statusStrip.TabIndex = 1;
            // 
            // _statusRows
            // 
            this._statusRows.BackColor = System.Drawing.Color.Transparent;
            this._statusRows.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._statusRows.Font = new System.Drawing.Font("Segoe UI", 11F);
            this._statusRows.Margin = new System.Windows.Forms.Padding(30, -2, 0, 0);
            this._statusRows.Name = "_statusRows";
            this._statusRows.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this._statusRows.Padding = new System.Windows.Forms.Padding(150, 0, 0, 10);
            this._statusRows.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._statusRows.Size = new System.Drawing.Size(214, 35);
            this._statusRows.Text = "(rows)";
            // 
            // _statusName
            // 
            this._statusName.BackColor = System.Drawing.Color.Transparent;
            this._statusName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._statusName.Font = new System.Drawing.Font("Segoe UI", 11F);
            this._statusName.ForeColor = System.Drawing.Color.Black;
            this._statusName.Margin = new System.Windows.Forms.Padding(0, -2, 0, 0);
            this._statusName.Name = "_statusName";
            this._statusName.Padding = new System.Windows.Forms.Padding(50, 0, 0, 10);
            this._statusName.Size = new System.Drawing.Size(121, 35);
            this._statusName.Text = "(name)";
            // 
            // _mainPanel
            // 
            this._mainPanel.Controls.Add(this._tabControl);
            this._mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainPanel.Location = new System.Drawing.Point(0, 0);
            this._mainPanel.Name = "_mainPanel";
            this._mainPanel.Size = new System.Drawing.Size(1529, 803);
            this._mainPanel.TabIndex = 2;
            // 
            // TestingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1529, 836);
            this.Controls.Add(this._mainPanel);
            this.Controls.Add(this._statusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TestingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TestingForm";
            this._mainSplitter.Panel1.ResumeLayout(false);
            this._mainSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._mainSplitter)).EndInit();
            this._mainSplitter.ResumeLayout(false);
            this.panelParametersGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._parameters)).EndInit();
            this._panelTop.ResumeLayout(false);
            this._panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._loaderPic)).EndInit();
            this._panelSQL.ResumeLayout(false);
            this._tabControl.ResumeLayout(false);
            this._tabSQL.ResumeLayout(false);
            this._sqlSplitter.Panel1.ResumeLayout(false);
            this._sqlSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._sqlSplitter)).EndInit();
            this._sqlSplitter.ResumeLayout(false);
            this._sqlContentPanel.ResumeLayout(false);
            this._statusStrip.ResumeLayout(false);
            this._statusStrip.PerformLayout();
            this._mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer _mainSplitter;
        private System.Windows.Forms.Panel _panelTop;
        private System.Windows.Forms.TabControl _tabControl;
        private System.Windows.Forms.Label _name;
        private System.Windows.Forms.PictureBox _loaderPic;
        private System.Windows.Forms.LinkLabel _interrupt;
        private System.Windows.Forms.TabPage _tabSQL;
        private System.Windows.Forms.SplitContainer _sqlSplitter;
        private System.Windows.Forms.RichTextBox _sql;
        private System.Windows.Forms.RichTextBox _error;
        private System.Windows.Forms.Panel _sqlContentPanel;
        private System.Windows.Forms.Label _elapsed;
        private System.Windows.Forms.DataGridView _parameters;
        private System.Windows.Forms.ProgressBar _progress;
        private System.Windows.Forms.Panel panelParametersGrid;
        private System.Windows.Forms.LinkLabel _off;
        private System.Windows.Forms.Label _rowCount;
        private System.Windows.Forms.Button _run;
        private System.Windows.Forms.LinkLabel _clear;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Panel _panelSQL;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _statusRows;
        private System.Windows.Forms.Panel _mainPanel;
        private System.Windows.Forms.ToolStripStatusLabel _statusName;

    }
}