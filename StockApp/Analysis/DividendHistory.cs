using AngleSharp.Dom;
using AngleSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Analysis
{
    internal class DividendHistory
    {
        public int Year { get; set; }
        public decimal? TotalDividend { get; set; }

        public static List<DividendHistory> Get(string stockCode)
        {
            var url = $"https://goodinfo.tw/tw/StockDividendSchedule.asp?STOCK_ID={stockCode}";
            var request = Web.WebRequest.CreateGoodInfo();
            var resp = request.GetAsync(url).Result;
            var bytes = resp.Content.ReadAsByteArrayAsync().Result;
            var content = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

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
            return dividends;
        }
    }
}
