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
        public womPingForm()
        {
            InitializeComponent();

            targets = new List<Target>();
            readHostList();
            startPingThreads();
            doWomPing();




            //Target test = new Target("LOL Game Server", "54.201.56.143", 443);
           //test.doPing();
        }

        public void doWomPing()
        {
            //while (true)
            //{
                startPingThreads();
                for (int k = 0; k < targets.Count; k++)
                {
                    ListViewItem row = new ListViewItem(targets[k].getHostname());
                    row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getIP()));
                    row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getMostRecentPing().ToString()));
                    row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getDelta().ToString()));
                    row.SubItems.Add(new ListViewItem.ListViewSubItem(row, targets[k].getAverage().ToString()));
                    this.pingTable.Items.Add(row);
                }
                
                //Thread.Sleep(15000);
            //}
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
            if(System.IO.File.Exists("C:\\womping\\hosts.txt"))
            {
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
                hosts.Add("google DNS, 8.8.8.8, 80");
                hosts.Add("LOL Game Servers, 54.201.56.143, 443");
                hosts.Add("facebook.com, 69.63.176.13, 80");
                System.IO.File.WriteAllLines("C:\\womping\\hosts.txt", hosts);
                readHostList();
            }

        }
    }
}
