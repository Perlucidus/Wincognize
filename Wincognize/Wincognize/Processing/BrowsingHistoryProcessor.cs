using System;
using System.Collections.Generic;
using System.Linq;
using Wincognize.Data;

namespace Wincognize.Processing
{
    public class BrowsingHistoryProcessor : Processor
    {
        private const int MinResults = 3000;

        public BrowsingHistoryProcessor() : base(10000) { }

        protected override void Process()
        {
            List<BrowsingHistory> all;
            lock (DataContext.Main)
                all = DataContext.Main.BrowsingHistory.Where(k => k.VisitTime > 0).OrderBy(k => k.VisitTime).ToList();
            if (all.Count() < MinResults)
                return;
            List<BrowsingHistory> last = all.Skip(Math.Max(0, all.Count() - MinResults)).ToList();
            Dictionary<string, int> visits1 = new Dictionary<string, int>();
            Dictionary<string, int> visits2 = new Dictionary<string, int>();
            for (int i = 1; i < last.Count(); i++)
            {
                if (i < last.Count() / 2)
                {
                    if (!visits1.ContainsKey(last[i].URL))
                        visits1.Add(last[i].URL, 1);
                    else
                        visits1[last[i].URL]++;
                }
                else
                {
                    if (!visits2.ContainsKey(last[i].URL))
                        visits2.Add(last[i].URL, 1);
                    else
                        visits2[last[i].URL]++;
                }
            }
            double approxSum = 0;
            foreach (BrowsingHistory history in last)
            {
                int visited1 = visits1.ContainsKey(history.URL) ? visits1[history.URL] : 0;
                int visited2 = visits2.ContainsKey(history.URL) ? visits2[history.URL] : 0;
                approxSum += (Math.Max(visited1, visited2) - Math.Min(visited1, visited2)) / Math.Max(visited1, visited2);
            }
            Approximation = approxSum / last.Count();
        }
    }
}
