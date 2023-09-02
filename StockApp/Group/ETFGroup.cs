using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Group
{
    class ETFGroup : CustomGroup
    {
        public override bool IsFavorite => false;
        public override int SortIndex => (int)GroupType.ETFGroup;
        public override GroupType Group => GroupType.ETFGroup;
    }
}
