using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Group
{
    class HateGroup : CustomGroup
    {
        public override int SortIndex => (int)DefaultSortIndexType.HateGroup;
    }
}
