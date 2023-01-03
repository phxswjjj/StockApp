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
using Newtonsoft.Json;

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

            var url = $"https://cwapi.cathaysite.com.tw/api/ETF/GetETFDetailStockList?FundCode={this.SiteCode}&SearchDate={offseted:yyyy-MM-dd}";

            var request = Web.WebRequest.Create();
            var resp = request.GetAsync(url);
            var content = resp.Result.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CathayResult>(content.Result);

            foreach (var data in result.result)
                cache.ComCodes.Add(data.stockCode.Trim());

            if (cache.ComCodes.Count > 0)
                JsonCache.Store(jsonFilePath, cache);
            return cache;
        }

        private class CathayResult
        {
            public List<CathayData> result;
        }
        private class CathayData
        {
            public string stockCode;
            public string stockName;
        }
    }
}
