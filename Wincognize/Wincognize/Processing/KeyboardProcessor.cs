using System;
using System.Collections.Generic;
using System.Linq;
using Wincognize.Data;

namespace Wincognize.Processing
{
    public class KeyboardProcessor : Processor
    {
        private const int MinResults = 200;

        public KeyboardProcessor() : base(10000) { }

        protected override void Process()
        {
            List<Keyboard> all;
            lock (DataContext.Main)
                all = DataContext.Main.Keyboard.Where(k => k.Timestamp > 0).OrderBy(k => k.Timestamp).ToList(); //Select all data
            if (all.Count() < MinResults)
                return;
            //Filter data for a specified amount of rows
            List<Keyboard> last = all.Skip(Math.Max(0, all.Count() - MinResults)).ToList();
            //Create two lists of time intervals inbetween keyboard actions
            List<int> intervals1 = new List<int>();
            List<int> intervals2 = new List<int>();
            //Fill both lists each based on a different half of the filtered data
            for (int i = 1; i < last.Count(); i++)
                if (i < last.Count() / 2)
                    intervals1.Add(last[i].Timestamp - last[i - 1].Timestamp);
                else
                    intervals2.Add(last[i].Timestamp - last[i - 1].Timestamp);
            //Sort lists
            intervals1.Sort();
            intervals2.Sort();
            //Calculate medians of each list
            double median1 = intervals1[intervals1.Count() / 2];
            double median2 = intervals2[intervals2.Count() / 2];
            //Calculate approximation error based on average of the medians
            Approximation = Math.Min(1, Math.Abs((median2 - median1) / median2));
            Console.WriteLine($"Keyboard: {Approximation}");
        }
    }
}
