using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockApp
{
    public partial class FrmYearROE : Form
    {
        internal List<CompanyROE> ROEData { get; }
        private List<DisplayModel> ViewData = new List<DisplayModel>();
        private BindingList<FormData> DataSource = new BindingList<FormData>();

        public FrmYearROE()
        {
            InitializeComponent();

            this.ROEData = CompanyROE.GetAll();

            var data = ROEData.First();
            var gv = dataGridView1;
            gv.AutoGenerateColumns = false;
            gv.VirtualMode = true;

            var addColumnFunc = new Func<string, string, DataGridViewColumn>((dataMemberName, headerText) =>
            {
                var col = new DataGridViewColumn();
                col.Name = $"Col{dataMemberName}";
                col.DataPropertyName = dataMemberName;
                col.HeaderText = headerText;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                var cell = new DataGridViewTextBoxCell();
                col.CellTemplate = cell;

                return col;
            });
            var addPercentColumnFunc = new Func<string, string, DataGridViewColumn>((dataMemberName, headerText) =>
            {
                var col = addColumnFunc(dataMemberName, headerText);
                var cell = col.CellTemplate;
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                cell.Style.Format = "0.00";

                return col;
            });
            gv.Columns.Add(addColumnFunc(nameof(FormData.ComName), "名稱"));
            foreach (var headerText in data.ROEHeaders)
                gv.Columns.Add(addPercentColumnFunc(headerText, headerText));

            gv.DataSource = this.DataSource;
        }

        private void FrmYearInfo_Load(object sender, EventArgs e)
        {
        }

        internal bool AddData(DisplayModel data)
        {
            if (ViewData.Any(d => d.ComCode == data.ComCode))
                return false;

            var rdata = ROEData.FirstOrDefault(d => d.ComCode == data.ComCode);
            if (rdata == null)
                return false;

            ViewData.Add(data);
            var fdata = new FormData(data, rdata);
            AddGridData(fdata);
            return true;
        }

        private void AddGridData(FormData fdata)
        {
            var gv = dataGridView1;
            this.DataSource.Add(fdata);
        }

        private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            var gv = (DataGridView)sender;
            var grow = gv.Rows[e.RowIndex];
            var col = gv.Columns[e.ColumnIndex];
            var data = (FormData)grow.DataBoundItem;
            var v = data[col.DataPropertyName];
            e.Value = v;
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            var gv = (DataGridView)sender;
            foreach (DataGridViewRow grow in gv.Rows)
                RefreshCellStyle(grow);
        }

        private void RefreshCellStyle(DataGridViewRow grow)
        {
            var data = (FormData)grow.DataBoundItem;

            var greenForeColor = Color.Green;
            var redForeColor = Color.Red;

            decimal? prevValue = null;
            foreach (DataGridViewColumn col in grow.DataGridView.Columns)
            {
                switch (col.DataPropertyName)
                {
                    case nameof(FormData.ComName):
                        continue;
                }
                var cell = grow.Cells[col.Index];
                var v = (decimal?)cell.Value;
                if (!v.HasValue) continue;

                var isGrow = prevValue.HasValue && v.Value > prevValue.Value;

                if (v.Value > 0 && isGrow)
                    cell.Style.ForeColor = redForeColor;
                else if (v.Value < 0)
                    cell.Style.ForeColor = greenForeColor;

                prevValue = v;
            }
        }

        private class FormData
        {
            private DisplayModel Model;
            private CompanyROE ROEData;

            public string ComName => $"{Model.ComCode} - {Model.ComName}";
            public decimal? this[int index] => ROEData.ROEValues[index];
            public decimal? this[string key] => ROEData.ROEValues[Index(key)];

            public FormData(DisplayModel data, CompanyROE rdata)
            {
                this.Model = data;
                this.ROEData = rdata;
            }
            private int Index(string key)
            {
                return ROEData.ROEHeaders.FindIndex(h => h == key);
            }
        }
    }
}
