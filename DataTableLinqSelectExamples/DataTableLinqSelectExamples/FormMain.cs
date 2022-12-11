using System;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel;

namespace DataTableLinqSelectExamples
{
    public partial class FormMain : Form
    {
        private DataTable fullTable; // complete table, for example from a select in a database.
        private readonly string defaultDateMask; // To check if MaskedTextBoxes are empty value.

        public FormMain()
        {
            InitializeComponent();

            #region Set DataGridView visual configurations
            DataGridView_Output.EnableHeadersVisualStyles = false;
            DataGridView_Output.BorderStyle = BorderStyle.None;

            DataGridView_Output.AutoSize = true;
            DataGridView_Output.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            DataGridView_Output.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            DataGridView_Output.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            DataGridView_Output.AllowUserToAddRows = false;
            DataGridView_Output.AllowUserToDeleteRows = false;
            DataGridView_Output.AllowUserToResizeRows = false;
            DataGridView_Output.RowHeadersVisible = false;
            DataGridView_Output.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            DataGridView_Output.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            #endregion

            #region Set MakedTextBoxes format
            MaskedTextBox_Begin.Mask = "00/00/0000";
            MaskedTextBox_End.Mask = "00/00/0000";
            defaultDateMask = MaskedTextBox_Begin.Text;
            #endregion
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            #region Build table example

            fullTable = new DataTable();
            fullTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Text", typeof(string)),
                new DataColumn("DateTime", typeof(DateTime))
            });
            string[,] values =
            {
                { "Pitágoras", "14/03/2021 03:14:15" },
                { "Euclídes",  "03/08/2022 15:59:01" },
                { "Euler",     "07/09/2020 02:12:12" },
                { "Newton",    "08/09/2022 16:00:12" },
                { "Leibniz",   "08/09/2021 03:00:12" },
                { "Poincaré",  "01/05/2020 16:55:12" },
                { "Lusternik", "12/08/2021 17:54:09" },
            };
            for (var row=0; row<values.GetLength(0); row++)
            {
                DataRow dataRow = fullTable.NewRow();
                for (var col=0; col<values.GetLength(1); col++)
                {
                    dataRow[col] = values[row, col];
                }
                fullTable.Rows.Add(dataRow);
            }

            #endregion

            DataGridView_Output.DataSource = fullTable;
        }

        private void DataFilter()
        {
            DataTable tmpTable = fullTable.Copy();
            if (TextBox_Search.Text != "")
            {
                tmpTable = FilterTable.Filter(tmpTable, "Text", TextBox_Search.Text, ConditionalFilter.Contains);
            }
            if (MaskedTextBox_Begin.Text != defaultDateMask)
            {
                if (DateTime.TryParse(MaskedTextBox_Begin.Text, out DateTime dateTime))
                {
                    tmpTable = FilterTable.Filter(tmpTable, "DateTime", dateTime, ConditionalFilter.GreaterOrEqual);
                }
            }
            if (MaskedTextBox_End.Text != defaultDateMask)
            {
                if (DateTime.TryParse(MaskedTextBox_End.Text, out DateTime dateTime))
                {
                    tmpTable = FilterTable.Filter(tmpTable, "DateTime", dateTime, ConditionalFilter.LessOrEqual);
                }
            }
            DataGridView_Output.DataSource = tmpTable;
        }

        private void Filters_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataFilter();
            }
        }

        private void MaskedTextBox_Begin_Validating(object sender, CancelEventArgs e)
        {
            if (MaskedTextBox_Begin.Text != defaultDateMask)
            {
                if (DateTime.TryParse(MaskedTextBox_Begin.Text, out _))
                {
                    DataFilter();
                }
                else
                {
                    MessageBox.Show("The begin date entered cannot be converted.");
                    e.Cancel = true;
                }
            }
        }

        private void MaskedTextBox_End_Validating(object sender, CancelEventArgs e)
        {
            if (MaskedTextBox_End.Text != defaultDateMask)
            {
                if (DateTime.TryParse(MaskedTextBox_End.Text, out _))
                {
                    DataFilter();
                }
                else
                {
                    MessageBox.Show("The end date entered cannot be converted.");
                    e.Cancel = true;
                }
            }
        }
    }
}
