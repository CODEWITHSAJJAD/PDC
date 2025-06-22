
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
namespace TCPBroadCastServerD
{
    class TcpServer
    {
        private TcpListener listener;
        private List<TcpClient> clients;
        private readonly object lockObject = new object();

        public TcpServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            clients = new List<TcpClient>();
        }

        public void Start()
        {
            listener.Start();
            Console.WriteLine($"Server started on port {((IPEndPoint)listener.LocalEndpoint).Port}");
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                lock (lockObject)
                {
                    clients.Add(client);
                }
                Console.WriteLine("Client connected...");
                Thread clientThread = new Thread(HandleClient);
                clientThread.Start(client);
            }
        }

        private void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received: " + message);
                    BroadcastMessage("Server received: " + message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                lock (lockObject)
                {
                    clients.Remove(client);
                }
                client.Close();
                Console.WriteLine("Client disconnected...");
            }
        }

        private void BroadcastMessage(string message)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            lock (lockObject)
            {
                foreach (var client in clients)
                {
                    try
                    {
                        NetworkStream stream = client.GetStream();
                        stream.Write(messageBytes, 0, messageBytes.Length);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error occured while sending to client: {ex.Message}");
                    }
                }
            }
        }

        public static void Main()
        {
            TcpServer server = new TcpServer(8888);
            server.Start();
        }
    }
}