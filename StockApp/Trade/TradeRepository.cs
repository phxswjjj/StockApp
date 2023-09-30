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
                this.Imports(caches);
            }
            list.EnsureIndex(d => d.ComCode);

            imported.Insert(new Data.LocalDb.ImportHistory(typeof(T).Name));

            return true;
        }

        internal void Imports(IEnumerable<TradeInfo> entities)
        {
            var db = this.Db;

            var list = db.GetCollection<TradeInfo>();
            list.Insert(entities);
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

        public void Updates(string comCode, IEnumerable<TradeInfo> trades)
        {
            var db = this.Db;

            foreach (var trade in trades)
            {
                if (trade.ComCode != comCode)
                    throw new Exception($"Batch Updates 只能更新 {comCode}，但含有 {trade.ComCode}");
            }

            var list = db.GetCollection<TradeInfo>();
            list.DeleteMany(x => x.ComCode == comCode);
            Imports(trades);
        }
    }
}
