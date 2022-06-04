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
    class CompanyDayVolume
    {
        const string RefererUrl = "https://goodinfo.tw/tw/StockList.asp?SEARCH_WORD=&SHEET=交易狀況&SHEET2=日&MARKET_CAT=熱門排行&INDUSTRY_CAT=成交張數+(高→低)@@成交張數@@由高→低&STOCK_CODE=&RPT_TIME=最新資料&STEP=DATA&RANK=1";
        const string QueryBaseUrl = "https://goodinfo.tw/tw/StockList.asp?SEARCH_WORD=&SHEET=%E4%BA%A4%E6%98%93%E7%8B%80%E6%B3%81&SHEET2=%E9%80%B1&MARKET_CAT=%E7%86%B1%E9%96%80%E6%8E%92%E8%A1%8C&INDUSTRY_CAT=%E6%88%90%E4%BA%A4%E5%BC%B5%E6%95%B8+%28%E9%AB%98%E2%86%92%E4%BD%8E%29%40%40%E6%88%90%E4%BA%A4%E5%BC%B5%E6%95%B8%40%40%E7%94%B1%E9%AB%98%E2%86%92%E4%BD%8E&STOCK_CODE=&RPT_TIME=%E6%9C%80%E6%96%B0%E8%B3%87%E6%96%99&STEP=DATA&RANK=";

        [JsonProperty]
        public string ComCode { get; private set; }
        [JsonProperty]
        public string ComName { get; private set; }
        [JsonProperty]
        public int DayVolume { get; private set; }

        public static List<CompanyDayVolume> GetAll()
        {
            //offset 1330
            var offseted = Utility.TWSEDate.Today;
            var jsonFilePath = Path.Combine("CompanyDayVolume", $"{offseted:yyyyMMdd}.json");
            var caches = JsonCache.Load<List<CompanyDayVolume>>(jsonFilePath);
            if (caches != null)
                return caches;

            var totalRank = GetTotalRank();
            var result = new List<CompanyDayVolume>();

            var bags = new ConcurrentBag<List<CompanyDayVolume>>();
            var r = Parallel.ForEach(Enumerable.Range(0, totalRank), rank =>
            {
                var subResult = GetByRank(rank);
                bags.Add(subResult);
            });

            foreach (var res in bags)
                result.AddRange(res);

            JsonCache.Store(jsonFilePath, result);
            return result;
        }
        private static int GetTotalRank()
        {
            var request = WebRequest.Create();
            request.DefaultRequestHeaders.Add("Referer", RefererUrl);
            var resp = request.PostAsync(QueryBaseUrl + "0", null).Result;
            var content = resp.Content.ReadAsByteArrayAsync().Result;
            var data = Encoding.UTF8.GetString(content, 0, content.Length);

            IDocument doc = BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                .OpenAsync(req => req.Content(data)).Result;
            var rankOptions = doc.QuerySelectorAll("#selRANK>option")
                .Where(op => op.NodeValue != "99999");
            return rankOptions.Count();
        }
        private static List<CompanyDayVolume> GetByRank(int rank)
        {
            var request = WebRequest.Create();
            request.DefaultRequestHeaders.Add("Referer", RefererUrl);
            var resp = request.PostAsync(QueryBaseUrl + rank, null).Result;
            var bytes = resp.Content.ReadAsByteArrayAsync().Result;
            var content = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            IDocument doc = BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                .OpenAsync(req => req.Content(content)).Result;

            var trs = doc.QuerySelectorAll("#tblStockList>tbody>tr")
                .Where(tr => !string.IsNullOrEmpty(tr.Id) && tr.Id.StartsWith("row"));

            var result = new List<CompanyDayVolume>();
            foreach (var tr in trs)
            {
                var tds = tr.QuerySelectorAll("td");
                var data = new CompanyDayVolume();
                data.ComCode = tds[1].Text();
                data.ComName = tds[2].Text();

                var sDayVolume = tds[10].Text()
                    .Replace(",", "");
                data.DayVolume = int.Parse(sDayVolume);

                result.Add(data);
            }
            return result;
        }
    }
}
