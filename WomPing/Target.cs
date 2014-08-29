using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Threading;

namespace WomPing
{
    class Target
    {
        private double average;
        private double standardDev;
        private string hostname;
        private string ip;
        private int port;
        private List<long> pingTimes;
        private long mostRecentPing;
        private long lastPing;
        private bool isRunning;
        private Socket sock;
        private Stopwatch stop;

        public Target(string hostname, string ip, int port)
        {
            pingTimes = new List<long>();
            this.average = 0;
            this.hostname = hostname;
            this.ip = ip;
            this.port = port;
            mostRecentPing = 0;
            stop = new Stopwatch();
        }

        public void doICMPPing()
        {
            Ping ping = new Ping();
            PingOptions options = new PingOptions();
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            PingReply reply = ping.Send("8.8.8.8", timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine("Address: {0}", reply.Address.ToString());
                Console.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                Console.WriteLine("Time to live: {0}", reply.Options.Ttl);
                Console.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                Console.WriteLine("Buffer size: {0}", reply.Buffer.Length);
            }
            int a = 3;
        }

        public void doPing(Object stateinfo)
        {
            isRunning = true;
            try
            {
                SocketPermission permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, "", SocketPermission.AllPorts);
                IPAddress addr = Dns.GetHostEntry(ip).AddressList[0];
                IPEndPoint endpoint = new IPEndPoint(addr, port);
                sock = new Socket(addr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sock.ReceiveTimeout = 2000;
                stop.Reset();

                string message = "asdfadsfadsf";
                byte[] msg = Encoding.Unicode.GetBytes(message);
                byte[] recvbytes = new byte[1024];

                sock.Connect(endpoint);
                stop.Start();
                int bytesSent = sock.Send(msg);
                sock.Receive(recvbytes);
                stop.Stop();
            }
            catch (SocketException except)
            {
                stop.Stop();
                if (except.ErrorCode == 10038)
                {
                    lastPing = -1;
                }
            }
            catch(NullReferenceException)
            {
                stop.Stop();
            }
            catch (Exception)
            {
                stop.Stop();
            }
            closeSocket();
            lastPing = mostRecentPing;
            mostRecentPing = stop.ElapsedMilliseconds;
            pingTimes.Add(mostRecentPing);
            doMath();
            stop.Reset();
            isRunning = false;
        }

        public void closeSocket()
        {
            try
            {
                sock.Close();
            }
            catch (NullReferenceException) { }
        }

        private void doMath()
        {
            double sum = 0;
            int max = 100;
            if (pingTimes.Count <= 100)
            {
                max = pingTimes.Count;
            }
            List<long> subPings = pingTimes.GetRange(pingTimes.Count - max, max);

            for (int k = 0; k < subPings.Count; k++)
            {
                sum += subPings[k];
            }
            average = sum / (double)max;

            double acc = 0;
            for(int k=0; k<max; k++)
            {
                acc += Math.Pow((subPings[k] - average), 2);
            }

            standardDev = Math.Sqrt(acc / max);
        }

        public double getAverage()
        {
            return average;
        }

        public double getStandardDev()
        {
            return standardDev;
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
