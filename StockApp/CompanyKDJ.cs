using AngleSharp;
using AngleSharp.Dom;
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
    class CompanyKDJ
    {
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
            var offseted = Utility.TWSEDate.Today;
            var jsonFilePath = Path.Combine("CompanyKDJ", $"{offseted:yyyyMMdd}.json");
            var caches = JsonCache.Load<List<CompanyKDJ>>(jsonFilePath);
            if (caches != null)
                return caches;

            var urls = new string[] {
                "https://goodinfo.tw/tw/StockList.asp?MARKET_CAT=%E4%B8%8A%E5%B8%82&INDUSTRY_CAT=%E4%B8%8A%E5%B8%82%E5%85%A8%E9%83%A8&SHEET=KD%E6%8C%87%E6%A8%99&SHEET2=%E6%97%A5%2F%E9%80%B1%2F%E6%9C%88",
                "https://goodinfo.tw/tw/StockList.asp?MARKET_CAT=%E4%B8%8A%E6%AB%83&INDUSTRY_CAT=%E4%B8%8A%E6%AB%83%E5%85%A8%E9%83%A8&SHEET=KD%E6%8C%87%E6%A8%99&SHEET2=%E6%97%A5%2F%E9%80%B1%2F%E6%9C%88",
            };

            var result = new List<CompanyKDJ>();

            var bags = new ConcurrentBag<List<CompanyKDJ>>();
            var r = Parallel.ForEach(urls, url =>
            {
                bags.Add(GetAllByUrl(url));
            });

            foreach (var res in bags)
                result.AddRange(res);

            if (result.Count > 300)
                JsonCache.Store(jsonFilePath, result);
            return result;
        }
        private static List<CompanyKDJ> GetAllByUrl(string url)
        {
            var request = WebRequest.CreateGoodInfo();
            var resp = request.GetAsync(url).Result;
            var bytes = resp.Content.ReadAsByteArrayAsync().Result;
            var content = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            resp.EnsureSuccessStatusCode();

            IDocument doc = BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                .OpenAsync(req => req.Content(content)).Result;

            var trs = doc.QuerySelectorAll("#tblStockList>tbody>tr:not(.bg_h2)");

            var list = new List<CompanyKDJ>();
            foreach (var tr in trs)
            {
                var tds = tr.QuerySelectorAll("td");
                if (tds.Length == 0) continue;
                var data = new CompanyKDJ();
                data.ComCode = tds[0].Text();
                data.ComName = tds[1].Text();
                var dayK = tds[7].Text();
                if (string.IsNullOrEmpty(dayK))
                    continue;
                data.DayK = Parse(dayK).Value;
                data.DayD = Parse(tds[8].Text()).Value;
                data.DayJ = Parse(tds[9].Text()).Value;
                data.WeekK = Parse(tds[11].Text());
                data.WeekD = Parse(tds[12].Text());
                data.WeekJ = Parse(tds[13].Text());
                data.MonthK = Parse(tds[15].Text());
                data.MonthD = Parse(tds[16].Text());
                data.MonthJ = Parse(tds[17].Text());

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
                .Replace("↘", "");
            return decimal.Parse(formattedText);
        }
    }
}
