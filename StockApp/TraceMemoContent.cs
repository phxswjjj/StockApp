using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class TraceMemoContent : IMemoContent
    {
        const string FilePath = "TraceMemoContent.json";

        [JsonConstructor]
        private TraceMemoContent() { }
        public TraceMemoContent(DisplayModel data)
        {
            this.ComCode = data.ComCode;
            this.ComName = data.ComName;
            this.TraceValue = data.HoldValue;
        }

        [JsonProperty]
        public string ComCode { get; private set; }
        [JsonProperty]
        public string ComName { get; private set; }
        [JsonProperty]
        public decimal? TraceValue { get; set; }
        public int? Stock { get => null; set => throw new NotImplementedException(); }
        public decimal? Value { get => this.TraceValue; set => this.TraceValue = value; }

        public static List<TraceMemoContent> Load()
        {
            var result = new List<TraceMemoContent>();
            if (File.Exists(FilePath))
                result = JsonCache.Load<List<TraceMemoContent>>(FilePath);
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
