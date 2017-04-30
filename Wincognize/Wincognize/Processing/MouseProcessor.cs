using System;
using System.Collections.Generic;
using System.Linq;
using Wincognize.Data;
using Wincognize.Hooking.Mouse;

namespace Wincognize.Processing
{
    public class MouseProcessor : Processor
    {
        private const int MinResults = 5000;

        public MouseProcessor() : base(10000) { }

        protected override void Process()
        {
            List<Mouse> all;
            lock (DataContext.Main)
            {
                all = DataContext.Main.Mouse
                    .Where(k => k.Timestamp > 0 && k.Action == (int)MouseAction.WM_MOUSEMOVE)
                    .OrderBy(k => k.Timestamp).ToList();
            }
            if (all.Count() < MinResults)
                return;
            List<Mouse> last = all.Skip(Math.Max(0, all.Count() - MinResults)).ToList();
            List<double> distances1 = new List<double>();
            List<double> distances2 = new List<double>();
            for (int i = 1; i < last.Count(); i++)
                if (i < last.Count() / 2)
                    distances1.Add(last[i].Timestamp - last[i - 1].Timestamp);
                else
                    distances2.Add(last[i].Timestamp - last[i - 1].Timestamp);
            distances1.Sort();
            distances2.Sort();
            double median1 = distances1[distances1.Count() / 2];
            double median2 = distances2[distances2.Count() / 2];
            Approximation = (Math.Max(median1, median2) - Math.Min(median1, median2)) / Math.Max(median1, median2);
            Console.WriteLine($"Mouse: {Approximation}");
        }

        private double Distance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }
    }
}
