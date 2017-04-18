using System.Linq;
using Wincognize.Data;

namespace Wincognize.Processing
{
    public class KeyboardProcessor : Processor
    {
        public KeyboardProcessor() : base(10000) { }

        protected override void Process()
        {
            foreach (Keyboard keyboard in DataContext.Main.Keyboard.Where(k => k.Timestamp > 0).OrderByDescending(k => k.Timestamp))
                System.Console.WriteLine(System.Enum.GetName(typeof(Hooking.Keyboard.KeyboardAction), keyboard.Action));
        }
    }
}
