using AngleSharp;
using AngleSharp.Dom;
using Newtonsoft.Json;
using StockApp.Web;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class CompanyKDJ
    {
        public DateTime UpdateAt { get; set; } = DateTime.Today;
        [JsonProperty]
        public string ComCode { get; private set; }
        [JsonProperty]
        public string ComName { get; private set; }
        [JsonProperty]
        public decimal DayK { get; private set; }
        [JsonProperty]
        public decimal DayD { get; private set; }
        [JsonProperty]
        public decimal DayJ { get; private set; }
        [JsonProperty]
        public decimal? WeekK { get; private set; }
        [JsonProperty]
        public decimal? WeekD { get; private set; }
        [JsonProperty]
        public decimal? WeekJ { get; private set; }
        [JsonProperty]
        public decimal? MonthK { get; private set; }
        [JsonProperty]
        public decimal? MonthD { get; private set; }
        [JsonProperty]
        public decimal? MonthJ { get; private set; }

        public static List<CompanyKDJ> GetAll()
        {
            //offset 1330
            var today = Utility.TWSEDate.Today;

            var urls = new string[] {
                "https://goodinfo.tw/tw2/StockList.asp?MARKET_CAT=%E4%B8%8A%E5%B8%82&INDUSTRY_CAT=%E4%B8%8A%E5%B8%82%E5%85%A8%E9%83%A8&SHEET=KD%E6%8C%87%E6%A8%99&SHEET2=%E6%97%A5%2F%E9%80%B1%2F%E6%9C%88",
                "https://goodinfo.tw/tw2/StockList.asp?MARKET_CAT=%E4%B8%8A%E6%AB%83&INDUSTRY_CAT=%E4%B8%8A%E6%AB%83%E5%85%A8%E9%83%A8&SHEET=KD%E6%8C%87%E6%A8%99&SHEET2=%E6%97%A5%2F%E9%80%B1%2F%E6%9C%88",
            };

            var result = new List<CompanyKDJ>();

            var bags = new ConcurrentBag<List<CompanyKDJ>>();
            var r = Parallel.ForEach(urls, url =>
            {
                bags.Add(GetAllByUrl(url));
            });

            foreach (var res in bags)
                result.AddRange(res);

            foreach (var res in result)
            {
                res.UpdateAt = today;
            }

            return result;
        }
        private static List<CompanyKDJ> GetAllByUrl(string url)
        {
            string content;
            using (var page = ChromiumBrowser.NewPage(url))
            {
                page.WaitForSelectorAsync("#tblStockList").Wait();
                content = page.GetContentAsync().Result;
            }

            IDocument doc = BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                .OpenAsync(req => req.Content(content)).Result;

            var trs = doc.QuerySelectorAll("#tblStockList>tbody>tr:not(.bg_h2)");

            var list = new List<CompanyKDJ>();
            foreach (var tr in trs)
            {
                var tds = tr.QuerySelectorAll("td");
                if (tds.Length == 0) continue;
                var data = new CompanyKDJ
                {
                    ComCode = tds[0].Text(),
                    ComName = tds[1].Text()
                };
                const int colIndexK = 8;
                var dayK = tds[colIndexK].Text();
                if (string.IsNullOrEmpty(dayK))
                    continue;
                data.DayK = Parse(dayK).Value;
                data.DayD = Parse(tds[colIndexK + 1].Text()).Value;
                data.DayJ = Parse(tds[colIndexK + 2].Text()).Value;

                list.Add(data);
            }
            return list;
        }

        private static decimal? Parse(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;
            var formattedText = text
                .Replace("↗", "")
                .Replace("→", "")
                .Replace("↘", "");
            return decimal.Parse(formattedText);
        }
    }
}
