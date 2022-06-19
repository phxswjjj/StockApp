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
        internal CompanyDayPrice DayPrices { get; set; }
        internal CompanyBonusHistory BonusHistories { get; set; }
        public ToolTip CursorYTip { get; }
        public ToolTip CursorXTip { get; }

        internal FrmSimulator(CompanyDayPrice dp, CompanyBonusHistory bh)
        {
            this.DayPrices = dp;
            this.BonusHistories = bh;

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
            var dp = this.DayPrices;

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

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            var selectedDate = dateTimePicker1.Value;
            var dp = this.DayPrices.DayPrices.FirstOrDefault(d => d.Date == selectedDate);
            if (dp != null)
                numStartPrice.Value = dp.ClosingPrice;
        }

        private void numStartVolume_Enter(object sender, EventArgs e)
        {
            var ctrl = (NumericUpDown)sender;
            ctrl.Select(0, ctrl.Text.Length);
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

            var dayPrices = this.DayPrices.DayPrices;
            var bonusHistories = this.BonusHistories.DayBonusHistories.Where(b => cbxExDividend.Checked);

            var simulator = Simulator.Create(dayPrices, bonusHistories.ToList());

            var startPriceDate = dayPrices.FirstOrDefault(d => d.Date >= startDate);
            if (startPriceDate == null)
            {
                MessageBox.Show($"找不到 {startDate: yy/MM/dd} 之後的成交記錄");
                return;
            }
            var startPrice = numStartPrice.Value;
            simulator.BuyFirst(startDate, startVolume, startPrice);

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
            if (!LoadSimulateResult(seriesName, simulateResults))
                MessageBox.Show($"{seriesName} 重複");
        }

        private bool LoadSimulateResult(string seriesName, IEnumerable<SimulateDayPrice> simulateResults)
        {
            var chart = chart1;
            var area = chart.ChartAreas.First();

            if (chart.Series.Any(s => s.Name == seriesName))
                return false;

            var series = new Series(seriesName);
            chart.Series.Add(series);

            series.ChartArea = area.Name;
            series.ChartType = SeriesChartType.Point;
            series.MarkerStyle = MarkerStyle.Cross;
            series.MarkerSize = 20;
            series.IsXValueIndexed = true;

            series.Points.DataBind(simulateResults,
                nameof(SimulateDayPrice.Date), nameof(SimulateDayPrice.Price), null);
            return true;
        }

        private class Simulator
        {
            private List<SimulateDayPrice> Source;

            public List<DayBonus> BonusHistories { get; }

            private int ItemIndex;
            private readonly int ItemCount;
            private static int PricePerVolume = 10;

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

            public Simulator(IEnumerable<DayPrice> dayPrices, List<DayBonus> dayBonusHistories)
            {
                this.Source = dayPrices
                    .Select(d => new SimulateDayPrice(d))
                    .ToList();
                this.BonusHistories = dayBonusHistories;
                this.ItemIndex = -1;
                this.ItemCount = this.Source.Count;
            }

            internal static Simulator Create(IEnumerable<DayPrice> dayPrices, List<DayBonus> dayBonusHistories)
            {
                var simulator = new Simulator(dayPrices, dayBonusHistories);
                return simulator;
            }

            internal void BuyFirst(DateTime startDate, int startVolume, decimal startPrice)
            {
                var dataFound = false;
                while (this.MoveNext())
                {
                    if (this.Current.Date >= startDate)
                    {
                        dataFound = true;
                        break;
                    }
                }
                if (!dataFound)
                    throw new Exception("no data found");
                var price = this.Current.BuyFirst(startVolume, startPrice);

                this.TotalVolume = startVolume;
                this.TotalValue = startVolume * price;
            }

            private void ExDividend()
            {
                var date = this.Current.Date;
                var list = this.BonusHistories
                    .Where(b => b.ExDate == date);

                foreach (var bonus in list)
                {
                    var buys = this.Source.Where(s => s.Date < date);
                    switch (bonus.ExType)
                    {
                        case ExDividendType.Money:
                            //降低成本
                            var exBonus = bonus.ExBonus * buys.Sum(b => b.Volume ?? 0);
                            this.TotalValue -= exBonus;
                            Console.WriteLine($"{date}, bonus: {exBonus:#,##0}, result: {TotalValue:#,##0}");
                            break;
                        case ExDividendType.Volume:
                            var exVolume = (int)(bonus.ExBonus / PricePerVolume * buys.Sum(b => b.Volume ?? 0));
                            this.TotalVolume += exVolume;
                            Console.WriteLine($"{date}, volume: {exVolume:#,##0}, result: {TotalVolume:#,##0}");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            internal bool MoveNext()
            {
                this.ItemIndex++;

                if (this.ItemIndex < this.ItemCount)
                {
                    //當日不計除息
                    this.ExDividend();
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

            internal decimal BuyFirst(int startVolume, decimal startPrice)
            {
                this.Volume = startVolume;
                this.Price = startPrice;
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
