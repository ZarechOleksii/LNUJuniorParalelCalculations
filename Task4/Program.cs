using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task4Forms
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Thread[] threads = new Thread[4];

            threads[0] = new Thread(() => { Application.Run(new Form1()); });
            threads[1] = new Thread(() => { Application.Run(new Form2()); });
            threads[2] = new Thread(() => { Application.Run(new Form3()); });
            threads[3] = new Thread(() => { Application.Run(new Form4()); });

            foreach (var x in threads)
                x.Start();
            foreach (var x in threads)
                x.Join();
        }
    }
}
