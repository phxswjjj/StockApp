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

    class TraceGroup : CustomGroup
    {
        public override bool IsFavorite => false;
    }

    class ETFGroup : CustomGroup
    {
        public override bool IsFavorite => false;
    }

    class FavoriteGroup : CustomGroup
    {
    }
    class HateGroup : CustomGroup
    {
    }
}
