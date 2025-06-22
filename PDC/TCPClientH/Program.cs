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
            client.Connect(ep);

            var localep = client.Client.LocalEndPoint as IPEndPoint;
            Console.WriteLine($"Your local endpoint {localep}");

            Stream stream = client.GetStream();//get the network stream object

            Console.WriteLine("Connected to Server");

            BinaryReader reader = new BinaryReader(stream);
            BinaryWriter writer = new BinaryWriter(stream);
            var aridNo = "";
            do
            {
                Console.WriteLine("Enter Arid No. ");
                 aridNo = Console.ReadLine();

                writer.Write(aridNo);
                if (aridNo.Equals(""))
                {
                    Console.WriteLine("You ended session.");
                    break;
                }

                var cgpa = reader.ReadSingle();
                Console.WriteLine($"CGPA is: {cgpa}");

            } while (aridNo != "");
            Console.ReadKey();

        }
    }
}
