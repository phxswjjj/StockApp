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
        public ToolTip CursorYTip { get; }
        public ToolTip CursorXTip { get; }

        public FrmSimulator()
        {
            InitializeComponent();

            var tipX = new ToolTip();
            tipX.ShowAlways = true;
            this.CursorXTip = tipX;

            var tipY = new ToolTip();
            tipY.ShowAlways = true;
            this.CursorYTip = tipY;
        }

        private void FrmSimulator_Load(object sender, EventArgs e)
        {
            var dp = this.RefData;

            this.Text = $"{dp.ComCode} - {dp.ComName}";

            #region Init Chart
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

            area.CursorX.LineColor = Color.CornflowerBlue;
            area.CursorY.LineColor = Color.CornflowerBlue;
            area.CursorX.LineDashStyle = ChartDashStyle.Dash;
            area.CursorY.LineDashStyle = ChartDashStyle.Dash;

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
            #endregion

            #region Setting
            dateTimePicker1.MinDate = dp.DayPrices.Select(d => d.Date).Min();
            dateTimePicker1.Value = dateTimePicker1.MinDate;
            #endregion
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
                            XValue = DateTime.FromOADate(d.XValue),
                            YValue = d.YValues[1],
                            YValues = d.YValues,
                        })
                        .OrderBy(d => d.delta)
                        .First();
                    var indexX = dp.index + 1;

                    area.CursorX.SetCursorPosition(indexX);
                    area.CursorY.SetCursorPosition(dp.YValue);

                    var posX = new
                    {
                        X = e.X,
                        Y = (int)area.AxisY.ValueToPixelPosition(area.AxisY.Minimum),
                    };
                    var posY = new
                    {
                        X = (int)area.AxisX.ValueToPixelPosition(0),
                        Y = (int)area.AxisY.ValueToPixelPosition(dp.YValue),
                    };

                    var tipX = this.CursorXTip;
                    tipX.Show(dp.XValue.ToString("M/d"), chart, new Point(posX.X, posX.Y));

                    var tipY = this.CursorYTip;
                    tipY.Show(dp.YValue.ToString(), chart, new Point(posY.X, posY.Y));
                    break;
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            var startDate = dateTimePicker1.Value;
            var startVolume = (int)numStartVolume.Value;
            var downRate = numDownRate.Value / 100;
            var buyVolume = 1000;

            var dayPrices = this.RefData.DayPrices;

            var simulator = Simulator.Create(dayPrices, startDate);

            simulator.BuyFirst(startVolume);

            while (simulator.MoveNext())
            {
                simulator.Buy(buyVolume, downRate);
            }

            var simulateResults = simulator.Result.ToList();

            var diffPrice = dayPrices.Last().ClosingPrice - simulator.AvgPrice;
            diffPrice = Math.Floor(diffPrice * 100) / 100;
            var diffTotalPrice = diffPrice * simulator.TotalVolume;
            var diffRate = diffTotalPrice / simulator.TotalValue;
            var seriesName = $"{downRate:P0} / {simulator.AvgPrice:0.##} / {diffRate:P2}";
            LoadSimulateResult(seriesName, simulateResults);
        }

        private void LoadSimulateResult(string seriesName, IEnumerable<SimulateDayPrice> simulateResults)
        {
            var chart = chart1;
            var area = chart.ChartAreas.First();
            var series = new Series(seriesName);
            chart.Series.Add(series);

            series.ChartArea = area.Name;
            series.ChartType = SeriesChartType.Point;
            series.MarkerStyle = MarkerStyle.Cross;
            series.MarkerSize = 20;
            series.IsXValueIndexed = true;

            series.Points.DataBind(simulateResults,
                nameof(SimulateDayPrice.Date), nameof(SimulateDayPrice.Price), null);
        }

        private class Simulator
        {
            private List<SimulateDayPrice> Source;
            private int ItemIndex;
            private readonly int ItemCount;

            private DateTime StartDate;

            public SimulateDayPrice Current => this.Source[this.ItemIndex];

            public decimal TotalValue { get; private set; }
            public int TotalVolume { get; private set; }
            public decimal AvgPrice => TotalValue / TotalVolume;

            public IEnumerable<SimulateDayPrice> Result
            {
                get
                {
                    return this.Source;
                }
            }

            public Simulator(IEnumerable<DayPrice> dayPrices, DateTime startDate)
            {
                this.Source = dayPrices
                    .Select(d => new SimulateDayPrice(d))
                    .ToList();
                this.ItemIndex = -1;
                this.ItemCount = this.Source.Count;
                this.StartDate = startDate;
            }

            internal static Simulator Create(IEnumerable<DayPrice> dayPrices, DateTime startDate)
            {
                var simulator = new Simulator(dayPrices, startDate);
                return simulator;
            }

            internal void BuyFirst(int startVolume)
            {
                var dataFound = false;
                while (this.MoveNext())
                {
                    if (this.Current.Date >= this.StartDate)
                    {
                        dataFound = true;
                        break;
                    }
                }
                if (!dataFound)
                    throw new Exception("no data found");
                var price = this.Current.BuyFirst(startVolume);

                this.TotalVolume = startVolume;
                this.TotalValue = startVolume * price;
            }

            internal bool MoveNext()
            {
                if (this.ItemIndex < this.ItemCount - 1)
                {
                    this.ItemIndex++;
                    return true;
                }
                return false;
            }

            internal bool Buy(int buyVolume, decimal downRate)
            {
                var data = this.Current;

                var expectPrice = Math.Floor(this.AvgPrice * (1 - downRate) * 100) / 100;

                if (data.Buy(expectPrice, buyVolume))
                {
                    this.TotalVolume += buyVolume;
                    this.TotalValue += buyVolume * expectPrice;
                    return true;
                }

                return false;
            }
        }
        private class SimulateDayPrice
        {
            private DayPrice Source;

            public DateTime Date => Source.Date;
            public int? Volume { get; private set; }
            public decimal? Price { get; private set; }

            public SimulateDayPrice(DayPrice source)
            {
                this.Source = source;
            }

            internal decimal BuyFirst(int startVolume)
            {
                this.Volume = startVolume;
                this.Price = Source.OpeningPrice;
                return this.Price.Value;
            }

            internal bool Buy(decimal expectPrice, int buyVolume)
            {
                if (expectPrice >= this.Source.MinPrice)
                {
                    this.Volume = buyVolume;
                    this.Price = expectPrice;
                    return true;
                }
                return false;
            }
        }
    }
}
