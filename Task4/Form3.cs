using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task4Forms
{
    public partial class Form3 : Form
    {
        static Timer myTimer = new Timer();

        private double k = 0;
        private double passed = 0;

        public Form3()
        {
            myTimer.Enabled = true;
            myTimer.Interval = 5;
            myTimer.Tick += new EventHandler(myTimer_Tick);
            myTimer.Start();
            InitializeComponent();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            myTimer.Stop();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            myTimer.Start();
        }

        private void myTimer_Tick(object sender, EventArgs e)
        {
            Graphics g = panel1.CreateGraphics();

            g.FillRectangle(Brushes.Green, Convert.ToInt32(k), Convert.ToInt32(Math.Round(Math.Sin(passed / 10) * 20)) + 100, 1, 1);

            if (k >= panel1.Width)
            {
                g.Clear(SystemColors.Control);
                k = 0;
            }
            passed++;
            k++;

            g.Dispose();
        }
    }
}
