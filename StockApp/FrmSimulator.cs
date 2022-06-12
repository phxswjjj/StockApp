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

namespace StockApp
{
    public partial class FrmSimulator : Form
    {
        internal CompanyDayPrice RefData { get; set; }
        public ToolTip MouseTip { get; }

        public FrmSimulator()
        {
            InitializeComponent();

            var tip = new ToolTip();
            tip.ShowAlways = true;
            this.MouseTip = tip;
        }

        private void FrmSimulator_Load(object sender, EventArgs e)
        {
            var dp = this.RefData;

            this.Text = $"{dp.ComCode} - {dp.ComName}";

            var area = chart1.ChartAreas.First();
            //不顯示格線
            area.AxisX.MajorGrid.LineWidth = 0;
            area.AxisY.MajorGrid.LineWidth = 0;
            //不從 0 開始
            area.AxisY.IsStartedFromZero = false;
            //允許 X zoom in
            area.CursorX.IsUserSelectionEnabled = true;
            //設定最小單位才能精準定位
            area.CursorY.Interval = 0.01d;

            var series = chart1.Series.First();
            series.XValueMember = nameof(DayPrice.Date);
            series.XValueType = ChartValueType.Date;
            series.IsXValueIndexed = true;

            //依序：Max/Min/Open/Close
            var yValueMembers = new string[] {
                nameof(DayPrice.MaxPrice),
                nameof(DayPrice.MinPrice),
                nameof(DayPrice.OpeningPrice),
                nameof(DayPrice.ClosingPrice),
            };
            series.YValueMembers = string.Join(",", yValueMembers);
            series.SetCustomProperty("PriceDownColor", "Green");
            series.SetCustomProperty("PriceUpColor", "Red");
            series["ShowOpenClose"] = "Both";
            chart1.DataManipulator.IsStartFromFirst = true;
            chart1.DataSource = dp.DayPrices;
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var result = chart.HitTest(e.X, e.Y, false);

            switch (result.ChartElementType)
            {
                case ChartElementType.Gridlines:
                case ChartElementType.PlottingArea:
                case ChartElementType.DataPoint:
                    var area = chart.ChartAreas.First();
                    var series = chart.Series.First();

                    //X index offset
                    var valX = area.AxisX.PixelPositionToValue(e.X) - 1;

                    var dp = series.Points
                        .Select((d, i) => new
                        {
                            delta = Math.Abs(i - valX),
                            index = i,
                            YValue = d.YValues[1],
                            YValues = d.YValues,
                        })
                        .OrderBy(d => d.delta)
                        .First();
                    var indexX = dp.index + 1;

                    area.CursorX.SetCursorPosition(indexX);
                    area.CursorY.SetCursorPosition(dp.YValue);

                    var tip = this.MouseTip;
                    tip.Show($"Open: {dp.YValues[2]}, High: {dp.YValues[0]}, Low: {dp.YValues[1]}, Close: {dp.YValues[3]}",
                        chart, new Point(e.X, e.Y - 20));
                    break;
            }
        }
    }
}
