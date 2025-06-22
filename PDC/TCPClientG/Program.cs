using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace TCPClientG
{
  
    class MainClass
    {
        public static void Main(string[] args)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5257);
            TcpClient client = new TcpClient();
            client.Connect(ep);//sending connection request to the server,
            //which is ready for listening on the mentioned port no.
            //this Connect function will hit the AcceptSocket function on remote side.

            var localep = client.Client.LocalEndPoint as IPEndPoint;
            Console.WriteLine($"Your local endpoint {localep}");

            Stream stream = client.GetStream();//get the network stream object


            BinaryReader reader = new BinaryReader(stream);
            var input = reader.R.ReadInt32();
            Console.WriteLine(input);

            
            Console.ReadKey();

        }
    }
}
