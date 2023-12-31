using Dapper;
using StockApp.Trace;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static StockApp.Group.CustomGroup;

namespace StockApp.Group
{
    internal class CustomGroupRepository
    {
        const string GROUP_NAME_TRACE_STOCK = "追蹤價格";

        private readonly IDbConnection DbConn;

        public CustomGroupRepository(IDbConnection conn)
        {
            this.DbConn = conn;
        }

        public List<CustomGroup> GetAll()
        {
            var conn = this.DbConn;
            var list = conn.Query<CustomGroup>("select * from CustomGroup").ToList();
            foreach (var data in list)
                LoadComCodes(data);
            return list;
        }

        internal void Updates(List<CustomGroup> customGroups)
        {
            foreach (var customGroup in customGroups)
                Update(customGroup);
        }
        private void Update(CustomGroup customGroup)
        {
            var conn = this.DbConn;

            conn.Execute(@"delete from CustomGroup where Name=:Name",
                new { Name = customGroup.Name });
            conn.Execute(@"delete from CustomGroupComCode where GroupName=:GroupName",
                                new { GroupName = customGroup.Name });

            //系統定義群組(IsFavorite=false)沒有內容也要保留群組
            if (customGroup.ComCodes.Count > 0 || !customGroup.IsFavorite)
            {
                conn.Execute("insert into CustomGroup(Name, IsFavorite, GroupTypeName) values(:Name, :IsFavorite, :GroupTypeName)",
                    customGroup);
                foreach (var comCode in customGroup.ComCodes)
                {
                    conn.Execute("insert into CustomGroupComCode(GroupName, ComCode) values(:GroupName, :ComCode)",
                        new { GroupName = customGroup.Name, ComCode = comCode });
                }
            }
        }
        public CustomGroup GetGroupByName(string groupName)
        {
            var conn = this.DbConn;
            var data = conn.QueryFirstOrDefault<CustomGroup>("select * from CustomGroup where Name=:Name",
                new { Name = groupName });
            if (data != null)
                LoadComCodes(data);
            return data;
        }
        public void LoadComCodes(CustomGroup group)
        {
            var conn = this.DbConn;
            var codes = conn.Query<string>("select ComCode from CustomGroupComCode where GroupName=:GroupName",
                new { GroupName = group.Name }).ToList();
            group.ComCodes = codes;
        }

        public bool AddTraceStock(StockDetail stock)
        {
            var existsGroup = GetGroupByName(GROUP_NAME_TRACE_STOCK);
            if (existsGroup == null)
            {
                var newGroup = new TraceGroup()
                {
                    Name = GROUP_NAME_TRACE_STOCK,
                    ComCodes = new List<string> { stock.ComCode },
                    Group = GroupType.TraceGroup,
                };
                Update(newGroup);
            }
            else
            {
                if (!existsGroup.ComCodes.Contains(stock.ComCode))
                {
                    existsGroup.ComCodes.Add(stock.ComCode);
                    Update(existsGroup);
                }
            }
            return true;
        }

        internal bool DeleteTraceStock(StockDetail stock)
        {
            var existsGroup = GetGroupByName(GROUP_NAME_TRACE_STOCK);
            if (existsGroup != null)
            {
                if (existsGroup.ComCodes.Contains(stock.ComCode))
                {
                    existsGroup.ComCodes.Remove(stock.ComCode);
                    Update(existsGroup);
                    return true;
                }
            }
            return false;
        }
    }
}
