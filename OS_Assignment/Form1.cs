
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
    public partial class Form1 : Form
    {
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
        int burstCount = 0;
        Label[] lblBurst = new Label[3];
        Label[] lblArrival = new Label[3];

        //Arrays vir die 3 prosesse word geskep
        int arr;
        string temp;
        Button[] btnoneProcess = new Button[10];
        Button[] btntwoProcess = new Button[10];
        Button[] btnthreeProcess = new Button[10];

        //Array vir progressbar
        ProgressBar[] arrPrg = new ProgressBar[3];
        public Form1()
        {
            InitializeComponent();
            //load Labels in 'n label array vir die stats wat val onder BT 
            lblBurst[0] = lbl1RT;
            lblBurst[1] = lbl2RT;
            lblBurst[2] = lbl3RT;

            //load labels in 'n label array vir die stats wat val onder AT
            lblArrival[0] = lbl1WT;
            lblArrival[1] = lbl2WT;
            lblArrival[2] = lbl3WT;


            prgOne.Value = 0;
            prgTwo.Value = 0;
            prgThree.Value = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();//Close the Program
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;//Make the window minimized
        }
        //Variables for the length of the processes that the user add
        int iLengthOfProcess1 = 0;
        int iLengthOfProcess2 = 0;
        int iLengthOfProcess3 = 0;

        //Time the Processes starts of which the user adds
        int iStartTime1 = -1;
        int iStartTime2 = -1;
        int iStartTime3 = -1;

        /*Wanneer die button gekliek word, word nog 'n proses geadd en wanneer die radio button 
         * radMQ gekies is word die proses wat geadd is in 'n array gesorteer en daar word bepaal 
         of die nuwe proses kleiner is as die huidige proses is. Indien dit waar, word daar gekyk 
         hoeveel tyd die proses oor het om uit te voer en dan weer vergelyk met die nuwe proses as die
         huidige proses minder tyd nodig het om klaar te maak word die proses klaar gemaak anders word die 
         nuwe proses uitgevoer en daarna weer die huidige proses*/
        private void btnAdd_Click(object sender, EventArgs e)//Add a process
        {
            if (Pcount < 3)
            {
                Pcount++;//Proses counter
                if (Pcount == 1)
                {
                    burstTime[burstCount] = Convert.ToInt16(txtTimeProcess.Text);//Burst Time = Process Time/Length of Process
                    prgOne.Maximum = burstTime[burstCount];
                    arrivalTime[burstCount] = Convert.ToInt16(txtStartTime.Text);
                    process[burstCount] = "P" + Pcount;
                    burstCount++;//BurstTime counter
                }
                else
                {
                    //Labels word waardes gegee
                    burstTime[burstCount] = Convert.ToInt16(txtTimeProcess.Text);
                    if (burstCount == 1)
                        prgTwo.Maximum = burstTime[burstCount];
                    else
                        prgThree.Maximum = burstTime[burstCount];
                    arrivalTime[burstCount] = Convert.ToInt16(txtStartTime.Text);
                    process[burstCount] = "P" + Pcount;
                    burstCount++;
                }
                if (Pcount == 1)
                {
                    //Labels word waardes gegee
                    lblBurst[0].Text = Convert.ToString(burstTime[0]);
                    lblArrival[0].Text = Convert.ToString(arrivalTime[0]);
                }
                 else if (Pcount > 1 && Pcount < 4)
                {
                    //Labels word waardes gegee
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
                //Kyk of die gebruiker Multiple Queues gekies het
                if (radMQ.Checked == true)
                {
                    btnStart.Visible = false;
                    MultipleQaddVanProses();
                    MultipleQ();
                }
            }
            else
                MessageBox.Show("Jy het reeds die proses limiet beriek");
        }
        private void MultipleQaddVanProses()
        {
            //Kyk of die proses wat geadd is Proses 1 is
            if (iStartTime1 == -1) //-1 is sodat as die gebruiker kies dat die tyd van die proses by 0 begin die Proses nie twee
                                   // keer sal begin nie.
            {
                iStartTime1 = Convert.ToInt16(txtStartTime.Text);//Eerste Proses se start time word hier bygesit
                iLengthOfProcess1 = Convert.ToInt32(txtStartTime.Text) + Convert.ToInt32(txtTimeProcess.Text);
                prgOne.Maximum = iLengthOfProcess1;
            }
            //Kyk of daar al voorheen 'n Tweede proses was
            else if (iStartTime2 == -1)
            {
                iStartTime2 = Convert.ToInt16(txtStartTime.Text);//Tweede Proses se start time word gestoor
                iLengthOfProcess2 = Convert.ToInt32(txtStartTime.Text) + Convert.ToInt32(txtTimeProcess.Text);
                prgTwo.Maximum = iLengthOfProcess2;
            }
            //Kyk of daar al voorheen 'n derde proses was
            else if (iStartTime3 == -1)
            {
                iStartTime3 = Convert.ToInt16(txtStartTime.Text);//Derde Proses se start time word gestoor
                iLengthOfProcess3 = Convert.ToInt32(txtStartTime.Text) + Convert.ToInt32(txtTimeProcess.Text);
                prgThree.Maximum = iLengthOfProcess3;

            }
            if (iStartTime2 != -1)
            {
                /*Die volgende vind net in die laaste twee prosesse plaas om te kyk of daar die vorige prosesse se tyd wat oor is kleiner is as die 
                     vorige prosesse. As daar die vorige prosesse reeds klaar is moet die proses net dadelik hardloop. 
                     Dis belangrik dat die algoritme nie in die eerste if ook plaas vind nie, want dit sal onnodige tyd mors 
                     wat die CPU aan ander prosesse kon aandag gee*/

                if (iStartTime1 - counter < iStartTime2)
                {
                    VertoonButtons(1);
                }
                else if (iLengthOfProcess1 <= iLengthOfProcess2)
                {
                    VertoonButtons(1);
                    VertoonButtons(2);
                }
                else if (iLengthOfProcess2 < iLengthOfProcess1)
                {
                    VertoonButtons(2);
                    VertoonButtons(1);
                    
                }
            }
            if (iStartTime3 != -1)
            {
                /*Die volgende vind laaste plaas om te kyk of daar die vorige prosesse se tyd wat oor is kleiner is as die 
                 vorige prosesse. As daar die vorige prosesse reeds klaar is moet die proses net dadelik hardloop. 
                 Dis belangrik dat die algoritme nie in die eerste if ook plaas vind nie, want dit sal onnodige tyd mors 
                 wat die CPU aan ander prosesse kon aandag gee*/

                if (iStartTime1 - counter < iStartTime3)
                {
                    VertoonButtons(3);
                }
                else if (iStartTime2 - counter <= iStartTime3)
                {
                    VertoonButtons(2);
                }
                else if (iLengthOfProcess1 < iLengthOfProcess3)
                {
                    VertoonButtons(1);
                }
                else if (iLengthOfProcess3 <= iLengthOfProcess1)
                {
                    VertoonButtons(1);
                    VertoonButtons(3);
                }
                else if (iLengthOfProcess3 < iLengthOfProcess2)
                {
                    VertoonButtons(3);
                    VertoonButtons(2);
                }
            }
        }
        //Die begin van 'n proses word gehardloop
        private void StartProses()
        {
            //Sortering
            for (int i = 0; i < Pcount; i++)
                for (int j = i + 1; j < Pcount; j++)
                {
                    if (arrivalTime[j] < arrivalTime[i])
                    {
                        //Sortering van arrivalTime
                        arr = arrivalTime[j];
                        arrivalTime[j] = arrivalTime[i];
                        arrivalTime[i] = arr;

                        //Sortering van BurstTime
                        arr = burstTime[j];
                        burstTime[j] = burstTime[i];
                        burstTime[i] = arr;

                        //Sortering van Prosesse
                        temp = process[j];
                        process[j] = process[i];
                        process[i] = temp;
                    }
                }
            ProcessStartTime.Start();//Timer word gestart
        }
        private void btnStart_Click(object sender, EventArgs e)//Start the processes
        {
            StartProses();
            VertoonButtons();
        }
        //Timer se intervalle word bepaal
        private void InitializeTimer()
        {
            counter = 0; 
            t.Interval = 100; //Tydstip is een sekonde
            t.Enabled = true;
            StartTimeTick(null, null); 
            t.Tick += new EventHandler(StartTimeTick); //Eventhandler vir Timer word geskep
        }
        private void btnStop_Click(object sender, EventArgs e)//Stop the processes
        {
            timeStop = lblTime.Text; //Die waarde van die Timer word in variable gebêre
            ProcessStartTime.Stop();//Timer word gestop            
        }
        int Val1 = 0;
        int Val2 = 0;
        int Val3 = 0;
        private void StartTimeTick(object sender, EventArgs e)//Timer wat hardloop
        {
            counter++;
            //Timer se waarde word op label geplaas
            if (counter < 10)
            {
                lblTime.Text = "0"+minute+":0" + counter.ToString(); //As Timer minder as 10 sekondes is word daar 'n 0 vooraan die Timer
                                                                     //se waarde geplaas sodat die sekondes 01, 02, 03 ens kan wees
            }
            else if(counter<10 || counter<60)
            {
                lblTime.Text = "0"+minute+":" + counter.ToString();//Sodra timer 10 en meer bereik maar minder is as 60 sekondes word die 
                                                                 //  Timer se waarde netso oorgedra soos 10, 11, 12 ens
            }
            else
            {
                minute++;                                           //Minute word meer sodra sekondes 60 bereik
                counter = 0;
                lblTime.Text = "0"+minute+":0" + counter.ToString();//Minute en sekondes word vertoon op label
            }

            VertoonButtons();

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
        
        //Vertoon Buttons soos wat proses hardloop
        /*Vertoon die buttons van die prosesse soos die prosesse begin om te hardloop. 
         D.m.v. die metode kan die gebruiker sien hoe ver 'n proses op die oomblik is asook
         hoelank die proses nog oor het om te hardloop. 
         Die gebruik van Buttons wat net vertoon word is beter, want dis meer gebruikersvriendelik*/
        private void VertoonButtons()
        {
            for (int i = 0;i<3;i++)
            {
                //Buttons word vertoon soos prosesse hardloop
                for (int a = 0; a < Pcount; a++)
                {
                    if (arrivalTime[a] <= counter)
                    {
                        burstTime[a] = burstTime[a] - 1;
                        arrivalTime[a] = arrivalTime[a] + 1;
                        if (process[a] == "P1" && Val1 < prgOne.Maximum)
                        {
                            addnProcess(1);
                            ProgressOFProcess(arrPrg[0]);
                            Val1++;
                            lblProcessRunning.Text = "1";
                            if (prgTwo.Value < prgTwo.Maximum && prgThree.Value < prgThree.Maximum)
                                lblProcessQueued.Text = "2 & 3";
                            else if (prgTwo.Value < prgTwo.Maximum && prgThree.Value == prgThree.Maximum)
                                lblProcessQueued.Text = "2";
                            else if (prgTwo.Value == prgTwo.Maximum && prgThree.Value < prgThree.Maximum)
                                lblProcessQueued.Text = "3";
                            if ( prgThree.Value == prgThree.Maximum && prgTwo.Value == prgTwo.Maximum)
                                lblProcessQueued.Text = "0";
                        }
                        else if (process[a] == "P2" && Val2 < prgTwo.Maximum)
                        {
                            addnProcess(2);
                            ProgressOFProcess(arrPrg[1]);
                            Val2++;
                            lblProcessRunning.Text = "2";
                            
                            if (prgOne.Value < prgOne.Maximum && prgThree.Value < prgThree.Maximum)
                                lblProcessQueued.Text = "1 & 3";
                            else if (prgOne.Value < prgOne.Maximum && prgThree.Value == prgThree.Maximum)
                                lblProcessQueued.Text = "1";
                            else if (prgTwo.Value == prgTwo.Maximum && prgThree.Value < prgThree.Maximum)
                                lblProcessQueued.Text = "3";
                            if (prgOne.Value == prgOne.Maximum && prgThree.Value == prgThree.Maximum)
                                lblProcessQueued.Text = "0";
                        }
                        else if (process[a] == "P3" && Val3 < prgThree.Maximum)
                        {
                            addnProcess(3);
                            ProgressOFProcess(arrPrg[2]);
                            Val3++;
                            lblProcessRunning.Text = "3";
                            
                            if (prgOne.Value < prgOne.Maximum && prgTwo.Value < prgTwo.Maximum)
                                lblProcessQueued.Text = "1 & 2";
                            else if (prgTwo.Value < prgTwo.Maximum && prgOne.Value == prgOne.Maximum)
                                lblProcessQueued.Text = "2";
                            else if (prgTwo.Value == prgTwo.Maximum && prgOne.Value == prgOne.Maximum)
                                lblProcessQueued.Text = "1";
                            if (prgOne.Value == prgOne.Maximum && prgTwo.Value == prgTwo.Maximum)
                                lblProcessQueued.Text = "0";
                        }
                    }
                }
            }
        }
        
        private void VertoonButtons(int Value)
        {
            int Val = 0;//Variable word gebruik as value counter 

            if (Value == 1)
                Val = Val1;
            else if (Value == 2)
                Val = Val2;
            else if (Value == 3)
                Val = Val3;
            for (int i = 0; i < 3; i++)
            {
                if (ProsesNommer[i] == 1)
                {
                    //Buttons word vertoon soos prosesse hardloop
                    for (int a = 0; a < Pcount; a++)
                    {
                        if (counter % 2 == 0)
                        {
                            if (arrivalTime[a] <= counter)
                            {
                                burstTime[a] = burstTime[a] - 1;
                                arrivalTime[a] = arrivalTime[a] + 1;
                                                        // Value.toString() convert string to int
                                if (process[a] == "P" + Value.ToString() && Val < 10)
                                {
                                    addnProcess(i);
                                    ProgressOFProcess(arrPrg[i]);
                                    lblProcessRunning.Text = i.ToString();
                                    Val++;
                                    MultipleQaddVanProses();
                                }
                            }
                        }
                    }
                }
            }
        }
        //Progressbars se values word met 1 vermeerder
        private void ProgressOFProcess(ProgressBar naamvanProgressbar)
        {
            try
            {
                if (naamvanProgressbar.Value < naamvanProgressbar.Maximum)
                {
                    if (naamvanProgressbar.Maximum - naamvanProgressbar.Value == 1)
                        naamvanProgressbar.Value ++;
                    else if (naamvanProgressbar.Value == 0)
                        naamvanProgressbar.Value ++;
                    else
                        naamvanProgressbar.Value += naamvanProgressbar.Value;
                }
            }
            catch
            {
                naamvanProgressbar.Value = naamvanProgressbar.Maximum;
                
            }
        }
        private void button29_Click(object sender, EventArgs e)
        {    }
        int iTotaal1 = 0;
        int iTotaal2 = 0;
        int iTotaal3 = 0;
        bool bKlaar = false;
        private void addnProcess(int processno)
        {
            iTotaal1 = 0;
            iTotaal2 = 0;
            iTotaal3 = 0;
            bKlaar = false;
            if (processno == 1 && (arrPrg[1].Value == 0 && arrPrg[2].Value == 0))
                ProgressOFProcess(prgOne);
            else if (processno == 1 && (arrPrg[1].Value != 0 || arrPrg[2].Value != 0) && arrPrg[0].Value < arrPrg[0].Maximum)
            {
                while ((arrPrg[0].Value != arrPrg[1].Value || arrPrg[0].Value != arrPrg[2].Value) && bKlaar == false)
                {
                    if (arrPrg[1].Value != 0)
                    {
                        try
                        {
                            iTotaal1 = arrPrg[1].Value - arrPrg[0].Value;
                            arrPrg[0].Value += iTotaal1;
                            if (iTotaal1 == 0)
                                bKlaar = true;
                        }
                        catch
                        {
                            iTotaal1 = 0;
                            if (arrPrg[1].Value >= arrPrg[0].Maximum)
                                arrPrg[0].Value = arrPrg[0].Maximum;
                            else
                                arrPrg[0].Value = arrPrg[1].Value;
                            bKlaar = true;
                        }                      
                    }
                    else if (arrPrg[2].Value != 0)
                    {
                        try
                        {
                            iTotaal1 = arrPrg[2].Value - arrPrg[0].Value;
                            arrPrg[0].Value += iTotaal1;
                            if (iTotaal1 == 0)
                                bKlaar = true;
                        }
                        catch
                        {
                            iTotaal1 = 0;
                            if (arrPrg[2].Value >= arrPrg[0].Maximum)
                                arrPrg[0].Value = arrPrg[0].Maximum;
                            else
                                arrPrg[0].Value = arrPrg[2].Value;
                            bKlaar = true;
                        }
                    }
                }
            }
            else if (processno == 2 && (arrPrg[0].Value == 0 && arrPrg[2].Value == 0))
                ProgressOFProcess(prgTwo);
            else if (processno == 2 && (arrPrg[0].Value != 0 || arrPrg[2].Value != 0) && arrPrg[1].Value < arrPrg[1].Maximum)
            {
                while ((arrPrg[1].Value != arrPrg[0].Value || arrPrg[1].Value != arrPrg[2].Value) && bKlaar == false)
                {
                    if (arrPrg[0].Value != 0)
                    {
                        try
                        {
                            iTotaal2 = (arrPrg[0].Value - arrPrg[1].Value);
                            arrPrg[1].Value += iTotaal2;
                            if (iTotaal2 == 0)
                                bKlaar = true;
                        }
                        catch
                        {
                            iTotaal2 = 0;
                            if (arrPrg[0].Value >= arrPrg[1].Maximum)
                                arrPrg[1].Value = arrPrg[1].Maximum;
                            else
                                arrPrg[1].Value = arrPrg[0].Value;
                            bKlaar = true;
                        }
                    }
                    else if (arrPrg[2].Value != 0)
                    {
                        try
                        {
                            iTotaal2 = arrPrg[2].Value - arrPrg[1].Value;
                            arrPrg[1].Value += iTotaal2;
                            if (iTotaal2 == 0)
                                bKlaar = true;
                        }
                        catch
                        {
                            iTotaal2 = 0;
                            if (arrPrg[2].Value >= arrPrg[1].Maximum)
                                arrPrg[1].Value = arrPrg[1].Maximum;
                            else
                                arrPrg[1].Value = arrPrg[2].Value;
                            bKlaar = true;
                        }
                    }
                }
            }
            else if (processno == 3 && (arrPrg[0].Value == 0 && arrPrg[1].Value == 0))
                ProgressOFProcess(prgThree);
            else if (processno == 3 && (arrPrg[0].Value != 0 || arrPrg[1].Value != 0) && arrPrg[2].Value < arrPrg[2].Maximum)
            {
                while ((arrPrg[2].Value != arrPrg[0].Value || arrPrg[2].Value != arrPrg[1].Value) && bKlaar == false)
                {
                    if (arrPrg[0].Value != 0)
                    {
                        try
                        {
                            iTotaal3 = arrPrg[0].Value - arrPrg[2].Value;
                            arrPrg[2].Value += iTotaal3;
                            if (iTotaal3 == 0)
                                bKlaar = true;
                        }
                        catch
                        {
                            iTotaal3 = 0;
                            if (arrPrg[0].Value >= arrPrg[2].Maximum)
                                arrPrg[2].Value = arrPrg[2].Maximum;
                            else
                                arrPrg[1].Value = arrPrg[0].Value;
                            bKlaar = true;
                        }
                    }
                    else if (arrPrg[1].Value != 0)
                    {
                        try
                        {
                            iTotaal3 = arrPrg[1].Value - arrPrg[2].Value;
                            arrPrg[2].Value += iTotaal3;
                            if (iTotaal3 == 0)
                                bKlaar = true;
                        }
                        catch
                        {
                            iTotaal3 = 0;
                            if (arrPrg[1].Value >= arrPrg[2].Maximum)
                                arrPrg[2].Value = arrPrg[2].Maximum;
                            else
                                arrPrg[2].Value = arrPrg[1].Value;
                            bKlaar = true;
                        }
                    }
                }
            }
        }
        private void radMQ_CheckedChanged(object sender, EventArgs e)
        {
            prgOne.Value = 0;
            prgTwo.Value = 0;
            prgThree.Value = 0;
        }

        //Multiple queues
        int[] ProsesNommer = new int[3];
        int tempProses;
        private void MultipleQ()
        {
            ProsesNommer[0] = 1;
            ProsesNommer[1] = 2;
            ProsesNommer[2] = 3;

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

            StartProses();
        }
        private void rgbPriority_CheckedChanged(object sender, EventArgs e)
        {
            prgOne.Value = 0;
            prgTwo.Value = 0;
            prgThree.Value = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            arrPrg[0] = prgOne;
            arrPrg[1] = prgTwo;
            arrPrg[2] = prgThree;

            prgOne.Value = 0;
            prgTwo.Value = 0;
            prgThree.Value = 0;
        }
    }
}
