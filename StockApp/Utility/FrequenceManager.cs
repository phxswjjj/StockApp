using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Utility
{
    internal class FrequenceManager
    {
        internal DateTime? NextTime { get; private set; }
        private readonly object ExecuteLockObject = new object();
        private readonly List<IFrequenceRule> Rules = new List<IFrequenceRule>();
        private FixedSizeQueue<DateTime> Histories = new FixedSizeQueue<DateTime>(100);

        public FrequenceManager()
        {
            this.Rules.Add(new MinuteRule());
        }

        public bool Execute()
        {
            lock (ExecuteLockObject)
            {
                if (this.NextTime.HasValue)
                {
                    var diff = this.NextTime.Value - DateTime.Now;
                    if (diff.TotalMilliseconds > 0)
                        return false;
                }
                foreach (var rule in this.Rules)
                {
                    var nextTime = rule.NextTime(this.Histories);
                    if (nextTime != null)
                    {
                        if (!this.NextTime.HasValue || this.NextTime.Value < nextTime)
                            this.NextTime = nextTime;
                        var diff = this.NextTime.Value - DateTime.Now;
                        if (diff.TotalMilliseconds > 0)
                            return false;
                    }
                }
                this.Histories.Push(DateTime.Now);
                return true;
            }
        }

        private interface IFrequenceRule
        {
            DateTime? NextTime(FixedSizeQueue<DateTime> histories);
        }

        private class MinuteRule : IFrequenceRule
        {
            const int Minutes = 1;
            const int MaxCount = 4;

            public DateTime? NextTime(FixedSizeQueue<DateTime> histories)
            {
                if (histories.Count < MaxCount)
                    return null;
                var recents = histories.Latest(DateTime.Now.AddMinutes(-Minutes));
                if (recents.Count() >= MaxCount)
                    return recents.Min().AddMinutes(Minutes);
                else
                    return null;
            }
        }
    }
}
