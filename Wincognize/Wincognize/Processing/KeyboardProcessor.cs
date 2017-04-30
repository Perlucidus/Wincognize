using System;
using System.Collections.Generic;
using System.Linq;
using Wincognize.Data;

namespace Wincognize.Processing
{
    public class KeyboardProcessor : Processor
    {
        private const int MinResults = 5000;

        public KeyboardProcessor() : base(10000) { }

        protected override void Process()
        {
            List<Keyboard> all;
            lock (DataContext.Main)
                all = DataContext.Main.Keyboard.Where(k => k.Timestamp > 0).OrderBy(k => k.Timestamp).ToList();
            if (all.Count() < MinResults)
                return;
            List<Keyboard> last = all.Skip(Math.Max(0, all.Count() - MinResults)).ToList();
            List<int> intervals1 = new List<int>();
            List<int> intervals2 = new List<int>();
            for (int i = 1; i < last.Count(); i++)
                if (i < last.Count() / 2)
                    intervals1.Add(last[i].Timestamp - last[i - 1].Timestamp);
                else
                    intervals2.Add(last[i].Timestamp - last[i - 1].Timestamp);
            intervals1.Sort();
            intervals2.Sort();
            double median1 = intervals1[intervals1.Count() / 2];
            double median2 = intervals2[intervals2.Count() / 2];
            Approximation = (Math.Max(median1, median2) - Math.Min(median1, median2)) / Math.Max(median1, median2);
            Console.WriteLine($"Keyboard: {Approximation}");
        }
    }
}
