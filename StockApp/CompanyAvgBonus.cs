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
    class CompanyAvgBonus
    {
        [JsonProperty]
        public string ComCode { get; private set; }
        [JsonProperty]
        public string ComName { get; private set; }
        [JsonProperty]
        public decimal AvgBonus { get; private set; }
        [JsonProperty]

        public decimal AvgYield { get; private set; }

        public decimal Expect5 => Math.Floor(this.AvgBonus / 0.05m * 100) / 100;
        public decimal Expect7 => Math.Floor(this.AvgBonus / 0.07m * 100) / 100;
        public decimal Expect9 => Math.Floor(this.AvgBonus / 0.09m * 100) / 100;

        public static List<CompanyAvgBonus> GetAll()
        {
            //offset 1330
            var offseted = Utility.TWSEDate.Today;
            var jsonFilePath = Path.Combine("CompanyAvgBonus", $"{offseted:yyyyMMdd}.json");
            var caches = JsonCache.Load<List<CompanyAvgBonus>>(jsonFilePath);
            if (caches != null)
                return caches;

            var jsonLastFilePath = Path.Combine("CompanyAvgBonus", $"last.json");

            var request = Web.WebRequest.Create();
            var requestEx = Web.WebRequest.Create();
            var resp = request.GetAsync($"https://www.twse.com.tw/exchangeReport/BWIBBU_d?response=json&date={offseted:yyyyMMdd}&selectType=ALL&_=1658329489142");
            var respEx = requestEx.GetAsync("https://www.tpex.org.tw/web/stock/aftertrading/peratio_analysis/pera_result.php?l=zh-tw&_=1658330021875");
            var content = resp.Result.Content.ReadAsStringAsync();
            var contentEx = respEx.Result.Content.ReadAsStringAsync();

            var model = JsonConvert.DeserializeObject<TWSEDataModel>(content.Result);
            if (model.data == null)
            {
                var cashesLast = JsonCache.Load<List<CompanyAvgBonus>>(jsonLastFilePath);
                if (cashesLast != null)
                    return cashesLast;
            }

            var result = new List<CompanyAvgBonus>();
            foreach (var modelData in model.data)
            {
                var data = new CompanyAvgBonus();
                data.ComCode = modelData[0];
                data.ComName = modelData[1];

                var sAvgYield = modelData[2];
                if (string.IsNullOrEmpty(sAvgYield))
                    continue;
                data.AvgYield = decimal.Parse(sAvgYield);

                result.Add(data);
            }

            var modelEx = JsonConvert.DeserializeObject<TPEXDataModel>(contentEx.Result);
            foreach (var modelData in modelEx.aaData)
            {
                var data = new CompanyAvgBonus();
                data.ComCode = modelData[0];
                data.ComName = modelData[1];

                var sAvgBonus = modelData[3];
                if (string.IsNullOrEmpty(sAvgBonus))
                    continue;
                data.AvgBonus = decimal.Parse(sAvgBonus);

                var sAvgYield = modelData[5];
                if (string.IsNullOrEmpty(sAvgYield))
                    continue;
                data.AvgYield = decimal.Parse(sAvgYield);

                result.Add(data);
            }

            if (result.Count > 300)
            {
                JsonCache.Store(jsonFilePath, result);
                JsonCache.Store(jsonLastFilePath, result);
            }
            return result;
        }

        private class TWSEDataModel
        {
            public List<List<string>> data;
        }
        private class TPEXDataModel
        {
            public List<List<string>> aaData;
        }
    }
}
