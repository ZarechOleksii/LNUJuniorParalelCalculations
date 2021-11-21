using System;
using System.Collections.Concurrent;
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
    public partial class TaskB : UserControl
    {
        private readonly ConcurrentQueue<string> jobs = new();

        public Form1 form1;
        public Form2 form2;
        public Form3 form3;

        public TaskB(Form1 form1p, Form2 form2p, Form3 form3p)
        {
            jobs.Enqueue("");

            form1 = form1p;
            form2 = form2p;
            form3 = form3p;

            IFormInterface[] forms = new IFormInterface[] { form1, form2, form3 };
            int threads = forms.Length;

            Thread[] threadsStart = new Thread[3];
            Thread[] threadsGoing = new Thread[3];
            
            for (int i = 0; i < threads; i++)
            {
                int index = i;
                threadsStart[i] = new Thread(() => { Application.Run((Form)forms[index]); });
                threadsGoing[i] = new Thread(() =>
                {
                    Thread.Sleep(2000);
                    var x = Application.OpenForms.Count;
                    while (x == 3)
                    {
                        if (jobs.TryDequeue(out string result))
                        {
                            jobs.Enqueue(forms[index].AddMessage(result));
                        }
                        x = Application.OpenForms.Count;
                    }
                    if (Application.OpenForms[((Form)forms[index]).Name] != null)
                        forms[index].ClosingForm();
                });
            }
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
