using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
namespace TCP_Server
{
    class Server
    {
        static void Main()
        {
            // Server configuration
            IPAddress Address = IPAddress.Parse("127.0.0.1");
            IPEndPoint LocalEp = new IPEndPoint(Address, 5259);
            TcpListener listener = new TcpListener(LocalEp);
            Console.WriteLine("Server is ready to listener request");
            listener.Start();
            Socket socket = listener.AcceptSocket();
            NetworkStream stream = new NetworkStream(socket);
            StreamReader sr = new StreamReader(stream);
            StreamWriter sw = new StreamWriter(stream);
            sw.AutoFlush = true;
            sw.WriteLine("Accepted, What you want to learn!");
            string clientmsg = sr.ReadLine();
            Console.WriteLine(clientmsg);
            Console.ReadKey();
            listener.Stop();


        }
    }
}
