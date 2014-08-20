using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;


namespace WomPing
{
    class Target
    {
        private double average;
        private string hostname;
        private string ip;
        private int port;
        private List<long> pingTimes;
        private long mostRecentPing;
        private long lastPing;
        private bool isRunning;

        public Target(string hostname, string ip, int port)
        {
            pingTimes = new List<long>();
            this.average = 0;
            this.hostname = hostname;
            this.ip = ip;
            this.port = port;
            mostRecentPing = 0;
        }

        public void doPing(Object stateinfo)
        {
            isRunning = true;
            try
            {
                Stopwatch stop = new Stopwatch();
                SocketPermission permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, "", SocketPermission.AllPorts);
                IPAddress addr = Dns.GetHostEntry("54.201.56.143").AddressList[0];
                IPEndPoint endpoint = new IPEndPoint(addr, port);
                Socket sock = new Socket(addr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sock.SendTimeout = 5000;
                sock.ReceiveTimeout = 5000;

                string message = "asdfadsfadsf";
                byte[] msg = Encoding.Unicode.GetBytes(message);
                byte[] recvbytes = new byte[1024];

                sock.Connect(endpoint);
                stop.Start();
                int bytesSent = sock.Send(msg);
                sock.Receive(recvbytes);
                stop.Stop();
                sock.Close();

                lastPing = mostRecentPing;
                mostRecentPing = stop.ElapsedMilliseconds;
                stop.Reset();
                pingTimes.Add(mostRecentPing);
                doAverages();
                
            }
            catch (Exception){}
            isRunning = false;
        }

        private void doAverages()
        {
            double sum = 0;
            for (int k = 0; k < pingTimes.Count; k++)
            {
                sum += pingTimes[k];
            }
            this.average = sum / pingTimes.Count;
        }

        public double getAverage()
        {
            return average;
        }

        public string getHostname()
        {
            return hostname;
        }

        public string getIP()
        {
            return ip;
        }

        public bool getIsRunning()
        {
            return isRunning;
        }

        public long getMostRecentPing()
        {
            return mostRecentPing;
        }

        public long getLastPing()
        {
            return lastPing;
        }

        public long getDelta()
        {
            return mostRecentPing - lastPing;
        }
    }
}
