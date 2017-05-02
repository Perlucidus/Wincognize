using System;
using System.Collections.Generic;
using System.Linq;
using Wincognize.Data;

namespace Wincognize.Processing
{
    public class BrowsingHistoryProcessor : Processor
    {
        private const int MinResults = 10;

        public BrowsingHistoryProcessor() : base(10000) { }

        protected override void Process()
        {
            List<BrowsingHistory> all;
            lock (DataContext.Main)
                all = DataContext.Main.BrowsingHistory.OrderBy(k => k.VisitTime).ToList();
            if (all.Count() < MinResults)
                return;
            List<BrowsingHistory> last = all.Skip(Math.Max(0, all.Count() - MinResults)).ToList();
            IEnumerable<string> unique = last.GroupBy(h => h.URL).Select(g => g.Key);
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
            foreach (string url in unique)
            {
                int visited1 = visits1.ContainsKey(url) ? visits1[url] : 0;
                int visited2 = visits2.ContainsKey(url) ? visits2[url] : 0;
                approxSum += Math.Abs((visited2 - visited1) / visited2);
            }
            Approximation = Math.Min(1, approxSum / unique.Count());
            Console.WriteLine($"Browsing History: {Approximation}");
        }
    }
}
