﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Group
{
    class FavoriteGroup : CustomGroup
    {
        public override int SortIndex => (int)GroupType.FavoriteGroup;

        public override string Name => "觀察清單";
        public override GroupType Group => GroupType.FavoriteGroup;
    }
}
