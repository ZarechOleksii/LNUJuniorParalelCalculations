using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TASK5
{
    public partial class Form1 : Form, IFormInterface
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string AddMessage(string data)
        {
            try
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    InputDataLabel.Text = data;
                    WaitingLabel.Visible = false;
                    WorkingLabel.Visible = true;
                }));

                Thread.Sleep(300);

                Invoke(new MethodInvoker(delegate ()
                {
                    data += "1";
                    ResultDataLabel.Text = data;
                    WaitingLabel.Visible = true;
                    WorkingLabel.Visible = false;
                }));
            }
            catch { }
            return data;
        }
        public void ClosingForm()
        {
            Invoke(new MethodInvoker(delegate ()
            {
                Close();
            }));
        }
    }
}
