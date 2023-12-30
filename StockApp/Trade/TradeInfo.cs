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
        public string SysId { get; set; }
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
        public string StockCenterName { get; set; }
        [JsonProperty]
        public StockCenterType? StockCenter
        {
            get
            {
                if (Enum.TryParse<StockCenterType>(StockCenterName, out var result))
                    return result;
                else
                    return null;
            }
            set
            {
                if (value.HasValue)
                    StockCenterName = value.ToString();
                else
                    StockCenterName = null;
            }
        }
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
