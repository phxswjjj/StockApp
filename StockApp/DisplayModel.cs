using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class DisplayModel
    {

        public DisplayModel(CompanyAvgBonus d)
        {
            this.ComCode = d.ComCode;
            this.ComName = d.ComName;
            this.AvgBonus = d.AvgBonus;
            this.CurrentPrice = d.CurrentPrice;
            this.AvgYield = d.AvgYield;
        }

        internal void SetExtra(CompanyContBonus b)
        {
            this.ContBonusTimes = b.ContBonusTimes;
        }

        [DisplayName("代號")]
        public string ComCode { get; private set; }
        [DisplayName("名稱")]
        public string ComName { get; private set; }
        [DisplayName("成交")]
        public decimal CurrentPrice { get; set; }
        [DisplayName("平均股利")]
        public decimal AvgBonus { get; set; }

        [DisplayName("殖利率(%)")]
        public decimal CurrentYield => Math.Round(this.AvgBonus / this.CurrentPrice * 100, 1);
        [DisplayName("連續次數")]
        public int ContBonusTimes { get; private set; }
        [DisplayName("平均殖利率")]

        public decimal AvgYield { get; private set; }
        [DisplayName("期望(5%)")]

        public decimal Expect5 => Math.Floor(this.AvgBonus / 0.05m * 100) / 100;
        [DisplayName("期望(7%)")]
        public decimal Expect7 => Math.Floor(this.AvgBonus / 0.07m * 100) / 100;
        [DisplayName("期望(9%)")]
        public decimal Expect9 => Math.Floor(this.AvgBonus / 0.09m * 100) / 100;
        [DisplayName("成交量")]
        public int LastDayVolume { get; private set; }
        [DisplayName("除息T")]
        public int? ExDividendDateT { get; private set; }
        [DisplayName("股利")]
        public decimal? ExDividendBonus { get; private set; }
        [DisplayName("庫存(張)")]
        public int? HoldStock { get; private set; }
        [DisplayName("成本")]
        public decimal? HoldValue { get; private set; }

        internal class Expect7DiffComparer : IComparer<DisplayModel>
        {
            public int Compare(DisplayModel x, DisplayModel y)
            {
                return -x.AvgYield.CompareTo(y.AvgYield);
            }
        }

        internal void SetExtra(CompanyExDividend find)
        {
            if (find.ExDividendDate.HasValue)
                this.ExDividendDateT = (int)(find.ExDividendDate.Value - DateTime.Today).TotalDays;
            this.ExDividendBonus = find.ExDividendBonus;
        }

        internal void SetExtra(CompanyDayVolume find)
        {
            this.LastDayVolume = find.DayVolume;
        }

        internal void SetExtra(MemoContent data)
        {
            this.HoldStock = data.HoldStock;
            this.HoldValue = data.HoldValue;
        }
    }
}
