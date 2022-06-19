using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class BasicSetting
    {
        static BasicSetting _Instance;
        public static BasicSetting Instance => System.Threading.LazyInitializer.EnsureInitialized(ref _Instance, () =>
        {
            var ins = new BasicSetting();
            ins.Load();
            return ins;
        });
        public decimal PriceLimit { get; private set; }
        public int ContBonusTimesLimit { get; private set; }
        public int SimulateMaxMonths { get; internal set; }

        public void Load()
        {
            this.PriceLimit = Properties.Settings.Default.PriceLimit;
            this.ContBonusTimesLimit = Properties.Settings.Default.ContBonusTimesLimit;
            this.SimulateMaxMonths = Properties.Settings.Default.SimulateMaxMonth;
        }
    }
}
