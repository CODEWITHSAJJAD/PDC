using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Client
{
    class Client
    {
        static void Main(string[] args)
        {
            var remotIp = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5259);
            TcpClient client = new TcpClient();
            client.Connect(remotIp);
            Console.WriteLine("Request accepted");
            var stream = client.GetStream();
            var sr = new StreamReader(stream);
            var sw = new StreamWriter(stream);
            sw.AutoFlush = true;
            var servermsg = sr.ReadLine();
            Console.WriteLine(servermsg);
            sw.WriteLine("i want to win");
            Console.ReadKey();


        }
    }
}
