using System;
using System.Windows.Forms;

namespace Wincognize
{
    static class Program
    {
        public static Context MainContext;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(MainContext = new Context());
        }
    }
}
