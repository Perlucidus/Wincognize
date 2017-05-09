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
                all = DataContext.Main.BrowsingHistory.OrderBy(k => k.VisitTime).ToList(); //Select all data
            if (all.Count() < MinResults)
                return;
            //Filter data for a specified amount of rows
            List<BrowsingHistory> last = all.Skip(Math.Max(0, all.Count() - MinResults)).ToList();
            //Create a set of website URLs
            IEnumerable<string> unique = last.GroupBy(h => h.URL).Select(g => g.Key);
            //Create two dictionaries of URL and the amount of visits to that URL
            Dictionary<string, int> visits1 = new Dictionary<string, int>();
            Dictionary<string, int> visits2 = new Dictionary<string, int>();
            //Fill both dictionaries each based on a different half of the filtered data
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
            //Calculate approximation error for each dictionary entry
            double approxSum = 0;
            foreach (string url in unique)
            {
                int visited1 = visits1.ContainsKey(url) ? visits1[url] : 0;
                int visited2 = visits2.ContainsKey(url) ? visits2[url] : 0;
                approxSum += visited2 == 0 ? 1 : Math.Abs((visited2 - visited1) / visited2);
            }
            //Calculate final approximation error based on average of previous approximation errors
            Approximation = Math.Min(1, approxSum / unique.Count());
            Console.WriteLine($"Browsing History: {Approximation}");
        }
    }
}
