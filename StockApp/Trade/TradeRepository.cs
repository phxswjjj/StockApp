using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Trade
{
    internal class TradeRepository
    {
        private readonly IDbConnection DbConn;

        public TradeRepository(IDbConnection conn)
        {
            this.DbConn = conn;
        }

        public List<TradeInfo> GetAll()
        {
            var conn = this.DbConn;
            var list = conn.Query<TradeInfo>("select * from TradeInfo");
            return list.ToList();
        }

        public List<string> GetAllComCodes()
        {
            var conn = this.DbConn;
            var codes = conn.Query<string>("select distinct ComCode from TradeInfo");
            return codes.ToList();
        }

        internal void Updates(List<TradeInfo> updateTrades)
        {
            var conn = this.DbConn;
            foreach (var trade in updateTrades)
            {
                if (!string.IsNullOrEmpty(trade.SysId))
                    conn.Execute("delete from TradeInfo where SysId=:SysId",
                        new { SysId = trade.SysId });
                else
                    trade.SysId = Guid.NewGuid().ToString("N");
                conn.Execute(@"
insert into TradeInfo(SysId, ComCode, TradeDate, TradePrice, TradeVolume, StockCenterName, Memo)
values(:SysId, :ComCode, :TradeDate, :TradePrice, :TradeVolume, :StockCenterName, :Memo)
", trade);
            }
        }

        internal void Deletes(List<TradeInfo> deleteTrades)
        {
            var conn = this.DbConn;
            foreach (var trade in deleteTrades)
            {
                conn.Execute("delete from TradeInfo where SysId=:SysId",
                    new { SysId = trade.SysId });
            }
        }
    }
}
