using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoFiller
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            bool result;
            var mutex = new System.Threading.Mutex(true, "AutoFillerMutex", out result); // check if already running

            if (!result)
            {
                return; // if already running, exit
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            GC.KeepAlive(mutex);
        }
    }
}
