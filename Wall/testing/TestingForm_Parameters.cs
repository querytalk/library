#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QueryTalk;

namespace QueryTalk.Wall
{
    internal partial class TestingForm : Form
    {
        private void ShowParameterGrid()
        {
            var explicitParams = _connectable.Executable.Compilable.GetRoot().ExplicitParams;
            if (explicitParams.Count() == 0)
            {
                _parameters.Visible = false;
                _clear.Visible = false;
                return;
            }

            // set grid appearance
            _parameters.Location = new Point(20, _parameters.Location.Y);
            _parameters.ColumnHeadersDefaultCellStyle.BackColor = _defaultColor;
            _parameters.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            _parameters.ColumnHeadersDefaultCellStyle.Font = new Font(Wall.Text.Free.DefaultFontFamily, 11.0f, FontStyle.Bold);
            _parameters.Font = new Font(Wall.Text.Free.DefaultFontFamily, 11.0f, FontStyle.Regular);
            _parameters.BackgroundColor = _defaultColor;
            _parameters.BorderStyle = BorderStyle.None;
            _parameters.RowHeadersVisible = false;
            _parameters.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            _parameters.DefaultCellStyle = new DataGridViewCellStyle()
            {
                Padding = new Padding(5)
            };
            _parameters.GridColor = _cellBorderColor;

            _clear.Visible = true;

            int i = 0;
            foreach (var param in explicitParams)
            {
                var argument = _connectable.Executable.Arguments.Where(a => a.ParamName == param.Name).FirstOrDefault();
                if (argument == null) { continue; }

                // get param type
                string paramType;
                if (param.IsConcatenator())
                {
                    paramType = "Concatenator";
                }
                else if (argument.DT.IsDataType())
                {
                    paramType = argument.DataType.Build() + (argument.IsOutput ? " (OUT)" : String.Empty);
                }
                else
                {
                    paramType = argument.DT.ToString();
                }

                // get param value
                string value = GetParamDisplayValue(argument).ToString();

                var row = new string[] { argument.ParamName, paramType, value };
                _parameters.Rows.Add(row);

                // create a table-valued param object and add it into the collection
                var prms = new TVP(argument.ParamName, argument.DT.IsTable(), argument.IsBulk);
                _params[argument.ParamName] = prms;

                // get Value cell
                var cell = _parameters.Rows[i].Cells[2];

                // freeze the inline parameters & bulk insert data
                if (argument.DT.IsInliner() || argument.IsBulk)
                {
                    _parameters.Rows[i].ReadOnly = true;
                }
                // disable editing of a table-valued param
                else if (argument.DT.IsTable())
                {
                    _parameters.Rows[i].ReadOnly = true;
                }
                // param value can be modified
                else
                {
                    cell.Style = new DataGridViewCellStyle() { ForeColor = _defaultEditableForeColor };
                }

                ++i;
            }
        }

        private static string GetParamDisplayValue(ParameterArgument argument)
        {
            if (argument.DT.IsDataType())                
            {
                if (argument.DT == DT.Datetime2)
                {
                    return Filter.TrimSingleQuotes(Mapping.BuildUnchecked((DateTime)argument.Value));
                }
                else
                {
                    return Filter.TrimSingleQuotes(Mapping.BuildUnchecked(argument.Value));
                }
            }
            else if (argument.DT.IsNameType() || argument.DT.IsTable())
            {
                if (argument.IsBulk)                   
                {
                    return Wall.Text.NotAvailable;
                }
                else if (argument.DT.IsTable())
                {
                    return Wall.Text.ThreeDots;       
                }
                else
                {
                    return argument.Value.ToString();  
                }
            }
            else if (argument.DT.IsInlinerOrConcatenator())
            {
                return argument.Value.ToString();
            }
            else
            {
                return Wall.Text.NotAvailable;
            }
        }

        // sets testing parameters
        //   note:
        //      The testing parameters does not interfere with the execution ones.
        private void SetTestingParameters(Executable executable)
        {
            int i = 0;

            foreach (var param in executable.Compilable.GetRoot().ExplicitParams)
            {
                var argument = executable.Arguments.Where(a => a.ParamName == param.Name).FirstOrDefault();
                if (argument == null)
                {
                    continue;
                }

                if (argument.DT.IsNotInliner())
                {
                    if (!argument.DT.IsTable())
                    {
                        argument.TestValue = Mapping.BuildTestValue(_parameters.Rows[i].Cells[2].Value.ToString(), argument.DataType);
                    }
                    else
                    {
                        var tab = FindTab(param.Name);
                        if (tab != null)
                        {
                            DataGridView grid = (DataGridView)tab.Controls[0];
                            argument.TestValue = Designer.ToView(grid.DataSource);
                        }
                    }
                }
                ++i;
            }
        }

        private void ClearParameters()
        {
            var i = 0;
            foreach (var param in _connectable.Executable.Compilable.GetRoot().ExplicitParams)
            {
                var argument = _connectable.Executable.Arguments.Where(a => a.ParamName == param.Name).FirstOrDefault();
                if (argument == null) { continue; }

                string defaultValue = Wall.Text.Null;

                if (param.IsOptional && param.Default.Value != null)
                {
                    defaultValue = Filter.TrimSingleQuotes(Mapping.BuildUnchecked(param.Default.Value));
                }

                if (param.IsDbType() || param.DT == DT.Udt)
                {
                    _parameters.Rows[i].Cells[2].Value = defaultValue;
                }

                ++i;
            }
        }

    }
}
