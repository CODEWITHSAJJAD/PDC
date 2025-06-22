using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TCPClientC
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5256);
            TcpClient client = new TcpClient();
            client.Connect(ep);//sending connection request to the server,
            //which is ready for listening on the mentioned port no.
            //this Connect function will hit the AcceptSocket function on remote.

            var localep = client.Client.LocalEndPoint as IPEndPoint;
            Console.WriteLine($"Your local endpoint {localep}");

            Stream stream = client.GetStream();//get the network stream object


            var message = "";
            do
            {
                Console.WriteLine("Enter Message");
                message = Console.ReadLine();
                var buffer = System.Text.Encoding.UTF8.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);

                var response = new byte[1024];
               int bytesCount = stream.Read(response, 0, response.Length);

                var receivedMessage = System.Text.Encoding.UTF8.GetString(response, 0, bytesCount);
                Console.WriteLine(receivedMessage);
            } while (message!="");

        }
    }
}
