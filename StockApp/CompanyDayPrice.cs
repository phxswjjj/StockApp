using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    abstract class CompanyDayPrice
    {
        protected static readonly TimeSpan DayCacheTimeSpan = new TimeSpan(12, 0, 0);

        public string ComCode { get; protected set; }
        public string ComName { get; protected set; }

        public List<DayPrice> DayPrices { get; set; } = new List<DayPrice>();

        private CompanyDayPrice() { }
        public CompanyDayPrice(string comCode)
        {
            ComCode = comCode;
        }

        internal static CompanyDayPrice New(DisplayModel data)
        {
            CompanyDayPrice dp;
            if (data.IsTwSE)
                dp = new ListedCompanyDayPrice(data.ComCode);
            else
                dp = new ExListedCompanyDayPrice(data.ComCode);
            dp.ComName = data.ComName;
            return dp;
        }

        protected string GetJsonPath(int year, int month)
        {
            return System.IO.Path.Combine("CompanyDayPrice", this.ComCode, $"{year:0000}{month:00}.json");
        }
        public virtual List<DayPrice> GetMonth(int year, int month)
        {
            var today = Utility.TWSEDate.Today;
            var ts = TimeSpan.MaxValue;
            if (today.Year == year && today.Month == month)
                ts = DayCacheTimeSpan;

            var jsonPath = GetJsonPath(year, month);
            var list = JsonCache.Load<List<DayPrice>>(jsonPath, ts);
            if (list != null)
            {
                lock (this.DayPrices)
                    this.DayPrices.AddRange(list);
            }
            return list;
        }
        public void Sort()
        {
            this.DayPrices.Sort(new DayPriceComparer());
        }
    }

    class DayPrice
    {
        [JsonProperty]
        public DateTime Date { get; set; }
        [JsonProperty]
        public decimal OpeningPrice { get; set; }
        [JsonProperty]
        public decimal ClosingPrice { get; set; }
        [JsonProperty]
        public decimal MaxPrice { get; set; }
        [JsonProperty]
        public decimal MinPrice { get; set; }
    }
    class DayPriceComparer : IComparer<DayPrice>
    {
        public int Compare(DayPrice x, DayPrice y)
        {
            return x.Date.CompareTo(y.Date);
        }
    }

    class ListedCompanyDayPrice : CompanyDayPrice
    {
        public ListedCompanyDayPrice(string comCode) : base(comCode)
        {
        }

        public override List<DayPrice> GetMonth(int year, int month)
        {
            var list = base.GetMonth(year, month);
            if (list != null)
                return list;
            else
                list = new List<DayPrice>();

            var url = $"https://www.twse.com.tw/exchangeReport/STOCK_DAY?date={year:0000}{month:00}01&stockNo={this.ComCode}";

            var request = Web.WebRequest.Create();
            var resp = request.GetAsync(url).Result;
            var content = resp.Content.ReadAsStringAsync().Result;
            var model = JsonConvert.DeserializeObject<DataModel>(content);
            if (model.data == null && model.stat != "OK")
                return list;
            var yearMonthPrefix = $"{year - 1911:000}/{month:00}";
            var yearMonthPrefixLen = yearMonthPrefix.Length;
            foreach (var data in model.data)
            {
                var dayOfMonth = int.Parse(data.First().Substring(yearMonthPrefixLen + 1, 2));
                var openingPrice = decimal.Parse(data[3]);
                var maxPrice = decimal.Parse(data[4]);
                var minPrice = decimal.Parse(data[5]);
                var closingPrice = decimal.Parse(data[6]);
                var dp = new DayPrice()
                {
                    Date = new DateTime(year, month, dayOfMonth),
                    OpeningPrice = openingPrice,
                    MaxPrice = maxPrice,
                    MinPrice = minPrice,
                    ClosingPrice = closingPrice,
                };
                list.Add(dp);
            }

            var jsonPath = GetJsonPath(year, month);
            JsonCache.Store(jsonPath, list);

            lock (this.DayPrices)
                this.DayPrices.AddRange(list);
            return list;
        }

        private class DataModel
        {
            public string stat;
            public List<List<string>> data;
        }
    }

    class ExListedCompanyDayPrice : CompanyDayPrice
    {
        public ExListedCompanyDayPrice(string comCode) : base(comCode)
        {
        }

        public override List<DayPrice> GetMonth(int year, int month)
        {
            var list = base.GetMonth(year, month);
            if (list != null)
                return list;
            else
                list = new List<DayPrice>();

            var url = $"https://www.tpex.org.tw/web/stock/aftertrading/daily_trading_info/st43_result.php?l=en-us&d={year:0000}/{month:00}&stkno={this.ComCode}";

            var request = Web.WebRequest.Create();
            var resp = request.GetAsync(url).Result;
            var content = resp.Content.ReadAsStringAsync().Result
                .Replace(@"\/", "/");
            var model = JsonConvert.DeserializeObject<DataModel>(content);
            var yearMonthPrefix = $"{year:0000}/{month:00}";
            var yearMonthPrefixLen = yearMonthPrefix.Length;
            foreach (var data in model.aaData)
            {
                var dayOfMonth = int.Parse(data.First().Substring(yearMonthPrefixLen + 1, 2));
                if (data[3] == "--")
                    continue;
                var openingPrice = decimal.Parse(data[3]);
                var maxPrice = decimal.Parse(data[4]);
                var minPrice = decimal.Parse(data[5]);
                var closingPrice = decimal.Parse(data[6]);
                var dp = new DayPrice()
                {
                    Date = new DateTime(year, month, dayOfMonth),
                    OpeningPrice = openingPrice,
                    MaxPrice = maxPrice,
                    MinPrice = minPrice,
                    ClosingPrice = closingPrice,
                };
                list.Add(dp);
            }

            var jsonPath = GetJsonPath(year, month);
            JsonCache.Store(jsonPath, list);

            lock (this.DayPrices)
                this.DayPrices.AddRange(list);
            return list;
        }

        private class DataModel
        {
            public List<List<string>> aaData;
        }
    }
}
