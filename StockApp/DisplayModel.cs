using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class DisplayModel
    {

        public DisplayModel(CompanyDayVolume d)
        {
            this.ComCode = d.ComCode;
            this.ComName = d.ComName;
            this.SetExtra(d);
        }

        [DisplayName("市")]
        public string ComType { get; private set; }
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
        [DisplayName("庫存")]
        public int? HoldStock { get; private set; }
        [DisplayName("成本")]
        public decimal? HoldValue { get; private set; }
        protected decimal ValueK { get; private set; }
        protected decimal ValueJ { get; private set; }
        [DisplayName("K")]
        public decimal ValueKDJ => this.ValueK;
        internal Color KDJColor
        {
            get
            {
                var c = Color.Transparent;
                if (this.ValueK > this.ValueJ && this.ValueK < 20m && this.ValueJ < 20m)
                    c = Color.FromArgb(0x2F72EF73);
                else if (this.ValueK < this.ValueJ && this.ValueK > 80m && this.ValueJ > 80m)
                    c = Color.FromArgb(0x2FDB7A82);
                return c;
            }
        }

        internal void SetExtra(CompanyContBonus b)
        {
            this.ContBonusTimes = b.ContBonusTimes;
        }
        internal void SetExtra(CompanyExDividend find)
        {
            if (find.ExDividendDate.HasValue)
                this.ExDividendDateT = (int)(find.ExDividendDate.Value - DateTime.Today).TotalDays;
            this.ExDividendBonus = find.ExDividendBonus;
        }
        internal void SetExtra(CompanyDayVolume find)
        {
            this.CurrentPrice = find.CurrentPrice;
            this.LastDayVolume = find.DayVolume;
            this.ComType = find.ComType;
        }
        internal void SetExtra(MemoContent data)
        {
            this.HoldStock = data.HoldStock;
            this.HoldValue = data.HoldValue;
        }

        internal void SetExtra(CompanyAvgBonus d)
        {
            this.AvgBonus = d.AvgBonus;
            this.CurrentPrice = d.CurrentPrice;
            this.AvgYield = d.AvgYield;
        }

        internal void SetExtra(CompanyKDJ find)
        {
            var range = (KDJRangeType)Properties.Settings.Default.KDJRange;
            var k = find.MonthK;
            var j = find.MonthJ;
            switch (range)
            {
                case KDJRangeType.Week:
                    k = find.WeekK;
                    j = find.WeekJ;
                    break;
                case KDJRangeType.Day:
                    k = find.DayK;
                    j = find.DayJ;
                    break;
            }
            this.ValueK = k ?? find.DayK;
            this.ValueJ = j ?? find.DayJ;
        }

        #region Comparer
        internal class Expect7DiffComparer : IComparer<DisplayModel>
        {
            public int Compare(DisplayModel x, DisplayModel y)
            {
                return -x.AvgYield.CompareTo(y.AvgYield);
            }
        }
        internal class ExDividendDateTComparer : IComparer<DisplayModel>
        {
            public int Compare(DisplayModel x, DisplayModel y)
            {
                if (!y.ExDividendDateT.HasValue)
                    return 1;
                else if (!x.ExDividendDateT.HasValue)
                    return 1;
                return x.ExDividendDateT.Value.CompareTo(y.ExDividendDateT.Value);
            }
        }
        #endregion

        public enum KDJRangeType
        {
            Day,
            Week,
            Month,
        }
    }
}
