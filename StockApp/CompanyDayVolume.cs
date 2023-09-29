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
        public DateTime UpdateAt { get; set; } = DateTime.Today;
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
            var today = Utility.TWSEDate.Today;

            var request = Web.WebRequest.Create();
            var requestEx = Web.WebRequest.Create();
            var resp = request.GetAsync($"https://www.twse.com.tw/exchangeReport/MI_INDEX?response=json&date={today:yyyyMMdd}&type=ALL&_=1658241396683");
            var twYear = today.Year - 1911;
            var respEx = requestEx.GetAsync($"https://www.tpex.org.tw/web/stock/aftertrading/otc_quotes_no1430/stk_wn1430_result.php?l=zh-tw&d={twYear}/{today:MM/dd}&se=AL&_=1658242781730");
            var content = resp.Result.Content.ReadAsStringAsync();
            var contentEx = respEx.Result.Content.ReadAsStringAsync();

            var model = JsonConvert.DeserializeObject<TWSEDataModel>(content.Result);
            if (model.data9 == null)
                return null;

            var result = new List<CompanyDayVolume>();
            foreach (var modelData in model.data9)
            {
                var data = new CompanyDayVolume
                {
                    UpdateAt = today,
                    ComCode = modelData[0],
                    ComName = modelData[1],
                    ComType = "市"
                };

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
                var data = new CompanyDayVolume
                {
                    ComCode = modelData[0],
                    ComName = modelData[1],
                    ComType = "櫃"
                };

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
            {
                var jsonFilePath = Path.Combine("CompanyDayVolume", $"{today:yyyyMMdd}.json");
                JsonCache.Store(jsonFilePath, result);

                var jsonLastFilePath = Path.Combine("CompanyDayVolume", $"last.json");
                JsonCache.Store(jsonLastFilePath, result);
            }
            return result;
        }

        private class TWSEDataModel
        {
            public List<List<string>> data9 = default;
        }
        private class TPEXDataModel
        {
            public List<List<string>> aaData = default;
        }
    }
}
