using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

namespace TCPServerH
{
    class MainClass
    {
        public static void Main(string[] args)
        {

            var studentsData = new Dictionary<String, Single>();
            studentsData.Add("2020-Arid-1234", 3.4F);
            studentsData.Add("2020-Arid-1235", 2.4F);
            studentsData.Add("2020-Arid-1236", 3.2F);
            studentsData.Add("2020-Arid-1237", 3.9F);



            IPAddress address = IPAddress.Any;
            //define listening end point having IP and portNo
            IPEndPoint ep = new IPEndPoint(address, 5257);
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

                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);

                var aridNo = "";
                do
                {
                    aridNo = reader.ReadString();
                    if (aridNo == "")
                    {
                        Console.WriteLine("Client ended session");
                        break;
                    }
                    Console.WriteLine($"Request for {aridNo}");
                    if (studentsData.ContainsKey(aridNo))
                        writer.Write(studentsData[aridNo]);
                    else
                        writer.Write(0.0F);


                } while (aridNo!="");


                Console.ReadKey();
            }
        }
    }
}
