using LiteDB;
using StockApp.Group;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Trace
{
    internal class TraceStockRepository
    {
        const string TABLE_NAME = "TraceStock";

        private readonly ILiteDatabase Db;

        public TraceStockRepository(ILiteDatabase db)
        {
            this.Db = db;
        }

        internal bool Initialize<T>()
            where T : StockDetail
        {
            var db = this.Db;

            var imported = db.GetCollection<Data.LocalDb.ImportHistory>();
            var everImportRecord = imported.Query()
                .Where(x => x.Name == TABLE_NAME)
                .OrderByDescending(x => x.Timestamp)
                .FirstOrDefault();
            if (everImportRecord != null)
                return true;

            var jsonFilePath = "TraceStock.json";
            //沒檔案=第一次執行，不拋異常
            if (!File.Exists(jsonFilePath))
                return true;
            var caches = JsonCache.Load<List<T>>(jsonFilePath);
            //源頭沒資料拋異常
            if (caches?.Count == 0)
                return false;

            var list = db.GetCollection<T>(TABLE_NAME);
            //匯入資料
            foreach (var cache in caches)
            {
                list.Insert(cache);
            }
            list.EnsureIndex(d => d.ComCode);

            imported.Insert(new Data.LocalDb.ImportHistory(TABLE_NAME));

            return true;
        }

        public IEnumerable<StockDetail> GetAll() => this.Db.GetCollection<StockDetail>(TABLE_NAME).FindAll();

        internal void Update(StockDetail stock)
        {
            var db = this.Db;

            var list = db.GetCollection<StockDetail>(TABLE_NAME);
            var existsStock = list.FindById(stock.ComCode);
            if (existsStock == null)
                list.Insert(stock);
            else
                list.Update(stock);
        }

        internal bool Delete(StockDetail stock)
        {
            var db = this.Db;

            var list = db.GetCollection<StockDetail>(TABLE_NAME);
            return list.Delete(stock.ComCode);
        }
    }
}
