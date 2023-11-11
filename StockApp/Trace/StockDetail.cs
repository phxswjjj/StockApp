using LiteDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Trace
{
    class StockDetail
    {
        [JsonConstructor]
        private StockDetail() { }
        public StockDetail(DisplayModel data)
        {
            this.ComCode = data.ComCode;
        }

        [BsonId]
        [JsonProperty]
        public string ComCode { get; private set; }
        [JsonProperty]
        public decimal Value { get; set; }

        private DateTime? _LimitDate;
        [JsonProperty]
        public DateTime? LimitDate
        {
            get
            {
                return _LimitDate;
            }
            set
            {
                _LimitDate = value;
                if (value.HasValue)
                {
                    var today = Utility.TWSEDate.Today;
                    this.LimitDateT = (int)(value.Value - today).TotalDays;
                }
                else
                    this.LimitDateT = null;
            }
        }
        [BsonIgnore]
        public int? LimitDateT { get; private set; }
    }
}
