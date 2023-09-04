using AngleSharp;
using AngleSharp.Dom;
using LiteDB;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    /// <summary>
    /// 連續股息
    /// </summary>
    class CompanyContBonus
    {
        const string RefererUrl = "https://goodinfo.tw/tw/StockList.asp?SEARCH_WORD=&SHEET=%E8%82%A1%E5%88%A9%E6%94%BF%E7%AD%96%E7%99%BC%E6%94%BE%E5%B9%B4%E5%BA%A6&SHEET2=%E9%80%A3%E7%BA%8C%E9%85%8D%E7%99%BC%E8%82%A1%E5%88%A9%E7%B5%B1%E8%A8%88&MARKET_CAT=%E7%86%B1%E9%96%80%E6%8E%92%E8%A1%8C&INDUSTRY_CAT=%E7%9B%88%E9%A4%98%E7%B8%BD%E5%88%86%E9%85%8D%E7%8E%87&STOCK_CODE=&RPT_TIME=%E6%9C%80%E6%96%B0%E8%B3%87%E6%96%99";
        const string QueryBaseUrl = "https://goodinfo.tw/tw2/StockList.asp?SEARCH_WORD=&SHEET=%E8%82%A1%E5%88%A9%E6%94%BF%E7%AD%96%E7%99%BC%E6%94%BE%E5%B9%B4%E5%BA%A6&SHEET2=%E9%80%A3%E7%BA%8C%E9%85%8D%E7%99%BC%E8%82%A1%E5%88%A9%E7%B5%B1%E8%A8%88&MARKET_CAT=%E7%86%B1%E9%96%80%E6%8E%92%E8%A1%8C&INDUSTRY_CAT=%E7%9B%88%E9%A4%98%E7%B8%BD%E5%88%86%E9%85%8D%E7%8E%87&STOCK_CODE=&RPT_TIME=%E6%9C%80%E6%96%B0%E8%B3%87%E6%96%99&STEP=DATA&RANK=99999";

        public DateTime UpdateAt { get; set; }
        [JsonProperty]
        public string ComCode { get; private set; }
        [JsonProperty]
        public string ComName { get; private set; }
        [JsonProperty]
        public int ContBonusTimes { get; private set; }
        [JsonProperty]
        public decimal AvgBonus { get; private set; }
        [JsonProperty]
        public decimal CurrentPrice { get; private set; }
        [BsonIgnore]
        public decimal CurrentYield => Math.Round(this.AvgBonus / this.CurrentPrice * 100, 1);

        [JsonProperty]
        public decimal AvgYield { get; private set; }

        [BsonIgnore]
        public decimal Expect5 => Math.Floor(this.AvgBonus / 0.05m * 100) / 100;
        [BsonIgnore]
        public decimal Expect7 => Math.Floor(this.AvgBonus / 0.07m * 100) / 100;
        [BsonIgnore]
        public decimal Expect9 => Math.Floor(this.AvgBonus / 0.09m * 100) / 100;

        public static List<CompanyContBonus> GetAll()
        {
            var request = Web.WebRequest.CreateGoodInfo();
            request.DefaultRequestHeaders.Referrer = new Uri(RefererUrl);
            var resp = request.PostAsync(QueryBaseUrl, null).Result;
            var bytes = resp.Content.ReadAsByteArrayAsync().Result;
            var content = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            IDocument doc = BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                .OpenAsync(req => req.Content(content)).Result;

            var trs = doc.QuerySelectorAll("#tblStockList>tbody>tr")
                .Where(tr => !string.IsNullOrEmpty(tr.Id) && tr.Id.StartsWith("row"));

            var offseted = Utility.TWSEDate.Today;
            var result = new List<CompanyContBonus>();
            foreach (var tr in trs)
            {
                var tds = tr.QuerySelectorAll("td");
                var data = new CompanyContBonus
                {
                    ComCode = tds[1].Text(),
                    ComName = tds[2].Text(),
                    CurrentPrice = decimal.Parse(tds[4].Text()),
                    UpdateAt = offseted.Date,
                };

                var sContBonusTimes = tds[17].Text();
                if (string.IsNullOrEmpty(sContBonusTimes))
                    continue;
                data.ContBonusTimes = int.Parse(sContBonusTimes);

                var sAvgBonus = tds[19].Text();
                if (string.IsNullOrEmpty(sAvgBonus))
                    continue;
                data.AvgBonus = decimal.Parse(sAvgBonus);
                if (data.AvgBonus <= 0)
                    continue;

                var sAvgYield = tds[20].Text();
                if (string.IsNullOrEmpty(sAvgYield))
                    continue;
                data.AvgYield = decimal.Parse(sAvgYield);

                result.Add(data);
            }

            return result;
        }
    }
}
