using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace StockApp.Analysis
{
    internal class KDJRepository
    {
        private readonly IDbConnection DbConn;

        public KDJRepository(IDbConnection conn)
        {
            this.DbConn = conn;
        }

        internal void Imports(IEnumerable<CompanyKDJ> entities)
        {
            var conn = this.DbConn;
            foreach (var data in entities)
            {
                conn.Execute(@"
insert into CompanyKDJ(UpdateAt, ComCode, ComName, DayK, DayD, DayJ, WeekK, WeekD, WeekJ, MonthK, MonthD, MonthJ)
values(:UpdateAt, :ComCode, :ComName, :DayK, :DayD, :DayJ, :WeekK, :WeekD, :WeekJ, :MonthK, :MonthD, :MonthJ)
", data);
            }
        }

        internal CompanyKDJ GetLatest()
        {
            var conn = this.DbConn;
            var latestDate = conn.ExecuteScalar<DateTime?>("select max(UpdateAt) from CompanyKDJ");
            if (!latestDate.HasValue)
                return null;
            var data = conn.QueryFirstOrDefault<CompanyKDJ>("select * from CompanyKDJ where UpdateAt=:UpdateAt",
                new { UpdateAt = latestDate });
            return data;
        }

        public List<CompanyKDJ> GetAll()
        {
            var conn = this.DbConn;
            var latest = GetLatest();
            if (latest == null)
                return null;
            var list = conn.Query<CompanyKDJ>("select * from CompanyKDJ where UpdateAt=:UpdateAt",
                new { UpdateAt = latest.UpdateAt });
            return list.ToList();
        }
    }
}
