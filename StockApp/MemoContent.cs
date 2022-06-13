using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class MemoContent
    {
        const string FilePath = "MemoContent.json";

        [JsonConstructor]
        private MemoContent() { }
        public MemoContent(DisplayModel data)
        {
            this.ComCode = data.ComCode;
            this.ComName = data.ComName;
            this.HoldStock = data.HoldStock;
            this.HoldValue = data.HoldValue;
        }

        [JsonProperty]
        public string ComCode { get; private set; }
        [JsonProperty]
        public string ComName { get; private set; }
        [JsonProperty]
        public int? HoldStock { get; set; }
        [JsonProperty]
        public decimal? HoldValue { get; set; }

        public static List<MemoContent> Load()
        {
            var result = new List<MemoContent>();
            if (File.Exists(FilePath))
                result = JsonCache.Load<List<MemoContent>>(FilePath);
            return result;
        }

        public static void Update(MemoContent data)
        {
            var list = Load();
            var existsIndex = list.FindIndex(d => d.ComCode == data.ComCode);
            if (existsIndex == -1)
                list.Add(data);
            else
                list[existsIndex] = data;
            JsonCache.Store(FilePath, list);
        }

        internal static void Remove(MemoContent data)
        {
            var list = Load();
            var existsIndex = list.FindIndex(d => d.ComCode == data.ComCode);
            if (existsIndex != -1)
            {
                list.RemoveAt(existsIndex);
                JsonCache.Store(FilePath, list);
            }
        }
    }
}
