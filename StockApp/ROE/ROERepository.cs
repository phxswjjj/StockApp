using LiteDB;
using StockApp.Group;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static StockApp.Group.CustomGroup;

namespace StockApp.ROE
{
    internal class ROERepository
    {
        private readonly ILiteDatabase Db;

        public ROERepository(ILiteDatabase db)
        {
            this.Db = db;
        }

        internal CompanyROE GetROELatest()
        {
            var db = this.Db;

            var list = db.GetCollection<CompanyROE>();
            var data = list.Query()
                .OrderByDescending(x => x.UpdateAt)
                .FirstOrDefault();
            return data;
        }

        internal CompanyROE GetROE(string comCode)
        {
            var db = this.Db;

            var list = db.GetCollection<CompanyROE>();
            var data = list.Query()
                .Where(x => x.ComCode == comCode)
                .OrderByDescending(x => x.UpdateAt)
                .FirstOrDefault();
            return data;
        }

        internal bool Initialize<T>()
            where T : CompanyROE
        {
            var db = this.Db;

            var imported = db.GetCollection<Data.LocalDb.ImportHistory>();
            var everImportRecord = imported.Query()
                .Where(x => x.Name == typeof(T).Name)
                .OrderByDescending(x => x.Timestamp)
                .FirstOrDefault();
            if (everImportRecord != null)
                return true;

            var today = Utility.TWSEDate.Today;
            var jsonFilePath = Path.Combine("CompanyROE", $"{today:yyyyMM}.json");
            //沒檔案=第一次執行，不拋異常
            if (!File.Exists(jsonFilePath))
                return true;
            var caches = JsonCache.Load<List<T>>(jsonFilePath);
            //源頭沒資料拋異常
            if (caches?.Count == 0)
                return false;

            var list = db.GetCollection<T>();
            //匯入資料
            foreach (var cache in caches)
            {
                if (cache.UpdateAt.Year < 2023)
                    cache.UpdateAt = today.Date;
            }
            this.Imports(caches);
            list.EnsureIndex(d => new { d.ComCode, d.UpdateAt }, true);
            list.EnsureIndex(d => d.ComCode);
            list.EnsureIndex(d => d.UpdateAt);

            imported.Insert(new Data.LocalDb.ImportHistory(typeof(T).Name));

            return true;
        }

        internal void Imports(IEnumerable<CompanyROE> entities)
        {
            var db = this.Db;

            var list = db.GetCollection<CompanyROE>();
            list.Insert(entities);
        }
    }
}
