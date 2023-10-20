using LiteDB;
using Newtonsoft.Json;
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
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.Empty;
        [BsonIgnore]
        public DisplayModel Source { get; private set; }
        [JsonProperty]
        public string ComCode { get; private set; }
        [JsonProperty]
        public DateTime? TradeDate { get; set; }
        [JsonProperty]
        public decimal TradePrice { get; set; }
        [JsonProperty]
        public int TradeVolume { get; set; }
        [BsonIgnore]
        public decimal? CurrentValue => this.Source?.CurrentPrice * this.TradeVolume;
        [JsonProperty]
        public StockCenterType? StockCenter { get; set; }
        [JsonProperty]
        public string Memo { get; set; }

        [BsonCtor]
        public TradeInfo() { }
        public TradeInfo(DisplayModel data)
        {
            this.Source = data;
            this.ComCode = data.ComCode;
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    enum StockCenterType
    {
        元大,
        永豐,
    }
}
