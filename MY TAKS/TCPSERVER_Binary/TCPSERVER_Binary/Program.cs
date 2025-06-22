using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using StudentData;

namespace TCPSERVER_Binary
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            //define listening end point having IP and portNo
            IPEndPoint ep = new IPEndPoint(address, 5256);
            //ready the Listener on the EP
            TcpListener listener = new TcpListener(ep);
            listener.Start();
            Console.WriteLine($"Server is ready to listen on {ep}");
            while (true)
            {
                Socket socket = listener.AcceptSocket();//blocking function.

                //after connection is established.
                IPEndPoint remoteEP = socket.RemoteEndPoint as IPEndPoint;
                Console.WriteLine($"Server accepted the request from client {remoteEP}");

                //now server is ready to send and receive data.
                Stream stream = new NetworkStream(socket);

                int a = 1223;//4 bytes

                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(a);

                BinaryFormatter formatter = new BinaryFormatter();
                var studentInfo = formatter.Deserialize(stream) as Student;

                Console.WriteLine(studentInfo);
                Console.ReadKey();
            }
        }
    }
}
