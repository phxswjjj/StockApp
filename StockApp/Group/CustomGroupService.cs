using MongoDB.Driver;
using StockApp.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace StockApp.Group
{
    internal class CustomGroupService
    {
        private readonly IMongoCollection<CustomGroup> Collection;

        public CustomGroupService(IMongoDatabase db)
        {
            this.Collection = db.GetCollection<CustomGroup>("CustomGroup");
        }

        internal void Save(CustomGroup group)
        {
            var doc = this.Collection;

            if (group.ComCodes.Count > 0 || group.IsFavorite)
                this.InsertOrUpdate(group);
            else
                this.Delete(group.Name);
        }

        private void InsertOrUpdate(CustomGroup customGroup)
        {
            var doc = this.Collection;
            doc.ReplaceOne(d => d.Name == customGroup.Name,
                customGroup,
                new ReplaceOptions() { IsUpsert = true });
        }
        private void Delete(string name)
        {
            var doc = this.Collection;
            doc.DeleteOne(d => d.Name == name);
        }
    }
}
