using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using StudentData;

namespace TCPCLIENT_Binary
{
    //[Serializable]
    //public class Student
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public DateTime DOB { get; set; }

    //    public override string ToString()
    //    {
    //        return $"{Id},{Name}({DOB})";
    //    }
    //}

    class MainClass
    {
        public static void Main(string[] args)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5256);
            TcpClient client = new TcpClient();
            client.Connect(ep);//sending connection request to the server,
            //which is ready for listening on the mentioned port no.
            //this Connect function will hit the AcceptSocket function on remote side.

            var localep = client.Client.LocalEndPoint as IPEndPoint;
            Console.WriteLine($"Your local endpoint {localep}");

            Stream stream = client.GetStream();//get the network stream object


            BinaryReader reader = new BinaryReader(stream);
            var input = reader.ReadInt32();
            Console.WriteLine(input);

            BinaryFormatter formatter = new BinaryFormatter();

            var sInf = new Student { Id = 101, Name = "Abul Salam", DOB = Convert.ToDateTime("02-27-2020") };
            formatter.Serialize(stream, sInf);

            Console.WriteLine("Sent");
            Console.ReadKey();

        }
    }
}
