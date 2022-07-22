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
    class CompanyExDividend
    {
        const string QueryBaseUrl = "https://goodinfo.tw/tw/StockDividendScheduleList.asp?MARKET_CAT=%E5%85%A8%E9%83%A8&INDUSTRY_CAT=%E5%85%A8%E9%83%A8&YEAR=%E5%8D%B3%E5%B0%87%E9%99%A4%E6%AC%8A%E6%81%AF";

        [JsonProperty]
        public string ComCode { get; private set; }
        [JsonProperty]
        public string ComName { get; private set; }
        [JsonProperty]
        public decimal? ExDividendBonus { get; private set; }
        [JsonProperty]
        public DateTime? ExDividendDate { get; private set; }

        public static List<CompanyExDividend> GetAll()
        {
            //offset 1330
            var offseted = Utility.TWSEDate.Today;
            var jsonFilePath = Path.Combine("CompanyExDividend", $"{offseted:yyyyMMdd}.json");
            var caches = JsonCache.Load<List<CompanyExDividend>>(jsonFilePath);
            if (caches != null)
                return caches;

            var request = WebRequest.CreateGoodInfo();
            var resp = request.GetAsync(QueryBaseUrl).Result;
            var bytes = resp.Content.ReadAsByteArrayAsync().Result;
            var content = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            IDocument doc = BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                .OpenAsync(req => req.Content(content)).Result;

            var trs = doc.QuerySelectorAll("#tblDetail>tbody>tr")
                .Where(tr => !tr.HasAttribute("class"));

            var result = new List<CompanyExDividend>();
            foreach (var tr in trs)
            {
                var tds = tr.QuerySelectorAll("td");
                if (tds.Length == 0)
                    continue;
                var data = new CompanyExDividend();
                data.ComCode = tds[1].Text();
                data.ComName = tds[2].Text();

                var sBonus = tds.Last().Text();
                data.ExDividendBonus = decimal.Parse(sBonus);

                var sExDividendDate = tds[4].Text()
                    .Replace("即將除息", "")
                    .Replace("今日除息", "")
                    .Replace("'", "/")
                    .Trim();
                if (string.IsNullOrEmpty(sExDividendDate))
                    continue;
                data.ExDividendDate = DateTime.ParseExact(sExDividendDate, "yy/MM/dd",
                    System.Globalization.CultureInfo.InvariantCulture);

                result.Add(data);
            }

            if (result.Count > 10)
                JsonCache.Store(jsonFilePath, result);
            return result;
        }
    }
}
