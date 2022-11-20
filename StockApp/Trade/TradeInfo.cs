﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Trade
{
    internal class TradeInfo
    {
        [JsonIgnore]
        public DisplayModel Source { get; private set; }
        [JsonProperty]
        public string ComCode { get; private set; }
        [JsonProperty]
        public DateTime? TradeDate { get; set; }
        [JsonProperty]
        public decimal TradePrice { get; set; }
        [JsonProperty]
        public int TradeVolume { get; set; }
        public decimal? CurrentValue => this.Source?.CurrentPrice * this.TradeVolume;
        [JsonProperty]
        public StockCenterType? StockCenter { get; set; }
        [JsonProperty]
        public string Memo { get; set; }

        [JsonConstructor]
        private TradeInfo() { }
        public TradeInfo(DisplayModel data)
        {
            this.Source = data;
            this.ComCode = data.ComCode;
        }

        internal static List<TradeInfo> GetAll()
        {
            var folder = "Trade";
            var trades = new List<TradeInfo>();
            if (!Directory.Exists(folder))
                return trades;
            foreach (var filePath in Directory.GetFiles(folder, "*.json"))
            {
                var list = JsonCache.Load<List<TradeInfo>>(filePath);
                trades.AddRange(list);
            }
            return trades;
        }

        internal static void Store(string comCode, List<TradeInfo> trades)
        {
            var folder = "Trade";
            var filePath = Path.Combine(folder, $"{comCode}.json");
            JsonCache.Store(filePath, trades);
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    enum StockCenterType
    {
        元大,
        永豐,
    }
}