using LiteDB;
using StockApp.Day;
using StockApp.Group;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StockApp.Group.CustomGroup;

namespace StockApp.Trade
{
    internal class TradeRepository
    {
        private readonly ILiteDatabase Db;

        public TradeRepository(ILiteDatabase db)
        {
            this.Db = db;
        }

        internal bool Initialize() => Initialize<TradeInfo>();
        private bool Initialize<T>()
            where T : TradeInfo
        {
            var db = this.Db;

            var imported = db.GetCollection<Data.LocalDb.ImportHistory>();
            var everImportRecord = imported.Query()
                .Where(x => x.Name == typeof(T).Name)
                .OrderByDescending(x => x.Timestamp)
                .FirstOrDefault();
            if (everImportRecord != null)
                return true;

            var tradeFolder = "Trade";
            //沒目錄/檔案=第一次執行，不拋異常
            if (!Directory.Exists(tradeFolder))
                return true;
            if (Directory.GetFiles(tradeFolder, "*.json").Length == 0)
                return true;

            var list = db.GetCollection<T>();
            foreach (var filePath in Directory.GetFiles(tradeFolder, "*.json"))
            {
                var caches = JsonCache.Load<List<T>>(filePath);
                caches.ForEach(cache => cache.Id = ObjectId.NewObjectId());
                list.Insert(caches);
            }
            list.EnsureIndex(d => d.ComCode);

            imported.Insert(new Data.LocalDb.ImportHistory(typeof(T).Name));

            return true;
        }

        public List<TradeInfo> GetAll()
        {
            var db = this.Db;

            var list = db.GetCollection<TradeInfo>();
            var entities = list.Query();
            return entities.ToList();
        }

        public List<string> GetAllComCodes()
        {
            var db = this.Db;

            var list = db.GetCollection<TradeInfo>();
            var codes = list.Query().Select(d => d.ComCode).ToList();
            return codes;
        }

        internal void Updates(List<TradeInfo> updateTrades)
        {
            var db = this.Db;

            var list = db.GetCollection<TradeInfo>();
            foreach (var trade in updateTrades)
            {
                if (trade.Id == ObjectId.Empty)
                {
                    trade.Id = ObjectId.NewObjectId();
                    list.Insert(trade);
                }
                else
                    list.Update(trade);
            }
        }

        internal void Deletes(List<TradeInfo> deleteTrades)
        {
            var db = this.Db;

            var list = db.GetCollection<TradeInfo>();
            foreach (var trade in deleteTrades)
                list.Delete(trade.Id);
        }
    }
}
