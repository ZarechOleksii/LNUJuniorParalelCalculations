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
    public partial class Form2 : Form
    {
        private bool Forward = true;

        private Timer myTimer = new Timer();

        private int k = 0;

        public Form2()
        {
            myTimer.Interval = 20;
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
            Pen pen = new Pen(Color.Black, 1);

            g.Clear(Color.White);
            g.DrawRectangle(pen, 0, 0, k, k);

            if (k >= panel1.Height)
                Forward = false;
            else if (k == 0)
                Forward = true;
            if (Forward)
                k++;
            else
                k--;

            g.Dispose();
            pen.Dispose();
        }
    }
}
