using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StockApp.Group.CustomGroup;

namespace StockApp.Group
{
    internal class CustomGroupRepository
    {
        private readonly ILiteDatabase Db;

        public CustomGroupRepository(ILiteDatabase db)
        {
            this.Db = db;
        }

        internal bool Initialize<T>()
            where T : CustomGroup
        {
            var db = this.Db;

            var imported = db.GetCollection<Data.LocalDb.ImportHistory>();
            var everImportRecord = imported.Query()
                .Where(x => x.Name == typeof(T).Name)
                .OrderByDescending(x => x.Timestamp)
                .FirstOrDefault();
            if (everImportRecord != null)
                return true;

            var jsonFilePath = $"CustomGroup\\{typeof(T)}.json";
            //沒檔案=第一次執行，不拋異常
            if (!File.Exists(jsonFilePath))
                return true;
            var caches = JsonCache.Load<List<T>>(jsonFilePath);
            //源頭沒資料拋異常
            if (caches?.Count == 0)
                return false;

            var list = db.GetCollection<CustomGroup>();
            //匯入資料
            foreach (var cache in caches)
            {
                cache.Group = (GroupType)cache.SortIndex;
                list.Insert(cache);
            }
            list.EnsureIndex(d => d.Name);

            imported.Insert(new Data.LocalDb.ImportHistory(typeof(T).Name));

            return true;
        }

        public IEnumerable<CustomGroup> GetAll() => this.Db.GetCollection<CustomGroup>().FindAll();

        internal void Updates(List<CustomGroup> customGroups)
        {
            var db = this.Db;

            var list = db.GetCollection<CustomGroup>();
            foreach (var customGroup in customGroups)
            {
                var existsGroup = list.FindById(customGroup.Name);
                if (existsGroup != null)
                {
                    if (customGroup.ComCodes.Count > 0)
                    {
                        existsGroup.ComCodes = customGroup.ComCodes;
                        existsGroup.Timestamp = DateTime.Now;
                        list.Update(existsGroup);
                    }
                    else if (existsGroup.Group == GroupType.CustomGroup)
                        list.Delete(existsGroup.Name);
                }
                else if (customGroup.ComCodes.Count > 0)
                    list.Insert(customGroup);
            }
        }
    }
}
