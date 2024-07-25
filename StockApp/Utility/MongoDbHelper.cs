using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Utility
{
    internal class MongoDbHelper
    {
        public static IMongoDatabase Create()
        {
            var connStr = System.Configuration.ConfigurationManager.AppSettings["mongodb-conn"];
            var mongoClient = new MongoClient(connStr);
            var db = mongoClient.GetDatabase("Stock");
            return db;
        }
    }
}
