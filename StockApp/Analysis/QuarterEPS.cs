using AngleSharp.Dom;
using AngleSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Analysis
{
    internal class QuarterEPS
    {
        /// <summary>
        /// yyyyQQ
        /// </summary>
        public string YearQuarter { get; private set; }
        public decimal EPS { get; private set; }

        public static List<QuarterEPS> Get(string stockCode)
        {
            var url = $"https://goodinfo.tw/tw/StockFinDetail.asp?RPT_CAT=IS_M_QUAR_ACC&STOCK_ID={stockCode}";
            var request = Web.WebRequest.CreateGoodInfo();
            var resp = request.GetAsync(url).Result;
            var bytes = resp.Content.ReadAsByteArrayAsync().Result;
            var content = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            IDocument doc = BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                .OpenAsync(req => req.Content(content)).Result;

            var trs = doc.QuerySelectorAll("#divFinDetail tbody>tr");
            var trHead = trs.First();
            var quartTds = trHead.QuerySelectorAll("th").Skip(1).GetEnumerator();
            var trEPS = trs.Not(".bg_h1").Last();
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
            return quarts;
        }
    }
}
