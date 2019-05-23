using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OS_Assignment
{
    public partial class frmMultiple_Queue : Form
    {
        public frmMultiple_Queue()
        {
            InitializeComponent();
        }
        //Timer word gemaak
        Timer t = new Timer();
        int counter;
        int minute = 0;
        String timeStop;
        int Pcount = 0;

        //Variables vir BurstTime en arrivalTime
        int[] burstTime = new int[100];
        int[] arrivalTime = new int[100];
        string[] process = new string[100];

        //Arrays vir burst en Arrival se labels word geskep
        int burstCount;
        Label[] lblBurst = new Label[3];
        Label[] lblArrival = new Label[3];

        //Arrays vir die 3 prosesse word geskep
        int arr;
        string temp;
        Button[] btnoneProcess = new Button[10];
        Button[] btntwoProcess = new Button[10];
        Button[] btnthreeProcess = new Button[10];
        private void gbProcess_Enter(object sender, EventArgs e)
        {

        }

        private void StartTimeTick(object sender, EventArgs e)
        {

            counter++;
            if (counter < 10)
            {
                lblTime.Text = "0" + minute + ":0" + counter.ToString();
            }
            else if (counter < 10 || counter < 60)
            {
                lblTime.Text = "0" + minute + ":" + counter.ToString();
            }
            else
            {
                minute++;
                counter = 0;
                lblTime.Text = "0" + minute + ":0" + counter.ToString();

            }

            for (int a = 0; a < Pcount; a++)
            {

                if (counter % 2 == 0)
                {
                    if (arrivalTime[a] <= counter)
                    {
                        burstTime[a] = burstTime[a] - 2;
                        arrivalTime[a] = arrivalTime[a] + 2;
                        if (process[a] == "P1" && Val1 < 10)
                        {
                            btnoneProcess[Val1].Visible = true;
                            Val1++;

                        }
                    }
                }
                for (int i = 0; i < Pcount; i++)
                {
                    for (int j = i + 1; j < Pcount; j++)
                    {
                        if (arrivalTime[j] < arrivalTime[i])
                        {
                            arr = arrivalTime[j];
                            arrivalTime[j] = arrivalTime[i];
                            arrivalTime[i] = arr;

                            arr = burstTime[j];
                            burstTime[j] = burstTime[i];
                            burstTime[i] = arr;

                            temp = process[j];
                            process[j] = process[i];
                            process[i] = temp;
                        }
                    }
                }
            }
        }

        private void InitializeTimer()
        {
            counter = 0;
            t.Interval = 1000;
            t.Enabled = true;
            StartTimeTick(null, null);
            t.Tick += new EventHandler(StartTimeTick);
        }

        private void rgbPriority_CheckedChanged(object sender, EventArgs e)
        {
            //Priority form shown
            frmPriority frmPrior = new frmPriority();
            frmPrior.Visible = true;
            //Multiple Queue form hidden
            frmMultiple_Queue frmMQ = new frmMultiple_Queue();
            frmMQ.Visible = false; 
        }

        private void radRR_CheckedChanged(object sender, EventArgs e)
        {
            //Multiple Queue form shown
            frmMultiple_Queue frmMQ = new frmMultiple_Queue();
            frmMQ.Visible = false;
            //Hide RR form
            Form1 frmRR = new Form1();
            frmRR.Visible = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Pcount++;
            if (Pcount == 1)
            {
                burstCount++;
                burstTime[0] = Convert.ToInt16(txtTimeProcess.Text);
                arrivalTime[0] = Convert.ToInt16(txtStartTime.Text);
                process[0] = "P" + Pcount;
            }
            else
            {
                burstTime[burstCount] = Convert.ToInt16(txtTimeProcess.Text);
                arrivalTime[burstCount] = Convert.ToInt16(txtStartTime.Text);
                process[burstCount] = "P" + Pcount;
                burstCount++;

            }
            if (Pcount == 1)
            {
                lblBurst[0].Text = Convert.ToString(burstTime[0]);
                lblArrival[0].Text = Convert.ToString(arrivalTime[0]);
            }
            else if (Pcount > 1 && Pcount < 4)
            {
                lblBurst[Pcount - 1].Text = Convert.ToString(burstTime[Pcount - 1]);
                lblArrival[Pcount - 1].Text = Convert.ToString(arrivalTime[Pcount - 1]);
            }

            //Text word op richedit geadd
            if (rtxtShow.Text == "")
            {
                rtxtShow.Text = "Queued Process:    " + "Running Process:     " + "Finished Process:    ";
                rtxtShow.Text += "\n            P" + Pcount;
            }
            else
            {
                rtxtShow.Text += "\n            P" + Pcount;
            }

            
        }

        int tempProses;
        private void MultipleQ()
        {
            //Multiple queues
            int[] ProsesNommer = new int[3];
            ProsesNommer[0] = 1;
            ProsesNommer[1] = 2;
            ProsesNommer[2] = 3;

            if (radMQ.Checked == true)
            {
                for (int i = 0; i < Pcount; i++)
                    for (int j = i + 1; j < Pcount; j++)
                    {
                        if (arrivalTime[j] < arrivalTime[i])
                        {
                            arr = arrivalTime[j];
                            arrivalTime[j] = arrivalTime[i];
                            arrivalTime[i] = arr;

                            arr = burstTime[j];
                            burstTime[j] = burstTime[i];
                            burstTime[i] = arr;

                            temp = process[j];
                            process[j] = process[i];
                            process[i] = temp;

                            tempProses = ProsesNommer[j];
                            ProsesNommer[j] = ProsesNommer[i];
                            ProsesNommer[i] = tempProses;
                        }
                    }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)//Start the processes
        {
            MultipleQ();
            tmrProcessTime.Start();
        }

        int Val1 = 0;

        private void btnStart_Click_1(object sender, EventArgs e)
        {
            counter++;
            if (counter < 10)
            {
                lblTime.Text = "0" + minute + ":0" + counter.ToString();
            }
            else if (counter < 10 || counter < 60)
            {
                lblTime.Text = "0" + minute + ":" + counter.ToString();
            }
            else
            {
                minute++;
                counter = 0;
                lblTime.Text = "0" + minute + ":0" + counter.ToString();

            }

            for (int a = 0; a < Pcount; a++)
            {

                if (counter % 2 == 0)
                {
                    if (arrivalTime[a] <= counter)
                    {
                        burstTime[a] = burstTime[a] - 2;
                        arrivalTime[a] = arrivalTime[a] + 2;
                        if (process[a] == "P1" && Val1 < 10)
                        {
                            btnoneProcess[Val1].Visible = true;
                            Val1++;

                        }
                    }
                }
                for (int i = 0; i < Pcount; i++)
                {
                    for (int j = i + 1; j < Pcount; j++)
                    {
                        if (arrivalTime[j] < arrivalTime[i])
                        {
                            arr = arrivalTime[j];
                            arrivalTime[j] = arrivalTime[i];
                            arrivalTime[i] = arr;

                            arr = burstTime[j];
                            burstTime[j] = burstTime[i];
                            burstTime[i] = arr;

                            temp = process[j];
                            process[j] = process[i];
                            process[i] = temp;
                        }
                    }
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timeStop = lblTime.Text;
            tmrProcessTime.Stop();
        }

        private void btnMinimized_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;//Make the window minimized
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();//Close the Program
        }
    }
}
