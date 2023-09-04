using StockApp.Trade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
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

        [DisplayName("市/櫃")]
        public string ComType => this.DayVolumeSource?.ComType;
        internal bool IsTwSE => this.ComType == "市";
        [DisplayName("代號")]
        public string ComCode { get; private set; }
        [DisplayName("名稱")]
        public string ComName { get; private set; }

        private CompanyDayVolume DayVolumeSource;

        [DisplayName("成交")]
        public decimal CurrentPrice => this.DayVolumeSource?.CurrentPrice ?? 0;

        private CompanyAvgBonus AvgBonusSource;

        [DisplayName("殖利率(%)")]

        public decimal AvgYield => this.AvgBonusSource?.AvgYield ?? 0;

        private CompanyContBonus ContinueBounsSource;
        private CompanyExDividend ExDividendSource;
        private CompanyKDJ KDJSource;

        [DisplayName("連續次數")]
        public int ContBonusTimes => this.ContinueBounsSource?.ContBonusTimes ?? 0;
        [DisplayName("平均股利")]
        public decimal AvgBonus => this.ContinueBounsSource?.AvgBonus ?? 0;

        [DisplayName("期望(5%)")]

        public decimal Expect5 => Math.Floor(this.AvgBonus / 0.05m * 100) / 100;
        [DisplayName("期望(7%)")]
        public decimal Expect7 => Math.Floor(this.AvgBonus / 0.07m * 100) / 100;
        [DisplayName("期望(9%)")]
        public decimal Expect9 => Math.Floor(this.AvgBonus / 0.09m * 100) / 100;
        [DisplayName("成交量")]
        public int LastDayVolume => this.DayVolumeSource?.DayVolume ?? 0;

        [DisplayName("除息T")]
        public int? ExDividendDateT
        {
            get
            {
                int? result = null;
                var data = this.ExDividendSource;
                if (data != null && data.ExDividendDate.HasValue)
                {
                    var t = (int)(data.ExDividendDate.Value - DateTime.Today).TotalDays;
                    //已除息，股息無效
                    if (t >= 0)
                        return t;
                }
                return result;
            }
        }

        [DisplayName("股利")]
        public decimal? ExDividendBonus => this.ExDividendSource?.ExDividendBonus;
        [DisplayName("庫存")]
        public int? HoldStock
        {
            get
            {
                var v = this.Trades.Sum(t => t.TradeVolume);
                if (v == 0)
                    return null;
                return v;
            }
        }
        [DisplayName("成本")]
        public decimal? HoldValue
        {
            get
            {
                var totalVolume = this.HoldStock;
                if (!totalVolume.HasValue || totalVolume.Value == 0)
                    return null;
                var totalValue = this.Trades.Sum(t => t.TradeVolume * t.TradePrice);
                var avg = totalValue / totalVolume.Value;
                return Math.Floor(avg * 100) / 100;
            }
        }
        [DisplayName("追蹤")]
        public decimal? TraceValue => this.TraceData?.Value;
        [DisplayName("追蹤T")]
        public int? TraceDateT => this.TraceData?.LimitDateT;

        internal Trace.StockDetail TraceData { get; private set; }

        protected decimal ValueK
        {
            get
            {
                var d = this.KDJSource;
                var range = BasicSetting.Instance.KDJRange;
                var k = d.MonthK;
                switch (range)
                {
                    case KDJRangeType.Week:
                        k = d.WeekK;
                        break;
                    case KDJRangeType.Day:
                        k = d.DayK;
                        break;
                }
                return k ?? d.DayK;
            }
        }
        protected decimal ValueJ
        {
            get
            {
                var d = this.KDJSource;
                var range = BasicSetting.Instance.KDJRange;
                var j = d.MonthJ;
                switch (range)
                {
                    case KDJRangeType.Week:
                        j = d.WeekJ;
                        break;
                    case KDJRangeType.Day:
                        j = d.DayJ;
                        break;
                }
                return j ?? d.DayJ;
            }
        }
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

        public List<Trade.TradeInfo> Trades { get; private set; } = new List<Trade.TradeInfo>();
        internal bool IsETF => this.ComCode.StartsWith("0");

        internal void SetExtra(CompanyContBonus b)
        {
            this.ContinueBounsSource = b;
        }
        internal void SetExtra(CompanyExDividend d)
        {
            this.ExDividendSource = d;
        }
        internal void SetExtra(CompanyDayVolume v)
        {
            this.DayVolumeSource = v;
        }

        internal void SetExtra(CompanyAvgBonus d)
        {
            this.AvgBonusSource = d;
        }

        internal void SetExtra(CompanyKDJ d)
        {
            this.KDJSource = d;
        }

        internal void SetExtra(Trace.StockDetail d)
        {
            this.TraceData = d;
        }

        internal void ResetTrades(IEnumerable<TradeInfo> trades)
        {
            this.Trades.Clear();
            this.Trades.AddRange(trades);
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
