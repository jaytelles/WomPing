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

namespace WomPing
{
    public partial class womPingForm : Form
    {
        //http://msdn.microsoft.com/en-us/library/system.threading.threadpool%28v=vs.110%29.aspx
        private List<Target> targets;
        private bool paused;
        private readonly int pingInterval = 2000;
        public womPingForm()
        {
            InitializeComponent();
            targets = new List<Target>();
            paused = false;

            readHostList();
            startPingThreads();
            doWomPing();
        }

        public void doWomPing()
        {
            startPingThreads();
            try
            {
                BeginInvoke(new MethodInvoker(() => { this.pingTable.Items.Clear(); }));
            } catch(Exception)
            {
                this.pingTable.Items.Clear();
            }

            for (int k = 0; k < targets.Count; k++)
            {
                ListViewItem row = new ListViewItem(targets[k].getHostname());
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getIP()));
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getMostRecentPing().ToString()));
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getDelta().ToString()));
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getAverage().ToString()));
                try
                {
                    BeginInvoke(new MethodInvoker(() => { this.pingTable.Items.Add(row); this.pingTable.Refresh(); }));
                } catch(Exception)
                {
                    this.pingTable.Items.Add(row);
                    this.pingTable.Refresh();
                }   
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void startPingThreads()
        {
            Target threadTarget;
            for (int k = 0; k < targets.Count; k++)
            {
                threadTarget = targets[k];
                //ThreadPool.QueueUserWorkItem(threadTarget.ThreadPoolCallback);
                ThreadPool.QueueUserWorkItem(new WaitCallback(threadTarget.doPing));
            }
            Thread.Sleep(1000);

            bool allDone = false;
            while(!allDone)
            {
                allDone = true;
                for(int k=0; k<targets.Count; k++)
                {
                    if(targets[k].getIsRunning())
                    {
                        allDone = false;
                    }
                }
            }
        }

        private void readHostList()
        {
            if(System.IO.File.Exists("C:\\womping\\hosts.xml"))
            {
                StreamReader reader = File.OpenText("C:\\womping\\hosts.xml");
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
                hosts.Add("google DNS, 8.8.8.8, 80");
                hosts.Add("LOL Game Servers, 54.201.56.143, 443");
                hosts.Add("facebook.com, 69.63.176.13, 80");
                System.IO.File.WriteAllLines("C:\\womping\\hosts.txt", hosts);
                readHostList();
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
    }
}
