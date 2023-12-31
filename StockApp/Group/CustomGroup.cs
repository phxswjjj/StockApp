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
        [JsonProperty]
        public virtual bool IsFavorite { get; protected set; } = true;
        [JsonProperty]
        public virtual int SortIndex => (int)this.Group;
        public string GroupTypeName
        {
            get
            {
                return Group.ToString();
            }
            set
            {
                if (Enum.TryParse<GroupType>(value, out var t))
                    this.Group = t;
                else
                    throw new Exception($"value 轉型 GroupType 失敗");
            }
        }
        public virtual GroupType Group { get; set; } = GroupType.CustomGroup;

        public static CustomGroup Create(string name)
        {
            var group = new CustomGroup() { Name = name };
            return group;
        }

        public enum GroupType
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
