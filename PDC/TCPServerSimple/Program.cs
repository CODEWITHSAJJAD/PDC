using System;
using System.Net.Sockets;//TcpListener,Socket   for communication purpose
using System.IO;//for data streams. Stream,NetworkStream,StreaReader/Writer
using System.Net;//IPAddress, IPEndPoint


namespace TCPServerSimple
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            //define listening end point having IP and portNo
            IPEndPoint localEP = new IPEndPoint(address,5256);
            //ready the Listener on the EP
            TcpListener listener = new TcpListener(localEP);
            listener.Start();

            Console.WriteLine($"Server is ready to listen on {localEP}");

            Socket socket = listener.AcceptSocket();//blocking function.


            //after connection is established.
            IPEndPoint remoteEP = socket.RemoteEndPoint as IPEndPoint;
            Console.WriteLine($"Server accepted the request from client {remoteEP}");


            //now server is ready to send and receive data.
            Stream stream = new NetworkStream(socket);

            //for sending data.
            StreamWriter writer = new StreamWriter(stream);
            //Immidiate send, and don't buffer the data
            writer.AutoFlush = true;

            //for sending receving.
            StreamReader reader = new StreamReader(stream);


            //for sending data to client
            Console.WriteLine("Enter the message for client:");
            string messageToSend = Console.ReadLine();//"Welcome Client! You are connected.";
            writer.WriteLine(messageToSend);//sending data to the connected  client.
            //writer.Flush(); //when autoflush is false.


            //for receiving data from client
            string messageToReceive = reader.ReadLine();//blocking function.
            Console.WriteLine("Message From Client");
            Console.WriteLine(messageToReceive);



            //now close the above connection related objects to free
            //resources and memory
            socket.Close();
            listener.Stop();

            Console.ReadKey();


        }
    }
}
