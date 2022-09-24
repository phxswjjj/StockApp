using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class CustomGroup
    {
        const string JsonFilePath = "CustomGroup.json";

        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public List<string> ComCodes { get; set; } = new List<string>();
        [JsonIgnore]
        public virtual bool IsFavorite { get; protected set; } = true;
        [JsonIgnore]
        public virtual int SortIndex { get; set; } = 10000;

        public static CustomGroup Create(string name)
        {
            var group = new CustomGroup() { Name = name };
            return group;
        }

        internal static List<CustomGroup> GetAll()
        {
            var caches = JsonCache.Load<List<CustomGroup>>(JsonFilePath);
            if (caches != null)
                return caches;
            else
                return new List<CustomGroup>();
        }

        internal void Distinct()
        {
            this.ComCodes = this.ComCodes.Distinct().ToList();
        }

        internal static void Store(List<CustomGroup> groups)
        {
            JsonCache.Store(JsonFilePath, groups);
        }
    }

    class FavoriteGroup : CustomGroup
    {
        public const int DefaultSortIndex = 1;
        public override int SortIndex => DefaultSortIndex;
    }
    class HateGroup : CustomGroup
    {
        public const int DefaultSortIndex = 2;
        public override int SortIndex => DefaultSortIndex;
    }

    class ETFGroup : CustomGroup
    {
        public const int DefaultSortIndex = 11000;
        public override bool IsFavorite => false;
        public override int SortIndex => DefaultSortIndex;
    }

    class TraceGroup : CustomGroup
    {
        public const int DefaultSortIndex = 12000;
        public override bool IsFavorite => false;
        public override int SortIndex => DefaultSortIndex;
    }
}
