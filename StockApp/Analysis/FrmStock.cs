using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockApp.Analysis
{
    public partial class FrmStock : Form
    {
        private readonly DisplayModel StockData;
        private readonly DividendHistory LastDividend;

        public FrmStock()
        {
            InitializeComponent();

            InitializeNumComponent();
        }

        private void InitializeNumComponent()
        {
            numLastYearQuarter.ValueChanged += NumLastYearQuarter_ValueChanged;
            numPreviousYearQuarter.ValueChanged += NumLastYearQuarter_ValueChanged;
            numLastDividend.ValueChanged += NumLastYearQuarter_ValueChanged;
            numExceptDividend.ValueChanged += NumExceptDividend_ValueChanged;
            numCurrentPrice.ValueChanged += NumExceptDividend_ValueChanged;
        }

        private void NumExceptDividend_ValueChanged(object sender, EventArgs e) => RefreshCurrentDividendYield();

        private void NumLastYearQuarter_ValueChanged(object sender, EventArgs e) => RefreshExceptDividend();

        internal FrmStock(DisplayModel data) : this()
        {
            this.StockData = data;

            var loading = new FrmLoading();

            var taskYearQuarters = loading.AddTask("Quarter EPS", () =>
            {
                var quarts = QuarterEPS.Get(data.ComCode);
                return quarts;
            });

            var taskDividendHistory = loading.AddTask("Dividend History", () =>
            {
                var histories = DividendHistory.Get(data.ComCode);
                return histories;
            });

            if (!loading.Start())
                loading.ShowDialog(this);

            #region Quarter EPS
            var quarters = taskYearQuarters.Result;
            var lastYearQuarter = quarters.First();

            var year = int.Parse(lastYearQuarter.YearQuarter.Substring(0, 4));
            var quarter = lastYearQuarter.YearQuarter.Substring(4);
            var searchPreviousYearQuarter = $"{year - 1:0000}{quarter}";

            var previousYearQuarter = quarters.First(q => q.YearQuarter == searchPreviousYearQuarter);

            lblLastYearQuarterTip.Text = lastYearQuarter.YearQuarter;
            lblPreviousYearQuarterTip.Text = previousYearQuarter.YearQuarter;

            numLastYearQuarter.Value = lastYearQuarter.EPS;
            numPreviousYearQuarter.Value = previousYearQuarter.EPS;
            #endregion

            #region Dividend History
            var dividendHistories = taskDividendHistory.Result;
            var lastDividend = dividendHistories.First(d => d.TotalDividend.HasValue);
            numLastDividend.Value = lastDividend.TotalDividend.Value;
            lblLastDividendTip.Text = lastDividend.Year.ToString();

            this.LastDividend = lastDividend;
            #endregion
        }

        private void FrmStock_Load(object sender, EventArgs e)
        {
            numCurrentPrice.Value = this.StockData.CurrentPrice;

            RefreshExceptDividend();

            RefreshCurrentDividendYield();
        }

        private void btnApplyLastDivdend_Click(object sender, EventArgs e) => numExceptDividend.Value = numLastDividend.Value;

        private void RefreshExceptDividend()
        {
            numExceptDividend.Value = 0;
            numLastYearQuarter.BackColor = default(Color);
            numLastYearQuarter.ForeColor = NumericUpDown.DefaultForeColor;

            var lastEPS = numLastYearQuarter.Value;
            var previousEPS = numPreviousYearQuarter.Value;
            var lastDividend = numLastDividend.Value;

            if (lastEPS > previousEPS)
            {
                numLastYearQuarter.BackColor = Color.Red;
                numLastYearQuarter.ForeColor = Color.White;
            }
            else if (lastEPS < previousEPS)
            {
                numLastYearQuarter.BackColor = Color.Green;
                numLastYearQuarter.ForeColor = Color.White;
            }

            var epsRatio = 1m;
            if (previousEPS > 0)
                epsRatio = lastEPS / previousEPS;

            var lastDividendData = this.LastDividend;
            numExceptDividend.Value = lastDividend * epsRatio;
        }
        private void RefreshCurrentDividendYield()
        {
            lblCurrentDividendYield.Text = "NA";
            numExceptDividend.BackColor = default(Color);
            numExceptDividend.ForeColor = NumericUpDown.DefaultForeColor;

            var currentPrice = numCurrentPrice.Value;
            if (currentPrice == 0)
                return;

            var dividend = numExceptDividend.Value;
            var dividendYield = dividend / currentPrice;
            lblCurrentDividendYield.Text = $"{dividendYield:P2}";

            var lastDividend = numLastDividend.Value;
            if (dividend > lastDividend)
            {
                numExceptDividend.BackColor = Color.Red;
                numExceptDividend.ForeColor = Color.White;
            }
            else if (dividend < lastDividend)
            {
                numExceptDividend.BackColor = Color.Green;
                numExceptDividend.ForeColor = Color.White;
            }
        }
    }
}
