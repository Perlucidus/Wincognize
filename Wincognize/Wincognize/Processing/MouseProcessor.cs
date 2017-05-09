using System;
using System.Collections.Generic;
using System.Linq;
using Wincognize.Data;
using Wincognize.Hooking.Mouse;

namespace Wincognize.Processing
{
    public class MouseProcessor : Processor
    {
        private const int MinResults = 200;

        public MouseProcessor() : base(10000) { }

        protected override void Process()
        {
            List<Mouse> all;
            lock (DataContext.Main)
            {
                //Select data of mouse movements only
                all = DataContext.Main.Mouse
                    .Where(k => k.Action == (int)MouseAction.WM_MOUSEMOVE)
                    .OrderBy(k => k.Timestamp).ToList();
            }
            if (all.Count() < MinResults)
                return;
            //Filter data for a specified amount of rows
            List<Mouse> last = all.Skip(Math.Max(0, all.Count() - MinResults)).ToList();
            //Create two lists of time intervals inbetween mouse movements
            List<int> distances1 = new List<int>();
            List<int> distances2 = new List<int>();
            //Fill both lists each based on a different half of the filtered data
            for (int i = 1; i < last.Count(); i++)
                if (i < last.Count() / 2)
                    distances1.Add(last[i].Timestamp - last[i - 1].Timestamp);
                else
                    distances2.Add(last[i].Timestamp - last[i - 1].Timestamp);
            //Sort lists
            distances1.Sort();
            distances2.Sort();
            //Calculate medians of each list
            double median1 = distances1[distances1.Count() / 2];
            double median2 = distances2[distances2.Count() / 2];
            //Calculate approximation error based on average of the medians
            Approximation = Math.Min(1, Math.Abs((median2 - median1) / median2));
            Console.WriteLine($"Mouse: {Approximation}");
        }
    }
}
