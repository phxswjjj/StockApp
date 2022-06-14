using AngleSharp;
using AngleSharp.Dom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class CompanyBonusHistory
    {
        protected static readonly TimeSpan DayCacheTimeSpan = new TimeSpan(12, 0, 0);

        const string BaseUrl = "https://goodinfo.tw/tw/StockDividendSchedule.asp?STOCK_ID=";

        public string ComCode { get; protected set; }
        public string ComName { get; protected set; }

        public List<DayBonus> DayBonusHistories { get; set; } = new List<DayBonus>();

        internal static CompanyBonusHistory New(DisplayModel data)
        {
            CompanyBonusHistory dp = new CompanyBonusHistory()
            {
                ComCode = data.ComCode,
                ComName = data.ComName,
            };
            return dp;
        }

        public List<DayBonus> GetAll()
        {
            var today = Utility.TWSEDate.Today;
            var ts = DayCacheTimeSpan;

            var jsonPath = Path.Combine("CompanyBonus", $"{this.ComCode}.json");
            var list = JsonCache.Load<List<DayBonus>>(jsonPath, ts);
            if (list == null)
            {
                list = new List<DayBonus>();

                var url = BaseUrl + this.ComCode;
                var request = WebRequest.Create();
                var resp = request.GetAsync(url).Result;
                var bytes = resp.Content.ReadAsByteArrayAsync().Result;
                var content = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                IDocument doc = BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                    .OpenAsync(req => req.Content(content)).Result;

                var trs = doc.QuerySelectorAll("#tblDetail>tbody>tr:not(.bg_h2)");

                foreach (var tr in trs)
                {
                    var tds = tr.QuerySelectorAll("td");
                    if (tds.Length == 0) continue;
                    DayBonus data;

                    if (TryParse(tds[3].Text(), tds[4].Text(),
                        tds[14].Text(), out data))
                    {
                        data.ExType = ExDividendType.Money;
                        list.Add(data);
                    }

                    if (TryParse(tds[8].Text(), tds[9].Text(),
                        tds[17].Text(), out data))
                    {
                        data.ExType = ExDividendType.Volume;
                        list.Add(data);
                    }
                }
            }
            this.DayBonusHistories = list;
            return list;
        }

        private bool TryParse(string sExDate, string sExPrice, string sExBonus, out DayBonus data)
        {
            data = new DayBonus();

            if (string.IsNullOrEmpty(sExDate) || string.IsNullOrEmpty(sExPrice)
                || string.IsNullOrEmpty(sExBonus))
                return false;

            sExDate = sExDate.Replace("'", "/");
            data = new DayBonus()
            {
                ExDate = DateTime.ParseExact(sExDate, "yy/MM/dd",
                    System.Globalization.CultureInfo.InvariantCulture),
                ExPrice = decimal.Parse(sExPrice),
                ExBonus = decimal.Parse(sExBonus),
            };

            return true;
        }

        public void Sort()
        {
            this.DayBonusHistories.Sort(new DayBonusComparer());
        }
    }
    class DayBonus
    {
        [JsonProperty]
        public ExDividendType ExType { get; internal set; }
        [JsonProperty]
        public DateTime ExDate { get; internal set; }
        [JsonProperty]
        public decimal ExPrice { get; internal set; }
        [JsonProperty]
        public decimal ExBonus { get; internal set; }
    }
    class DayBonusComparer : IComparer<DayBonus>
    {
        public int Compare(DayBonus x, DayBonus y)
        {
            return x.ExDate.CompareTo(y.ExDate);
        }
    }
    enum ExDividendType
    {
        Money,
        Volume,
    }
}
