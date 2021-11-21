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
    public partial class Form4 : Form
    {
        private bool Forward = true;

        static Timer myTimer = new Timer();

        private int k = 0;
        private float ang = 0;
        public Form4()
        {
            myTimer.Enabled = true;
            myTimer.Interval = 10;
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
            ang += (float)3;
            Graphics g = panel1.CreateGraphics();
            Pen pen = new Pen(Color.Blue, 1);

            g.Clear(Color.White);
            
            g.TranslateTransform(panel1.Width / 2, panel1.Height / 2);
            g.RotateTransform(ang);
            g.DrawLine(pen, 0, 0, 100, 100);

            g.Dispose();
            pen.Dispose();
        }
    }
}
