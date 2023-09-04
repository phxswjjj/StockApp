using LiteDB;
using StockApp.Data;
using StockApp.Group;
using StockApp.ROE;
using StockApp.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Unity;

namespace StockApp.ROE
{
    internal partial class FrmYearROE : Form
    {
        private BindingList<FormData> DataSource = new BindingList<FormData>();
        private List<string> Headers;

        public FrmYearROE()
        {
            InitializeComponent();

            CompanyROE data;
            var container = UnityHelper.Create();
            using (ILiteDatabase db = LocalDb.Create())
            {
                container.RegisterInstance(db);
                var roeRepo = container.Resolve<ROERepository>();

                data = roeRepo.GetROELatest();
            }

            if (data == null)
                throw new Exception("ROE data Not Found");

            InitGrid(dataGridView1, data);

            chart1.Series.Clear();
        }
        private void InitGrid(DataGridView gv, CompanyROE data)
        {
            gv.ReadOnly = true;
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
            var headers = data.ROEHeaders;
            foreach (var headerText in headers)
                gv.Columns.Add(addPercentColumnFunc(headerText, headerText));
            this.Headers = headers;

            gv.DataSource = this.DataSource;
        }

        private void FrmYearInfo_Load(object sender, EventArgs e)
        {
        }

        internal void AddData(List<DisplayModel> list)
        {
            var container = UnityHelper.Create();
            using (ILiteDatabase db = LocalDb.Create())
            {
                container.RegisterInstance(db);
                var roeRepo = container.Resolve<ROERepository>();

                foreach (var data in list)
                {
                    if (this.DataSource.Any(d => d.ComCode == data.ComCode))
                        continue;

                    var rdata = roeRepo.GetROE(data.ComCode);
                    var fdata = new FormData(data, rdata);
                    this.DataSource.Add(fdata);
                    AddSeries(fdata);
                }
            }
        }
        private void AddSeries(FormData fdata)
        {
            var chart = chart1;
            var area = chart.ChartAreas.First();
            var series = new Series(fdata.ComName);
            chart.Series.Add(series);

            series.ChartArea = area.Name;
            series.ChartType = SeriesChartType.Spline;
            series.BorderWidth = 2;
            series.IsXValueIndexed = true;

            var headers = this.Headers;
            var list = headers.Select((h, i) => new { TimeLabel = h, YValue = fdata[i] });
            series.Points.DataBind(list,
                "TimeLabel", "YValue", null);
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

            public string ComCode => this.Model.ComCode;
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
