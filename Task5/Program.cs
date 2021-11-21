using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TASK5
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

            Form1 form1 = new();
            Form2 form2 = new();
            Form3 form3 = new();

            new TaskA(form1, form2, form3);

            form1 = new();
            form2 = new();
            form3 = new();

            new TaskB(form1, form2, form3);
        }
    }
}
