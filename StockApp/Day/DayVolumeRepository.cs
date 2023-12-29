using Dapper;
using StockApp.Bonus;
using StockApp.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Day
{
    internal class DayVolumeRepository : IPurgeHistory
    {
        private readonly IDbConnection DbConn;

        public DayVolumeRepository(IDbConnection conn)
        {
            this.DbConn = conn;
        }

        internal void Imports(IEnumerable<CompanyDayVolume> entities)
        {
            var conn = this.DbConn;
            foreach (var data in entities)
            {
                conn.Execute(@"
insert into CompanyDayVolume(UpdateAt, ComCode, ComName, ComType, DayVolume, CurrentPrice)
values(:UpdateAt, :ComCode, :ComName, :ComType, :DayVolume, :CurrentPrice)
", data);
            }
        }

        internal CompanyDayVolume GetLatest()
        {
            var conn = this.DbConn;
            var lastDate = conn.ExecuteScalar<DateTime?>("select max(UpdateAt) from CompanyDayVolume");
            if (lastDate.HasValue)
            {
                var data = conn.QueryFirst<CompanyDayVolume>("select * from CompanyDayVolume where UpdateAt=:UpdateAt",
                    new { UpdateAt = lastDate });
                return data;
            }
            return null;
        }

        public List<CompanyDayVolume> GetAll()
        {
            var conn = this.DbConn;
            var lastDate = GetLatest()?.UpdateAt;
            if (!lastDate.HasValue)
                return null;
            var list = conn.Query<CompanyDayVolume>("select * from CompanyDayVolume where UpdateAt=:UpdateAt",
                    new { UpdateAt = lastDate.Value });
            return list.ToList();
        }

        public int PurgeHistory()
        {
            var conn = this.DbConn;
            var lastDate = GetLatest()?.UpdateAt;
            if (!lastDate.HasValue)
                return 0;

            var cnt = conn.Execute("delete from CompanyDayVolume where UpdateAt<:UpdateAt",
                new { UpdateAt = lastDate.Value.AddDays(-7) });
            return cnt;
        }
    }
}
