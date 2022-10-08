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
        public DisplayModel.KDJRangeType KDJRange { get; private set; }
        public int TraceDays { get; private set; }

        public void Load()
        {
            var setting = Properties.Settings.Default;

            this.PriceLimit = setting.PriceLimit;
            this.ContBonusTimesLimit = setting.ContBonusTimesLimit;
            this.SimulateMaxMonths = setting.SimulateMaxMonth;
            this.KDJRange = (DisplayModel.KDJRangeType)Properties.Settings.Default.KDJRange;
            this.TraceDays = setting.TraceDays;
        }
    }
}
