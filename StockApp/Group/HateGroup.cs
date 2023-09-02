using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Group
{
    class HateGroup : CustomGroup
    {
        public override int SortIndex => (int)GroupType.HateGroup;

        public override string Name => "排除清單";
        public override GroupType Group => GroupType.HateGroup;
    }
}
