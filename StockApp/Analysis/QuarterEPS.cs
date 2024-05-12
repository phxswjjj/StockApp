using AngleSharp.Dom;
using AngleSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.IO.Compression;
using System.IO.Pipes;
using StockApp.Web;
using PuppeteerSharp;
using StockApp.Group;
using System.Reflection.Emit;
using Newtonsoft.Json;

namespace StockApp.Analysis
{
    internal class QuarterEPS
    {
        static readonly TimeSpan CacheTimeSpan = new TimeSpan(7, 0, 0, 0);

        /// <summary>
        /// yyyyQQ
        /// </summary>
        [JsonProperty]
        public string YearQuarter { get; private set; }
        [JsonProperty]
        public decimal EPS { get; private set; }
        public static List<QuarterEPS> Get(string stockCode)
        {
            var jsonFilePath = Path.Combine("Analysis", $"{stockCode}-QuarterEPS.json");
            var cache = JsonCache.Load<List<QuarterEPS>>(jsonFilePath, CacheTimeSpan);
            if (cache != null)
                return cache;

            var url = $"https://goodinfo.tw/tw/StockFinDetail.asp?RPT_CAT=IS_M_QUAR_ACC&STOCK_ID={stockCode}";
            string content;
            using (var page = ChromiumBrowser.NewPage(url))
            {
                page.WaitForSelectorAsync("#divFinDetail").Wait();
                content = page.GetContentAsync().Result;
            }

            IDocument doc = BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                .OpenAsync(req => req.Content(content)).Result;

            var trs = doc.QuerySelectorAll("#divFinDetail tbody>tr");
            var trHead = trs.First();
            var quartTds = trHead.QuerySelectorAll("th").Skip(1).GetEnumerator();
            var trEPS = trs.Not(".bg_h1")
                //Id start with 'hrow' 只有標題
                .Where(row => row.Id.StartsWith("row"))
                .Last();
            var epsTds = trEPS.QuerySelectorAll("td").Skip(1).GetEnumerator();

            var quarts = new List<QuarterEPS>();
            while (quartTds.MoveNext())
            {
                epsTds.MoveNext();
                var quart = new QuarterEPS()
                {
                    YearQuarter = quartTds.Current.TextContent,
                    EPS = decimal.Parse(epsTds.Current.TextContent),
                };
                quarts.Add(quart);

                //skip empty
                epsTds.MoveNext();
            }
            if (quarts.Count > 0)
                JsonCache.Store(jsonFilePath, quarts);
            return quarts;
        }
    }
}
