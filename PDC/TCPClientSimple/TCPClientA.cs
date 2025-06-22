using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace TCPClientSimple
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"),5256);

            var localEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);

            TcpClient client = new TcpClient(localEP);
            client.Connect(remoteEP);//sending connection request to the server,

            //which is ready for listening on the mentioned port no.
            //this Connect function will hit the AcceptSocket function on remote.

            //var localEP = client.Client.LocalEndPoint as IPEndPoint;
           // Console.WriteLine($"Your local endpoint {localEP}");

            Stream stream = client.GetStream();//get the network stream object
            
            StreamReader reader = new StreamReader(stream);

            StreamWriter writer = new StreamWriter(stream);
            writer.AutoFlush = true;

            var msgToReceive = reader.ReadLine();
            Console.WriteLine("Server mesage");
            Console.WriteLine(msgToReceive);

            var msgToSend = "Yes got the message";
            writer.WriteLine(msgToSend);


            Console.ReadKey();


        }
    }
}
