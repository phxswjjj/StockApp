using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockApp.Analysis
{
    public partial class FrmStock : Form
    {
        private DisplayModel StockData;

        public FrmStock()
        {
            InitializeComponent();
        }

        internal FrmStock(DisplayModel data)
        {
            this.StockData = data;

            InitializeComponent();

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
            #endregion
        }

        private void FrmStock_Load(object sender, EventArgs e)
        {
            RefreshExceptDividend();

            numCurrentPrice.Value = this.StockData.CurrentPrice;

            RefreshCurrentDividendYield();
        }

        private void RefreshExceptDividend()
        {
            numExceptDividend.Value = 0;

            var lastEPS = numLastYearQuarter.Value;
            var previousEPS = numPreviousYearQuarter.Value;
            var lastDividend = numLastDividend.Value;

            if (lastEPS <= 0 || previousEPS <= 0)
                return;

            numExceptDividend.Value = lastDividend * lastEPS / previousEPS;
        }
        private void RefreshCurrentDividendYield()
        {
            lblCurrentDividendYield.Text = "NA";

            var currentPrice = numCurrentPrice.Value;
            if (currentPrice == 0)
                return;

            var dividend = numExceptDividend.Value;
            var dividendYield = dividend / currentPrice;
            lblCurrentDividendYield.Text = $"{dividendYield:P2}";
        }
    }
}
