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
        const string FilePath = "TraceStock.json";

        [JsonConstructor]
        private StockDetail() { }
        public StockDetail(DisplayModel data)
        {
            this.ComCode = data.ComCode;
        }

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
        public int? LimitDateT { get; private set; }

        public static List<StockDetail> Load()
        {
            List<StockDetail> result;
            if (File.Exists(FilePath))
                result = JsonCache.Load<List<StockDetail>>(FilePath);
            else
                result = new List<StockDetail>();
            return result;
        }

        public void Update()
        {
            var list = Load();
            var existsIndex = list.FindIndex(d => d.ComCode == this.ComCode);
            if (existsIndex == -1)
                list.Add(this);
            else
                list[existsIndex] = this;
            JsonCache.Store(FilePath, list);
        }

        public void Remove()
        {
            var list = Load();
            var existsIndex = list.FindIndex(d => d.ComCode == this.ComCode);
            if (existsIndex != -1)
            {
                list.RemoveAt(existsIndex);
                JsonCache.Store(FilePath, list);
            }
        }
    }
}
