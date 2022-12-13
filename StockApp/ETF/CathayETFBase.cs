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
    abstract class CathayETFBase
    {
        public abstract string ComCode { get; }
        protected abstract string SiteCode { get; }

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

            var url = $"https://www.cathaysite.com.tw/ETF/detail/{SiteCode}?tab=etf3";

            string content;
            using (var page = ChromiumBrowser.NewPage(url))
            {
                page.WaitForSelectorAsync("div[data-anchor=\"股票\"] div.bar_table_body div.bar_table_line:nth-child(1)").Wait();
                content = page.EvaluateFunctionAsync<string>(@"() => {
return document.querySelector('div[data-anchor=""股票""] div.bar_table_body').innerHTML;
}").Result;
            }

            IDocument doc = BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                .OpenAsync(req => req.Content(content)).Result;

            var tds = doc.QuerySelectorAll("span.code:nth-child(1)");
            foreach (var td in tds)
                cache.ComCodes.Add(td.Text().Trim());

            if (cache.ComCodes.Count > 0)
                JsonCache.Store(jsonFilePath, cache);
            return cache;
        }
    }
}
