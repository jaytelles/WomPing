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

namespace WomPing
{
    public partial class WomPingForm : Form
    {
        private List<Target> targets;
        public WomPingForm()
        {
            InitializeComponent();
            targets = new List<Target>();
            readHostList();
            Target test = new Target("LOL Game Server", "54.201.56.143", 443);
            test.doPing();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
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
