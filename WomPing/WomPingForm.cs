using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace WomPing
{
    public partial class womPingForm : Form
    {
        //http://msdn.microsoft.com/en-us/library/system.threading.threadpool%28v=vs.110%29.aspx
        private List<Target> targets;
        private bool paused;
        private bool threadsComplete;
        public static readonly int pingInterval = 1500;
        private int cycles;
        public womPingForm()
        {
            InitializeComponent();
            targets = new List<Target>();
            paused = false;
            threadsComplete = true;
            cycles = 0;
            readTargets();
            createBeginningDisplay();

            //Target test = new Target("LOL Game Server", "54.201.56.143", 443);
            //Target test = new Target("google DNS", "8.8.8.8", 443);
            //Target test = new Target("google DNS", "facebook.com", 80);
            //test.doPing(null);
            int a = 3;
        }

        public void doWomPing()
        {
            threadsComplete = false;
            startPingThreads();
            try
            {
                BeginInvoke(new MethodInvoker(() => { this.pingTable.Items.Clear(); }));
            } catch(Exception)
            {
                this.pingTable.Items.Clear();
            }

            for(int k=targets.Count-1; k>=0; k--)
            {
                if(targets[k].getLastPing()==-1)
                {
                    targets.RemoveAt(k);
                }
            }
            
            for (int k = 0; k < targets.Count; k++)
            {
                ListViewItem row = new ListViewItem(targets[k].getHostname());
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getIP()));
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getMostRecentPing().ToString()));
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getDelta().ToString()));
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getAverage().ToString()));
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getStandardDev().ToString()));
                try
                {
                    BeginInvoke(new MethodInvoker(() => { this.pingTable.Items.Add(row); this.pingTable.Refresh(); }));
                } 
                catch(Exception)
                {
                    this.pingTable.Items.Add(row);
                    this.pingTable.Refresh();
                }   
            }
            threadsComplete = true;
        }

        private void createBeginningDisplay()
        {
            for (int k = 0; k < targets.Count; k++)
            {
                ListViewItem row = new ListViewItem(targets[k].getHostname());
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getIP()));
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getMostRecentPing().ToString()));
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getDelta().ToString()));
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getAverage().ToString()));
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getStandardDev().ToString()));
                this.pingTable.Items.Add(row);
                /*try
                {
                    BeginInvoke(new MethodInvoker(() => { this.pingTable.Items.Add(row); this.pingTable.Refresh(); }));
                }
                catch (Exception)
                {
                    
                    this.pingTable.Refresh();
                }*/
            }
        }

        private void startPingThreads()
        {
            Target threadTarget;
            for (int k = 0; k < targets.Count; k++)
            {
                threadTarget = targets[k];
                ThreadPool.QueueUserWorkItem(new WaitCallback(threadTarget.doPing));
            }
            //Thread.Sleep(pingInterval/4);

            Stopwatch stop = new Stopwatch();
            stop.Start();
            bool allDone = false;
            while(!allDone)
            {
                allDone = true;
                for(int k=0; k<targets.Count; k++)
                {
                    if(targets[k].getIsRunning() && stop.ElapsedMilliseconds > 2*pingInterval)
                    {
                        targets[k].endAttempt();
                    }
                    if(targets[k].getIsRunning())
                    {
                        allDone = false;
                    }
                }
            }
            cycles++;
            if(cycles == 100)
            {
                cycles = 0;
                readTargets();
            }
        }

        private void readTargets()
        {
            if(System.IO.File.Exists("C:\\womping\\hosts.txt"))
            {
                targets.Clear();
                StreamReader reader = File.OpenText("C:\\womping\\hosts.txt");
                string line;
                
                Target target;
                while((line = reader.ReadLine()) != null)
                {
                    string[] items = line.Split(',');
                    string tempname = items[0];
                    target = new Target(items[0], items[1], int.Parse(items[2]));
                    targets.Add(target);
                }
            }
            else
            {
                Directory.CreateDirectory("C:\\womping");
                List<String> hosts = new List<String>();
                hosts.Add("google DNS,8.8.8.8,80");
                hosts.Add("LOL Game Servers,54.201.56.143,443");
                hosts.Add("facebook.com,69.63.176.13,80");
                System.IO.File.WriteAllLines("C:\\womping\\hosts.txt", hosts);
                readTargets();
            }
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            Thread goThread = new Thread(new ThreadStart(startRecording));
            goThread.Start();
        }

        public void startRecording()
        {
            this.paused = false;
            while (!this.paused)
            {
                doWomPing();
                Thread.Sleep(pingInterval);
            }
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            this.paused = true;
        }

        private void womPingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.paused = true;
            while (!threadsComplete) { Thread.Sleep(5); }
            Thread.Sleep(pingInterval + 500);
        }
    }
}
