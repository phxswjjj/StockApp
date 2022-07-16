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
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public List<string> ComCodes { get; set; } = new List<string>();

        public static CustomGroup Create(string name)
        {
            var group = new CustomGroup() { Name = name };
            return group;
        }

        internal static List<CustomGroup> GetAll()
        {
            var jsonFilePath = "CustomGroup.json";
            var caches = JsonCache.Load<List<CustomGroup>>(jsonFilePath);
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
            var jsonFilePath = "CustomGroup.json";
            JsonCache.Store(jsonFilePath, groups);
        }
    }
}
