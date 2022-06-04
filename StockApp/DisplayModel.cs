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

        [DisplayName("代號")]
        public string ComCode { get; private set; }
        [DisplayName("名稱")]
        public string ComName { get; private set; }
        [DisplayName("平均股利")]
        public decimal AvgBonus { get; set; }
        [DisplayName("成交")]
        public decimal CurrentPrice { get; set; }
        [DisplayName("殖利率")]
        public decimal CurrentYield => Math.Round(this.AvgBonus / this.CurrentPrice * 100, 1);
        [DisplayName("平均殖利率")]

        public decimal AvgYield { get; private set; }
        [DisplayName("期望股價(5%)")]

        public decimal Expect5 => Math.Floor(this.AvgBonus / 0.05m * 100) / 100;
        [DisplayName("期望股價(7%)")]
        public decimal Expect7 => Math.Floor(this.AvgBonus / 0.07m * 100) / 100;
        [DisplayName("期望股價(9%)")]
        public decimal Expect9 => Math.Floor(this.AvgBonus / 0.09m * 100) / 100;
        [DisplayName("期望股價(7%)比")]

        public decimal Expect7Ratio => Math.Round(this.CurrentPrice / this.Expect7, 2);

        internal class Expect7DiffComparer : IComparer<DisplayModel>
        {
            public int Compare(DisplayModel x, DisplayModel y)
            {
                return -x.AvgYield.CompareTo(y.AvgYield);
            }
        }
    }
}
