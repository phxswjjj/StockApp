using Dapper;
using StockApp.Group;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.ROE
{
    internal class ROERepository
    {
        private readonly IDbConnection DbConn;

        public ROERepository(IDbConnection conn)
        {
            this.DbConn = conn;
        }

        /// <summary>
        /// 取得最新、任一資料
        /// </summary>
        /// <returns></returns>
        internal CompanyROE GetROELatestAny()
        {
            var conn = this.DbConn;
            var latestDate = GetROELatestDate();
            var anyComCode = conn.QueryFirstOrDefault<string>("select ComCode from CompanyROE where UpdateAt=:UpdateAt limit 1",
                new { UpdateAt = latestDate.Value });
            if (string.IsNullOrEmpty(anyComCode))
                return null;
            return GetROE(anyComCode);
        }
        internal DateTime? GetROELatestDate()
            => this.DbConn.ExecuteScalar<DateTime?>("select max(UpdateAt) from CompanyROE");

        internal CompanyROE GetROE(string comCode)
        {
            var conn = this.DbConn;
            var entities = conn.Query<CompanyROE.Entity>("select * from CompanyROE where ComCode=:ComCode",
                new { ComCode = comCode });
            var result = entities.GroupBy(entity => entity.ComCode)
                .Select(g =>
                {
                    var data = new CompanyROE()
                    {
                        ComCode = g.Key,
                    };
                    foreach (var roeItem in g.ToList())
                    {
                        data.ROEHeaders.Add(roeItem.ROEHeader);
                        data.ROEValues.Add(roeItem.ROEValue);
                    }
                    return data;
                }).FirstOrDefault();
            return result;
        }

        internal void Imports(IEnumerable<CompanyROE> entities)
        {
            var conn = this.DbConn;
            foreach (var data in entities)
            {
                conn.Execute("delete from CompanyROE where ComCode=:ComCode",
                    new { ComCode = data.ComCode });
                if (data.ROEHeaders.Count == 0)
                    continue;

                for (var headerIndex = 0; headerIndex < data.ROEHeaders.Count; headerIndex++)
                {
                    var header = data.ROEHeaders[headerIndex];
                    var roeValue = data.ROEValues[headerIndex];
                    conn.Execute(@"
insert into CompanyROE(UpdateAt, ComCode, ROEHeader, ROEValue)
values(:UpdateAt, :ComCode, :ROEHeader, :ROEValue)
", new { UpdateAt = data.UpdateAt, ComCode = data.ComCode, ROEHeader = header, ROEValue = roeValue });
                }
            }
        }
    }
}
