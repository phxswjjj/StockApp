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

            if (!loading.Start())
                loading.ShowDialog(this);

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
        }
    }
}
