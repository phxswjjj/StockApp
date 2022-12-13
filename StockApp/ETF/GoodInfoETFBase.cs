using AngleSharp.Dom;
using AngleSharp;
using StockApp.Group;
using StockApp.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.ETF
{
    abstract class GoodInfoETFBase
    {
        public abstract string ComCode { get; }

        internal CustomGroup GetAll()
        {
            var offseted = Utility.TWSEDate.Today;
            var jsonFilePath = Path.Combine("CustomGroup", ComCode, $"{offseted:yyyyMM}.json");

            var cache = JsonCache.Load<ETFGroup>(jsonFilePath);
            if (cache != null)
                return cache;

            cache = new ETFGroup()
            {
                Name = $"{ComCode} 成份股",
            };

            var url = $"https://goodinfo.tw/tw/StockDetail.asp?STOCK_ID={ComCode}";

            string content;
            using (var page = ChromiumBrowser.NewPage(url))
            {
                page.WaitForSelectorAsync("table.p4_2").Wait();
                content = page.EvaluateFunctionAsync<string>(@"() => {
return document.querySelectorAll('table.p4_2')[6].innerHTML;
}").Result;
            }

            IDocument doc = BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                .OpenAsync(req => req.Content(content)).Result;

            var tds = doc.QuerySelectorAll("a.link_black nobr:nth-child(1)");
            foreach (var td in tds)
                cache.ComCodes.Add(td.Text().Trim());

            if (cache.ComCodes.Count > 0)
                JsonCache.Store(jsonFilePath, cache);
            return cache;
        }
    }
}
