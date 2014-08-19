using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;


namespace WomPing
{
    class Target
    {
        private double average;
        private string hostname;
        private string ip;
        private int port;

        public Target(string hostname, string ip, int port)
        {
            this.average = 0;
            this.hostname = hostname;
            this.ip = ip;
            this.port = port;
        }

        public void doPing()
        {
            Stopwatch stop = new Stopwatch();
            SocketPermission permission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, "", SocketPermission.AllPorts);
            IPHostEntry host = Dns.GetHostEntry("54.201.56.143");
            IPAddress addr = host.AddressList[0];
            IPEndPoint endpoint = new IPEndPoint(addr, port);
           

            string message = "asdfadsfadsf";
            byte[] msg = Encoding.Unicode.GetBytes(message);

            for (int k = 0; k < 10; k++ )
            {
                //NEED TO SET A DEFAULT TIMEOUT!!!!
                Socket sock = new Socket(addr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sock.Connect(endpoint);
                byte[] bytes = new byte[1024];
                stop.Start();
                int bytesSent = sock.Send(msg);
                sock.Receive(bytes);
                stop.Stop();
                long something = stop.ElapsedMilliseconds;
                stop.Reset();
                Console.WriteLine("here now!");
                sock.Close();
            }
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

    }
}
