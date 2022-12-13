using AngleSharp.Dom;
using AngleSharp;
using Newtonsoft.Json;
using PuppeteerSharp;
using StockApp.Group;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using StockApp.Web;
using StockApp.Utility;

namespace StockApp.ETF
{
    abstract class YuanTaETFBase
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

            var url = $"https://www.yuantaetfs.com/product/detail/{ComCode}/ratio";

            string content;
            using (var page = ChromiumBrowser.NewPage(url))
            {
                //等抓到第一個項目再點 more
                page.WaitForSelectorAsync("div.each_table div.tr:nth-child(1)").Wait();
                page.ClickAsync("div.moreBtn").Wait();
                page.WaitForSelectorAsync("div.each_table div.tr:nth-child(6)").Wait();
                content = page.EvaluateFunctionAsync<string>(@"() => {
return document.querySelectorAll('div.each_table')[1].innerHTML;
}").Result;
            }

            IDocument doc = BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                .OpenAsync(req => req.Content(content)).Result;

            var tds = doc.QuerySelectorAll("div.tr div.td:nth-child(1) span:not(.d-md-none)");
            foreach (var td in tds)
                cache.ComCodes.Add(td.Text());

            if (cache.ComCodes.Count > 0)
                JsonCache.Store(jsonFilePath, cache);
            return cache;
        }
    }

    class DataModelE210
    {
        public List<Company> Data;

        public class Company
        {
            public string CommKey;
            public string CommName;
            /// <summary>
            /// 期貨, 股票
            /// </summary>
            public string Type;
        }
    }
}
