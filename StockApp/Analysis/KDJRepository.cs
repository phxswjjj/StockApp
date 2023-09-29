using LiteDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Analysis
{
    internal class KDJRepository
    {
        private readonly ILiteDatabase Db;

        public KDJRepository(ILiteDatabase db)
        {
            this.Db = db;
        }

        internal void Imports(IEnumerable<CompanyKDJ> entities)
        {
            var db = this.Db;

            var list = db.GetCollection<CompanyKDJ>();
            list.Insert(entities);
        }

        internal CompanyKDJ GetLatest()
        {
            var db = this.Db;

            var list = db.GetCollection<CompanyKDJ>();
            var data = list.Query()
                .OrderByDescending(x => x.UpdateAt)
                .FirstOrDefault();
            return data;
        }

        public List<CompanyKDJ> GetAll()
        {
            var db = this.Db;

            var latest = GetLatest();
            if (latest == null)
                return null;
            var list = db.GetCollection<CompanyKDJ>();
            var entities = list.Query()
                .Where(x => x.UpdateAt == latest.UpdateAt);
            return entities.ToList();
        }
    }
}
