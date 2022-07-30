using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class MemoContent : IMemoContent
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
        public int? Stock { get => this.HoldStock; set => this.HoldStock = value; }
        public decimal? Value { get => this.HoldValue; set => this.HoldValue = value; }

        public static List<MemoContent> Load()
        {
            var result = new List<MemoContent>();
            if (File.Exists(FilePath))
                result = JsonCache.Load<List<MemoContent>>(FilePath);
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

        public void UpdateModel(DisplayModel model)
        {
            model.SetExtra(this);
        }
    }

    interface IMemoContent
    {
        int? Stock { get; set; }
        decimal? Value { get; set; }

        void Update();
        void Remove();
        void UpdateModel(DisplayModel model);
    }
}
