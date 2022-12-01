using Newtonsoft.Json;
using StockApp.Group;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.ETF
{
    abstract class ETFBase
    {
        public abstract string ComCode { get; }

        internal CustomGroup GetAll()
        {
            var offseted = Utility.TWSEDate.Today;
            //有問題，先拿舊資料(11 月)
            offseted = new DateTime(2022, 11, 1);
            var jsonFilePath = Path.Combine("CustomGroup", ComCode, $"{offseted:yyyyMM}.json");

            var cache = JsonCache.Load<ETFGroup>(jsonFilePath);
            if (cache != null)
                return cache;

            cache = new ETFGroup()
            {
                Name = $"{ComCode} 成份股",
            };

            var url = "https://www.cmoney.tw/etf/ashx/e210.ashx";
            var request = Web.WebRequest.Create();
            var formData = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                {"action", "GetShareholdingDetails" },
                {"stockId", ComCode },
            });
            var resp = request.PostAsync(url, formData).Result;
            var content = resp.Content.ReadAsStringAsync().Result;
            var model = JsonConvert.DeserializeObject<DataModelE210>(content);
            cache.ComCodes.AddRange(model.Data.Select(d => d.CommKey));

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
