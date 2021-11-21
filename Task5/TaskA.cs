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
    public partial class TaskA : UserControl
    {
        public Form1 form1;
        public Form2 form2;
        public Form3 form3;

        public int Turn { get; set; }
        public string Data { get; set; }

        public TaskA(Form1 form1p, Form2 form2p, Form3 form3p)
        {
            Turn = 1;
            form1 = form1p;
            form2 = form2p;
            form3 = form3p;

            Thread[] threadsStart = new Thread[3];
            Thread[] threadsGoing = new Thread[3];

            threadsStart[0] = new Thread(() => { Application.Run(form1); });
            threadsStart[1] = new Thread(() => { Application.Run(form2); });
            threadsStart[2] = new Thread(() => { Application.Run(form3); });

            threadsGoing[0] = new Thread(() => 
            {
                Thread.Sleep(2000);
                var x = Application.OpenForms.Count;
                while (x == 3)
                {
                    if (Turn == 1)
                    {
                        Data = form1.AddMessage(Data);
                        Turn = 2;
                    }
                    x = Application.OpenForms.Count;
                }
                if (Application.OpenForms["Form1"] != null)
                    form1.ClosingForm();
            });

            threadsGoing[1] = new Thread(() => 
            {
                Thread.Sleep(2000);
                var x = Application.OpenForms.Count;
                while (x == 3)
                {
                    if (Turn == 2)
                    {
                        Data = form2.AddMessage(Data);
                        Turn = 3;
                    }
                    x = Application.OpenForms.Count;
                }
                if (Application.OpenForms["Form2"] != null)
                    form2.ClosingForm();
            });

            threadsGoing[2] = new Thread(() =>
            {
                Thread.Sleep(2000);
                var x = Application.OpenForms.Count;
                while (x == 3)
                {
                    if (Turn == 3)
                    {
                        Data = form3.AddMessage(Data);
                        Turn = 1;
                    }
                    x = Application.OpenForms.Count;
                }
                if (Application.OpenForms["Form3"] != null)
                    form3.ClosingForm();
            });

            foreach (var x in threadsStart)
                x.Start();
            foreach (var x in threadsGoing)
                x.Start();
            foreach (var x in threadsGoing)
                x.Join();
            foreach (var x in threadsStart)
                x.Join();
            
        }
    }
}
