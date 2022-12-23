using AngleSharp.Dom;
using AngleSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using StockApp.Web;
using System.IO;
using Newtonsoft.Json;

namespace StockApp.Analysis
{
    internal class DividendHistory
    {
        static readonly TimeSpan CacheTimeSpan = new TimeSpan(7, 0, 0, 0);

        [JsonProperty]
        public int Year { get; private set; }
        [JsonProperty]
        public decimal? TotalDividend { get; private set; }

        public static List<DividendHistory> Get(string stockCode)
        {
            var jsonFilePath = Path.Combine("Analysis", $"{stockCode}-DividendHistory.json");
            var cache = JsonCache.Load<List<DividendHistory>>(jsonFilePath, CacheTimeSpan);
            if (cache != null)
                return cache;

            var url = $"https://goodinfo.tw/tw/StockDividendSchedule.asp?STOCK_ID={stockCode}";
            string content;
            using (var page = ChromiumBrowser.NewPage(url))
            {
                page.WaitForSelectorAsync("#tblDetail").Wait();
                content = page.GetContentAsync().Result;
            }

            IDocument doc = BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                .OpenAsync(req => req.Content(content)).Result;

            var trs = doc.QuerySelectorAll("#tblDetail tbody>tr:not(.bg_h2)");
            var dividends = new List<DividendHistory>();
            foreach (var tr in trs)
            {
                var tds = tr.QuerySelectorAll("td");
                if (tds.Count() == 0)
                    continue;
                var dividend = new DividendHistory()
                {
                    Year = int.Parse(tds.First().TextContent),
                };
                var s = tds.Last().TextContent;
                if (!string.IsNullOrEmpty(s))
                    dividend.TotalDividend = decimal.Parse(s);
                dividends.Add(dividend);
            }
            if (dividends.Count > 0)
                JsonCache.Store(jsonFilePath, dividends);
            return dividends;
        }
    }
}
