using AngleSharp;
using AngleSharp.Dom;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class CompanyDayVolume
    {
        [JsonProperty]
        public string ComCode { get; private set; }
        [JsonProperty]
        public string ComName { get; private set; }
        [JsonProperty]
        public string ComType { get; private set; }
        [JsonProperty]
        public int DayVolume { get; private set; }
        [JsonProperty]
        public decimal CurrentPrice { get; private set; }

        public static List<CompanyDayVolume> GetAll()
        {
            //offset 1330
            var offseted = Utility.TWSEDate.Today;
            var jsonFilePath = Path.Combine("CompanyDayVolume", $"{offseted:yyyyMMdd}.json");
            var caches = JsonCache.Load<List<CompanyDayVolume>>(jsonFilePath);
            if (caches != null)
                return caches;

            var request = WebRequest.Create();
            var requestEx = WebRequest.Create();
            var resp = request.GetAsync("https://www.twse.com.tw/exchangeReport/MI_INDEX?response=json&date=20220719&type=ALL&_=1658241396683");
            var respEx = requestEx.GetAsync("https://www.tpex.org.tw/web/stock/aftertrading/otc_quotes_no1430/stk_wn1430_result.php?l=zh-tw&d=111/07/19&se=AL&_=1658242781730");
            var content = resp.Result.Content.ReadAsStringAsync();
            var contentEx = respEx.Result.Content.ReadAsStringAsync();

            var model = JsonConvert.DeserializeObject<TWSEDataModel>(content.Result);

            var result = new List<CompanyDayVolume>();
            foreach (var modelData in model.data9)
            {
                var data = new CompanyDayVolume();
                data.ComCode = modelData[0];
                data.ComName = modelData[1];
                data.ComType = "市";

                var sPrice = modelData[8]
                    .Replace("-", "");
                if (string.IsNullOrEmpty(sPrice))
                    continue;
                data.CurrentPrice = decimal.Parse(sPrice);
                var sDayVolume = modelData[2]
                    .Replace(",", "");
                //換算張數
                data.DayVolume = int.Parse(sDayVolume) / 1000;

                result.Add(data);
            }

            var modelEx = JsonConvert.DeserializeObject<TPEXDataModel>(contentEx.Result);
            foreach (var modelData in modelEx.aaData)
            {
                var data = new CompanyDayVolume();
                data.ComCode = modelData[0];
                data.ComName = modelData[1];
                data.ComType = "櫃";

                var sPrice = modelData[2]
                    .Replace("-", "");
                if (string.IsNullOrEmpty(sPrice))
                    continue;
                data.CurrentPrice = decimal.Parse(sPrice);
                var sDayVolume = modelData[7]
                    .Replace(",", "");
                //換算張數
                data.DayVolume = int.Parse(sDayVolume) / 1000;

                result.Add(data);
            }

            if (result.Count > 300)
                JsonCache.Store(jsonFilePath, result);
            return result;
        }

        private class TWSEDataModel
        {
            public List<List<string>> data9;
        }
        private class TPEXDataModel
        {
            public List<List<string>> aaData;
        }
    }
}
