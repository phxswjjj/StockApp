using LiteDB;
using PuppeteerSharp;
using StockApp.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Bonus
{
    internal class DividendRepository : IPurgeHistory
    {
        private readonly ILiteDatabase Db;

        public DividendRepository(ILiteDatabase db)
        {
            this.Db = db;
        }

        internal bool Initialize<T>()
            where T : CompanyExDividend
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
            var jsonFilePath = Path.Combine("CompanyExDividend", $"{today:yyyyMMdd}.json");
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

        internal void Imports(IEnumerable<CompanyExDividend> entities)
        {
            var db = this.Db;

            var list = db.GetCollection<CompanyExDividend>();
            list.Insert(entities);
        }

        internal CompanyExDividend GetDividendLatest()
        {
            var db = this.Db;

            var list = db.GetCollection<CompanyExDividend>();
            var data = list.Query()
                .OrderByDescending(x => x.UpdateAt)
                .FirstOrDefault();
            return data;
        }

        internal List<CompanyExDividend> GetDividends()
        {
            var db = this.Db;

            var latest = GetDividendLatest();
            if (latest == null)
                return null;
            var list = db.GetCollection<CompanyExDividend>();
            var entities = list.Query()
                .Where(x => x.UpdateAt == latest.UpdateAt);
            return entities.ToList();
        }

        public int PurgeHistory()
        {
            var db = this.Db;

            var latest = GetDividendLatest();
            if (latest == null)
                return 0;

            var list = db.GetCollection<CompanyExDividend>();
            return list.DeleteMany(d => d.UpdateAt < latest.UpdateAt.AddDays(-7));
        }
    }
}
