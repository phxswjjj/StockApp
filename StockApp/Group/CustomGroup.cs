using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Group
{
    class CustomGroup
    {
        [JsonProperty]
        public virtual string Name { get; set; }
        [JsonProperty]
        public List<string> ComCodes { get; set; } = new List<string>();
        /// <summary>
        /// true: 允許使用者異動
        /// </summary>
        [JsonIgnore]
        public virtual bool IsFavorite { get; protected set; } = true;
        [JsonIgnore]
        public virtual int SortIndex { get; set; } = (int)DefaultSortIndexType.CustomGroup;

        public static CustomGroup Create(string name)
        {
            var group = new CustomGroup() { Name = name };
            return group;
        }

        internal static List<CustomGroup> GetAll()
        {
            var caches = GetAll<CustomGroup>();
            if (caches == null)
                caches = new List<CustomGroup>();

            caches.AddRange(GetAll<FavoriteGroup>());
            caches.AddRange(GetAll<HateGroup>());
            caches.AddRange(GetAll<ETFGroup>());
            caches.AddRange(GetAll<TraceGroup>());
            return caches
                .Distinct(new CustomGroupEqualComparer())
                .ToList();
        }
        private static List<CustomGroup> GetAll<T>()
            where T : CustomGroup, new()
        {
            var jsonFilePath = $"CustomGroup\\{typeof(T)}.json";

            var caches = JsonCache.Load<List<T>>(jsonFilePath);
            if (caches == null)
                caches = new List<T>();

            if (caches.Count == 0)
                caches.Add(new T());

            return caches.Cast<CustomGroup>().ToList();
        }

        internal void Distinct()
        {
            this.ComCodes = this.ComCodes.Distinct().ToList();
        }

        internal static void Store(List<CustomGroup> groups)
        {
            Store<CustomGroup>(groups.Where(g => g.SortIndex == (int)DefaultSortIndexType.CustomGroup));
            Store<FavoriteGroup>(groups.Where(g => g.SortIndex == (int)DefaultSortIndexType.FavoriteGroup));
            Store<HateGroup>(groups.Where(g => g.SortIndex == (int)DefaultSortIndexType.HateGroup));
            Store<ETFGroup>(groups.Where(g => g.SortIndex == (int)DefaultSortIndexType.ETFGroup));
            Store<TraceGroup>(groups.Where(g => g.SortIndex == (int)DefaultSortIndexType.TraceGroup));
        }
        private static void Store<T>(IEnumerable<CustomGroup> groups)
            where T : CustomGroup
        {
            var jsonFilePath = $"CustomGroup\\{typeof(T)}.json";
            JsonCache.Store(jsonFilePath, groups);
        }

        public enum DefaultSortIndexType
        {
            FavoriteGroup = 1,
            HateGroup = 2,
            CustomGroup = 10000,
            ETFGroup = 11000,
            TraceGroup = 12000,
        }
        protected class CustomGroupEqualComparer : IEqualityComparer<CustomGroup>
        {
            public bool Equals(CustomGroup x, CustomGroup y)
            {
                return x.GetHashCode() == y.GetHashCode();
            }

            public int GetHashCode(CustomGroup obj)
            {
                if (!string.IsNullOrEmpty(obj.Name))
                    return obj.Name.GetHashCode();
                else
                    return obj.GetType().GetHashCode();
            }
        }
    }
}
