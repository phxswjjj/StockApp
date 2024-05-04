using AngleSharp;
using AngleSharp.Dom;
using Newtonsoft.Json;
using StockApp.Web;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.ROE
{
    class CompanyROE
    {
        const string QueryBaseUrl = "https://goodinfo.tw/tw/StockList.asp?SEARCH_WORD=&SHEET=%E5%B9%B4%E7%8D%B2%E5%88%A9%E8%83%BD%E5%8A%9B%5F%E8%BF%91N%E5%B9%B4%E4%B8%80%E8%A6%BD&SHEET2=ROE%28%25%29&MARKET_CAT=%E7%86%B1%E9%96%80%E6%8E%92%E8%A1%8C&INDUSTRY_CAT=%E5%B9%B4%E5%BA%A6ROE%E6%9C%80%E9%AB%98%40%40%E8%82%A1%E6%9D%B1%E6%AC%8A%E7%9B%8A%E5%A0%B1%E9%85%AC%E7%8E%87+%28ROE%29%40%40%E5%B9%B4%E5%BA%A6ROE%E6%9C%80%E9%AB%98&STOCK_CODE=&RPT_TIME=%E6%9C%80%E6%96%B0%E8%B3%87%E6%96%99&STEP=DATA&RANK=99999";

        public DateTime UpdateAt { get; set; }
        [JsonProperty]
        public string ComCode { get; set; }
        [JsonProperty]
        public string ComName { get; private set; }
        [JsonProperty]
        public List<string> ROEHeaders { get; set; } = new List<string>();
        [JsonProperty]
        public List<decimal?> ROEValues { get; set; } = new List<decimal?>();

        public static List<CompanyROE> GetAll()
        {
            string content;
            using (var page = ChromiumBrowser.NewPage(QueryBaseUrl))
            {
                page.WaitForSelectorAsync("#tblStockList").Wait();
                content = page.GetContentAsync().Result;
            }

            IDocument doc = BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                .OpenAsync(req => req.Content(content)).Result;

            var trs = doc.QuerySelectorAll("#tblStockList>tbody>tr");
            var headerTr = trs.First();
            var dataTrs = trs.Where(tr => !string.IsNullOrEmpty(tr.Id) && tr.Id.StartsWith("row"));

            var headerThs = headerTr.QuerySelectorAll("th");
            var headers = new Dictionary<int, string>();
            var fetchHeader = new Func<IElement, string>((th) =>
            {
                var text = th.Text()
                    .Replace("ROE(%)", "")
                    .Trim();
                return text;
            });
            for (var i = 7; i < headerThs.Length; i++)
                headers.Add(i, fetchHeader(headerThs[i]));

            var fetchROE = new Func<IElement, decimal?>((td) =>
            {
                var text = td.Text();
                try
                {
                    if (string.IsNullOrEmpty(text))
                        return null;
                    return decimal.Parse(text);
                }
                catch { }
                return null;
            });

            var offseted = Utility.TWSEDate.Today;
            var results = new List<CompanyROE>();
            var headerTexts = headers.Values.ToList();
            foreach (var tr in dataTrs)
            {
                var tds = tr.QuerySelectorAll("td");
                var data = new CompanyROE
                {
                    ComCode = tds[1].Text(),
                    ComName = tds[2].Text(),
                    ROEHeaders = headerTexts,
                    UpdateAt = offseted.Date,
                };
                var roeValues = new List<decimal?>();
                foreach (var header in headers)
                    roeValues.Add(fetchROE(tds[header.Key]));
                data.ROEValues = roeValues;

                results.Add(data);
            }

            return results;
        }

        public class Entity
        {
            public DateTime UpdateAt { get; set; }
            public string ComCode { get; set; }
            public string ROEHeader { get; set; }
            public decimal? ROEValue { get; set; }
        }
    }
}
